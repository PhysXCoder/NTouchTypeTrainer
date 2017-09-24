using Microsoft.Win32;
using NTouchTypeTrainer.Common.Files;
using NTouchTypeTrainer.Interfaces.View;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NTouchTypeTrainer.Views
{
    public class DialogProvider : IDialogProvider
    {
        public FileInfo OpenFile(
            string title = null,
            IEnumerable<FileFilter> filters = null,
            DirectoryInfo defaultFolder = null)
        {
            var openFileDialog = new OpenFileDialog() { Multiselect = false };
            InitDialog(openFileDialog, title, filters, defaultFolder);

            var selectedFile = File(openFileDialog);
            return selectedFile;
        }

        public FileInfo SaveFile(
            string title = null,
            IEnumerable<FileFilter> filters = null,
            DirectoryInfo defaultFolder = null)
        {
            var saveFileDialog = new SaveFileDialog();
            InitDialog(saveFileDialog, title, filters, defaultFolder);

            var selectedFile = File(saveFileDialog);
            return selectedFile;
        }

        private static FileInfo File(FileDialog dialog)
        {
            var okPressed = dialog.ShowDialog();

            var selectedFile = (okPressed == true) ? new FileInfo(dialog.FileName) : null;

            return selectedFile;
        }

        private static void InitDialog(
            FileDialog fileDialog,
            string title,
            IEnumerable<FileFilter> filters = null,
            FileSystemInfo defaultFolder = null)
        {
            fileDialog.Title = title;
            fileDialog.Filter = BuildFilterString(filters);
            fileDialog.RestoreDirectory = true;

            if (defaultFolder != null)
            {
                fileDialog.InitialDirectory = defaultFolder.FullName;
            }
        }

        private static string BuildFilterString(IEnumerable<FileFilter> filters)
        {
            var filterString = "All files|*.*";

            if (filters != null)
            {
                filterString = string.Join("|",
                    filters.Select(filt => filt.Description + "|*." + filt.FileExtension));
            }

            return filterString;
        }
    }
}