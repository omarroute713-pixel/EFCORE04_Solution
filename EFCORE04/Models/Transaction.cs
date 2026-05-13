using EFCORE04.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCORE04.Models
{
    internal class Transaction
    {
        #region Properties
      
        public int TransactionNumber { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Amount { get; set; }

        public string Note { get; set; } = null!;

        public TransactionType TransactionType { get; set; }
        #endregion

        #region Relationshipes

        #region Account - Transaction (1 - M Mandatory)
        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;

        #endregion

        #endregion

    }
}
