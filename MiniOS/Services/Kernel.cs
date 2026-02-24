namespace MiniOS.Services
{
    public class Kernel
    {
        public IProcessManager ProcessManager { get; }
        public IMemoryManager MemoryManager { get; }
        public IFileSystemManager FileSystemManager { get; }

        public Kernel(
            IProcessManager processManager,
            IMemoryManager memoryManager,
            IFileSystemManager fileSystemManager)
        {
            ProcessManager = processManager;
            MemoryManager = memoryManager;
            FileSystemManager = fileSystemManager;
        }
    }
}