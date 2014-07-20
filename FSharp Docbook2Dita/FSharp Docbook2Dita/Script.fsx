#r "../packages/FSharp.Data.2.0.9/lib/net40/FSharp.Data.dll"
#r "System.Xml.Linq.dll"

open FSharp.Data
open System.Xml.Linq

type DocbookProvider = XmlProvider<"in_docbook.xml", Global = true>
type DitaMapProvider = XmlProvider<"out_ditamap.xml", Global = true>
type TopicProvider   = XmlProvider<"out_topic1.xml", Global = true>

type Docbook         = DocbookProvider.Book

type DitaMap         = DitaMapProvider.Map
type Topic           = TopicProvider.Topic

type Composite = {DitaMap : DitaMap; Topics : List<Topic>}

let docbook2Dita (docbook : Docbook) : Composite =
    failwith "implement me"

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