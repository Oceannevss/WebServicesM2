namespace RightsManagement
#nowarn "20"
open Asp.Versioning

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
open RightsManagement.Controllers

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddControllers()

        builder.Services.AddApiVersioning(fun options ->
            options.DefaultApiVersion <- ApiVersion(1, 0)
            options.AssumeDefaultVersionWhenUnspecified <- true
            options.ReportApiVersions <- true
            
        )
        
        builder.Services.AddEndpointsApiExplorer()
        builder.Services.AddSwaggerGen(fun options ->
            options.SwaggerDoc("v1.0", new Microsoft.OpenApi.Models.OpenApiInfo(Title = "Right Management Api", Version = "1.0", Description = "Cette api g�re les droit et les utilisateurs"))
        )

        let app = builder.Build()

        app.UseSwagger(fun options ->
            options.SerializeAsV2 <- true
        )
        app.UseSwaggerUI(fun options -> 
            options.DocumentTitle <- "Right Management Api"
            options.SwaggerEndpoint("/swagger/v1.0/swagger.json", "v1.0")
        )

        app.UseHttpsRedirection()

        app.UseAuthorization()

        app.MapControllers()

        Dapper.FSharp.MSSQL.OptionTypes.register()
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