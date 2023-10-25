using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PortableDeviceApiLib;
using PortableDevices;

namespace FileTransferWindowApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            String? error = null;
            PortableDeviceCollection? devices = null;
            MainWindow window = new MainWindow();
            try
            {

                //DriveInfo[] drives = DriveInfo.GetDrives();
                char cmdCharacter = ' ';

                int deviceCount = 0;
                int devNDriveCount = 0;

                window.refreshDeviceList();


                DatePicker startDate = (DatePicker)window.FindName("StartDate");
                startDate.BlackoutDates.Add(new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue));

                DatePicker endDate = (DatePicker)window.FindName("EndDate");
                endDate.BlackoutDates.Add(new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue));

            }
            catch (Exception ex)
            {
                error = ex.Message;
                MessageBox.Show(error, "Error occurred", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (null != devices)
                {
                    devices.Dispose();
                }
            }


            window.Show();
        }



        
    }
}
