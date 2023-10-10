using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel
{
    public class NotesViewModel
    {
        public IList<NoteDetail> NoteList { get; set; }
    }
}