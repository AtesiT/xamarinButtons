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
        private void Button5_Click(object sender, EventArgs e)
        {
            Flashlight.TurnOnAsync();
            Flashlight.TurnOffAsync();
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
                    // Use GeolocationRequest for better accuracy and timeout
                    var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
                    var location = await Geolocation.GetLocationAsync(request);

                    if (location != null)
                    {
                        // Display latitude and longitude with more details
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
