namespace Penny.DataAccess

open System
open System.Xml
open System.Xml.Linq
open System.IO
open Penny.DataAccess.Utils
open Penny.Common
open Penny.Common.Utils

module Entry = 

    let save (date: DateTime) (text: String) (tags: String) = 
        let date = date |> stringifyDate
        if text = null || text.Trim() = "" then
            Cloud.deleteBlob (Config.blobContainerName) (date + ".xml")
        else
            let text = text.Trim()
            let tags = (tags |> defaultIfNull "").Split(',')
            let xml = new XElement(xName "Entry",
                        new XElement(xName "Date", date),
                        new XElement(xName "Text", text |> defaultIfNull ""),
                        new XElement(xName "Tags",
                            tags |> Array.map (fun x -> new XElement(xName "Tag", x.Trim()))))
            Cloud.saveBlob (Config.blobContainerName) (date + ".xml") (xml.ToString())

    let load (date: DateTime) = 
        let entryName = (date |> stringifyDate) + ".xml"
        let blob = Cloud.getBlob (Config.blobContainerName) entryName
        match blob with
        | Some blob ->
            let xml = XElement.Parse(blob)
            let date = xml |> (xElement "Date") |> xValue |> parseDate
            let entry = xml |> (xElement "Text") |> xValue
            let tags = match xml |> xElementExists "Tags" with
                       | true -> xml |> xElement "Tags"
                                     |> xElements "Tag"
                                     |> Seq.map xValue
                                     |> Seq.toArray
                       | false -> Array.empty
            let tags = String.Join(", ", tags)
            (date, entry, tags)
        | None -> (DateTime.Today, String.Empty, String.Empty)

    let loadEntryDates year month = 
        let dateString = new DateTime(year, month, 1) |> stringifyDate
        let yearAndMonth = dateString.Substring(0, 6);
        let entriesForMonth = Cloud.listBlobs (Config.blobContainerName)
                              |> Array.map Path.GetFileNameWithoutExtension
                              |> Array.filter (fun fileName -> fileName.StartsWith(yearAndMonth))
                              |> Array.map parseDate
        entriesForMonth


        
            

