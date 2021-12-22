using CsvHelper.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViventiumTest.Models
{
    /// <summary>
    /// Wrapper used for forcing desired JSON root element
    /// </summary>
    public class CompanyWrapper
    {
        public Company CompanyHeader { get; set; } = null!;
    }

    /// <summary>
    /// Company Model
    /// </summary>
    public class Company
    {
        [Key]
        [JsonPropertyName("Id")]
        public uint CompanyId { get; set; }

        [JsonPropertyName("Code")]
        public string? CompanyCode { get; set; }
        
        [JsonPropertyName("Description")]
        public string? CompanyDescription { get; set; }
        
        public int EmployeeCount => this.Employees.Count();
        
        /// <summary>
        /// Use this view to wrap Employee records in order to get desired root element name in JSON
        /// </summary>
        [NotMapped]
        [JsonPropertyName("Employees")]
        public IEnumerable<EmployeeWrapper>? Emp => this.Employees.Select(e => new EmployeeWrapper { EmployeeHeader = e });

        [JsonIgnore]
        public List<Employee> Employees { get; set; } = new List<Employee>();

        public void SortEmployees() => this.Employees.Sort((x, y) => x.EmployeeId.CompareTo(y.EmployeeId));
    }

    
}
