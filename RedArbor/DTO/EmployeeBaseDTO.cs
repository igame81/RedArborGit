using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RedArbor.DTO
{
    public class EmployeeBaseDTO : BaseAuditableEntityDTO
    {
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?([.])([0-9]{3})\2([0-9]{3})$", ErrorMessage = "Entered fax format is not valid.")]
        public string Fax { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Lastlogin { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        public int PortalId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public int StatusId { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?([.])([0-9]{3})\2([0-9]{3})$", ErrorMessage = "Entered telephone format is not valid.")]
        public string Telephone { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }
    }
}