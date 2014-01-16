namespace thinkhard.Protocol.Dns

open System
open System.IO

open thinkhard
open thinkhard.Stream

(* 3.2.1. Format

All RRs have the same top level format shown below:

                                    1  1  1  1  1  1
      0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                                               |
    /                                               /
    /                      NAME                     /
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                      TYPE                     |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                     CLASS                     |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                      TTL                      |
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                   RDLENGTH                    |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--|
    /                     RDATA                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    
where: *)
type rr = {
/// NAME: an owner name, i.e., the name of the node to which this resource record pertains.
  name     : domain_name
/// TYPE: two octets containing one of the RR TYPE codes.
  rrtype   : RRType
/// CLASS: two octets containing one of the RR CLASS codes.
  rrclass  : RRClass
/// TTL: a 32 bit signed integer that specifies the time interval that the resource record may be cached before the source of the information should again be consulted.  Zero values are interpreted to mean that the RR can only be used for the transaction in progress, and should not be cached.  For example, SOA records are always distributed with a zero TTL to prohibit caching.  Zero values can also be used for extremely volatile data.
  ttl      : uint32
/// RDLENGTH: an unsigned 16 bit integer that specifies the length in octets of the RDATA field.
  rdlength : uint16
/// RDATA: a variable length string of octets that describes the resource.  The format of this information varies according to the TYPE and CLASS of the resource record.
  rdata    : RData } with
  member o.pickle = lazy (seq {
    yield! !!o.name.pickle
    yield! o.rrtype |> uint16 |> BitConverter.GetBytes |> Array.rev
    yield! o.rrclass |> uint16 |> BitConverter.GetBytes |> Array.rev
    yield! o.ttl |> uint16 |> BitConverter.GetBytes |> Array.rev
    yield! o.rdata.pickle } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    //printfn "%A" stream.Position
    let name     = domain_name.unpickle stream
    //skip stream 3L
    let rrtype   = readUInt16 stream |> LanguagePrimitives.EnumOfValue<uint16,RRType>
    let rrclass  = readUInt16 stream |> LanguagePrimitives.EnumOfValue<uint16,RRClass>
    let ttl      = readUInt32 stream
    let rdlength = readUInt16 stream
    let rdata    = RData.unpickle stream rrtype
    { name     = name
      rrtype   = rrtype
      rrclass  = rrclass 
      ttl      = ttl
      rdlength = rdlength
      rdata    = rdata }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream(bytes)
    rr.unpickle ms


