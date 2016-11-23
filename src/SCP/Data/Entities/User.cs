using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite.CodeFirst;

namespace SCP.Data.Entities
{
    public class User : EntityBase
    {
        [Required]
        [MaxLength(100)]
        [Column("Name", TypeName = DataTypeStrings.Varchar)]
        public string Name { get; set; }

        [MaxLength(100)]
        [Column("UserName", TypeName = DataTypeStrings.Varchar)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("EMail", TypeName = DataTypeStrings.Varchar)]
        public string EMail { get; set; }

        [MaxLength(100)]
        [Column("Password", TypeName = DataTypeStrings.Varchar)]
        public string Password { get; set; }

        public bool IsStaff { get; set; }

        public bool IsAdmin { get; set; }
    }
}