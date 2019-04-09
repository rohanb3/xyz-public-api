using Microsoft.WindowsAzure.Storage.Blob;
using System;

namespace Xyzies.TWC.Public.Data.Providers
{
    public class CloudBlobWrapper : ICloudBlobWrapper, IDisposable
    {
        public CloudBlobClient BlobClient { get; set; }

        public CloudBlobWrapper(CloudBlobClient blobClient)
        {
            BlobClient = blobClient ?? throw new ArgumentNullException(nameof(blobClient));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            BlobClient = null;
        }
    }
}
