using Microsoft.EntityFrameworkCore;

namespace TaskAuthenticationAuthorization.Models
{
    [PrimaryKey("CourseID", "CurriculumID")]
    public class Course_Curriculum
    {
        public int CourseID { get; set; }
        public int CurriculumID { get; set; }

        public virtual Course Course { get; set; }
        public virtual Curriculum Curriculum { get; set; }
    }
}
