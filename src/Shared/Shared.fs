namespace Shared

open System

type Todo = { Id: Guid; Description: string }

type Blotter = { Id: Guid; DateTime: DateTime; Price: float; Quantity: int; Pair: string }
type Market = { Id: Guid; Provider: string; Pair: string; PricevsTime: Object;}

module Blotter =
    // let isValidPrice (price: float) =
    //     float.IsNullOrWhiteSpace price |> not
    let create dateTime price quantity pair =
        { Id = Guid.NewGuid()
          DateTime = dateTime
          Price = price
          Quantity = quantity
          Pair = pair }

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
