using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SCP.Web.Models
{
    public class InputValidationModel
    {
        public InputValidationModel()
        {
        }

        public InputValidationModel(string name, int age, string notes)
        {
            Name = name;
            Age = age;
            Notes = notes;
        }

        [Required(AllowEmptyStrings = false)]
        [MinLength(2)]
        [MaxLength(100)]
        public string Name { get; set; } = "";

        [Required]
        [Range(1, 200)]
        public int Age { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(5)]
        [MaxLength(400)]
        [AllowHtml]
        public string Notes { get; set; } = "";
    }
}