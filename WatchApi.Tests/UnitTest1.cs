using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WatchApi.Tests
{
    [TestClass]
    public class USBDeviceTests
    {

        [TestMethod]
        public void ReadSomeData()
        {
            //int num = USBDevice.GetNumberOfDevices() - 1;
            USBDevice device = new USBDevice(0);
            device.Open();
            Thread.Sleep(500);
            var data = device.Read();
            Debug.WriteLine(data);
            
        }
    }
}
