using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace xamarinThreeButtons
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            StartAccelerometer();
        }

        private void StartAccelerometer()
        {
            try
            {
                if (!Accelerometer.IsMonitoring)
                {
                    Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                    Accelerometer.Start(SensorSpeed.UI);
                }
            }
            catch (FeatureNotSupportedException ex)
            {
                DisplayAlert("Error", "Accelerometer not supported on this device: " + ex.Message, "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Failed to start accelerometer: " + ex.Message, "OK");
            }
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            // Calculate a simple movement threshold
            double movementThreshold = 0.2; // Adjust this value based on sensitivity
            double totalMovement = Math.Abs(data.Acceleration.X) + Math.Abs(data.Acceleration.Y) + Math.Abs(data.Acceleration.Z);

            if (totalMovement > movementThreshold)
            {
                // Change background color based on movement
                // You can use different logic to map movement to colors
                if (Math.Abs(data.Acceleration.X) > Math.Abs(data.Acceleration.Y) && Math.Abs(data.Acceleration.X) > Math.Abs(data.Acceleration.Z))
                {
                    BackgroundColor = Color.Red; // Dominant movement along X-axis
                }
                else if (Math.Abs(data.Acceleration.Y) > Math.Abs(data.Acceleration.X) && Math.Abs(data.Acceleration.Y) > Math.Abs(data.Acceleration.Z))
                {
                    BackgroundColor = Color.Blue; // Dominant movement along Y-axis
                }
                else
                {
                    BackgroundColor = Color.Green; // Dominant movement along Z-axis
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (Accelerometer.IsMonitoring)
            {
                Accelerometer.Stop();
                Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            BackgroundColor = Color.Red;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            BackgroundColor = Color.Blue;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            BackgroundColor = Color.Green;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Vibration.Vibrate(TimeSpan.FromMilliseconds(10000));
        }

        private async void Button5_Click(object sender, EventArgs e)
        {
            await Flashlight.TurnOnAsync();
            await Task.Delay(1000); // Flash for 1 second
            await Flashlight.TurnOffAsync();
        }

        private async void ButtonLocation_Click(object sender, EventArgs e)
        {
            // Check and request location permission
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status == PermissionStatus.Granted)
            {
                try
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
                    var location = await Geolocation.GetLocationAsync(request);

                    if (location != null)
                    {
                        await DisplayAlert("Ваше местоположение", $"Широта: {location.Latitude}, Долгота: {location.Longitude}, " +
                            $"Точность: {location.Accuracy} метров", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", "Не удалось получить местоположение. Возможно, тайм-аут истек.", "OK");
                    }
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    await DisplayAlert("Ошибка", "Геолокация не поддерживается на этом устройстве. Пожалуйста, проверьте аппаратные возможности.", "OK");
                }
                catch (PermissionException pEx)
                {
                    await DisplayAlert("Ошибка", "Разрешение на доступ к геолокации не предоставлено. Включите его в настройках устройства.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка", "Произошла ошибка при получении местоположения. Подробности: " + ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Разрешение на доступ к геолокации не предоставлено. Пожалуйста, разрешите доступ в настройках приложения.", "OK");
            }
        }
    }
}