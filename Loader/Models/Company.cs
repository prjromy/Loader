using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("Company", Schema = "LG")]
    public class Company
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }

        [Display(Name = "BranchName")]
        [Required(ErrorMessage = "BranchName Is Required"), MaxLength(300), Column(TypeName = "nvarchar")]
        public string BranchName { get; set; }

        [Display(Name = "Region")]
        [Column(TypeName ="nvarchar")]
        public string Region { get; set; }

        [Display(Name = "State")]
        [Column(TypeName = "nvarchar")]
        public string State { get; set; }

        [Required(ErrorMessage = "Address Is Required"), MaxLength(300), Column(TypeName = "nvarchar")]
        public string Address { get; set; }

        [Display(Name = "PhoneNo.")]
        [Column(TypeName = "nvarchar")]
        public string PhoneNo { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email Address Is Required"), MaxLength(300), Column(TypeName = "nvarchar")]
        public string Email { get; set; }


        [Display(Name = "FaxNo.")]
        [Column(TypeName = "nvarchar")]
        public string FaxNo{get; set;}

        [Display(Name = "Prefix")]
        [Required(ErrorMessage = "Prefix Is Required"), MaxLength(300), Column(TypeName = "nvarchar")]
        public string Prefix { get; set; }
        //[Required(ErrorMessage = "Parent Menu Is Required")]
        [Display(Name = "Parent Branch")]
        public int ParentId { get; set; }

        public  Nullable<bool> IsBranch { get; set; }

        [Display(Name = "Additional User")]
        public int AdditionalUser { get; set; }

        [Display(Name = "IP Address")]
        [Required(ErrorMessage = "IP Address Is Required"), MaxLength(300), Column(TypeName = "nvarchar")]
        [RegularExpression(@"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b", ErrorMessage = "Enter Valid IP Address")]
        public string IPAddress { get; set; }

        [Display(Name ="Transaction Date")]
        public Nullable<DateTime> TDate { get; set; }

        [NotMapped]
        public bool IsGroup { get; set; }

    }
}