using BucketBox.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BucketBox.OS
{
    public class Wallpaper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(
    UInt32 action, UInt32 uParam, String vParam, UInt32 winIni);

        private static readonly UInt32 SPI_SETDESKWALLPAPER = 0x14;
        private static readonly UInt32 SPIF_UPDATEINIFILE = 0x01;
        private static readonly UInt32 SPIF_SENDWININICHANGE = 0x02;
        private static readonly UInt32 SPI_GETDESKWALLPAPER = 0x73;
        private static readonly int MAX_PATH = 260;
        private static readonly UInt32 WM_SETTINGCHANGE = 0x1;
        FileSystem fileSystem = new FileSystem();



        public void SetWallpaper(String path)
        {
            try
            {
                if (path != null && fileSystem.FileExists(path)==true)
                {
                    SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path,
                        SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
                }
            }
            catch (Exception e)
            {

                Base.exceptionHandle(e);
                

            }
        }
        public String GetWallpaper()
        {
            try
            {
                String wallpaper = new String('\0', MAX_PATH);
                SystemParametersInfo(SPI_GETDESKWALLPAPER,
                    (UInt32)wallpaper.Length, wallpaper, 0);
                wallpaper = wallpaper.Substring(0, wallpaper.IndexOf('\0'));
                return wallpaper;
            }
            catch (Exception e)
            {

                Base.exceptionHandle(e);
                return null;


            }
        }
        public Boolean DetectWallpaperChange(ref Message message)
        {
            try
            {
                Boolean ap = false;
                if (message.Msg == WM_SETTINGCHANGE)
                {
                    if (message.WParam.ToInt32() == SPI_SETDESKWALLPAPER)
                    {
                        ap = true;
                        // Handle that wallpaper has been changed.
                    }
                }
                return ap;
            }

           // base.WndProc(ref message);
             catch (Exception e)
            {

                Base.exceptionHandle(e);
                return false;
              


            }
        }
    }
}
