namespace MiniOS.Models
{
    public class Process
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ExecutionTime { get; set; }

        // NOVO: Guarda o ID do bloco de RAM deste processo!
        public int MemoryBlockId { get; set; }

        public Process(int id, string name, int executionTime, int memoryBlockId)
        {
            Id = id;
            Name = name;
            ExecutionTime = executionTime;
            MemoryBlockId = memoryBlockId; // Guarda o bilhete
        }

        public void ExecuteQuantum(int quantum)
        {
            ExecutionTime -= quantum;
            if (ExecutionTime < 0) ExecutionTime = 0;
        }

        public bool IsFinished => ExecutionTime == 0;
    }
}