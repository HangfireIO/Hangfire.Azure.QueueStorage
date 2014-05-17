HangFire.Azure.QueueStorage
============================

<a href="https://ci.appveyor.com/project/odinserj/hangfire-azure-queuestorage"><img title="Build status" width="113" src="https://ci.appveyor.com/api/projects/status/kog7v4hd4a5o5s8d?retina=true" /></a>

This library adds support for Azure Queue Storage to be used together
with HangFire.SqlServer job storage implementation. All job data is saved
to SQL Azure database, but job queues use Queue Storage.

**Note:** This is pre-alpha version, that has not been ever tested. Please, use it with care :)

Installation
-------------

This library is available as a NuGet Package, so type the following
command into your Package Manager Console window:

```
PM> Install-Package HangFire.Azure.QueueStorage
```

Usage
------

```csharp

CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
    CloudConfigurationManager.GetSetting("StorageConnectionString"));

CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

// Here are the default options
var options = new QueueStorageOptions
{
    VisibilityTimeout = TimeSpan.FromMinutes(30),
    QueuePollInterval = TimeSpan.FromSeconds(5)
};

JobStorage.Current = new SqlServerStorage("<connection string>")
    // Use the "default" queue with default options...
    .UseAzureQueues(queueClient) // "default" queue and default options
    // ... or specify either queues...
    .UseAzureQueues(queueClient, "critical", "default")
    // ... or queues with options
    .UseAzureQueues(queueClient, options, "critical", "default");
```

License
--------

HangFire.Azure.QueueStorage is released under the [MIT License](http://www.opensource.org/licenses/MIT).
