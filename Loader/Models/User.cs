using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loader.Models
{
    [Table("Users", Schema = "LG")]
    public class Users
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Display(Name = "Employee Name")]
        public int EmployeeId { get; set; }
        
        [Display(Name = "MenuTemplate")]
        public int MTId { get; set; }
        [Required(ErrorMessage = "UserName Is Required"), MaxLength(100), Column(TypeName = "nvarchar")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password Is Required"), MaxLength(100), MinLength(8), Column(TypeName = "nvarchar")]
        public string  Password { get; set; }
        [Display(Name = "Re-Enter Password")]
        [Required(ErrorMessage = "Re-Enter Password"), MaxLength(100),MinLength(8), Column(TypeName = "nvarchar")]
        [Remote("CheckPassword", "Users", AdditionalFields = "Password",ErrorMessage = "Password Does Not Match")]
        public string ReEnterPassword { get; set; }
        [Required(ErrorMessage = "Email Is Required"), MaxLength(100), Column(TypeName = "nvarchar")]
        public string Email { get; set; }
        public bool IsUnlimited { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime From { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime To { get; set; }
        [Display(Name = "Designation Id")]
        public int UserDesignationId { get; set; }
    }
}