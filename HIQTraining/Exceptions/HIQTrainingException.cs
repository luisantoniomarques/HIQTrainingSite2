using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.Exceptions
{
    public class HIQTrainingException : Exception
    {
        public HIQTrainingException(string message)
            : base(message)
        {

        }
    }
}
