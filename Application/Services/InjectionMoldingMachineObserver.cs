using InjectionMoldingMachineDataAdapter.Application.Dtos;
using InjectionMoldingMachineDataAdapter.Application.Notifications;
using InjectionMoldingMachineDataAdapter.Infrastructure.Communication;
using Newtonsoft.Json;

namespace InjectionMoldingMachineDataAdapter.Application.Services;
public class InjectionMoldingMachineObserver
{
    private readonly ManagedMqttClient _mqttClient;

    public event Func<MachineCycleCompletedNotification, Task>? MachineCycleCompleted;

    public InjectionMoldingMachineObserver(ManagedMqttClient mqttClient)
    {
        _mqttClient = mqttClient;
        _mqttClient.MessageReceived += OnMqttClientMessageReceivedAsync;
    }

    public async Task StartAsync()
    {
        await _mqttClient.ConnectAsync();

        await _mqttClient.Subscribe("IMM/+/Metric/+");
    }

    private async Task OnMqttClientMessageReceivedAsync(MqttMessage e)
    {
        var topic = e.Topic;
        var payloadMessage = e.Payload;

        if (topic is null || payloadMessage is null)
        {
            return;
        }

        var topicSegments = topic.Split('/');
        var machineId = topicSegments[1];

        var metrics = JsonConvert.DeserializeObject<List<MetricMessage>>(payloadMessage);

        if (metrics is null)
        {
            return;
        }

        foreach (var metric in metrics)
        {
            if (metric is null)
            {
                continue;
            }

            if (metric.Name == "injectionCycle" && MachineCycleCompleted is not null)
            {
                var injectionCycle = (double)metric.Value;
                var notification = new MachineCycleCompletedNotification(machineId, injectionCycle, metric.Timestamp);

                await MachineCycleCompleted.Invoke(notification);
            }
        }
    }
}
