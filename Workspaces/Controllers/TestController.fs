namespace Workspaces.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Http
open WebServiceM2Lib.Mapping.tables
open WebServiceM2Lib.Database
open RabbitMQ.Client
open System
open System.Text

[<ApiController>]
[<Route("[controller]")>]
type TestController (logger : ILogger<TestController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() =
        let factory = new ConnectionFactory()
        factory.HostName <- "localhost"
        use connection = factory.CreateConnection()
        use channel = connection.CreateModel()

        channel.ExchangeDeclare("logs", ExchangeType.Direct)

        let message = "info: Hello World!"
        let body = Encoding.UTF8.GetBytes(message)
        channel.BasicPublish("logs", String.Empty , null, body)
        Console.WriteLine($" [x] Sent {message}")

        

        