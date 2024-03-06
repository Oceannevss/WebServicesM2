namespace DatabaseFunction

open System
open Dapper
open Dapper.FSharp
open Dapper.FSharp.MySQL
open MySqlConnector
open Workspaces.tables

module Database =

    let getWorkspaces (conn: MySqlConnector.MySqlConnection) = 
        task{
            let mappingWorkspaces =
                    table'<Workspaces> "workspace" |> inSchema "workspaces"
            
            let! select =
                select {
                    for w in mappingWorkspaces do
                    selectAll
                } |> conn.SelectAsync<Workspaces>

            return select
        }

    let getWorkspacesById (conn: MySqlConnector.MySqlConnection) (id)= 
        task{
            let mappingWorkspaces =
                    table'<Workspaces> "workspace" |> inSchema "workspaces"
            
            let! select =
                select {
                    for w in mappingWorkspaces do
                    where (w.Id = id)
                } |> conn.SelectAsync<Workspaces>

            return select
        }
        

    let postWorkspaces (conn: MySqlConnector.MySqlConnection)(workspace: Workspaces) =
        task {
            let mappingWorkspaces =
                    table'<Workspaces> "workspace" |> inSchema "workspaces"
            let transformWorkspace (workspace): Workspaces =
                { Id = None
                  Name = workspace.Name
                  Creation_date = DateOnly.FromDateTime(DateTime.Now)
                  Members_nbr = 1
                  Groups_nbr = 0
                  Id_creator = None }

            let newWorkspace = transformWorkspace workspace

            let! insert =
                insert {
                    into mappingWorkspaces
                    value newWorkspace
                }
                |> conn.InsertAsync

            return insert
        }

    let updateWorkspaces (conn: MySqlConnector.MySqlConnection)(workspace: Workspaces) =
        task {
            
        }

    let deleteWorkspaces (conn: MySqlConnector.MySqlConnection)(id) = 
        task{
            let mappingWorkspaces =
                    table'<Workspaces> "workspace" |> inSchema "workspaces"
            
            let! delete =
                delete {
                    for w in mappingWorkspaces do
                    where (w.Id = id)
                } |> conn.DeleteAsync

            return delete
        }
