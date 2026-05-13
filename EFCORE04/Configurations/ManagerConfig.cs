using EFCORE04.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCORE04.Configurations
{
    internal class ManagerConfig : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.HasKey(m => m.Id);

            builder.HasOne(m => m.Branch)
                .WithOne(b => b.Manager)
                .HasForeignKey<Branch>(b => b.ManagerId);

            builder.HasIndex(m => m.Email)
                  .IsUnique();

            builder.HasData(
                new Manager
                {
                Id = 1,
                FullName = "Ahmed Hassan",
                Email = "ahmed.hassan@nationalbank.com",
                PhoneNumber = "01011112222",
                HireDate = new DateTime(2020, 1, 15)
                },
                new Manager
                {
                Id = 2,
                FullName = "Sara Mohamed",
                Email = "sara.mohamed@nationalbank.com",
                PhoneNumber = "01033334444",
                HireDate = new DateTime(2021, 5, 10)
                }
            );
        }
    }
}
