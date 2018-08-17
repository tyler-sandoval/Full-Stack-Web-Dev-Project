using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSDP.DATA.EF//.Metadata
{
    public class CourseMetadata
    {
        [Required(ErrorMessage = "* Course Name Required.")]
        [Display(Name = "Course Name")]
        [StringLength(200, ErrorMessage = "* Course Name cannot exceed 200 characters.")]
        public string CourseName { get; set; }

        [UIHint("MultilineText")]
        [DisplayFormat(NullDisplayText = "- No description available -")]
        [StringLength(500, ErrorMessage = "* Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        //bool does not require any further data annotations
        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

    }

    [MetadataType(typeof(CourseMetadata))]
    public partial class Cours { }
}
