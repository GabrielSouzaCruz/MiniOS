using System.Collections.Generic;
using System.Linq;
using MiniOS.Models;

namespace MiniOS.Hardware
{
    public class RAM
    {
        public int Capacity { get; }
        private readonly List<MemoryBlock> _memory = new();

        public RAM(int capacity = 1024) // 1024 MB por defeito
        {
            Capacity = capacity;
        }

        public int GetUsedMemory()
        {
            // Soma o tamanho de todos os blocos que estão alocados
            return _memory.Where(b => b.IsAllocated).Sum(b => b.Size);
        }

        public int GetFreeMemory()
        {
            return Capacity - GetUsedMemory();
        }

        public void AddBlock(MemoryBlock block)
        {
            _memory.Add(block);
        }

        public void RemoveBlock(MemoryBlock block)
        {
            _memory.Remove(block);
        }

        public IEnumerable<MemoryBlock> Blocks => _memory;
    }
}