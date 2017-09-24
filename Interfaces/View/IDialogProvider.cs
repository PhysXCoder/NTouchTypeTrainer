using NTouchTypeTrainer.Common.Files;
using System.Collections.Generic;
using System.IO;

namespace NTouchTypeTrainer.Interfaces.View
{
    public interface IDialogProvider
    {
        FileInfo OpenFile(
            string title = null,
            IEnumerable<FileFilter> filters = null,
            DirectoryInfo defaultFolder = null);

        FileInfo SaveFile(
            string title = null,
            IEnumerable<FileFilter> filters = null,
            DirectoryInfo defaultFolder = null);
    }
}