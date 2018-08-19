using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace FSDP.UI.MVC.Models
{
    public class ContactViewModel
    {
        [ScaffoldColumn(false)]
        public DateTime DateSent { get; set; }

        [Required(ErrorMessage = "* Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "* Required")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "* Required")]
        public string Message { get; set; }

        [Required(ErrorMessage = "* Required")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

    }
}