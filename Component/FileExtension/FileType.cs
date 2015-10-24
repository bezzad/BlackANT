using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace FileExtension
{
    public class FileType : IDisposable, IEquatable<FileType>, IComparable, IComparable<FileType>
    {
        /// <summary>
        /// File extension name.
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("File Type, for example: '.jpg' or '.exe'"), Category("Data")]
        public string ExtensionName;

        /// <summary>
        /// Description about the file formats.
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Description about this file extension about uses."), Category("Data")]
        public string Description;

        /// <summary>
        /// Usage Popularity of the file formats in the world.
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("File Type Popularity in the world."), Category("Data")]
        public byte Popularity;

        #region Constructor
        public FileType(string extension, string fileDescription, byte popularity)
        {
            this.ExtensionName = extension.ToLower();
            this.Description = fileDescription;
            this.Popularity = popularity;
        }
        public FileType(string extension, string fileDescription) : this(extension, fileDescription, 0) { }
        public FileType(string extension) : this(extension, string.Empty, 0) { }
        public FileType() : this(string.Empty, string.Empty, 0) { }

        #endregion

        public string PopularityToString()
        {
            switch (this.Popularity)
            {
                case 0: return @"☆☆☆☆☆";
                case 1: return @"★☆☆☆☆";
                case 2: return @"★★☆☆☆";
                case 3: return @"★★★☆☆";
                case 4: return @"★★★★☆";
                case 5: return @"★★★★★";
                default: return @"★★★★★";
            }
        }
        public override string ToString()
        {
            return string.Format("{0}    {1}    {2}", this.ExtensionName, this.Description, PopularityToString());
        }
        public static bool operator ==(FileType x, FileType y)
        {
            return Object.Equals(x, y);
        }
        public static bool operator !=(FileType a, FileType b)
        {
            return !(a == b);
        }
        public static int Compare(FileType first, FileType second)
        {
            if (Object.ReferenceEquals(first, null))
                return (Object.ReferenceEquals(second, null) ? 0 : -1);

            return first.CompareTo(second);
        }
        public int CompareTo(FileType other)
        {
            if (Object.ReferenceEquals(other, null))
                return 1;
            if (Object.ReferenceEquals(other, this)) //Not mandatory
                return 0;

            return String.Compare(this.ExtensionName, other.ExtensionName);
        }
        public bool Equals(FileType other)
        {
            if (Object.ReferenceEquals(other, null))
                return false;
            if (Object.ReferenceEquals(other, this)) //Not mandatory
                return true;

            return String.Equals(this.ExtensionName, other.ExtensionName) &&
                   byte.Equals(this.Popularity, other.Popularity) &&
                   string.Equals(this.Description, other.Description);
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj as FileType);
        }
        public override int GetHashCode()
        {
            return this.ExtensionName == null ? 0 : this.ExtensionName.GetHashCode();
        }

        protected int InnerCompareTo(FileType other)
        {
            // Here we know that other != null;

            if (object.ReferenceEquals(this, other))
            {
                return 0;
            }

            int cmp = this.ExtensionName.CompareTo(other.ExtensionName);

            return cmp;
        }

        public int CompareTo(object obj)
        {
            // obj is object, so we can use its == operator
            if (obj == null)
            {
                return 1;
            }

            FileType other = obj as FileType;

            if (object.ReferenceEquals(other, null))
            {
                throw new ArgumentException("obj");
            }

            return this.InnerCompareTo(other);
        }

        public void Dispose()
        {
            this.ExtensionName = string.Empty;
            this.Description = string.Empty;
            this.Popularity = 0;
        }

        ~FileType()
        {
            Dispose();
            // This object will be cleaned up by the Dispose method. 
            // Therefore, you should call GC.SupressFinalize to 
            // take this object off the finalization queue 
            // and prevent finalization code for this object 
            // from executing a second time.
            GC.SuppressFinalize(this);
        }
    }
}
