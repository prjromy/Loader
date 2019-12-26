using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("BloodGroup",Schema = "LG")]
    public class BloodGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BGId { get; set; }
        public string BGName { get; set; }
    }
}