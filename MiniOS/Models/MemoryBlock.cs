namespace MiniOS.Models
{
    public class MemoryBlock
    {
        public int Id { get; }
        public int Size { get; }
        public bool IsAllocated { get; private set; }

        public MemoryBlock(int id, int size)
        {
            Id = id;
            Size = size;
        }

        public void Allocate() => IsAllocated = true;
        public void Free() => IsAllocated = false;
    }
}