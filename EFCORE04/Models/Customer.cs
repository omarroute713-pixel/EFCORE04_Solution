using EFCORE04.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCORE04.Models
{
    internal class Customer
    {
        #region Properties
        public int Id { get; set; }

        public string Address { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string NationalId { get; set; } = null!;

        public CustomerType CustomerType { get; set; }
        #endregion


        #region Relationshipes


        #region Customer - Account  (M - M)
        public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new List<CustomerAccount>();
        #endregion


        #endregion



    }
}
