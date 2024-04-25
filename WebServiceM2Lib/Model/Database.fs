namespace WebServiceM2Lib.Database

open System
open Dapper
open Dapper.FSharp.MSSQL
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
                    where (w.Id = id)
                } |> conn.SelectAsync<Workspaces>

            return select
        }
        
    let postWorkspaces (conn: SqlConnection) (memberId: int32) (name: string) =
        task {
            let mappingWorkspaces =
                    table'<Workspaces> "workspace" |> inSchema "workspaces"

            let transformWorkspace (mId: int32) (n: string) : Workspaces =
                { Id = 0
                  Name = n
                  Creation_date = DateTime.UtcNow.Date
                  Members_nbr = 1
                  Groups_nbr = 0
                  Id_creator = mId }

            let newWorkspace = transformWorkspace memberId name

            let! insert =
                insert {
                    for w in mappingWorkspaces do
                    value newWorkspace
                    excludeColumn w.Id
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

            return update
        }

    let updateWorspacesGroup (conn: SqlConnection)(newGroupNbr: int32)(workspaceId: int32) =
        task {
            let mappingWorkspaces =
                    table'<Workspaces> "workspace" |> inSchema "workspaces"
            let update = 
                update {
                    for w in mappingWorkspaces do
                        setColumn w.Groups_nbr newGroupNbr
                        where (w.Id = workspaceId )
                } |> conn.UpdateAsync

            return update
        }

    let deleteWorkspaces (conn: SqlConnection)(id: int32) = 
        task{
            let mappingWorkspaces =
                    table'<Workspaces> "workspace" |> inSchema "workspaces"
            let mappingGroups =
                    table'<Groups> "groups" |> inSchema "workspaces"

            //let parameters = new DynamicParameters()
            //parameters.Add("@id_workspace", id)
            //let selectSql = "SELECT * FROM workspace_group WHERE id_workspace = @id_workspace ; "
            //let groups = conn.Query<WorkspaceGroups>(selectSql, id).AsList()
            
            let! deleteGroupsFromworkspace, deleteWorkspace =
                task{
                    let! deleteGroupsFromworkspace =
                        delete {
                            for g in mappingGroups do
                            where (g.Id_workspace = id )
                        } |> conn.DeleteAsync
            
                    let! deleteWorkspace =
                        delete {
                            for w in mappingWorkspaces do
                            where (w.Id = id)
                        } |> conn.DeleteAsync

                    return deleteGroupsFromworkspace, deleteWorkspace
                }

            return

                match (deleteGroupsFromworkspace, deleteWorkspace) with
                | (0, 0)
                | (0, _)
                | (_, 0)
                | _ -> 1

        }

    let getGroups (conn: SqlConnection) = 
        task{
            let mappingGroups =
                    table'<Groups> "groups" |> inSchema "workspaces"
            
            let! select =
                select {
                    for w in mappingGroups do
                    selectAll
                } |> conn.SelectAsync<Groups>

            return select
        }

    let getGroupById (conn: SqlConnection) (id)= 
        task{
            let mappingGroups =
                    table'<Groups> "groups" |> inSchema "workspaces"
            
            let! select =
                select {
                    for g in mappingGroups do
                    where (g.Id = id)
                } |> conn.SelectAsync<Groups>

            return select
        }
        
    let postGroups (conn: SqlConnection) (new_group: Groups) =
        task {
            let mappingGroups =
                    table'<Groups> "groups" |> inSchema "workspaces"

            let transformGroup (g: Groups) =
                { Id = 0
                  Name = g.Name
                  Description = g.Description
                  Creation_date = DateTime.UtcNow.Date
                  Id_workspace = g.Id_workspace
                  Groupscol = ""}

            let newGroup = transformGroup new_group

            
            let! insert =
                insert {
                    for g in mappingGroups do
                    value newGroup
                    excludeColumn g.Id
                }
                |> conn.InsertAsync

            return insert
        }

    let updateGroups (conn: SqlConnection)(input: Groups) =
        task {
            let mappingGroups =
                    table'<Groups> "groups" |> inSchema "workspaces"
            let update = 
                update {
                    for g in mappingGroups do
                        setColumn g.Name input.Name
                        setColumn g.Description input.Description
                        where (g.Id = input.Id )
                } |> conn.UpdateAsync

            return update
        }

    let deleteGroups (conn: SqlConnection)(id: int32) = 
        task{
            let mappingGroups =
                    table'<Groups> "groups" |> inSchema "workspaces"
           
            let! delete =
                delete {
                    for g in mappingGroups do
                    where (g.Id = id)
                } |> conn.DeleteAsync

            return delete

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
                    where (p.Id = id)
                } |> conn.SelectAsync<Permissions>

            return select
        }

    let postRight (conn: SqlConnection)(permission: Permissions) =
        task {
            let mappingPermission =
                    table'<Permissions> "permission" |> inSchema "workspaces"
            let transformPermission (p: Permissions) =
                { Id = 0
                  Permission = p.Permission }

            let newPermission = transformPermission permission

            let! insert =
                insert {
                    for p in mappingPermission do
                    value newPermission
                    excludeColumn p.Id
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
                        setColumn m.Id_permission None.Value
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
                    where (m.Id = id)
                } |> conn.SelectAsync<Members>

            return select
        }

    let postMember (conn: SqlConnection)(members: Members) =
        task {
            let mappingMembers =
                    table'<Members> "members" |> inSchema "workspaces"
            let transformMembers (m: Members) =
                { Id = 0
                  Firstname = m.Firstname
                  Lastname = m.Lastname
                  Mail = m.Mail
                  Id_permission = m.Id_permission }

            let newMember = transformMembers members

            let! insert =
                insert {
                    for m in mappingMembers do
                    value newMember
                    excludeColumn m.Id
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
                    where (r.Id = id)
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
                    where (m.Id = id)
                } |> conn.SelectAsync<Messages>

            return select
        }

    let postMessage (conn: SqlConnection)(message: string) (channelid: int32) (memberId: int32) =
        task {
            let mappingMessages =
                    table'<Messages> "messages" |> inSchema "workspaces"
            let transformMessages (m: string) (cId: int32) (mId: int32) =
                { Id = 0
                  Message = m
                  Creation_date = DateTime.UtcNow
                  Id_channels = cId
                  Id_members = mId
                }

            let newMessage = transformMessages message channelid memberId

            let! insert =
                insert {
                    for m in mappingMessages do
                    value newMessage
                    excludeColumn m.Id
                }
                |> conn.InsertAsync

            return insert
        }

    let updateMessage (conn: SqlConnection)(message: string) (messageId: int32) = 
        task {
            let mappingMessages =
                    table'<Messages> "messages" |> inSchema "workspaces"
            let! update =
                update {
                    for m in mappingMessages do
                    setColumn m.Message message
                    setColumn m.Creation_date DateTime.UtcNow
                    where (m.Id = messageId)
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
                    where (m.Id = id)
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
                    where (c.Id = id)
                } |> conn.SelectAsync<Channels>

            return select
        }

    let postChannel (conn: SqlConnection)(channelName: string) (groupId: int32) =
        task {
            let mappingChannels =
                    table'<Channels> "channels" |> inSchema "workspaces"
            let transformChannel (c: string) (gId: int32)=
                { Id = 0
                  Name = c
                  Id_groups = gId
                }

            let newChannel = transformChannel channelName groupId

            let! insert =
                insert {
                    for c in mappingChannels do
                    value newChannel
                    excludeColumn c.Id
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
            let mappingMessages =
                    table'<Messages> "messages" |> inSchema "workspaces"

            let! deleteMessageFromChannel, deleteChannel =
                task{
                    let! deleteMessageFromChannel =
                        delete {
                            for m in mappingMessages do
                            where (m.Id_channels = id )
                        } |> conn.DeleteAsync
            
                    let! deleteChannel = 
                        delete {
                            for c in mappingChannels do
                            where (c.Id = id)
                        }|> conn.DeleteAsync

                    return deleteMessageFromChannel, deleteChannel
                }

            return

                match (deleteMessageFromChannel, deleteChannel) with
                | (0, 0)
                | (0, _)
                | (_, 0)
                | _ -> 1

        }

