namespace InjectionMoldingMachineDataAdapter.Application.Dtos;
public class CreateManufacturingRecordDto
{
    public string ManufacturingOrderId { get; set; }
    public string WorkOrderId { get; set; }
    public List<string> EquipmentIds { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal Output { get; set; }
    public decimal Defects { get; set; }

    public CreateManufacturingRecordDto(string manufacturingOrderId, string workOrderId, List<string> equipmentIds, DateTime startTime, DateTime endTime, decimal output, decimal defects)
    {
        ManufacturingOrderId = manufacturingOrderId;
        WorkOrderId = workOrderId;
        EquipmentIds = equipmentIds;
        StartTime = startTime;
        EndTime = endTime;
        Output = output;
        Defects = defects;
    }
}
