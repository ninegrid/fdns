namespace thinkhard.Protocol.Dns

open System
open System.IO

open thinkhard
open thinkhard.Stream

type question = {
  (*  octets   1,2..n    *)
  /// N bits. The domain name being queried.
  qname  : domain_name
  (*  octets   n+1,n+2   *)
  /// 16 bits. The resource records being requested
  qtype  : QType
  (*  octets   n+3,n+4   *)
  /// 16 bits. The Resource Record(s) class being requested e.g. internet, chaos etc.
  qclass : QClass } with
  member o.pickle = lazy (seq { 
      yield! !!o.qname.pickle
      yield! BitConverter.GetBytes(uint16 o.qtype)  |> Array.rev
      yield! BitConverter.GetBytes(uint16 o.qclass) |> Array.rev } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    { qname  = domain_name.unpickle stream
      qtype  = readUInt16 stream |> LanguagePrimitives.EnumOfValue<uint16,QType>
      qclass = readUInt16 stream |> LanguagePrimitives.EnumOfValue<uint16,QClass> }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream(bytes)
    question.unpickle ms
    
    