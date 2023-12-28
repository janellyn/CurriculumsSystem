using Microsoft.EntityFrameworkCore;

namespace TaskAuthenticationAuthorization.Models
{
    [PrimaryKey("CabinetID", "CurriculumID")]
    public class Cabinet_Curriculum
    {
        public int CabinetID { get; set; }
        public int CurriculumID { get; set; }

        public virtual Cabinet Cabinet { get; set; } 
        public virtual Curriculum Curriculum { get; set; }
    }
}
