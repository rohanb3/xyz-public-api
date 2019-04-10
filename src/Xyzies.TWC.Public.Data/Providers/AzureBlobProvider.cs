using Xyzies.TWC.Public.Data.Providers.Behaviour;

namespace Xyzies.TWC.Public.Data.Providers
{
    public sealed class AzureBlobProvider : AccessPointProvider<ICloudBlobWrapper>, IAccessPointProvider<ICloudBlobWrapper>
    {
        public AzureBlobProvider(ICloudBlobWrapper blobWrapper) : base(blobWrapper) { }
    }
}
