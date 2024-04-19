namespace WebServiceM2Lib.Mapping.tables

open System

[<CLIMutable>]
type Groups = {
    Id: int32 option
    Name: string
    Description: string
    Creation_date: DateOnly
    Id_workspace: int32
}

[<CLIMutable>]
type Channels = {
    Id: int32 option
    Name: string
    Id_groups: int32
}

[<CLIMutable>]
type Messages = {
    Id: int32 option
    Message: string
    Creation_date: DateTime
    Id_channels: int32
    Id_members: int32
}

[<CLIMutable>]
type Documents = {
    Id: int32 option 
    Name: string
    Type: string
    Creation_date: DateTime
    Id_members: int32
}

[<CLIMutable>]
type Workspaces = {
    Id: int32 option
    Name: string
    Creation_date: DateTime
    Members_nbr: int32
    Groups_nbr: int32
    Id_creator: int32
}

[<CLIMutable>]
type WorkspaceGroups = {
    Id_workspace: int32 
    Id_group: int32
}

[<CLIMutable>]
type Members = {

    Id: int32 option
    Firstname: string
    Lastname: string
    Mail: string
    Id_permission: int32
}

[<CLIMutable>]
type Permissions = {

    Id: int32 option
    Permission: string 
}


//Faire un micro service uniquement pour les droits
//faire un micro service uniquement pour les documents
//faire un micro service uniquement pour les messages