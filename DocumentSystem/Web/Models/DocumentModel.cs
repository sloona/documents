using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class DocumentModel
    {
        [DisplayName("Название")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Файл")]
        [Required]
        public HttpPostedFileBase Attachment { get; set; }

        public object[] GetParametrs()
        {
            return new object[] { Name };
        }
    }
}