namespace Shared

open System

type Todo = { Id: Guid; Description: string }

type Blotter = { Id: Guid; DateTime: DateTime; Price: float; Quantity: int; Pair: string }
type Market = { Id: Guid; Provider: string; Pair: string; Price: float; Time: DateTime}

type Customer = { Id : int; Name : string }

module Blotter =
    // let isValidPrice (price: float) =
    //     float.IsNullOrWhiteSpace price |> not
    let create dateTime price quantity pair =
        { Id = Guid.NewGuid()
          DateTime = dateTime
          Price = price
          Quantity = quantity
          Pair = pair }

module Market =
    let create provider pair price time =
        { Id = Guid.NewGuid()
          Provider = provider
          Pair = pair
          Price = price
          Time = time }

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
