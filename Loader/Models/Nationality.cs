using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("Nationality", Schema = "LG")]
    public class Nationality
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NId { get; set; }
        public string NName { get; set; }
    }
}