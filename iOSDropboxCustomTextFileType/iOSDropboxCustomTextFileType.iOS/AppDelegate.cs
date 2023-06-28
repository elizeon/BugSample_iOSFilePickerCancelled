using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Foundation;
using UIKit;

namespace iOSDropboxCustomTextFileType.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public const string TestFilename = "test.txt";

        public override UIWindow Window { get; set; }
        public bool HasiCloud { get; set; }
        public bool CheckingForiCloud { get; set; }
        public static NSUrl iCloudUrl { get; set; }
        public NSMetadataQuery Query { get; set; }
        public NSData Bookmark { get; set; }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            // Check if the URL scheme and host match the file type you want to handle
            if (url.Scheme == "file")
            {

                iOSDocumentPicker.PickDocUrlRead(url);

                return true;
            }

            return false;
        }
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            ObjCRuntime.Class.ThrowOnInitFailure = false;

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
