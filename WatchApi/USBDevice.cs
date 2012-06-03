using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WatchApi
{

    public class RxQueueData
    {
        private readonly QueueStatus _status;
        private readonly int _bytesAvailable;

        public RxQueueData(QueueStatus status, int bytesAvailable)
        {
            this._status = status;
            this._bytesAvailable = bytesAvailable;
        }
       
        public QueueStatus Status
        {
            get { return this._status; }
        }

        public int BytesAvailable
        {
            get { return this._bytesAvailable; }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} bytes available", Status, BytesAvailable);
        }
    }

    public class USBDevice : IDisposable
    {
        private readonly int _deviceNumber;
        private IntPtr _deviceHandle;
        private readonly string _serialNumber;
        private readonly string _vendorId;
        private readonly string _productId;
        private string _deviceProductString;

        public string SerialNumber
        {
            get { return this._serialNumber; }
        }

        public string VendorId
        {
            get { return this._vendorId; }
        }

        public string ProductId
        {
            get { return this._productId; }
        }

        public string DeviceProductString
        {
            get { return this._deviceProductString; }
        }

        public USBDevice(int deviceNumber)
        {
            this._deviceNumber = deviceNumber;
            this._serialNumber = USBExpressApi.GetProductString(this._deviceNumber, SiUSBXp.ProductStringOption.SI_RETURN_SERIAL_NUMBER);
            this._productId = USBExpressApi.GetProductString(this._deviceNumber, SiUSBXp.ProductStringOption.SI_RETURN_PID);
            this._vendorId = USBExpressApi.GetProductString(this._deviceNumber, SiUSBXp.ProductStringOption.SI_RETURN_VID);
            
        }

        private void SetBaudRate()
        {
            USBExpressApi.SetBaudRate(this._deviceHandle, 115200);
            //USBExpressApi.SetBaudDivisor(this._deviceHandle, 8);
        }

        public void Open()
        {
            this._deviceHandle = USBExpressApi.Open(this._deviceNumber);
            this._deviceProductString = USBExpressApi.GetDeviceProductString(this._deviceHandle);
            this.SetBaudRate();
        }
      

        public byte[] Read()
        {
            this.VerifyOpen();
            //var length = 1024;
            var data = USBExpressApi.GetRxQueueData(this._deviceHandle);
            var readCount = 1;
            while (data.Status != QueueStatus.Ready && readCount < 10000)
            {
                var length = data.BytesAvailable;
                if (data.Status == QueueStatus.Ready || length > 0)
                {
                    return USBExpressApi.Read(this._deviceHandle, length);
                }
                readCount++;
                
            }
            return USBExpressApi.Read(this._deviceHandle, 4096);
           // return null;
        }
    
       

        private void VerifyOpen()
        {
            if (this._deviceHandle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Device must be opened before reading");
            }
        }

        public void Close()
        {
            USBExpressApi.Close(this._deviceHandle);
        }

        public void Dispose()
        {
            this._deviceHandle = IntPtr.Zero;
        }
    }
}
