using EventArguments;
using FTP.FileLib;
using Security;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FTP
{
    public partial class Transaction
    {
        #region Field
        public DriveInfo MassStorageDrive;
        #endregion

        #region Methods

        public void TransferToDisk()
        {
            if (DetectedFiles.Count == 0 || this.MassStorageDrive == null || !this.MassStorageDrive.IsReady ||
                TransformPhysicalDisk.GetState == TaskStatus.WaitingToRun ||
                TransformPhysicalDisk.GetState == TaskStatus.Running)
                return;

            TransformPhysicalDisk.Start(this.MassStorageDrive, DetectedFiles, TransferredFiles, HashingPass,
                TransferredData_Path, DetectedData_Path);
        }

        public void CancelTransferToDisk()
        { TransformPhysicalDisk.Stop(); }

        #endregion

        public static class TransformPhysicalDisk
        {
            #region Field

            internal static long PSS = 100 * 1024 * 1024; // Packet Standard Size = 100 MB
            internal static string TargetDirectory = @"APOLLO\Ant Probe Orifices of Logical Lazy Objects\"; // مورچه کاوشگر سوراخ اشیاء منطقی تنبل

            private static CancellationTokenSource CTS = new CancellationTokenSource();
            private static int TargetTrustFileNameCounter = 1;

            private static DriveInfo MassStorage;
            private static FileInfoCollection DetectedFiles;
            private static FileInformationCollection TransferredFiles;

            private static Zipper.ZipEncryption ZipE;
            private static Task TransferAsyncAwaiter;

            #endregion

            #region Events
            internal static event EventHandler<Ionic.Zip.SaveProgressEventArgs> ProgressChanged = delegate { };
            internal static event EventHandler<ReportEventArgs> Reporter = delegate { };
            #endregion

            #region Methods

            internal static void Start(DriveInfo TargetDrive, FileInfoCollection DetectedList,
                FileInformationCollection TransferredList, string HashPassword,
                string Transferred_DAT_Path, string Detected_DAT_Path)
            {
                #region Set Variable Values
                //
                CTS = new CancellationTokenSource();
                MassStorage = TargetDrive;
                DetectedFiles = DetectedList;
                TransferredFiles = TransferredList;
                //
                #region ZipEncryption Initialization
                ZipE = new Zipper.ZipEncryption(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), HashPassword);
                ZipE.ReportOccurrence += (source, e) => Reporter(source, e);
                ZipE.SaveProgress += (source, e) => ProgressChanged(source, e);
                #endregion
                #endregion
                //
                TransferAsyncAwaiter = TransferAsync(HashPassword).ContinueWith(async (setter) =>
                    {
                        //
                        // Reports TransferAsync Task Exceptions...
                        //
                        await Task.Run(() =>
                        {
                            AggregateException AExps = setter.Exception;

                            if (AExps != null)
                            {
                                foreach (var exp in AExps.InnerExceptions)
                                {
                                    Reporter("TransferToDisk", new ReportEventArgs("TransformPhysicalDisk.TransferAsync", exp));
                                }
                            }
                        });
                        //
                        // Save Files
                        //
                        await SecureDataSaverAsync(DetectedFiles.ToString(), Detected_DAT_Path, HashPassword);

                        await SecureDataSaverAsync(TransferredFiles.ToString(), Transferred_DAT_Path, HashPassword);
                    });

                Task.WaitAll(TransferAsyncAwaiter);
            }

            internal static void Stop()
            {
                CTS.Cancel();
            }

            // Signature specifies Task
            private static async Task TransferAsync(string Password)
            {
                await Task.Run(() =>
                    {
                        if (MassStorage.AvailableFreeSpace >= PSS * 2)
                        {
                            PackageFiles Packs = new PackageFiles(DetectedFiles, PSS, MassStorage.AvailableFreeSpace);

                            int PackCounter = 1;
                            long allSize = 0;

                            foreach (var PackReader in Packs)
                            {
                                if (CTS.IsCancellationRequested)
                                {
                                    DetectedFiles.PushRange(PackReader); // ReLoad Pop data, because data not stored!
                                    break;
                                }

                                long PackSize = PackReader.AsParallel().Sum(f => f.Length);
                                allSize += PackSize;

                                Reporter("TransformPhysicalDisk",
                                    new ReportEventArgs("TransformPhysicalDisk.TransferAsync",
                                    ReportCodes.PackagingStarted,
                                    "Packaging [{0}] by Size: [{1}] bytes", PackCounter, PackSize));


                                if (!ZipE.ZipFiles(PackReader, GetTrustFileName(), CTS))
                                {
                                    CTS.Cancel();
                                    DetectedFiles.PushRange(PackReader); // ReLoad Pop data, because data not stored!

                                    Reporter("TransformPhysicalDisk",
                                     new ReportEventArgs("TransformPhysicalDisk.TransferAsync",
                                     ReportCodes.PackagingDone,
                                     "Pack [{0}] by Size: [{1}] bytes Rejected", PackCounter++, PackSize));

                                    break;
                                }

                                TransferredFiles.PushRange(PackReader);

                                Reporter("TransformPhysicalDisk",
                                    new ReportEventArgs("TransformPhysicalDisk.TransferAsync",
                                    ReportCodes.PackagingDone,
                                    "Pack [{0}] by Size: [{1}] bytes Transferred", PackCounter++, PackSize));

                            }

                            Reporter("TransformPhysicalDisk",
                                                new ReportEventArgs("TransformPhysicalDisk.TransferAsync",
                                                ReportCodes.PackagingDone,
                                                "Packaging Complete {0} Packs, Total Packs Size is [{1}] bytes", PackCounter, allSize));
                            /// -----------------------------------
                            /// 
                        }
                    });
            }

            private static string GetTrustFileName()
            {
                string PathDirectory = Path.Combine(MassStorage.Name, TargetDirectory);

                if (!Directory.Exists(PathDirectory)) Directory.CreateDirectory(PathDirectory);

                while (File.Exists(Path.Combine(PathDirectory, string.Format("Apollo [{0}] .bant", TargetTrustFileNameCounter))))
                { TargetTrustFileNameCounter++; }

                return Path.Combine(PathDirectory, string.Format("Apollo [{0}] .bant", TargetTrustFileNameCounter++));
            }


            public static TaskStatus GetState
            {
                get { return (TransferAsyncAwaiter != null) ? TransferAsyncAwaiter.Status : TaskStatus.Faulted; }
            }

            public static async Task<string> SecureDataReaderAsync(string Path, string HashPassword)
            {
                return await Task.Run<string>(() =>
                {
                    try
                    {
                        //
                        // Set Normal Attributes for file to UnLock File to change data
                        //
                        if (File.Exists(Path))
                        {
                            new FileInfo(Path).NormalAttributer();
                        }
                        else return null;
                        //
                        // Read Data
                        //
                        string encryptedData = File.ReadAllText(Path);
                        //
                        // Close File:
                        //
                        // Set Secure Attributes for file to Lock File
                        //
                        if (File.Exists(Path))
                        {
                            new FileInfo(Path).SecureAttributer();
                        }

                        string decryptedDate = encryptedData.Decrypt(HashPassword, true);

                        //
                        Reporter("Transaction", new ReportEventArgs("Transaction.SecureDataReaderAsync", ReportCodes.DataLoaded,
                            "{0} Loaded", Path));
                        //
                        return decryptedDate;
                    }
                    catch (Exception ex)
                    {
                        Reporter("Transaction", new ReportEventArgs("Transaction.SecureDataReaderAsync", ex));
                        return null;
                    }
                });
            }

            public static async Task SecureDataSaverAsync(string DataValue, string Path, string HashPassword)
            {
                await Task.Run(() =>
                       {
                           try
                           {
                               //
                               // Set Normal Attributes for file to UnLock File to change data
                               //
                               if (File.Exists(Path)) new FileInfo(Path).NormalAttributer();
                               //
                               // Encrypt data
                               //
                               string encryptedData = DataValue.Encrypt(HashPassword, true);
                               //
                               // Save in I/O Path
                               // 
                               File.WriteAllText(Path, encryptedData);
                               //
                               // Set Secure Attributes for file to Lock File
                               //
                               if (File.Exists(Path)) new FileInfo(Path).SecureAttributer();
                               //
                               Reporter("Transaction", new ReportEventArgs("Transaction.SecureDataSaverAsync", ReportCodes.DataStored,
                                   "{0} Stored", Path));
                           }
                           catch (Exception exp)
                           { Reporter("Transaction", new ReportEventArgs("Transaction.SecureDataSaverAsync", exp)); }
                       });
            }
            #endregion
        }
    }
}