namespace thinkhard.Protocol.Dns

open System
open System.IO

open thinkhard

/// represents a byte array for a resolver query
type requestion = {
  header     : header
  question   : question list
  answer     : rr list
  authority  : rr list
  additional : rr list } with
  member o.pickle = lazy (seq {
    yield! !!o.header.pickle
    yield! seq { for q in (o.question   |> Seq.map (fun x -> x.pickle)) do yield! !!q }
    yield! seq { for a in (o.authority  |> Seq.map (fun x -> x.pickle)) do yield! !!a }
    yield! seq { for a in (o.additional |> Seq.map (fun x -> x.pickle)) do yield! !!a } } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    let header = header.unpickle stream
    { header     = header
      question   = if header.qdcount > 0us then [ for x in 1us .. header.qdcount do yield question.unpickle stream ]
                   else []
      answer     = if header.ancount > 0us then [ for x in 1us .. header.ancount do yield rr.unpickle stream ]
                   else []
      authority  = if header.nscount > 0us then [ for x in 1us .. header.nscount do yield rr.unpickle stream ]
                   else []
      additional = if header.arcount > 0us then [ for x in 1us .. header.arcount do yield rr.unpickle stream ]
                   else [] }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream(bytes)
    requestion.unpickle ms
    
