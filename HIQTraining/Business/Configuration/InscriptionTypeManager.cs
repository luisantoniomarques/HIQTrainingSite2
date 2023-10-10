using HIQTraining.DAL.Configuration;
using HIQTraining.Model;
using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.Business.Configuration
{
    public class InscriptionTypeManager
    {
        IInscriptionTypeDao dao;

        public InscriptionTypeManager(IInscriptionTypeDao dao)
        {
            this.dao = dao;
        }

        /// <summary>
        /// Gets all inscriptions types
        /// </summary>
        /// <returns></returns>
        public List<InscriptionTypeDetail> GetInscriptionTypes()
        {
            return dao.GetInscriptionTypes();
        }

        /// <summary>
        /// gets all the inscription types (paged)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<InscriptionTypeDetail> GetPagedInscriptionTypes(int page = 1, int pageSize = 10) {
            return dao.GetPagedInscriptionsTypes(page, pageSize);
        }

        /// <summary>
        /// Gets a inscription type by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InscriptionType GetInscriptionTypeById(int id) {
            return dao.GetInscriptionTypeById(id);
        }

        /// <summary>
        /// Adds a new inscription type
        /// </summary>
        /// <param name="inscriptionType"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public InscriptionType Add(InscriptionType inscriptionType, string userName) {
            inscriptionType.UserCreated = userName;
            inscriptionType.CreatedDate = DateTime.Now;

            return dao.Add(inscriptionType);
        }

        /// <summary>
        /// updates a inscription type
        /// </summary>
        /// <param name="inscriptionType"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int Update(InscriptionType inscriptionType, string userName) {
            inscriptionType.CreatedDate = DateTime.Now;
            inscriptionType.UserCreated = userName;

            return dao.Update(inscriptionType);
        }

        /// <summary>
        /// deletes a inscription type
        /// </summary>
        /// <param name="inscriptionType"></param>
        /// <returns></returns>
        public int Delete(InscriptionType inscriptionType) {
            return dao.Delete(inscriptionType);
        }

    }
}
