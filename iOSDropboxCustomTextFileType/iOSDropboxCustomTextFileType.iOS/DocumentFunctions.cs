using Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSDropboxCustomTextFileType.iOS.DocumentFunctions))]
namespace iOSDropboxCustomTextFileType.iOS
{
    internal class DocumentFunctions : IDocumentFunctions
    {
        public Task<Stream> ReadFileDialogAsyncTask()
        {
            return iOSDocumentPicker.ReadFileDialogAsyncTask();
        }

        public void WriteFileDialogAsyncTask()
        {
            iOSDocumentPicker.WriteFileDialogAsyncTask(GetNewDocumentUrl("test.txt", "contents"), "contents2");
        }
        public NSUrl GetNewDocumentUrl(string filename, string contents)
        {

            NSFileManager fileManager = NSFileManager.DefaultManager;
            NSUrl documentsUrl = fileManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0];
            // YW_TODO: Causes bug! Special characers are destroyed and it causes a crash. HTML maybe? So for now we don't enter a filename.
            filename = System.Net.WebUtility.UrlEncode(filename);
            NSUrl newDocumentUrl = documentsUrl.Append(filename, false);

            fileManager.CreateFile(documentsUrl.Path, NSData.FromString(contents), new NSDictionary());

            return newDocumentUrl;
        }
    }
}