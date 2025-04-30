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
            Vibration.Vibrate(TimeSpan.FromMilliseconds(1000));
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            Flashlight.TurnOnAsync();
            Flashlight.TurnOffAsync();
        }
    }
}
