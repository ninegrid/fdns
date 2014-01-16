namespace thinkhard.Protocol.Dns

[<AutoOpen>]
module Utility =
  open System
  open System.IO
  open System.Text

  open thinkhard
  open thinkhard.Stream

/// [RFC1035] 3.3. Standard RRs. 
/// <character-string> is a single
/// length octet followed by that number of characters.  <character-string> 
/// is treated as binary information, and can be up to 256 characters in 
/// length (including the length octet).
  type character_string = { characters : string } with
    member o.pickle = lazy Array.append [|byte o.characters.Length|] (Encoding.ASCII.GetBytes(o.characters))
    static member unpickle (stream : Stream) = { characters = readString stream }
    static member unpickle (bytes  : byte []) = use ms = new MemoryStream(bytes) in character_string.unpickle ms

/// [RFC1035] 3.3. Standard RRs. 
/// <domain-name> is a domain name represented as a series of labels, and
/// terminated by a label with zero length.
  type domain_name = { name : string } with 
    member o.pickle = lazy (
      (o.name.ToLower().Split([|'.'|]) |> Array.map (fun x -> !!({ characters = x }).pickle)) |> Array.concat |> flip Array.append [|0uy|])
    static member unpickle (stream : Stream) =
      let sb = new StringBuilder()
      // this needs to be moved/modified so as to handle pointers to this location from recursive calls in aux.
      if (readByte stream = 0uy) then { name = "<Root>" }

      else 
        rewind stream 1L
        let rec aux (sb : StringBuilder) (pos : int64 option) =
          let length = readByte stream |> int
          if (length &&& 0xC0) = 0xC0 then
            let pos = Some (stream.Position)
            stream.Position <- ((length &&& 0x3F <<< 8 ||| (int <| readByte stream)) |> int64)
            aux sb pos
          else // length > 0 
            rewind stream 1L
            sb.Append((character_string.unpickle stream).characters) |> ignore
            let length' = readByte stream |> int
            if length' > 0 then
              rewind stream 1L
              aux (sb.Append(".")) pos
            else 
              Option.iter (fun x -> stream.Position <- x + 1L) pos
              { name = sb.ToString() }
        in aux sb None
    static member unpickle (bytes : byte []) =
      use memstream = new MemoryStream(bytes)
      domain_name.unpickle memstream

