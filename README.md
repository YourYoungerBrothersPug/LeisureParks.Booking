# LeisureParks

## Running Locally
This solution depends on Azure Service Bus, an emulator is available in the `/asb`. In that folder in terminal, you can run `docker-compose up -d`.

The solution includes a `.slnLaunch` to start both function apps in debug.

Once running, under `LeisureParks.Booking.External` there is a provided `.http` file to be able to send a test payload to the API from within your IDE.

This solution targets .NET 9.

## Key Files
The solution is structured into 4 projects:
* LeisureParks.Booking.External
* LeisureParks.Booking.Internal
* LeisureParks.Booking.Internal.Test
* LeisureParks.Booking.Models

Each project contains a features folder, of which currently there is only the booking workflow.

### LeisureParks.Booking.External\Features\Booking\BookingFunction.cs
* Exposes a HTTP endpoint for external requests.
* This returns accepted and fires a message onto the ASB.

### LeisureParks.Booking.Internal\Features\Booking\Received\BookingReceivedFunction.cs
* Consumes the message from ASB
* Triggers orchestration function

### LeisureParks.Booking.Internal\Features\Booking\Orchestration\BookingOrchestrationFunction.cs
* Orchestrates validation, booking, payment, and email functions
* Logs processing duration

### LeisureParks.Booking.Internal.Test\Features\Booking\Orchestration\BookingOrchestrationFunctionTests.cs
* Contains a test to check invalid bookings receive an email

## Patterns Used
### Clean Architecture
Using a layered architecture e.g., services are behind an interface and so can be swapped out.

Currently, as we have an orchestrator function with separate activity functions. We can test the orchestration logic without implementing the other functions.

In the future when we want to actually send emails, we could have `BookingEmailServiceV2` that implements the same `IBookingEmailService` interface.

### SOLID Principles
#### S - Single-responsibility Principle
Definition: _One and only one reason to change e.g., class should have only one job_

Each of the classes are broken down into small pieces with simple methods within them

#### O - Open-closed Principle
Definition: _Objects open for extension but closed for modification
Add more, don't change existing_

As above with `BookingEmailServiceV2`.

#### L - Liskov Substitution Principle
Definition: _A derived class works in place of the base class_

I haven't needed to work with derived classes.

#### I - Interface Segregation Principle
Definition: _Should never need to implement an unused method_

The interfaces are all clean and simple.

#### D - Dependency Inversion Principle
Definition: _High-level code should depend on an abstraction_

Activity functions are all dependent on their own services.

### Producer-Consumer Pattern
The external facing HTTP function fires a message off that can then be consumed and retried in the background without holding up the request back to the user.

### Retry Pattern
The orchestrator function uses retry policies when calling activity functions. The service bus trigger also will retry a message if there's a wider-failure.