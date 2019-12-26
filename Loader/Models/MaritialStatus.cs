using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("MaritialStatus", Schema = "LG")]
    public class MaritialStatus
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MSId { get; set; }
        public string MSName { get; set; }
    }
}