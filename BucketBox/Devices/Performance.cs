using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BucketBox.Devices
{
    /// <summary>
    ///  Retrieves info about the performance of the process/script
    /// </summary>
    public class Performance
    {
        /// <summary>
        /// Returns the memory usage of the process
        /// </summary>
        /// <param name="proc">the process  of the memory usage</param>
        /// <returns>Returns the memory usage of the process</returns>
        public double GetMemory(Process proc)
        {
            try
            {
                double memsize = 0;

                if (proc != null)
                {
                    PerformanceCounter PC = new PerformanceCounter();
                    PC.CategoryName = "Process";
                    PC.CounterName = "Working Set - Private";
                    PC.InstanceName = proc.ProcessName;
                    memsize = Convert.ToDouble(PC.NextValue());// / (int)(1024);
                    PC.Close();
                    PC.Dispose();
                }
                return memsize;
            }
            catch (Exception e)
            {

                Base.exceptionHandle(e);
                return -1;

            }

        }
    }
}
