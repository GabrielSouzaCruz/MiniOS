using MiniOS.Models;
using MiniOS.Hardware;
using System.Collections.Generic;

namespace MiniOS.Services
{
    public class ProcessManager : IProcessManager
    {
        private readonly CPU _cpu;
        private readonly List<Process> _processes = new();
        private int _idCounter = 1;

        public ProcessManager(CPU cpu)
        {
            _cpu = cpu;
        }

        public void CreateProcess(string name)
        {
            var process = new Process(_idCounter++, name);
            _processes.Add(process);
            _cpu.Execute(process);
        }
    }
}