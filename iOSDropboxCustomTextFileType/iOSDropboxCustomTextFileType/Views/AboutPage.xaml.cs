using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iOSDropboxCustomTextFileType.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        public async void OpenDocumentPicker(object o, EventArgs e)
        {
            IDocumentFunctions doc = DependencyService.Get<IDocumentFunctions>();
            await doc.ReadFileDialogAsyncTask();
        }
        public async void OpenDocumentPickerWrite(object o, EventArgs e)
        {
            IDocumentFunctions doc = DependencyService.Get<IDocumentFunctions>();
            doc.WriteFileDialogAsyncTask();
        }
    }
}