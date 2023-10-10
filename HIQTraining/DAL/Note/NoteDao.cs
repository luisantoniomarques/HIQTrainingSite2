using HIQTraining.Model;
using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using PagedList;

namespace HIQTraining.DAL.Notes
{
    public class NoteDao : INoteDao
    {

        /// <summary>
        /// Gets all notes
        /// </summary>
        /// <returns></returns>
        public void SaveChanges(IList<NoteDetail> notes, string Username)
        {
            using (DBEntities db = new DBEntities())
            {
                foreach (NoteDetail noteDetail in notes)
                {

                    Note note = new Note
                    {
                        eventstart = noteDetail.EventStart,
                        eventend = noteDetail.EventEnd,
                        UserCreated = Username,
                        text = noteDetail.text
                    };

                    if (noteDetail.UserCreated == null && noteDetail.isDeleted == false)
                    {
                        db.Notes.Add(note);
                    }

                    if(noteDetail.isUpdated == true && noteDetail.isDeleted == false)
                    {
                        note.id = noteDetail.id;

                        db.Notes.Attach(note);
                        db.Entry(note).Property(x => x.eventstart).IsModified = true;
                        db.Entry(note).Property(x => x.eventend).IsModified = true;
                        db.Entry(note).Property(x => x.text).IsModified = true;
                    }

                    if(noteDetail.UserCreated != null && noteDetail.isDeleted == true)
                    {
                        note.id = noteDetail.id;

                        db.Notes.Attach(note);
                        db.Notes.Remove(note);
                    }
                }

                db.SaveChanges();
            }
        }

        public List<NoteDetail> GetWeekEventsFromUser(DateTime? startDate, DateTime? endDate)
        {
            IList<NoteDetail> notes = new List<NoteDetail>();
            using (DBEntities db = new DBEntities())
            {
               notes = (from note in db.Notes.AsEnumerable()
                        join user in db.AspNetUsers on note.UserCreated equals user.UserName
                        where !(note.eventend <= startDate || note.eventstart >= endDate) //&& note.UserCreated == Username
                                           select new NoteDetail
                                           {
                                               id = note.id,
                                               text = note.text,
                                               EventStart = note.eventstart,
                                               EventEnd = note.eventend,
                                               UserCreated = note.UserCreated,
                                               barColor = user.Color.Trim()
                                           }).ToList();
            }
            return notes.ToList();
        }


        public long GetLastEventFromUser()
        {
            long lastEvent = 0;
            using (DBEntities db = new DBEntities())
            {
                lastEvent = (from note in db.Notes.AsEnumerable()
                                 //where note.UserCreated == Username
                                 select note.id
                            ).Max();
            }
            return lastEvent;
        }


    }
}
