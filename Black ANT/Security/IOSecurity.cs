using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;

namespace Security
{
    public static class IOSecurity
    {
        public static event ErrorEventHandler ErrorOccured;
        public static FileAttributes SecureAttributes
        {
            get
            {
                return 
                  FileAttributes.Hidden | FileAttributes.System |
                  FileAttributes.ReadOnly | FileAttributes.ReparsePoint |
                  FileAttributes.Encrypted | FileAttributes.Compressed;
            }
        }
        public static FileAttributes NormalAttributes 
        { get { return FileAttributes.Archive | FileAttributes.Normal; } }


        /// <summary>
        /// Adds an ACL entry on the specified directory for the specified account.
        /// </summary>
        /// <param name="dir">The path of directory as DirectoryInfo</param>
        /// <param name="Account">User Domain Name + @"\" + UserName</param>
        /// <param name="Rights">Define a operate for your file request. Example: FullControl | Read | Write | ...</param>
        /// <param name="ControlType">Define what is type of control me needs to access the file. Example: Allow | Deny</param>
        public static void AddDirectorySecurity(this DirectoryInfo dir, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            try
            {
                // Get a DirectorySecurity object that represents the 
                // current security settings.
                System.Security.AccessControl.DirectorySecurity dSecurity = dir.GetAccessControl();

                // Add the FileSystemAccessRule to the security settings. 
                dSecurity.AddAccessRule(new FileSystemAccessRule(Account, Rights, ControlType));

                // Set the new access settings.
                dir.SetAccessControl(dSecurity);
            }
            catch (Exception ex) { ErrorOccured(dir, new ErrorEventArgs(ex)); }
        }

        /// <summary>
        /// Adds an ACL entry on the specified directory for the current user account.
        /// </summary>
        /// <param name="dir">The path of directory as DirectoryInfo</param>
        /// <param name="Rights">Define a operate for your file request. Example: FullControl | Read | Write | ...</param>
        /// <param name="ControlType">Define what is type of control me needs to access the file. Example: Allow | Deny</param>
        public static void AddDirectorySecurity(this DirectoryInfo dir, FileSystemRights Rights, AccessControlType ControlType)
        {
            string Account = Environment.UserDomainName + @"\" + Environment.UserName;
            dir.AddDirectorySecurity(Account, Rights, ControlType);
        }

        /// <summary>
        /// Removes an ACL entry on the specified directory for the specified account.
        /// </summary>
        /// <param name="dir">Directory Path as DirectoryInfo</param>
        public static void RemoveDirectorySecurity(this DirectoryInfo dir)
        {
            try
            {
                System.Security.AccessControl.DirectorySecurity directorySecurity = dir.GetAccessControl();
                AuthorizationRuleCollection rules = directorySecurity.GetAccessRules(true, false, typeof(System.Security.Principal.NTAccount));
                foreach (FileSystemAccessRule rule in rules)
                    directorySecurity.RemoveAccessRule(rule);

                Directory.SetAccessControl(dir.FullName, directorySecurity);
            }
            catch (Exception ex) { ErrorOccured(dir, new ErrorEventArgs(ex)); }
        }

        /// <summary>
        /// Forces a System.Security.SecurityException at run time if all callers higher
        ///     in the call stack have not been granted the permission specified by the current
        ///     instance.
        /// </summary>    
        /// <remarks>
        /// Exceptions:
        ///     System.Security.SecurityException:
        ///     A caller higher in the call stack does not have the permission specified
        ///     by the current instance.-or- A caller higher in the call stack has called
        ///     System.Security.CodeAccessPermission.Deny() on the current permission object.
        /// </remarks>
        /// <param name="path">The absolute path of the file or directory.</param>
        /// <param name="AllFiles">Permitted access to all files? true or false</param>
        public static void PermissionReadDirectory(this string path, bool AllFiles)
        {
            try
            {
                FileIOPermission FIOP = new FileIOPermission(FileIOPermissionAccess.Read, path);
                FIOP.Demand();
                FIOP.AllFiles = FileIOPermissionAccess.Read;
            }
            catch (Exception ex) { ErrorOccured(path, new ErrorEventArgs(ex)); }
        }

        /// <summary>
        /// Adds an ACL entry on the specified file for the specified account.
        /// </summary>
        /// <param name="file">The path of file as FileInfo</param>
        /// <param name="Account">User Domain Name + @"\" + UserName</param>
        /// <param name="Rights">Define a operate for your file request. Example: FullControl | Read | Write | ...</param>
        /// <param name="ControlType">Define what is type of control me needs to access the file. Example: Allow | Deny</param>
        public static void AddFileSecurity(this FileInfo file, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            try
            {
                // Get a FileSecurity object that represents the 
                // current security settings.
                System.Security.AccessControl.FileSecurity fSecurity = file.GetAccessControl();

                // Add the FileSystemAccessRule to the security settings. 
                fSecurity.AddAccessRule(new FileSystemAccessRule(Account, Rights, ControlType));

                // Set the new access settings.
                file.SetAccessControl(fSecurity);
            }
            catch (Exception ex) { ErrorOccured(file, new ErrorEventArgs(ex)); }
        }

        /// <summary>
        /// Adds an ACL entry on the specified file for the current user.
        /// </summary>
        /// <param name="file">The path of file as FileInfo</param>
        /// <param name="Rights">Define a operate for your file request. Example: FullControl | Read | Write | ...</param>
        /// <param name="ControlType">Define what is type of control me needs to access the file. Example: Allow | Deny</param>
        public static void AddFileSecurity(this FileInfo file, FileSystemRights Rights, AccessControlType ControlType)
        {
            string Account = Environment.UserDomainName + @"\" + Environment.UserName;
            file.AddFileSecurity(Account, Rights, ControlType);
        }

        /// <summary>
        /// Removes an ACL entry on the specified file for the specified account.
        /// </summary>
        /// <param name="file">File Path as FileInfo</param>
        public static void RemoveFileSecurity(this FileInfo file)
        {
            try
            {
                //get security access
                System.Security.AccessControl.FileSecurity fileSecurity = file.GetAccessControl();

                //remove any inherited access
                fileSecurity.SetAccessRuleProtection(true, false);

                //get any special user access
                AuthorizationRuleCollection rules = fileSecurity.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));

                //remove any special access
                foreach (FileSystemAccessRule rule in rules)
                    fileSecurity.RemoveAccessRuleAll(rule);

                File.SetAccessControl(file.FullName, fileSecurity);
            }
            catch (Exception ex) { ErrorOccured(file, new ErrorEventArgs(ex)); }
        }

        // Removes an ACL entry on the specified file for the specified account. 
        public static void RemoveFileSecurity(string fileName, string account,
            FileSystemRights rights, AccessControlType controlType)
        {

            // Get a FileSecurity object that represents the 
            // current security settings.
            FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Remove the FileSystemAccessRule from the security settings.
            fSecurity.RemoveAccessRule(new FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);

        }

        /// <summary>
        /// Set File Attributes to Secure as Hidden and System Files...
        /// </summary>
        /// <param name="file">The target file as FileInfo</param>
        public static void SecureAttributer(this FileInfo file)
        {
            try
            {
                //
                // Set file attribute's
                file.Attributes = SecureAttributes;
                //
                // Add File Security as System ---> Read and Run
                file.RemoveFileSecurity();
                
                file.AddFileSecurity("Authenticated Users", FileSystemRights.ReadAndExecute, AccessControlType.Allow);
            }
            catch(Exception ex)
            {
                ErrorOccured(file, new ErrorEventArgs(ex));
                throw ex;
            }
        }

        /// <summary>
        /// Set File Attributes to Normal as Archive...
        /// </summary>
        /// <param name="file">The target file as FileInfo</param>
        public static void NormalAttributer(this FileInfo file)
        {
            try
            {
                //
                // Add File Security as System ---> Read
                file.RemoveFileSecurity();

                file.AddFileSecurity(FileSystemRights.FullControl, AccessControlType.Allow);
                //
                // First set file attribute's
                file.Attributes = NormalAttributes;
            }
            catch (Exception ex)
            {
                ErrorOccured(file, new ErrorEventArgs(ex));
                throw ex;
            }
        }
    }
}
