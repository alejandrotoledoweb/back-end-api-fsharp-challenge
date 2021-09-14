namespace Shared

open System

type Todo = { Id: Guid; Description: string }

type Blotter = { Id: Guid; TimeISO: DateTime; Price: float; Quantity: int; Pair: string }
type Market = { Id: Guid; Provider: string; Pair: string; PricevsTime: Object;}

module Todo =
    let isValid (description: string) =
        String.IsNullOrWhiteSpace description |> not

    let create (description: string) =
        { Id = Guid.NewGuid()
          Description = description }

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type ITodosApi =
    { getTodos: unit -> Async<Todo list>
      addTodo: Todo -> Async<Todo> }
