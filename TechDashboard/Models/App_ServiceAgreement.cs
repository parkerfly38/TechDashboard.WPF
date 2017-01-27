using System;
using System.Collections.Generic;
using System.Text;

namespace TechDashboard.Models
{
    /*********************************************************************************************************
     * App_ServiceAgreement.cs
     * 12/02/2016 DCH Add LaborCovered, IsPMLaborCovered, DetailRate, StandardLaborRate
     *********************************************************************************************************/
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
                return ((PmDetail != null) &&
                        (PmDetail.PartsCovered != null) &&
                        (PmDetail.PartsCovered.Trim().ToUpper() == "Y"));
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

        //  dch rkl 12/01/2016 Add LaborCovered
        public bool IsLaborCovered
        {
            get
            {
                return ((_detail != null) &&
                        (_detail.LaborCovered != null) &&
                        (_detail.LaborCovered.Trim().ToUpper() == "Y"));
            }
        }

        //  dch rkl 12/01/2016 Add PM LaborCovered
        public bool IsPMLaborCovered
        {
            get
            {
                return ((_pmDetail != null) &&
                        (_pmDetail.LaborCovered != null) &&
                        (_pmDetail.LaborCovered.Trim().ToUpper() == "Y"));
            }
        }

        //  dch rkl 12/02/2016 Add Detail Rate
        public decimal DetailRate
        {
            get
            {
                if (_detail != null)
                {
                    return _detail.Rate;
                }
                else
                {
                    return 0;
                }
            }
        }

        //  dch rkl 12/02/2016 Add PM LaborCovered
        public decimal StandardLaborRate
        {
            get
            {
                if (_header != null)
                {
                    return _header.StandardLaborRate;
                }
                else
                {
                    return 0;
                }
            }
        }

        // dch rkl 11/03/2016 make this available as a propertly
        public JT_ServiceAgreementPMDetail PmDetail
        {
            get { return _pmDetail; }
        }

        public App_ServiceAgreement(JT_ServiceAgreementHeader header, JT_ServiceAgreementDetail detail, JT_ServiceAgreementPMDetail pmDetail)
        {
            _header = header;
            _detail = detail;
            _pmDetail = pmDetail;
        }
    }
}
