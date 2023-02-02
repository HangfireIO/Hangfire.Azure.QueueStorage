// Copyright (c) 2014 Sergey Odinokov
// See the file license.txt for copying permission.

using Hangfire.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;

namespace HangFire.Azure.QueueStorage
{
    internal class QueueStorageFetchedJob : IFetchedJob
    {
        private readonly CloudQueue _queue;
        private readonly CloudQueueMessage _message;

        public QueueStorageFetchedJob(CloudQueue queue, CloudQueueMessage message)
        {
            if (queue == null) throw new ArgumentNullException("queue");
            if (message == null) throw new ArgumentNullException("message");

            _queue = queue;
            _message = message;

            JobId = _message.AsString;
        }

        public string JobId { get; private set; }

        public void RemoveFromQueue()
        {
            _queue.DeleteMessage(_message);
        }

        void IDisposable.Dispose()
        {
        }

        public void Requeue()
        {
            _queue.DeleteMessage(_message);
            _queue.AddMessage(_message);
        }
    }
}