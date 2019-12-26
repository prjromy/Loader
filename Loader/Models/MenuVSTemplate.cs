
namespace Loader.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MenuVsTemplate",Schema ="LG")]

    public partial class MenuVsTemplate
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MenuId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TemplateId { get; set; }
        //public virtual MenuTemplate MenuTemplate { get; set; }
    }
}
