namespace Messaging
#nowarn "20"
open Asp.Versioning

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

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

        Dapper.FSharp.MSSQL.OptionTypes.register()
        app.Run()

        exitCode
