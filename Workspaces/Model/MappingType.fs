namespace Workspaces.tables

open System

[<CLIMutable>]
type Groupes = {
    Id: int
    Name: string
    Description: string
    Creation_date: DateOnly
    Id_workspace: int
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
    Id: int 
    Name: string
    Creation_date: DateOnly
    Members_nbr: int
    Groups_nbr: int
    Id_creator: int
}


//Faire un micro service uniquement pour les droits
//faire un micro service uniquement pour les documents
//faire un micro service uniquement pour les messages