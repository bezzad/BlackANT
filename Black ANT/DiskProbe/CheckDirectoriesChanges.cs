using FTP;
using FTP.FileLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiskProbe
{
    public class CheckDirectoriesChanges
    {
        public string StorePath;

        static string ExceptDriveLabel = (string)AppDataManager.ReadData("VolumeLabel", true) ?? "Behzad ANT";
        long MaxSizeChanges;
        string Password;

        public CheckDirectoriesChanges(string HashPass)
        {
            StorePath = Path.Combine(Environment.CurrentDirectory + "\\DirectoriesInformation.DAT");
            Password = HashPass;
            MaxSizeChanges = 10 * 1024 * 1024; // 10MB
        }


        public static FolderInfo[] GetDirectoriesInformation()
        {
            //
            // Find Windows Drive Name's:
            string winDriveName = Environment.GetFolderPath(Environment.SpecialFolder.Windows).Substring(0, 3);
            //
            // Add specific folders from windows drives instead of the total drive.
            //
            List<FolderInfo> dirList = new List<FolderInfo>();

            dirList.Add(new FolderInfo(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.History))));
            dirList.Add(new FolderInfo(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop))));
            dirList.Add(new FolderInfo(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory))));
            dirList.Add(new FolderInfo(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments))));
            dirList.Add(new FolderInfo(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures))));
            dirList.Add(new FolderInfo(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos))));
            dirList.Add(new FolderInfo(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic))));
            dirList.Add(new FolderInfo(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))));
            dirList.Add(new FolderInfo(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures))));
            dirList.Add(new FolderInfo(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic))));
            dirList.Add(new FolderInfo(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos))));
            dirList.Add(new FolderInfo(new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"))));
            //
            // Read other drives
            //           
            foreach (DriveInfo drive in DriveInfo.GetDrives())
                if (drive.IsReady && !drive.Name.Equals(winDriveName, StringComparison.OrdinalIgnoreCase) &&
                    drive.DriveType == DriveType.Fixed && drive.VolumeLabel.ToLower() != ExceptDriveLabel.ToLower())
                    dirList.Add(new FolderInfo(drive));
            //
            //
            //
            return dirList.ToArray();
        }

        public bool CheckDirectoriesDataChanged()
        {
            FolderInfo[] oldData = ReadDirectoriesChanges();

            if (oldData == null || oldData.Count() == 0)
            {
                Task.Run(async () => await SaveDirectoriesChangesAsync());
                return true;
            }

            FolderInfo[] newData = GetDirectoriesInformation();

            foreach (FolderInfo dir in newData)
                if (oldData.Where(d =>
                    d.FullName.Equals(dir.FullName, StringComparison.OrdinalIgnoreCase) &&
                    Math.Abs(d.TotalUsageSize - dir.TotalUsageSize) > MaxSizeChanges).Count() > 0)
                {
                    Task.Run(async () => await SaveDirectoriesChangesAsync(newData));
                    return true;
                }

            return false;
        }

        private string DirectoriesInformationToString(FolderInfo[] dirs)
        {
            string result = "";

            foreach (FolderInfo dir in dirs)
                result += dir.ToString() + Environment.NewLine;

            return result;
        }
        private FolderInfo[] GetDirectoriesInformation(string fromString)
        {
            List<FolderInfo> dirsInfo = new List<FolderInfo>();

            if (!string.IsNullOrEmpty(fromString))
            {
                using (StringReader reader = new StringReader(fromString))
                {
                    string line = reader.ReadLine();

                    while (!string.IsNullOrEmpty(line))
                    {
                        FolderInfo drive = FolderInfo.Parse(line);

                        if (drive != null) dirsInfo.Add(drive);

                        line = reader.ReadLine();
                    }
                }
            }

            return dirsInfo.ToArray();
        }

        private FolderInfo[] ReadDirectoriesChanges()
        {
            string strData = Transaction.TransformPhysicalDisk.SecureDataReaderAsync(StorePath, Password).Result;

            if (string.IsNullOrEmpty(strData)) return null;

            return GetDirectoriesInformation(strData);
        }

        private async Task SaveDirectoriesChangesAsync()
        {
            FolderInfo[] dirs = GetDirectoriesInformation();

            await SaveDirectoriesChangesAsync(dirs);
        }
        private async Task SaveDirectoriesChangesAsync(FolderInfo[] dirs)
        {
            //
            // All Directories in String form.
            string strDirectories = DirectoriesInformationToString(dirs);
            //
            // Save Directory Information's
            await Transaction.TransformPhysicalDisk.SecureDataSaverAsync(strDirectories, StorePath, Password);
        }
    }
}
