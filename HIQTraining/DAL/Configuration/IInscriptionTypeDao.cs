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
    public interface IInscriptionTypeDao
    {
        List<InscriptionTypeDetail> GetInscriptionTypes();

        IPagedList<InscriptionTypeDetail> GetPagedInscriptionsTypes(int page, int pageSize);

        InscriptionType GetInscriptionTypeById(int id);

        InscriptionType Add(InscriptionType inscriptionType);

        int Update(InscriptionType inscriptionType);

        int Delete(InscriptionType inscriptionType);
    }
}
