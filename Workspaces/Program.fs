namespace Workspaces
#nowarn "20"
open Asp.Versioning
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Workspaces.Controllers
open Microsoft.Extensions.Logging
open RabbitMQ.Client
open RabbitMQ.Client.Events

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =
        //https://github.com/edgarsanchez/FsRabbitMQ-Tutorials/tree/master/src/Receive
        //let factory = ConnectionFactory (HostName = "localhost")
        //use connection = factory.CreateConnection ()
        //use channel = connection.CreateModel ()
    
        //channel.QueueDeclare (queue = "hello", durable = false, exclusive = false, autoDelete = false, arguments = null) |> ignore
        //printfn " [*] Waiting for messages."

        //let consumer = EventingBasicConsumer (channel)
        //consumer.Received.AddHandler (fun _ ea ->
        //    let body = ea.Body.ToArray ()
        //    let message = Encoding.UTF8.GetString body
        //    printfn " [x] Received %s" message )
        //channel.BasicConsume (queue = "hello", autoAck = true, consumer = consumer) |> ignore

        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddEndpointsApiExplorer()
        builder.Services.AddSwaggerGen()
        builder.Services.AddControllers()
        builder.Services.AddApiVersioning(fun options ->
            options.DefaultApiVersion <- ApiVersion(1, 0)
            options.AssumeDefaultVersionWhenUnspecified <- true
            options.ReportApiVersions <- true
        )
        |> ignore

        let app = builder.Build()

        app.UseSwagger()
        app.UseSwaggerUI()

        app.UseHttpsRedirection()

        app.UseAuthorization()

        app.MapControllers()

        //Dapper.FSharp.MySQL.OptionTypes.register()
        app.Run()

        exitCode
