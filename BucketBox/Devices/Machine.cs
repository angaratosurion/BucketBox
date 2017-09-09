using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace BucketBox.Devices
{
    public enum ChassisTypes
    {
        Other = 1,
        Unknown,
        Desktop,
        LowProfileDesktop,
        PizzaBox,
        MiniTower,
        Tower,
        Portable,
        Laptop,
        Notebook,
        Handheld,
        DockingStation,
        AllInOne,
        SubNotebook,
        SpaceSaving,
        LunchBox,
        MainSystemChassis,
        ExpansionChassis,
        SubChassis,
        BusExpansionChassis,
        PeripheralChassis,
        StorageChassis,
        RackMountChassis,
        SealedCasePC
    }
    public class Machine
    {
       /// <summary>
       /// Returns the Type of Device the application is runing on
       /// </summary>
       /// <returns></returns>

        public static ChassisTypes GetCurrentChassisType()
        {
            ManagementClass systemEnclosures = new ManagementClass("Win32_SystemEnclosure");
            foreach (ManagementObject obj in systemEnclosures.GetInstances())
            {
                foreach (int i in (UInt16[])(obj["ChassisTypes"]))
                {
                    if (i > 0 && i < 25)
                    {
                        return (ChassisTypes)i;
                    }
                }
            }
            return ChassisTypes.Unknown;
        }
    }
}
