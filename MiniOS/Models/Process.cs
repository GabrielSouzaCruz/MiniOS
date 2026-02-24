namespace MiniOS.Models
{
    public class Process
    {
        public int Id { get; }
        public string Name { get; }
        public int ExecutionTime { get; }

        public Process(int id, string name, int executionTime)
        {
            Id = id;
            Name = name;
            ExecutionTime = executionTime;
        }
    }
}