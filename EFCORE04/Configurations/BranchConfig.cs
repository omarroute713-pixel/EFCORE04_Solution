using EFCORE04.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EFCORE04.Configurations
{
    internal class BranchConfig : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasKey(b => b.Code);



            builder.HasMany(b => b.Accounts)
                .WithOne(a => a.Branch)
                .HasForeignKey(a => a.Code);

            builder.HasData(

                new Branch
                {
                    Code = "CAI-02",
                    Name = "Cairo Main Branch",
                    Address = "Nasr City, Cairo",
                    PhoneNumber = "022222222",
                    ManagerId = 1
                },
                new Branch
                {
                    Code = "CAI-01",
                    Name = "Alexandria Branch",
                    Address = "Stanley, Alexandria",
                    PhoneNumber = "033333333",
                    ManagerId = 2
                }
            );
        }
    }
}
