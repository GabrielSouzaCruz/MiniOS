using System.Collections.Generic;
using System.Linq;
using MiniOS.Models;

namespace MiniOS.Services.Scheduling
{
    public class FifoStrategy : ISchedulingStrategy
    {
        public Process GetNextProcess(List<Process> readyQueue)
        {
            if (readyQueue.Count == 0)
                return null; // Não há processos para executar

            // Pega o primeiro da fila
            var nextProcess = readyQueue.First();

            // Remove da fila de prontos, pois ele vai para a CPU
            readyQueue.Remove(nextProcess);

            return nextProcess;
        }
    }
}