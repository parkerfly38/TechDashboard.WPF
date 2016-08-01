using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    public class NotesPageViewModel 
    {
        App_WorkTicketText _workTicketText;
        public App_WorkTicketText WorkTicketText
        {
            get { return _workTicketText; }
        }

        public NotesPageViewModel(App_WorkTicket workTicket)
        {
            // puke
            //throw new System.NotImplementedException();

            _workTicketText = App.Database.RetrieveTextFromWorkTicket(workTicket);

            if (_workTicketText == null)
            {
                _workTicketText = new App_WorkTicketText(workTicket);
            }
        }

        public NotesPageViewModel()
        {
            _workTicketText = App.Database.RetrieveTextFromCurrentAppWorkTicket();
            
            if(_workTicketText == null)
            {
                App_WorkTicket currentWorkTicket = App.Database.GetCurrentWorkTicket();
                
                _workTicketText = new App_WorkTicketText(currentWorkTicket);
            }
        }

        public void UpdateNotes(string text)
        {
            _workTicketText.Text = text;
            App.Database.SaveWorkTicketText(_workTicketText);
        }
    }
}
