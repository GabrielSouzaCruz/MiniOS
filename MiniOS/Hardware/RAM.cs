using System.Collections.Generic;
using MiniOS.Models;

namespace MiniOS.Hardware
{
    public class RAM
    {
        private readonly List<MemoryBlock> _memory = new();

        public void AddBlock(MemoryBlock block)
        {
            _memory.Add(block);
        }

        public IEnumerable<MemoryBlock> Blocks => _memory;
    }
}