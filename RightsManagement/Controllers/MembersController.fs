namespace Rights.Controllers

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

    //[<HttpGet"{id}">]
    //member this.Get(id: int option) =
    //    task
    //        {   
    //            try
    //                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
    //                let! model = Database.getRightById conn id
    //                return
    //                    model
    //            with 
    //                | error -> return error |> raise
    //        }

    //[<HttpPost>]
    //[<ProducesResponseType(StatusCodes.Status201Created)>]
    //member this.PostRight(permission: Permissions) =
    //    task
    //        {   
    //            try
    //                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
    //                let! model = Database.postRight conn permission
    //                return
    //                    model
    //            with 
    //                | error -> return error |> raise
    //        }

    //[<HttpPut>]
    //[<ProducesResponseType(StatusCodes.Status200OK)>]
    //member this.UpdateRightMember(memberId: int ,rightId: int) =
    //    task
    //        {   
    //            try
    //                use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
    //                let! model = Database.updateRight conn workspace
    //                return
    //                    model
    //            with 
    //                | error -> return error |> raise
    //        }

    //[<HttpDelete"{id}">]
    //[<ProducesResponseType(StatusCodes.Status200OK)>]
    //member this.DeleteRightMember(MemberId: int option) =
    //    task
    //        {
    //            use conn = System.Environment.GetEnvironmentVariable("MyDb") |> MySqlConnection
    //            let! model = Database.deleteWorkspaces conn id
    //            return
    //                model
    //        }