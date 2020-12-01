using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;
using XamarinEssentials = Xamarin.Essentials;

using Xamarin.Forms.Xaml;

namespace AwesomeApp
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly IAdapter _bluetoothAdapter;
        private List<IDevice> _gattDevices = new List<IDevice>();


        public MainPage()
        {
            InitializeComponent();

            _bluetoothAdapter = CrossBluetoothLE.Current.Adapter;
            _bluetoothAdapter.DeviceDiscovered += (sender, foundBleDevice) =>
            {
                if (foundBleDevice.Device != null && !string.IsNullOrEmpty(foundBleDevice.Device.Name))
                    _gattDevices.Add(foundBleDevice.Device);
            };
        }

        private async Task<bool> PermssionsGrantedAsync()
        {
            var locationPermissionStatus = await XamarinEssentials.Permissions.CheckStatusAsync<XamarinEssentials.Permissions.LocationAlways>();
            
            if (locationPermissionStatus != XamarinEssentials.PermissionStatus.Granted)
            {
                var status = await XamarinEssentials.Permissions.RequestAsync<XamarinEssentials.Permissions.LocationAlways>();
                return status == XamarinEssentials.PermissionStatus.Granted;
            }
            return true;
        }

        private async void ScanButton_Clicked(object sender, EventArgs e)
        {
            IsBusyIndicator.IsVisible = IsBusyIndicator.IsRunning = !(ScanButton.IsEnabled = false);
            foundBleDevicesListView.ItemsSource = null;

            if (!await PermssionsGrantedAsync())
            {
                await App.Current.MainPage.DisplayAlert("Permission required", "Application needs location permission", "OK");
                IsBusyIndicator.IsVisible = IsBusyIndicator.IsRunning = !(ScanButton.IsEnabled = true);
                return;
            }

            _gattDevices.Clear();

            foreach (var device in _bluetoothAdapter.ConnectedDevices)
                _gattDevices.Add(device);

            await _bluetoothAdapter.StartScanningForDevicesAsync();

            foundBleDevicesListView.ItemsSource = _gattDevices.ToArray();
            IsBusyIndicator.IsVisible = IsBusyIndicator.IsRunning = !(ScanButton.IsEnabled = true);
        }

        private async void FoundBluetoothDevicesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            IsBusyIndicator.IsVisible = IsBusyIndicator.IsRunning = !(ScanButton.IsEnabled = false);
            IDevice selectedItem = e.Item as IDevice;

            if(selectedItem.State == DeviceState.Connected)
            {
                await App.Current.MainPage.Navigation.PushAsync(new BluetoothDataPage(selectedItem));
            }
            else
            {
                try
                {
                    var connectParameters = new ConnectParameters(false, true);
                    await _bluetoothAdapter.ConnectToDeviceAsync(selectedItem, connectParameters);
                    await App.Current.MainPage.Navigation.PushAsync(new BluetoothDataPage(selectedItem));
                }
                catch
                {
                    await App.Current.MainPage.DisplayAlert("Error connecting", $"Error connecting to BLE device: {selectedItem.Name ?? "N/A"}", "Retry");
                }

                IsBusyIndicator.IsVisible = IsBusyIndicator.IsRunning = !(ScanButton.IsEnabled = true);
            }
        }
    }
}