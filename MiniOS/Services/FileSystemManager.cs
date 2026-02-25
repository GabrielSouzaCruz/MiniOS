using System.Collections.Generic;
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

        // CORRIGIDO: Agora recebe os 3 parâmetros perfeitamente
        public void CreateFile(string directory, string name, string content)
        {
            _disk.CreateFile(directory, name, content);
        }

        public void ReadFile(string name)
        {
            _disk.ReadFile(name);
        }

        public void DeleteFile(string name)
        {
            _disk.DeleteFile(name);
        }

        public IReadOnlyList<FileEntry> GetAllFiles()
        {
            return _disk.GetAllFiles().AsReadOnly();
        }
    }
}