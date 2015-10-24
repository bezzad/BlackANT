using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace SolRegistry
{
    /// <summary>
    /// All Registry Entry store in Current User Type
    /// Path of Current User - HKEY_CURRENT_USER
    /// 
    /// Registry Element	Size Limit
    /// Key name	        255 characters. The key name includes the absolute path of the key in the registry, 
    ///                     always starting at a base key, for example, HKEY_LOCAL_MACHINE.
    /// Value name	        16,383 characters
    ///                     Windows 2000:  260 ANSI characters or 16,383 Unicode characters.
    /// Value	            Available memory (latest format)
    ///                     1 MB (standard format)
    /// Tree	            A registry tree can be 512 levels deep.
    ///                     You can create up to 32 levels at a time through a single registry API call.
    /// </summary>
    public static class RegistryHelper
    {
        #region Field

        private static RegistryKey RegistryKeyObj = null;
        private static RegistryKey RKP = Registry.CurrentUser;
        #endregion

        #region Methods

        /// <summary>
        /// Creates a registry entry at the given path with given key and value
        /// </summary>
        /// <param name="Path">Specify the Path to Create Registry Entry</param>
        /// <param name="Key">Specify Name of the Key</param>
        /// <param name="Value">Specify Value of the Key</param>
        /// <returns>Boolean</returns>
        public static Boolean Write(String Path, String Key, String Value)
        {
            Boolean Flag = false;
            try
            {
                // Create a New SubKey
                RKP.CreateSubKey(Path);
                // Open a Sub Key for Write Value
                RegistryKeyObj = RKP.OpenSubKey(Path, true);
                // Set the Specified Key and Value
                RegistryKeyObj.SetValue(Key, Value);
                
                Flag = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                RKP.Dispose();
                RegistryKeyObj.Dispose();
                RegistryKeyObj = null;
            }

            return Flag;
        }

        /// <summary>
        /// Creates a registry entry at the given path with given key and value
        /// </summary>
        /// <param name="Path">Specify the Path to Create Registry Entry</param>
        /// <param name="Key">Specify Name of the Key</param>
        /// <param name="Value">Specify Value of the Key</param>
        /// <returns>Boolean</returns>
        public static Boolean Write(String Path, String Key, String[] Value)
        {
            Boolean Flag = false;
            try
            {
                // Create a New SubKey
                RKP.CreateSubKey(Path);
                // Open a Sub Key for Write Value
                RegistryKeyObj = RKP.OpenSubKey(Path, true);
                // Set the Specified Key and Value
                RegistryKeyObj.SetValue(Key, Value, RegistryValueKind.MultiString);

                Flag = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                RKP.Dispose();
                RegistryKeyObj.Dispose();
                RegistryKeyObj = null;
            }

            return Flag;
        }


        /// <summary>
        /// Reads the registry value at the given path with the given key name
        /// </summary>
        /// <typeparam name="T">Specify the Return Type</typeparam>
        /// <param name="Path">Specify the Path for Read Value</param>
        /// <param name="Key">Specify the Key</param>
        /// <returns>T</returns>
        public static T Read<T>(String Path, String Key)
        {
            T TObj = default(T);
            try
            {
                //  Open a Sub Key for Read Value
                RegistryKeyObj = RKP.OpenSubKey(Path, false);
                // Get Value by given Key
                TObj = (T)RegistryKeyObj.GetValue(Key);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                RKP.Dispose();
                RegistryKeyObj.Dispose();
                RegistryKeyObj = null;
            }

            return TObj;
        }

        /// <summary>
        /// Deletes the registry entry at the given path
        /// </summary>
        /// <param name="Path">Specify the Path</param>
        /// <returns>Boolean</returns>
        public static Boolean Delete(String Path)
        {
            Boolean Flag = false;
            try
            {
                // Delete the Specified Sub Key
                RKP.DeleteSubKey(Path);
                Flag = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                RKP.Dispose();
            }

            return Flag;
        }

        /// <summary>
        ///  Deletes the registry entry at the given Key
        /// </summary>
        /// <param name="Path">Specify the Path</param>
        /// <param name="Key">Specify the Key</param>
        /// <returns>Boolean</returns>
        public static Boolean Delete(String Path, String Key)
        {
            Boolean Flag = false;
            try
            {
                //  Open a Sub Key for Delete Value
                RegistryKeyObj = RKP.OpenSubKey(Path, true);
                // Delete the Specified Value from this Key
                RegistryKeyObj.DeleteValue(Key);
                Flag = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                RKP.Dispose();
                RegistryKeyObj.Dispose();
                RegistryKeyObj = null;
            }

            return Flag;
        }

        #endregion
    }
}
