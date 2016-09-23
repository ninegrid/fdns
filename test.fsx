
#r @"F:\Development\thinkhard\bin\Debug\thinkhard.dll"
#r @"F:\Development\thinkhard\bin\Debug\thinkhard.Protocol.dll"

open System
open System.Collections
open System.Collections.Generic
open System.IO
open System.Net
open System.Net.Sockets
open thinkhard
open thinkhard.Stream
open thinkhard.Protocol
open thinkhard.Protocol.Dns

module CharacterStringTest =
  let cs1   = { characters = "It was the best of times, it was the worst of times." }
  let cs1xs = cs1.pickle
  let cs1ms = new MemoryStream(!!cs1xs)
  let cs1'  = character_string.unpickle cs1ms

// add a comment
module DomainNameTest =
  let dn1   = { name = "nfin.ch" }
  let dn1xs = dn1.pickle
  let dn1ms = new MemoryStream(!!dn1xs)
  let dn1'  = domain_name.unpickle dn1ms

module RRTest = begin
  end

module HeaderTest =
  let h1 = { id = 100us;
             qr = false;
             opcode = OPCode.Query;
             aa = false;
             tc = false;
             rd = true;
             ra = false;
             z = 7uy;
             rcode = RCode.NoError;
             qdcount = 1us;
             ancount = 0us;
             nscount = 0us;
             arcount = 0us; }

  let h1xs = h1.pickle
  let h1ms = new MemoryStream(!!h1xs)
  let h1' = header.unpickle h1ms

module QuestionTest =
  let q1 = { qname  = DomainNameTest.dn1
             qtype  = QType.MX
             qclass = QClass.IN }
  
  let q1xs = q1.pickle
  let q1ms = new MemoryStream(!!q1xs)
  let q1'  = question.unpickle q1ms

printfn "%A\tcharacter_string" (CharacterStringTest.cs1 = CharacterStringTest.cs1')
printfn "%A\tdomain_name" (DomainNameTest.dn1 = DomainNameTest.dn1')
printfn "%A\theader" (HeaderTest.h1 = HeaderTest.h1')
printfn "%A\tquestion" (QuestionTest.q1 = QuestionTest.q1')

let rq1 = { header     =   HeaderTest.h1
            question   = [ QuestionTest.q1 ]
            answer     = []
            authority  = []
            additional = [] }


let req = {
  header     = { id = 100us; // random ushort
                 qr = false;
                 opcode = OPCode.Query;
                 aa = false;
                 tc = false;
                 rd = true;  // recursion desired
                 ra = false;
                 z = 7uy;
                 rcode = RCode.NoError;
                 qdcount = 1us;
                 ancount = 0us;
                 nscount = 0us;
                 arcount = 0us; }
  question   = [ { qname  = { name = "transaffiliate.com" }
                   qtype  = QType.AAAA
                   qclass = QClass.IN } ]
  answer     = []
  authority  = []
  additional = [] }


let timeout = 10
let socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
let sw = new System.Diagnostics.Stopwatch()
sw.Start()
do  socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, timeout * 1000)
do  socket.SendTo(!!req.pickle, new IPEndPoint(IPAddress.Parse("10.0.0.5"), 53)) |> ignore
let responseMessage = Array.create 65534 0uy
let received = socket.Receive(responseMessage)
do  socket.Close()
sw.Stop()
printfn "Network Elapsed: %A, Ticks: %A, Milliseconds: %A" sw.Elapsed sw.ElapsedTicks sw.ElapsedMilliseconds
sw.Reset()
sw.Start()
let ms = new MemoryStream(Array.sub responseMessage 0 received)

let resolved = requestion.unpickle ms
sw.Stop()
printfn "Data Elapsed: %A, Ticks: %A, Milliseconds: %A" sw.Elapsed sw.ElapsedTicks sw.ElapsedMilliseconds



let testDataWithIPAndUDPHeaders =
(*0000*)[| 0b00000000;0b00011101;0b10010010;0b00110011;0b01110111;0b01001101;0b00000000;0b01010000;//...3wM.P
(*0008*)   0b11111100;0b10011010;0b01000101;0b01101001;0b00001000;0b00000000;0b01000101;0b00000000;//..Ei..E.
(*0010*)   0b00000000;0b01010100;0b01101111;0b11011100;0b00000000;0b00000000;0b10000000;0b00010001;//.To.....
(*0018*)   0b10110110;0b10100011;0b00001010;0b00000000;0b00000000;0b00000101;0b00001010;0b00000000;//........
(*0020*)   0b00000000;0b00010101;0b00000000;0b00110101;0b11010011;0b10101100;0b00000000;0b01000000;//...5...@
(*0028*)   0b00100111;0b00000000;0b10010110;0b01101001;0b10000001;0b10000000;0b00000000;0b00000001;//'..i....
(*0030*)   0b00000000;0b00000001;0b00000000;0b00000000;0b00000000;0b00000000;0b00000011;0b01110111;//.......w
(*0038*)   0b01110111;0b01110111;0b00001110;0b01110100;0b01110010;0b01100001;0b01101110;0b01110011;//ww.trans
(*0040*)   0b01100001;0b01100110;0b01100110;0b01101001;0b01101100;0b01101001;0b01100001;0b01110100;//affiliat
(*0048*)   0b01100101;0b00000011;0b01100011;0b01101111;0b01101101;0b00000000;0b00000000;0b00000001;//e.com...
(*0050*)   0b00000000;0b00000001;0b11000000;0b00001100;0b00000000;0b00000001;0b00000000;0b00000001;//........
(*0058*)   0b00000000;0b00000000;0b00001110;0b00010000;0b00000000;0b00000100;0b11001111;0b01000111;//.......G
(*0060*)   0b00000101;0b11000010; |] |> Array.map (byte)


open System.Text

let rec dn_aux (sb : StringBuilder) (pos : int64 option) =
  let length = readByte ms |> int
  if (length &&& 0xC0) = 0xC0 then
      let pos = Some (ms.Position)
      ms.Position <- ((length &&& 0x3F <<< 8 ||| (int <| readByte ms)) |> int64)
      dn_aux sb pos
  else
      rewind ms 1L
      sb.Append((character_string.unpickle ms).characters) |> ignore
      let length' = readByte ms |> int
      if length' > 0 then
        rewind ms 1L
        dn_aux (sb.Append(".")) pos
      else
        Option.iter (fun x -> ms.Position <- x + 1L) pos
        { name = sb.ToString() }





  (*
  let data = 
    new MemoryStream([| 0x00; 0x02; 0x01; 0x00; 0x00; 0x01; 0x00; 0x00; 0x00; 0x00; 0x00; 0x00;
                        0x04; 0x70; 0x6F; 0x70; 0x64; 0x02; 0x69; 0x78; 0x06; 0x6E; 0x65; 0x74;
                        0x63; 0x6F; 0x6D; 0x03; 0x63; 0x6F; 0x6D; 0x00; 0x00; 0x01; 0x00; 0x01; |] |> Array.map (byte))
  // header.streamUnpickle data
  question.streamUnpickle data
  


  let resp = [|  
     // header
    0x00;0x02;0x85;0x80;0x00;0x01;0x00;0x03;0x00;0x06;0x00;0x06;
    // question
    0x04;0x70;0x6F;0x70;0x64;0x02;0x69;0x78;0x06;0x6E;0x65;0x74;0x63;0x6F;0x6D;0x03;0x63;0x6F;0x6D;0x00;0x00;0x01;0x00;0x01;
    // answer
    // rr header
    0xC0;0x0C;0x00;0x05;0x00;0x01;0x00;0x00;0x00;0x3C;0x00;0x19;
    // rr rdata
    0x04;0x70;0x6F;0x70;0x64;0x04;0x62;0x65;0x73;0x74;0x02;0x69;0x78;0x06;0x6E;0x65;0x74;0x63;0x6F;0x6D;0x03;0x63;0x6F;0x6D;0x00;
    0xC0;0x30;0x00;0x05;0x00;0x01;0x00;0x00;0x00;0x00;0x00;0x06;
    0x03;0x69;0x78;0x36;0xC0;0x3A;
    0xC0;0x55;0x00;0x01;0x00;0x01;0x00;0x00;0x1C;0x20;0x00;0x04;
    0xC7;0xB6;0x78;0x06;
    //authority
    0xC0;0x3A;0x00;0x02;0x00;0x01;0x00;0x00;0x1C;0x20;0x00;0x06;
    0x03;0x6E;0x73;0x31;0xC0;0x3A;
    0xC0;0x3A;0x00;0x02;0x00;0x01;0x00;0x00;0x1C;0x20;0x00;0x06;
    0x03;0x6E;0x73;0x32;0xC0;0x3A;
    0xC0;0x3A;0x00;0x02;0x00;0x01;0x00;0x00;0x1C;0x20;0x00;0x06;0x03;
    0x6E;0x73;0x33;0xC0;0x3A;
    0xC0;0x3A;0x00;0x02;0x00;0x01;0x00;0x00;0x1C;0x20;0x00;0x06;0x03;
    0x6E;0x73;0x34;0xC0;0x3A;
    0xC0;0x3A;0x00;0x02;0x00;0x01;0x00;0x00;0x1C;0x20;0x00;0x0C;0x09;
    0x64;0x66;0x77;0x2D;0x69;0x78;0x6E;0x73;0x31;0xC0;0x3A;
    0xC0;0x3A;0x00;0x02;0x00;0x01;0x00;0x00;0x1C;0x20;0x00;0x0C;0x09;
    0x64;0x66;0x77;0x2D;0x69;0x78;0x6E;0x73;0x32;0xC0;0x3A;
    // additional
    0xC0;0x77;0x00;0x01;0x00;0x01;0x00;0x00;0x1C;0x20;0x00;0x04;
    0xC7;0xB6;0x78;0xCB;
    0xC0;0x89;0x00;0x01;0x00;0x01;0x00;0x00;0x1C;0x20;0x00;0x04;
    0xC7;0xB6;0x78;0xCA;
    0xC0;0x9B;0x00;0x01;0x00;0x01;0x00;0x00;0x1C;0x20;0x00;0x04;
    0xC7;0xB6;0x78;0x01;
    0xC0;0xAD;0x00;0x01;0x00;0x01;0x00;0x00;0x1C;0x20;0x00;0x04;
    0xC7;0xB6;0x78;0x02;
    0xC0;0xBF;0x00;0x01;0x00;0x01;0x00;0x00;0x1C;0x20;0x00;0x04;
    0xCE;0xD6;0x62;0x21;
    0xC0;0xD7;0x00;0x01;0x00;0x01;0x00;0x00;0x1C;0x20;0x00;0x04;
    0xCE;0xD6;0x62;0x22 |] |> Array.map (byte)

  let rrdata = [| (* rr       *) 0xC0; 0x0C ;0x00 ;0x05 ;0x00 ;0x01 ;0x00 ;0x00 ;0x00 ;0x3C ;0x00 ;0x19;
                  (* rdata    *) 0x04 ;0x70 ;0x6F ;0x70 ;0x64 ;0x04 ;0x62 ;0x65 ;0x73 ;0x74 ;0x02 ;0x69 ;0x78 ;0x06 ;0x6E ;0x65 ;0x74 ;0x63 ;0x6F ;0x6D ;0x03 ;0x63 ;0x6F ;0x6D ;0x00; |] |> Array.map (byte)

  let respms = new MemoryStream(resp)
  requestion.streamUnpickle (respms)

  respms.Position
  (*

  let h = [| 0x00; 0x02; 0x85; 0x80; 0x00; 0x01; 0x00; 0x03; 0x00; 0x06; 0x00; 0x06 |] |> Array.map (byte)
  let q = [| 0x04; 0x70; 0x6F; 0x70; 0x64; 0x02; 0x69; 0x78; 0x06; 0x6E; 0x65; 0x74; 0x63; 0x6F; 0x6D; 0x03; 0x63; 0x6F; 0x6D; 0x00; 0x00; 0x01; 0x00; 0x01; 
             |] |> Array.map (byte)

  let h' = header.unpickle h

  let q' = question.unpickle q

  q'.pickle

  question.unpickle(q)
  header.unpickle(h)
  
*)
(*

  BitConverter.ToUInt16([|0xC0;0x0C|] |> Array.map (byte),0)

  character_string.unpickle (new MemoryStream({ characters = "onetwothreefour" }.pickle))

  let xs = { characters = "onetwothreefour" }.pickle
  let ms = (new MemoryStream(xs))

  ms.ReadByte()
  ms.Position <- 0L
  ms.ToArray()

  let length = readByte ms
  let sb = new System.Text.StringBuilder()
  for x in 1uy .. length do sb.Append(readChar ms) |> ignore
  sb.ToString()

  character_string.streamUnpickle ms *)

  *)


