using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SystemManagement.Data.Entities
{
    [Table("SubModel")]
    public class SubModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SM_Id { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(30)]
        [Display(Name = "Name")]
        public string SM_Name { get; set; }

        [Required]
        [Column(TypeName = "varchar(max)")]

        [Display(Name = "Discription")]
        public string SM_Discription { get; set; }

        [Required]
        [Column(TypeName = "varchar(max)")]

        [Display(Name = "Feature")]
        public string SM_Feature { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Price")]
        public decimal SM_Price { get; set; }
     
        [Column(TypeName = "DateTime")]
        [Display(Name = "Create Date")]
        public DateTime CreatedDate { get; set; }
       
        [Column(TypeName = "varchar")]
        [StringLength(30)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

   
        [Column(TypeName = "DateTime")]
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        
        [Column(TypeName = "varchar")]
        [StringLength(30)]
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public Guid MO_Id { get; set; }
        [ForeignKey("MO_Id")]
        public virtual Model Models { get; set; }

    }
}
