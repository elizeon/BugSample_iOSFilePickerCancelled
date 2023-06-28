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

        public async void WriteFileDialogAsyncTask()
        {
            await iOSDocumentPicker.WriteFileDialogAsyncTask(GetNewDocumentUrl("test.txt", "contents"), "contents2");
        }
        public NSUrl GetNewDocumentUrl(string filename, string contents)
        {
            NSFileManager fileManager = NSFileManager.DefaultManager;
            NSUrl documentsUrl = fileManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0];
            filename = System.Net.WebUtility.UrlEncode(filename);
            NSUrl newDocumentUrl = documentsUrl.Append(filename, false);
            newDocumentUrl.StartAccessingSecurityScopedResource();
            bool newFilecreated = fileManager.CreateFile(newDocumentUrl.Path, NSData.FromString(contents), new NSDictionary());
            newDocumentUrl.StopAccessingSecurityScopedResource();
            if (!newFilecreated)
            {
                return null;
            }
            return newDocumentUrl;
        }
    }
}