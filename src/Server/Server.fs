module Server

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn
open Giraffe
open FSharp.Control.Tasks

open Shared

type Storage() =
    let todos = ResizeArray<_>()
    let blotters = ResizeArray<_>()
    let markets = ResizeArray<_>()

    member __.GetTodos() = List.ofSeq todos

    member __.AddTodo(todo: Todo) =
        if Todo.isValid todo.Description then
            todos.Add todo
            Ok()
        else
            Error "Invalid todo"

    member __.AddBlotter(data : Blotter) =
        blotters.Add data
        Ok()

    member __.AddMarket(dataMarket : Market) =
        markets.Add dataMarket
        Ok()



let storage = Storage()

storage.AddTodo(Todo.create "Create new SAFE project")
|> ignore

storage.AddTodo(Todo.create "Write your app")
|> ignore

storage.AddTodo(Todo.create "Ship it !!!")
|> ignore

let todosApi =
    { getTodos = fun () -> async { return storage.GetTodos() }
      addTodo =
          fun todo ->
              async {
                  match storage.AddTodo todo with
                  | Ok () -> return todo
                  | Error e -> return failwith e
              } }

let webApp =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue todosApi
    |> Remoting.buildHttpHandler

let app =
    application {
        url "http://0.0.0.0:8085"
        use_router webApp
        memory_cache
        use_static "public"
        use_gzip
    }

run app

let loadBlottersInfo() =
    [ { Id = System.Guid();
        DateTime = System.DateTime()
        Price = 1.4528;
        Quantity = 1500;
        Pair = "GBP/USD" } ]

let loadMarketsInfo() =
    [ { Id = System.Guid();
        Provider = "CBOE";
        Pair = "GBP/USD";
        Price = 1.554;
        Time = System.DateTime() } ]


/// Loads a customer from the DB and returns as a Customer in json.
let loadBlotter (blotter:obj) next ctx = task {
    let blotterInfo = loadBlottersInfo()
    return! json blotterInfo next ctx
}

let loadMarket (market:obj) next ctx = task {
    let marketInfo = loadMarketsInfo()
    return! json marketInfo next ctx
}
// Example
let loadCustomersFromDb() =
    [ { Id = 1; Name = "Joe Bloggs" } ]

/// Returns the results of loadCustomersFromDb as JSON.
let getCustomers next ctx =
    json (loadCustomersFromDb()) next ctx

let webApi = router {
    get "/api/blotter" (loadBlotter()) // Add this
    get "/api/markets/" (loadMarket())
    get "/api/customers/" getCustomers
}
