namespace Workspaces.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open System.Data.SqlClient
open Microsoft.AspNetCore.Http
open WebServiceM2Lib.Mapping.tables
open WebServiceM2Lib.Database
open Asp.Versioning

[<ApiController>]
[<Route("api/v{version:apiVersion}/workspaces")>]
[<ApiVersion("1.0")>]
type WorkspacesController (logger : ILogger<WorkspacesController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! get = Database.getWorkspaces conn
                return Ok get
            with 
                | error -> return error |> raise
        }

    [<HttpGet("{id}")>]
    member this.Get(id: int32) =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! getById = Database.getWorkspacesById conn id
                return getById
            with 
                | error -> return error |> raise
        }

    [<HttpPost>]
    [<ProducesResponseType(StatusCodes.Status201Created)>]
    //member this.PostWorkspace(workspace: Workspaces) =
    member this.Post (memberId: int32) (workspaceName: string) =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! post = Database.postWorkspaces conn memberId workspaceName//workspace
                return post
            with 
                | error -> return error |> raise
        }

    [<HttpPut>]
    [<ProducesResponseType(StatusCodes.Status200OK)>]
    member this.UpdateWorkspace(workspace: Workspaces) =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! update = Database.updateWorkspaces conn workspace
                return update
            with 
                | error -> return error |> raise
        }

    [<HttpDelete("{id}")>]
    [<ProducesResponseType(StatusCodes.Status200OK)>]
    member this.DeleteWorkspace(id: int32) =
        task {
            use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
            let! delete = Database.deleteWorkspaces conn id
            return delete
        }
