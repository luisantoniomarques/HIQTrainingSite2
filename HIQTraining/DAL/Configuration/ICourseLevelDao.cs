using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;


namespace HIQTraining.DAL.Configuration
{
    public interface ICourseLevelDao {

        IPagedList<CourseLevelDetail> GetPagedCoursesLevels(int page, int pageSize);

        IEnumerable<CourseLevelDetail> GetCoursesLevels();

        Model.CourseLevel GetCourseLevelById(int id);
        
        Model.CourseLevel Add(Model.CourseLevel courseLevel);

        int Update(Model.CourseLevel courseLevel);

        int Delete(Model.CourseLevel courseLevel);
    }
}
