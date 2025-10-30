using Microsoft.Extensions.Logging;

namespace LeisureParks.Booking.Internal.Features.Booking.Payment;

public interface IBookingPaymentService
{
    public Task<BookingPaymentResult> TakePaymentAsync(Guid userId, decimal amount, CancellationToken ct);
}

public class BookingPaymentService(ILogger<BookingPaymentService> logger) : IBookingPaymentService
{
    private readonly ILogger<BookingPaymentService> _logger = logger;

    public async Task<BookingPaymentResult> TakePaymentAsync(Guid userId, decimal amount, CancellationToken ct)
    {
        _logger.LogInformation("Taking payment of {Amount} for {UserId}", amount, userId);

        await Task.Delay(Random.Shared.Next(5000), ct);

        return Random.Shared.Next(10) switch
        {
            0 => throw new Exception("Error occurred"),
            1 => BookingPaymentResult.AlreadyProcessed, // If this has already been processed in a previous attempt of this message
            _ => BookingPaymentResult.Success
        };
    }
}