using MiniOS.Models;
using MiniOS.Hardware;
using MiniOS.Services.Scheduling;
using System;
using System.Collections.Generic;

namespace MiniOS.Services
{
    public class ProcessManager : IProcessManager
    {
        private readonly CPU _cpu;
        private readonly ISchedulingStrategy _scheduler;
        private readonly List<Process> _readyQueue = new();
        private int _idCounter = 1;

        // Injetamos a estratégia pelo construtor (Injeção de Dependência)
        public ProcessManager(CPU cpu, ISchedulingStrategy scheduler)
        {
            _cpu = cpu;
            _scheduler = scheduler;
        }

        public void CreateProcess(string name, int executionTime)
        {
            var process = new Process(_idCounter++, name, executionTime);
            _readyQueue.Add(process);
            Console.WriteLine($"[ProcessManager] Processo '{process.Name}' (Tempo: {process.ExecutionTime}) criado e a aguardar na fila.");
        }
        public void ExecuteNextProcess()
        {
            var next = _scheduler.GetNextProcess(_readyQueue);

            if (next != null)
            {
                _cpu.Execute(next);
            }
            else
            {
                Console.WriteLine("[Escalonador] Nenhum processo na fila de prontos.");
            }
        }
    }
}