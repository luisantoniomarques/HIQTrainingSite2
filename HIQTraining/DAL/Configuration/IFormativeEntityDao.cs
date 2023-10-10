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
    public interface IFormativeEntityDao
    {
        IEnumerable<FormativeEntityDetail> GetFormativesEntities();
        IPagedList<FormativeEntityDetail> GetPagedFormativeEntities(int page, int pageSize);

        IEnumerable<FormativeEntityDetail> GetFormativeEntitiesByName(string name);

        FormativeEntity GetFormativeEntityById(int id);

        FormativeEntity Add(FormativeEntity formativeEntity, string user);

        int Save(FormativeEntity formativeEntity, string user);

        int Delete(int id);

    }
}
