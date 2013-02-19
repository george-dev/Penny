open System
open Penny.IndexingApp

[<EntryPoint>]
let main argv = 
    printfn "re-indexing..."
    IndexingUtils.reIndexEverything()
    printfn "done. Any key to close."
    Console.ReadKey() |> ignore
    0
