using System;
using System.Collections.Generic;
using System.Text;

namespace TechDashboard.Models
{
    public class App_WorkTicketText
    {
        public bool _isModified;
        public string _salesOrderNo;
        public string _workTicketNumber;
        public string _workTicketStep;
        public string _sequenceNo;
        public string _text;
        public int _id;

        /// <summary>
        /// ID from table - needed for save and update
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// puke
        /// </summary>
        public bool IsModified
        {
            get { return _isModified; }
            set { _isModified = value; }
        }

        /// <summary>
        /// Sales Order Number
        /// </summary>
        public string SalesOrderNo
        {
            get { return _salesOrderNo; }
        }

        /// <summary>
        /// Work Ticket Number
        /// </summary>
        public string WTNumber
        {
            get { return _workTicketNumber; }
        }

        /// <summary>
        /// Work Ticket Step 
        /// </summary>
        public string WTStep
        {
            get { return _workTicketStep; }
        }

        /// <summary>
        /// Sequence Number 
        /// </summary>
        public string SequenceNo
        {
            get { return _sequenceNo; }
        }

        /// <summary>
        /// Text 
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public App_WorkTicketText(JT_WorkTicketText workTicketText)
        {
            _isModified = workTicketText.IsModified;
            _salesOrderNo = workTicketText.SalesOrderNo;
            _workTicketNumber = workTicketText.WTNumber;
            _workTicketStep = workTicketText.WTStep;
            _sequenceNo = workTicketText.SequenceNo;
            _text = workTicketText.Text;
            _id = workTicketText.ID;
        }

        public App_WorkTicketText(App_WorkTicket workTicket)
        {
            _salesOrderNo = workTicket.SalesOrderNo;
            _workTicketNumber = workTicket.WTNumber;
            _workTicketStep = workTicket.WTStep;
            _sequenceNo = "000";
            _text = string.Empty;
            _id = 0;
        }
    }
}
