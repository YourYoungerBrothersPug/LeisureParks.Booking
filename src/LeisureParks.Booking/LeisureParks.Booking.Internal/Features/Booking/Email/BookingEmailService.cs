using Microsoft.Extensions.Logging;

namespace LeisureParks.Booking.Internal.Features.Booking.Email;

public interface IBookingEmailService
{
    public Task SendEmailAsync(Guid userId, string contents, CancellationToken ct);
}

public class BookingEmailService(ILogger<BookingEmailService> logger) : IBookingEmailService
{
    private readonly ILogger<BookingEmailService> _logger = logger;

    public async Task SendEmailAsync(Guid userId, string contents, CancellationToken ct)
    {
        _logger.LogInformation("Send email to {UserId} with content {Contents}", userId, contents);
    }
}