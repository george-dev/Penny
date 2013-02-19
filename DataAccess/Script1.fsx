#r "System.Xml"
#r "System.Xml.Linq"
#r "System.Configuration"
#load @"d:\Projects\Penny\DataAccess\Config.fs"
#load @"d:\Projects\Penny\DataAccess\Utils.fs"
#load @"d:\Projects\Penny\DataAccess\Entry.fs"

open Penny.DataAccess.Config
open Penny.DataAccess.Utils
open Penny.DataAccess.Entry
open System
open System.Configuration

ConfigurationManager.AppSettings.["BaseDirectory"] <- @"d:\temp"

Penny.DataAccess.Entry.save (DateTime.Today) "dfafd"
