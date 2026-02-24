namespace MiniOS.Services
{
    public interface IFileSystemManager
    {
        void CreateFile(string name, string content);
        void ReadFile(string name); // Novo método!
    }
}