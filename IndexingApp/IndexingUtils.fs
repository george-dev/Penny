namespace Penny.IndexingApp

open System.IO
open Penny.API
open Penny.Common
open Penny.Common.Utils

module IndexingUtils = 

    let reIndexEverything () = 
        Cloud.clearContainer "lucene"
        Cloud.listBlobs (Config.blobContainerName)
            |> Array.map Path.GetFileNameWithoutExtension
            |> Array.collect (fun fileName -> 
                                    match tryParseDate fileName with
                                    | true, date -> [| date |]
                                    | false, _   -> Array.empty)
            |> Array.iter Indexing.indexEntry

    let moveEntriesToCloud () = 
        Cloud.clearContainer (Config.blobContainerName)
        Directory.GetFiles(Config.baseDirectory)
            |> Array.map (fun path -> Path.GetFileName(path), File.ReadAllText(path))
            |> Array.iter (fun (path, text) -> Cloud.saveBlob (Config.blobContainerName) path text)
        

