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
        public void CreateFile(string directory, string name, string content)
        {
            var file = new FileEntry(directory, name, content);

            _fileIndex.Insert(file);

            Console.WriteLine($"[Disco] Ficheiro guardado em: {file.FullPath}");
        }

        // Método para ler um ficheiro rapidamente através da árvore
        public void ReadFile(string name)
        {
            // O Search vai procurar pelo FullPath (ex: "Root/Ficheiro.txt")
            var file = _fileIndex.Search(name);

            if (file != null)
            {
                Console.WriteLine($"\n[Disco] A LER FICHEIRO: {file.FullPath}");
                Console.WriteLine($"[Conteúdo] {file.Content}\n");
            }
            else
            {
                Console.WriteLine($"[Disco] Erro: O ficheiro '{name}' não existe no disco.");
            }
        }
        public List<FileEntry> GetAllFiles()
        {
            return _fileIndex.GetAllFiles();
        }
        public void DeleteFile(string name)
        {
            bool success = _fileIndex.Delete(name);
            if (success)
                Console.WriteLine($"[Disco] O ficheiro '{name}' foi obliterado com sucesso!");
            else
                Console.WriteLine($"[Disco] Erro: O ficheiro '{name}' não foi encontrado para apagar.");
        }
    }
}