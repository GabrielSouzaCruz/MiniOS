using MiniOS.Controllers;
using System;

namespace MiniOS.Views
{
    public class ConsoleView
    {
        private readonly SystemController _controller;

        public ConsoleView(SystemController controller)
        {
            _controller = controller;
        }

        public void Start()
        {
            while (true)
            {
                Console.WriteLine("\n1 - Criar Processo");
                Console.WriteLine("2 - Alocar Memória");
                Console.WriteLine("3 - Criar Arquivo");
                Console.WriteLine("4 - Executar Próximo Processo (Escalonador)");
                Console.WriteLine("5 - Ler Arquivo");
                Console.WriteLine("6 - Ver Estado da RAM");
                Console.WriteLine("7 - Libertar Memória");
                Console.WriteLine("0 - Sair");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Write("Nome do processo: ");
                        var processName = Console.ReadLine();
                        Console.Write("Tempo de execução (ex: 10): ");
                        var executionTime = int.Parse(Console.ReadLine());
                        _controller.CreateProcess(processName, executionTime);
                        break;

                    case "2":
                        Console.Write("Tamanho da memória: ");
                        _controller.AllocateMemory(int.Parse(Console.ReadLine()));
                        break;

                    case "3":
                        Console.Write("Nome do arquivo: ");
                        var name = Console.ReadLine();
                        Console.Write("Conteúdo: ");
                        var content = Console.ReadLine();
                        _controller.CreateFile(name, content);
                        break;
                    case "4":
                        _controller.ExecuteNextProcess();
                        break;
                    case "5":
                        Console.Write("Nome do arquivo a ler: ");
                        var searchName = Console.ReadLine();
                        _controller.ReadFile(searchName);
                        break;
                    case "6":
                        _controller.ShowMemoryStatus();
                        break;

                    case "7":
                        Console.Write("Digite o ID do bloco a libertar: ");
                        var blockId = int.Parse(Console.ReadLine());
                        _controller.FreeMemory(blockId);
                        break;

                    case "0":
                        return;
                }
            }
        }
    }
}