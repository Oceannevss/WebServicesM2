﻿namespace Rights.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MySqlConnector
open Microsoft.AspNetCore.Http
open WebServiceM2Lib.Mapping.tables
open WebServiceM2Lib.Database

[<ApiController>]
[<Route("members")>]
type MembersController (logger : ILogger<MembersController>) =
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
                    let! model = Database.getMemberById conn id
                    return
                        model
                with 
                    | error -> return error |> raise
            }

    [<HttpPost>]
    [<ProducesResponseType(StatusCodes.Status201Created)>]
    member this.Post(members: Members) =
        task
            {   
                try
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
                    let! model = Database.postMember conn members
                    return
                        model
                with 
                    | error -> return error |> raise
            }

    [<HttpPut>]
    [<ProducesResponseType(StatusCodes.Status200OK)>]
    member this.Update(members: Members) =
        task
            {   
                try
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
                    let! model = Database.updateMember conn members
                    return
                        model
                with 
                    | error -> return error |> raise
            }

    [<HttpDelete"{id}">]
    [<ProducesResponseType(StatusCodes.Status200OK)>]
    member this.Delete(id: int option) =
        task
            {
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
                let! model = Database.deleteWorkspaces conn id
                return
                    model
            }