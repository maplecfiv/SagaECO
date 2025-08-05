namespace SagaMap.Partner
{
    public interface AICommand //Interface for all AI commands
    {
        CommandStatus Status { get; set; }
        string GetName();
        void Update(object para);
        void Dispose();
    }

    public enum CommandStatus
    {
        INIT,
        RUNNING,
        RUNNING_NOTUPDATE,
        PAUSED,
        FINISHED,
        DELETING
    }
}