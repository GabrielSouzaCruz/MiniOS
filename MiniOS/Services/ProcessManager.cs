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

        public void CreateProcess(string name, int executionTime)
        {
            var process = new Process(_processCounter++, name, executionTime);
            _readyQueue.Add(process);
            Console.WriteLine($"[Gestor de Processos] Processo {process.Name} (Tempo: {process.ExecutionTime}) criado e adicionado à fila.");
        }

        public int GetReadyQueueCount()
        {
            return _readyQueue.Count;
        }

        public IReadOnlyList<Process> GetReadyQueue()
        {
            return _readyQueue.AsReadOnly();
        }

        public void ExecuteNextProcess()
        {
            if (_readyQueue.Count == 0)
            {
                Console.WriteLine("[Escalonador] A fila está vazia.");
                return;
            }

            // Agora o C# já sabe quem é o _strategy!
            var process = _strategy.GetNextProcess(_readyQueue);

            if (process != null)
            {
                int quantum = 10; // O nosso "pedaço" de tempo

                Console.WriteLine($"\n[CPU] A executar {process.Name} (Tempo atual: {process.ExecutionTime})...");

                // Simula a execução na CPU descontando o quantum
                process.ExecuteQuantum(quantum);

                if (process.IsFinished)
                {
                    Console.WriteLine($"[CPU] O {process.Name} terminou a sua execução e saiu do sistema!");
                }
                else
                {
                    Console.WriteLine($"[CPU] Fim do Quantum. O {process.Name} ainda precisa de {process.ExecutionTime} de tempo. Volta para a fila!");
                    _readyQueue.Add(process); // Volta para o fim da fila
                }
            }
        }
    }
}