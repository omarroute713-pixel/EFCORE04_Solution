using EFCORE04.Enums.NonEntityEnums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCORE04.Models
{
    internal class CustomerAccount
    {
        #region Properties
        public DateTime OwnershipStartDate { get; set; }
        public OwnershipType OwnershipType { get; set; }

        public AccountStatus AccountStatus { get; set; }

        #endregion

        #region Relationshipes

        #region Customer - Account  (M - M)
        public Customer Customer { get; set; } = null!;
        public int CustomerId { get; set; }

        public Account Account { get; set; } = null!;
        public int AccountId { get; set; }
        #endregion

        #endregion

    }
}
