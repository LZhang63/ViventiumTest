using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Infrastructure;
using ViventiumTest.Helpers;

namespace ViventiumTest.Models
{
    public class ViventiumTestDbContext:DbContext
    {
        public ViventiumTestDbContext(DbContextOptions<ViventiumTestDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Add CompanyId to be used as foreign key
            _ = modelBuilder.Entity<Employee>().Property<uint>("CompanyId");
          
            _ = modelBuilder.Entity<Employee>()
            .HasOne(e => e.Company)
            .WithMany(c => c.Employees)
            .HasForeignKey("CompanyId")               
            .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;


        /// <summary>
        /// Replace all records of Companies with records loaded from CSV file
        /// </summary>
        /// <param name="companyList"></param>
        /// <returns></returns>
        public async Task ReloadAllCompaniesAsync(List<CompanyCSV> companyList)
        {
            this.Companies.RemoveRange(this.Companies.Include(c => c.Employees));

            List<Company> allCompanies = new();

            foreach (int companyID in companyList.Select(r => r.CompanyId).Distinct())
            {
                Company company = companyList.Where(r => r.CompanyId == companyID).First()._company;
                company.Employees = companyList.Where(r => r.CompanyId == company.CompanyId).Select(r => r._employee).ToList();
                allCompanies.Add(company);

            }
            await this.Companies.AddRangeAsync(allCompanies);
            _ = await this.SaveChangesAsync();
        }
    }
}
