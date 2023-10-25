using PortableDeviceApiLib;
using PortableDevices;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace FileTransferWindowApplication
{

    public class ImageImportData
    {

        public String StartDate { get; set; }
        public String EndDate { get; set; }

        public String TripName { get; set; }

        public PortableDevices.PortableDevice PortableDevice { get; set; }

    }

    public class ImageExportData
    {

        public string TripName { get; set; }

        public string DriveName { get; set; }

    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public List<String> Devices { get; set;}

        public List<string> Drives { get; set;}

        public string SelectedDrive { get; set; }

        public List<string> Trips { get; set;}

        public string SelectedTrip { get; set; }

        public PortableDeviceCollection devs { get; set; }

        public MainWindow()
        {
            Devices = new List<string>();
            Drives = new List<string>();
            Trips = new List<string>();
            InitializeComponent();
            this.DataContext = this;
        }

        public void RefreshButton(object sender, RoutedEventArgs e)
        {
            refreshDeviceList();
        }

        public void refreshDeviceList()
        {
            devs = new PortableDeviceCollection();
            devs.Refresh();

            Devices.Clear();
            if (devs.Count == 0)
            {
                ((StackPanel)App.Current.MainWindow.FindName("NoDevDisplay")).Visibility = Visibility.Visible;
                ((StackPanel)App.Current.MainWindow.FindName("DeviceDisplay")).Visibility = Visibility.Collapsed;
                ((StackPanel)App.Current.MainWindow.FindName("TMDevDisplay")).Visibility = Visibility.Collapsed;
            }
            else if (devs.Count == 1)
            {
                ((StackPanel)App.Current.MainWindow.FindName("NoDevDisplay")).Visibility = Visibility.Collapsed;
                ((StackPanel)App.Current.MainWindow.FindName("DeviceDisplay")).Visibility = Visibility.Visible;
                ((StackPanel)App.Current.MainWindow.FindName("TMDevDisplay")).Visibility = Visibility.Collapsed;
            }
            else if (devs.Count > 1)
            {
                ((StackPanel)App.Current.MainWindow.FindName("NoDevDisplay")).Visibility = Visibility.Collapsed;
                ((StackPanel)App.Current.MainWindow.FindName("DeviceDisplay")).Visibility = Visibility.Collapsed;
                ((StackPanel)App.Current.MainWindow.FindName("TMDevDisplay")).Visibility = Visibility.Visible;
            }
        }

        public void RefreshDriveList(object sender, EventArgs e)
        {
            
            DriveInfo[] drvs = DriveInfo.GetDrives();
            Drives.Clear();
            foreach (DriveInfo drive in drvs)
            {
                if (!drive.Name.Contains("C"))
                {
                    Drives.Add(drive.Name);
                }
            }

            DriveDD.ItemsSource = Drives;
        }


        public void TabChanged(object sender, SelectionChangedEventArgs e)
        {
            string temp = System.IO.Path.DirectorySeparatorChar + "ImportedPhotos";
            // TODO check for trip already exists.
            string destinationFolder = System.IO.Path.Combine(GetUserHome(), temp);

            IEnumerable<string> directoriesForTrips = Directory.EnumerateDirectories(destinationFolder);

            Trips.Clear();

            foreach(string trip in directoriesForTrips)
            {
                Trips.Add(System.IO.Path.GetFileName(trip));
            }

            TripsDD.ItemsSource = Trips;
            RefreshDriveList(null, null);
        }

        public void ExportPhotos_Click(object sender, RoutedEventArgs e)
        {
            // TODO
            // Validate that a drive is selected
            if(SelectedDrive == null || SelectedDrive.Equals(""))
            {
                MessageBox.Show("Must select a USB drive to write files to.", "No USB Selected", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Validate that a source is selected
            if (SelectedTrip == null || SelectedTrip.Equals(""))
            {
                MessageBox.Show("Must select a trip to write to the USB drive.", "No Trip Selected", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ExProgressSection.Visibility = Visibility.Visible;
            epbutton.Visibility = Visibility.Collapsed;

            ImageExportData imageExportData = new ImageExportData();
            imageExportData.TripName = SelectedTrip;
            imageExportData.DriveName = SelectedDrive;


            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += export_worker_doWork;
            worker.ProgressChanged += export_worker_ProgressChanged;
            worker.RunWorkerCompleted += export_worker_RunWorkerCompleted;
            worker.RunWorkerAsync(argument: imageExportData);

        }

        public void ImportPhotos_Click(object sender, RoutedEventArgs e)
        {

            // TODO (Validate the Date boxes are set)
            if (!validDateBoxes())
            {
                
                return;
            }

            // TODO (Validate the Device is set)
            if(devs.Count != 1 )
            {
                MessageBox.Show("Didn't find a device to pull photos from.", "No Device Found", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(TripName.Text == null || TripName.Text.Length == 0)
            {
                MessageBox.Show("Must provide a trip name.", "No Trip Name", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            pbText.Text = "Scanning for Photos...";
            ProgressSection.Visibility = Visibility.Visible;
            ipbutton.Visibility = Visibility.Collapsed;

            if(devs.Count == 1)
            {
                processDevicePhotos(devs[0]);
            }

        }

        private bool validDateBoxes()
        {
            if (StartDate.Text.Length == 0 || EndDate.Text.Length == 0)
            {
                MessageBox.Show("Must select a start and end date!", "Invalid Dates", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (DateTime.Parse(StartDate.Text).CompareTo(DateTime.Parse(EndDate.Text))>0)
            {
                MessageBox.Show("Must select an end date after the start date!", "Invalid Dates", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
            
        }

        private void processDevicePhotos(PortableDevices.PortableDevice device)
        {
            ImageImportData imageImportData = new ImageImportData();
            imageImportData.PortableDevice = device;
            imageImportData.StartDate = StartDate.Text;
            imageImportData.EndDate = EndDate.Text;
            imageImportData.TripName = TripName.Text;


            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_doWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync(argument: imageImportData);
            
        }

        // TODO DriveInfo[] drives = DriveInfo.GetDrives();
        private void worker_doWork(object sender, DoWorkEventArgs e)
        {
            ImageImportData imageImportData = e.Argument as ImageImportData;

            PortableDevices.PortableDevice device = imageImportData.PortableDevice;

            // Validate and fetch the current device.
            if(device == null)
            {
                MessageBox.Show("Unable to connect to device " + device.Name, "Error connecting to device", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string temp = System.IO.Path.DirectorySeparatorChar + "ImportedPhotos" + 
                System.IO.Path.DirectorySeparatorChar + imageImportData.TripName;
            // TODO check for trip already exists.
            string destinationFolder = System.IO.Path.Combine(GetUserHome(), temp );

            IPortableDeviceContent content = device.getContents();
            // list all contents in the root - 1 level in 
            //see GetFiles method to enumerate everything in the device 
            PortableDeviceFolder.EnumerateContents(ref content, device.Root);

            List<PortableDeviceFile> files = getJpgFilesList(content, device.Root, device.Root.Name, 
                DateTime.Parse(imageImportData.StartDate), DateTime.Parse(imageImportData.EndDate));

            (sender as BackgroundWorker).ReportProgress(0, files.Count);
            int i = 0;
            foreach (PortableDeviceFile jpgFile in files)
            {
                device.TransferContentFromDevice(jpgFile, destinationFolder, jpgFile.Name);
                (sender as BackgroundWorker).ReportProgress(i);
                i++;
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(e.UserState != null)
            {
                pbStatus.IsIndeterminate = false;
                pbStatus.Maximum = (int)e.UserState;
                pbStatus.Value = 0;
            }

            pbText.Text = String.Format("Transferring {0:P1} complete.", (e.ProgressPercentage/pbStatus.Maximum));
            pbStatus.Value = e.ProgressPercentage;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string temp = System.IO.Path.DirectorySeparatorChar + "ImportedPhotos" +
                System.IO.Path.DirectorySeparatorChar + TripName.Text;
            // TODO check for trip already exists.
            string destinationFolder = System.IO.Path.Combine(GetUserHome(), temp);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = destinationFolder,
                FileName = "explorer.exe"
            };

            Process.Start(startInfo);

            pbStatus.IsIndeterminate = true;
            ProgressSection.Visibility = Visibility.Collapsed;
            ipbutton.Visibility = Visibility.Visible;
        }


        public static List<PortableDeviceFile> getJpgFilesList(IPortableDeviceContent contentHolder,
            PortableDeviceFolder folder, 
            string currentDir, 
            DateTime StartDate,
            DateTime EndDate)
        {
            List<PortableDeviceFile> jpgFiles = new List<PortableDeviceFile>();
            try
            {
                contentHolder.Properties(out IPortableDeviceProperties portableDeviceProperties);
                PortableDeviceFolder.EnumerateContents(ref contentHolder, folder);
                foreach (var fileItem in folder.Files)
                {
                    if (fileItem is PortableDeviceFolder childFolder)
                    {
                        jpgFiles.AddRange(getJpgFilesList(contentHolder, childFolder, System.IO.Path.Combine(currentDir, childFolder.Name), StartDate, EndDate));
                    }
                    else
                    {
                        if (fileItem.Name.ToLower().EndsWith("jpg"))
                        {
                            if (fileInDateRange(fileItem, StartDate, EndDate, portableDeviceProperties))
                            {
                                string path = System.IO.Path.Combine(currentDir, fileItem.Name);

                                jpgFiles.Add((PortableDeviceFile)fileItem);
                            }
                        }
                    }
                }
            } catch(Exception ex)
            {
                // NO-OP
            }
            return jpgFiles;
        }

        static bool fileInDateRange(PortableDeviceObject file, DateTime start, DateTime end, IPortableDeviceProperties portableDeviceProperties)
        {
            portableDeviceProperties.GetSupportedProperties(file.Id, out PortableDeviceApiLib.IPortableDeviceKeyCollection objKeys);

            portableDeviceProperties.GetValues(file.Id, objKeys, out PortableDeviceApiLib.IPortableDeviceValues pdValues);

            PortableDeviceApiLib._tagpropertykey WPD_OBJECT_MODIFIED_DATE = new PortableDeviceApiLib._tagpropertykey();
            WPD_OBJECT_MODIFIED_DATE.fmtid = Guid.Parse("ef6b490d-5cd8-437a-affc-da8b60ee4a3c");
            WPD_OBJECT_MODIFIED_DATE.pid = 19;

            pdValues.GetStringValue(WPD_OBJECT_MODIFIED_DATE, out string value);
            DateTime dateOfFile = DateTime.Parse(value.Substring(0,value.IndexOf(':')));

            if (start.AddDays(-1).CompareTo(dateOfFile) < 0 && end.AddDays(1).CompareTo(dateOfFile) > 0) {
                return true;
            }
            return false;
        }

        static string GetUserHome()
        {
            var homeDrive = Environment.GetEnvironmentVariable("HOMEDRIVE");
            if (!string.IsNullOrWhiteSpace(homeDrive))
            {
                var homePath = Environment.GetEnvironmentVariable("HOMEPATH");
                if (!string.IsNullOrWhiteSpace(homePath))
                {
                    var fullHomePath = homeDrive + System.IO.Path.DirectorySeparatorChar + homePath;
                    return System.IO.Path.Combine(fullHomePath, "myFolder");
                }
                else
                {
                    throw new Exception("Environment variable error, there is no 'HOMEPATH'");
                }
            }
            else
            {
                throw new Exception("Environment variable error, there is no 'HOMEDRIVE'");
            }
        }

        void export_worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                exPbStatus.IsIndeterminate = false;
                exPbStatus.Maximum = (int)e.UserState;
                exPbStatus.Value = 0;
            }

            exPbText.Text = String.Format("Transferring {0:P1} complete.", (e.ProgressPercentage / exPbStatus.Maximum));
            exPbStatus.Value = e.ProgressPercentage;
        }

        void export_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            exPbStatus.IsIndeterminate = true;
            ExProgressSection.Visibility = Visibility.Collapsed;
            epbutton.Visibility = Visibility.Visible;

            MessageBox.Show("Successfully exported all images.", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void export_worker_doWork(object sender, DoWorkEventArgs e)
        {
            ImageExportData imageExportData = e.Argument as ImageExportData;

            string destinationDrive = imageExportData.DriveName;

            string temp = System.IO.Path.DirectorySeparatorChar + "ImportedPhotos" +
                System.IO.Path.DirectorySeparatorChar + imageExportData.TripName;
            // TODO check for trip already exists.
            string sourceFolder = System.IO.Path.Combine(GetUserHome(), temp);

            string[] jpgFiles = Directory.GetFiles(sourceFolder, "*.jpg", SearchOption.AllDirectories);

            string destinationFolder = System.IO.Path.Combine(destinationDrive, "FunCountryTours");

            Directory.CreateDirectory(destinationFolder);

            (sender as BackgroundWorker).ReportProgress(0, jpgFiles.Length);

            int i = 0;
            foreach (string jpgFile in jpgFiles)
            {
                string fileName = System.IO.Path.GetFileName(jpgFile);
                string destinationPath = System.IO.Path.Combine(destinationFolder, fileName);
                File.Copy(jpgFile, destinationPath, true);
                i++;
                (sender as BackgroundWorker).ReportProgress(i);
            }
        }
    }
}
