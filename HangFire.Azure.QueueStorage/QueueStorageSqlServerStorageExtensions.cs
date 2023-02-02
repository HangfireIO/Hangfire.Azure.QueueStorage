// Copyright (c) 2014 Sergey Odinokov
// See the file license.txt for copying permission.

using Hangfire.SqlServer;
using Hangfire.States;
using Microsoft.WindowsAzure.Storage.Queue;
using System;

namespace HangFire.Azure.QueueStorage
{
    public static class QueueStorageSqlServerStorageExtensions
    {
        public static SqlServerStorage UseAzureQueues(
            this SqlServerStorage storage,
            CloudQueueClient client)
        {
            return UseAzureQueues(storage, client, new[] { EnqueuedState.DefaultQueue });
        }

        public static SqlServerStorage UseAzureQueues(
            this SqlServerStorage storage,
            CloudQueueClient client,
            params string[] queues)
        {
            return UseAzureQueues(storage, client, new QueueStorageOptions(), queues);
        }

        public static SqlServerStorage UseAzureQueues(
            this SqlServerStorage storage,
            CloudQueueClient client,
            QueueStorageOptions options,
            params string[] queues)
        {
            if (storage == null) throw new ArgumentNullException("storage");
            if (client == null) throw new ArgumentNullException("client");
            if (options == null) throw new ArgumentNullException("options");

            var provider = new QueueStorageJobQueueProvider(client, options, queues);
            storage.QueueProviders.Add(provider, queues);

            return storage;
        }
    }
}
