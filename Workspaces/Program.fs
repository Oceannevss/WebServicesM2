namespace Workspaces
#nowarn "20"
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
        //builder.Services.AddApiVersioning( 
        //    fun v -> 
        //    v.DefaultApiVersion = new ApiVersion(1.0
        //    v.AssumeDefaultVersionWhenUnspecified = true
        //    v.ReportApiVersions = true
        //).AddApiExplorer(fun ae ->
        //    ae.GroupNameFormat = "'v'VVV"
        //    ae.SubstituteApiVersionInUrl = true
        //    )
        //builder.Services
        //    .AddSwaggerGen(fun c ->
        //        c.IncludeXmlComments(
        //            IO.Path.Combine(
        //                AppContext.BaseDirectory,
        //                Reflection.Assembly.GetEntryAssembly().GetName().Name + ".xml")))
        //    .AddControllers()

        builder.Services.AddEndpointsApiExplorer()
        builder.Services.AddSwaggerGen()

        let app = builder.Build()

        app.UseSwagger()
        app.UseSwaggerUI()

        app.UseHttpsRedirection()

        app.UseAuthorization()

        app.MapControllers()

        Dapper.FSharp.MySQL.OptionTypes.register()
        app.Run()
        exitCode
