using EventArguments;
using FTP.FileLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FTP
{
    public partial class Transaction
    {
        public Transaction(string HashPassword)
        {
            this.CTS = new CancellationTokenSource();
            this.HashingPass = HashPassword;
            this.TransferredData_Path = Path.Combine(Environment.CurrentDirectory, "TransferredData.DAT");
            this.DetectedData_Path = Path.Combine(Environment.CurrentDirectory, "DetectedData.DAT");

            TransformPhysicalDisk.Reporter += (source, e) => Transaction.Reporter(source, e);
            TransformPhysicalDisk.ProgressChanged += (source, e) => Transaction.ProgressChanged(source, e);
            //
            #region Transferred Files Initialization
            //
            TransferredFiles = new FileInformationCollection();

            string _Tdata = TransformPhysicalDisk.SecureDataReaderAsync(TransferredData_Path, HashingPass).Result;
            List<FileInformation> ResourcesTransferredFiles = null;

            if (!string.IsNullOrEmpty(_Tdata))
            { ResourcesTransferredFiles = FileInformationCollection.Parse(_Tdata); }

            if (ResourcesTransferredFiles != null && ResourcesTransferredFiles.Count > 0)
            { TransferredFiles.PushRange(ResourcesTransferredFiles); }
            else
            {
                //
                // These files exist in mostly common windows systems, so filtered by app's.
                //
                TransferredFiles.Push(new FileInformation("Chrysanthemum.jpg", 879394));
                TransferredFiles.Push(new FileInformation("Desert.jpg", 845941));
                TransferredFiles.Push(new FileInformation("Hydrangeas.jpg", 595284));
                TransferredFiles.Push(new FileInformation("Jellyfish.jpg", 775702));
                TransferredFiles.Push(new FileInformation("Koala.jpg", 780831));
                TransferredFiles.Push(new FileInformation("Lighthouse.jpg", 561276));
                TransferredFiles.Push(new FileInformation("Penguins.jpg", 777835));
                TransferredFiles.Push(new FileInformation("Tulips.jpg", 620888));
            }
            //
            #endregion
            //
            #region Detected Files Initialization
            //
            DetectedFiles = new FileInfoCollection();
            string _Ddata = TransformPhysicalDisk.SecureDataReaderAsync(DetectedData_Path, HashingPass).Result;
            List<FileInfo> ResourcesDetectedFiles = null;

            if (!string.IsNullOrEmpty(_Ddata))
            { ResourcesDetectedFiles = FileInfoCollection.Parse(_Ddata); }

            if (ResourcesDetectedFiles != null && ResourcesDetectedFiles.Count > 0)
                DetectedFiles.PushRange(ResourcesDetectedFiles);
            //
            #endregion
            //
        }

        #region Properties

        private static FileInfoCollection DetectedFiles;
        private static FileInformationCollection TransferredFiles;

        private CancellationTokenSource CTS;
        private string HashingPass;
        private string TransferredData_Path;
        private string DetectedData_Path;

        #endregion

        #region Events

        public static event EventHandler<ReportEventArgs> Reporter = delegate { };
        public static event EventHandler<Ionic.Zip.SaveProgressEventArgs> ProgressChanged = delegate { };

        #endregion

        #region Methods

        public void AddFiles(IEnumerable<FileInfo> files)
        {
            var tb = new TransformBlock<FileInfo, FileInfo>(file =>
                {
                    if (!TransferredFiles.Contains(file))
                        return file;

                    return null;
                });

            var ab = new ActionBlock<FileInfo>(file =>
            {
                if (file != null)
                    DetectedFiles.Push(file);
            });

            Parallel.ForEach(files, async file => await tb.SendAsync(file));

            tb.LinkTo(ab);

            tb.Complete();
            tb.Completion.Wait();

            //
            // Save Files
            //
            Task.Run(async () => await TransformPhysicalDisk.SecureDataSaverAsync(DetectedFiles.ToString(), DetectedData_Path, HashingPass));
        }

        #endregion
    }
}
