using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FSDP.DATA.EF.Repositories
{
    public class LessonViewsRepository : GenericRepository<LessonViews>
    {
        public LessonViewsRepository(DbContext db) : base(db) { }
    }
}
