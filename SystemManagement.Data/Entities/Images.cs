using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemManagement.Data.Entities
{
    [Table("Images")]
    public class Images
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Img_Id { get; set; }
       
        [Display(Name = "Img")]
        public byte[] Img { get; set; }   
              
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

        public Guid MO_Id { get; set; }
        [ForeignKey("MO_Id")]
        public virtual Model Model{ get; set; }

    }
}