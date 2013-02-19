namespace Penny.DataAccess

open System
open System.Xml
open System.Xml.Linq
open Penny.Common

module Utils = 
    
    let xName name =
        XName.Get(name)

    let xAttr name (element: XElement) = 
        element.Attribute(XName.Get(name)).Value

    let xElement name (container: XContainer) = 
        container.Element(name |> xName)

    let xElements name (container: XContainer) = 
        container.Elements(name |> xName)

    let xElementExists name (container: XContainer) = 
        not (container |> xElements name |> Seq.isEmpty)

    let xValue (element: XElement) = 
        element.Value


