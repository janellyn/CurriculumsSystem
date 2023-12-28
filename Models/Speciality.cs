namespace TaskAuthenticationAuthorization.Models
{
    public class Speciality
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Faculty_InstituteId { get; set; } 
        public virtual Faculty_Institute Faculty_Institute { get; set; }
    }
}
