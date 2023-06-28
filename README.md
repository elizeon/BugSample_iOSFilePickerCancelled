# BugSample_iOSDropboxCustomTextFileType

Hi,



I am updating a Xamarin.iOS app to open and edit files of the custom text type '.yw7' using a UIDocumentPicker. However, I'm unable to select files of my custom file type in Dropbox - they are greyed out with a question mark icon. The same code works just fine for iCloud and Google Drive.



I have defined the .yw7 type in my info.plist which I hoped would allow me to pick a .yw7 file in the UIDocumentPicker, but the only effect this config seems to have is allowing my app to be selected as an Open With type in the Dropbox app itself - the UIDocumentPicker from within my app still doesn't work.



Because it needs to be compatible with the desktop app, I cannot change from .yw7 to a normal text extension.



I also don't want to use Dropbox-specific code, I don't want to handle each cloud storage individually if I can help it. Especially since Dropbox API support in Xamarin is very limited.



I'd appreciate any advice from community or support about this. I put a quick sample project together to demo the issue which can be downloaded here: https://github.com/ywriterapp/BugSample_iOSDropboxCustomTextFileType
