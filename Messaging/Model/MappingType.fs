module MappingType

open System

type Messages = {
    Id:int
    Message: string
    Creation_date: DateTime
    Id_channel: int
    Id_members: int
}

and Channels = {
    Id: int
    Name: string
    Id_groups: int
}