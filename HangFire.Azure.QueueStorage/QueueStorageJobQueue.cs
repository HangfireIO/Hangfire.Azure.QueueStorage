// Copyright (c) 2014 Sergey Odinokov
// See the file license.txt for copying permission.

using System;
using System.Linq;
using System.Threading;
using HangFire.SqlServer;
using HangFire.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace HangFire.Azure.QueueStorage
{
    internal class QueueStorageJobQueue : IPersistentJobQueue
    {
        private readonly CloudQueueClient _client;
        private readonly QueueStorageOptions _options;

        public QueueStorageJobQueue(CloudQueueClient client, QueueStorageOptions options)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (options == null) throw new ArgumentNullException("options");

            _client = client;
            _options = options;
        }

        public IFetchedJob Dequeue(string[] queues, CancellationToken cancellationToken)
        {
            if (queues == null) throw new ArgumentNullException("queues");
            if (queues.Length == 0) throw new ArgumentException("Queue array must be non-empty.", "queues");

            var cloudQueues = queues.Select(queue => _client.GetQueueReference(queue)).ToArray();
            CloudQueueMessage message;

            var currentQueueIndex = 0;

            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                message = cloudQueues[currentQueueIndex].GetMessage(_options.VisibilityTimeout);

                if (message == null)
                {
                    if (currentQueueIndex == cloudQueues.Length - 1)
                    {
                        cancellationToken.WaitHandle.WaitOne(_options.QueuePollInterval);
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    currentQueueIndex = (currentQueueIndex + 1) % queues.Length;
                }
            } while (message == null);

            return new QueueStorageFetchedJob(cloudQueues[currentQueueIndex], message);
        }

        public void Enqueue(string queue, string jobId)
        {
            var cloudQueue = _client.GetQueueReference(queue);
            var message = new CloudQueueMessage(jobId);

            cloudQueue.AddMessage(message);
        }
    }
}