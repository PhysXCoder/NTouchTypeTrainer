using System.IO;
using NTouchTypeTrainer.Contracts.Common.Files;

namespace NTouchTypeTrainer.Common.Files
{
    public class StringFileReaderWriter : IFileReaderWriter<string>
    {
        private readonly IFileStreamProvider _streamProvider;

        public StringFileReaderWriter(IFileStreamProvider streamProvider)
        {
            _streamProvider = streamProvider;
        }

        public void Write(FileInfo destinationFile, string data)
        {
            using (var fileStream = _streamProvider.OpenWriteStream(destinationFile))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(data);
                }
            }
        }

        public string Read(FileInfo sourceFile)
        {
            using (var fileStream = _streamProvider.OpenReadStream(sourceFile))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}