module WorkspacesController

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open RightsManagement.tables
open MySqlConnector
open Microsoft.AspNetCore.Http
open DatabaseFunction
open Workspaces.tables

[<ApiController>]
[<Route("workspaces")>]
type WorkspacesController (logger : ILogger<WorkspacesController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() =
        task
            {   
                try
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
                    let! model = Database.getWorkspaces conn
                    return
                        model
                with 
                    | error -> return error |> raise
            }

    [<HttpGet"{id}">]
    member this.Get(id: int) =
        task
            {   
                try
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
                    let! model = Database.getWorkspacesById conn id
                    return
                        model
                with 
                    | error -> return error |> raise
            }

    [<HttpPost>]
    [<ProducesResponseType(StatusCodes.Status201Created)>]
    member this.PostWorkspace(workspace: Workspaces) =
        task
            {   
                try
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
                    let! model = Database.postWorkspaces conn workspace
                    return
                        model
                with 
                    | error -> return error |> raise
            }

    [<HttpPut>]
    [<ProducesResponseType(StatusCodes.Status200OK)>]
    member this.UpdateWorkspace(workspacename: string, groupname: string, members: Members List) =
        task
            {   
                try
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
                    let! model = Database.updateWorkspaces conn input
                    return
                        model
                with 
                    | error -> return error |> raise
            }

    [<HttpDelete"{id}">]
    [<ProducesResponseType(StatusCodes.Status200OK)>]
    member this.DeleteWorkspace(id: int) =
        task
            {
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
                let! model = Database.deleteWorkspaces conn id
                return
                    model
            }

