using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Ionic.Zip;
using System.IO;
using System.Threading;
using EventArguments;
using Security;

namespace FTP.Zipper
{
    /// <summary>
    /// ZipEncryption Class jobs is Zipping or Extract some files and directories into the target directory...
    /// This class always encrypt all file names by zip file keys.
    /// </summary>
    public class ZipEncryption
    {
        #region Fields

        protected string Password;
        protected string TempFolderPath;
        protected FileAttributes SecureAttributes;
        protected FileAttributes NormalAttributes;
        protected long totalFilesSize = 0;
        protected double totalTransferredPercentForAllEntry = 0;
        protected double totalTransferredPercentForCurrentEntry = 0;
        public long TotalFilesSizeBytes
        {
            get { return totalFilesSize; }
        }
        public double TotalTransferredPercentForAllEntry
        {
            get { return totalTransferredPercentForAllEntry; }
        }
        public double TotalTransferredPercentForCurrentEntry
        {
            get { return totalTransferredPercentForCurrentEntry; }
        }

        #endregion

        #region Events

        public event EventHandler<ReportEventArgs> ReportOccurrence = delegate { };
        public event EventHandler<ExtractProgressEventArgs> ExtractProgress = delegate { };
        public event EventHandler<SaveProgressEventArgs> SaveProgress = delegate { };
        public event EventHandler<AddProgressEventArgs> AddProgress = delegate { };
        public event EventHandler<ReadProgressEventArgs> ReadProgress = delegate { };

        #endregion

        #region Class Constructors
        /// <summary>
        /// ZipEncryption Constructor
        /// </summary>
        /// <param name="PasswordKey">Set Public Password key's for extract or zipping files.</param>
        public ZipEncryption(string TempFolder, string PasswordKey)
        {
            this.SetPassword(PasswordKey);

            SecureAttributes = System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System |
                   System.IO.FileAttributes.Encrypted | System.IO.FileAttributes.Compressed;

            NormalAttributes = System.IO.FileAttributes.Archive | System.IO.FileAttributes.Normal;

            this.TempFolderPath = Directory.Exists(TempFolder) ? TempFolder : Environment.CurrentDirectory;
        }
        #endregion


        private void zip_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            #region Calculate process percentile ratio for all transferred bytes
            try
            {
                if (e.TotalBytesToTransfer > 0)
                {
                    totalTransferredPercentForCurrentEntry = e.BytesTransferred * 100 / e.TotalBytesToTransfer;

                    if (e.TotalBytesToTransfer == e.BytesTransferred) // in this condition that going to next file entry
                    {
                        totalTransferredPercentForAllEntry += ((double)e.TotalBytesToTransfer * 100) / (double)this.TotalFilesSizeBytes;

                        if (totalTransferredPercentForAllEntry == 100)
                            this.ReportOccurrence(this, new ReportEventArgs("ZipEncryption", 
                                ReportCodes.ExtractCompleted,
                                "Extract Operate for {0} has been completed.", e.ArchiveName));
                    }
                }
            }
            catch { }
            #endregion
        }

        private void zip_SaveProgress(object sender, SaveProgressEventArgs e)
        {
            #region Calculate process percentile ratio for all transferred bytes
            try
            {
                if (e.TotalBytesToTransfer > 0)
                {
                    totalTransferredPercentForCurrentEntry = e.BytesTransferred * 100 / e.TotalBytesToTransfer;
                }
                if (e.EntriesSaved > 0 && e.EntriesTotal > 0)
                {
                    totalTransferredPercentForAllEntry = ((double)e.EntriesSaved * 100) / (double)e.EntriesTotal;
                }
                if (e.EventType == Ionic.Zip.ZipProgressEventType.Saving_Completed)
                {
                    this.ReportOccurrence(this, new ReportEventArgs("ZipEncryption", ReportCodes.Zipped,
                        "Zip Operate for {0} has been Completed.", e.ArchiveName));
                }
            }
            catch (Exception ex)
            {
                this.ReportOccurrence(this, new ReportEventArgs("ZipEncryption.zip_SaveProgress", ex));
            }
            #endregion
        }

        protected ZipFile EncryptedZipFile()
        {
            ZipFile zip = new ZipFile();

            zip = EncryptedZipFile(zip);

            zip.Comment = String.Format("This zip archive was created by the {1} application from:{0}    Machine:  '{2}'{0}    UserName: '{3}'{0}{0}",
                    Environment.NewLine, Application.ProductName, System.Net.Dns.GetHostName(), Environment.UserName);

            return zip;
        }
        protected ZipFile EncryptedZipFile(ZipFile zip)
        {
            totalTransferredPercentForAllEntry = 0;

            zip.TempFileFolder = this.TempFolderPath;
            zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
            zip.CompressionMethod = CompressionMethod.None;
            zip.Strategy = Ionic.Zlib.CompressionStrategy.Default;
            zip.AlternateEncodingUsage = ZipOption.Never;
            zip.AlternateEncoding = Encoding.Unicode;
            zip.BufferSize = 64 * 1024; // default = 32768 = 32 * 1024
            zip.UseZip64WhenSaving = (Environment.Is64BitOperatingSystem & Environment.Is64BitProcess) ? Zip64Option.Always : Zip64Option.Never;
            zip.Encryption = (string.IsNullOrEmpty(this.Password)) ? EncryptionAlgorithm.None : EncryptionAlgorithm.PkzipWeak;

            if (!string.IsNullOrEmpty(this.Password)) zip.Password = this.Password;

            // Send Zip class events to other class...
            zip.SaveProgress += zip_SaveProgress;
            zip.SaveProgress += (source, e) => this.SaveProgress(source, e);

            zip.ExtractProgress += zip_ExtractProgress;
            zip.ExtractProgress += (source, e) => this.ExtractProgress(source, e);

            zip.AddProgress += (source, e) => this.AddProgress(source, e);
            zip.ReadProgress += (source, e) => this.ReadProgress(source, e);
            zip.ZipError += (source, e) => this.ReportOccurrence(source, new ReportEventArgs("ZipEncryption.ZipError", e.Exception));

            return zip;
        }

        /// <summary>
        /// Set Public Password key's for extract or zipping files
        /// </summary>
        /// <param name="PasswordKey">Password</param>
        public void SetPassword(string PasswordKey)
        {
            this.Password = PasswordKey;
        }

        /// <summary>
        /// Zip a folder to the target folder path's
        /// </summary>
        /// <param name="sourceFolder">A directory to Zip</param>
        /// <param name="targetPath">A file name path to zip on it.</param>
        public void ZipDirectory(string sourceFolder, string targetPath, CancellationTokenSource CTS)
        {
            if (CTS.IsCancellationRequested)
            {
                ReportOccurrence(this, new ReportEventArgs("ZipEncryption.ZipDirectory", ReportCodes.CancelRequested, "Cancellation Requested"));
                return;
            }
            try
            {
                using (ZipFile zip = EncryptedZipFile())
                {
                    zip.AddDirectory(sourceFolder); // recourses subdirectories
                    zip.Save(targetPath);
                }
            }
            catch (Exception ex) { ReportOccurrence(this, new ReportEventArgs("ZipEncryption.ZipDirectory", ex)); }
        }

        /// <summary>
        /// Zip a folder to the target folder path's
        /// </summary>
        /// <param name="sourceFiles_NewNames">Source files to zip</param>
        /// <param name="toPath">A file name path to zip on it.</param>
        public Boolean ZipFiles(List<FileInfo> sourceFiles, string toPath, System.Threading.CancellationTokenSource CTS)
        {
            try
            {
                if (CTS.IsCancellationRequested)
                {
                    ReportOccurrence(this, new ReportEventArgs("ZipEncryption.ZipFiles", ReportCodes.CancelRequested, "Cancellation Requested"));
                    return false;
                }
                //
                // Calculate Total files size...
                this.totalFilesSize = sourceFiles.Select(x => x.Length).Sum();
                //
                // Create new name for any files...
                Dictionary<FileInfo, string> sourceFiles_NewNames = sourceFiles.FileNamesEncoder();

                using (ZipFile zip = EncryptedZipFile())
                {
                    // This is just a sample, provided to illustrate the DotNetZip interface.  
                    // This logic does not recourse through sub-directories.
                    // If you are zipping up a directory, you may want to see the AddDirectory() method, 
                    // which operates recursively. 
                    foreach (var file_NewName in sourceFiles_NewNames)
                    {
                        if (file_NewName.Key.Exists)
                        {
                            //
                            // Add File into ZipFile ...
                            ZipEntry zipEntry = zip.AddFile(file_NewName.Key.FullName);
                            zipEntry.FileName = file_NewName.Value; // Change Original FileName
                            zipEntry.Attributes = SecureAttributes;
                        }
                    }
                    //
                    // Encrypt files new name information
                    string encryptedFilesNewNameInformation = FileNameEncoder.EncryptEncodedList(sourceFiles_NewNames, this.Password, zip.Comment);
                    //
                    // Add files new name information's in zip data...
                    zip.AddEntry("FilesInformation.dll", encryptedFilesNewNameInformation).Attributes = SecureAttributes;
                    //
                    // Zip All Entry Files
                    //
                    zip.Save(toPath);
                }
            }
            catch (Exception ex)
            { ReportOccurrence(this, new ReportEventArgs("ZipEncryption.ZipFiles", ex)); return false; }

            return true;
        }

        /// <summary>
        /// Un Zip a zip file into Target path's
        /// </summary>
        /// <param name="ExistingZipFile">Source File</param>
        /// <param name="TargetDirectory">A directory path to unZip on it.</param>
        public void UnZip(string ExistingZipFile, string TargetDirectory, CancellationTokenSource CTS)
        {
            try
            {
                if (CTS.IsCancellationRequested)
                {
                    ReportOccurrence(this, new ReportEventArgs("ZipEncryption.UnZip", ReportCodes.CancelRequested, "Cancellation Requested"));
                    return;
                }
                if (File.Exists(ExistingZipFile))
                {
                    using (ZipFile zip = EncryptedZipFile(ZipFile.Read(ExistingZipFile)))
                    {
                        // 
                        // Calc Total Size
                        this.totalFilesSize = zip.Entries.Select(x => x.UncompressedSize).Sum();
                        //
                        // Analysis FileInformation.dll files for how to change File Names...
                        //
                        zip["FilesInformation.dll"].Attributes = NormalAttributes;
                        ZipEntry ze = zip["FilesInformation.dll"]; // Set ze to FileInformation.dll Zip file's
                        ze.FileName = "FilesInformation.txt";

                        // Extract FilesInformation.dll
                        if (string.IsNullOrEmpty(this.Password))
                            ze.Extract(TargetDirectory, ExtractExistingFileAction.OverwriteSilently);
                        else
                            ze.ExtractWithPassword(TargetDirectory, ExtractExistingFileAction.OverwriteSilently, this.Password);

                        string encryptedEncodedList = File.ReadAllText(TargetDirectory + "\\FilesInformation.txt");
                        string decryptedEncodedList;
                        Dictionary<string, string> changeNamesList = FileNameEncoder.DecryptEncodedList(encryptedEncodedList, this.Password, out decryptedEncodedList);

                        // Xchange "FilesInformation.txt" by decrypted data...
                        File.WriteAllText(TargetDirectory + "\\FilesInformation.txt", decryptedEncodedList);

                        //
                        // Read Other Real Files to Extract...
                        //
                        for (int i = 0; i < zip.Count; i++)
                        {
                            if (zip[i].FileName == "FilesInformation.txt") continue;// Extract any file except FilesInformation.dll

                            zip[i].Attributes = NormalAttributes;
                            // Change file name
                            if (changeNamesList.ContainsKey(zip[i].FileName))
                                zip[i].FileName = changeNamesList[zip[i].FileName];

                            if (string.IsNullOrEmpty(this.Password))
                                zip[i].Extract(TargetDirectory, ExtractExistingFileAction.OverwriteSilently);
                            else
                                zip[i].ExtractWithPassword(TargetDirectory, ExtractExistingFileAction.OverwriteSilently, this.Password);
                        }
                    }
                }
            }
            catch (Exception ex) { ReportOccurrence(this, new ReportEventArgs("ZipEncryption.UnZip", ex)); }
        }
    }
}
