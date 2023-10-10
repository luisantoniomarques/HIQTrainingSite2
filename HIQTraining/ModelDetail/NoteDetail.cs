using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ModelDetail
{
    public class NoteDetail : ModelBaseDetail
    {
        public int id { get; set; }
        public string text { get; set; }
        public string barColor { get; set; }
        public string backColor { get; set; }
        public DateTime EventStart { get; set; }
        public DateTime EventEnd { get; set; }
        public bool isDeleted { get; set; }
        public bool isUpdated { get; set; }
        public string start
        {
            get
            {
                return EventStart.ToString("s");
            }
        }
        public string end
        {
            get
            {
                return EventEnd.ToString("s");
            }
        }
    }
}
