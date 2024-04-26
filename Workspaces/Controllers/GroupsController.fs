namespace Workspaces.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open System.Data.SqlClient
open Microsoft.AspNetCore.Http
open WebServiceM2Lib.Mapping.tables
open WebServiceM2Lib.Database
open Asp.Versioning

[<ApiController>]
[<Route("api/v1.0/groups")>]
[<ApiVersion("1.0")>]
type GroupsController (logger : ILogger<GroupsController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! get = Database.getGroups conn
                return get
            with 
                | error -> return error |> raise
        }

    [<HttpGet("{id}")>]
    member this.Get(id: int32) =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! getById = Database.getChannelById conn id
                return getById
            with 
                | error -> return error |> raise
        }

    [<HttpPost>]
    [<ProducesResponseType(StatusCodes.Status201Created)>]
    member this.Post (group: Groups) =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! post = Database.postGroups conn group
                return CreatedResult("", post)
            with 
                | error -> return error |> raise
        }

    [<HttpPut>]
    [<ProducesResponseType(StatusCodes.Status200OK)>]
    member this.UpdateWorkspace(group: Groups) =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! update = Database.updateGroups conn group
                if update.Result = 1 then
                    return StatusCodeResult(204)
                else
                    return StatusCodeResult(400)
            with 
                | error -> return error |> raise
        }

    [<HttpDelete("{id}")>]
    [<ProducesResponseType(StatusCodes.Status200OK)>]
    member this.DeleteWorkspace(id: int32) =
        task {
            use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
            let! delete = Database.deleteGroups conn id
            if delete = 1 then
                    return StatusCodeResult(204)
                else
                    return StatusCodeResult(400)
        }
