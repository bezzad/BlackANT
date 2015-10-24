using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace FileExtension
{
    // Summary:
    //      A FileType class for compare files by file Name and length
    [Serializable]
    [ComVisible(true)]
    [ProvideProperty("FileExtensionOption", typeof(FileType))]
    public class FileExtensionOption : FileType, IEquatable<FileExtensionOption>
    {
        #region Members
        /// <summary>
        /// Maximum File Length (Bytes)
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(long.MaxValue)]
        public long MaxSizeLimit { set; get; }


        /// <summary>
        /// Minimum File Length (Bytes)
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(0)]
        public long MinSizeLimit { set; get; }


        /// <summary>
        /// Is file sensitive to interval time from last access time?
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(false)]
        public bool DateSensitive { set; get; }


        /// <summary>
        /// Maximum interval time from last access time (days)
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(90)]
        public int MaxIntervalFromLastAccessTime { set; get; }
        #endregion
        #region Class Constructors
        /// <summary>
        /// Set a File Extension bye Limit in files size for smart search.
        /// </summary>
        /// <param name="ExtensionType">Extension of target file</param>
        /// <param name="MinSize">Minimum size (bytes)</param>
        /// <param name="MaxSize">Maximum size (bytes)</param>
        /// <param name="dateSensitive">Date Sensitive Extension (Example: .docx , .xlsx, ...)</param>
        /// <param name="maxInterval">Max interval from last access time</param>
        public FileExtensionOption(FileType ExtensionType, long MinSize, long MaxSize, bool dateSensitive, int maxInterval)
            : base(ExtensionType.ExtensionName, ExtensionType.Description, ExtensionType.Popularity)
        {
            this.MaxSizeLimit = MaxSize;
            this.MinSizeLimit = MinSize;
            this.DateSensitive = dateSensitive;
            this.MaxIntervalFromLastAccessTime = maxInterval;
        }

        /// <summary>
        /// Set a File Extension bye Limit in files size for smart search.
        /// Note: Do not date sensitive extensions.
        /// </summary>
        /// <param name="ExtensionName">Extension of target file</param>
        /// <param name="MaxSize">Maximum size (bytes)</param>
        /// <param name="MinSize">Minimum size (bytes)</param>
        public FileExtensionOption(FileType ExtensionType, long MinSize, long MaxSize)
            : this(ExtensionType, MinSize, MaxSize, false, int.MaxValue) { }

        /// <summary>
        /// Set a File Extension bye Limit in files size for smart search.
        /// Note: Do not date sensitive extensions.
        /// </summary>
        /// <param name="ExtensionName">Extension of target file</param>
        public FileExtensionOption(FileType ExtensionType)
            : this(ExtensionType, 0, long.MaxValue, false, int.MaxValue) { }

        /// <summary>
        /// Set a File Extension bye Limit in files size for smart search.
        /// </summary>
        /// <param name="ExtensionName">Extension of target file</param>
        /// <param name="dateSensitive">Date Sensitive Extension (Example: .docx , .xlsx, ...)</param>
        public FileExtensionOption(FileType ExtensionType, bool dateSensitive, int maxInterval)
            : this(ExtensionType, 0, long.MaxValue, dateSensitive, maxInterval) { }

        public FileExtensionOption()
        {
            this.MaxSizeLimit = long.MaxValue;
            this.MinSizeLimit = 0;
            this.DateSensitive = false;
            this.MaxIntervalFromLastAccessTime = 0;
        }
        #endregion
        #region Methods
        public override string ToString()
        {
            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

            return string.Format(culture, @"{0}  -  Minimum Size[{1}] ~ Maximum Size[{2}] {3}",
                this.ExtensionName,
                this.MinSizeLimit,
                this.MaxSizeLimit,
                this.DateSensitive ? string.Format(@"--->  Date-Sensitive Max({0})", this.MaxIntervalFromLastAccessTime) : "");
        }

        public static FileExtensionOption Parse(string fromString)
        {
            /// http://regexpal.com/

            FileExtensionOption fileExtension;

            try
            {
                // Example FileExtension.ToString() list's:
                //
                // .mp4  -  Minimum Size[32165466546] ~ Maximum Size[244554] --->    Date-Sensitive Max(12)
                // .3gp  -  Minimum Size[32165466546] ~ Maximum Size[244554] --->    Date-Sensitive Max(90)
                // .pdf  -  Minimum Size[32165466546] ~ Maximum Size[244554]
                // .cs  -  Minimum Size[32165466546] ~ Maximum Size[244554]
                // .htm  -  Minimum Size[32165466546] ~ Maximum Size[244554] --->    Date-Sensitive Max(120)
                // .html  -  Minimum Size[32165466546] ~ Maximum Size[244554]
                // .avi  -  Minimum Size[32165466546] ~ Maximum Size[244554]
                //                
                MatchCollection matches =
                    Regex.Matches(fromString, @"[\.\d][A-Za-z0-9]*|Minimum Size|Maximum Size|Date-Sensitive Max", RegexOptions.Singleline);

                if (matches.Count < 5 || matches.Count > 7)
                    throw new Exception("This string is not a File Extension!");
                else
                {
                    /// For example:  ".mp4  -  Minimum Size[0] ~ Maximum Size[244554] --->    Date-Sensitive Max(120)"
                    /// Pattern:      "[\.\d][A-Za-z0-9]*|Minimum Size|Maximum Size|Date-Sensitive Max"
                    ///
                    /// then matches collection is:
                    /// 
                    ///             matches[0].Value = ".mp4"
                    ///             matches[1].Value = "Minimum Size"
                    ///             matches[2].Value = long Number = "0"
                    ///             matches[3].Value = "Maximum Size"
                    ///             matches[4].Value = long Number = "244554"
                    ///             matches[5].Value? = "Date-Sensitive Max"
                    ///             matches[6].Value? = int number
                    ///             
                    string extensionName = matches[0].Value;
                    long MinSize = long.Parse(matches[2].Value);
                    long MaxSize = long.Parse(matches[4].Value);
                    bool dateSensitive = matches.Count > 5 ? true : false;
                    int maxInterval = dateSensitive ? int.Parse(matches[6].Value) : int.MaxValue;
                    FileType extension = new FileType(extensionName);
                    fileExtension = new FileExtensionOption(extension, MinSize, MaxSize, dateSensitive, maxInterval);
                }
            }
            catch (Exception ex) { throw ex; }

            return fileExtension;
        }

        /// <summary>
        /// Converts the string representation of a extension to its FileExtension
        /// equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="fromString">A string containing a file extension to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the FileExtension value equivalent
        /// to the number contained in fromString, if the conversion succeeded, or zero if the
        /// conversion failed. The conversion fails if the s parameter is null, is not
        /// of the correct format.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string fromString, out FileExtensionOption result)
        {
            /// http://regexpal.com/

            bool Succes = true;
            result = null;
            try
            {
                // Example FileExtension.ToString() list's:
                //
                // .mp4  -  Minimum Size[32165466546] ~ Maximum Size[244554] --->    Date-Sensitive Max(12)
                // .3gp  -  Minimum Size[32165466546] ~ Maximum Size[244554] --->    Date-Sensitive Max(90)
                // .pdf  -  Minimum Size[32165466546] ~ Maximum Size[244554]
                // .cs  -  Minimum Size[32165466546] ~ Maximum Size[244554]
                // .htm  -  Minimum Size[32165466546] ~ Maximum Size[244554] --->    Date-Sensitive Max(120)
                // .html  -  Minimum Size[32165466546] ~ Maximum Size[244554]
                // .avi  -  Minimum Size[32165466546] ~ Maximum Size[244554]
                //                
                MatchCollection matches =
                    Regex.Matches(fromString, @"[\.\d][A-Za-z0-9]*|Minimum Size|Maximum Size|Date-Sensitive Max", RegexOptions.Singleline);

                if (matches.Count < 5 || matches.Count > 7)
                    Succes = false;
                else
                {
                    /// For example:  ".mp4  -  Minimum Size[321654665] ~ Maximum Size[244554] --->    Date-Sensitive Max(120)"
                    /// Pattern:      "[\.\d][A-Za-z0-9]*|Minimum Size|Maximum Size|Date-Sensitive Max"
                    ///
                    /// then matches collection is:
                    /// 
                    ///             matches[0].Value = ".mp4"
                    ///             matches[1].Value = "Minimum Size"
                    ///             matches[2].Value = long Number
                    ///             matches[3].Value = "Maximum Size"
                    ///             matches[4].Value = long Number
                    ///             matches[5].Value? = "Date-Sensitive Max"
                    ///             matches[6].Value? = int number
                    ///             
                    string extensionName = matches[0].Value;
                    long MinSize = long.Parse(matches[2].Value);
                    long MaxSize = long.Parse(matches[4].Value);
                    bool dateSensitive = matches.Count > 5 ? true : false;
                    int maxInterval = dateSensitive ? int.Parse(matches[6].Value) : int.MaxValue;
                    FileType extension = new FileType(extensionName);
                    result = new FileExtensionOption(extension, MinSize, MaxSize, dateSensitive, maxInterval);
                }
            }
            catch { Succes = false; }

            return Succes;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as FileExtensionOption);
        }
        public bool Equals(FileExtensionOption other)
        {
            if (Object.ReferenceEquals(other, null))
                return false;
            if (Object.ReferenceEquals(other, this)) //Not mandatory
                return true;

            return String.Equals(this.ExtensionName, other.ExtensionName) &&
                long.Equals(this.MinSizeLimit, other.MinSizeLimit) &&
                long.Equals(this.MaxSizeLimit, other.MaxSizeLimit) &&
                bool.Equals(this.DateSensitive, other.DateSensitive);
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(FileExtensionOption x, FileExtensionOption y)
        {
            return Object.Equals(x, y);
        }

        public static bool operator !=(FileExtensionOption a, FileExtensionOption b)
        {
            return !(a == b);
        }

        public string FileTypeToString()
        {
            return base.ToString();
        }
        ~FileExtensionOption()
        {
            this.Dispose();
            // This object will be cleaned up by the Dispose method. 
            // Therefore, you should call GC.SupressFinalize to 
            // take this object off the finalization queue 
            // and prevent finalization code for this object 
            // from executing a second time.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
