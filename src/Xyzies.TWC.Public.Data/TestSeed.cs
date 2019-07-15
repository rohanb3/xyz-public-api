using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xyzies.TWC.Public.Data.Entities;

namespace Xyzies.TWC.Public.Data
{
    public class TestSeed : IDisposable
    {
        private readonly ILogger<TestSeed> _logger = null;
        private readonly AppDataContext _dbContext = null;

        public TestSeed(ILogger<TestSeed> logger, AppDataContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Seed()
        {
            var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var requestStatusId = _dbContext.RequestStatuses.FirstOrDefault(x => x.Name.ToLower() == Consts.OnBoardedStatusName.ToLower())?.Id;
                if (requestStatusId == null)
                    requestStatusId = _dbContext.RequestStatuses.Add(GetRequestStatus()).Entity.Id;

                var companyId = _dbContext.Companies.FirstOrDefault(x => x.CompanyName.ToLower() == Consts.DefaultCompanyName.ToLower())?.Id;
                if (companyId == null)
                    companyId = _dbContext.Companies.Add(GetCompany(requestStatusId.Value)).Entity.Id;

                if (!_dbContext.Branches.Any(x => x.BranchName.ToLower() == Consts.DefaultBranchName.ToLower()))
                    _dbContext.Branches.Add(GetBranch(companyId.Value));

                _dbContext.SaveChanges();
                transaction.Commit();
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "[Seed] Database is not filling by default data");
                throw new ApplicationException("Database is not filling by default data");
            }
        }

        private Company GetCompany(Guid? requestStatusId)
        => new Company
        {
            CompanyName = Consts.DefaultCompanyName,
            Email = $"{Guid.NewGuid()}test.com",
            CompanyStatusKey = requestStatusId
        };

        private Branch GetBranch(int companyId)
        => new Branch
        {
            BranchName = Consts.DefaultBranchName,
            Email = $"{Guid.NewGuid()}test.com",
            CompanyId = companyId
        };

        private RequestStatus GetRequestStatus()
        => new RequestStatus()
        {
            Name = Consts.OnBoardedStatusName,
            CreatedOn = DateTime.UtcNow
        };

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
