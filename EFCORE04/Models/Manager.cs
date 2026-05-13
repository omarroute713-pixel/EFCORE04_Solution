using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCORE04.Models
{
    internal class Manager
    {
        #region Properties
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;


        public DateTime HireDate { get; set; }
        #endregion


        #region Relationshipes

        #region Branch - Manager (1 - 1)
        public Branch Branch { get; set; } = null!;
        #endregion

        #endregion
    }
}
