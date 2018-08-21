using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FSDP.DATA.EF.Repositories
{
    public class LessonsRepository : GenericRepository<Lesson>
    {
        public LessonsRepository(DbContext db) : base(db) { }

        public Lesson UntrackedFind(int id)
        {
            return db.Set<Lesson>().AsNoTracking()
                .Where(l => l.LessonID == id).FirstOrDefault();
        }



    }
}
