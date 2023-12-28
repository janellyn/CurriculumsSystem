namespace TaskAuthenticationAuthorization.Models
{
    public class Course
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Department_Name { get; set; }
        public int Disciplines { get; set; }
        public int Lectures { get; set; }
        public int Practical { get; set; }
        public int Laboratory { get; set; }
        public int Self_Study { get; set; }
    }
}
