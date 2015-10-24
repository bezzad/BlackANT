using System;
using System.Collections.Generic;
using System.Linq;

namespace FileExtension
{
    public class CommonFileType : List<FileType>
    {
        public string Description;
        public string FileFormats;

        #region Constructor
        public CommonFileType(List<FileType> fileTypes, string FileFormatName, string description)
        {
            this.AddRange(fileTypes);
            this.FileFormats = FileFormatName;
            this.Description = description;
        }
        public CommonFileType(List<FileType> fileTypes, string FileFormatName) : this(fileTypes, FileFormatName, string.Empty) { }
        public CommonFileType(List<FileType> fileTypes) : this(fileTypes, string.Empty, string.Empty) { }
        public CommonFileType() { }
        #endregion

        public override string ToString()
        {
            string combinedList = string.Empty;
            foreach (string item in this.Select(x => x.ToString() + Environment.NewLine).AsParallel())
                combinedList += item;

            string value = string.Format("{1}{0} {2}{0}{0}{3}",
                Environment.NewLine, this.FileFormats, this.Description, combinedList);


            return value;
        }
    }
}
