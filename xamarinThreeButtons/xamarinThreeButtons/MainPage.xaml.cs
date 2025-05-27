using System;
using System.Threading.Tasks;
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

        private async void Button5_Click(object sender, EventArgs e) 
        {
            await Flashlight.TurnOnAsync();
            await Task.Delay(2000); // Задержка в 2 секунды
            await Flashlight.TurnOffAsync();
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
            var data = e.Reading;
            // Вывод значений в консоль для отладки
            Console.WriteLine($"Reading: X: {data.Acceleration.X}, Y: {data.Acceleration.Y}, Z: {data.Acceleration.Z}");
            // Используем значения ускорения напрямую как компоненты RGB
            // Преобразуем значения из диапазона [-1, 1] в [0, 1] для корректного отображения цвета
            double r = (data.Acceleration.X + 1) / 2;
            double g = (data.Acceleration.Y + 1) / 2;
            double b = (data.Acceleration.Z + 1) / 2;
            // Устанавливаем цвет фона на основе значений акселерометра
            BackgroundColor = Color.FromRgb(r, g, b);
        }
    }
}
