using System.Collections.Generic;
using System.IO;

namespace FTP.FileLib
{
    public class PackageFiles : IEnumerable<List<FileInfo>>
    {
        FileInfoCollection _Files;
        long PackageStandardSize;
        long FreeSpceLimit;

        /// <summary>
        /// Package all data in a specific size pack and total packs size will be less than total free space.
        /// </summary>
        /// <param name="PSS">Pack Standard Size (bytes)</param>
        /// <param name="FSL">Total Packs Size (bytes)</param>
        public PackageFiles(FileInfoCollection files, long PSS, long FSL)
        {
            _Files = files;

            PackageStandardSize = PSS;
            FreeSpceLimit = FSL;
        }

        /// <summary>
        /// Package all data in a specific size pack and total packs size will be less than total free space.
        /// </summary>
        /// <returns>The Pack as <see cref="T:System.Collection.Generic.List<FileInfo>"/>.</returns>
        public IEnumerator<List<FileInfo>> GetEnumerator()
        {
            do
            {
                long _CurrentPackSize = 0; // just last packing size
                List<FileInfo> CurrentPack = new List<FileInfo>(); // a file info pack

            reload:
                FileInfo file;
                try
                {
                    file = _Files.Peek();
                }
                catch { break; }

                //
                // if it's very large file for Mass storage, and the list is sorted , so next files large too, then break [END]
                if (file.Length + PackageStandardSize > FreeSpceLimit)
                {
                    if (_CurrentPackSize > 0) // Current Pack has data
                    {
                        FreeSpceLimit -= _CurrentPackSize;
                        yield return CurrentPack;
                    }
                    // The pack is empty and the file is large for packing, so break loop;
                    break;
                }
                //
                // if this file added to OnePack then size of the pack will be larger than decided package size
                else if (file.Length + _CurrentPackSize > PackageStandardSize)
                {
                    if (_CurrentPackSize == 0) // Current Pack is empty
                    {
                        _CurrentPackSize += file.Length;
                        CurrentPack.Add(_Files.Pop());

                        FreeSpceLimit -= _CurrentPackSize;
                        yield return CurrentPack;
                        continue;
                    }
                    else // Current Pack has data, so send that.
                    {
                        FreeSpceLimit -= _CurrentPackSize;
                        yield return CurrentPack;

                        _CurrentPackSize = 0; // clear current pack
                        CurrentPack = new List<FileInfo>(); // create new file info pack
                        goto reload;
                    }
                }
                else
                {
                    _CurrentPackSize += file.Length;
                    CurrentPack.Add(_Files.Pop());
                    goto reload;
                }
            } while (true);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
