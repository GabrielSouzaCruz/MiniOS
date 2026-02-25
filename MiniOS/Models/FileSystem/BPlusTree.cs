using System;
using System.Collections.Generic;

namespace MiniOS.Models.FileSystem
{
    public class BPlusTree
    {
        private BPlusNode _root;
        private readonly int _degree; // Capacidade máxima de chaves por nó

        public BPlusTree(int degree = 3) // Grau 3 por defeito para testes rápidos
        {
            _degree = degree;
            _root = new BPlusNode(true);
        }

        public FileEntry Search(string fileName)
        {
            return SearchInternal(_root, fileName);
        }

        private FileEntry SearchInternal(BPlusNode node, string fileName)
        {
            int i = 0;

            if (node.IsLeaf)
            {
                // Nas folhas, queremos parar exatamente em cima da chave (ou logo a seguir)
                while (i < node.Keys.Count && string.Compare(fileName, node.Keys[i]) > 0)
                    i++;

                // Verifica se encontrou a chave exata
                if (i < node.Keys.Count && node.Keys[i] == fileName)
                    return node.Values[i];

                return null; // Não encontrou
            }
            else
            {
                // Nos NÓS INTERNOS (índices), se a chave for MAIOR ou IGUAL, 
                // temos de descer pelo filho da DIREITA! É aqui que estava o bug.
                while (i < node.Keys.Count && string.Compare(fileName, node.Keys[i]) >= 0)
                    i++;

                return SearchInternal(node.Children[i], fileName);
            }
        }

        // 1. Ponto de entrada da Inserção
        public void Insert(FileEntry file)
        {
            // Se a raiz estiver cheia, a árvore tem de crescer em altura
            if (_root.Keys.Count == _degree)
            {
                var newRoot = new BPlusNode(false);
                newRoot.Children.Add(_root);

                SplitChild(newRoot, 0, _root);
                _root = newRoot;
            }

            InsertNonFull(_root, file);
        }

        // 2. Navega até à folha e insere
        private void InsertNonFull(BPlusNode node, FileEntry file)
        {
            int i = node.Keys.Count - 1;

            if (node.IsLeaf)
            {
                // Encontra a posição correta alfabeticamente
                while (i >= 0 && string.Compare(file.Name, node.Keys[i]) < 0)
                    i--;

                // Insere a chave (nome) e o valor (ficheiro)
                node.Keys.Insert(i + 1, file.Name);
                node.Values.Insert(i + 1, file);
                Console.WriteLine($"[B+ Tree] Ficheiro '{file.Name}' inserido na folha.");
            }
            else
            {
                // É um nó interno, temos de descobrir para que filho descer
                while (i >= 0 && string.Compare(file.Name, node.Keys[i]) < 0)
                    i--;

                i++; // O filho correto está um índice à frente da chave menor

                // Se o filho para onde vamos descer estiver cheio, dividimo-lo primeiro
                if (node.Children[i].Keys.Count == _degree)
                {
                    SplitChild(node, i, node.Children[i]);

                    // Após a divisão, a chave do meio subiu. Precisamos decidir se descemos 
                    // para a metade da esquerda ou da direita
                    if (string.Compare(file.Name, node.Keys[i]) > 0)
                        i++;
                }

                InsertNonFull(node.Children[i], file);
            }
        }

        // 3. O algoritmo de Divisão (Split)
        private void SplitChild(BPlusNode parentNode, int childIndex, BPlusNode fullChild)
        {
            var newNode = new BPlusNode(fullChild.IsLeaf);
            int mid = _degree / 2;

            if (fullChild.IsLeaf)
            {
                // SPLIT DE FOLHA: Mantemos as chaves e valores e ligamos os ponteiros Next
                newNode.Keys.AddRange(fullChild.Keys.GetRange(mid, fullChild.Keys.Count - mid));
                newNode.Values.AddRange(fullChild.Values.GetRange(mid, fullChild.Values.Count - mid));

                fullChild.Keys.RemoveRange(mid, fullChild.Keys.Count - mid);
                fullChild.Values.RemoveRange(mid, fullChild.Values.Count - mid);

                newNode.Next = fullChild.Next;
                fullChild.Next = newNode;

                // Copia a primeira chave do novo nó folha para o pai
                parentNode.Keys.Insert(childIndex, newNode.Keys[0]);
                parentNode.Children.Insert(childIndex + 1, newNode);
            }
            else
            {
                // SPLIT DE NÓ INTERNO: Promove a chave do meio e não a duplica
                newNode.Keys.AddRange(fullChild.Keys.GetRange(mid + 1, fullChild.Keys.Count - (mid + 1)));
                newNode.Children.AddRange(fullChild.Children.GetRange(mid + 1, fullChild.Children.Count - (mid + 1)));

                var keyToPromote = fullChild.Keys[mid];

                fullChild.Keys.RemoveRange(mid, fullChild.Keys.Count - mid);
                fullChild.Children.RemoveRange(mid + 1, fullChild.Children.Count - (mid + 1));

                parentNode.Keys.Insert(childIndex, keyToPromote);
                parentNode.Children.Insert(childIndex + 1, newNode);
            }

            Console.WriteLine("[B+ Tree] Split realizado com sucesso.");
        }
        public List<FileEntry> GetAllFiles()
        {
            var result = new List<FileEntry>();
            var current = _root;

            // 1. Desce até à folha mais à esquerda
            while (!current.IsLeaf)
            {
                current = current.Children[0];
            }

            // 2. Caminha pelas folhas usando o ponteiro 'Next' recolhendo os ficheiros
            while (current != null)
            {
                result.AddRange(current.Values);
                current = current.Next;
            }

            return result;
        }
    }
}