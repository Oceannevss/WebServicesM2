namespace Mapping.tables

open System

[<CLIMutable>]
type Groups = {
    Id: int option
    Name: string
    Description: string
    Creation_date: DateOnly
    Id_workspace: int option
}

[<CLIMutable>]
type Documents = {
    Id: int
    Name: string
    Type: string
    Creation_date: DateOnly
    Id_members: int
}

[<CLIMutable>]
type Workspaces = {
    Id: int option
    Name: string
    Creation_date: DateOnly
    Members_nbr: int
    Groups_nbr: int
    Id_creator: int option
}

[<CLIMutable>]
type WorkspaceGroups = {
    Id_workspace: int option
    Id_group: int option
}

[<CLIMutable>]
type Members = {

    Id: int
    Fistname: string
    Lastname: string
    Mail: string
    Id_permission: int
}

[<CLIMutable>]
type Permissions = {

    Id:int
    Permission: string 
}


//Faire un micro service uniquement pour les droits
//faire un micro service uniquement pour les documents
//faire un micro service uniquement pour les messages