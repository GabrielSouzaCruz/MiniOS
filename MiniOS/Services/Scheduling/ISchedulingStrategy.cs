using System.Collections.Generic;
using MiniOS.Models;

namespace MiniOS.Services.Scheduling
{
    public interface ISchedulingStrategy
    {
        // Recebe a fila de processos prontos e decide qual será o próximo a executar
        Process GetNextProcess(List<Process> readyQueue);
    }
}