using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViventiumTest.Helpers;
using ViventiumTest.Models;

namespace ViventiumTest.Controllers
{
    
    [Route(EndPoints.CompanyController.Root)]
    public class CompanyController : Controller
    {
        private readonly ViventiumTestDbContext _dbContext;
        public CompanyController(ViventiumTestDbContext dbContext) => _dbContext = dbContext;

        [HttpGet]
        [Route(EndPoints.CompanyController.GetAllCompanies)]
        public async Task<ActionResult<IEnumerable<CompanyWrapper>>> GetAllCompanies()
        {
            List<CompanyWrapper> companyList = await _dbContext.Companies
                .Include(r=>r.Employees)
                .Select(r => new CompanyWrapper { CompanyHeader = r })
                .ToListAsync();
            
            //Sort company list
            companyList.Sort((x, y) => x.CompanyHeader.CompanyId.CompareTo(y.CompanyHeader.CompanyId));

            return companyList;
        }

        [HttpGet]
        [Route(EndPoints.CompanyController.GetCompany)]
        public async Task<ActionResult<CompanyWithEmployeesWrapper>> GetCompany(uint id)
        {
            Company? company = await this._dbContext.Companies.Include(c => c.Employees)
                .FirstOrDefaultAsync(c => c.CompanyId == id);

            if (company == null)
            {
                return this.NotFound();
            }
            //----Sorting is not required---------
            company.SortEmployees();
            return  new CompanyWithEmployeesWrapper() { Company = new CompanyWithEmployees(  company) };
            
        }


        [HttpPost]
        [Route(EndPoints.CompanyController.UploadCompanies)]
        public async Task<IActionResult> UploadCompanies([FromBody]string strContent)
        {   
            List<CompanyCSV> companyList;
            try
            {
                companyList = CompanyCSV.LoadCompanyRecordsFromString(strContent);
            }
            catch
            {
                return this.BadRequest("Invalid CSV File");
            }

            if ( !CompanyCSV.ValidateCompanyRecords(companyList))
            {
                return this.BadRequest("Invalid CSV File");
            }
        
            _ = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.ReloadAllCompaniesAsync(companyList);
                await _dbContext.Database.CommitTransactionAsync();
            }
            catch
            {
                await _dbContext.Database.RollbackTransactionAsync();
                throw;
            }
            
            return this.Ok();
        }
    }
}
