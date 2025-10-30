using LeisureParks.Booking.Models.Features.Booking;
using Microsoft.Extensions.Logging;

namespace LeisureParks.Booking.Internal.Features.Booking.BookDates;

public interface IBookDatesService
{
    public Task<BookDatesResult> BookDatesAsync(BookingMessageDto booking, CancellationToken ct);
}

public class BookDatesService(ILogger<BookDatesService> logger) : IBookDatesService
{
    private readonly ILogger<BookDatesService> _logger = logger;

    public async Task<BookDatesResult> BookDatesAsync(BookingMessageDto booking, CancellationToken ct)
    {
        _logger.LogInformation("Booking dates for {UserId} from {StartDate} to {EndDate} for lodge {LodgeCode}", booking.UserId, booking.StartDate, booking.EndDate, booking.LodgeCode);

        await Task.Delay(Random.Shared.Next(5000), ct);

        return Random.Shared.Next(10) switch
        {
            0 => throw new Exception("Error occurred"),
            1 => BookDatesResult.Duplicate,
            2 => BookDatesResult.AlreadyProcessed, // If this has already been processed in a previous attempt of this message
            _ => BookDatesResult.Success
        };
    }
}