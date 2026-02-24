using System.Collections.Generic;

namespace MiniOS.Models.FileSystem
{
    public class BPlusNode
    {
        public bool IsLeaf { get; set; }

        // Chaves de procura (Nomes dos ficheiros)
        public List<string> Keys { get; set; } = new();

        // -----------------------------------------
        // Propriedades para Nós Internos (Índices)
        // -----------------------------------------
        // Apontadores para os nós filhos
        public List<BPlusNode> Children { get; set; } = new();

        // -----------------------------------------
        // Propriedades para Nós Folha (Dados)
        // -----------------------------------------
        // Os ficheiros reais guardados no disco
        public List<FileEntry> Values { get; set; } = new();

        // Numa Árvore B+, as folhas estão ligadas como uma lista ligada
        // para facilitar a leitura sequencial
        public BPlusNode Next { get; set; }

        public BPlusNode(bool isLeaf)
        {
            IsLeaf = isLeaf;
        }
    }
}