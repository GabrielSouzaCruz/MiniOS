namespace MiniOS.Models
{
    public class FileEntry
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public FileEntry(string name, string content)
        {
            Name = name;
            Content = content;
        }
    }
}