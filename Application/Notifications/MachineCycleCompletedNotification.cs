namespace InjectionMoldingMachineDataAdapter.Application.Notifications;
public class MachineCycleCompletedNotification
{
    public string MachineId { get; set; }
    public double MachineCycleBySecond { get; set; }
    public DateTime Timestamp { get; set; }

    public MachineCycleCompletedNotification(string machineId, double machineCycleBySecond, DateTime timestamp)
    {
        MachineId = machineId;
        MachineCycleBySecond = machineCycleBySecond;
        Timestamp = timestamp;
    }
}
