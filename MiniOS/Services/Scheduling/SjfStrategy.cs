using System.Collections.Generic;
using System.Linq;
using MiniOS.Models;

namespace MiniOS.Services.Scheduling
{
    public class SjfStrategy : ISchedulingStrategy
    {
        public Process GetNextProcess(List<Process> readyQueue)
        {
            if (readyQueue.Count == 0)
                return null;

            // Ordena a lista pelo tempo de execução (do menor para o maior) e pega no primeiro
            var shortestProcess = readyQueue.OrderBy(p => p.ExecutionTime).First();

            readyQueue.Remove(shortestProcess);

            return shortestProcess;
        }
    }
}