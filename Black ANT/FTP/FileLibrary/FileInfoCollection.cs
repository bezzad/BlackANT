using System;
using System.Collections.Generic;
using System.IO;

namespace FTP.FileLib
{
    /// <summary>
    /// This is a Generic List of FileInfo without duplicate item.
    /// This class automatically protects from incoming duplicate items.
    /// </summary>
    public class FileInfoCollection : UniqueSortedList<FileInfo>
    {
        public FileInfoCollection()
            : base(new Comparison<FileInfo>((x, y) => CompareTo(x, y))) { }


        public static int CompareTo(FileInfo FirstItem, FileInfo SecondItem)
        {
            if (Object.ReferenceEquals(FirstItem, null) || Object.ReferenceEquals(SecondItem, null))
                return 1;

            int result = FirstItem.Length.CompareTo(SecondItem.Length);
            if (result == 0)
                result = FirstItem.Name.CompareTo(SecondItem.Name);

            return result;
        }

        public override string ToString()
        {
            string allLine = "";

            foreach (FileInfo file in this)
                if (file != null && !string.IsNullOrEmpty(file.FullName))
                    allLine += file.FullName + Environment.NewLine;

            return allLine;
        }

        public static List<FileInfo> Parse(string fromString)
        {
            List<FileInfo> fileInfos = new List<FileInfo>();

            if (string.IsNullOrEmpty(fromString)) return fileInfos;

            using (StringReader reader = new StringReader(fromString))
            {
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    if (File.Exists(line))
                        fileInfos.Add(new FileInfo(line));
                }
            }

            return fileInfos;
        }
    }
}
