using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("LoginLogs", Schema = "LG")]
    public class LoginLogs
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int BranchId { get; set; }
        [Required]
        public int RoleId { get; set; }

        public DateTime From { get; set; }
        public Nullable<DateTime> To { get; set; }
        [MaxLength(200), Column(TypeName = "nvarchar")]
        public string IP { get; set; }
    }
}