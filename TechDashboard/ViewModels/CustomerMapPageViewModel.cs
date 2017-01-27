using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sage.SData.Client;
using TechDashboard.Models;
using TechDashboard.Data;
using TechDashboard.Services;
using System.Xml;

namespace TechDashboard.ViewModels
{
    /*********************************************************************************************************
     * CustomerMapPageViewModel.cs
     * 11/30/2016 DCH Add TODO
     * 12/07/2016 DCH Add error handling
     *********************************************************************************************************/
    public class CustomerMapPageViewModel
    {
        XmlDocument _customerLocation;
       // Xamarin.Forms.Maps.Pin _customerLocationPin;
        public XmlDocument CustomerLocation
        {
            get { return _customerLocation; }
        }

        //AR_Customer _customer;
        //public AR_Customer Customer
        //{
        //    get { return _customer; }
        //}

        // TODO
        public CustomerMapPageViewModel()
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                SetCustomerLocationPin();
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.CustomerMapPageViewModel()");
            }
        }

        public CustomerMapPageViewModel(XmlDocument customerLocation)
        {
            // dch rkl 12/07/2016 catch exception
            try
            {
                _customerLocation = customerLocation;
            }
            catch (Exception ex)
            {
                // dch rkl 12/07/2016 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.CustomerMapPageViewModel(XmlDocument customerLocation)");
            }

        }

        protected  void SetCustomerLocationPin()
        {
            // TODO ??

            //Geocoder geoCoder = new Geocoder();

            //string xamarinAddress = "91 Oak Glen Dr, Pequea, PA 17565";
            ////Task<IEnumerable<Position>> geoCoderTask = (geoCoder.GetPositionsForAddressAsync(xamarinAddress));
            ////geoCoderTask.Wait();

            ////List<Position> approximateLocations = geoCoderTask.Result.ToList();
            ////if (approximateLocations.Count < 1)
            ////{
            ////    return;
            ////}
            

            //_customerLocationPin = new Pin();
            //_customerLocationPin.Position = new Position(39.909630, -76.299061); //approximateLocations[0];
            //_customerLocationPin.Label = "Greg Hripto"; // _customer.CustomerName;
            //_customerLocationPin.Address = xamarinAddress;
            
        }
    }
}
