namespace Penny.IndexingApp

open System.IO
open Penny.API
open Penny.Common
open Penny.Common.Utils

module IndexingUtils = 

    let reIndexEverything () = 
        let indexPath = Path.Combine(Config.baseDirectory, "Index")
        Directory.Delete(indexPath, true)
        Directory.GetFiles(Config.baseDirectory)
            |> Array.map Path.GetFileNameWithoutExtension
            |> Array.collect (fun fileName -> 
                                    match tryParseDate fileName with
                                    | true, date -> [| date |]
                                    | false, _   -> Array.empty)
            |> Array.iter Indexing.indexEntry
        

