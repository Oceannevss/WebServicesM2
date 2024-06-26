﻿namespace Messaging.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open System.Data.SqlClient
open Microsoft.AspNetCore.Http
open WebServiceM2Lib.Mapping.tables
open WebServiceM2Lib.Database
open Asp.Versioning

[<ApiController>]
[<Route("api/v1.0/messages")>]
[<ApiVersion("1.0")>]
type MessagesController (logger : ILogger<MessagesController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! get = Database.getMessages conn
                return get
            with 
                | error -> return error |> raise
        }

    [<HttpGet("{id}")>]
    member this.Get(id: int32) =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! getById = Database.getMessageById conn id
                return getById
            with 
                | error -> return error |> raise
        }

    [<HttpPost>]
    [<ProducesResponseType(StatusCodes.Status201Created)>]
    member this.Post(message: string) (channelId: int32) (memberId: int32)=
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! post = Database.postMessage conn message channelId memberId
                if post = 1 then
                    return StatusCodeResult(201)
                else
                    return StatusCodeResult(400)
            with 
                | error -> return error |> raise
        }

    [<HttpPut>]
    [<ProducesResponseType(StatusCodes.Status200OK)>]
    member this.Update(message: string) (messageId: int32) =
        task {
            try
                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> SqlConnection
                let! update = Database.updateMessage conn message messageId
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
            let! delete = Database.deleteMessage conn id
            if delete = 1 then
                    return StatusCodeResult(204)
                else
                    return StatusCodeResult(400)
        }