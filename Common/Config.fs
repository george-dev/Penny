namespace Penny.Common

open System
open System.IO
open System.Configuration

module Config = 

    let baseDirectory = Path.GetFullPath(ConfigurationManager.AppSettings.["BaseDirectory"])

    let dateFormat = "yyyyMMdd - dddd dd MMMM yyyy"

    let storageConnectionString = ConfigurationManager.AppSettings.["StorageConnectionString"]

    let blobContainerName = "penny"

