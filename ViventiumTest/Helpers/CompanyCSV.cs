using CsvHelper;
using CsvHelper.Configuration;

namespace ViventiumTest.Helpers
{
    public class CompanyCSV
    {
        /// <summary>
        /// Using CsvHelper for parsing Company CSV file
        /// </summary>
        /// <param name="csvContent"></param>
        /// <returns></returns>
        public static List<CompanyCSV> LoadCompanyRecordsFromString(string csvContent)
        {
            List<CompanyCSV> records = new();
            using StringReader reader = new(csvContent);

            CsvConfiguration cfg = new(System.Globalization.CultureInfo.CurrentCulture);
            cfg.Delimiter = ",";
            cfg.MissingFieldFound = null;
                
            using CsvReader csv = new(reader, cfg);
                
            while (csv.Read())
            {
                CompanyCSV Record = csv.GetRecord<CompanyCSV>();
                records.Add(Record);
            }
            return records;
        }

        /// <summary>
        /// Validate loaded company list for,
        /// 1. Company header has no conflicts between different records.
        /// 2. EmplayeeId's are unique
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public static bool ValidateCompanyRecords(List<CompanyCSV> records)
        {
            //1. Company headers should have no conflict
            if (records.Select(r => r.CompanyId).Distinct().Count() !=
                records.Select(r => (r.CompanyId, r.CompanyCode, r.CompanyDescription)).Distinct().Count())
            {
                return false;
            }
            //2. No two records should have the same employee ID
            if (records.Select(r => r.EmployeeId).Distinct().Count() != records.Count())
            {
                return false;
            }

            return true;
        }

        public CompanyCSV()
        {
            _company = new();
            _employee = new();
        }

        public uint CompanyId
        {
            get => _company.CompanyId;
            set => _company.CompanyId = value;
        }


        public string? CompanyCode
        {
            get => _company.CompanyCode;
            set => _company.CompanyCode = value;
        }
        public string? CompanyDescription
        {
            get => _company.CompanyDescription;
            set => _company.CompanyDescription = value;
        }

        public uint EmployeeId
        {
            get => _employee.EmployeeId;
            set => _employee.EmployeeId = value;
        }
        public string? EmployeeFirstName
        {
            get => _employee.EmployeeFirstName;
            set => _employee.EmployeeFirstName = value;
        }

        public string? EmployeeLastName
        {
            get => _employee.EmployeeLastName;
            set => _employee.EmployeeLastName = value;
        }
        public string? EmployeeEmail
        {
            get => _employee.EmployeeEmail;
            set => _employee.EmployeeEmail = value;
        }
        public string? EmployeeDepartment
        {
            get => _employee.EmployeeDepartment;
            set => _employee.EmployeeDepartment = value;
        }
        public string EmployeeHireDate
        {
            get => string.Format("{0:yyyy-MM-dd}", _employee.EmployeeHireDate);
            set => this._employee.EmployeeHireDate = string.IsNullOrWhiteSpace(value)
                    ? DateTime.MinValue
                    : DateTime.ParseExact(value, "yyyy-MM-dd", null);
        }

        public readonly Models.Company _company;
        public readonly Models.Employee _employee;
    }
}
