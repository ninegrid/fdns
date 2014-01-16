namespace thinkhard.Protocol.Dns

open System
open System.IO

open thinkhard.Stream

type header = {
(*  octets    1,2     *)
  /// 16 bit message ID supplied by the requestion (the questioner) and reflected back unchanged by the responder (answerer). Identifies the transaction.
  id      : uint16
(*  octets    3,4     *)
  /// 1 bit. Set to 0 by the questioner (query) and to 1 in the response (answer).
  qr      : bool
  /// 4 bits. Identifies the request/operation type.
  opcode  : OPCode
  /// 1 bit. Authoritative Answer. Valid in responses only. Because of aliases multiple owners may exists so the AA bit corresponds to the name which matches the query name, OR the first owner name in the answer section.
  aa      : bool
  /// 1 bit. TrunCation - specifies that this message was truncated due to length greater than that permitted on the transmission channel. Set on all truncated messages except the last one.
  tc      : bool
  /// 1 bit. Recursion Desired - this bit may be set in a query and is copied into the response if recursion supported if rejected the response (answer) does not have this bit set. Recursive query support is optional.
  rd      : bool
  /// 1 bit. Recursion Available - this bit is valid in a response (answer) and denotes whether recursive query support is available (1) or not (0) in the name server.
  ra      : bool
  /// 3 bits. Res1, Res2, Res3 -- Should explain what these mean...
  z       : byte
  /// 4 bits. Identifies the response type to the query. Ignored on a request (question).
  rcode   : RCode 
(*  octets    5,6     *)
  /// Unsigned 16 bit integer specifying the number of entries in the question section.
  qdcount : uint16
(*  octets    7,8     *)
  /// Unsigned 16 bit integer specifying the number of resource records in the answer section. May be 0 in which case no answer record is present in the message.
  ancount : uint16
(*  octets    9,10    *)
  /// Unsigned 16 bit integer specifying the number of name server resource records in the authority section. May be 0 in which case no authority record(s) is(are) present in the message.
  nscount : uint16
(*  octets    11,12   *)
  /// Unsigned 16 bit integer specifying the number of resource records in the additional records section. May be 0 in which case no addtional record(s) is(are) present in the message.
  arcount : uint16 } with
  member o.pickle = lazy (
    let rev = Array.rev
    let id = BitConverter.GetBytes(o.id)
    let qr = BitConverter.GetBytes(o.qr).[0]
    let opcode = byte o.opcode
    let aa = BitConverter.GetBytes(o.aa).[0]
    let tc = BitConverter.GetBytes(o.tc).[0]
    let rd = BitConverter.GetBytes(o.rd).[0]
    let ra = BitConverter.GetBytes(o.ra).[0]
    let z = o.z
    let rcode = byte o.rcode
    let qdcount = BitConverter.GetBytes(o.qdcount)
    let ancount = BitConverter.GetBytes(o.ancount)
    let nscount = BitConverter.GetBytes(o.nscount)
    let arcount = BitConverter.GetBytes(o.arcount)
    seq {
      yield! rev id
      yield  (qr <<< 7) ||| (opcode <<< 3) ||| (aa <<< 2) ||| (tc <<< 1) ||| rd
      yield  (ra <<< 7) ||| (z <<< 4) ||| rcode
      yield! rev qdcount
      yield! rev ancount
      yield! rev nscount
      yield! rev arcount
    } |> Array.ofSeq)

  /// unpickle a header record from the current position in the stream
  static member unpickle (stream : Stream) =
    let id         = readUInt16 stream
    let qrMask     = 0b10000000uy
    let opcodeMask = 0b01111000uy
    let aaMask     = 0b00000100uy
    let tcMask     = 0b00000010uy
    let rdMask     = 0b00000001uy
    let raMask     = 0b10000000uy
    let zMask      = 0b01110000uy
    let rcodeMask  = 0b00001111uy
    let flags      = readByte stream
    let flags'     = readByte stream
    { id      = id
      qr      = (flags  &&& qrMask) >>> 7 > 0uy
      opcode  = (flags  &&& opcodeMask) >>> 3 |> LanguagePrimitives.EnumOfValue<byte,OPCode>
      aa      = (flags  &&& aaMask) >>> 2 > 0uy
      tc      = (flags  &&& tcMask) >>> 1 > 0uy
      rd      = (flags  &&& rdMask) > 0uy
      ra      = (flags' &&& raMask) >>> 7 > 0uy
      z       = (flags' &&& zMask) >>> 4
      rcode   = (flags' &&& rcodeMask) |> LanguagePrimitives.EnumOfValue<byte,RCode>
      qdcount = readUInt16 stream
      ancount = readUInt16 stream
      nscount = readUInt16 stream
      arcount = readUInt16 stream }

  /// Given a 12 byte array in host order unpickle will return a DNS header record
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream(bytes)
    header.unpickle ms
