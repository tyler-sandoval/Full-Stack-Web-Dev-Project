using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSDP.DATA.EF//.Metadata
{
    public class LessonMetadata
    {
        [Required(ErrorMessage = "* Lesson Title Required.")]
        [Display(Name = "Lesson Title")]
        [StringLength(200, ErrorMessage = "* Lesson Title cannot exceed 200 characters.")]
        public string LessonTitle { get; set; }

        [Required(ErrorMessage = "* Course ID Required.")]
        [Display(Name = "Course ID")]
        [Range(0, int.MaxValue, ErrorMessage = "* Invalid Range.")]
        public int CourseID { get; set; }

        [DisplayFormat(NullDisplayText = "- Introduction not available -")]
        [UIHint("MultilineText")]
        [StringLength(300, ErrorMessage = "* Introduction cannot exceed 300 characters.")]
        public string Introduction { get; set; }

        [Display(Name = "Video")]
        [DisplayFormat(NullDisplayText = "- Video not available -")]
        [StringLength(250, ErrorMessage = "* Video URL cannot exceed 250 characters.")]
        public string VideoUrl { get; set; }

        [Display(Name = "PDF")]
        [DisplayFormat(NullDisplayText = "- PDF not available -")]
        [StringLength(100, ErrorMessage = "* PDF File Name cannot exceed 100 characters.")]
        public string PdfFilename { get; set; }

        //bool does not require any data annotations for validation
        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

    }
    [MetadataType(typeof(LessonMetadata))]
    public partial class Lesson { }
}
