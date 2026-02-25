namespace MiniOS.Services
{
    public interface IMemoryManager
    {
        int Allocate(int size);
        void Free(int blockId); 
        void ShowStatus();
        int GetTotalMemory();
        int GetUsedMemory();
    }
}