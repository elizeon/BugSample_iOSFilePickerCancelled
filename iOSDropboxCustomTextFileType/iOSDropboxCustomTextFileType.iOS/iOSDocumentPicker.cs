﻿using DocPicker;
using Foundation;
using iOSDropboxCustomTextFileType;
using MobileCoreServices;
using System;
using System.IO;
using System.Threading.Tasks;
using UIKit;


//https://learn.microsoft.com/en-au/xamarin/ios/platform/document-picker

public class iOSDocumentPicker : Foundation.NSObject
{
    public async Task<GenericTextDocument> OpenDocument(NSUrl url)
    {

        var Document = new GenericTextDocument(url);

        // Open the document
        TaskCompletionSource<bool> tcs = new System.Threading.Tasks.TaskCompletionSource<bool>();
        Document.Open((success) => {
            if (success)
            {
                App.Instance.MainPage.DisplayAlert("Contents",Document.Contents,"OK");
                tcs.SetResult(true);
            }
            else
            {
                tcs.SetResult(false);
            }
        });

        await tcs.Task;

        return Document;
    }

    public static async Task WriteFileDialogAsyncTask(NSUrl suggestedNSUrl, string contents)
    {
        var docPicker = new UIDocumentPickerViewController(suggestedNSUrl, UIDocumentPickerMode.ExportToService);

        // Set event handlers
        // Note that DidPickDocumentAt is depreciated so we don't use that event. https://developer.apple.com/documentation/uikit/uidocumentpickerdelegate/1618680-documentpicker
        docPicker.DidPickDocumentAtUrls += delegate (object o, UIDocumentPickedAtUrlsEventArgs e) { OnDocPickerFinishedPickingAtUrlsWRITE(o, e, contents); };
        docPicker.WasCancelled += OnDocPickerCancelled;

        // Present file picker
        UIWindow window = UIApplication.SharedApplication.KeyWindow;
        var viewController = window.RootViewController;
        viewController.PresentViewController(docPicker, true, null);

        // Await TCS whose completion is set by the pick document event.
        writeTaskCompletionSource = new TaskCompletionSource<GenericTextDocument>();
        var doc = await writeTaskCompletionSource.Task;

        doc.Contents = contents;

        // Save document to path
        doc.Save(doc.FileUrl, UIDocumentSaveOperation.ForCreating, (saveSuccess) => {
            if (saveSuccess)
            {
            }
            else
            {
                Console.WriteLine("Unable to Save Document " + doc.FileUrl);
            }
        });
    }
    public NSUrl GetNewDocumentUrl(string filename, string contents)
    {
        NSFileManager fileManager = NSFileManager.DefaultManager;
        NSUrl documentsUrl = fileManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0];
        NSUrl newDocumentUrl = documentsUrl.Append(filename, false);

        fileManager.CreateFile(newDocumentUrl.Path, NSData.FromString(contents), new NSDictionary());

        return newDocumentUrl;
    }

    public async Task<GenericTextDocument> CreateNewDocument(string filename, string contents)
    {
        NSUrl documentsUrl = GetNewDocumentUrl(filename, contents);

        // Pick the document and set contents
        await WriteFileDialogAsyncTask(documentsUrl, contents);
    }
    public class FileReference
    {
        public FileReference(string key)
        {
            Key = key;
        }
        public string Key;

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Key);
        }
    }
    public static async Task<bool> SaveDocument(FileReference file, string newContents)
    {
        try
        {
            NSUrl url = NSUrl.FromString(file.Key);
            //url.StartAccessingSecurityScopedResource();
            var Document = new GenericTextDocument(url);
            Document.Contents = newContents;


            // Save document to path
            TaskCompletionSource<bool> tcs = new System.Threading.Tasks.TaskCompletionSource<bool>();
            Document.Save(Document.FileUrl, UIDocumentSaveOperation.ForOverwriting, (saveSuccess) => {
                if (saveSuccess)
                {
                    tcs.SetResult(true);
                }
                else
                {
                    tcs.SetResult(false);
                }
            });

            // Return results

            var val = await tcs.Task;
            //url.StopAccessingSecurityScopedResource();
            return val;
        }
        catch (Exception ex)
        {
            return false;
        }

    }
    static async void PickDocUrlWrite(NSUrl url, string contents)
    {
        var fref = new FileReference(url.ToString());
        bool success = await SaveDocument(fref, contents);
    }
    /// <summary>
    /// Saves the document.
    /// </summary>
    /// <returns><c>true</c>, if document was saved, <c>false</c> otherwise.</returns>
    public static async Task<bool> SaveDocument(string key, string newContents)
    {

        var Document = new GenericTextDocument(NSUrl.FromString(key));
        Document.Contents = newContents;

        var securityEnabled = Document.FileUrl.StartAccessingSecurityScopedResource();

        // Save document to path
        TaskCompletionSource<bool> tcs = new System.Threading.Tasks.TaskCompletionSource<bool>();
        Document.Save(Document.FileUrl, UIDocumentSaveOperation.ForOverwriting, (saveSuccess) => {
            if (saveSuccess)
            {
                tcs.SetResult(true);
                Document.FileUrl.StopAccessingSecurityScopedResource();
            }
            else
            {
                tcs.SetResult(false);
                Document.FileUrl.StopAccessingSecurityScopedResource();
            }
        });

        // Return results

        return await tcs.Task;

    }

    static TaskCompletionSource<Stream> readTaskCompletionSource;
    static TaskCompletionSource<GenericTextDocument> writeTaskCompletionSource;
    public static Task<Stream> ReadFileDialogAsyncTask()
    {
        //string yw7 = GetYWriterSupportedFormats()[0];
        string plain = UTType.PlainText;
        var docPicker = new UIDocumentPickerViewController(new string[] {"com.spacejock.ywriter.yw7", "text/plain", UTType.Data, UTType.Content, UTType.Text, UTType.Application, UTType.ApplicationFile}, UIDocumentPickerMode.Open);


        // Set event handlers
        docPicker.DidPickDocument += OnDocPickerFinishedPickingREAD;
        docPicker.DidPickDocumentAtUrls += OnDocPickerFinishedPickingAtUrlsREAD;
        docPicker.WasCancelled += OnDocPickerCancelled;

        // Present UIImagePickerController;
        UIWindow window = UIApplication.SharedApplication.KeyWindow;
        var viewController = window.RootViewController;
        viewController.PresentViewController(docPicker, true, null);

        // Return Task object
        readTaskCompletionSource = new TaskCompletionSource<Stream>();
        return readTaskCompletionSource.Task;
    }
    static async void OnDocPickerCancelled(object sender, EventArgs args)
    {
        await App.Instance.MainPage.DisplayAlert("Contents", "test", "Cancel");
    }
    static void OnDocPickerFinishedPickingREAD(object sender, UIDocumentPickedEventArgs pArgs)
    {
        iOSDocumentPicker picker = new iOSDocumentPicker();
        PickDocUrlRead(pArgs.Url);
        readTaskCompletionSource.SetResult(null);
    }
    public static async void PickDocUrlRead(NSUrl url)
    {

        // IMPORTANT! You must lock the security scope before you can
        // access this file
        var securityEnabled = url.StartAccessingSecurityScopedResource();

        // Open the document
        iOSDocumentPicker picker = new iOSDocumentPicker();
        var doc = await picker.OpenDocument(url);

        await App.Instance.MainPage.DisplayAlert("Contents", doc.Contents, "Cancel");

        // IMPORTANT! You must release the security lock established
        // above.
        url.StopAccessingSecurityScopedResource();
    }
    static void OnDocPickerFinishedPickingAtUrlsREAD(object sender, UIDocumentPickedAtUrlsEventArgs pArgs)
    {
        PickDocUrlRead(pArgs.Urls[0]);
        readTaskCompletionSource.SetResult(null);
    }

    /// <summary>
    /// Write the file contents to the URL
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="pArgs"></param>
    static void OnDocPickerFinishedPickingAtUrlsWRITE(object sender, UIDocumentPickedAtUrlsEventArgs pArgs, string contents)
    {
        PickDocUrlWrite(pArgs.Urls[0], contents);
        GenericTextDocument doc = new GenericTextDocument(pArgs.Urls[0]);
        doc.Contents = contents;
        writeTaskCompletionSource.SetResult(doc);
    }
}