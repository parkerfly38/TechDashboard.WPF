using System;
using TechDashboard.Data;
using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * NotesPageViewModel.cs
     * 11/22/2016 DCH Add TODO
     * 12/07/2016 DCH Add error handling
     *********************************************************************************************************/
    public class NotesPageViewModel 
    {
        #region Properties

        App_WorkTicketText _workTicketText;
        public App_WorkTicketText WorkTicketText
        {
            get { return _workTicketText; }
        }

        #endregion

        public NotesPageViewModel(App_WorkTicket workTicket)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _workTicketText = App.Database.RetrieveTextFromWorkTicket(workTicket);

                if (_workTicketText == null)
                {
                    _workTicketText = new App_WorkTicketText(workTicket);
                }
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.NotesPageViewModel(App_WorkTicket workTicket)");
            }
        }

        public NotesPageViewModel()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _workTicketText = App.Database.RetrieveTextFromCurrentAppWorkTicket();
            
                if(_workTicketText == null)
                {
                    App_WorkTicket currentWorkTicket = App.Database.GetCurrentWorkTicket();
                
                    _workTicketText = new App_WorkTicketText(currentWorkTicket);
                }
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.NotesPageViewModel()");
            }
        }

        public void UpdateNotes(string text)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _workTicketText.Text = text;
                App.Database.SaveWorkTicketText(_workTicketText);
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.NotesPageViewModel.UpdateNotes");
            }
        }
    }
}
