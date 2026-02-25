using MiniOS.Models;
using MiniOS.Models.FileSystem;

namespace MiniOS.Hardware
{
    public class HardDisk
    {
        // O disco agora é gerido por uma Árvore B+ com capacidade de 3 elementos por nó (para forçar splits rápidos e testarmos)
        private readonly BPlusTree _fileIndex = new BPlusTree(3);

        public void Save(FileEntry file)
        {
            _fileIndex.Insert(file);
        }

        // Método para ler um ficheiro rapidamente através da árvore
        public FileEntry Read(string fileName)
        {
            return _fileIndex.Search(fileName);
        }
        public List<FileEntry> GetAllFiles()
        {
            return _fileIndex.GetAllFiles();
        }
    }
}