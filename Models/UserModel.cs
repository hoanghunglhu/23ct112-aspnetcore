namespace LearnApiNetCore.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public string name { get; set; }

        public bool gender { get; set; }

        public DateTime birthday { get; set; }

        public string phone { get; set; }

        public string email { get; set; }

        public string address { get; set; }
    }
}