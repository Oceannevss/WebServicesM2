namespace WebServiceM2Lib.Database

open System
open Dapper
open Dapper.FSharp
open Dapper.FSharp.MySQL
open System.Data.SqlClient
open WebServiceM2Lib.Mapping.tables

module Database =

    let getWorkspaces (conn: SqlConnection) = 
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

    let getWorkspacesById (conn: SqlConnection) (id)= 
        task{
            let mappingWorkspaces =
                    table'<Workspaces> "workspace" |> inSchema "workspaces"
            
            let! select =
                select {
                    for w in mappingWorkspaces do
                    where (w.Id = Some id)
                } |> conn.SelectAsync<Workspaces>

            return select
        }
        

    //let postWorkspaces (conn: SqlConnection)(workspace: Workspaces) =
    let postWorkspaces (conn: SqlConnection) (memberId: int32) (name: string) =
        task {
            let mappingWorkspaces =
                    table'<Workspaces> "workspace" |> inSchema "workspaces"

            let transformWorkspace (mId: int32) (name: string) : Workspaces =
                { Id = None
                  Name = name
                  Creation_date = DateTime.UtcNow.Date
                  Members_nbr = 1
                  Groups_nbr = 0
                  Id_creator = mId }

            let newWorkspace = transformWorkspace memberId name

            let! insert =
                insert {
                    into mappingWorkspaces
                    value newWorkspace
                }
                |> conn.InsertAsync

            return insert
        }

    let updateWorkspaces (conn: SqlConnection)(input: Workspaces) =
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

    let updateWorspacesGroup (conn: SqlConnection)(newGroupNbr: int)(workspaceId: int option) =
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

    let deleteWorkspaces (conn: SqlConnection)(id: int32) = 
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
                            where (w.Id = Some id)
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

    let getRights (conn: SqlConnection) = 
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

    let getRightById (conn: SqlConnection) (id)= 
        task{
            let mappingRights =
                    table'<Permissions> "permission" |> inSchema "workspaces"
            
            let! select =
                select {
                    for p in mappingRights do
                    where (p.Id = Some id)
                } |> conn.SelectAsync<Permissions>

            return select
        }

    let postRight (conn: SqlConnection)(permission: Permissions) =
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

    let updateRight (conn: SqlConnection)(right: Permissions) = 
        task {
            let mappingRight =
                    table'<Permissions> "permission" |> inSchema "workspaces"
            let! update =
                update {
                    for p in mappingRight do
                    setColumn p.Permission right.Permission
                    where (p.Id = right.Id)
                }|> conn.UpdateAsync

            return update
        }

    let deleteRight (conn: SqlConnection)(id: int32) = 
        task{
            let mappingRights =
                    table'<Permissions> "permission" |> inSchema "workspaces"
            let mappingMembers =
                    table'<Members> "members"|> inSchema "workspaces"

            let! update =  
                    update {
                        for m in mappingMembers do
                        setColumn m.Id_permission 0
                        where (m.Id_permission = id )
                    }|> conn.UpdateAsync

            let! delete = 
                delete {
                    for r in mappingRights do
                    where (r.Id = Some id)
                }|> conn.DeleteAsync

            return
                match (update, delete) with
                | (0, 0)
                | (0, _)
                | (_, 0) -> 0
                | _ -> 1
        }

    let getMembers (conn: SqlConnection) = 
        task{
            let mappingMembers =
                    table'<Members> "members" |> inSchema "workspaces"
            
            let! select =
                select {
                    for p in mappingMembers do
                    selectAll
                } |> conn.SelectAsync<Members>

            return select
        }

    let getMemberById (conn: SqlConnection) (id)= 
        task{
            let mappingMembers =
                    table'<Members> "members" |> inSchema "workspaces"
            
            let! select =
                select {
                    for m in mappingMembers do
                    where (m.Id = Some id)
                } |> conn.SelectAsync<Members>

            return select
        }

    let postMember (conn: SqlConnection)(members: Members) (permissionId: int32) =
        task {
            let mappingMembers =
                    table'<Members> "members" |> inSchema "workspaces"
            let transformMembers (m: Members) (pId: int32) =
                { Id = None
                  Firstname = m.Firstname
                  Lastname = m.Lastname
                  Mail = m.Mail
                  Id_permission = pId }

            let newMember = transformMembers members permissionId

            let! insert =
                insert {
                    into mappingMembers
                    value newMember
                }
                |> conn.InsertAsync

            return insert
        }

    let updateMember (conn: SqlConnection)(members: Members) = 
        task {
            let mappingMembers =
                    table'<Members> "members" |> inSchema "workspaces"
            let! update =
                update {
                    for m in mappingMembers do
                    setColumn m.Id_permission members.Id_permission
                    setColumn m.Lastname members.Lastname
                    setColumn m.Firstname members.Firstname
                    setColumn m.Mail members.Mail
                    where (m.Id = members.Id)
                }|> conn.UpdateAsync

            return update
        }

    let deleteMember (conn: SqlConnection)(id: int32) = 
        task{
            let mappingMembers =
                    table'<Members> "members"|> inSchema "workspaces"

            let! delete = 
                delete {
                    for r in mappingMembers do
                    where (r.Id = Some id)
                }|> conn.DeleteAsync

            return delete
        }

    let getMessages (conn: SqlConnection) = 
        task{
            let mappingMessages =
                    table'<Messages> "messages" |> inSchema "workspaces"
            
            let! select =
                select {
                    for p in mappingMessages do
                    selectAll
                } |> conn.SelectAsync<Messages>

            return select
        }

    let getMessageById (conn: SqlConnection) (id)= 
        task{
            let mappingMessages =
                    table'<Messages> "messages" |> inSchema "workspaces"
            
            let! select =
                select {
                    for m in mappingMessages do
                    where (m.Id = Some id)
                } |> conn.SelectAsync<Messages>

            return select
        }

    let postMessage (conn: SqlConnection)(message: Messages) (channelid: int32) (memberId: int32) =
        task {
            let mappingMessages =
                    table'<Messages> "messages" |> inSchema "workspaces"
            let transformMessages (m: Messages) (cId: int32) (mId: int32) =
                { Id = None
                  Message = m.Message
                  Creation_date = DateTime.UtcNow
                  Id_channels = cId
                  Id_members = mId
                }


            let newMessage = transformMessages message channelid memberId

            let! insert =
                insert {
                    into mappingMessages
                    value newMessage
                }
                |> conn.InsertAsync

            return insert
        }

    let updateMessage (conn: SqlConnection)(message: Messages) = 
        task {
            let mappingMessages =
                    table'<Messages> "messages" |> inSchema "workspaces"
            let! update =
                update {
                    for m in mappingMessages do
                    setColumn m.Message message.Message
                    setColumn m.Creation_date DateTime.UtcNow
                    where (m.Id = message.Id)
                }|> conn.UpdateAsync

            return update
        }

    let deleteMessage (conn: SqlConnection)(id: int32) = 
        task{
            let mappingMessages =
                    table'<Messages> "messages" |> inSchema "workspaces"

            let! delete = 
                delete {
                    for m in mappingMessages do
                    where (m.Id = Some id)
                }|> conn.DeleteAsync

            return delete
        }

    let getChannel (conn: SqlConnection) = 
        task{
            let mappingChannels =
                    table'<Channels> "channels" |> inSchema "workspaces"
            
            let! select =
                select {
                    for c in mappingChannels do
                    selectAll
                } |> conn.SelectAsync<Channels>

            return select
        }

    let getChannelById (conn: SqlConnection) (id)= 
        task{
            let mappingChannels =
                    table'<Channels> "channels" |> inSchema "workspaces"
            
            let! select =
                select {
                    for c in mappingChannels do
                    where (c.Id = Some id)
                } |> conn.SelectAsync<Channels>

            return select
        }

    let postChannel (conn: SqlConnection)(channel: Channels) (groupId: int32) =
        task {
            let mappingChannels =
                    table'<Channels> "channels" |> inSchema "workspaces"
            let transformChannel (c: Channels) (gId: int32)=
                { Id = None
                  Name = c.Name
                  Id_groups = gId
                }

            let newChannel = transformChannel channel groupId

            let! insert =
                insert {
                    into mappingChannels
                    value newChannel
                }
                |> conn.InsertAsync

            return insert
        }

    let updateChannel (conn: SqlConnection)(channel: Channels) = 
        task {
            let mappingChannels =
                    table'<Channels> "channels" |> inSchema "workspaces"

            let! update =
                update {
                    for c in mappingChannels do
                    setColumn c.Name channel.Name
                    where (c.Id = channel.Id)
                }|> conn.UpdateAsync

            return update
        }

    let deleteChannel (conn: SqlConnection)(id: int32) = 
        task{
            let mappingChannels =
                    table'<Channels> "channels" |> inSchema "workspaces"

            let! delete = 
                delete {
                    for c in mappingChannels do
                    where (c.Id = Some id)
                }|> conn.DeleteAsync

            return delete
        }

