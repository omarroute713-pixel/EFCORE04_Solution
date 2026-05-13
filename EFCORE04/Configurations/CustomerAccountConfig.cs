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
    internal class CustomerAccountConfig : IEntityTypeConfiguration<CustomerAccount>
    {
        public void Configure(EntityTypeBuilder<CustomerAccount> builder)
        {
            builder.HasKey(ca => new { ca.AccountId, ca.CustomerId });

            builder.Property(ca => ca.OwnershipStartDate)
                  .HasDefaultValueSql("GETDATE()");
        }
    }
}
