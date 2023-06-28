using Foundation;
using System;
using System.Text;
using UIKit;

namespace DocPicker
{
    public class GenericTextDocument : UIDocument
    {
        private NSString _dataModel;

        public string Contents
        {
            get { return _dataModel.ToString(); }
            set {
                _dataModel = new NSString(value); 
            }
        }

        public GenericTextDocument(NSUrl url) : base(url)
        {
            // Set the default document text
            this.Contents = "";
        }

        public override bool LoadFromContents(NSObject contents, string typeName, out NSError outError)
        {
            outError = null;

            if (contents is NSData data)
            {
                // Convert the binary data to a string using different encodings
                string fileContents = null;

                // Try different encodings
                foreach (var encodingName in new[] { "utf-8", "utf-16", "unicodeFFFE" })
                {
                    var encoding = Encoding.GetEncoding(encodingName, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);
                    try
                    {
                        fileContents = encoding.GetString(data.ToArray());
                        break; // Successfully decoded, break the loop
                    }
                    catch (DecoderFallbackException)
                    {
                        // Failed to decode with the current encoding, continue with the next one
                    }
                }

                if (fileContents != null)
                {
                    // Process the file contents
                    // ...
                    _dataModel = new NSString(fileContents);
                    return true;
                }
                else
                {
                    // Failed to decode the file contents with any of the attempted encodings
                    //outError = NSError.FromDomain(NSError.OsStatusErrorDomain, (int)errSecInvalidEncoding, null);
                    return false;
                }
            }
            else
            {
                // Invalid contents type
                //outError = NSError.FromDomain(NSError.OsStatusErrorDomain, (int)errSecParam, null);
                return false;
            }
        }

        public override NSObject ContentsForType(string typeName, out NSError outError)
        {
            // Clear the error state
            outError = null;

            // Convert the contents to a NSData object and return it
            NSData docData = _dataModel.Encode(NSStringEncoding.Unicode);
            return docData;
        }

        public override void Open(UIOperationHandler completionHandler)
        {
            base.Open((success) =>
            {
                if (success)
                {
                    // Document opened successfully
                    Console.WriteLine("Document opened successfully.");

                    // Access document contents or perform any other operations

                    completionHandler(true);
                }
                else
                {
                    // Handle the error 
                    Console.WriteLine($"Error opening document");
                    completionHandler(false);
                }
            });
        }
    }
}