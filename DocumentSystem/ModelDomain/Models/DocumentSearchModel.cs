using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class DocumentSearchModel
    {
        [DisplayName("Идентификатор")]
        public int? Id { get; set; }
        [DisplayName("Название")]
        public string Title { get; set; }
        [DisplayName("Дата создания")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        [DisplayName("Автор")]
        public string Author { get; set; }

    }
}