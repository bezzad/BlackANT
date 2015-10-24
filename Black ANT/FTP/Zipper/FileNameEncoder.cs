using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Security;
using System.Text.RegularExpressions;

namespace FTP.Zipper
{
    public static class FileNameEncoder
    {
        /// <summary>
        /// Convert list of File Names to list of files and New file Names
        /// </summary>
        /// <param name="files">List of files</param>
        /// <returns>List of files + encoded file names</returns>
        public static Dictionary<FileInfo, string> FileNamesEncoder(this List<FileInfo> files)
        {
            Dictionary<FileInfo, string> encodedList = new Dictionary<FileInfo, string>();
            try
            {
                // Set name for any files for zipping.
                int counterName = 0;
                //
                foreach (FileInfo file in files)
                {
                    encodedList.Add(file, counterName++.ToString()); // Go to next file name's
                }
            }
            catch { }

            return encodedList;
        }

        /// <summary>
        /// Encrypt Files by new names in a string variable.
        /// </summary>
        /// <param name="encodedList">List of files + encoded file names</param>
        /// <param name="Password">Encryption Password Keys</param>
        /// <returns>Encrypted context in string variable</returns>
        public static string EncryptEncodedList(Dictionary<FileInfo, string> encodedList, string Password, string fileTitle)
        {
            string encrypted = fileTitle;

            foreach (var file_NewName in encodedList)
            {
                //
                // Fill FilesInformaion.dll data...
                //
                // Example:
                // 14[.JPG]  23423423 byte  --->  FullName: D:\Picture data\IMG_8056.JPG
                //
                encrypted += string.Format("{0}[{1}]  {2} byte  --->  FullName: {3}{4}",
                    file_NewName.Value, file_NewName.Key.Extension, file_NewName.Key.Length, file_NewName.Key.FullName, Environment.NewLine);
            }

            return encrypted.Encrypt(Password, true);
        }

        public static Dictionary<string, string> DecryptEncodedList(string encryptedFile, string Password, out string decryptedFile)
        {
            //
            // First Decrypt the Encrypted Encoded List file's
            decryptedFile = encryptedFile.Decrypt(Password, true);
            //
            // Analyze the decrypted encoded list and convert that's to file names List...
            //
            Dictionary<string, string> decryptedList = new Dictionary<string, string>();
            //
            // Read All Lines of decryptedFile
            // e.g. of a decryptedFile:
            //
            // This zip archive was created by the Zipping application from:
            //      Machine: 'HAF-932'
            //      UserName: 'Khosravifar'
            //
            // 0[.JPG]  23423423 byte  --->  FullName: D:\Picture data\IMG_8056.JPG
            //
            // 1[.JPG]  13443434 byte  --->  FullName: D:\Picture data\IMG_8057.JPG
            //
            // 2[.JPG]  1242342433 byte  --->  FullName: D:\Picture data\IMG_8060.JPG
            //
            // 3[.JPG]  1443657 byte  --->  FullName: D:\Picture data\IMG_8061.JPG
            //
            // Pattern: "\d+\[\.[A-Za-z0-9]+\]"
            //
            string Pattern = @"\d+\[\.[A-Za-z0-9]+\]";
            MatchCollection matches = Regex.Matches(decryptedFile, Pattern, RegexOptions.Multiline);

            foreach (Match match in matches)
            {
                string newName = match.Value.Replace("[", string.Empty).Replace("]", string.Empty);
                int dashIndex = newName.IndexOf('.');
                string realName = newName.Substring(0, dashIndex);

                decryptedList.Add(realName, newName);
            }

            decryptedFile = decryptedFile.Replace("[", string.Empty).Replace("]", string.Empty);

            return decryptedList;
        }
    }
}
