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
        [Required(ErrorMessage = "* User Required.")]
        [Display(Name = "User")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public string UserID { get; set; }

        [Required(ErrorMessage = "* Course Required.")]
        [Display(Name = "Course")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public int CourseID { get; set; }

        [Required(ErrorMessage = "* Date Viewed Required.")]
        [Display(Name = "Date Viewed")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public System.DateTime DateCompleted { get; set; }

    }

    [MetadataType(typeof(CourseCompletionsMetadata))]
    public partial class CourseCompletion { }
}
