using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BucketBox.Devices
{
    public class MonitorProfile
    {
        [Flags()]
        enum DisplayDeviceStateFlags : UInt32
        {
            // from: http://www.pinvoke.net/default.aspx/Enums/DisplayDeviceStateFlags.html
            // equvalent to defines from: wingdi.h (c:\Program Files (x86)\Windows Kits\10\Include\10.0.10240.0\um\wingdi.h)
            //#define DISPLAY_DEVICE_ATTACHED_TO_DESKTOP      0x00000001
            //#define DISPLAY_DEVICE_MULTI_DRIVER             0x00000002
            //#define DISPLAY_DEVICE_PRIMARY_DEVICE           0x00000004
            //#define DISPLAY_DEVICE_MIRRORING_DRIVER         0x00000008
            //#define DISPLAY_DEVICE_VGA_COMPATIBLE           0x00000010
            //#if (_WIN32_WINNT >= _WIN32_WINNT_WIN2K)
            //#define DISPLAY_DEVICE_REMOVABLE                0x00000020
            //#endif // (_WIN32_WINNT >= _WIN32_WINNT_WIN2K)
            //#if (_WIN32_WINNT >= _WIN32_WINNT_WIN8)
            //#define DISPLAY_DEVICE_ACC_DRIVER               0x00000040
            //#endif
            //#define DISPLAY_DEVICE_MODESPRUNED              0x08000000
            //#if (_WIN32_WINNT >= _WIN32_WINNT_WIN2K)
            //#define DISPLAY_DEVICE_REMOTE                   0x04000000
            //#define DISPLAY_DEVICE_DISCONNECT               0x02000000
            //#endif
            //#define DISPLAY_DEVICE_TS_COMPATIBLE            0x00200000
            //#if (_WIN32_WINNT >= _WIN32_WINNT_LONGHORN)
            //#define DISPLAY_DEVICE_UNSAFE_MODES_ON          0x00080000
            //#endif

            ///* Child device state */
            //#if (_WIN32_WINNT >= _WIN32_WINNT_WIN2K)
            //#define DISPLAY_DEVICE_ACTIVE              0x00000001
            //#define DISPLAY_DEVICE_ATTACHED            0x00000002
            //#endif // (_WIN32_WINNT >= _WIN32_WINNT_WIN2K)
            /// <summary>The device is part of the desktop.</summary>
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            /// <summary>The device is part of the desktop.</summary>
            PrimaryDevice = 0x4,
            /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
            MirroringDriver = 0x8,
            /// <summary>The device is VGA compatible.</summary>
            VGACompatible = 0x10,
            /// <summary>The device is removable; it cannot be the primary display.</summary>
            Removable = 0x20,
            /// <summary>The device has more display modes than its output devices support.</summary>
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000,

            /// <summary>Child device state: DISPLAY_DEVICE_ACTIVE</summary>
            Active = 0x1,
            /// <summary>Child device state: DISPLAY_DEVICE_ATTACHED</summary>
            Attached = 0x2
        }

        enum DeviceClassFlags : UInt32
        {
            // from: c:\Program Files (x86)\Windows Kits\10\Include\10.0.10240.0\um\Icm.h
            /// <summary>
            ///#define CLASS_MONITOR           'mntr' 
            /// </summary>
            CLASS_MONITOR = 0x6d6e7472,

            /// <summary>
            /// #define CLASS_PRINTER           'prtr'
            /// </summary>
            CLASS_PRINTER = 0x70727472,

            /// <summary>
            /// #define CLASS_SCANNER           'scnr'
            /// </summary>
            CLASS_SCANNER = 0x73636e72
        }

        enum WCS_PROFILE_MANAGEMENT_SCOPE : UInt32
        {
            // from: c:\Program Files (x86)\Windows Kits\10\Include\10.0.10240.0\um\Icm.h
#pragma warning disable CA1712 // Do not prefix enum values with type name
            WCS_PROFILE_MANAGEMENT_SCOPE_SYSTEM_WIDE,
#pragma warning restore CA1712 // Do not prefix enum values with type name
            WCS_PROFILE_MANAGEMENT_SCOPE_CURRENT_USER
        }

        enum COLORPROFILETYPE : UInt32
        {
            // from: c:\Program Files (x86)\Windows Kits\10\Include\10.0.10240.0\um\Icm.h
            CPT_ICC,
            CPT_DMP,
            CPT_CAMP,
            CPT_GMMP
        }

        enum COLORPROFILESUBTYPE : UInt32
        {
            // from: c:\Program Files (x86)\Windows Kits\10\Include\10.0.10240.0\um\Icm.h
            // intent
            CPST_PERCEPTUAL = 0,
            CPST_RELATIVE_COLORIMETRIC = 1,
            CPST_SATURATION = 2,
            CPST_ABSOLUTE_COLORIMETRIC = 3,

            // working space
            CPST_NONE,
            CPST_RGB_WORKING_SPACE,
            CPST_CUSTOM_WORKING_SPACE,
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct DISPLAY_DEVICE
        {
            // from: http://www.pinvoke.net/default.aspx/Structures/DISPLAY_DEVICE.html
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        const UInt32 EDD_GET_DEVICE_INTERFACE_NAME = 0x1;

        //BOOL EnumDisplayDevices(
        //  _In_ LPCTSTR         lpDevice,
        //  _In_ DWORD           iDevNum,
        //  _Out_ PDISPLAY_DEVICE lpDisplayDevice,
        //  _In_ DWORD           dwFlags
        //);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern UInt32 EnumDisplayDevices(string s, UInt32 iDevNum, ref DISPLAY_DEVICE displayDevice, UInt32 dwFlags);

        // from: c:\Program Files (x86)\Windows Kits\10\Include\10.0.10240.0\um\Icm.h
        //BOOL WINAPI WcsGetUsePerUserProfiles(
        //  _In_ LPCWSTR pDeviceName,
        //  _In_ DWORD dwDeviceClass,
        //  _Out_ BOOL *pUsePerUserProfiles
        //);
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/dd372253(v=vs.85).aspx
        /// </summary>
        /// <returns>0, if failed</returns>
        [DllImport("Mscms.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern UInt32 WcsGetUsePerUserProfiles(string deviceName, DeviceClassFlags deviceClass, out UInt32 usePerUserProfiles);

        // from: c:\Program Files (x86)\Windows Kits\10\Include\10.0.10240.0\um\Icm.h
        //BOOL WINAPI WcsGetDefaultColorProfileSize(
        //  _In_ WCS_PROFILE_MANAGEMENT_SCOPE profileManagementScope,
        //  _In_opt_ PCWSTR pDeviceName,
        //  _In_ COLORPROFILETYPE cptColorProfileType,
        //  _In_ COLORPROFILESUBTYPE cpstColorProfileSubType,
        //  _In_ DWORD dwProfileID,
        //  _Out_ PDWORD pcbProfileName
        //);
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/dd372249(v=vs.85).aspx
        /// </summary>
        /// <param name="cbProfileName">Size in bytes! String length is /2</param>
        /// <returns>0, if failed</returns>
        [DllImport("Mscms.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern UInt32 WcsGetDefaultColorProfileSize(WCS_PROFILE_MANAGEMENT_SCOPE scope,
            string deviceName,
            COLORPROFILETYPE colorProfileType,
            COLORPROFILESUBTYPE colorProfileSubType,
            UInt32 dwProfileID,
            out UInt32 cbProfileName
        );

        // from: c:\Program Files (x86)\Windows Kits\10\Include\10.0.10240.0\um\Icm.h
        //BOOL WINAPI WcsGetDefaultColorProfile(
        //  _In_ WCS_PROFILE_MANAGEMENT_SCOPE profileManagementScope,
        //  _In_opt_ PCWSTR pDeviceName,
        //  _In_ COLORPROFILETYPE cptColorProfileType,
        //  _In_ COLORPROFILESUBTYPE cpstColorProfileSubType,
        //  _In_ DWORD dwProfileID,
        //  _In_ DWORD cbProfileName,
        //  _Out_ LPWSTR pProfileName
        //);
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/dd372247(v=vs.85).aspx
        /// </summary>
        /// <returns>0, if failed</returns>
        [DllImport("Mscms.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern UInt32 WcsGetDefaultColorProfile(WCS_PROFILE_MANAGEMENT_SCOPE scope,
            string deviceName,
            COLORPROFILETYPE colorProfileType,
            COLORPROFILESUBTYPE colorProfileSubType,
            UInt32 dwProfileID,
            UInt32 cbProfileName,
            StringBuilder profileName
        );

        public static string GetMonitorProfile()
        {
            // c++ recommendation: http://stackoverflow.com/questions/13533754/code-example-for-wcsgetdefaultcolorprofile

            DISPLAY_DEVICE displayDevice = new DISPLAY_DEVICE();
            displayDevice.cb = Marshal.SizeOf(displayDevice);

            // First, find the primary adaptor
            string adaptorName = null;
            UInt32 deviceIndex = 0;

            while (EnumDisplayDevices(null, deviceIndex++, ref displayDevice, EDD_GET_DEVICE_INTERFACE_NAME) != 0)
            {
                if ((displayDevice.StateFlags & DisplayDeviceStateFlags.AttachedToDesktop) != 0 &&
                    (displayDevice.StateFlags & DisplayDeviceStateFlags.PrimaryDevice) != 0)
                {
                    adaptorName = displayDevice.DeviceName;
                    break;
                }
            }

            // Second, find the first active (and attached) monitor
            string deviceName = null;
            deviceIndex = 0;
            while (EnumDisplayDevices(adaptorName, deviceIndex++, ref displayDevice, EDD_GET_DEVICE_INTERFACE_NAME) != 0)
            {
                if ((displayDevice.StateFlags & DisplayDeviceStateFlags.Active) != 0 &&
                    (displayDevice.StateFlags & DisplayDeviceStateFlags.Attached) != 0)
                {
                    deviceName = displayDevice.DeviceKey;
                    break;
                }
            }

            // Third, find out whether to use the global or user profile
            UInt32 usePerUserProfiles = 0;
            UInt32 res = WcsGetUsePerUserProfiles(deviceName, DeviceClassFlags.CLASS_MONITOR, out usePerUserProfiles);
            if (res == 0)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }

            // Finally, get the profile name
            WCS_PROFILE_MANAGEMENT_SCOPE scope = (usePerUserProfiles != 0) ?
                WCS_PROFILE_MANAGEMENT_SCOPE.WCS_PROFILE_MANAGEMENT_SCOPE_CURRENT_USER :
                WCS_PROFILE_MANAGEMENT_SCOPE.WCS_PROFILE_MANAGEMENT_SCOPE_SYSTEM_WIDE;

            UInt32 cbProfileName = 0;   // in bytes
            res = WcsGetDefaultColorProfileSize(scope,
                deviceName,
                COLORPROFILETYPE.CPT_ICC,
                COLORPROFILESUBTYPE.CPST_RGB_WORKING_SPACE,
                0,
                out cbProfileName);
            if (res == 0)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }

            int nLengthProfileName = (int)cbProfileName / 2;    // WcsGetDefaultColor... is using LPWSTR, i.e. 2 bytes/char
            StringBuilder profileName = new StringBuilder(nLengthProfileName);
            res = WcsGetDefaultColorProfile(scope,
                deviceName,
                COLORPROFILETYPE.CPT_ICC,
                COLORPROFILESUBTYPE.CPST_RGB_WORKING_SPACE,
                0,
                cbProfileName,
                profileName);
            if (res == 0)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }

            return profileName.ToString();
        }

        [DllImport("Mscms.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool GetColorDirectory(IntPtr pMachineName, StringBuilder pBuffer, ref uint pdwSize);

        public static string GetColorDirectory()
        {
            // s. http://stackoverflow.com/questions/14792764/is-there-an-equivalent-to-winapi-getcolordirectory-in-net
            uint pdwSize = 260;  // MAX_PATH 
            StringBuilder sb = new StringBuilder((int)pdwSize);
            if (GetColorDirectory(IntPtr.Zero, sb, ref pdwSize))
            {
                return sb.ToString();
            }
            else
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
        }

    }
    public struct IDirect3DDevice9
    {
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct PHYSICAL_MONITOR
    {
        public IntPtr hPhysicalMonitor;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szPhysicalMonitorDescription;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RAMP
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public UInt16[] Red;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public UInt16[] Green;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public UInt16[] Blue;
    }
    public class Monitor : IDisposable
    {
       
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool LockWorkStation();
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("kernel32.dll")]
       public  static extern uint GetLastError();

        [DllImport("gdi32.dll")]
        public static extern bool SetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);
        [DllImport("user32.dll", EntryPoint = "MonitorFromWindow")]
        public static extern IntPtr MonitorFromWindow([In] IntPtr hwnd, uint dwFlags);

        [DllImport("dxva2.dll", EntryPoint = "DestroyPhysicalMonitors")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyPhysicalMonitors(uint dwPhysicalMonitorArraySize, ref PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        [DllImport("dxva2.dll", EntryPoint = "GetNumberOfPhysicalMonitorsFromHMONITOR")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, ref uint pdwNumberOfPhysicalMonitors);
        [DllImport("dxva2.dll", EntryPoint = "GetPhysicalMonitorsFromIDirect3DDevice9")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPhysicalMonitorsFromIDirect3DDevice9(IDirect3DDevice9 pDirect3DDevice9, uint dwPhysicalMonitorArraySize,
            ref PHYSICAL_MONITOR pPhysicalMonitorArray);

        [DllImport("dxva2.dll", EntryPoint = "GetPhysicalMonitorsFromHMONITOR")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, uint dwPhysicalMonitorArraySize, [Out] PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorBrightness")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorBrightness(IntPtr handle, ref uint minimumBrightness, ref uint currentBrightness, ref uint maxBrightness);

        [DllImport("dxva2.dll", EntryPoint = "SetMonitorBrightness")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetMonitorBrightness(IntPtr handle, uint newBrightness);
        [DllImport("dxva2.dll", EntryPoint = "GetMonitorContrast")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorContrast(IntPtr handle, ref uint minimumContrast, ref uint currentContrast, ref uint maxContast);


        [DllImport("dxva2.dll", EntryPoint = "SetMonitorContrast")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetMonitorContrast(IntPtr handle, uint newContruat);
        [DllImport("gdi32.dll")]
        public static extern int GetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);
        private uint _physicalMonitorsCount = 0;
        private PHYSICAL_MONITOR[] _physicalMonitorArray;

        private IntPtr _firstMonitorHandle;

        private uint _minValue = 0;
        private uint _maxValue = 0;
        private uint _currentValue = 0;
        public Monitor(IntPtr windowHandle)
        {
            uint dwFlags = 0u;

            if (Machine.GetCurrentChassisType() == ChassisTypes.Desktop)
            {
                IntPtr ptr = MonitorFromWindow(windowHandle, dwFlags);
                if (!GetNumberOfPhysicalMonitorsFromHMONITOR(ptr, ref _physicalMonitorsCount))
                {
                    throw new Exception("Cannot get monitor count!" );
                }
                _physicalMonitorArray = new PHYSICAL_MONITOR[_physicalMonitorsCount];

                if (!GetPhysicalMonitorsFromHMONITOR(ptr, _physicalMonitorsCount, _physicalMonitorArray))
                {
                    throw new Exception("Cannot get phisical monitor handle! " +Convert.ToString(GetLastError()));
                }
                _firstMonitorHandle = _physicalMonitorArray[0].hPhysicalMonitor;
                if(_firstMonitorHandle==IntPtr.Zero)
                {
                    //  GetPhysicalMonitorsFromIDirect3DDevice9(ptr, _physicalMonitorsCount,  _physicalMonitorArray);
                   
                }

                if (!GetMonitorBrightness(_firstMonitorHandle, ref _minValue, ref _currentValue, ref _maxValue))
                {

                    if (Machine.GetCurrentChassisType() == ChassisTypes.Desktop)
                    {
                        throw new Exception("Cannot get monitor brightness!" + Convert.ToString(GetLastError()));
                    }
                }
                if (!GetMonitorContrast(_firstMonitorHandle, ref _minValue, ref _currentValue, ref _maxValue))
                {
                    throw new Exception("Cannot get monitor contrast!");
                }
            } 
        }
        /// <summary>
        /// Turns off the pc monitor
        /// </summary>
        /// <param name="hwnd"></param>
        public void TurnOffMonitor(int hwnd)
        {
            try
            {
                
                Monitor.SendMessage(hwnd, 0x0112, 0xF170, 2);
            }
            catch (Exception ex)
            {
                Base.exceptionHandle(ex);

            }
        }
        /// <summary>
        /// Turns on the monitor
        /// </summary>
        /// <param name="hwnd"></param>
        public void TurnOnMonitor(int hwnd)
        {
            try
            {
               
                Monitor.SendMessage(hwnd, 0x0112, 0xF170, -1);
            }
            catch (Exception ex)
            {
                Base.exceptionHandle(ex);

            }



        }

        public void SetBrightness(int newValue)
        {
            try
            {
                if (Machine.GetCurrentChassisType() == ChassisTypes.Desktop)
                {
                    newValue = Math.Min(newValue, Math.Max(0, newValue));
                    _currentValue = (_maxValue - _minValue) * (uint)newValue / 100u + _minValue;
                    var ap = SetMonitorBrightness(_firstMonitorHandle, _currentValue);
                }
                else
                {
                    byte[] bLevels = GetWMIBrightnessLevels();
                    SetWMIBrightness(bLevels[newValue]);
                }
            }
            catch (Exception ex)
            {
                Base.exceptionHandle(ex);

            }
        }
        public int GetBrightness()
        {
            try
            { int ap = -1;
                if (Machine.GetCurrentChassisType() == ChassisTypes.Desktop)
                {
                   
                     GetMonitorBrightness(  _firstMonitorHandle,ref _minValue, ref _currentValue,ref _maxValue);
                    ap =(int) _currentValue;
                }
                else
                {
                    
                    ap=GetWMIBrightness();
                }
                return ap;
            }
            catch (Exception ex)
            {
                Base.exceptionHandle(ex);
                return -1;

            }
        }

        public int  GetContrast()
        {
            try
            {
                int ap = -1;
                if (Machine.GetCurrentChassisType() == ChassisTypes.Desktop)
                {
                    GetMonitorContrast(_firstMonitorHandle, ref _minValue, ref _currentValue, ref _maxValue);
                    ap = (int)_currentValue;
                    
                }
                else
                {

                   
                }
                return ap;
            }
            catch (Exception ex)
            {
                Base.exceptionHandle(ex);
                return -1;

            }
        }
        public void SetContrast(int newValue)
        {
            try
            {
                if (Machine.GetCurrentChassisType() == ChassisTypes.Desktop)
                {
                    newValue = Math.Min(newValue, Math.Max(0, newValue));
                    _currentValue = (_maxValue - _minValue) * (uint)newValue / 100u + _minValue;
                    SetMonitorContrast(_firstMonitorHandle, _currentValue);
                }
               else
                {


                }
            }
            catch (Exception ex)
            {
                Base.exceptionHandle(ex);

            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_physicalMonitorsCount > 0)
                {
                    DestroyPhysicalMonitors(_physicalMonitorsCount, ref _physicalMonitorArray);
                }
            }
        }


        public void SetGamma(int gamma)
        {

            try
            {
                
                    if (gamma <= 256 && gamma >= 1)
                {
                        if (Machine.GetCurrentChassisType() == ChassisTypes.Desktop)
                        {
                            RAMP ramp = new RAMP();
                            ramp.Red = new ushort[256];
                            ramp.Green = new ushort[256];
                            ramp.Blue = new ushort[256];
                            for (int i = 1; i < 256; i++)
                            {
                                int iArrayValue = i * (gamma + 128);

                                if (iArrayValue > 65535)
                                    iArrayValue = 65535;
                                ramp.Red[i] = ramp.Blue[i] = ramp.Green[i] = (ushort)iArrayValue;
                            }
                            SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref ramp);
                        }
                        else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Base.exceptionHandle(ex);

            }
        }
        #region WMIforLpatops

        


        public static int GetWMIBrightness()
        {
            try
            {

                //define scope (namespace)
                System.Management.ManagementScope s = new System.Management.ManagementScope("root\\WMI");

                //define query
                System.Management.SelectQuery q = new System.Management.SelectQuery("WmiMonitorBrightness");

                //output current brightness
                System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(s, q);

                System.Management.ManagementObjectCollection moc = mos.Get();

                //store result
                byte curBrightness = 0;
                foreach (System.Management.ManagementObject o in moc)
                {
                    curBrightness = (byte)o.GetPropertyValue("CurrentBrightness");
                    break; //only work on the first object
                }

                moc.Dispose();
                mos.Dispose();

                return (int)curBrightness;
            }
            catch (Exception ex)
            {
                Base.exceptionHandle(ex);
                return -1;

            }
        }


        //array of valid brightness values in percent
        public static byte[] GetWMIBrightnessLevels()
        {
            try
            {
                //define scope (namespace)
                System.Management.ManagementScope s = new System.Management.ManagementScope("root\\WMI");

                //define query
                System.Management.SelectQuery q = new System.Management.SelectQuery("WmiMonitorBrightness");

                //output current brightness
                System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(s, q);
                byte[] BrightnessLevels = new byte[0];

                try
                {
                    System.Management.ManagementObjectCollection moc = mos.Get();

                    //store result


                    foreach (System.Management.ManagementObject o in moc)
                    {
                        BrightnessLevels = (byte[])o.GetPropertyValue("Level");
                        break; //only work on the first object
                    }

                    moc.Dispose();
                    mos.Dispose();

                }
                catch (Exception ex)
                {
                    throw (ex);
                }

                return BrightnessLevels;
            }
            catch (Exception ex)
            {
                Base.exceptionHandle(ex);
                return null;
            }
        }

        public static   void SetWMIBrightness(byte targetBrightness)
        {
            try
            {
                //define scope (namespace)
                System.Management.ManagementScope s = new System.Management.ManagementScope("root\\WMI");

                //define query
                System.Management.SelectQuery q = new System.Management.SelectQuery("WmiMonitorBrightnessMethods");

                //output current brightness
                System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(s, q);

                System.Management.ManagementObjectCollection moc = mos.Get();

                foreach (System.Management.ManagementObject o in moc)
                {
                    o.InvokeMethod("WmiSetBrightness", new Object[] { UInt32.MaxValue, targetBrightness }); //note the reversed order - won't work otherwise!
                    break; //only work on the first object
                }

                moc.Dispose();
                mos.Dispose();
            }
            catch (Exception ex)
            {
                Base.exceptionHandle(ex);

            }
        }
#endregion
    }
    }


