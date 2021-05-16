﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MqttAgent
{
    // There is no autogenerated class for this information model so create one here.
    // IVendorNameplateType: https://reference.opcfoundation.org/DI/docs/4.5.2/
    public class VendorNameplate
    {
        public string Manufacturer { get; set; }
        public string ManufacturerUri { get; set; }
        public string ProductInstanceUri { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string ProductCode { get; set; }
        public string HardwareRevision { get; set; }
        public string SoftwareRevision { get; set; }
        public string DeviceRevision { get; set; }


        
    }
}