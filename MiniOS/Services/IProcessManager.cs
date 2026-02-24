using MiniOS.Models;

namespace MiniOS.Services
{
    public interface IProcessManager
    {
        void CreateProcess(string name, int executionTime);
        void ExecuteNextProcess();
    }
}