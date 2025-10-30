using Azure.Messaging.ServiceBus;
using LeisureParks.Booking.Internal.Features.Booking.Orchestration;
using LeisureParks.Booking.Models.Features.Booking;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace LeisureParks.Booking.Internal.Features.Booking.Received;

public class BookingReceivedFunction(ILogger<BookingReceivedFunction> logger)
{
    private readonly ILogger<BookingReceivedFunction> _logger = logger;

	[Function(nameof(BookingReceivedFunction))]
    public async Task RunAsync([ServiceBusTrigger("bookings", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions, [DurableClient] DurableTaskClient client)
    {
        _logger.LogInformation("Received message {Id} with delivery count {DeliveryCount}", message.MessageId, message.DeliveryCount);

        var booking = JsonSerializer.Deserialize<BookingMessageDto>(message.Body);

        var instanceId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(BookingOrchestrationFunction), booking);
        var response = await client.WaitForInstanceCompletionAsync(instanceId);

        if (response.RuntimeStatus is OrchestrationRuntimeStatus.Failed)
            await messageActions.AbandonMessageAsync(message); // Makes the message ready to retry
        else
            await messageActions.CompleteMessageAsync(message);

        // Should send email if reaches max retry and alert operations with Azure Monitor
    }
}