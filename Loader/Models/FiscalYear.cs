using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("FiscalYears", Schema = "LG")]
    public class FiscalYear
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short FYID { get; set; }

        public string FyName { get; set; }

        public System.DateTime StartDt { get; set; }

        public System.DateTime EndDt { get; set; }
    }
}