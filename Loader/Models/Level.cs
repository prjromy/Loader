using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("Level", Schema = "LG")]
    public class Level
    {
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public int PostedBy { get; set; }
        public DateTime PostedOn { get; set; }
    }
}