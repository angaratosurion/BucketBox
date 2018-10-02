using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BucketBox.Devices
{
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
                    throw new Exception("Cannot get monitor count!");
                }
                _physicalMonitorArray = new PHYSICAL_MONITOR[_physicalMonitorsCount];

                if (!GetPhysicalMonitorsFromHMONITOR(ptr, _physicalMonitorsCount, _physicalMonitorArray))
                {
                    throw new Exception("Cannot get phisical monitor handle!");
                }
                _firstMonitorHandle = _physicalMonitorArray[0].hPhysicalMonitor;

                if (!GetMonitorBrightness(_firstMonitorHandle, ref _minValue, ref _currentValue, ref _maxValue))
                {

                    if (Machine.GetCurrentChassisType() == ChassisTypes.Desktop)
                    {
                        throw new Exception("Cannot get monitor brightness!");
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


