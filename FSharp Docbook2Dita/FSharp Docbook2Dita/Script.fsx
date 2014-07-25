#r "../packages/FSharp.Data.2.0.9/lib/net40/FSharp.Data.dll"
#r "System.Xml.Linq.dll"

open FSharp.Data
open System.Xml.Linq

type DocbookProvider = XmlProvider<"in_docbook.xml", Global = true>
type DitaMapProvider = XmlProvider<"out_ditamap.xml", Global = true>
type TopicProvider   = XmlProvider<"out_topic1.xml", Global = true>

type Docbook         = DocbookProvider.Book
type Chapter         = DocbookProvider.Chapter

type DitaMap         = DitaMapProvider.Map
type Topic           = TopicProvider.Topic

type TopicRef        = DitaMapProvider.Topicref

type Composite = {DitaMap : DitaMap; Topics : List<Topic>}

let topicRefFactory = fun i -> TopicRef(sprintf "out_topic%d.xml" (i+1))  

let chapterToTopic (chapter : Chapter) = 
    Topic(chapter.Id, chapter.Title, TopicProvider.Body chapter.Paras)

let docbook2Dita (docbook : Docbook) : Composite =
    { 
    DitaMap = DitaMap(docbook.Title, 
                        topicRefFactory |> (docbook.Chapters.Length |> Array.init) ) 
    Topics = (docbook.Chapters |> (chapterToTopic |> Array.map)) |> Array.toList }

let actual = DocbookProvider.Load "in_docbook.xml" |> docbook2Dita

let expected =
    { DitaMap = DitaMapProvider.Load("out_ditamap.xml")
      Topics  = [TopicProvider.Load("out_topic1.xml")
                 TopicProvider.Load("out_topic2.xml")]}

let compare expected actual =
    expected.DitaMap.ToString() = actual.DitaMap.ToString() &&
        expected.Topics.Length  = actual.Topics.Length      &&
        (expected.Topics, actual.Topics)
        ||> Seq.forall2 (fun e a -> e.ToString() = a.ToString())

let result = compare expected actual