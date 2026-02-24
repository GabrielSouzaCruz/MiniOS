using MiniOS.Models;
using System;

namespace MiniOS.Hardware
{
    public class CPU
    {
        public void Execute(Process process)
        {
            Console.WriteLine($"CPU executando processo: {process.Name}");
        }
    }
}