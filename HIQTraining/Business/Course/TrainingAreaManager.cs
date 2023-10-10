using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.ModelDetail;
using HIQTraining.DAL.Course;

namespace HIQTraining.Business.Course
{
    class TrainingAreaManager
    {
        TrainingAreaDao dao;

        public IEnumerable<TrainingAreaDetail> GetCertificationsTypes()
        {
            return dao.GetTrainingArea();
        }
    }
}
