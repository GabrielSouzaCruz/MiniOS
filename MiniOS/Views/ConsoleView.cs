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
                Console.WriteLine("0 - Sair");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Write("Nome do processo: ");
                        _controller.CreateProcess(Console.ReadLine());
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

                    case "0":
                        return;
                }
            }
        }
    }
}