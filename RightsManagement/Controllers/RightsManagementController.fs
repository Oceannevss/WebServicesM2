namespace Rights.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MySqlConnector
open Microsoft.AspNetCore.Http
open WebServiceM2Lib.Mapping.tables
open WebServiceM2Lib.Database
open RabbitMQ.Client
open RabbitMQ.Client.Events
open System
open System.Text

[<ApiController>]
[<Route("rights")>]
type RightsManagementController (logger : ILogger<RightsManagementController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() =
        task
            {   
                try
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
                    let! model = Database.getRights conn
                    return
                        model
                with 
                    | error -> return error |> raise
            }

    [<HttpGet"{id}">]
    member this.Get(id: int option) =
        task
            {   
                try
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
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
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
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
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
                    let! model = Database.updateRight conn right
                    return
                        model
                with 
                    | error -> return error |> raise
            }

    [<HttpDelete"{id}">]
    [<ProducesResponseType(StatusCodes.Status200OK)>]
    member this.DeleteRight(rightId: int option) =
        task
            {
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
                let! model = Database.deleteWorkspaces conn rightId
                return
                    model
            }
