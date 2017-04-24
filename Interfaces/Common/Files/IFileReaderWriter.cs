using System.IO;

namespace NTouchTypeTrainer.Interfaces.Common.Files
{
    public interface IFileReaderWriter<TData>
    {
        void Write(FileInfo destinationFile, TData data);

        TData Read(FileInfo sourceFile);
    }
}