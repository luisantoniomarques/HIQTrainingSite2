using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace HIQTraining.DAL.Configuration
{
   public interface ICourseTypeDao {

        IPagedList<CourseTypeDetail> GetPagedCoursesTypes(int page, int pageSize);

        IEnumerable<CourseTypeDetail> GetCoursesTypes();
      
        Model.CourseType GetCourseTypeById(int id);

        Model.CourseType Add(Model.CourseType courseLevel);

        int Update(Model.CourseType courseLevel);

        int Delete(Model.CourseType courseLevel);
    }
}
