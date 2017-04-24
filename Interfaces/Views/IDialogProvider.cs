using System.IO;
using Eto.Forms;

namespace NTouchTypeTrainer.Interfaces.Views
{
    public interface IDialogProvider
    {
        FileInfo OpenFile(string title, string filetypeName, string filetypeExtension, Control parent = null);

        FileInfo SaveFile(string title, string filetypeName, string filetypeExtension, Control parent = null);
    }
}