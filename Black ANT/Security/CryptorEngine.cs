using System;
using System.Text;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace Security
{
    /// <summary>
    /// Provider	            Length (bits)	Security	Speed
    /// Hash.Provider.CRC32	    32	            low	        fast
    /// Hash.Provider.SHA1	    160	            moderate	medium
    /// Hash.Provider.SHA256	256	            high	    slow
    /// Hash.Provider.SHA384	384	            high	    slow
    /// Hash.Provider.SHA512	512	            extreme	    slow
    /// Hash.Provider.MD5	    128	            moderate	medium 
    /// </summary>
    public static class CryptorEngine
    {
        ///  Call this function to remove the key from memory after use for security
        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);

        /// <summary>
        /// Function to Generate a 64 bits Key.
        /// </summary>
        /// <returns></returns>
        public static string Generate64BitKey()
        {
            // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

            // Use the Automatically generated key for Encryption. 
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }

        /// <summary>
        /// This constant is used to determine the keysize of the encryption algorithm.
        /// </summary>
        private const int keysize = 256;

        public static void EncryptFile(string sInputFilename, string sOutputFilename, string PassKey, bool useHashing)
        {
            try
            {
                // Create a file stream to read the file data.
                string toEncrypt = File.ReadAllText(sInputFilename, Encoding.Default);

                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(PassKey));
                    hashmd5.Clear();
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(PassKey);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();

                string Encrypted = Convert.ToBase64String(resultArray, 0, resultArray.Length);

                //Create a file stream to write the encrypted data.
                File.WriteAllText(sOutputFilename, Encrypted, Encoding.Default);
            }
            catch (System.Security.SecurityException se)
            {
                System.Windows.Forms.MessageBox.Show(se.Message, se.Source);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, ex.Source);
            }
        }

        public static void DecryptFile(string sInputFilename, string sOutputFilename, string PassKey, bool useHashing)
        {
            try
            {
                // Create a file stream to read the encrypted file back.
                string cipherString = File.ReadAllText(sInputFilename, Encoding.Default);

                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(cipherString);

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(PassKey));
                    hashmd5.Clear();
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(PassKey);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                tdes.Clear();
                string decrypted = UTF8Encoding.UTF8.GetString(resultArray);

                //Create a file stream to write the decrypted data.
                File.WriteAllText(sOutputFilename, decrypted, Encoding.Default);
            }
            catch (System.Security.SecurityException se)
            {
                System.Windows.Forms.MessageBox.Show(se.Message, se.Source);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, ex.Source);
            }
        }

        /// <summary>
        /// Encrypt a string using dual encryption method. Return a encrypted cipher Text
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra security</param>
        /// <returns></returns>
        public static string Encrypt(this string toEncrypt, string PassKey, bool useHashing)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(PassKey));
                    hashmd5.Clear();
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(PassKey);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (System.Security.SecurityException se)
            {
                System.Windows.Forms.MessageBox.Show(se.Message, se.Source);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, ex.Source);
            }
            return null;
        }
        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        public static string Decrypt(this string cipherString, string PassKey, bool useHashing)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(cipherString);

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(PassKey));
                    hashmd5.Clear();
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(PassKey);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                tdes.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (System.Security.SecurityException se)
            {
                System.Windows.Forms.MessageBox.Show(se.Message, se.Source);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, ex.Source);
            }
            return null;
        }

        public static string GetMD5HashCode(this string inputString)
        {
            //Create a byte array from source data
            byte[] inputBytes = ASCIIEncoding.ASCII.GetBytes(inputString);
            //
            //Compute hash based on source data
            byte[] hashBytes = new MD5CryptoServiceProvider().ComputeHash(inputBytes);
            //
            // Convert the byte array to hexadecimal string
            //
            StringBuilder sOutput = new StringBuilder(hashBytes.Length);
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sOutput.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sOutput.Append(hashBytes[i].ToString("x2")); 
            }
            return sOutput.ToString();
        }

        public static string GetSHA256HashCode(this string inputString)
        {
            //Create a byte array from source data
            byte[] inputBytes = ASCIIEncoding.ASCII.GetBytes(inputString);
            //
            //Compute hash based on source data
            byte[] hashBytes = new SHA256CryptoServiceProvider().ComputeHash(inputBytes);
            //
            // Convert the byte array to hexadecimal string
            //
            StringBuilder sOutput = new StringBuilder(hashBytes.Length);
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sOutput.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sOutput.Append(hashBytes[i].ToString("x2")); 
            }
            return sOutput.ToString();
        }

        public static string GetSHA512HashCode(this string inputString)
        {
            //Create a byte array from source data
            byte[] inputBytes = ASCIIEncoding.ASCII.GetBytes(inputString);
            //
            //Compute hash based on source data
            byte[] hashBytes = new SHA512CryptoServiceProvider().ComputeHash(inputBytes);
            //
            // Convert the byte array to hexadecimal string
            //
            StringBuilder sOutput = new StringBuilder(hashBytes.Length);
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sOutput.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sOutput.Append(hashBytes[i].ToString("x2")); 
            }
            return sOutput.ToString();
        }
    }
}
