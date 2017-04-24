using System.IO;

namespace NTouchTypeTrainer.Interfaces.Common.Files
{
    public interface IFileStreamProvider
    {
        Stream OpenReadStream(FileInfo fileToRead);
        Stream OpenWriteStream(FileInfo fileToWrite, bool append = false);
    }
}