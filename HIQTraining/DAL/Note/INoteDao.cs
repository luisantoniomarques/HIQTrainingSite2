using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.Model;
using PagedList;

namespace HIQTraining.DAL.Notes {
    public interface INoteDao {
        List<NoteDetail> GetWeekEventsFromUser(DateTime? startDate, DateTime? endDate);
        void SaveChanges(IList<NoteDetail> notes, string Username);
        long GetLastEventFromUser();
    }
}
