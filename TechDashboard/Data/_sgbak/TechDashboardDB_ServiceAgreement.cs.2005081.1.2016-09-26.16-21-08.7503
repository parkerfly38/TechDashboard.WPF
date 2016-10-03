using System;
using System.Collections.Generic;
using System.Text;

using SQLite;
using System.Linq;
using TechDashboard.Models;

namespace TechDashboard.Data
{
    public partial class TechDashboardDatabase
    {
        #region Service Agreement Header

        public void FillServiceAgreementHeaderTable()
        {
            FillLocalTable<JT_ServiceAgreementHeader>();  // puke filter
        }

        #endregion

        #region App_ServiceAgreement

        public App_ServiceAgreement GetServiceAgreement(App_Customer customer)
        {
            JT_ServiceAgreementHeader serviceAgreementHeader = null;
            JT_ServiceAgreementDetail serviceAgreementDetail = null;
            JT_ServiceAgreementPMDetail serviceAgreementPmDetail = null;

            lock (_locker)
            {
                serviceAgreementHeader =
                    _database.Table<JT_ServiceAgreementHeader>().Where(
                        sah => (sah.ARDivisionNo == customer.ARDivisionNo) &&
                               (sah.CustomerNo == customer.CustomerNo)
                    ).FirstOrDefault();

                serviceAgreementDetail =
                    _database.Table<JT_ServiceAgreementDetail>().Where(
                        sad => (sad.ARDivisionNo == serviceAgreementHeader.ARDivisionNo) &&
                               (sad.CustomerNo == serviceAgreementHeader.CustomerNo) &&
                               (sad.ContractCode == serviceAgreementHeader.ContractCode)
                    ).FirstOrDefault();

                serviceAgreementPmDetail =
                    _database.Table<JT_ServiceAgreementPMDetail>().Where(
                        sapmd => (sapmd.ARDivisionNo == serviceAgreementHeader.ARDivisionNo) &&
                                 (sapmd.CustomerNo == serviceAgreementHeader.CustomerNo) &&
                                 (sapmd.ContractCode == serviceAgreementHeader.ContractCode)
                    ).FirstOrDefault();
            }

            return new App_ServiceAgreement(serviceAgreementHeader, serviceAgreementDetail, serviceAgreementPmDetail);
        }

        public App_ServiceAgreement GetServiceAgreement(JT_WorkTicket workTicket)
        {
			JT_ServiceAgreementHeader serviceAgreementHeader = new JT_ServiceAgreementHeader();
			JT_ServiceAgreementDetail serviceAgreementDetail = new JT_ServiceAgreementDetail();
			JT_ServiceAgreementPMDetail serviceAgreementPmDetail = new JT_ServiceAgreementPMDetail();

            lock (_locker)
            {
                serviceAgreementHeader =
                    _database.Table<JT_ServiceAgreementHeader>().Where(
                        sah => (sah.ContractCode == workTicket.HdrServiceContractCode)
                    ).FirstOrDefault();

                if (serviceAgreementHeader != null)
                {
                    serviceAgreementDetail =
                        _database.Table<JT_ServiceAgreementDetail>().Where(
                            sad => (sad.ARDivisionNo == serviceAgreementHeader.ARDivisionNo) &&
                                   (sad.CustomerNo == serviceAgreementHeader.CustomerNo) &&
                                   (sad.ContractCode == serviceAgreementHeader.ContractCode)
                        ).FirstOrDefault();

                    serviceAgreementPmDetail =
                        _database.Table<JT_ServiceAgreementPMDetail>().Where(
                            sapmd => (sapmd.ARDivisionNo == serviceAgreementHeader.ARDivisionNo) &&
                                     (sapmd.CustomerNo == serviceAgreementHeader.CustomerNo) &&
                                     (sapmd.ContractCode == serviceAgreementHeader.ContractCode)
                        ).FirstOrDefault();
                }
            }

            return new App_ServiceAgreement(serviceAgreementHeader, serviceAgreementDetail, serviceAgreementPmDetail);
        }

        #endregion
    }
}
