using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace iOSDropboxCustomTextFileType
{
    public interface IDocumentFunctions
    {
        Task<Stream> ReadFileDialogAsyncTask();
        void WriteFileDialogAsyncTask();
    }
}
