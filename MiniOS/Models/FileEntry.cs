namespace MiniOS.Models
{
    public class FileEntry
    {
        public string Name { get; }
        public string Content { get; }

        public FileEntry(string name, string content)
        {
            Name = name;
            Content = content;
        }
    }
}