namespace Penny.UI

open System.Configuration

type UIConfig() = 
    static member Title 
        with get() = ConfigurationManager.AppSettings.["Title"]

