namespace LearnApiNetCore.Models
{
    public class UserModel
    {
        public int id { get; set; }

        public string name { get; set; } = string.Empty;

        public bool gender { get; set; }

        public DateTime birthday { get; set; }

        public string phone { get; set; } = string.Empty;

        public string email { get; set; } = string.Empty;

        public string address { get; set; } = string.Empty;
    }
}