using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Document
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual int UserId { get; set; }
        public virtual User Author { get; set;  }
        public virtual string Path { get; set; }
    }

}