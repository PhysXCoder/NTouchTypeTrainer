using System.IO;

namespace NTouchTypeTrainer.Contracts.Common.Files
{
    public interface IFileStreamProvider
    {
        Stream OpenReadStream(FileInfo fileToRead);
        Stream OpenWriteStream(FileInfo fileToWrite, bool append = false);
    }
}