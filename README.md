I'm trying to use a Document Picker in iOS to 'Save As' or create a new file in a document editing app. I then need to get the NSUrl so that future saves can keep saving to that location.

From what I can tell, the Document Picker is supposed to return the DidPickDocumentAtUrls event to tell me the new NSUrl for the moved file when the user clicks Save.

However, instead the document picker WasCancelled event is incorrectly invoked so I can't get the new NSUrl.

I'm following these guides

https://developer.apple.com/documentation/uikit/uidocumentpickerviewcontroller

https://learn.microsoft.com/en-us/xamarin/ios/platform/document-picker

Steps to Reproduce

1. Run my sample app at which is an iOS app that handles .txt files and use a UIDocumentPicker to write a .txt file to a new location

2. Click the Open Document Picker Write button

3. Using breakpoints that if you choose to save in the default directory, it calls the WasCancelled event despite choosing a location. This occurs even if you change the file name.

4 Observe that if you choose to save to dropbox or other cloud storage, the correct event is called which gives you an NSUrl. Very odd. Xamarin bug?
