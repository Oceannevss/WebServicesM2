namespace WebServiceM2Lib.Database

open System
open Dapper
open Dapper.FSharp
open Dapper.FSharp.MySQL
open MySqlConnector
open WebServiceM2Lib.Mapping.tables

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
            let transformWorkspace (w: Workspaces) =
                { Id = None
                  Name = w.Name
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

    let getRights (conn: MySqlConnector.MySqlConnection) = 
        task{
            let mappingRights =
                    table'<Permissions> "permission" |> inSchema "workspaces"
            
            let! select =
                select {
                    for p in mappingRights do
                    selectAll
                } |> conn.SelectAsync<Permissions>

            return select
        }

    let getRightById (conn: MySqlConnector.MySqlConnection) (id)= 
        task{
            let mappingRights =
                    table'<Permissions> "permission" |> inSchema "workspaces"
            
            let! select =
                select {
                    for p in mappingRights do
                    where (p.Id = id)
                } |> conn.SelectAsync<Permissions>

            return select
        }

    let postRight (conn: MySqlConnector.MySqlConnection)(permission: Permissions) =
        task {
            let mappingPermission =
                    table'<Permissions> "permission" |> inSchema "workspaces"
            let transformPermission (p: Permissions) =
                { Id = None
                  Permission = p.Permission }

            let newPermission = transformPermission permission

            let! insert =
                insert {
                    into mappingPermission
                    value newPermission
                }
                |> conn.InsertAsync

            return insert
        }

    let updateRight (conn: MySqlConnector.MySqlConnection)(right: Permissions) = 
        task {
            let mappingRights =
                    table'<Permissions> "permission" |> inSchema "workspaces"
            let! update =
                update {
                    for p in mappingRights do
                    setColumn p.Permission right.Permission
                    where (p.Id = right.Id)
                }|> conn.UpdateAsync

            return update
        }

    let deleteRight (conn: MySqlConnector.MySqlConnection)(id: int option) = 
        task{
            let mappingRights =
                    table'<Permissions> "permission" |> inSchema "workspaces"
            let mappingMembers =
                    table'<Members> "members"|> inSchema "workspaces"

            let! update =  
                    update {
                        for m in mappingMembers do
                        setColumn m.Id_permission None
                        where (m.Id_permission = id )
                    }|> conn.UpdateAsync

            let! delete = 
                delete {
                    for r in mappingRights do
                    where (r.Id = id)
                }|> conn.DeleteAsync

            return
                match (update, delete) with
                | (0, 0)
                | (0, _)
                | (_, 0) -> 0
                | _ -> 1
        }

