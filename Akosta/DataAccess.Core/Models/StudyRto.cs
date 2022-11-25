using System.ComponentModel.DataAnnotations.Schema;

namespace Akosta.DataAccess.Core.Models
{
    [Table("Studys")]
    public class StudyRto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserRto User { get; set; }
        public string SkillsInCource { get; set; }
        public int Store { get; set; }
    }
}
