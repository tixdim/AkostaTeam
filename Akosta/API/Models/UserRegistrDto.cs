namespace Akosta.API.Models
{
    public class UserRegistrDto
    {
        public string Telegram { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FirstPassword { get; set; }
        public string SecondPassword { get; set; }
        public bool IsWorker { get; set; }
        public string Skill { get; set; }
    }
}
