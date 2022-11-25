using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akosta.DataAccess.Core.Models
{
    [Table("Users")]
    public class UserRto
    {
        [Key] public int Id { get; set; }
        [Required] public string Telegram { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Surname { get; set; }
        [Required, MinLength(6)] public string Password { get; set; }
        [Required] public bool IsWorker { get; set; }
        [Required] public string Skill { get; set; }
        public List<StudyRto> Studys { get; set; }
    }
}
