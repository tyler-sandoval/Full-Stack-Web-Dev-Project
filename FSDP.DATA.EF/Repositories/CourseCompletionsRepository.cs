using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSDP.DATA.EF.Repositories
{
    public class CourseCompletionsRepository : GenericRepository<CourseCompletion>
    {
        public CourseCompletionsRepository(DbContext db) : base(db) { }
    }
}
