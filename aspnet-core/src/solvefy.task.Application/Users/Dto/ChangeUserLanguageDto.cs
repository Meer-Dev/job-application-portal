using System.ComponentModel.DataAnnotations;

namespace solvefy.task.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}