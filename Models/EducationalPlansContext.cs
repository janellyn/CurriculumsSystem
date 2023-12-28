using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;


namespace TaskAuthenticationAuthorization.Models
{
    public class EducationalPlansContext : DbContext
    {
        public DbSet<Faculty_Institute> Faculty_Institute { get; set; }
        public DbSet<Speciality> Speciality { get; set; }
        public DbSet<Educational_Level> Educational_Level { get; set; }
        public DbSet<Educational_Program> Educational_Program { get; set; }
        public DbSet<Learning_Form> Learning_Form { get; set; }
        public DbSet<Curriculum> Curriculum { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Cabinet> Cabinet { get; set; }
        public DbSet<Userr> Userr { get; set; }
        public DbSet<Cabinet_Curriculum> Cabinet_Curriculum { get; set; }
        public DbSet<Course_Curriculum> Course_Curriculum { get; set; }

        public EducationalPlansContext(DbContextOptions<EducationalPlansContext> options)
            : base(options)
        {
            
        }

        public EducationalPlansContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
