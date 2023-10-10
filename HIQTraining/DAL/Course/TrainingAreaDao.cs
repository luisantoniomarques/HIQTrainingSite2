using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.ModelDetail;
using HIQTraining.Model;

namespace HIQTraining.DAL.Course
{
    class TrainingAreaDao
    {
        public IEnumerable<TrainingAreaDetail> GetTrainingArea()
        {
            using (DBEntities db = new DBEntities())
            {
                var trainingArea = db.TrainingAreas.Select(c => new TrainingAreaDetail { Id = c.Id, Descriptio = c.Descriptio});

                return trainingArea.ToList();
            }
        }
    }
}
