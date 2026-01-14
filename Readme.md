# Build and run instructions
.NET 8 is a prerequisite.
The application can be built and run by calling dotnet run from the RedactionAPI directory that holds RedactionAPI.csproj. If desired, a port can 
be specified as a command-line parameter, for example "dotnet run 8082" - if no port is specified, a default of 8080 will be used.

The words to be redacted can be specified as a comma-separated list in appsettings.json within the RedactionSettings collection with the key "BannedWords" 

The log file is written to the directory from which the application is run, with the name redact_log.txt

# Timing and reflection
To implement a solution that met the requirements took approximately 4 hours (from initial commit to "712db63: Modified port configuration to 
use command line argument instead of config file"). Initial creation of this readme took 24 minutes. Improvements since meeting the requirements have so far 
taken 32 minutes. The following areas are those which stand out as less polished than I would
like for reasons of brevity.

## Server host/port configuration
I interpreted the requirement for the port to be configurable at runtime to be a requirement, for the purposes of the exercise, to add a
capability that does not currently exist to specify the port via an argument to the command used to run the application. The current implementation
of that functionality causes the webservice to only accept http communication, and not https. With the Kestrel configuration in appsettings.Development.json
as in commit 317a284: Implemented controllers and configured port to 8080, https communication is possible. The .NET WebApplicationBuilder provides the
capacity to specify host and port for specific protocols via the --urls argument or the ASPNETCORE_URLS environment varaiable.

## Logging
I was surprised to find that .NET does not include a file-writing logging provider by default. I implemented the simplest service that
fulfilled the requirement. Implementing a custom logging provider to implement the ILogger interface would be more consistent with other
logging providers, allowing the provider to be swapped out via DI and use of features such as log levels. in a production scenario, I would
likely investigate the 3rd party logging libraries recommended by the Microsoft.Learn resource, as they would likely provide not only consistency 
but also established solutions to issues such as concurrent writing and log rotation.


