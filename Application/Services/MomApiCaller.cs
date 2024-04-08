using InjectionMoldingMachineDataAdapter.Application.Dtos;
using InjectionMoldingMachineDataAdapter.Infrastructure.Communication;

namespace InjectionMoldingMachineDataAdapter.Application.Services;
public class MomApiCaller
{
    private readonly RestClient _restClient;
    private readonly string _serverUrl;

    public MomApiCaller(RestClient restClient, string serverUrl)
    {
        _restClient = restClient;
        _serverUrl = serverUrl;
    }

    public async Task<WorkOrderDto?> GetEquipmentCurrentWorkOrderAsync(string equipmentId)
    {
        var url = $"{_serverUrl}/api/workorders?WorkCenterId={equipmentId}&Status=3";
        var workOrders = await _restClient.GetAsync<QueryResult<WorkOrderDto>>(url);

        return workOrders?.Items.FirstOrDefault();
    }

    public async Task UpdateWorkOrderProgress(string manufacturingOrderId, string workOrderId, decimal quantity)
    {
        var url = $"{_serverUrl}/api/workorders/{manufacturingOrderId}/{workOrderId}/actualQuantity";

        await _restClient.PutAsync(url, quantity);
    }

    public async Task CreateManufacturingRecord(CreateManufacturingRecordDto createManufacturingRecordDto)
    {
        var url = $"{_serverUrl}/api/manufacturingrecords";

        await _restClient.PostAsync(url, createManufacturingRecordDto);
    }
}
