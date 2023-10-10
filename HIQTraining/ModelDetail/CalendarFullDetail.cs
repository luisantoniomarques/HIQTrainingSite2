using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HIQTraining.ModelDetail {
    public class CalendarFullDetail {

        
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string CourseName { get; set; }

        public string DisplayColor { get; set; }

        public string RoomName { get; set; }
    }
}
