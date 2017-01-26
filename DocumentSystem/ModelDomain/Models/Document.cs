using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Document
    {
        [Key]
        public virtual int Id { get; set; }
        [DisplayName("Название")]
        public virtual string Title { get; set; }
        [DisplayName("Дата создания")]
        public virtual DateTime CreationDate { get; set; }
        public virtual int UserId { get; set; }
        [DisplayName("Автор")]
        public virtual User Author { get; set;  }
        public virtual string ShortName
        {
            get
            {
                int viewlenght = 15;
                return (Title.Length > viewlenght) ? $"{Title.Substring(0, viewlenght)}..." : Title;
            }
        }
        public virtual string Path { get; set; }
    }

}