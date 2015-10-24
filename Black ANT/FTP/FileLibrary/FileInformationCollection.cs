using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FTP.FileLib
{
    /// <summary>
    /// This is a Generic List of FileInformation without duplicate item.
    /// This class automatically protects from incoming duplicate items.
    /// </summary>
    public class FileInformationCollection : UniqueSortedList<FileInformation>
    {
        public FileInformationCollection()
            : base(new Comparison<FileInformation>((x, y) => x.CompareTo(y))) { }



        public bool Contains(FileInfo item)
        {
            return base.Contains(new FileInformation(item));
        }

        public override string ToString()
        {
            string allLine = "";

            foreach (FileInformation fi in this)
                allLine += fi.ToString() + Environment.NewLine;

            return allLine;
        }

        public void PushRange(IEnumerable<FileInfo> items)
        {
            base.PushRange(items.Select(x => new FileInformation(x)));
        }

        public static List<FileInformation> Parse(string fromString)
        {
            List<FileInformation> fileInfos = new List<FileInformation>();

            if (!string.IsNullOrEmpty(fromString))
            {
                using (StringReader reader = new StringReader(fromString))
                {
                    string line = string.Empty;
                    do
                    {
                        line = reader.ReadLine();
                        if (line != null)
                        {
                            FileInformation fileInfo;
                            if (FileInformation.TryParse(line, out fileInfo))
                                fileInfos.Add(fileInfo);
                        }
                    } while (line != null);
                }
            }

            return fileInfos;
        }
    }
}
