namespace MiniOS.Models
{
    public class Process
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ExecutionTime { get; set; }

        public Process(int id, string name, int executionTime)
        {
            Id = id;
            Name = name;
            ExecutionTime = executionTime;
        }

        // NOVO: Método para reduzir o tempo de execução
        public void ExecuteQuantum(int quantum)
        {
            ExecutionTime -= quantum;
            if (ExecutionTime < 0)
            {
                ExecutionTime = 0; // Garante que não fica negativo
            }
        }

        // NOVO: Propriedade que nos diz se o processo já acabou
        public bool IsFinished => ExecutionTime == 0;
    }
}