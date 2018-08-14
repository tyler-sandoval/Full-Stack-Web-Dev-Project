using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSDP.DATA.EF//.Metadata
{
    public class CourseCompletionsMetadata
    {
        [Required(ErrorMessage = "* User ID Required.")]
        [Display(Name = "User ID")]
        [StringLength(128, ErrorMessage = "* User ID cannot exceed 128 characters.")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "* Lesson ID Required.")]
        [Display(Name = "Lesson ID")]
        [Range(0, int.MaxValue, ErrorMessage = "* Invalid Range, Try Again.")]
        public int CourseID { get; set; }

        [Required(ErrorMessage = "* Date Viewed Required.")]
        [Display(Name = "Date Viewed")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public System.DateTime DateCompleted { get; set; }

    }

    [MetadataType(typeof(CourseCompletionsMetadata))]
    public partial class CourseCompletions { }
}
