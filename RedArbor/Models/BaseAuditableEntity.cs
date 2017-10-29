using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedArbor.Models
{
    public class BaseAuditableEntity
    {
        public DateTime? CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}