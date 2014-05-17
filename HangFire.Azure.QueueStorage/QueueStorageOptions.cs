// Copyright (c) 2014 Sergey Odinokov
// See the file license.txt for copying permission.

using System;

namespace HangFire.Azure.QueueStorage
{
    public class QueueStorageOptions
    {
        public QueueStorageOptions()
        {
            VisibilityTimeout = TimeSpan.FromMinutes(30);
            QueuePollInterval = TimeSpan.FromSeconds(5);
        }

        public TimeSpan? VisibilityTimeout { get; set; }
        public TimeSpan QueuePollInterval { get; set; }
    }
}