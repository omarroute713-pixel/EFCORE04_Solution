using EFCORE04.Context;
using EFCORE04.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCORE04.Models
{
    internal class Account
    {
        #region Properties

        public int AccountNumber { get; set; }

        public decimal CurrentBalance { get; set; }

        public AccountType AccountType { get; set; }

        public DateTime OpeningDate { get; set; }

        #endregion

        #region Relationshipes

        #region Branch - Account (1 - M Mandatory)
        public string Code { get; set; }
        public Branch Branch { get; set; } = null!;
        #endregion

        #region Account - Transaction (1 - M Mandatory)

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        #endregion

        #region Customer - Account  (M - M)
        public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new List<CustomerAccount>();
        #endregion

        #endregion

    }
}
