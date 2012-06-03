using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchApi
{
    internal static class USBExpressApi
    {
        private static void HandleResult(SiUSBXp.SI_STATUS result)
        {
            if (result != SiUSBXp.SI_STATUS.SI_SUCCESS)
            {
                throw new Win32Exception(String.Format("Call to USB failed: {0}", result));
            }
        }

        public static int GetNumberOfDevices()
        {
            uint num = 0;
            var result = SiUSBXp.SI_GetNumDevices(ref num);
            HandleResult(result);
            return num.ToInt();
        }

        public static void SetBaudRate(IntPtr handle, int rate)
        {
            var result = SiUSBXp.SI_SetBaudRate(handle, rate.ToUint());
            HandleResult(result);
        }
        
        public static void SetBaudDivisor(IntPtr handle, short divisor)
        {
            var result = SiUSBXp.SI_SetBaudDivisor(handle, (ushort)divisor);
            HandleResult(result);
        }

        public static IntPtr Open(int deviceNumber)
        {
            IntPtr deviceHandle = IntPtr.Zero;
            var result = SiUSBXp.SI_Open(deviceNumber.ToUint(), ref deviceHandle);
            HandleResult(result);
            return deviceHandle;
        }

        public static Timeouts GetTimeouts()
        {
            uint read = 0, write = 0;
            var result = SiUSBXp.SI_GetTimeouts(ref read, ref write);
            HandleResult(result);
            return new Timeouts(read.ToInt(), write.ToInt());
        }

        public static void SetTimeouts(int read, int write)
        {
            var result = SiUSBXp.SI_SetTimeouts(read.ToUint(), write.ToUint());
            HandleResult(result);
        }

        public static RxQueueData GetRxQueueData(IntPtr handle)
        {
            uint queueStatus = 0;
            uint bytesInQueue = 0;
            var result = SiUSBXp.SI_CheckRXQueue(handle, ref bytesInQueue, ref queueStatus);
            HandleResult(result);
            var status = (QueueStatus)queueStatus;
            return new RxQueueData(status, bytesInQueue.ToInt());
        }

        public static byte[] Read(IntPtr handle, int length)
        {
            byte[] buffer = new byte[length*2];
            uint byteCountReturned = 0;
            var result = SiUSBXp.SI_Read(handle, ref buffer[0], (length*2).ToUint(), ref byteCountReturned, 0);
            HandleResult(result);
            var readBytes = new byte[byteCountReturned];
            Array.Copy(buffer, readBytes, byteCountReturned);
            return readBytes;
        }

        public static string GetProductString(int deviceNumber, SiUSBXp.ProductStringOption stringType)
        {
            StringBuilder sb = new StringBuilder();
            var result = SiUSBXp.SI_GetProductString(deviceNumber.ToUint(), sb, (uint)stringType);
            HandleResult(result);
            var productString = sb.ToString();
            return productString;
        }

        public static string GetDeviceProductString(IntPtr handle)
        {
            byte length = 0;
            byte[] product = new byte[sizeof(byte)];
            var result = SiUSBXp.SI_GetDeviceProductString(handle, product, ref length, true);
            HandleResult(result);
            var str = System.Text.Encoding.ASCII.GetString(product);
            return str;
        }

        public static void Close(IntPtr handle)
        {
            var result = SiUSBXp.SI_Close(handle);
            HandleResult(result);
        }

    }
}
