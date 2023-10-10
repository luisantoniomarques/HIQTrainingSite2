using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.ModelDetail;
using HIQTraining.Model;

namespace HIQTraining.DAL.Course
{
    public interface ITrainingAreaDao
    {
        IEnumerable<TrainingAreaDetail> GetTrainingArea();
    }
}
