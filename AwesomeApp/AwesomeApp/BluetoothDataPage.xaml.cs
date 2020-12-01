using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinEssentials = Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AwesomeApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BluetoothDataPage : ContentPage
    {

        private readonly IDevice _connectedDevice;

        public BluetoothDataPage(IDevice connectedDevice)
        {
            InitializeComponent();
            _connectedDevice = connectedDevice;
            InitButton.IsEnabled = !(ScanButton.IsEnabled = false);

        }

        private ICharacteristic sendCharacteristic;
        private ICharacteristic receiveCharacteristic;

        private async void InitializeCommandButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var service = await _connectedDevice.GetServiceAsync(GattIdentifiers.UartGattServiceId);

                if(service != null)
                {
                    sendCharacteristic = await service.GetCharacteristicAsync(GattIdentifiers.UartGattCharacteristicSendId);

                    receiveCharacteristic = await service.GetCharacteristicAsync(GattIdentifiers.UartGattCharacteristicReceivedId);
                    if(receiveCharacteristic != null)
                    {
                        var descriptors = await receiveCharacteristic.GetDescriptorsAsync();

                        receiveCharacteristic.ValueUpdated += (o, args) =>
                        {
                            var receivedBytes = args.Characteristic.Value;
                            XamarinEssentials.MainThread.BeginInvokeOnMainThread(() =>
                            {
                                Output.Text += Encoding.UTF8.GetString(receivedBytes, 0, receivedBytes.Length) + Environment.NewLine;
                            });
                        };
                        await receiveCharacteristic.StartUpdatesAsync();
                        InitButton.IsEnabled = !(ScanButton.IsEnabled = true);
                    }
                    else
                    {
                        Output.Text += "UART GATT service not found." + Environment.NewLine;
                    }
                }
            }
            catch
            {
                Output.Text += "Error initializing UART GATT service." + Environment.NewLine;
            }
        }

        private async void SendCommandButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(sendCharacteristic != null)
                {
                    var bytes = await sendCharacteristic.WriteAsync(Encoding.ASCII.GetBytes($"{CommandTxt.Text}\r\n"));
                }
            }
            catch
            {
                Output.Text += "Error sending command to UART." + Environment.NewLine;
            }
        }
    }
}