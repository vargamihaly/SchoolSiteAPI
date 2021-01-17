using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entities
{
    [Table("Users")]
    public class User
    {
        [Column("UserId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "varbinary(max)")]
        public byte[] PasswordHash { get; set; }
        [Required]
        [Column(TypeName = "varbinary(max)")]
        public byte[] PasswordSalt { get; set; }
    }
}

