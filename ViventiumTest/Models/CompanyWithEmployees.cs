using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViventiumTest.Models
{
    public class CompanyWithEmployeesWrapper
    {
        public CompanyWithEmployees Company { get; set; } = null!;
    }

    public class CompanyWithEmployees : Company
    {
        public CompanyWithEmployees(Company company) : base(company)
        {
        }
        [JsonPropertyName("Employees")]
        [JsonPropertyOrder(3)]
        public IEnumerable<EmployeeWrapper>? Emp => this.Employees.Select(e => new EmployeeWrapper { EmployeeHeader = e });

    }
}
