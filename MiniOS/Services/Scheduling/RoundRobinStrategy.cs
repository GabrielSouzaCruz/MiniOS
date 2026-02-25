using System.Collections.Generic;
using MiniOS.Models;

namespace MiniOS.Services.Scheduling
{
    public class RoundRobinStrategy : ISchedulingStrategy
    {
        public Process GetNextProcess(List<Process> readyQueue)
        {
            if (readyQueue.Count == 0) return null;

            // No Round-Robin, pegamos simplesmente no primeiro da fila (FIFO)
            var process = readyQueue[0];
            readyQueue.RemoveAt(0); // Tira da fila provisoriamente
            return process;
        }
    }
}