// Copyright (c) 2014 Sergey Odinokov
// See the file license.txt for copying permission.

using Hangfire.SqlServer;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HangFire.Azure.QueueStorage
{
    internal class QueueStorageMonitoringApi : IPersistentJobQueueMonitoringApi
    {
        private readonly CloudQueueClient _client;
        private readonly string[] _queues;

        public QueueStorageMonitoringApi(CloudQueueClient client, string[] queues)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (queues == null) throw new ArgumentNullException("queues");

            _client = client;
            _queues = queues;
        }

        public IEnumerable<string> GetQueues()
        {
            return _queues;
        }

        public IEnumerable<int> GetEnqueuedJobIds(string queue, int @from, int perPage)
        {
            var cloudQueue = _client.GetQueueReference(queue);
            var messages = cloudQueue.PeekMessages(perPage);

            return messages.Select(x => int.Parse(x.AsString)).ToArray();
        }

        public IEnumerable<int> GetFetchedJobIds(string queue, int @from, int perPage)
        {
            return Enumerable.Empty<int>();
        }

        public EnqueuedAndFetchedCountDto GetEnqueuedAndFetchedCount(string queue)
        {
            var cloudQueue = _client.GetQueueReference(queue);
            cloudQueue.FetchAttributes();

            return new EnqueuedAndFetchedCountDto
            {
                EnqueuedCount = cloudQueue.ApproximateMessageCount,
                FetchedCount = null
            };
        }
    }
}
