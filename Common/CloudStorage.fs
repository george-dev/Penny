namespace Penny.Common

open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Auth
open Microsoft.WindowsAzure.Storage.Blob
open System.IO
open System.Text

module Cloud = 

    let saveBlob containerName blobName (blob:string) = 
        let storageAccount = CloudStorageAccount.Parse(Config.storageConnectionString)
        let blobClient = storageAccount.CreateCloudBlobClient()
        let container = blobClient.GetContainerReference(containerName)
        container.CreateIfNotExists() |> ignore
        let blockBlob = container.GetBlockBlobReference(blobName)
        use stream = new MemoryStream(Encoding.UTF8.GetBytes(blob))
        blockBlob.UploadFromStream(stream)

    let getBlob containerName blobName = 
        let storageAccount = CloudStorageAccount.Parse(Config.storageConnectionString)
        let blobClient = storageAccount.CreateCloudBlobClient()
        let container = blobClient.GetContainerReference(containerName)
        let blockBlob = container.GetBlockBlobReference(blobName)
        use memoryStream = new MemoryStream()
        blockBlob.DownloadToStream(memoryStream)
        Encoding.UTF8.GetString(memoryStream.ToArray())    

    let blobExists containerName blobName = 
        let storageAccount = CloudStorageAccount.Parse(Config.storageConnectionString)
        let blobClient = storageAccount.CreateCloudBlobClient()
        let container = blobClient.GetContainerReference(containerName)
        let blockBlob = container.GetBlockBlobReference(blobName)
        blockBlob.Exists()

    let listBlobs containerName = 
        let storageAccount = CloudStorageAccount.Parse(Config.storageConnectionString)
        let blobClient = storageAccount.CreateCloudBlobClient()
        let container = blobClient.GetContainerReference(containerName)
        match container.Exists() with
        | true -> container.ListBlobs() |> Seq.map (fun blob -> blob.Uri.ToString()) |> Seq.toArray
        | false -> Array.empty

    let getStorageAccount() = 
        CloudStorageAccount.Parse(Config.storageConnectionString)