using HIQTraining.DAL.Notes;
using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.Business.Note {
    public class NoteManager {
        INoteDao dao;

        public NoteManager(INoteDao dao) {
            this.dao = dao;
        }

        public void SaveChanges(IList<NoteDetail> notes, string UserName)
        {
            dao.SaveChanges(notes, UserName);
        }

        public List<NoteDetail> GetWeekEventsFromUser(DateTime? startDate, DateTime? endDate)
        {
            return dao.GetWeekEventsFromUser(startDate, endDate);
        }

        public long GetLastEventFromUser()
        {
            return dao.GetLastEventFromUser();
        }


    }
}

