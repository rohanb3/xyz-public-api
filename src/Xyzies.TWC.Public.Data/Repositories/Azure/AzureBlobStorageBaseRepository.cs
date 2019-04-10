using Xyzies.TWC.Public.Data.Core;
using Xyzies.TWC.Public.Data.Providers;
using Xyzies.TWC.Public.Data.Providers.Behaviour;

namespace Xyzies.TWC.Public.Data.Repositories.Azure
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class AzureBaseBlobRepository<TEntity>
        : BaseRepository<string, TEntity, ICloudBlobWrapper>
        where TEntity : BaseEntity<string>, IEntity<string>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientWraper"></param>
        public AzureBaseBlobRepository(ICloudBlobWrapper clientWraper) : base(AccessPointProvider<ICloudBlobWrapper>.Create(clientWraper))
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessPointProvider"></param>
        public AzureBaseBlobRepository(IAccessPointProvider<ICloudBlobWrapper> accessPointProvider) : base(accessPointProvider)
        {

        }
    }
}
