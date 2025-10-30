using LeisureParks.Booking.Internal.Features.Booking.BookDates;
using LeisureParks.Booking.Internal.Features.Booking.Email;
using LeisureParks.Booking.Internal.Features.Booking.Payment;
using LeisureParks.Booking.Internal.Features.Booking.Validation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LeisureParks.Booking.Internal;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = FunctionsApplication.CreateBuilder(args);

        builder.ConfigureFunctionsWebApplication();

        builder.Services
            .AddApplicationInsightsTelemetryWorkerService()
            .ConfigureFunctionsApplicationInsights()
            .AddTransient<IBookingValidationService, BookingValidationService>()
            .AddTransient<IBookDatesService, BookDatesService>()
            .AddTransient<IBookingPaymentService, BookingPaymentService>()
            .AddTransient<IBookingEmailService, BookingEmailService>();

        await builder.Build().RunAsync();
    }
}