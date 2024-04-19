namespace RightsManagement.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open System.Data.SqlClient
open Microsoft.AspNetCore.Http
open WebServiceM2Lib.Mapping.tables
open WebServiceM2Lib.Database
open RabbitMQ.Client
open RabbitMQ.Client.Events
open System
open System.Text
open Asp.Versioning

[<ApiController>]
[<Route("api/v{version:apiVersion}/rights")>]
[<ApiVersion("1.0")>]
type RightsManagementController (logger : ILogger<RightsManagementController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() =
        task
            {   
                try
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                    let! model = Database.getRights conn
                    return
                        model
                with 
                    | error -> return error |> raise
            }

    [<HttpGet"{id}">]
    member this.Get(id: int32) =
        task
            {   
                try
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                    let! model = Database.getRightById conn id
                    return
                        model
                with 
                    | error -> return error |> raise
            }

    [<HttpPost>]
    [<ProducesResponseType(StatusCodes.Status201Created)>]
    member this.PostRight(permission: Permissions) =
        task
            {   
                try
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                    let! model = Database.postRight conn permission
                    return
                        model
                with 
                    | error -> return error |> raise
            }

    [<HttpPut>]
    [<ProducesResponseType(StatusCodes.Status200OK)>]
    member this.UpdateRight(right: Permissions) =
        task
            {   
                try
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                    let! model = Database.updateRight conn right
                    return
                        model
                with 
                    | error -> return error |> raise
            }

    [<HttpDelete"{id}">]
    [<ProducesResponseType(StatusCodes.Status200OK)>]
    member this.DeleteRight(rightId: int32) =
        task
            {
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! model = Database.deleteWorkspaces conn rightId
                return
                    model
            }
