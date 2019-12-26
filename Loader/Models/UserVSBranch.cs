using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("UserVSBranch", Schema = "LG")]
    public class UserVSBranch
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name ="User")]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        public DateTime From { get; set; }
        
        public Nullable<DateTime> To { get; set; }
        
        public bool IsEnable { get; set; }
        [Required]
        [Display(Name = "Assign Role")]
        public int RoleId { get; set; }
        [Required]
        public int PostedBy { get; set; }
        [Required]
        public DateTime PostedOn { get; set; }

        [NotMapped]
        public bool IsPermanent { get; set; }
    }
}