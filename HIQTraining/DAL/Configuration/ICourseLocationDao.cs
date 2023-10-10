using HIQTraining.Model;
using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.DAL.Configuration
{
    public interface ICourseLocationDao {

        List<CourseLocationDetail> GetCourseLocations();
        IPagedList<CourseLocationDetail> GetPagedCourseLocations(int page, int pageSize);

        CourseLocation GetCourseLocationById(int id);

        CourseLocation Add(CourseLocation courseLocation);

        int Update(CourseLocation courseLocation);

        int Delete(CourseLocation courseLocation);
    }
}
