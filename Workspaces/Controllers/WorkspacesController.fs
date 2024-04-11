namespace Workspaces.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MySqlConnector
open Microsoft.AspNetCore.Http
open WebServiceM2Lib.Mapping.tables
open WebServiceM2Lib.Database

[<ApiController>]
[<Route("api/v{version:apiVersion}/workspaces")>]
[<ApiVersion("1.0")>]
type WorkspacesController (logger : ILogger<WorkspacesController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
                let! get = Database.getWorkspaces conn
                return Ok get
            with 
                | error -> return error |> raise
        }

    [<HttpGet("{id}")>]
    member this.Get(id: int option) =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
                let! getById = Database.getWorkspacesById conn id
                return getById
            with 
                | error -> return error |> raise
        }

    [<HttpPost>]
    [<ProducesResponseType(StatusCodes.Status201Created)>]
    member this.PostWorkspace(workspace: Workspaces) =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
                let! post = Database.postWorkspaces conn workspace
                return post
            with 
                | error -> return error |> raise
        }

    [<HttpPut>]
    [<ProducesResponseType(StatusCodes.Status200OK)>]
    member this.UpdateWorkspace(workspace: Workspaces) =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
                let! update = Database.updateWorkspaces conn workspace
                return update
            with 
                | error -> return error |> raise
        }

    [<HttpDelete("{id}")>]
    [<ProducesResponseType(StatusCodes.Status200OK)>]
    member this.DeleteWorkspace(id: int option) =
        task {
            use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
            let! delete = Database.deleteWorkspaces conn id
            return delete
        }
