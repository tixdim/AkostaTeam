using System;

namespace Akosta.API.Models
{
    public class UserInformationDto
    {
        public int Id { get; set; }
        public string Telegram { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsWorker { get; set; }
        public string Skill { get; set; }
    }
}
