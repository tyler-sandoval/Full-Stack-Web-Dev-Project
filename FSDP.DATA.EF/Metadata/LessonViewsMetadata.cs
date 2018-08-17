using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSDP.DATA.EF//.Metadata
{
    public class LessonViewsMetadata
    {
        [Required(ErrorMessage = "* User ID Required.")]
        [Display(Name = "User")]
        [StringLength(128, ErrorMessage = "* User ID cannot exceed 128 characters.")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "* Lesson ID Required")]
        [Display(Name = "Lesson")]
        [Range(0,int.MaxValue, ErrorMessage = "* Invalid Range, Try Again.")]
        public int LessonID { get; set; }

        [Required(ErrorMessage = "* Date Viewed Required.")]
        [Display(Name = "Date Viewed")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public System.DateTime DateViewed { get; set; }

    }

    [MetadataType(typeof(LessonViewsMetadata))]
    public partial class LessonView { }
}
