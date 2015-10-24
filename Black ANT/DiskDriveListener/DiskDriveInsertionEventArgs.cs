using System;
using System.IO;

namespace DiskDriveListener
{
    /// <summary>
    /// Disk Drive Insertion Event Arguments
    /// </summary>
    public class DiskDriveInsertionEventArgs : EventArgs
    {
        private DriveInfo driveInfo;
        private string context;

        /// <summary>
        /// Constructor of Event Arguments
        /// </summary>
        /// <param name="drive">Inserted Drive Information</param>
        public DiskDriveInsertionEventArgs(DriveInfo drive)
        {
            driveInfo = drive;

            double totalSize = drive.TotalSize;
            double totalFreeSpace = drive.TotalFreeSpace;
            double availableFreeSpace = drive.AvailableFreeSpace;
            double usedSpace = totalSize - totalFreeSpace;

            context = string.Format(
                        "Device Name: {1}" +
                        "{0}Device type: {2}" +
                        "{0}Device volume label: {3}" +
                        "{0}Total size: {4:#,0} bytes  -  {5:#,0.##} KB  -  {6:#,0.##} MB  -  {7:#,0.##} GB" +
                        "{0}Total free space: {8:#,0} bytes  -  {9:#,0.##} KB  -  {10:#,0.##} MB  -  {11:#,0.##} GB" +
                        "{0}Available free space: {12:#,0} bytes  -  {13:#,0.##} KB  -  {14:#,0.##} MB  -  {15:#,0.##} GB" +
                        "{0}Used space: {16:#,0} bytes  -  {17:#,0.##} KB  -  {18:#,0.##} MB  -  {19:#,0.##} GB" +
                        "{0}--------------------------------------------------------------{0}",
                        Environment.NewLine, drive.Name, drive.DriveType, drive.VolumeLabel,
                        totalSize, totalSize / 1024, totalSize / 1024 / 1024, totalSize / 1024 / 1024 / 1024,
                        totalFreeSpace, totalFreeSpace / 1024, totalFreeSpace / 1024 / 1024, totalFreeSpace / 1024 / 1024 / 1024,
                        availableFreeSpace, availableFreeSpace / 1024, availableFreeSpace / 1024 / 1024, availableFreeSpace / 1024 / 1024 / 1024,
                        usedSpace, usedSpace / 1024, usedSpace / 1024 / 1024, usedSpace / 1024 / 1024 / 1024);
        }

        /// <summary>
        /// Read-only properties of driveInfo variable
        /// </summary>
        public DriveInfo DriveInformation
        {
            get { return driveInfo; }
        }

        /// <summary>
        /// Read-only properties of context variable
        /// </summary>
        public string Context
        {
            get { return context; }
        }
    }
}
