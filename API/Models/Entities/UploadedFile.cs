using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entities
{
    [Table("UploadedFiles")]
    public class UploadedFile
    {
        [Column("FileId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int FileId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public FileCategory Category { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string FileName { get; set; }
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }
        [Required]
        [Column(TypeName = "varbinary(max)")]
        public byte[] FileAsByteArray { get; set; }
    }
}
