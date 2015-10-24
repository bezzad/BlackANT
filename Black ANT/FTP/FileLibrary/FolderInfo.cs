using System;
using System.IO;
using DiskProbe;

namespace FTP.FileLib
{
    /// <summary>
    /// A struct for compare drives by drive Name and total size
    /// </summary>
    public class FolderInfo : IComparable, ICloneable, IDisposable, IComparable<FolderInfo>, IEquatable<FolderInfo>
    {
        public string FullName;
        public long TotalUsageSize;


        /// <summary>
        /// Create a directory info
        /// </summary>
        /// <param name="directoryPath">Name of the drive or directory</param>
        /// <param name="size">Byte size of the drive or directory</param>
        public FolderInfo(string directoryPath, long usageSize)
        {
            FullName = directoryPath;
            TotalUsageSize = usageSize;
        }

        /// <summary>
        /// Create a directory info
        /// </summary>
        /// <param name="directoryPath">Name of the drive or directory</param>
        /// <param name="size">Byte size of the drive or directory</param>
        public FolderInfo(string directoryPath)
            : this(new DirectoryInfo(directoryPath)) { }

        /// <summary>
        /// Create a directory info
        /// </summary>
        public FolderInfo(DriveInfo drive)
        {
            FolderInfo DI = FolderInfo.Parse(drive);
            if (DI != null)
            {
                this.FullName = DI.FullName;
                this.TotalUsageSize = DI.TotalUsageSize;
            }
        }

        /// <summary>
        /// Create a directory info
        /// </summary>
        public FolderInfo(DirectoryInfo dir)
        {
            FolderInfo DI = FolderInfo.Parse(dir);

            if (DI != null)
            {
                this.FullName = DI.FullName;
                this.TotalUsageSize = DI.TotalUsageSize;
            }
        }

        public override string ToString()
        {
            return string.Format(@"{0}  ,  Total Usage Size: [{1}] Bytes", FullName, TotalUsageSize);
        }

        public DirectoryInfo GetDirectoryInfo
        {
            get { return new DirectoryInfo(this.FullName); }
        }

        /// <summary>
        /// Converts the string representation of a DirectoryInformation to its DirectoryInformation
        /// equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="fromString">>A string containing a DirectoryInformation to convert.</param>
        /// <returns> 
        /// When this method returns, contains the DirectoryInformation value equivalent
        /// to the number contained in fromString, if the conversion succeeded, or zero if the
        /// conversion failed. The conversion fails if the s parameter is null, is not
        /// of the correct format.
        /// This parameter is passed uninitialized.
        /// </returns>
        public static FolderInfo Parse(string fromString)
        {
            FolderInfo driveInfo = null;

            ///
            /// DirectoryInformation Text e.g: 
            ///                             'C:\                    ,   Total Usage Size: [6565446842] Bytes'
            ///                             'D:\                    ,   Total Usage Size: [83165479394] Bytes'
            ///                             'E:\                    ,   Total Usage Size: [86464545941] Bytes'
            ///                             'F:\TestFolder\Behzad   ,   Total Usage Size: [4664545941] Bytes'
            ///                             'E:\LPL\CiCpro\SOS      ,   Total Usage Size: [86464545941] Bytes'
            ///
            string name = fromString.Substring(0, fromString.LastIndexOf(',') - 1).Trim();
            string strLength = fromString.Substring(fromString.LastIndexOf('[') + 1);
            strLength = strLength.Substring(0, strLength.IndexOf(']'));
            long length;
            if (!long.TryParse(strLength, out length)) throw new FormatException("'" + strLength + "'" + " is not numerical value!");

            driveInfo = new FolderInfo(name, length);

            return driveInfo;
        }

        /// <summary>
        /// Converts the System.IO.FileInfo representation of a DirectoryInformation to its DirectoryInformation
        /// equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="fromString">>A FileInfo containing a DirectoryInformation to convert.</param>
        /// <returns> 
        /// When this method returns, contains the DirectoryInformation value equivalent
        /// to the number contained in fromString, if the conversion succeeded, or zero if the
        /// conversion failed. The conversion fails if the s parameter is null, is not
        /// of the correct format.
        /// This parameter is passed uninitialized.
        /// </returns>
        public static FolderInfo Parse(DriveInfo fromDriveInfo)
        {
            if (fromDriveInfo == null || !fromDriveInfo.IsReady) return null;

            return new FolderInfo(fromDriveInfo.Name, fromDriveInfo.TotalSize - fromDriveInfo.AvailableFreeSpace);
        }

        /// <summary>
        /// Converts the System.IO.FileInfo representation of a DirectoryInformation to its DirectoryInformation
        /// equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="fromString">>A FileInfo containing a DirectoryInformation to convert.</param>
        /// <returns> 
        /// When this method returns, contains the DirectoryInformation value equivalent
        /// to the number contained in fromString, if the conversion succeeded, or zero if the
        /// conversion failed. The conversion fails if the s parameter is null, is not
        /// of the correct format.
        /// This parameter is passed uninitialized.
        /// </returns>
        public static FolderInfo Parse(DirectoryInfo fromDirectoryInfo)
        {
            if (fromDirectoryInfo == null || !fromDirectoryInfo.Exists) return null;

            return new FolderInfo(fromDirectoryInfo.FullName, fromDirectoryInfo.GetDirectorySize());
        }

        /// <summary>
        /// Converts the string representation of a DirectoryInformation to its DirectoryInformation
        /// equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="fromString">>A string containing a DirectoryInformation to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the DirectoryInformation value equivalent
        /// to the number contained in fromString, if the conversion succeeded, or zero if the
        /// conversion failed. The conversion fails if the s parameter is null, is not
        /// of the correct format.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string fromString, out FolderInfo result)
        {
            bool Succes = true;

            result = null;

            try
            {
                ///
                /// DirectoryInformation Text e.g: 
                ///                             'C:\                    ,   Total Usage Size: [6565446842] Bytes'
                ///                             'D:\                    ,   Total Usage Size: [83165479394] Bytes'
                ///                             'E:\                    ,   Total Usage Size: [86464545941] Bytes'
                ///                             'F:\TestFolder\Behzad   ,   Total Usage Size: [4664545941] Bytes'
                ///                             'E:\LPL\CiCpro\SOS      ,   Total Usage Size: [86464545941] Bytes'
                ///
                string name = fromString.Substring(0, fromString.LastIndexOf(',') - 1).Trim();
                string strLength = fromString.Substring(fromString.LastIndexOf('[') + 1);
                strLength = strLength.Substring(0, strLength.IndexOf(']'));
                long length;
                if (!long.TryParse(strLength, out length)) throw new FormatException("'" + strLength + "'" + " is not numerical value!");

                result = new FolderInfo(name, length);
            }
            catch { Succes = false; }

            return Succes;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FolderInfo)) return false;

            FolderInfo ObjOnTheOpposite = (FolderInfo)obj;
            return string.Equals(ObjOnTheOpposite.FullName, this.FullName, StringComparison.OrdinalIgnoreCase) &&
                   ObjOnTheOpposite.TotalUsageSize == this.TotalUsageSize;
        }

        public static bool operator ==(FolderInfo x, FolderInfo y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(FolderInfo a, FolderInfo b)
        {
            return !(a == b);
        }

        public static long operator +(FolderInfo a, FolderInfo b)
        {
            return a.TotalUsageSize + b.TotalUsageSize;
        }

        public static long operator -(FolderInfo a, FolderInfo b)
        {
            return a.TotalUsageSize - b.TotalUsageSize;
        }

        public override int GetHashCode()
        {
            return this.FullName.GetHashCode() ^ this.TotalUsageSize.GetHashCode();
        }

        #region Implement IComparable
        public int CompareTo(object obj)
        {
            // obj is object, so we can use its == operator
            if (obj == null) { return 1; }

            if (obj is FolderInfo)
            {
                return this.CompareTo(obj as FolderInfo);
            }
            if (obj is DriveInfo)
            {
                return this.CompareTo(FolderInfo.Parse(obj as DriveInfo));
            }

            throw new InvalidCastException("The object is not type of DirectoryInformation or System.IO.DriveInfo");
        }

        #endregion

        #region Implement ICloneable
        public object Clone()
        {
            return new FolderInfo(this.FullName, this.TotalUsageSize);
        }
        #endregion

        #region Implement IDisposable
        public void Dispose()
        {
            this.FullName = string.Empty;
            this.TotalUsageSize = default(long);
        }
        #endregion

        #region Implement IComparable<DirectoryInformation>
        public int CompareTo(FolderInfo other)
        {
            if (Object.ReferenceEquals(other, null))
                return 1;
            if (Object.ReferenceEquals(other, this)) //Not mandatory
                return 0;

            int result = this.TotalUsageSize.CompareTo(other.TotalUsageSize);
            if (result == 0)
                result = this.FullName.CompareTo(other.FullName);

            return result;
        }
        #endregion

        #region Implement IEquatable<DirectoryInformation>
        public bool Equals(FolderInfo other)
        {
            if (Object.ReferenceEquals(other, null))
                return false;

            return String.Equals(this.FullName, other.FullName, StringComparison.OrdinalIgnoreCase) &&
                   long.Equals(this.TotalUsageSize, other.TotalUsageSize);
        }
        #endregion
    }
}
