using MiniOS.Models;
using System.Collections.Generic;


namespace MiniOS.Services
{
    public interface IProcessManager
    {
        void CreateProcess(string name, int executionTime, int memoryBlockId);
        Process ExecuteNextProcess();
        int GetReadyQueueCount();
        IReadOnlyList<Process> GetReadyQueue();
    }
}