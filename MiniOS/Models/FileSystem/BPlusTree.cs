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
                // temos de descer pelo filho da DIREITA!
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
                // CORRIGIDO: Agora usa o FullPath para comparar!
                while (i >= 0 && string.Compare(file.FullPath, node.Keys[i]) < 0)
                    i--;

                // CORRIGIDO: Insere a chave usando o FullPath
                node.Keys.Insert(i + 1, file.FullPath);
                node.Values.Insert(i + 1, file);

                // CORRIGIDO: Imprime o FullPath
                Console.WriteLine($"[B+ Tree] Ficheiro '{file.FullPath}' inserido na folha.");
            }
            else
            {
                // CORRIGIDO: É um nó interno, usa FullPath para descobrir para que filho descer
                while (i >= 0 && string.Compare(file.FullPath, node.Keys[i]) < 0)
                    i--;

                i++; // O filho correto está um índice à frente da chave menor

                // Se o filho para onde vamos descer estiver cheio, dividimo-lo primeiro
                if (node.Children[i].Keys.Count == _degree)
                {
                    SplitChild(node, i, node.Children[i]);

                    // Após a divisão, a chave do meio subiu. Precisamos decidir se descemos 
                    // para a metade da esquerda ou da direita. (CORRIGIDO com FullPath)
                    if (string.Compare(file.FullPath, node.Keys[i]) > 0)
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

        public bool Delete(string fileName)
        {
            return DeleteInternal(_root, fileName);
        }

        private bool DeleteInternal(BPlusNode node, string fileName)
        {
            int i = 0;

            if (node.IsLeaf)
            {
                // Procura a posição exata na folha
                while (i < node.Keys.Count && string.Compare(fileName, node.Keys[i]) > 0)
                    i++;

                // Se encontrou, "arranca" a chave e o valor!
                if (i < node.Keys.Count && node.Keys[i] == fileName)
                {
                    node.Keys.RemoveAt(i);
                    node.Values.RemoveAt(i);
                    return true; // Sucesso ao apagar
                }
                return false; // Ficheiro não existe
            }
            else
            {
                // Se é nó interno, usa o GPS para descer pelo caminho certo (maior ou igual vai pra direita)
                while (i < node.Keys.Count && string.Compare(fileName, node.Keys[i]) >= 0)
                    i++;

                return DeleteInternal(node.Children[i], fileName);
            }
        }
    }
}