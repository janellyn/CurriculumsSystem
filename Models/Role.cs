using System.Collections;
using System.Collections.Generic;

namespace TaskAuthenticationAuthorization.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Userr> Users { get; set; } = new List<Userr>();
    }
}
