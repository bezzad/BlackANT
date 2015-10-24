using EventArguments;
using FileExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow; // Add Reference --> Manage NuGet Packages... --> Microsoft.TPL.Dataflow
using FTP.FileLib;
using System.Windows.Forms;
using System.Diagnostics;

namespace DiskProbe
{
    /// <summary>
    /// A class for search job's in all disks.
    /// </summary>
    public static class SearchEngine
    {
        private static Boolean searchComplete = false;
        public static Boolean SearchComplete
        { get { return searchComplete; } }

        public static List<FileInfo> LastSearchResult;

        /// <summary>
        /// throw any report from this class by event.
        /// </summary>
        public static event EventHandler<ReportEventArgs> Reporter = delegate { };

        /// <summary>
        /// Search Asynchrony many extension in all of Fixed and Removable Disks.
        /// </summary>
        /// <param name="targetExtensions">Some file extensions for use search pattern.</param>
        /// <example>
        /// FileExtension example:
        ///     {".jpg", 646546Byte, 646Byte}
        ///     {".pdf", 25464645546Byte, 60000Byte}
        /// </example>
        /// <returns>A sorted list of detected files</returns>
        public static async Task<List<FileInfo>> DiskParallelProbingAsync(List<FileExtensionOption> targetExtensions, System.Threading.CancellationTokenSource CTS)
        {
            return await Task.Run(() =>
                {
                    searchComplete = false;
                    //
                    Reporter("DiskProbing", new ReportEventArgs("DiskProbing", ReportCodes.DiskProbingStarted, "---{Search Disks Started}---"));

                    List<FileInfo> _result = new List<FileInfo>();
                    //
                    // Find specific folders from windows drives instead of the total drive.
                    //
                    FolderInfo[] SpecificsDirectory = CheckDirectoriesChanges.GetDirectoriesInformation();
                    //
                    // Set Data-flow 
                    //
                    TransformBlock<FolderInfo, List<FileInfo>> TB = new TransformBlock<FolderInfo, List<FileInfo>>(dir =>
                    {
                        Reporter(dir, new ReportEventArgs("DiskProbing",
                            ReportCodes.TheSearchBeginning,
                            "Searching  {0} ...", dir.FullName));

                        List<FileInfo> res = dir.GetDirectoryInfo.SearchDirectory(targetExtensions, CTS);

                        Reporter(dir, new ReportEventArgs("DiskProbing",
                            ReportCodes.TheSearchCompleted,
                            "The Search  {0} was completed!", dir.FullName));

                        return res;
                    }, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount });

                    ActionBlock<List<FileInfo>> AB = new ActionBlock<List<FileInfo>>(lst => _result.AddRange(lst));

                    //
                    // Search specific folders from windows drives instead of the total drive.
                    //
                    try
                    {
                        TB.LinkTo(AB);

                        ParallelOptions opt = new ParallelOptions() { CancellationToken = CTS.Token, MaxDegreeOfParallelism = Environment.ProcessorCount };
                        var pLoop = Parallel.ForEach(SpecificsDirectory, opt, async dir => await TB.SendAsync(dir));

                        TB.Complete();
                        TB.Completion.Wait();
                    }
                    catch (Exception ex) { Reporter(SpecificsDirectory, new ReportEventArgs("SearchEngine.DiskProbing.SpecificsDirectory", ex)); }



                    searchComplete = true;
                    Reporter("DiskProbing", new ReportEventArgs("DiskProbing",
                        ReportCodes.DiskProbingFinished,
                        "---{Search Disks Finished}---"));

                    LastSearchResult = _result;
                    return _result;
                });
        }

        /// <summary>
        /// Search many extension in all of Fixed and Removable Disks.
        /// </summary>
        /// <param name="targetExtensions">Some file extensions for use search pattern.</param>
        /// <example>
        /// FileExtension example:
        ///     {".jpg", 646546Byte, 646Byte}
        ///     {".pdf", 25464645546Byte, 60000Byte}
        /// </example>
        /// <returns>A sorted list of detected files</returns>
        public static List<FileInfo> DiskProbing(List<FileExtensionOption> targetExtensions, System.Threading.CancellationTokenSource CTS)
        {
            searchComplete = false;
            //
            Reporter("DiskProbing", new ReportEventArgs("DiskProbing",
                ReportCodes.DiskProbingStarted,
                "---{Search Disks Started}---"));

            List<FileInfo> _result = new List<FileInfo>();
            //
            // Find specific folders from windows drives instead of the total drive.
            //
            FolderInfo[] SpecificsDirectory = CheckDirectoriesChanges.GetDirectoriesInformation();
            //
            // Search specific folders from windows drives instead of the total drive.
            //
            try
            {
                foreach (FolderInfo dir in SpecificsDirectory)
                {
                    Reporter(dir, new ReportEventArgs("DiskProbing",
                        ReportCodes.TheSearchBeginning,
                        "Searching  {0} ...", dir.FullName));

                    List<FileInfo> res = dir.GetDirectoryInfo.SearchDirectory(targetExtensions, CTS);

                    Reporter(dir, new ReportEventArgs("DiskProbing",
                        ReportCodes.TheSearchCompleted,
                        "The Search  {0} was completed!", dir.FullName));

                    _result.AddRange(res);
                }
            }
            catch (Exception ex) { Reporter(SpecificsDirectory, new ReportEventArgs("SearchEngine.DiskProbing.SpecificsDirectory", ex)); }


            searchComplete = true;
            Reporter("DiskProbing", new ReportEventArgs("DiskProbing",
                ReportCodes.DiskProbingFinished,
                "---{Search Disks Finished}---")); 

            return _result;
        }


        /// <summary>
        /// Search Any Directory Names in a Drive.
        /// Then get them by a TreeNode.
        /// </summary>
        /// <returns>Return all Directories Name in a TreeNode.</returns>
        public static TreeNode DriveTreeTraverse(this DriveInfo drive, System.Threading.CancellationTokenSource CTS)
        {
            // Create Master Node Names:
            string NodeName = string.IsNullOrEmpty(drive.VolumeLabel) ?
                drive.Name :
                drive.Name.Replace(@"\", string.Format(@"[{0}]", drive.VolumeLabel));

            // Create Master Node for TreeViewer
            TreeNode Node = new TreeNode(NodeName);

            // If cancellation requested from parent task...
            if (CTS.IsCancellationRequested) return Node;

            // If the drive is ready for read that directories
            if (drive.IsReady)
            {
                var dir = new DirectoryInfo(drive.Name);
                dir.PopulateTree(Node, CTS);
            }

            // Return result
            return Node;
        }

        /// <summary>
        /// Get Size of directory by all sub directory and files.
        /// </summary>
        /// <param name="dir">The Directory</param>
        /// <returns>Size (bytes) of Directory</returns>
        public static long GetDirectorySize(this DirectoryInfo dir)
        {
            long sum = 0;

            // get IEnumerable from all files in the current directory and all sub directories
            try
            {
                var subDirectories = dir.GetDirectories()
                        .Where(d => (d.Attributes & FileAttributes.ReparsePoint) == 0).AsParallel();

                Parallel.ForEach(subDirectories, d =>
                {
                    long value = d.GetDirectorySize();
                    System.Threading.Interlocked.Add(ref sum, value);
                });

                // get the size of all files
                sum += (from file in dir.GetFiles() select file.Length).Sum();
            }
            catch (Exception ex)
            { Reporter(dir, new ReportEventArgs("SearchEngine.GetDirectorySize", ex)); }


            return sum;
        }

        /// <summary>
        /// A method to populate a TreeView with directories and subdirectories.
        /// </summary>
        /// <param name="directory">The path of the directory</param>
        /// <param name="Node">The "master" node, to populate</param>
        public static void PopulateTree(this DirectoryInfo directory, TreeNode Node, System.Threading.CancellationTokenSource CTS)
        {
            if (CTS.IsCancellationRequested) return;

            try // Try to access Directory without Permission (Faster)
            {
                // Get any directories by except specific directory
                var subDirectories = (from d in directory.GetDirectories()
                                      where ((d.Attributes & FileAttributes.ReparsePoint) == 0 &&
                                              d.Name.ToLower() != "System Volume Information".ToLower() &&
                                              d.Name.ToLower() != "$RECYCLE.BIN".ToLower() &&
                                              d.Name.ToLower() != "RECYCLER".ToLower())
                                      select d).AsParallel().ToList();

                foreach (var dir in subDirectories.AsParallel())
                {
                    // create a new node
                    TreeNode t = new TreeNode(dir.Name);

                    // populate the new node recursively
                    PopulateTree(dir, t, CTS);

                    // Add this to "master" node
                    Node.Nodes.Add(t);
                }
            }
            catch (Exception ex) { Reporter(directory, new ReportEventArgs("SearchEngine.PopulateTree", ex)); }
        }

        /// <summary>
        /// Search many extension in all of Fixed and Removable Disks.
        /// </summary>
        /// <param name="dir">A directory for search on it.</param>
        /// <param name="targetExtensions">Some file extensions for use search pattern.</param>
        /// <example>
        /// FileExtension:
        ///     {".jpg", 646546Byte, 646Byte}
        ///     {".pdf", 25464645546Byte, 60000Byte}
        /// </example>
        /// <returns>A sorted list of detected files</returns>
        public static List<FileInfo> SearchDirectory(this DirectoryInfo dir, List<FileExtensionOption> targetExtensions, CancellationTokenSource CTS)
        {
            List<FileInfo> _result = new List<FileInfo>();
            if (CTS.IsCancellationRequested) return _result;

            try // Try to access Directory without Permission (Faster)
            {
                // Get any directories by except specific directory
                var subDirectories = (from d in dir.GetDirectories()
                                      where ((d.Attributes & FileAttributes.ReparsePoint) == 0 &&
                                              d.Name.ToLower() != "System Volume Information".ToLower() &&
                                              d.Name.ToLower() != "$RECYCLE.BIN".ToLower() &&
                                              d.Name.ToLower() != "RECYCLER".ToLower())
                                      select d).AsParallel().ToList();

                foreach (var subDir in subDirectories)
                {
                    // find subDirectory and files
                    _result.AddRange(subDir.SearchDirectory(targetExtensions, CTS));
                }
            }
            catch (Exception ex) { Reporter(dir, new ReportEventArgs("SearchEngine.SearchDirectory", ex)); }
            finally
            {
                // lastly, loop through each file in the directory, and add these as nodes
                try
                {
                    // Get array of EXTENSION files.
                    //string[] a = Directory.GetFiles(p, "*.extension");
                    foreach (var file in dir.GetFiles())
                    {
                        //
                        // Match the file.Extension by extension list
                        List<FileExtensionOption> iListFindedExts = targetExtensions.Where(x => x.ExtensionName == file.Extension.ToLower()).ToList();
                        if (iListFindedExts.Count() > 0)
                        {
                            bool AcceptedFile = false;
                            //
                            foreach (var opt in iListFindedExts)
                            {
                                if (file.Length > opt.MinSizeLimit && file.Length < opt.MaxSizeLimit)
                                {
                                    if (opt.DateSensitive)
                                    {
                                        // in date-sensitive files that last access time is important for us
                                        if (file.LastAccessTimeUtc != null && DateTime.UtcNow.Subtract(file.LastAccessTimeUtc).TotalDays < opt.MaxIntervalFromLastAccessTime)
                                        { AcceptedFile = true; break; }
                                    }
                                    else { AcceptedFile = true; break; }
                                }
                            }
                            //
                            if (AcceptedFile) _result.Add(file);
                        }
                    }
                }
                catch (Exception ex) { Reporter(dir, new ReportEventArgs("SearchEngine.SearchDirectory", ex)); }
            }
            return _result;
        }

        /// <summary>
        /// Search many extension in all of Fixed and Removable Disks.
        /// </summary>
        /// <param name="drive">A drive for search on it.</param>
        /// <param name="targetExtensions">Some file extensions for use search pattern.</param>
        /// <example>
        /// FileExtension:
        ///     {".jpg", 646546Byte, 646Byte}
        ///     {".pdf", 25464645546Byte, 60000Byte}
        /// </example>
        /// <returns>A sorted list of detected files</returns>
        public static List<FileInfo> SearchDrive(this DriveInfo drive, List<FileExtensionOption> targetExtensions, CancellationTokenSource CTS)
        {
            DirectoryInfo dir = new DirectoryInfo(drive.Name);
            return dir.SearchDirectory(targetExtensions, CTS);
        }
    }
}
