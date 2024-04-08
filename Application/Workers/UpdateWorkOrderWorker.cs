using InjectionMoldingMachineDataAdapter.Application.Dtos;
using InjectionMoldingMachineDataAdapter.Application.Notifications;
using InjectionMoldingMachineDataAdapter.Application.Services;
using Microsoft.Extensions.Hosting;

namespace InjectionMoldingMachineDataAdapter.Application.Workers;
public class UpdateWorkOrderWorker : BackgroundService
{
    private readonly InjectionMoldingMachineObserver _machineObserver;
    private readonly MomApiCaller _apiCaller;

    public UpdateWorkOrderWorker(MomApiCaller apiCaller, InjectionMoldingMachineObserver machineObserver)
    {
        _apiCaller = apiCaller;
        _machineObserver = machineObserver;

        _machineObserver.MachineCycleCompleted += UpdateWorkOrderAsync;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _machineObserver.StartAsync();
    }

    private async Task UpdateWorkOrderAsync(MachineCycleCompletedNotification notification)
    {
        var workOrderDto = await _apiCaller.GetEquipmentCurrentWorkOrderAsync(notification.MachineId);

        if (workOrderDto is not null)
        {
            var manufacturingOrderRecord = new CreateManufacturingRecordDto(
                workOrderDto.ManufacturingOrder,
                workOrderDto.WorkOrderId,
                new List<string>() { notification.MachineId },
                notification.Timestamp,
                notification.Timestamp.AddSeconds(notification.MachineCycleBySecond),
                1,
                0);

            await _apiCaller.CreateManufacturingRecord(manufacturingOrderRecord);
        }
    }
}
