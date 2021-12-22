using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ViventiumTest.Models
{
    /// <summary>
    /// Used for wrapping Employee in order to get desired root element name in JSON
    /// </summary>
    public class EmployeeWrapper
    {
        public Employee? EmployeeHeader {get; set;}
    }

    public class Employee
    {
        [Key]
        [JsonPropertyName("Id")]
        public uint EmployeeId { get; set; }
        [JsonPropertyName("FullName")]
        public string FullName => $"{this.EmployeeFirstName} {this.EmployeeLastName}";
        
        [JsonIgnore]
        public string? EmployeeFirstName { get; set; }
        [JsonIgnore]
        public string? EmployeeLastName { get; set; }
        [JsonIgnore] 
        public string? EmployeeEmail { get; set; }
        [JsonIgnore]
        public string? EmployeeDepartment { get; set; }
        [JsonIgnore] 
        public DateTime EmployeeHireDate { get; set; }
        [JsonIgnore]
        public Company? Company { get; set; }

    }
}
