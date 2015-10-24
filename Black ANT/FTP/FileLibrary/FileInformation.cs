using System;
using System.IO;

namespace FTP.FileLib
{
    /// <summary>
    /// A struct for compare files by file Name and length
    /// </summary>
    public class FileInformation : IComparable, ICloneable, IDisposable, IComparable<FileInformation>, IEquatable<FileInformation>
    {
        public string Name;
        public long Size;


        /// <summary>
        /// Create a file info
        /// </summary>
        /// <param name="name">Name of the file</param>
        /// <param name="size">Byte size of the file</param>
        public FileInformation(string name, long size)
        {
            Name = name;
            Size = size;
        }

        /// <summary>
        /// Create a file info
        /// </summary>
        public FileInformation(FileInfo file)
        {
            FileInformation FI = FileInformation.Parse(file);
            if (FI != null)
            {
                this.Name = FI.Name;
                this.Size = FI.Size;
            }
        }

        public override string ToString()
        {
            return string.Format(@"{0}  ,  [{1}] Bytes", Name, Size);
        }

        /// <summary>
        /// Converts the string representation of a FileInformation to its FileInformation
        /// equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="fromString">>A string containing a FileInformation to convert.</param>
        /// <returns> 
        /// When this method returns, contains the FileInformation value equivalent
        /// to the number contained in fromString, if the conversion succeeded, or zero if the
        /// conversion failed. The conversion fails if the s parameter is null, is not
        /// of the correct format.
        /// This parameter is passed uninitialized.
        /// </returns>
        public static FileInformation Parse(string fromString)
        {
            FileInformation fileInfo = null;

            ///
            /// FileInformation Text e.g: 
            ///                             'behzad khosravifar[2014].mp4    ,   [6566842] Bytes'
            ///                             'Chrysanthemum.jpg  ,   [879394] Bytes'
            ///                             'Desert.jpg     ,   [845941] Bytes'
            ///
            string name = fromString.Substring(0, fromString.LastIndexOf(',') - 1).Trim();
            string strLength = fromString.Substring(fromString.LastIndexOf('[') + 1);
            strLength = strLength.Substring(0, strLength.IndexOf(']'));
            long length;
            if (!long.TryParse(strLength, out length)) throw new FormatException("'" + strLength + "'" + " is not numerical value!");

            fileInfo = new FileInformation(name, length);

            return fileInfo;
        }

        /// <summary>
        /// Converts the System.IO.FileInfo representation of a FileInformation to its FileInformation
        /// equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="fromString">>A FileInfo containing a FileInformation to convert.</param>
        /// <returns> 
        /// When this method returns, contains the FileInformation value equivalent
        /// to the number contained in fromString, if the conversion succeeded, or zero if the
        /// conversion failed. The conversion fails if the s parameter is null, is not
        /// of the correct format.
        /// This parameter is passed uninitialized.
        /// </returns>
        public static FileInformation Parse(FileInfo fromFileInfo)
        {
            if (fromFileInfo == null) return null;

            return new FileInformation(fromFileInfo.Name, fromFileInfo.Length);
        }

        /// <summary>
        /// Converts the string representation of a FileInformation to its FileInformation
        /// equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="fromString">>A string containing a FileInformation to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the FileInformation value equivalent
        /// to the number contained in fromString, if the conversion succeeded, or zero if the
        /// conversion failed. The conversion fails if the s parameter is null, is not
        /// of the correct format.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string fromString, out FileInformation result)
        {
            bool Succes = true;

            result = null;

            try
            {
                ///
                /// FileInformation Text e.g: 
                ///                             'behzad khosravifar[2014].mp4    ,   [6566842] Bytes'
                ///                             'Chrysanthemum.jpg  ,   [879394] Bytes'
                ///                             'Desert.jpg     ,   [845941] Bytes'
                ///
                string name = fromString.Substring(0, fromString.LastIndexOf(',') - 1).Trim();
                string strLength = fromString.Substring(fromString.LastIndexOf('[') + 1);
                strLength = strLength.Substring(0, strLength.IndexOf(']'));
                long length;
                if (!long.TryParse(strLength, out length)) throw new FormatException("'" + strLength + "'" + " is not numerical value!");

                result = new FileInformation(name, length);
            }
            catch { Succes = false; }

            return Succes;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FileInformation)) return false;

            FileInformation ObjOnTheOpposite = (FileInformation)obj;
            return string.Equals(ObjOnTheOpposite.Name, this.Name, StringComparison.OrdinalIgnoreCase) && 
                ObjOnTheOpposite.Size == this.Size;
        }
        public static bool operator ==(FileInformation x, FileInformation y)
        {
            return x.Equals(y);
        }
        public static bool operator !=(FileInformation a, FileInformation b)
        {
            return !(a == b);
        }
        public override int GetHashCode()
        {
            return this.Name.GetHashCode() ^ this.Size.GetHashCode();
        }

        #region Implement IComparable
        public int CompareTo(object obj)
        {
            // obj is object, so we can use its == operator
            if (obj == null) { return 1; }

            if (obj is FileInformation)
            {
                FileInformation other = obj as FileInformation;

                return this.CompareTo(other);
            }
            if (obj is FileInfo)
            {
                FileInfo other = obj as FileInfo;

                if (Object.ReferenceEquals(other, null))
                    return 1;

                int result = this.Size.CompareTo(other.Length);
                if (result == 0)
                    result = this.Name.CompareTo(other.Name);

                return result;
            }

            throw new InvalidCastException("The object is not type of FileInformation or System.IO.FileInfo");
        }

        #endregion

        #region Implement ICloneable
        public object Clone()
        {
            return new FileInformation(this.Name, this.Size);
        }
        #endregion

        #region Implement IDisposable
        public void Dispose()
        {
            this.Name = string.Empty;
            this.Size = default(long);
        }
        #endregion

        #region Implement IComparable<FileInformation>
        public int CompareTo(FileInformation other)
        {
            if (Object.ReferenceEquals(other, null))
                return 1;
            if (Object.ReferenceEquals(other, this)) //Not mandatory
                return 0;

            int result = this.Size.CompareTo(other.Size);
            if (result == 0)
                result = this.Name.CompareTo(other.Name);

            return result;
        }
        #endregion

        #region Implement IEquatable<FileInformation>
        public bool Equals(FileInformation other)
        {
            if (Object.ReferenceEquals(other, null))
                return false;

            return String.Equals(this.Name, other.Name, StringComparison.OrdinalIgnoreCase) && 
                   long.Equals(this.Size, other.Size);
        }
        #endregion
    }
}
