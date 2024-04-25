namespace Messaging.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open System.Data.SqlClient
open Microsoft.AspNetCore.Http
open WebServiceM2Lib.Mapping.tables
open WebServiceM2Lib.Database
open Asp.Versioning

[<ApiController>]
[<Route("api/v1.0/channels")>]
[<ApiVersion("1.0")>]
type ChannelsController (logger : ILogger<ChannelsController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! get = Database.getChannel conn
                return get
            with 
                | error -> return error |> raise
        }

    [<HttpGet("{id}")>]
    member this.GetById(id: int32) =
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
    member this.Post(channelName: string) (groupId: int32)=
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! post = Database.postChannel conn channelName groupId
                if post = 1 then
                    return StatusCodeResult(201)
                else
                    return StatusCodeResult(400)
            with 
                | error -> return error |> raise
        }

    [<HttpPut>]
    [<ProducesResponseType(StatusCodes.Status200OK)>]
    member this.Update(channel: Channels) =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! update = Database.updateChannel conn channel
                if update = 1 then
                    return StatusCodeResult(204)
                else
                    return StatusCodeResult(400)
            with 
                | error -> return error |> raise
        }

    [<HttpDelete("{id}")>]
    [<ProducesResponseType(StatusCodes.Status200OK)>]
    member this.Delete(id: int32) =
        task {
            use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
            let! delete = Database.deleteChannel conn id
            if delete = 1 then
                    return StatusCodeResult(204)
                else
                    return StatusCodeResult(400)
        }