using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace FSDP.DATA.EF//.Metadata
{
    public class CourseAssignmentMetadata
    {

        [Required(ErrorMessage = "* Course Required.")]
        [Display(Name = "Course")]
        [Range(0, int.MaxValue, ErrorMessage = "* Invalid Range, Try Again.")]
        public int CourseID { get; set; }


        [Required(ErrorMessage = "* User Required.")]
        [Display(Name = "User")]
        [StringLength(128, ErrorMessage = "* User ID cannot exceed 128 characters.")]
        public string UserID { get; set; }

    }

    [MetadataType(typeof(CourseAssignmentMetadata))]
    public partial class CourseAssignment { }
}
