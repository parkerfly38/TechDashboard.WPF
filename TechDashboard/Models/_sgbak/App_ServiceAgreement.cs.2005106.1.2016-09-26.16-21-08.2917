using System;
using System.Collections.Generic;
using System.Text;

namespace TechDashboard.Models
{
    public class App_ServiceAgreement
    {
        JT_ServiceAgreementHeader _header = new JT_ServiceAgreementHeader();
        JT_ServiceAgreementDetail _detail = new JT_ServiceAgreementDetail();
        JT_ServiceAgreementPMDetail _pmDetail = new JT_ServiceAgreementPMDetail();

        public string ServiceAgreementNumber
        {
            get
            {
                if ((_header != null) &&
                    (_header.ContractCode != null))
                {
                    return _header.ContractCode;
                }
                else {
                    return null;
                }
            }
        }

        public string Description
        {
            get
            {
                if ((_header != null) &&
                    _header.ContractDescription != null)
                {

                    return _header.ContractDescription;
                }
                else {
                    return null;
                }
            }
        }

        public bool ArePartsCovered
        {
            get
            {
                return ((_detail != null) &&
                        (_detail.PartsCovered != null) &&
                        (_detail.PartsCovered.Trim().ToUpper() == "Y"));
            }
        }

        public bool ArePreventativeMaintenancePartsCovered
        {
            get
            {
                return ((_pmDetail != null) &&
                        (_pmDetail.PartsCovered != null) &&
                        (_pmDetail.PartsCovered.Trim().ToUpper() == "Y"));
            }
        }

        public string BillingType
        {
            get
            {
                if ((_detail != null) && (_detail.BillingType != null))
                {
                    return _detail.BillingType.Trim().ToUpper();
                }
                else
                {
                    return null;
                }
            }
        }


        public App_ServiceAgreement(JT_ServiceAgreementHeader header, JT_ServiceAgreementDetail detail, JT_ServiceAgreementPMDetail pmDetail)
        {
            _header = header;
            _detail = detail;
            _pmDetail = pmDetail;
        }
    }
}
