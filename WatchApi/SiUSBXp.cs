using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WatchApi
{
    // ReSharper disable InconsistentNaming
    public enum QueueStatus
    {
        //NoOverrun = 0x00,
        Empty = 0x00,
        Overrun = 0x01,
        Ready = 0x02,
    }

    

    internal class SiUSBXp
    {

        /*
            Windows Data Type	.NET Data Type
            BOOL, BOOLEAN	 Boolean or Int32
            BSTR	 String
            BYTE	 Byte
            CHAR	 Char
            DOUBLE	 Double
            DWORD/LPDWORD	 Int32 or UInt32
            FLOAT	 Single
            HANDLE (and all other handle types, such as HFONT and HMENU)	IntPtr, UintPtr, or HandleRef
            HRESULT	 Int32 or UInt32
            INT	 Int32
            LANGID	 Int16 or UInt16
            LCID	 Int32 or UInt32
            LONG	 Int32
            LPARAM	IntPtr, UintPtr, or Object
            LPCSTR	 String
            LPCTSTR	 String
            LPCWSTR	 String
            LPSTR	 String or StringBuilder*
            LPTSTR	 String or StringBuilder
            LPWSTR	 String or StringBuilder
            LPVOID	IntPtr, UintPtr, or Object
            LRESULT	IntPtr
            SAFEARRAY	 .NET array type
            SHORT	 Int16
            TCHAR	 Char
            UCHAR	SByte
            UINT	 Int32 or UInt32
            ULONG	 Int32 or UInt32
            VARIANT	 Object
            VARIANT_BOOL	 Boolean
            WCHAR	 Char
            WORD	 Int16 or UInt16
            WPARAM	IntPtr, UintPtr, or Object
         */

        public enum RxQueueStatus
        {

            SI_RX_NO_OVERRUN = 0x00,
            SI_RX_EMPTY = 0x00,
            SI_RX_OVERRUN = 0x01,
            SI_RX_READY = 0x02,
        }

        public enum SI_STATUS
        {
            // Return codes
            SI_SUCCESS = 0x00,
            SI_DEVICE_NOT_FOUND = 0xFF,
            SI_INVALID_HANDLE = 0x01,
            SI_READ_ERROR = 0x02,
            SI_RX_QUEUE_NOT_READY = 0x03,
            SI_WRITE_ERROR = 0x04,
            SI_RESET_ERROR = 0x05,
            SI_INVALID_PARAMETER = 0x06,
            SI_INVALID_REQUEST_LENGTH = 0x07,
            SI_DEVICE_IO_FAILED = 0x08,
            SI_INVALID_BAUDRATE = 0x09,
            SI_FUNCTION_NOT_SUPPORTED = 0x0a,
            SI_GLOBAL_DATA_ERROR = 0x0b,
            SI_SYSTEM_ERROR_CODE = 0x0c,
            SI_READ_TIMED_OUT = 0x0d,
            SI_WRITE_TIMED_OUT = 0x0e,
            SI_IO_PENDING = 0x0f,
        }

        
        public enum ProductStringOption
        {
            SI_RETURN_SERIAL_NUMBER = 0x00,
            SI_RETURN_DESCRIPTION = 0x01,
            //SI_RETURN_LINK_NAME = 0x02,
            SI_RETURN_VID = 0x03,
            SI_RETURN_PID = 0x04,
        }


        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_Open(uint dwDevice, ref IntPtr cyHandle);

        /// <summary>
        /// This function returns the number of devices connected to the host.
        /// </summary>
        /// <param name="NumDevices"></param>
        /// <returns></returns>
        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_GetNumDevices(ref uint NumDevices);
        
        /// <summary>
        /// This function returns a null terminated serial number (S/N) string or product description string for
        /// the device specified by an index passed in DeviceNum. The index for the first device is 0 and the
        /// last device is the value returned by SI_GetNumDevices – 1.
        /// </summary>
        /// <param name="dwDeviceNum">Index of the device for which the product description string or serial number string is desired</param>
        /// <param name="lpvDeviceString">Variable of type SI_DEVICE_STRING  which will contain a NULL terminated device descriptor or serial number string on return</param>
        /// <param name="dwFlags">DWORD containing flags to determine if DeviceString contains a serial number, product description, Vendor ID, or Product ID string. See <see cref="SiUSBXp.ProductStringOption"/>.</param>
        /// <returns></returns>
        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_GetProductString(uint dwDeviceNum, StringBuilder lpvDeviceString, uint dwFlags);
        
        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_Close(IntPtr cyHandle);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_Read(IntPtr cyHandle, ref byte lpBuffer, uint dwBytesToRead, ref uint lpdwBytesReturned, uint overlapped);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_Write(IntPtr cyHandle, byte[] lpBuffer, uint dwBytesToWrite, ref uint lpdwBytesWritten, IntPtr o);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_DeviceIOControl(IntPtr cyHandle, uint dwIoControlCode, byte[] lpInBuffer, uint dwBytesToRead, byte[] lpOutBuffer, uint dwBytesToWrite, ref uint lpdwBytesSucceeded);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_FlushBuffers(IntPtr cyHandle, byte FlushTransmit, byte FlushReceive);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_SetTimeouts(uint dwReadTimeout, uint dwWriteTimeout);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_GetTimeouts(ref uint lpdwReadTimeout, ref uint lpdwWriteTimeout);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_CheckRXQueue(IntPtr cyHandle, ref uint lpdwNumBytesInQueue, ref uint lpdwQueueStatus);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_SetBaudRate(IntPtr cyHandle, uint dwBaudRate);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_SetBaudDivisor(IntPtr cyHandle, ushort wBaudDivisor);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_SetLineControl(IntPtr cyHandle, ushort wLineControl);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_SetFlowControl(IntPtr cyHandle, byte bCTS_MaskCode, byte bRTS_MaskCode, byte bDTR_MaskCode, byte bDSR_MaskCode, byte bDCD_MaskCode, bool bFlowXonXoff);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_GetModemStatus(IntPtr cyHandle, ref byte ModemStatus);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_SetBreak(IntPtr cyHandle, ushort wBreakState);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_ReadLatch(IntPtr cyHandle, ref byte lpbLatch);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_WriteLatch(IntPtr cyHandle, byte bMask, byte bLatch);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_GetPartNumber(IntPtr cyHandle, ref byte lpbPartNum);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_GetDeviceProductString(IntPtr cyHandle, byte[] lpProduct, ref byte lpbLength, bool bConvertToASCII);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_GetDLLVersion(ref uint HighVersion, ref uint LowVersion);

        [DllImport("SiUSBXp.dll")]
        public static extern SI_STATUS SI_GetDriverVersion(ref uint HighVersion, ref uint LowVersion);
    }
    // ReSharper restore InconsistentNaming

}