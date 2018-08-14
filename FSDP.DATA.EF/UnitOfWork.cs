using FSDP.DATA.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FSDP.DATA.EF
{
    public class UnitOfWork
    {
        public FSDPEntities context = new FSDPEntities();

        private CoursesRespository _coursesRepository;
        public CoursesRespository CoursesRepository
        {
            get
            {
                if (this._coursesRepository == null)
                {
                    this._coursesRepository = new CoursesRespository(context);
                }
                return _coursesRepository;
            }
        }

        private CourseCompletionsRepository _courseCompletionsRepository;
        public CourseCompletionsRepository CourseCompletionsRepository
        {
            get
            {
                if (this._courseCompletionsRepository == null)
                {
                    this._courseCompletionsRepository = new CourseCompletionsRepository(context);
                }
                return _courseCompletionsRepository;
            }
        }

        private LessonsRepository _lessonsRepository;
        public LessonsRepository LessonsRepository
        {
            get
            {
                if (this._lessonsRepository == null)
                {
                    this._lessonsRepository = new LessonsRepository(context);
                }
                return _lessonsRepository;
            }
        }

        private LessonViewsRepository _lessonViewsRepository;
        public LessonViewsRepository LessonViewsRepository
        {
            get
            {
                if (this._lessonViewsRepository == null)
                {
                    this._lessonViewsRepository = new LessonViewsRepository(context);
                }
                return _lessonViewsRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
