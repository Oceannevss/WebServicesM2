namespace RightsManagement.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open System.Data.SqlClient
open Microsoft.AspNetCore.Http
open WebServiceM2Lib.Mapping.tables
open WebServiceM2Lib.Database
open Asp.Versioning

[<ApiController>]
[<Route("api/v1.0/members")>]
[<ApiVersion("1.0")>]
type MembersController (logger : ILogger<MembersController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() =
        task
            {   
                try
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                    let! model = Database.getMembers conn
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
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                    let! model = Database.postMember conn members
                    if model = 1 then
                        return StatusCodeResult(201)
                    else
                        return StatusCodeResult(400)
                with 
                    | error -> return error |> raise
            }

    [<HttpPut>]
    [<ProducesResponseType(StatusCodes.Status204NoContent)>]
    member this.Update(members: Members) =
        task
            {   
                try
                    use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                    let! model = Database.updateMember conn members
                    if model = 1 then
                        return StatusCodeResult(204)
                    else
                        return StatusCodeResult(400)
                with 
                    | error -> return error |> raise
            }

    [<HttpDelete"{id}">]
    [<ProducesResponseType(StatusCodes.Status204NoContent)>]
    member this.Delete(id: int32) =
        task
            {
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! model = Database.deleteMember conn id
                if model = 1 then
                        return StatusCodeResult(204)
                    else
                        return StatusCodeResult(400)
            }