using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using HIQTraining.Model;

namespace HIQTraining.DAL.Student
{
    public interface IStudentDao
    {
        IEnumerable<StudentDetail> GetStudents(string studentName, int companyId);
        IPagedList<StudentDetail> GetPagedStudents(int page, int pageSize);

        IPagedList<StudentDetail> GetPagedStudentsByName(string name, int? company, int? status, int page, int pageSize);

        Task<Model.Student> GetStudentByNameAndEmailAsync(string name, string email);

        Model.Student GetStudentByNameAndEmail(string name, string email);
        Model.Student GetStudentByNameAndEmail(string name, string email, Model.DBEntities db);

        Task<Model.Student> AddStudentAsync(Model.Student student, string user);

        int AddStudent(Model.Student student, string user);

        Model.Student GetStudentById(int id);
        StudentDetail GetStudentDetailById(int id);

        int AddStudent(Model.Student student, string user, Model.DBEntities db);

        int UpdateStudent(Model.Student student);

        int Delete(int id);
        IEnumerable<StudentDetail> SearchExternalUsersByName(string userName);
    }
}
