namespace Penny.API

open Lucene.Net
open Lucene.Net.Index
open Lucene.Net.QueryParsers
open Lucene.Net.Search
open Lucene.Net.Store
open Lucene.Net.Analysis.Standard
open Lucene.Net.Util
open Lucene.Net.Documents
open System
open System.IO
open Penny.Common.Config
open Penny.Common.Utils
open Penny.DataAccess

module Indexing = 

    let private indexPath = Path.Combine(baseDirectory, "Index") |> ensureDirectory
    let private idField = "id"

    let indexEntry (date: DateTime) = 
        use dir = FSDirectory.Open(DirectoryInfo(indexPath))
        use analyzer = new StandardAnalyzer(Version.LUCENE_30)
        use writer = new IndexWriter(dir, analyzer, IndexWriter.MaxFieldLength.LIMITED)
        
        let date, entry, tags = Penny.DataAccess.Entry.load date
        let date = date |> stringifyDate
        let tags = tags.Split(',') |> Array.map (fun tag -> tag.Trim())
        writer.DeleteDocuments(Term(idField, date))
        let indexDoc = Document()
        indexDoc.Add(Field(idField, date, Field.Store.YES, Field.Index.NOT_ANALYZED))
        indexDoc.Add(Field("date", date, Field.Store.NO, Field.Index.ANALYZED))
        indexDoc.Add(Field("all", date + " " + entry + " " + String.Join(" ", tags), Field.Store.NO, Field.Index.ANALYZED))
        tags |> Array.iter (fun tag -> indexDoc.Add(Field("tag", tag, Field.Store.YES, Field.Index.ANALYZED)))

        writer.AddDocument(indexDoc)
        writer.Optimize()
        writer.Commit()

    let search query = 
        use analyzer = new StandardAnalyzer(Version.LUCENE_30)
        let parser = QueryParser(Version.LUCENE_30, "all", analyzer)
        let userQuery = parser.Parse(query)
        use dir = FSDirectory.Open(DirectoryInfo(indexPath))
        use indexSearcher = new IndexSearcher(dir, true)
        let results = indexSearcher.Search(userQuery, 50)
        let resultDocs = results.ScoreDocs 
                        |> Array.map (fun result -> 
                            let doc = indexSearcher.Doc(result.Doc)
                            let date = doc.Get(idField)
                            date |> parseDate)
        resultDocs
        


