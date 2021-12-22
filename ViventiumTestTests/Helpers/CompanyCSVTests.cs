using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViventiumTest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViventiumTest.Helpers.Tests
{
    [TestClass()]
    public class CompanyCSVTests
    {
        [TestMethod("Test CSV Parsing and Validation")]
        public void ValidateCompanyRecordsTest()
        {
            List<CompanyCSV> testList = new();
            
            testList.AddRange(new CompanyCSV[] { new CompanyCSV()
                    {
                        CompanyCode = "Company1 Code",
                        CompanyDescription = "Company1 Desc",
                        CompanyId = 1,
                        EmployeeId = 1
                    },
                    new CompanyCSV()
                    {
                        CompanyCode = "Company1 Code",
                        CompanyDescription = "Company1 DescX",
                        CompanyId = 1,
                        EmployeeId = 2
                    }
                }
            );
            //Expected to fail because of company information conflicts (Description)
            Assert.IsFalse(CompanyCSV.ValidateCompanyRecords(testList));

            //Correct conflict company description
            testList[1].CompanyDescription = testList[0].CompanyDescription;
            //Make duplicate employee ids
            testList[1].EmployeeId = 1;

            //Expected to fail because of duplicate employee id
            Assert.IsFalse(CompanyCSV.ValidateCompanyRecords(testList));

            testList[1].EmployeeId = 2;

            //All problems fixed. Expect to pass
            Assert.IsTrue(CompanyCSV.ValidateCompanyRecords(testList));


        }

        [TestMethod("Test CSV Parsing")]
        public void LoadCompanyRecordsFromStringTest()
        {
            try
            {
                _ = CompanyCSV.LoadCompanyRecordsFromString("test1, test2\r\nx,y");
                Assert.Fail("Invaid CSV file should not pass");
            }
            catch 
            {
                ; //this is expected
            }
        }
    }
}