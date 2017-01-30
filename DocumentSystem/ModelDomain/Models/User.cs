using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class User
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual string FullName
        {
            get
            {
                return $"{LastName} {FirstName}";
            }
        }
        public virtual ISet<Document> Documents { get; set; }

    }

}