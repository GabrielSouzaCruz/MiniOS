namespace MiniOS.Services
{
    public interface IMemoryManager
    {
        void Allocate(int size);
        void Free(int blockId); 
        void ShowStatus();      
    }
}