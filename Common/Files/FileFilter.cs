namespace NTouchTypeTrainer.Common.Files
{
    public class FileFilter
    {
        public string Description { get; set; }

        public string FileExtension { get; set; }

        public static readonly FileFilter AllFiles = new FileFilter("All files", "*");

        public FileFilter()
        { }

        public FileFilter(string description, string fileExtension)
        {
            Description = description;
            FileExtension = fileExtension;
        }

        public override string ToString()
            => Description + "|*." + FileExtension;
    }
}