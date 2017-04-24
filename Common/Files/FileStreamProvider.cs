using NTouchTypeTrainer.Interfaces.Common.Files;
using System.IO;

namespace NTouchTypeTrainer.Common.Files
{
    public class FileStreamProvider : IFileStreamProvider
    {
        public Stream OpenReadStream(FileInfo fileToRead)
        {
            return new FileStream(
                path: fileToRead.FullName,
                mode: FileMode.Open,
                access: FileAccess.Read,
                share: FileShare.Read);
        }

        public Stream OpenWriteStream(FileInfo fileToWrite, bool append = false)
        {
            return new FileStream(
                path: fileToWrite.FullName,
                mode: append ? FileMode.Append : FileMode.OpenOrCreate,
                access: FileAccess.Write,
                share: FileShare.None);
        }
    }
}