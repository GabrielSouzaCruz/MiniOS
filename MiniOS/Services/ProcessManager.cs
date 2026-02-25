using System;
using System.Collections.Generic;
using MiniOS.Hardware;
using MiniOS.Models;
using MiniOS.Services.Scheduling;

namespace MiniOS.Services
{
    public class ProcessManager : IProcessManager
    {
        private readonly CPU _cpu;
        private readonly ISchedulingStrategy _strategy; // <--- A variável que estava a faltar!
        private readonly List<Process> _readyQueue = new();
        private int _processCounter = 1;

        // O construtor recebe a CPU e a Estratégia
        public ProcessManager(CPU cpu, ISchedulingStrategy strategy)
        {
            _cpu = cpu;
            _strategy = strategy;
        }

        public void CreateProcess(string name, int executionTime, int memoryBlockId)
        {
            var process = new Process(_processCounter++, name, executionTime, memoryBlockId);
            _readyQueue.Add(process);
            Console.WriteLine($"[Gestor de Processos] Processo {process.Name} criado (Usando RAM ID: {memoryBlockId}).");
        }

        public int GetReadyQueueCount()
        {
            return _readyQueue.Count;
        }

        public IReadOnlyList<Process> GetReadyQueue()
        {
            return _readyQueue.AsReadOnly();
        }

        public Process ExecuteNextProcess() // <--- Agora devolve um Process!
        {
            if (_readyQueue.Count == 0) return null;

            var process = _strategy.GetNextProcess(_readyQueue);

            if (process != null)
            {
                int quantum = 10;
                process.ExecuteQuantum(quantum);

                if (process.IsFinished)
                {
                    Console.WriteLine($"[CPU] O {process.Name} terminou a sua execução!");
                    return process; // <--- Devolve o processo que morreu para a RAM ser libertada!
                }
                else
                {
                    _readyQueue.Add(process);
                }
            }
            return null; // Ninguém morreu neste ciclo
        }
    }
}