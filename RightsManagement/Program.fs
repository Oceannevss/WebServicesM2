namespace RightsManagement
#nowarn "20"
open RabbitMQ.Client.Events

open System.Text

open RabbitMQ.Client

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddControllers()

        builder.Services.AddEndpointsApiExplorer()
        builder.Services.AddSwaggerGen()

        let app = builder.Build()

        app.UseSwagger()
        app.UseSwaggerUI()

        app.UseHttpsRedirection()

        app.UseAuthorization()

        app.MapControllers()

        Dapper.FSharp.MySQL.OptionTypes.register()
        //let factory = new ConnectionFactory()
        //factory.HostName <- "localhost"
        //use connection = factory.CreateConnection()
        //use channel = connection.CreateModel()

        //channel.ExchangeDeclare("logs", ExchangeType.Direct)

        //let queueName = channel.QueueDeclare().QueueName
        //channel.QueueBind(queueName, "logs", String.Empty)
        //Console.WriteLine("[*] Waiting for logs.")

        //let consumer = new EventingBasicConsumer(channel)
        //consumer.Received.Add(fun (model, ea)->
        //    let body = ea.Body.ToArray()
        //    let message = Encoding.UTF8.GetString(body)
        //    printfn " [x] %s" message)

        //channel.BasicConsume(queueName, true, consumer)

        app.Run()
        exitCode