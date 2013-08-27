namespace Penny.UI

open System
open System.Web
open System.Web.Mvc
open Penny.API.Domain

[<HandleError>]
[<Authorize>]
type HomeController() =
    inherit Controller()

    member x.Index () =
        x.View() :> ActionResult

    member x.SaveEntry (entry: Entry) = 
        entry.Save()
        new JsonResult(Data = "success")

    member x.LoadEntry date = 
        let entry = Entry.Load(date)
        new JsonResult(Data = entry)

    member x.Search query = 
        let results = Entry.Search(query)
        base.PartialView("SearchResult", results)

    member x.LoadEntriesForMonth(year, month) = 
        let results = Entry.LoadEntryDays(year, month)
        new JsonResult(Data = results)