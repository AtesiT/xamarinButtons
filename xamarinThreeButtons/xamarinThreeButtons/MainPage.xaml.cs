using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace xamarinThreeButtons
{
    public partial class MainPage : ContentPage
    {
        private bool isAccelerometerActive = false;

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
            Vibration.Vibrate(TimeSpan.FromMilliseconds(5000));
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Flashlight.TurnOnAsync();
            Flashlight.TurnOffAsync();
        }

        private async void ButtonLocation_Click(object sender, EventArgs e)
        {
            // Проверка и запрос разрешения на доступ к геолокации
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status == PermissionStatus.Granted)
            {
                try
                {
                    // Используем GeolocationRequest для большей точности и таймаута
                    var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
                    var location = await Geolocation.GetLocationAsync(request);

                    if (location != null)
                    {
                        // Отображаем широту и долготу
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

        private void ButtonAccelerometer_Click(object sender, EventArgs e)
        {
            if (!isAccelerometerActive)
            {
                StartAccelerometer();
            }
            else
            {
                StopAccelerometer();
            }
        }

        private void StartAccelerometer()
        {
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Accelerometer.Start(SensorSpeed.UI);
            isAccelerometerActive = true;
        }

        private void StopAccelerometer()
        {
            Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
            Accelerometer.Stop();
            isAccelerometerActive = false;
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var reading = e.Reading.Acceleration;

            // Устанавливаем цвет фона в зависимости от значений акселерометра
            if (Math.Abs(reading.X) > Math.Abs(reading.Y) && Math.Abs(reading.X) > Math.Abs(reading.Z))
            {
                // Если X больше, меняем на красный
                BackgroundColor = Color.Red;
            }
            else if (Math.Abs(reading.Y) > Math.Abs(reading.X) && Math.Abs(reading.Y) > Math.Abs(reading.Z))
            {
                // Если Y больше, меняем на зеленый
                BackgroundColor = Color.Green;
            }
            else if (Math.Abs(reading.Z) > Math.Abs(reading.X) && Math.Abs(reading.Z) > Math.Abs(reading.Y))
            {
                // Если Z больше, меняем на синий
                BackgroundColor = Color.Blue;
            }
        }
    }
}
