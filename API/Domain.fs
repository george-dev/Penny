namespace Penny.API

open System
open Penny.DataAccess
open Penny.DataAccess.Utils
open Penny.Common.Utils

module Domain = 

    type Entry() = 
        let mutable date = DateTime.Today
        let mutable text = ""
        let mutable tags = ""

        member x.Date with get() = date and set(v) = date <- v
        member x.Text with get() = text and set(v) = text <- v
        member x.Tags with get() = tags and set(v) = tags <- v

        member x.Save() = 
            Entry.save date text tags
            Indexing.indexEntry date

        static member Load(date: DateTime) = 
            let date, entry, tags = Penny.DataAccess.Entry.load date
            Entry(Date = date, Text = entry, Tags = tags)

        static member LoadEntryDays(year, month) = 
            Penny.DataAccess.Entry.loadEntryDates year month
            |> Array.map (fun date -> date.Day)

        static member Search(query: string) = 
            match query with
            | Empty -> Array.empty
            | Value query -> query |> Indexing.search 
                                   |> Array.map Entry.load
                                   |> Array.map (fun (date, entry, tags) -> Entry(Date = date, Text = entry, Tags = tags))
            
