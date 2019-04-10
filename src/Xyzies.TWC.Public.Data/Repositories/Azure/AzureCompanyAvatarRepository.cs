using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Entities.Azure;
using Xyzies.TWC.Public.Data.Providers;

namespace Xyzies.TWC.Public.Data.Repositories.Azure
{
    public class AzureCompanyAvatarRepository : AzureBaseBlobRepository<CompanyAvatar>, IAzureCompanyAvatarRepository
    {
        private CloudBlobClient _cloudBlobClient = null;
        private CloudBlobContainer _cloudBlobContainer = null;

        public AzureCompanyAvatarRepository(CloudStorageAccount storageAccount) : base(
            new CloudBlobWrapper(storageAccount.CreateCloudBlobClient()))
        {
        }

        public override string Add(CompanyAvatar entity)
        {
            throw new NotImplementedException();
        }

        public override async Task<string> AddAsync(CompanyAvatar entity)
        {
            try
            {
                if (string.IsNullOrEmpty(entity.Id))
                {
                    throw new ArgumentNullException("Company Id cannot be null or empty");
                }

                if (string.IsNullOrEmpty(entity.File.FileName))
                {
                    throw new ArgumentNullException("File name cannot be null or empty");
                }

                _cloudBlobContainer = base.AccessPoint.Provider.BlobClient.GetContainerReference(entity.Id);
                await _cloudBlobContainer.CreateIfNotExistsAsync();

                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };

                await _cloudBlobContainer.SetPermissionsAsync(permissions);

                var fileName = $"avatar.{entity.File.FileName.Split('.').LastOrDefault() ?? throw new ArgumentException("Invalid file name format", nameof(entity.File.FileName))}";

                CloudBlockBlob cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(fileName);
                await cloudBlockBlob.UploadFromStreamAsync(entity.File.OpenReadStream());

            }
            catch (StorageException)
            {
                if (_cloudBlobContainer != null)
                {
                    await _cloudBlobContainer.DeleteIfExistsAsync();
                }
            }

            return "Uploaded";
        }

        public override void AddRange(IEnumerable<CompanyAvatar> entities)
        {
            throw new NotImplementedException();
        }

        public override Task AddRangeAsync(IEnumerable<CompanyAvatar> entities)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<CompanyAvatar> Get()
        {
            throw new NotImplementedException();
        }

        public override CompanyAvatar Get(string id)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<CompanyAvatar> Get(Expression<Func<CompanyAvatar, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetAvatarPath(string companyId)
        {
            try
            {

                if (string.IsNullOrEmpty(companyId))
                {
                    throw new ArgumentNullException("Company Id cannot be null or empty");
                }

                _cloudBlobContainer = base.AccessPoint.Provider.BlobClient.GetContainerReference(companyId);

                var isExist = await _cloudBlobContainer.ExistsAsync();

                if (!isExist)
                {
                    return null;
                }

                BlobContinuationToken continuationToken = null;
                List<IListBlobItem> blobItems = new List<IListBlobItem>();

                do
                {
                    var response = await _cloudBlobContainer.ListBlobsSegmentedAsync(continuationToken);
                    continuationToken = response.ContinuationToken;
                    blobItems.AddRange(response.Results);
                } while (continuationToken != null);

                return blobItems.FirstOrDefault(item => item.Uri.AbsoluteUri.Contains("avatar")).Uri.AbsoluteUri;
            }
            catch (StorageException)
            {
                if (_cloudBlobContainer != null)
                {
                    await _cloudBlobContainer.DeleteIfExistsAsync();
                }

                throw;
            }
        }

        /// <inheritedoc />
        public override async Task<CompanyAvatar> GetAsync(string companyId)
        {
            throw new NotImplementedException();
        }

        public override Task<IQueryable<CompanyAvatar>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public override Task<IQueryable<CompanyAvatar>> GetAsync(Expression<Func<CompanyAvatar, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public override CompanyAvatar GetBy(Expression<Func<CompanyAvatar, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public override Task<CompanyAvatar> GetByAsync(Expression<Func<CompanyAvatar, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public override bool Has(string id)
        {
            throw new NotImplementedException();
        }

        public override bool Has(CompanyAvatar entity)
        {
            throw new NotImplementedException();
        }

        public override bool Has(Expression<Func<CompanyAvatar, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async override Task<bool> HasAsync(string id)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> HasAsync(CompanyAvatar entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> HasAsync(Expression<Func<CompanyAvatar, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(CompanyAvatar entity)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(string id)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAll()
        {
            throw new NotImplementedException();
        }

        public override Task RemoveAllAsync()
        {
            throw new NotImplementedException();
        }

        public override Task<bool> RemoveAsync(CompanyAvatar entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> RemoveAsync(string id)
        {
            throw new NotImplementedException();
        }

        public override bool Update(CompanyAvatar entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> UpdateAsync(CompanyAvatar entity)
        {
            throw new NotImplementedException();
        }

        public override Task UpdateRangeAsync(IEnumerable<CompanyAvatar> entities)
        {
            throw new NotImplementedException();
        }
    }
}
