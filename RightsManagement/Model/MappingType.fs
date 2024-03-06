namespace RightsManagement.tables

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
