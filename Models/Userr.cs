namespace TaskAuthenticationAuthorization.Models
{
    public class Userr
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; } 
        public int CabinetId { get; set; } 
        public virtual Role Role { get; set; }
        public virtual Cabinet Cabinet { get; set; }
    }
}
