using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("LicenseBranch", Schema = "fin")]
    public class LicenseBranch
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrnhID { get; set; }
        public string BrnhNam { get; set; }
        public string Addr { get; set; }
        public Nullable<short> RegID { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string IPAdd { get; set; }
        public Nullable<short> PBrnhID { get; set; }
        public Nullable<byte> ExtraUsrNo { get; set; }
        public string Prefix { get; set; }
        public int CalSID { get; set; }
        public int CalCid { get; set; }
        public System.DateTime TDate { get; set; }
        public bool UseLimit { get; set; }
        public bool atclosing { get; set; }
        public Nullable<System.DateTime> MigDate { get; set; }
        public string CleCode { get; set; }
        public Nullable<byte> inExpMode { get; set; }
        public Nullable<decimal> Floint { get; set; }
        public bool IsCalcIOnI { get; set; }
        public int schprovdys { get; set; }
        public Nullable<int> FYID { get; set; }
    }
}