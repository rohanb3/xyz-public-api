using Microsoft.WindowsAzure.Storage.Blob;
using System;

namespace Xyzies.TWC.Public.Data.Providers
{
    public interface ICloudBlobWrapper : IDisposable
    {
        /// <summary>
        /// Access to blob client
        /// </summary>
        CloudBlobClient BlobClient { get; set; }
    }
}
