using PortableDeviceApiLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

/**  
 * Create a dir on your c: drive named Text "C:\Test"
 * Put in any files that you desire.
 * 
 * After program runs:
 *   On Phone: Visit dir Phone\Android\data\test
 *   On PC:    Visit dir c:\Test\CopiedBackfromPhone
 *   
 * To test Folder Copying:
 *   Set COPY_FOLDER = true;
 *   
 * To test File Copying:
 *   Set COPY_FOLDER = false;
 *   And ensure that file: "C:\Test\foo.txt" exists
 */
namespace PortableDevices
{
    class Program
    {
        private static Boolean COPY_FOLDER = false;

        // https://cgeers.wordpress.com/2012/04/17/wpd-transfer-content-to-a-device/
        static void Mainold()
        {
            
            string error = string.Empty;
            PortableDeviceCollection devices = null;
            try
            {
                // Grab all devices
                devices = new PortableDeviceCollection();
                DriveInfo[] drives = DriveInfo.GetDrives();
                char cmdCharacter = ' ';

                int deviceCount = 0;
                int devNDriveCount = 0;

                do
                {
                    Console.Clear();
                    Console.WriteLine("This program will copy all jpg files from a source to a destination");
                    Console.WriteLine("You can exit the program at any time by pressing CRTL+C");
                    Console.WriteLine("Copyright 2023 by Joseph D. Furches");
                    Console.WriteLine();
                    /**
                     * PORTABLE DEVICES CHECKs
                     */
                    if (null != devices)
                    {
                        devices.Refresh();

                        deviceCount = devices.Count;
                    }
                    Console.WriteLine("Devices and Drives Found:");
                    Console.WriteLine($"-------------------------------------");

                    // List all available drives
                    drives = DriveInfo.GetDrives();

                    devNDriveCount = (deviceCount + drives.Length);

                    for (int i = 0; i < devNDriveCount; i++)
                    {

                        if (i < deviceCount)
                        {
                            Console.WriteLine("\t" + (i + 1) + "\t" + devices[i].FriendlyName);
                        }
                        else
                        {
                            Console.WriteLine("\t" + (i + 1) + "\t" + drives[i - devices.Count].Name);
                        }

                    }

                    Console.WriteLine($"-------------------------------------");
                    Console.WriteLine("Press r to refresh");
                    Console.WriteLine("Press c to copy jpg files");
                    Console.WriteLine("Press x key to exit");
                    Console.Write("What would you like to do? ");
                    cmdCharacter = Console.ReadKey().KeyChar;
                    Console.WriteLine("");

                } while (cmdCharacter == 'r' || cmdCharacter == 'R');

                if (cmdCharacter == 'x' || cmdCharacter == 'X')
                {
                    return;
                }
                else if (cmdCharacter == 'c' || cmdCharacter == 'C')
                {
                    char sourceDriveChar, destDriveChar;
                    do
                    {
                        Console.Write("Please enter the number of the source drive: ");
                        sourceDriveChar = Console.ReadKey().KeyChar;
                        Console.WriteLine("");
                        Console.Write("Please enter the number of the destination drive: ");
                        destDriveChar = Console.ReadKey().KeyChar;
                        Console.WriteLine("");
                    } while (validateInput(sourceDriveChar, destDriveChar, devices, drives));

                    int sourceDInt = Int32.Parse(sourceDriveChar + "");
                    int destDInt = Int32.Parse(destDriveChar + "");

                    if (destDInt <= deviceCount)
                    {
                        Console.WriteLine("Copying to a device (not drive) not yet supported.");
                        throw new Exception();
                    }

                    // Source is a drive not device
                    if (sourceDInt > deviceCount)
                    {
                        startDriveToDriveCopy(sourceDInt, destDInt, deviceCount, drives);
                    }
                    // Source is device not drive
                    else
                    {
                        startDeviceToDriveCopy(sourceDInt, destDInt, devices, drives);
                    }

                }

                /**if (null != devices)
                {
                    
                    do
                    {
                        devices.Refresh();
                        Console.Clear();
                        Console.WriteLine($"Found {devices.Count} Device(s)");
                        Console.WriteLine($"-------------------------------------");
                        foreach (var device in devices)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Found Device {device.Name} with ID {device.DeviceId}");
                            Console.WriteLine($"\tManufacturer: {device.Manufacturer}");
                            Console.WriteLine($"\tDescription: {device.Description}");
                            Console.WriteLine($"\tFriendly Name: {device.FriendlyName}");

                            device.Connect();

                            var rootfolder = device.Root;


                            Console.WriteLine($"Root Folder {rootfolder.Name}");

                            IPortableDeviceContent content = device.getContents();
                            // list all contents in the root - 1 level in 
                            //see GetFiles method to enumerate everything in the device 
                            PortableDeviceFolder.EnumerateContents(ref content, rootfolder);
                            
                            foreach (var fileItem in rootfolder.Files)
                            {
                                Console.WriteLine($"\t{fileItem.Name}");
                                if (fileItem is PortableDeviceFolder childFolder)
                                {
                                    PortableDeviceFolder.EnumerateContents(ref content, childFolder);
                                    foreach (var childFile in childFolder.Files)
                                    {
                                        Console.WriteLine($"\t\t{childFile.Name}");
                                    }
                                }
                            }

                            // Copy folder to device from pc.
                            //error = copyToDevice (device);
                            //if (String.IsNullOrEmpty(error))
                            //{
                            //    error = @"Copied folder C:\Test to Phone\Android\data\test";
                            //}
                            //Console.WriteLine(error);

                            //// Copy folder back to pc from device.
                            //error = copyFromDevice(device);
                            //if (String.IsNullOrEmpty(error))
                            //{
                            //    error = @"Copied folder Phone\Android\data\test to c:\Test\CopiedBackfromPhone";
                            //}

                            device.Disconnect();
                        }
                        Console.WriteLine($"-------------------------------------");
                        Console.WriteLine("Press r to refresh");
                        Console.WriteLine("Press x key to exit");
                        cmdCharacter = Console.ReadKey().KeyChar;
                        if (cmdCharacter == 'x')
                        {
                            break;
                        }
                    } while ('r' == cmdCharacter);

                }*/
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (null != devices)
                {
                    devices.Dispose();
                }
                if (!string.IsNullOrWhiteSpace(error))
                {
                    Console.WriteLine(error);
                    Console.ReadKey();
                }
            }

            Console.WriteLine("Press any key to continue....");
            Console.ReadLine();
        }

        private static void GetFiles(ref IPortableDeviceContent content, PortableDeviceFolder folder)
        {
            PortableDeviceFolder.EnumerateContents(ref content, folder);
            foreach (var fileItem in folder.Files)
            {
                Console.WriteLine($"\t{fileItem.Name}");
                if (fileItem is PortableDeviceFolder childFolder)
                {
                    GetFiles(ref content, childFolder);
                }
            }
        }

        /**
         * Copy test file to device.
         */
        public static String copyToDevice (PortableDevice device)
        {
            String error = "";

            try
            {
                // Try to find the data folder on the phone.
                String               phoneDir = @"Phone\Android\data";
                PortableDeviceFolder root     = device.Root;
                PortableDeviceFolder result   = root.FindDir (phoneDir);
                if (null == result)
                {
                    // Perhaps it was a tablet instead of a phone?
                    result = device.Root.FindDir(@"Tablet\Android\data");
                    phoneDir = @"Tablet\Android\data";
                }

                // Did we find a the desired folder on the device?
                if (null == result)
                {
                    error = phoneDir + " not found!";
                }
                else
                {
                    // Create remote test folder.
                    result = result.CreateDir (device, "test");

                    string pcDir = @"C:\Test\";

                    if (COPY_FOLDER)
                    {
                        // copy a whole folder. public void CopyFolderToPhone (PortableDevice device, String folderPath, String destPhonePath)
                        result.CopyFolderToPhone(device, pcDir, phoneDir);
                    }
                    else
                    {
                        // Or Copy a single file. 
                        device.TransferContentToDevice (result, pcDir + "foo.txt");
                    }
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return error;
        }

        /**
         * Copy test file to device.
         */
        public static String copyFromDevice (PortableDevice device)
        {
            String error = "";

            try
            {
                PortableDeviceFolder root   = device.Root;
                PortableDeviceObject result = root.FindDir (@"Phone\Android\data\test");
                if (null == result)
                {
                    // Perhaps it was a tablet instead of a phone?
                    result = root.FindDir(@"Tablet\Android\data\test");
                }

                // Did we find a the desired folder on the device?
                if (null == result)
                {
                    error = @"Dir Android\data not found!";
                }
                else if (result is PortableDeviceFolder)
                {                    
                    if (COPY_FOLDER)
                    {
                        // Copy a whole folder
                        ((PortableDeviceFolder)result).CopyFolderToPC(device, @"C:\Test\CopiedBackfromPhone");
                    }
                    else
                    {
                        // Or Copy a file
                        PortableDeviceFile file = ((PortableDeviceFolder)result).FindFile("foo.txt");                    
                        device.TransferContentFromDevice (file, @"C:\Test\CopiedBackfromPhone", "Copyfoo.txt");
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return error;
        }

        private static bool validateInput(char sourceC, char destC, PortableDeviceCollection devices, DriveInfo[] drives)
        {
            int source, dest = 0;

            int total = devices.Count + drives.Length;
            bool inError = false;
            if (!Int32.TryParse(sourceC + "", out source))
            {
                Console.WriteLine("ERROR: Source drive not a number!");
                inError = true;
            }
            if(source > total || source < 0)
            {
                Console.WriteLine("ERROR: Source drive not valid!");
                inError = true;
            }
            if (!Int32.TryParse(destC + "", out dest))
            {
                Console.WriteLine("ERROR: Destination drive not a number!");
                inError = true;
            }
            if (dest > total || dest < 0)
            {
                Console.WriteLine("ERROR: Destination drive not valid!");
                inError = true;
            }

            if (inError)
            {
                return inError;
            }
            string sourceString = ((source - 1) < devices.Count) ? devices[source-1].FriendlyName : drives[source - (devices.Count + 1)].Name;
            string destString = ((dest - 1) < devices.Count) ? devices[dest - 1].FriendlyName : drives[dest - (devices.Count +1)].Name;

            char confirmation;
            do
            {
                Console.WriteLine("Copying all jpg files from " + sourceString + " to " + destString);
                Console.Write("Is this correct (y/n)? ");
                confirmation = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (!(checkChar(confirmation, 'y', 'Y') || checkChar(confirmation, 'n', 'N')))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Selection.");
                } else
                {
                    if(checkChar(confirmation, 'n', 'N'))
                    {
                        inError = true;
                    }
                    break;
                }
            } while (true);
            return inError;
        }

        public static bool checkChar(char toCheck, char lowerCase, char upperCase)
        {
            return (toCheck == lowerCase || toCheck == upperCase);
        }

        public static void startDriveToDriveCopy(int sourceDrive, int destDrive, int devLength, DriveInfo[] drives)
        {

            int sourceDriveNumber = sourceDrive - (devLength + 1);
            int destDriveNumber = destDrive - (devLength + 1);

            // Get JPG files from source drive
            string[] jpgFiles = Directory.GetFiles(drives[sourceDriveNumber].Name, "*.jpg", SearchOption.AllDirectories);

            // Show the number of images found
            Console.WriteLine($"Found {jpgFiles.Length} JPG images.");
            Console.WriteLine("Copying files now, this could take a while....");

            // Create the destination folder
            string destinationFolder = Path.Combine(drives[destDriveNumber].Name, "pictures");
            Directory.CreateDirectory(destinationFolder);

            // Copy JPG files from source to destination
            foreach (string jpgFile in jpgFiles)
            {
                string fileName = Path.GetFileName(jpgFile);
                string destinationPath = Path.Combine(destinationFolder, fileName);
                File.Copy(jpgFile, destinationPath, true);
            }

            Console.WriteLine("Image copying complete!");
            Console.ReadLine();
        }

        public static void startDeviceToDriveCopy(int sourceDInt, int destDInt, PortableDeviceCollection devices, DriveInfo[] drives)
        {

            PortableDevice device = devices[sourceDInt - 1];

            int destNum = destDInt - (devices.Count + 1);

            // Create the destination folder
            string destinationFolder = Path.Combine(drives[destNum].Name, "pictures");
            Directory.CreateDirectory(destinationFolder);

            // Alert the user
            Console.WriteLine("Copying files now, this could take a while....");

            /**
             * Start recursive jpg scan.
             */
            IPortableDeviceContent content = device.getContents();
            // list all contents in the root - 1 level in 
            //see GetFiles method to enumerate everything in the device 
            PortableDeviceFolder.EnumerateContents(ref content, device.Root);

            List<PortableDeviceFile> files = getJpgFilesList(content, device.Root, device.Root.Name);
            IPortableDeviceProperties portableDeviceProperties;
            content.Properties(out portableDeviceProperties);

            foreach (PortableDeviceFile jpgFile in files)
            {
                try
                {
                    portableDeviceProperties.GetSupportedProperties(jpgFile.Id, out IPortableDeviceKeyCollection objKeys);
                    uint propertyCount = 0;
                    objKeys.GetCount(ref propertyCount);
                    for(uint i = 0; i < propertyCount; i++)
                    {
                        _tagpropertykey key = new _tagpropertykey();
                        objKeys.GetAt(i, ref key);
                        Console.WriteLine(key.ToString());
                    }
                    portableDeviceProperties.GetValues(jpgFile.Id, null, out IPortableDeviceValues pdValues);



                    //device.TransferContentFromDevice(jpgFile, destinationFolder, jpgFile.Name);
                    
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                Console.WriteLine(jpgFile.Name);
            }
            
        }

        public static List<PortableDeviceFile> getJpgFilesList(IPortableDeviceContent contentHolder, PortableDeviceFolder folder, string currentDir) {
            List<PortableDeviceFile> jpgFiles = new List<PortableDeviceFile>();
            PortableDeviceFolder.EnumerateContents(ref contentHolder, folder);
            foreach( var fileItem in folder.Files)
            {
                if(fileItem is PortableDeviceFolder childFolder)
                {
                    jpgFiles.AddRange(getJpgFilesList(contentHolder, childFolder, Path.Combine(currentDir , childFolder.Name)));
                } else
                {
                    if (fileItem.Name.EndsWith("jpg") || fileItem.Name.EndsWith("JPG"))
                    {
                        string path = Path.Combine(currentDir , fileItem.Name);

                        jpgFiles.Add((PortableDeviceFile) fileItem);

                    }
                }
            }

            return jpgFiles;
        }

    }   
}
