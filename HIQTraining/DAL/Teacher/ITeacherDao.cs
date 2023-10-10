using HIQTraining.Model;
using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.DAL.Teacher {
    public interface ITeacherDao {

        IQueryable<TeacherDetail> GetTeachers(DBEntities db);

        IEnumerable<TeacherDetail> GetTeachers(string teacherName);

        IPagedList<TeacherDetail> GetPagedTeachers(int page, int pageSize);

        IPagedList<TeacherDetail> GetPagedTeachersByName(string teacherName, int page, int pageSize);

        Model.Teacher GetTeacherById(int id);

        Model.Teacher Add(Model.Teacher teacher);

        int Update(Model.Teacher teacher);

        int Delete(int id);

        TeacherDetail GetTeacherDetailById(int id);

        Model.Teacher GetTeacherByNameAndEmail(string name, string email, Model.DBEntities db);

        Model.Teacher GetTeacherByName(string name, Model.DBEntities db);

        IPagedList<TeacherDetail> GetTeachersSearchResults(string name, int? companyId, int? state, int page, int pageSize);
    }
}
