using HIQTraining.DAL.Configuration;
using HIQTraining.Exceptions;
using HIQTraining.LocalResources;
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
    public class FormativeEntityManager
    {
        IFormativeEntityDao dao;

        public FormativeEntityManager(IFormativeEntityDao dao)
        {
            this.dao = dao;
        }


        /// <summary>
        /// Gets all course types
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FormativeEntityDetail> GetFormativeEntities()
        {
            return dao.GetFormativesEntities().ToList();
        }

        /// <summary>
        /// gets all the formative entities (paged)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<FormativeEntityDetail> GetPagedFormativeEntities(int page, int pageSize) {
            return dao.GetPagedFormativeEntities(page, pageSize);
        }


        /// <summary>
        /// Get a course type detail by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FormativeEntity GetFormativeEntityById(int id)
        {
            return dao.GetFormativeEntityById(id);
        }

        /// <summary>
        /// Creates a new course type
        /// </summary>
        /// <param name="formativeEntity"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public FormativeEntity Add(FormativeEntity formativeEntity, string user)
        {
            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("user parameter is required");

            formativeEntity.UserCreated = user;
            formativeEntity.CreatedDate = DateTime.Now;

            return dao.Add(formativeEntity, user);
        }

        /// <summary>
        /// Saves an existing course type
        /// </summary>
        /// <param name="formativeEntity"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Save(FormativeEntity formativeEntity, string user)
        {
            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("user parameter is required");

            return dao.Save(formativeEntity, user);
        }

        /// <summary>
        /// Deletes a course type from  database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id)
        {
            var courseType = dao.GetFormativeEntityById(id);
            if (courseType == null)
                throw new HIQTrainingException(HIQResource.errorMessageUnknownRecord);


            return dao.Delete(id);
        }

        /// <summary>
        /// gets all the formative entities by name
        /// </summary>
        /// <param name="formativeEntity"></param>
        /// <returns></returns>
        public IEnumerable<FormativeEntityDetail> GetFormativeEntitiesByName(string formativeEntity) {
            return dao.GetFormativeEntitiesByName(formativeEntity).ToList();
        }

    }
}
