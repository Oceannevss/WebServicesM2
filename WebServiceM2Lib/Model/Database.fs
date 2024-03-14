namespace DatabaseFunction

open System
open Dapper
open Dapper.FSharp
open Dapper.FSharp.MySQL
open MySqlConnector
open tables

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

    let updateWorkspaces (conn: MySqlConnector.MySqlConnection)(input: Workspaces) =
        task {
            let mappingWorkspaces =
                    table'<Workspaces> "workspace" |> inSchema "workspaces"
            let update = 
                update {
                    for w in mappingWorkspaces do
                        setColumn w.Name input.Name
                        where (w.Id = input.Id )
                } |> conn.UpdateAsync
                  |> Async.AwaitTask
                  |> Async.RunSynchronously

            return update
        }

    let updateWorspacesGroup (conn: MySqlConnector.MySqlConnection)(newGroupNbr: int)(workspaceId: int option) =
        task {
            let mappingWorkspaces =
                    table'<Workspaces> "workspace" |> inSchema "workspaces"
            let update = 
                update {
                    for w in mappingWorkspaces do
                        setColumn w.Groups_nbr newGroupNbr
                        where (w.Id = workspaceId )
                } |> conn.UpdateAsync
                  |> Async.AwaitTask
                  |> Async.RunSynchronously

            return update
        }

    let deleteWorkspaces (conn: MySqlConnector.MySqlConnection)(id: int option) = 
        task{
            let mappingWorkspaces =
                    table'<Workspaces> "workspace" |> inSchema "workspaces"
            //let mappingGroups =
            //        table'<Groups> "groups" |> inSchema "workspaces"
            let mappingWG = 
                    table'<WorkspaceGroups> "workspace_groups"|> inSchema "workspaces"

            //let parameters = new DynamicParameters()
            //parameters.Add("@id_workspace", id)
            //let selectSql = "SELECT * FROM workspace_group WHERE id_workspace = @id_workspace ; "
            //let groups = conn.Query<WorkspaceGroups>(selectSql, id).AsList()
            
            let! deleteGroupsFromWorkspace, deleteWorkspace =
                task{
                    let! deleteGroupsFromWorkspace =
                        delete {
                            for wg in mappingWG do
                            where (wg.Id_workspace = id )
                        } |> conn.DeleteAsync
            
                    let! deleteWorkspace =
                        delete {
                            for w in mappingWorkspaces do
                            where (w.Id = id)
                        } |> conn.DeleteAsync

                    return deleteGroupsFromWorkspace, deleteWorkspace
                }

            return

                match (deleteGroupsFromWorkspace, deleteWorkspace) with
                | (0, 0)
                | (0, _)
                | (_, 0) -> 0
                | _ -> 1
        }
