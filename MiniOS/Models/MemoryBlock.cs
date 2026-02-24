namespace MiniOS.Models
{
    public class MemoryBlock
    {
        public int Size { get; }
        public bool IsAllocated { get; private set; }

        public MemoryBlock(int size)
        {
            Size = size;
        }

        public void Allocate() => IsAllocated = true;
        public void Free() => IsAllocated = false;
    }
}