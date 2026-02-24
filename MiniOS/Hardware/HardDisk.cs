using System.Collections.Generic;
using MiniOS.Models;

namespace MiniOS.Hardware
{
    public class HardDisk
    {
        private readonly List<FileEntry> _files = new();

        public void Save(FileEntry file)
        {
            _files.Add(file);
        }

        public IEnumerable<FileEntry> Files => _files;
    }
}