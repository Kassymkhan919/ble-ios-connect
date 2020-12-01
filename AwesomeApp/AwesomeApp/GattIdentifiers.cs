using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeApp
{
    class GattIdentifiers
    {
        public static Guid UartGattServiceId = Guid.Parse("0003CDD0-0000-1000-8000-00805F9B0131");
        public static Guid UartGattCharacteristicReceivedId = Guid.Parse("0003CDD1-0000-1000-8000-00805F9B0131");
        public static Guid UartGattCharacteristicSendId = Guid.Parse("0003CDD2-0000-1000-8000-00805F9B0131");
        public static Guid SpecialNotificationDescriptorId = Guid.Parse("92495146-C796-44BE-8B50-2FBAEAB1A4E8"); 
        
        
        
        //public static Guid UartGattServiceId = Guid.Parse("");
        //public static Guid UartGattCharacteristicReceivedId = Guid.Parse("");
        //public static Guid UartGattCharacteristicSendId = Guid.Parse("");
        //public static Guid SpecialNotificationDescriptorId = Guid.Parse("");
        

    }
}
