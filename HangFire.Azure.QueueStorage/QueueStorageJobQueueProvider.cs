// Copyright (c) 2014 Sergey Odinokov
// See the file license.txt for copying permission.

using Hangfire.SqlServer;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Data;

namespace HangFire.Azure.QueueStorage
{
    internal class QueueStorageJobQueueProvider : IPersistentJobQueueProvider
    {
        private readonly CloudQueueClient _client;
        private readonly QueueStorageOptions _options;
        private readonly string[] _queues;

        public QueueStorageJobQueueProvider(
            CloudQueueClient client,
            QueueStorageOptions options,
            string[] queues)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (options == null) throw new ArgumentNullException("options");
            if (queues == null) throw new ArgumentNullException("queues");

            _client = client;
            _options = options;
            _queues = queues;

            CreateQueuesIfNotExists();
        }

        public IPersistentJobQueue GetJobQueue()
        {
            return new QueueStorageJobQueue(_client, _options);
        }

        public IPersistentJobQueue GetJobQueue(IDbConnection connection)
        {
            return new QueueStorageJobQueue(_client, _options);
        }

        public IPersistentJobQueueMonitoringApi GetJobQueueMonitoringApi()
        {
            return new QueueStorageMonitoringApi(_client, _queues);
        }

        public IPersistentJobQueueMonitoringApi GetJobQueueMonitoringApi(IDbConnection connection)
        {
            return new QueueStorageMonitoringApi(_client, _queues);
        }

        private void CreateQueuesIfNotExists()
        {
            foreach (var queue in _queues)
            {
                var cloudQueue = _client.GetQueueReference(queue);
                cloudQueue.CreateIfNotExists();
            }
        }
    }
}
