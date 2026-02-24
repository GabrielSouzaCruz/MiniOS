using MiniOS.Services;

namespace MiniOS.Controllers
{
    public class SystemController
    {
        private readonly Kernel _kernel;

        public SystemController(Kernel kernel)
        {
            _kernel = kernel;
        }

        public void CreateProcess(string name)
        {
            _kernel.ProcessManager.CreateProcess(name);
        }

        public void AllocateMemory(int size)
        {
            _kernel.MemoryManager.Allocate(size);
        }

        public void CreateFile(string name, string content)
        {
            _kernel.FileSystemManager.CreateFile(name, content);
        }
    }
}