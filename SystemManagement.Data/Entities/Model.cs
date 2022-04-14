using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemManagement.Data.Entities
{
    [Table("Model")]
    public class Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MO_Id { get; set; }
        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(30)]
        [Display(Name = "Model Name")]
        public string MO_Name { get; set; }
        [Required]
        [Column(TypeName = "varchar(max)")]

        [Display(Name = "Discription")]
        public string MO_Discription { get; set; }

        
        [Column(TypeName = "varchar(max)")]

        [Display(Name = "Feature")]
        public string MO_Feature { get; set; }
     
        [Column(TypeName = "DateTime")]
        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

      
        [Column(TypeName = "varchar")]
        [StringLength(30)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

       
        [Column(TypeName = "DateTime")]
        [StringLength(30)]
        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

       
        [Column(TypeName = "varchar")]
        [StringLength(30)]
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }
        public Guid CR_Id { get; set; }
        [ForeignKey("CR_Id")]   
        public virtual Car Cars { get; set; }
        public ICollection<SubModel> subModels { get; set; }
        public ICollection<Images> Images { get; set; }

    }
}
