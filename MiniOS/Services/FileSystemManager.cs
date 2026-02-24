using MiniOS.Hardware;
using MiniOS.Models;

namespace MiniOS.Services
{
    public class FileSystemManager : IFileSystemManager
    {
        private readonly HardDisk _disk;

        public FileSystemManager(HardDisk disk)
        {
            _disk = disk;
        }

        public void CreateFile(string name, string content)
        {
            var file = new FileEntry(name, content);
            _disk.Save(file);
        }
    }
}