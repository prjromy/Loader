using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("Gender", Schema = "LG")]
    public class Gender
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GenderId { get; set; }
        public string GenderName { get; set; }
    }
}