using CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BucketBox.Devices
{

    /// <summary>
    /// This class helps you manage audio devices on PC
    /// </summary>
    public class AudioDeviceManager
    {
        //  int MaxMicro = 0, MaxMicro2 = 0;
        MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
        public MMDeviceCollection GetAllAudioEndPoints()
        {
            try
            {
                MMDeviceCollection devices = null;

                devices = this.GetAllAudioEndPoints(EDataFlow.eAll, EDeviceState.DEVICE_STATEMASK_ALL);


                return devices;

            }
            catch (Exception e)
            {

                
                return null;

            }

        }

        /// <summary>
        /// Gets all the audio endpoints based on the criteria given
        /// </summary>
        /// <param name="edataflow"> choose if you want the input or output endpoints or both</param>
        /// <param name="devicestate">choose if you want to get the disabled,enabled or unpluged or all.</param>
        /// <returns></returns>
        public MMDeviceCollection GetAllAudioEndPoints(EDataFlow edataflow, EDeviceState devicestate)
        {
            try
            {
                MMDeviceCollection devices = null;

                devices = DevEnum.EnumerateAudioEndPoints(edataflow,
                    devicestate);


                return devices;

            }
            catch (Exception e)
            {

                
                return null;

            }

        }

        /// <summary>
        /// Find audio endpoint by name
        /// </summary>
        /// <param name="name">the name of the endpint</param>
        /// <param name="deviceflow">whenever it is input or output or both</param>
        /// <param name="state">whenever it is enabled,disabled unpluged or both</param>
        /// <returns>the desired endpoint</returns>
        public MMDevice FindEndpointByName(string name, EDataFlow deviceflow,
            EDeviceState state)
        {
            try
            {
                MMDevice device = null;
                MMDeviceCollection devices = this.GetAllAudioEndPoints(deviceflow, state);

                if (devices != null)
                {
                    for (int i = 0; i < devices.Count; i++)
                    {
                        MMDevice deviceAt = devices[i];
                        if (deviceAt.FriendlyName.ToLower() == name)
                        {
                            device = deviceAt;
                            break;

                        }
                    }
                }



                return device;

            }
            catch (Exception e)
            {

                Base.exceptionHandle(e);
                return null;

            }

        }
        /// <summary>
        /// Find audio input endpoint by name
        /// </summary>
        /// <param name="name">the name of the endpint</param>
        /// <returns>the desired endpoint</returns>

        public MMDevice FindEndpointByNameinInputEndpoints(string name)
        {
            try
            {
                MMDevice device = null;
                device = this.FindEndpointByName(name, EDataFlow.eCapture, EDeviceState.DEVICE_STATEMASK_ALL);



                return device;

            }
            catch (Exception e)
            {

                Base.exceptionHandle(e);
                return null;

            }

        }
        /// <summary>
        /// Find audio output endpoint by name
        /// </summary>
        /// <param name="name">the name of the endpint</param>
        /// <returns>the desired endpoint</returns>
        public MMDevice FindEndpointByNameinOutputEndpoints(string name)
        {
            try
            {
                MMDevice device = null;
                device = this.FindEndpointByName(name, EDataFlow.eRender, EDeviceState.DEVICE_STATEMASK_ALL);



                return device;

            }
            catch (Exception e)
            {

                Base.exceptionHandle(e);
                return null;

            }

        }

        /// <summary>
        /// Find all audio endpoint containing the given  name
        /// </summary>
        /// <param name="name">the name of the endpint</param>
        /// <param name="deviceflow">whenever it is input or output or both</param>
        /// <param name="state">whenever it is enabled,disabled unpluged or both</param>
        /// <returns>the desired endpoint</returns>
        public MMDevice[] FindEndpointContaininginName(string name, EDataFlow deviceflow,
          EDeviceState state)
        {
            try
            {
                MMDevice[] device = null;
                int MaxAudio = 0;
                MMDeviceCollection devices = this.GetAllAudioEndPoints(deviceflow, state);

                if (devices != null)
                {
                    for (int i = 0; i < devices.Count; i++)
                    {
                        MMDevice deviceAt = devices[i];
                        if (deviceAt.FriendlyName.Contains(name) == true)
                        {
                            ++MaxAudio;

                        }
                    }
                    device = new MMDevice[MaxAudio];
                    for (int i = 0; i < devices.Count; i++)
                    {
                        MMDevice deviceAt = devices[i];
                        if (deviceAt.FriendlyName.Contains(name) == true)
                        {
                            device[i] = deviceAt;

                        }
                    }

                }



                return device;

            }
            catch (Exception e)
            {

                Base.exceptionHandle(e);
                return null;

            }
        }
        /// <summary>
        /// Find all audio input endpoint containing the given  name
        /// </summary>
        /// <param name="name">the name of the endpint</param>

        /// <returns>the desired endpoint</returns>
        public MMDevice[] FindInputEndpointContaininginName(string name)
        {
            try
            {
                MMDevice[] device = null;
                device = this.FindEndpointContaininginName(name, EDataFlow.eCapture, EDeviceState.DEVICE_STATEMASK_ALL);


                return device;

            }
            catch (Exception e)
            {

                Base.exceptionHandle(e);
                return null;

            }
        }

        /// <summary>
        /// Find all audio output endpoint containing the given  name
        /// </summary>
        /// <param name="name">the name of the endpint</param>

        /// <returns>the desired endpoint</returns>
        public MMDevice[] FindOutputEndpointContaininginName(string name)
        {
            try
            {
                MMDevice[] device = null;
                device = this.FindEndpointContaininginName(name, EDataFlow.eRender, EDeviceState.DEVICE_STATEMASK_ALL);


                return device;

            }
            catch (Exception e)
            {
                Base.exceptionHandle(e);

                return null;

            }
        }

    }

}
