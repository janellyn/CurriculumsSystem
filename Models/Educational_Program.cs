namespace TaskAuthenticationAuthorization.Models
{
    public class Educational_Program
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int SpecialityId { get; set; } 
        public virtual Speciality Speciality { get; set; }
    }
}
