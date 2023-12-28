using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace TaskAuthenticationAuthorization.Models
{
    public class Curriculum
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Education_LevelId { get; set; } 
        public int Learning_FormId { get; set; } 
        public int Faculty_InstituteId { get; set; } 
        public int SpecialityId { get; set; } 
        public int Educational_ProgramId { get; set; } 
        public Educational_Level Education_Level { get; set; }
        public virtual Learning_Form Learning_Form { get; set; }
        public virtual Faculty_Institute Faculty_Institute { get; set; }
        public virtual Speciality Speciality { get; set; }
        public virtual Educational_Program Educational_Program { get; set; }

        public virtual ICollection<Cabinet_Curriculum> Cabinet_Curriculum { get; set; }
        public virtual ICollection<Course_Curriculum> Course_Curriculum { get; set; }
    }
}
