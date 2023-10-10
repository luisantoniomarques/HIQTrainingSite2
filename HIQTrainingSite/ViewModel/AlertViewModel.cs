using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel
{
    public class AlertViewModel
    {
        const string DISPLAY_MESSAGE = "display:block;";
        const string HIDE_MESSAGE = "display:none;";

        public AlertViewModel()
        {
            DisplayMessage = HIDE_MESSAGE;
        }


        public string AlertType { get; set; }
        public string MessagePrefix { get; set; }
        public string Message { get; set; }
        public string DisplayMessage { get; set; }


        public void SetSuccessMessage(string message)
        {
            AlertType = "alert-success";
            MessagePrefix = string.Concat(HIQResources.messageSuccess, "!");
            Message = message;
            DisplayMessage = DISPLAY_MESSAGE;
        }
        public void SetErrorMessage(string message)
        {
            AlertType = "alert-danger";
            MessagePrefix = string.Concat(HIQResources.messageError, "!");
            Message = message;
            DisplayMessage = DISPLAY_MESSAGE;
        }
    }
}