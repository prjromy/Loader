using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loader.Models
{
    [Table("Department", Schema = "LG")]
    public class Department
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("DepartmentId")]
        public int DeptId { get; set; }

        [Display(Name = "Department Name")]
        [Required(ErrorMessage = "Department Name Is Required"), MaxLength(100), Column(TypeName = "nvarchar")]
        public string DeptName { get; set; }
        [Display(Name = "Parent Department")]
        [Required(ErrorMessage = "Parent Department Is Required")]
        public int PDeptId { get; set; }
        public int PostedBy { get; set; }
        public DateTime PostedOn { get; set; }

    }

}