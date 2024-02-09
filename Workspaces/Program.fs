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
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

// J'ai rajouté ce fichier
open Swashbuckle.AspNetCore
open Microsoft.OpenApi.Models // N'oubliez pas d'ouvrir ce namespace pour accéder à OpenApiInfo

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddControllers()

        // Configuration de SwaggerGen
        builder.Services.AddSwaggerGen(fun c ->
            c.SwaggerDoc("v1", OpenApiInfo(
                Title = "My API",
                Version = "v1"
            ))
        )

        let app = builder.Build()

        app.UseSwagger()

        app.UseSwaggerUI(fun options ->
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1")
            options.RoutePrefix <- "swagger"
        )

        // Configuration de SwaggerGen FIN

        //app.UseHttpsRedirection()
        app.UseAuthorization()
        app.MapControllers()

        app.Run()

        exitCode
