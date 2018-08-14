using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSDP.DATA.EF.Repositories
{
    public class CourseAssignmentsRepository : GenericRepository<CourseAssignment>
    {
        public CourseAssignmentsRepository(DbContext db) : base(db) { }
    }
}
