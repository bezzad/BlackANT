using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Globalization;
using FileExtension.RegisteredFileTypes;

namespace FileExtension
{
    public static class WinExtensions
    {
        public static event EventHandler DatabaseLoaded = delegate { };

        public static CollectionCommonFileType CollectionCommonFileTypes { get; set; }

        public static void LoadDatabase()
        {
            if (CollectionCommonFileTypes == null)
                CollectionCommonFileTypes = new CollectionCommonFileType();

            if (CollectionCommonFileTypes.Count < 28) // is load completed or not?
            {
                CollectionCommonFileTypes.DatabaseLoaded += (source, e) => DatabaseLoaded(source, e);
            }
            else
                DatabaseLoaded(CollectionCommonFileTypes, new EventArgs()); // Load completed before run this method
        }

        /// <summary>
        /// Shows the icon associates with a specific file type.
        /// </summary>
        /// <param name="fileType">The type of file (or file extension).</param>
        public static Bitmap GetIcon(string fileType)
        {
            Bitmap fileTypeIcon = null;
            try
            {
                Hashtable iconsInfo = RegisteredFileType.GetFileTypeAndIcon();
                string fileAndParam = "";

                if (iconsInfo.ContainsKey(fileType))
                    fileAndParam = (iconsInfo[fileType]).ToString();

                if (String.IsNullOrEmpty(fileAndParam))
                    return fileTypeIcon;

                Icon icon = null;

                icon = RegisteredFileType.ExtractIconFromFile(fileAndParam, true);

                //The icon cannot be zero.
                if (icon != null)
                {
                    //Draw the icon to the picture box.
                    fileTypeIcon = icon.ToBitmap();
                }
                else //if the icon is invalid, show an error image.
                    return fileTypeIcon;
            }
            catch (Exception) { }

            return fileTypeIcon;
        }

        /// <summary>
        /// Shows the icon associates with a specific file type.
        /// </summary>
        /// <param name="fileType">The type of file (or file extension).</param>
        public static Bitmap GetIcon(this FileType fileType)
        {
            Bitmap fileTypeIcon = null;
            try
            {
                Hashtable iconsInfo = RegisteredFileType.GetFileTypeAndIcon();
                string fileAndParam = "";

                if (iconsInfo.ContainsKey(fileType.ExtensionName))
                    fileAndParam = (iconsInfo[fileType.ExtensionName]).ToString();

                if (String.IsNullOrEmpty(fileAndParam))
                    return fileTypeIcon;

                Icon icon = null;

                icon = RegisteredFileType.ExtractIconFromFile(fileAndParam, true);

                //The icon cannot be zero.
                if (icon != null)
                {
                    //Draw the icon to the picture box.
                    fileTypeIcon = icon.ToBitmap();
                }
                else //if the icon is invalid, show an error image.
                    return fileTypeIcon;
            }
            catch (Exception) { }

            return fileTypeIcon;
        }

        public static string CalcMemoryMensurableUnit(System.Numerics.BigInteger BigUnSignedNumber)
        {
            #region Guide
            /// . 1 Bit = Binary Digit
            /// · 8 Bits = 1 Byte
            /// · 1024 Bytes = 1 Kilobyte 
            /// · 1024 Kilobytes = 1 Megabyte 
            /// · 1024 Megabytes = 1 Gigabyte 
            /// · 1024 Gigabytes = 1 Terabyte 
            /// · 1024 Terabytes = 1 Petabyte 
            /// · 1024 Petabytes = 1 Exabyte
            /// · 1024 Exabytes = 1 Zettabyte 
            /// · 1024 Zettabytes = 1 Yottabyte 
            /// · 1024 Yottabytes = 1 Brontobyte
            /// · 1024 Brontobytes = 1 Geopbyte
            /// . Saganbyte = 1024 Geopbyte 
            /// . Pijabyte = 1024 Saganbyte 
            /// . Alphabyte = 1024 Pijabyte 
            /// . Kryatbyte = 1024 Alphabyte 
            /// . Amosbyte = 1024 Kryatbyte 
            /// . Pectrolbyte = 1024 Amosbyte 
            /// . Bolgerbyte = 1024 Pectrolbyte 
            /// . Sambobyte = 1024 Bolgerbyte 
            /// . Quesabyte = 1024 Sambobyte 
            /// . Kinsabyte = 1024 Quesabyte 
            /// . Rutherbyte = 1024 Kinsabyte 
            /// . Dubnibyte = 1024 Rutherbyte 
            /// . Seaborgbyte = 1024 Dubnibyte 
            /// . Bohrbyte = 1024 Seaborgbyte 
            /// . Hassiubyte = 1024 Bohrbyte 
            /// . Meitnerbyte = 1024 Hassiubyte 
            /// . Darmstadbyte = 1024 Meitnerbyte 
            /// . Roentbyte = 1024 Darmstadbyte 
            /// . Coperbyte = 1024 Roentbyte 
            /// . Koentekbyte = 1024 Coperbyte 
            /// . Silvanikbyte = 1024 Koentekbyte 
            /// . Golvanikbyte = 1024 Silvanikbyte 
            /// . Platvanikbyte = 1024 Golvanikbyte 
            /// . Einstanikbyte = 1024 Platvanikbyte 
            /// . Emeranikbyte = 1024 Einstanikbyte 
            /// . Rubanikbyte = 1024 Emeranikbyte 
            /// . Diamonikbyte = 1024 Rubanikbyte 
            /// . Amazonikbyte = 1024 Diamonikbyte 
            /// . Nilevanikbyte = 1024 Amazonikbyte 
            /// . Infinitybyte = 1024 Nilevanikbyte 
            /// . Websitebyte = 1024 Infinitybyte
            /// 
            #endregion

            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            var KB = BigUnSignedNumber / 1024; // · 1024 Bytes = 1 Kilobyte 
            var MB = KB / 1024; // · 1024 Kilobytes = 1 Megabyte 
            var GB = MB / 1024; // · 1024 Megabytes = 1 Gigabyte 
            var TB = GB / 1024; // · 1024 Gigabytes = 1 Terabyte 
            var PB = TB / 1024; // · 1024 Terabytes = 1 Petabyte 
            var EB = PB / 1024; // · 1024 Petabytes = 1 Exabyte
            var ZB = EB / 1024; // · 1024 Exabytes = 1 Zettabyte 
            var YB = ZB / 1024; // · 1024 Zettabytes = 1 Yottabyte 
            var BB = YB / 1024; // · 1024 Yottabytes = 1 Brontobyte
            var GeoB = BB / 1024; // · 1024 Brontobytes = 1 Geopbyte
            var SaganB = GeoB / 1024; // . Saganbyte = 1024 Geopbyte
            var PijaB = SaganB / 1024; // . Pijabyte = 1024 Saganbyte 
            var AlphaB = PijaB / 1024; // . Alphabyte = 1024 Pijabyte 
            var KryatB = AlphaB / 1024; // . Kryatbyte = 1024 Alphabyte 
            var AmosB = KryatB / 1024; // . Amosbyte = 1024 Kryatbyte 
            var PectrolB = AmosB / 1024; // . Pectrolbyte = 1024 Amosbyte
            var BolgerB = PectrolB / 1024; // . Bolgerbyte = 1024 Pectrolbyte 
            var SamboB = BolgerB / 1024; // . Sambobyte = 1024 Bolgerbyte
            var QuesaB = SamboB / 1024; // . Quesabyte = 1024 Sambobyte 
            var KinsaB = QuesaB / 1024; // . Kinsabyte = 1024 Quesabyte 
            var RutherB = KinsaB / 1024; // . Rutherbyte = 1024 Kinsabyte 
            var DubniB = RutherB / 1024; // . Dubnibyte = 1024 Rutherbyte 
            var SeaborgB = DubniB / 1024; // . Seaborgbyte = 1024 Dubnibyte 
            var BohrB = SeaborgB / 1024; // . Bohrbyte = 1024 Seaborgbyte 
            var HassiuB = BohrB / 1024; // . Hassiubyte = 1024 Bohrbyte 
            var Meitnerbyte = HassiuB / 1024; // . Meitnerbyte = 1024 Hassiubyte
            var Darmstadbyte = Meitnerbyte / 1024; // . Darmstadbyte = 1024 Meitnerbyte
            var Roentbyte = Darmstadbyte / 1024; // . Roentbyte = 1024 Darmstadbyte
            var Coperbyte = Roentbyte / 1024; // . Coperbyte = 1024 Roentbyte 
            var Koentekbyte = Coperbyte / 1024; // . Koentekbyte = 1024 Coperbyte 
            var Silvanikbyte = Koentekbyte / 1024; // . Silvanikbyte = 1024 Koentekbyte 
            var Golvanikbyte = Silvanikbyte / 1024; // . Golvanikbyte = 1024 Silvanikbyte 
            var Platvanikbyte = Golvanikbyte / 1024; // . Platvanikbyte = 1024 Golvanikbyte 
            var Einstanikbyte = Platvanikbyte / 1024; // . Einstanikbyte = 1024 Platvanikbyte 
            var Emeranikbyte = Einstanikbyte / 1024; // . Emeranikbyte = 1024 Einstanikbyte 
            var Rubanikbyte = Emeranikbyte / 1024; // . Rubanikbyte = 1024 Emeranikbyte 
            var Diamonikbyte = Rubanikbyte / 1024; // . Diamonikbyte = 1024 Rubanikbyte 
            var Amazonikbyte = Diamonikbyte / 1024; // . Amazonikbyte = 1024 Diamonikbyte 
            var Nilevanikbyte = Amazonikbyte / 1024; // . Nilevanikbyte = 1024 Amazonikbyte 
            var Infinitybyte = Nilevanikbyte / 1024; // . Infinitybyte = 1024 Nilevanikbyte 
            var Websitebyte = Infinitybyte / 1024; // . Websitebyte = 1024 Infinitybyte

            return Websitebyte > 1 ? String.Format(culture, "{0:N0} Websitebyte", Websitebyte) :
                   Infinitybyte > 1 ? String.Format(culture, "{0:N0} Infinitybyte", Infinitybyte) :
                   Nilevanikbyte > 1 ? String.Format(culture, "{0:N0} Nilevanikbyte", Nilevanikbyte) :
                   Amazonikbyte > 1 ? String.Format(culture, "{0:N0} Amazonikbyte", Amazonikbyte) :
                   Diamonikbyte > 1 ? String.Format(culture, "{0:N0} Diamonikbyte", Diamonikbyte) :
                   Rubanikbyte > 1 ? String.Format(culture, "{0:N0} Rubanikbyte", Rubanikbyte) :
                   Emeranikbyte > 1 ? String.Format(culture, "{0:N0} Emeranikbyte", Emeranikbyte) :
                   Einstanikbyte > 1 ? String.Format(culture, "{0:N0} Einstanikbyte", Einstanikbyte) :
                   Platvanikbyte > 1 ? String.Format(culture, "{0:N0} Platvanikbyte", Platvanikbyte) :
                   Golvanikbyte > 1 ? String.Format(culture, "{0:N0} Golvanikbyte", Golvanikbyte) :
                   Silvanikbyte > 1 ? String.Format(culture, "{0:N0} Silvanikbyte", Silvanikbyte) :
                   Koentekbyte > 1 ? String.Format(culture, "{0:N0} Koentekbyte", Koentekbyte) :
                   Coperbyte > 1 ? String.Format(culture, "{0:N0} Coperbyte", Coperbyte) :
                   Roentbyte > 1 ? String.Format(culture, "{0:N0} Roentbyte", Roentbyte) :
                   Darmstadbyte > 1 ? String.Format(culture, "{0:N0} Darmstadbyte", Darmstadbyte) :
                   Meitnerbyte > 1 ? String.Format(culture, "{0:N0} Meitnerbyte", Meitnerbyte) :
                   HassiuB > 1 ? String.Format(culture, "{0:N0} Hassiubyte", HassiuB) :
                   BohrB > 1 ? String.Format(culture, "{0:N0} Bohrbyte", BohrB) :
                   SeaborgB > 1 ? String.Format(culture, "{0:N0} Seaborgbyte", SeaborgB) :
                   DubniB > 1 ? String.Format(culture, "{0:N0} Dubnibyte", DubniB) :
                   RutherB > 1 ? String.Format(culture, "{0:N0} Rutherbyte", RutherB) :
                   KinsaB > 1 ? String.Format(culture, "{0:N0} Kinsabyte", KinsaB) :
                   QuesaB > 1 ? String.Format(culture, "{0:N0} Quesabyte", QuesaB) :
                   SamboB > 1 ? String.Format(culture, "{0:N0} Sambobyte", SamboB) :
                   BolgerB > 1 ? String.Format(culture, "{0:N0} Bolgerbyte", BolgerB) :
                   PectrolB > 1 ? String.Format(culture, "{0:N0} Pectrolbyte", PectrolB) :
                   AmosB > 1 ? String.Format(culture, "{0:N0} Amosbyte", AmosB) :
                   KryatB > 1 ? String.Format(culture, "{0:N0} Kryatbyte", KryatB) :
                   AlphaB > 1 ? String.Format(culture, "{0:N0} Alphabyte", AlphaB) :
                   PijaB > 1 ? String.Format(culture, "{0:N0} Pijabyte", PijaB) :
                   SaganB > 1 ? String.Format(culture, "{0:N0} Saganbyte", SaganB) :
                   GeoB > 1 ? String.Format(culture, "{0:N0} Geopbyte", GeoB) :
                   BB > 1 ? String.Format(culture, "{0:N0} Brontobytes", BB) :
                   YB > 1 ? String.Format(culture, "{0:N0} Yottabytes", YB) :
                   ZB > 1 ? String.Format(culture, "{0:N0} Zettabytes", ZB) :
                   EB > 1 ? String.Format(culture, "{0:N0} Exabytes", EB) :
                   PB > 1 ? String.Format(culture, "{0:N0} Petabytes", PB) :
                   TB > 1 ? String.Format(culture, "{0:N0} Terabytes", TB) :
                   GB > 1 ? String.Format(culture, "{0:N0} Gigabytes", GB) :
                   MB > 1 ? String.Format(culture, "{0:N0} Megabytes", MB) :
                   KB > 1 ? String.Format(culture, "{0:N0} Kilobytes", KB) :
                   String.Format(culture, "{0:N0} Bytes", BigUnSignedNumber);
        }

        public static string CalcMemoryMensurableUnit(string strUnSignedNumber)
        {
            System.Numerics.BigInteger value;
            // Parse currency value using en-GB culture. 
            // value = "�1,097.63";
            // Displays:  
            //       Converted '�1,097.63' to 1097.63
            var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            if (System.Numerics.BigInteger.TryParse(strUnSignedNumber, style, culture, out value))
            {
                return CalcMemoryMensurableUnit(value);
            }
            return string.Empty;
        }
    }
}
