namespace MiniOS.Models
{
    public class FileEntry
    {
        public string Directory { get; set; } // A pasta onde ele está
        public string Name { get; set; }
        public string Content { get; set; }

        // Esta propriedade junta a pasta e o nome automaticamente!
        public string FullPath => string.IsNullOrWhiteSpace(Directory) ? Name : $"{Directory}/{Name}";

        public FileEntry(string directory, string name, string content)
        {
            Directory = directory;
            Name = name;
            Content = content;
        }
    }
}