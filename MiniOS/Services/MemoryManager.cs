using MiniOS.Hardware;
using MiniOS.Models;
using System.Buffers;

namespace MiniOS.Services
{
    public class MemoryManager : IMemoryManager
    {
        private readonly RAM _ram;

        public MemoryManager(RAM ram)
        {
            _ram = ram;
        }

        public void Allocate(int size)
        {
            var block = new MemoryBlock(size);
            block.Allocate();
            _ram.AddBlock(block);
        }
    }
}