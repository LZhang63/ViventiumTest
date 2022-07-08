﻿using CsvHelper.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ViventiumTest.Models
{
    //Test line 1
    //Test line 1
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
        public Company()
        {

        }
        public Company(Company company)
        {
            this.CompanyId = company.CompanyId;
            this.CompanyCode = company.CompanyCode;
            this.CompanyDescription = company.CompanyDescription;
            this.Employees = company.Employees;
        }

        [Key]
        [JsonPropertyName("Id")]
        public uint CompanyId { get; set; }

        [JsonPropertyName("Code")]
        public string? CompanyCode { get; set; }
        
        [JsonPropertyName("Description")]
        public string? CompanyDescription { get; set; }

        [JsonPropertyName("EmployeeCount")]
        public int EmployeeCount => this.Employees.Count();

        [JsonIgnore]
        public List<Employee> Employees { get; set; } = new List<Employee>();

        public void SortEmployees() => this.Employees.Sort((x, y) => x.EmployeeId.CompareTo(y.EmployeeId));
    }

}
