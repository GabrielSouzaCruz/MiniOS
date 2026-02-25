using System;
using System.Linq;
using MiniOS.Hardware;
using MiniOS.Models;

namespace MiniOS.Services
{
    public class MemoryManager : IMemoryManager
    {
        private readonly RAM _ram;
        private int _idCounter = 1;

        public MemoryManager(RAM ram)
        {
            _ram = ram;
        }

        public void Allocate(int size)
        {
            if (size <= 0)
            {
                Console.WriteLine("[Gestor de Memória] Erro: O tamanho deve ser maior que zero.");
                return;
            }

            // Verifica se há espaço suficiente (Prevenção de Out of Memory)
            if (size > _ram.GetFreeMemory())
            {
                Console.WriteLine($"[Gestor de Memória] Erro (Out of Memory): Espaço insuficiente. Tentou alocar {size}MB, mas apenas {_ram.GetFreeMemory()}MB estão livres.");
                return;
            }

            var block = new MemoryBlock(_idCounter++, size);
            block.Allocate();
            _ram.AddBlock(block);

            Console.WriteLine($"[Gestor de Memória] Bloco {block.Id} alocado com sucesso ({size}MB).");
        }

        public void Free(int blockId)
        {
            var block = _ram.Blocks.FirstOrDefault(b => b.Id == blockId);

            if (block != null && block.IsAllocated)
            {
                block.Free();
                _ram.RemoveBlock(block);
                Console.WriteLine($"[Gestor de Memória] Bloco {blockId} libertado com sucesso ({block.Size}MB recuperados).");
            }
            else
            {
                Console.WriteLine($"[Gestor de Memória] Erro: O Bloco {blockId} não foi encontrado.");
            }
        }

        public void ShowStatus()
        {
            Console.WriteLine("\n=== Estado da RAM ===");
            Console.WriteLine($"Capacidade Total: {_ram.Capacity} MB");
            Console.WriteLine($"Memória Usada:    {_ram.GetUsedMemory()} MB");
            Console.WriteLine($"Memória Livre:    {_ram.GetFreeMemory()} MB");
            Console.WriteLine("Blocos Alocados:");

            var allocatedBlocks = _ram.Blocks.Where(b => b.IsAllocated).ToList();
            if (allocatedBlocks.Count == 0)
            {
                Console.WriteLine("  (Nenhum bloco a ocupar espaço)");
            }
            else
            {
                foreach (var block in allocatedBlocks)
                {
                    Console.WriteLine($"  - Bloco {block.Id}: {block.Size} MB");
                }
            }
            Console.WriteLine("=====================\n");
        }
        public int GetTotalMemory()
        {
            return _ram.Capacity;
        }

        public int GetUsedMemory()
        {
            return _ram.GetUsedMemory();
        }
    }
}