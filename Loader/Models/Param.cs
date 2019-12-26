using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("DataType", Schema = "LG")]
    public class Datatype
    {
        public Datatype()
        {
            this.paramValue = new HashSet<ParamValue>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public byte DTId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "DataType")]
        public string DType { get; set; }

        [ForeignKey("DTId")]

        public virtual ICollection<ParamValue> paramValue { get; set; }
    }

    [Table("Param", Schema = "LG")]
    public class Param
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Parameter Id")]
        public int PId { get; set; }
        [MaxLength(100)]
        [Required(ErrorMessage = "Parameter Name Is Required!")]
        [Display(Name = "Parameter Name")]
        public string PName { get; set; }
        [Display(Name = "Parent")]
        [Required]
        public int ParentId { get; set; }

        [Display(Name = "Is Group?")]
        public bool IsGroup { get; set; }

        public virtual ParamValue paramValue { get; set; }
    }


    [Table("ParamValue", Schema = "LG")]
    public class ParamValue
    {
        [Key, ForeignKey("param")]
        public int PId { get; set; }
        [Display(Name = "Datatype")]
        [Required(ErrorMessage = "Data Type Is Required!")]
        public byte DTId { get; set; }
        [Display(Name = "Description")]
        [Required]
        public string PDescription { get; set; }
        [Display(Name = "Parameter Value")]
        public string PValue { get; set; }

        public virtual Datatype dataType { get; set; }
        public virtual Param param { get; set; }
        public virtual ParamScript paramScript { get; set; }
    }



    [Table("ParamScript", Schema = "LG")]
    public class ParamScript
    {
        [Key, ForeignKey("paramValue")]
        public int PId { get; set; }
        [Display(Name = "Script")]
        [Required(ErrorMessage = "Script Is required!")]
        public string PScript { get; set; }

        public virtual ParamValue paramValue { get; set; }
    }


}