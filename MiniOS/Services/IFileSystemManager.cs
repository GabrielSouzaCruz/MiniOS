namespace MiniOS.Services
{
    public interface IFileSystemManager
    {
        void ReadFile(string name); // Novo método!
        System.Collections.Generic.IReadOnlyList<MiniOS.Models.FileEntry> GetAllFiles();
        void DeleteFile(string name);
        void CreateFile(string directory, string name, string content);
    }
}