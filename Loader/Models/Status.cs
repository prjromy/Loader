using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("Status", Schema = "LG")]
    public class Status
    {
      
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("StatusId")]
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }
}