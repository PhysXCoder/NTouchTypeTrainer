using System.IO;
using Eto.Forms;
using NTouchTypeTrainer.Contracts.Views;

namespace NTouchTypeTrainer.Views
{
    public class DialogProvider : IDialogProvider
    {
        public FileInfo OpenFile(string title, string filetypeName, string filetypeExtension, Control parent = null)
        {
            var ofd = new OpenFileDialog()
            {
                Title = title,
                Filters =
                {
                    new FileDialogFilter(filetypeName, filetypeExtension),
                    new FileDialogFilter("All files", "*.*"),
                }
            };

            if (ofd.ShowDialog(parent) == DialogResult.Ok)
            {
                return new FileInfo(ofd.FileName);
            }

            return null;
        }

        public FileInfo SaveFile(string title, string filetypeName, string filetypeExtension, Control parent = null)
        {
            var sfd = new SaveFileDialog()
            {
                Title = title,
                Filters =
                {
                    new FileDialogFilter(filetypeName, filetypeExtension),
                    new FileDialogFilter("All files", "*.*"),
                }
            };

            if (sfd.ShowDialog(parent) == DialogResult.Ok)
            {
                return new FileInfo(sfd.FileName);
            }

            return null;
        }
    }   
}