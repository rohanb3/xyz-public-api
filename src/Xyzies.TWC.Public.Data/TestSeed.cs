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
        private readonly CablePortalAppDataContext _dbContext = null;

        public TestSeed(ILogger<TestSeed> logger, CablePortalAppDataContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Seed()
        {
            var transaction = _dbContext.Database.BeginTransaction();
            Company companyIdWithoutAnyBranch = null;
            try
            {
                var requestStatusId = _dbContext.RequestStatuses.FirstOrDefault(x => x.Name.ToLower() == Consts.OnBoardedStatusName.ToLower())?.Id;
                if (requestStatusId == null)
                    requestStatusId = _dbContext.RequestStatuses.Add(GetRequestStatus()).Entity.Id;

                var companyId = _dbContext.Companies.FirstOrDefault(x => x.CompanyName.ToLower() == Consts.DefaultCompanyName.ToLower())?.Id;
                if (!companyId.HasValue)
                    companyId = _dbContext.Companies.Add(GetCompany(requestStatusId.Value, Consts.DefaultCompanyName)).Entity.Id;

                companyIdWithoutAnyBranch = _dbContext.Companies.Include(x=>x.Branches).FirstOrDefault(x => x.CompanyName == Consts.DefaultCompanyNameNotBindAnyBranch);
                if (companyIdWithoutAnyBranch == null)
                    companyIdWithoutAnyBranch = _dbContext.Companies.Add(GetCompany(requestStatusId.Value, Consts.DefaultCompanyNameNotBindAnyBranch)).Entity;

                if (!_dbContext.Branches.Any(x => x.BranchName.ToLower() == Consts.DefaultBranchName.ToLower()))
                    _dbContext.Branches.Add(GetBranch(companyId.Value));

                _dbContext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "[Seed] Database is not filling by default data");
                throw new ApplicationException("Database is not filling by default data");
            }

            var testBranchesForDeleteList = _dbContext.Branches.Where(x => x.CompanyId == companyIdWithoutAnyBranch.Id);
            if (testBranchesForDeleteList.Any())
                _dbContext.Branches.RemoveRange(testBranchesForDeleteList);
        }

        private Company GetCompany(Guid? requestStatusId, string companyName)
        => new Company
        {
            CompanyName = companyName,
            Email = $"{Guid.NewGuid()}test.com",
            CreatedDate = DateTime.UtcNow,
            CompanyStatusKey = requestStatusId
        };

        private Branch GetBranch(int companyId)
        => new Branch
        {
            BranchName = Consts.DefaultBranchName,
            Email = $"{Guid.NewGuid()}test.com",
            CompanyId = companyId,
            CreatedDate = DateTime.UtcNow,
            IsEnabled = true
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
