using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.UI.WebControls;

namespace Loader.Models
{
    [Table("Religion",Schema="LG")]
    public class Religion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RId { get; set; }
        public string ReligionName { get; set; }
    }
}