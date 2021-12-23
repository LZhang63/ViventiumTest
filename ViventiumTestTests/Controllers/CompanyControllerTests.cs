using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViventiumTest.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViventiumTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ViventiumTest.Controllers.Tests
{
    [TestClass()]
    public class CompanyControllerTests
    {
        //Test project cannot automatically resolve the path to the db file used by the target app. Hard code it here.
        readonly string _dbConnection = "C:\\prj\\ViventiumTest\\ViventiumTest\\ViventiumTest.db";

        //good payload with companies (1, 2, 3, 4, 5, 6, 7) and 10 employees total
        readonly string _testPayload = @"CompanyId,CompanyCode,CompanyDescription,EmployeeId,EmployeeFirstName,EmployeeLastName,EmployeeEmail,EmployeeDepartment,EmployeeHireDate
2,Delta,Delta Description,1,Marrilee,Suddaby,msuddaby0@opera.com,Engineering,2007-08-25
5,Whiskey,Whiskey Description,2,Jourdan,Lamlin,jlamlin1@uiuc.edu,Legal,2006-08-09
3,Zulu,Zulu Description,3,Christel,Proudlove,cproudlove2@reverbnation.com,Support,
6,Bravo,Bravo Description,4,Kaylee,Necolds,knecolds3@ucoz.com,Services,2014-11-11
5,Whiskey,Whiskey Description,5,Marty,Aslen,maslen4@cnet.com,Engineering,
1,Tango,Tango Description,6,Evyn,Bartaloni,ebartaloni5@ucoz.com,Legal,2016-05-07
4,Oscar,Oscar Description,7,Ansel,Chambers,achambers6@weather.com,Services,2008-11-02
1,Tango,Tango Description,8,Rhona,Essex,ressex7@accuweather.com,Sales,2016-05-14
7,Charlie,Charlie Description,9,Teresina,Ciardo,tciardo8@answers.com,Sales,2013-10-10
5,Whiskey,Whiskey Description,10,Stanislas,Oldman,soldman9@adobe.com,Human Resources,2018-04-09";

        //Bad test load case 1: conflict company information, company with id 1 has different name on different lines
        readonly string _badTestPayload1 = @"CompanyId,CompanyCode,CompanyDescription,EmployeeId,EmployeeFirstName,EmployeeLastName,EmployeeEmail,EmployeeDepartment,EmployeeHireDate
2,Delta,Delta Description,1,Marrilee,Suddaby,msuddaby0@opera.com,Engineering,2007-08-25
5,Whiskey,Whiskey Description,2,Jourdan,Lamlin,jlamlin1@uiuc.edu,Legal,2006-08-09
3,Zulu,Zulu Description,3,Christel,Proudlove,cproudlove2@reverbnation.com,Support,
6,Bravo,Bravo Description,4,Kaylee,Necolds,knecolds3@ucoz.com,Services,2014-11-11
5,Whiskey,Whiskey Description,5,Marty,Aslen,maslen4@cnet.com,Engineering,
1,Tango,Tango Description,6,Evyn,Bartaloni,ebartaloni5@ucoz.com,Legal,2016-05-07
4,Oscar,Oscar Description,7,Ansel,Chambers,achambers6@weather.com,Services,2008-11-02
1,TangoX,Tango Description,8,Rhona,Essex,ressex7@accuweather.com,Sales,2016-05-14
7,Charlie,Charlie Description,9,Teresina,Ciardo,tciardo8@answers.com,Sales,2013-10-10
5,Whiskey,Whiskey Description,10,Stanislas,Oldman,soldman9@adobe.com,Human Resources,2018-04-09";

        //Bad test load case 2: duplicate employee id 5
        readonly string _badTestPayload2 = @"CompanyId,CompanyCode,CompanyDescription,EmployeeId,EmployeeFirstName,EmployeeLastName,EmployeeEmail,EmployeeDepartment,EmployeeHireDate
2,Delta,Delta Description,1,Marrilee,Suddaby,msuddaby0@opera.com,Engineering,2007-08-25
5,Whiskey,Whiskey Description,2,Jourdan,Lamlin,jlamlin1@uiuc.edu,Legal,2006-08-09
3,Zulu,Zulu Description,3,Christel,Proudlove,cproudlove2@reverbnation.com,Support,
6,Bravo,Bravo Description,4,Kaylee,Necolds,knecolds3@ucoz.com,Services,2014-11-11
5,Whiskey,Whiskey Description,5,Marty,Aslen,maslen4@cnet.com,Engineering,
1,Tango,Tango Description,6,Evyn,Bartaloni,ebartaloni5@ucoz.com,Legal,2016-05-07
4,Oscar,Oscar Description,7,Ansel,Chambers,achambers6@weather.com,Services,2008-11-02
1,Tango,Tango Description,8,Rhona,Essex,ressex7@accuweather.com,Sales,2016-05-14
7,Charlie,Charlie Description,9,Teresina,Ciardo,tciardo8@answers.com,Sales,2013-10-10
5,Whiskey,Whiskey Description,5,Stanislas,Oldman,soldman9@adobe.com,Human Resources,2018-04-09";


        /// <summary>
        /// Because get tests depend on result of post. All test cases for the Company controller are combined into one test
        /// </summary>
        [TestMethod("Test CompanyController")]
        public void DataStoreTest()
        {
           
            DbContextOptionsBuilder<ViventiumTestDbContext> optionsBuilder = new();
            
            _ = optionsBuilder.UseSqlite($"Data Source={this._dbConnection}");

            using ViventiumTestDbContext db = new(optionsBuilder.Options);
            using CompanyController controller = new(db);
            
            //Fail case 1. Invalid data records - conflict company information
            IActionResult? response = controller.UploadCompanies(_badTestPayload1).Result;
            Assert.IsTrue(response != null
                          && response is BadRequestObjectResult result
                          && result.Value!.Equals("Invalid CSV File"),
                          "Uploading invalid CSV should get rejected with correct error message");

            //Fail case 2. Invalid data records - duplicate employee
            response = controller.UploadCompanies(_badTestPayload2).Result;
            Assert.IsTrue(response != null
                          && response is BadRequestObjectResult result2
                          && result2.Value!.Equals("Invalid CSV File"),
                          "Uploading invalid CSV should get rejected with correct error message");

            //Fail case 3. Invalid file format.
            response = controller.UploadCompanies("INVALID CSV UPLOAD").Result;
            Assert.IsTrue(response != null
                          && response is BadRequestObjectResult result3
                          && result3.Value!.Equals("Invalid CSV File"),
                          "Uploading invalid CSV should get rejected with correct error message");

            //Validate update
            response = controller.UploadCompanies(this._testPayload).Result;
            
            bool condition = response != null;
            condition = condition && response is OkResult;
            
            Assert.IsTrue(condition, "DataStore should be successful");

            List<CompanyWrapper> companies = controller.GetAllCompanies().Result.Value!.ToList();
            
            // company ids (1, 2, 3, 4, 5, 6, 7) should be there
            Assert.IsTrue(7 == companies.Count &&
                companies.Where(c => c.CompanyHeader.CompanyId == 1).Count() > 0 &&
                companies.Where(c => c.CompanyHeader.CompanyId == 2).Count() > 0 &&
                companies.Where(c => c.CompanyHeader.CompanyId == 3).Count() > 0 &&
                companies.Where(c => c.CompanyHeader.CompanyId == 4).Count() > 0 &&
                companies.Where(c => c.CompanyHeader.CompanyId == 5).Count() > 0 &&
                companies.Where(c => c.CompanyHeader.CompanyId == 6).Count() > 0 &&
                companies.Where(c => c.CompanyHeader.CompanyId == 7).Count() > 0 &&
                companies.Where(c => c.CompanyHeader.CompanyId == 8).Count() == 0,
                "Uploaded companies should match with retrieved companies"
                );

            //Get getting individual company. Case 1, getting an existing company
            CompanyWithEmployeesWrapper company = controller.GetCompany(1).Result.Value!;
            Assert.AreEqual((uint)1, company.Company.CompanyId, "Company 1 should exist being retrieved correctly");

            //Get getting individual company. Case 2, getting a company that does not exist
            ActionResult companyDoesNotExist = controller.GetCompany(10).Result.Result!;
            Assert.IsTrue(companyDoesNotExist is NotFoundResult, "Company 10 should not exist");
        }

    }
}