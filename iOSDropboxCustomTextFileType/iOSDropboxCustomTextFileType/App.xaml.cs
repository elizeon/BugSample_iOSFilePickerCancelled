using iOSDropboxCustomTextFileType.Views;
using Xamarin.Forms;

namespace iOSDropboxCustomTextFileType
{
    public partial class App : Application
    {
        public static App Instance { get; private set; }
        public App()
        {
            InitializeComponent();

            MainPage = new AboutPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
