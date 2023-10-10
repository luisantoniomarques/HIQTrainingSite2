using HIQTraining.Model;
using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.DAL.Inscription
{
    public interface IInscriptionDao
    {
        List<InscriptionDetail> GetCourseInscriptionList(int courseId);

        InscriptionDetail GetInscriptionById(int id);
        Model.Inscription GetInscriptionById(int id, DBEntities db);
        void UpdateInscription(Model.Inscription inscription, DBEntities db);

        InscriptionDetail GetInscriptionByCourseIDAndStudentId(int courseId, int studentId);

        List<InscriptionDetail> GetInscriptionByStudentId(int studentId, int courseId);

        Model.Inscription AddInscription(Model.Inscription inscription, string user, DBEntities db);

        int Delete(int id);
    }
}
