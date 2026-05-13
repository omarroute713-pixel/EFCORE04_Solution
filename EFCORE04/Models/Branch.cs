using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCORE04.Models
{
    internal class Branch
    {

        #region Properties
        public string Code { get; set; } = null!;

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        #endregion

        #region Relationshipes

        #region Branch - Account (1 - M Mandatory)
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
        #endregion

        #region Branch - Manager (1 - 1)
        public int ManagerId { get; set; }
        public Manager Manager { get; set; } = null!;
        #endregion

        #endregion


    }
}
