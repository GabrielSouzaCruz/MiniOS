using System;
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

        // Nova funcionalidade de leitura
        public void ReadFile(string name)
        {
            var file = _disk.Read(name);

            if (file != null)
            {
                Console.WriteLine($"\n[Sistema de Ficheiros] Ficheiro encontrado!");
                Console.WriteLine($"Nome: {file.Name}");
                Console.WriteLine($"Conteúdo: {file.Content}\n");
            }
            else
            {
                Console.WriteLine($"\n[Sistema de Ficheiros] Erro: O ficheiro '{name}' não existe no disco.\n");
            }
        }
        public System.Collections.Generic.IReadOnlyList<FileEntry> GetAllFiles()
        {
            return _disk.GetAllFiles().AsReadOnly();
        }
    }
}