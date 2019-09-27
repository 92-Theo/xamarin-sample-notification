using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace notification
{
    public delegate void EventHandler();

    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public event EventHandler LocalNotify;
        public event EventHandler TokenGet;

        public MainPage()
        {
            InitializeComponent();
        }

        private void LocalNotify_Clicked(object sender, EventArgs e)
        {
            LocalNotify?.Invoke();
        }
        private void GetToken_Clicked(object sender, EventArgs e)
        {
            TokenGet?.Invoke();
        }


        public void SetTxtShow (string txt)
        {
            txtShow.Text = txt;
        }
    }
}
