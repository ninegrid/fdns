namespace thinkhard.Protocol.Dns

open System
open System.IO
open System.Net

open thinkhard
open thinkhard.Stream

(* 3.4.1. A RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    ADDRESS                    |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// [RFC1035] 3.4.1 A RDATA format. A records cause no additional section processing.  The RDATA section of an A line in a master file is an Internet address expressed as four decimal numbers separated by dots without any imbedded spaces (e.g., "10.2.0.52" or "192.0.5.6").
type A = { 
/// ADDRESS: A 32 bit Internet address. Hosts that have multiple Internet addresses will have multiple A records.
  ipv4Address : IPAddress } with
  member o.pickle = lazy o.ipv4Address.GetAddressBytes()
  static member unpickle (stream : Stream) =
    { ipv4Address = new IPAddress(readBytes stream 4L) }
  static member unpickle (bytes : byte []) = 
    use ms = new MemoryStream(bytes)
    A.unpickle ms

(*  0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                                               |
    |                                               |
    |                                               |
    |                                               |
    |                                               |
    |                                               |
    |                                               |
    |                    ADDRESS                    |
    |                                               |
    |                                               |
    |                                               |
    |                                               |
    |                                               |
    |                                               |
    |                                               |
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// [RFC1886] 2.1 AAAA record type The AAAA resource record type is a new record specific to the Internet class that stores a single IPv6 address.
type AAAA = { 
/// [RFC1886] 2.2 AAAA data format. A 128 bit IPv6 address is encoded in the data portion of an AAAA resource record in network byte order (high-order byte first).
  ipv6Address : IPAddress } with
  member o.pickle = lazy o.ipv6Address.GetAddressBytes()
  static member unpickle (stream : Stream) =
    { ipv6Address = new IPAddress(readBytes stream 16L) }
  static member unpickle (bytes : byte []) = 
    use ms = new MemoryStream(bytes)
    AAAA.unpickle ms

(*  0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    SUBTYPE                    |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   HOSTNAME                    /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
where : *)
///   [RFC1183] 1. AFS Data Base location. The format of the AFSDB RR is class insensitive.  AFSDB records cause type A additional section processing for <hostname>.  This, in fact, is the rationale for using a new type code, rather than trying to build the same functionality with TXT RRs. (AFS is a registered trademark of Transarc Corporation) and for the Open Software Foundation's (OSF) Distributed Computing Environment (DCE) authenticated naming system using HP/Apollo's NCA, both to be components of the OSF DCE. The AFS (originally the Andrew File System) system uses the DNS to map from a domain name to the name of an AFS cell database server.  The DCE Naming service uses the DNS for a similar function: mapping from the domain name of a cell to authenticated name servers for that cell.  The method uses a new RR type with mnemonic AFSDB and type code of 18 (decimal).
type AFSDB = { 
/// The <subtype> field is a 16 bit integer. In the case of subtype 1, the host has an AFS version 3.0 Volume Location Server for the named AFS cell.  In the case of subtype 2, the host has an authenticated name server holding the cell-root directory node for the named DCE/NCA cell.
  subtype  : uint16
/// The <hostname> field is a domain name of a host that has a server for the cell named by the owner name of the RR.
  hostname : domain_name } with
  member o.pickle = lazy (seq { 
    yield! BitConverter.GetBytes(o.subtype) |> Array.rev
    yield! !!o.hostname.pickle } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    { subtype  = readUInt16 stream
      hostname = domain_name.unpickle stream }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream(bytes)
    AFSDB.unpickle ms

(* 3.3.1. CNAME RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                     CNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// [RFC1035] 3.3.1. CNAME RDATA format. CNAME RRs cause no additional section processing, but name servers may choose to restart the query at the canonical name in certain cases. See the description of name server logic in [RFC-1034] for details.
type CNAME = { 
/// CNAME: A <domain-name> which specifies the canonical or primary name for the owner.  The owner name is an alias. 
  cname : domain_name } with
  member o.pickle = o.cname.pickle
  static member unpickle (stream : Stream) = 
    { cname = domain_name.unpickle stream }
  static member unpickle (bytes : byte []) = 
    use ms = new MemoryStream(bytes)
    CNAME.unpickle ms
   
(* 3. The DNAME Resource Record
     
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                     DNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// [RFC2672] 3. The DNAME Resource Record. The DNAME RR causes type NS additional section processing.
type DNAME = { 
/// The RDATA field <target> is a <domain-name> [DNSIS]. 
  dname : domain_name } with
  member o.pickle = o.dname.pickle
  static member unpickle (stream : Stream) = 
    { dname = domain_name.unpickle stream }
  static member unpickle (bytes : byte []) = 
    use ms = new MemoryStream(bytes)
    DNAME.unpickle ms

(* [RFC3658] 2.4.  Wire Format of the DS record

                        1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
    0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |           key tag             |  algorithm    |  Digest type  |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                digest  (length depends on type)               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                (SHA-1 digest is 20 bytes)                     |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                                                               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-|
   |                                                               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-|
   |                                                               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
where: *)
/// [RFC3658] 2.4. Wire Format of the DS Record. The DS (type=43) record contains these fields: key tag, algorithm, digest type, and the digest of a public key KEY record that is allowed and/or used to sign the child's apex KEY RRset.  Other keys MAY sign the child's apex KEY RRset.
type DS = { 
/// The key tag is calculated as specified in RFC 2535.  
  keytag     : uint16
/// Algorithm MUST be allowed to sign DNS data.
  algorithm  : byte
/// The digest type is an identifier for the digest algorithm used
  digestType : byte
/// The digest is calculated over the canonical name of the delegated domain name followed by the whole RDATA of the KEY record (all four fields). digest = hash( canonical FQDN on KEY RR | KEY_RR_rdata).  And, KEY_RR_rdata = Flags | Protocol | Algorithm | Public Key
  digest     : byte [] } with
  member o.pickle = lazy (seq { 
    yield! BitConverter.GetBytes(o.keytag) |> Array.rev
    yield  o.algorithm
    yield  o.digestType
    yield! o.digest } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    rewind stream 2L
    let length   = readUInt16 stream |> int64
    { keytag     = readUInt16 stream
      algorithm  = readByte   stream
      digestType = readByte   stream
      digest     = readBytes  stream length }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream(bytes)
    DS.unpickle ms

(*
/// [RFC1035] 3.3.2. HINFO RDATA format
/// 
///     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
///     /                      CPU                      /
///     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
///     /                       OS                      /
///     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
/// 
/// where:
/// 
/// CPU             A <character-string> which specifies the CPU type.
/// 
/// OS              A <character-string> which specifies the operating
///                 system type.
/// 
/// Standard values for CPU and OS can be found in [RFC-1010].
/// 
/// HINFO records are used to acquire general information about a host.  The
/// main use is for protocols such as FTP that can use special procedures
/// when talking between machines or operating systems of the same type.
*)

/// [RFC1183] 3.2. The ISDN RR
type ISDN = {
/// <ISDN-address> identifies the ISDN number of <owner> and DDI (Direct Dial In) if any, as defined by E.164 [8] and E.163 [7], the ISDN and PSTN (Public Switched Telephone Network) numbering plan.  E.163 defines the country codes, and E.164 the form of the addresses.
  isdn_address : character_string
/// <sa> specifies the subaddress (SA).  The format of <sa> in master files is a <character-string>
  sa           : character_string } with
  member o.pickle = lazy (seq {
    yield! !!o.isdn_address.pickle
    yield! !!o.sa.pickle })
  static member unpickle (ms : MemoryStream) =
    { isdn_address = character_string.unpickle ms
      sa           = character_string.unpickle ms }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream()
    ISDN.unpickle ms

(* 3.1 KEY RDATA format

                        1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
    0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |             flags             |    protocol   |   algorithm   |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                                                               /
   /                          public key                           /
   /                                                               /
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-|

   3.1.2 The KEY RR Flag Field

     0   1   2   3   4   5   6   7   8   9   0   1   2   3   4   5
   +---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+
   |  A/C  | Z | XT| Z | Z | NAMTYP| Z | Z | Z | Z |      SIG      |
   +---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+
where: *)
/// [RFC2535] 3.1 KEY RDATA format. The KEY RR is not intended for storage of certificates and a separate certificate RR has been developed for that purpose, defined in [RFC2538]. The meaning of the KEY RR owner name, flags, and protocol octet are described in Sections 3.1.1 through 3.1.5 below.  The flags and algorithm must be examined before any data following the algorithm octet as they control the existence and format of any following data. The algorithm and public key fields are described in Section 3.2. The format of the public key is algorithm dependent.
type KEY = {
  flags     : uint16
  protocol  : byte
  algorithm : byte
  publicKey : character_string } with
  member o.pickle = lazy (seq {
    yield! o.flags |> BitConverter.GetBytes |> Array.rev 
    yield  o.protocol
    yield  o.algorithm
    yield! !!o.publicKey.pickle } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    { flags     = readUInt16 stream
      protocol  = readByte stream
      algorithm = readByte stream
      publicKey = { characters = readString stream } }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream(bytes)
    KEY.unpickle ms

(* 3.1 KX RDATA format

   The KX DNS record has the following RDATA format:

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                  PREFERENCE                   |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   EXCHANGER                   /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// [RFC2230] 3.1 KX RDATA format KX records MUST cause type A additional section processing for the host specified by EXCHANGER.  In the event that the host processing the DNS transaction supports IPv6, KX records MUST also cause type AAAA additional section processing. The KX RDATA field MUST NOT be compressed.
type KX = {
/// PREFERENCE: A 16 bit non-negative integer which specifies the preference given to this RR among other KX records at the same owner.  Lower values are preferred.
  preference : uint16
/// EXCHANGER: A <domain-name> which specifies a host willing to act as a mail exchange for the owner name.
  exchanger  : domain_name } with
  member o.pickle = lazy (seq {
    yield! o.preference |> BitConverter.GetBytes |> Array.rev
    yield! !!o.exchanger.pickle } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    { preference = readUInt16 stream
      exchanger  = domain_name.unpickle stream } 
  static member unpickle (bytes: byte []) =
    use ms = new MemoryStream(bytes)
    KX.unpickle ms

(* 2. LOC RDATA Format

       MSB                                           LSB
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
      0|        VERSION        |         SIZE          |
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
      2|       HORIZ PRE       |       VERT PRE        |
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
      4|                   LATITUDE                    |
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
      6|                   LATITUDE                    |
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
      8|                   LONGITUDE                   |
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
     10|                   LONGITUDE                   |
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
     12|                   ALTITUDE                    |
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
     14|                   ALTITUDE                    |
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
   (octet)
where: *)
/// Some uses for the LOC RR have already been suggested, including the USENET backbone flow maps, a "visual traceroute" application showing the geographical path of an IP packet, and network management applications that could use LOC RRs to generate a map of hosts and routers being managed.
type LOC = {
/// VERSION: Version number of the representation.  This must be zero. Implementations are required to check this field and make no assumptions about the format of unrecognized versions.
  version   : byte
/// SIZE: The diameter of a sphere enclosing the described entity, in centimeters, expressed as a pair of four-bit unsigned integers, each ranging from zero to nine, with the most significant four bits representing the base and the second number representing the power of ten by which to multiply the base.  This allows sizes from 0e0 (<1cm) to 9e9 (90,000km) to be expressed.  This representation was chosen such that the hexadecimal representation can be read by eye; 0x15 = 1e5. Four-bit values greater than 9 are undefined, as are values with a base of zero and a non-zero exponent. Since 20000000m (represented by the value 0x29) is greater than the equatorial diameter of the WGS 84 ellipsoid (12756274m), it is therefore suitable for use as a "worldwide" size.
  size      : byte
/// HORIZ PRE: The horizontal precision of the data, in centimeters, expressed using the same representation as SIZE.  This is the diameter of the horizontal "circle of error", rather than a "plus or minus" value.  (This was chosen to match the interpretation of SIZE; to get a "plus or minus" value, divide by 2.)
  horizpre  : byte
/// VERT PRE: The vertical precision of the data, in centimeters, expressed using the sane representation as for SIZE.  This is the total potential vertical error, rather than a "plus or minus" value.  (This was chosen to match the interpretation of SIZE; to get a "plus or minus" value, divide by 2.)  Note that if altitude above or below sea level is used as an approximation for altitude relative to the [WGS 84] ellipsoid, the precision value should be adjusted.
  vertpre   : byte
/// LATITUDE: The latitude of the center of the sphere described by the SIZE field, expressed as a 32-bit integer, most significant octet first (network standard byte order), in thousandths of a second of arc.  2^31 represents the equator; numbers above that are north latitude.
  latitude  : uint32
/// LONGITUDE: The longitude of the center of the sphere described by the SIZE field, expressed as a 32-bit integer, most significant octet first (network standard byte order), in thousandths of a second of arc, rounded away from the prime meridian. 2^31 represents the prime meridian; numbers above that are east longitude.
  longitude : uint32
/// ALTITUDE: The altitude of the center of the sphere described by the SIZE field, expressed as a 32-bit integer, most significant octet first (network standard byte order), in centimeters, from a base of 100,000m below the [WGS 84] reference spheroid used by GPS (semimajor axis a=6378137.0, reciprocal flattening rf=298.257223563).  Altitude above (or below) sea level may be used as an approximation of altitude relative to the the [WGS 84] spheroid, though due to the Earth's surface not being a perfect spheroid, there will be differences. (For example, the geoid (which sea level approximates) for the continental US ranges from 10 meters to 50 meters below the [WGS 84] spheroid. Adjustments to ALTITUDE and/or VERT PRE will be necessary in most cases. The Defense Mapping Agency publishes geoid height values relative to the [WGS 84] ellipsoid.
  altitude  : uint32 } with
  member o.pickle = lazy (seq { 
    yield  o.version
    yield  o.size
    yield  o.horizpre
    yield  o.vertpre
    yield! o.latitude  |> BitConverter.GetBytes |> Array.rev
    yield! o.longitude |> BitConverter.GetBytes |> Array.rev
    yield! o.altitude  |> BitConverter.GetBytes |> Array.rev } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    { version   = readByte   stream
      size      = readByte   stream
      horizpre  = readByte   stream
      vertpre   = readByte   stream
      latitude  = readUInt32 stream
      longitude = readUInt32 stream
      altitude  = readUInt32 stream }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream(bytes)
    LOC.unpickle ms

(* 3.3.3. MB RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MADNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// [RFC1035] 3.3.3. MB RDATA format (EXPERIMENTAL). MB records cause additional section processing which looks up  an A type RRs corresponding to MADNAME.
type MB = { 
/// MADNAME: A <domain-name> which specifies a host which has the specified mailbox. 
  mbmadname : domain_name } with
  member o.pickle = o.mbmadname.pickle
  static member unpickle (stream : Stream) = { mbmadname = domain_name.unpickle stream }
  static member unpickle (bytes : byte []) = { mbmadname = domain_name.unpickle bytes }

(* 3.3.4. MD RDATA format (Obsolete)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MADNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// [RFC1035] 3.3.4. MD RDATA format (Obsolete)  MD records cause additional section processing which looks up an A type record corresponding to MADNAME. MD is obsolete.  See the definition of MX and [RFC-974] for details of the new scheme.  The recommended policy for dealing with MD RRs found in a master file is to reject them, or to convert them to MX RRs with a preference of 0.
type MD = { 
/// MADNAME: A <domain-name> which specifies a host which has a mail agent for the domain which should be able to deliver mail for the domain.
  mdmadname : domain_name } with
  member o.pickle = o.mdmadname.pickle
  static member unpickle (stream : Stream) = { mdmadname = domain_name.unpickle stream }
  static member unpickle (bytes : byte []) = { mdmadname = domain_name.unpickle bytes }

(* 3.3.5. MF RDATA format (Obsolete)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MADNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// [RFC1035] 3.3.5. MF RDATA format (Obsolete). MF is obsolete.  See the definition of MX and [RFC-974] for details ofw the new scheme.  The recommended policy for dealing with MD RRs found in a master file is to reject them, or to convert them to MX RRs with a preference of 10.
type MF = { 
/// MADNAME: A <domain-name> which specifies a host which has a mail agent for the domain which will accept mail for forwarding to the domain. MF records cause additional section processing which looks up an A type record corresponding to MADNAME.
  mfmadname : domain_name } with
  member o.pickle = o.mfmadname.pickle
  static member unpickle (stream : Stream) = { mfmadname = domain_name.unpickle stream }
  static member unpickle (bytes : byte []) = { mfmadname = domain_name.unpickle bytes }

(* 3.3.6. MG RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MGMNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// [RFC1035] 3.3.6. MG RDATA format (Experimental).  MG records cause no additional section processing.
type MG = {
/// MGMNAME: A <domain-name> which specifies a mailbox which is a member of the mail group specified by the domain name.
  mgname : domain_name } with
  member o.pickle = o.mgname.pickle
  static member unpickle (stream : Stream) = { mgname = domain_name.unpickle stream }
  static member unpickle (bytes : byte []) = { mgname = domain_name.unpickle bytes }

(* 3.3.7. MINFO RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                    RMAILBX                    /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                    EMAILBX                    /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// [RFC1035] 3.3.7. MINFO RDATA format (Experimental). MINFO records cause no additional section processing.  Although these records can be associated with a simple mailbox, they are usually used with a mailing list.
type MINFO = {
/// RMAILBX: A <domain-name> which specifies a mailbox which is responsible for the mailing list or mailbox.  If this domain name names the root, the owner of the MINFO RR is responsible for itself.  Note that many existing mailing lists use a mailbox X-request for the RMAILBX field of mailing list X, e.g., Msgroup-request for Msgroup.  This field provides a more general mechanism.
  rmailbox : domain_name
/// EMAILBX: A <domain-name> which specifies a mailbox which is to receive error messages related to the mailing list or mailbox specified by the owner of the MINFO RR (similar to the ERRORS-TO: field which has been proposed).  If this domain name names the root, errors should be returned to the sender of the message.
  emailbox : domain_name } with 
  member o.pickle = lazy (seq { 
    yield! !!o.rmailbox.pickle
    yield! !!o.emailbox.pickle } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    { rmailbox = domain_name.unpickle stream
      emailbox = domain_name.unpickle stream }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream(bytes)
    MINFO.unpickle ms

(* 3.3.8. MR RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   NEWNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// [RFC1035] 3.3.8. MR RDATA format (Experimental). MR records cause no additional section processing.  The main use for MR is as a forwarding entry for a user who has moved to a different mailbox.
type MR = { 
/// NEWNAME: A <domain-name> which specifies a mailbox which is the proper rename of the specified mailbox.
  newname : domain_name } with
  member o.pickle = o.newname.pickle
  static member unpickle (stream : Stream) = { newname = domain_name.unpickle stream }
  static member unpickle (bytes : byte []) = { newname = domain_name.unpickle bytes }

(* 3.3.9. MX RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                  PREFERENCE                   |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   EXCHANGE                    /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// [RFC1035] 3.3.9. MX RDATA format. MX records cause type A additional section processing for the host specified by EXCHANGE.  The use of MX RRs is explained in detail in [RFC-974].
type MX = {
/// PREFERENCE: A 16 bit integer which specifies the preference given to this RR among others at the same owner.  Lower values are preferred.
  preference : uint16
/// EXCHANGE: A <domain-name> which specifies a host willing to act as a mail exchange for the owner name.
  exchange   : domain_name } with
  member o.pickle = lazy (seq { 
    yield! o.preference |> BitConverter.GetBytes |> Array.rev
    yield! !!o.exchange.pickle } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    { preference = readUInt16 stream
      exchange   = domain_name.unpickle stream }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream(bytes)
    MX.unpickle ms

(*
                                    1  1  1  1  1  1
      0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                     ORDER                     |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                   PREFERENCE                  |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                     FLAGS                     /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   SERVICES                    /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                    REGEXP                     /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                  REPLACEMENT                  /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
*)
type NAPTR = {
  order       : uint16
  preference  : uint16
  flags       : character_string
  services    : character_string
  regexp      : character_string
  replacement : domain_name } with
  member o.pickle = lazy (seq { 
    yield! o.order |> BitConverter.GetBytes |> Array.rev
    yield! o.preference |> BitConverter.GetBytes |> Array.rev
    yield! !!o.flags.pickle 
    yield! !!o.services.pickle
    yield! !!o.regexp.pickle 
    yield! !!o.replacement.pickle } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    { order       = readUInt16                stream
      preference  = readUInt16                stream
      flags       = character_string.unpickle stream
      services    = character_string.unpickle stream
      regexp      = character_string.unpickle stream
      replacement = domain_name.unpickle      stream }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream(bytes)
    NAPTR.unpickle ms

(* 3.3.11. NS RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   NSDNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// NS records cause both the usual additional section processing to locate a type A record, and, when used in a referral, a special search of the zone in which they reside for glue information. The NS RR states that the named host should be expected to have a zone starting at owner name of the specified class.  Note that the class may not indicate the protocol family which should be used to communicate with the host, although it is typically a strong hint.  For example, hosts which are name servers for either Internet (IN) or Hesiod (HS) class information are normally queried using IN class protocols.
type NS = { 
/// NSDNAME: A <domain-name> which specifies a host which should be authoritative for the specified class and domain.
  nsdname : domain_name } with
  member o.pickle = o.nsdname.pickle
  static member unpickle (stream : Stream) = { nsdname = domain_name.unpickle stream }
  static member unpickle (bytes : byte []) = { nsdname = domain_name.unpickle bytes }

(*            |--------------|
              | <-- IDP -->  |
              |--------------|-------------------------------------|
              | AFI |  IDI   |            <-- DSP -->              |
              |-----|--------|-------------------------------------|
              | 47  |  0005  | DFI | AA |Rsvd | RD |Area | ID |Sel |
              |-----|--------|-----|----|-----|----|-----|----|----|
       octets |  1  |   2    |  1  | 3  |  2  | 2  |  2  | 6  | 1  |
              |-----|--------|-----|----|-----|----|-----|----|----|

                    IDP    Initial Domain Part
                    DSP    Domain Specific Part
where: *)
/// [RFC1706] 
type NSAP = {
/// AFI: Authority and Format Identifier
  afi  : byte
/// IDI: Initial Domain Identifier
  idi  : uint16
/// DFI: DSP Format Identifier
  dfi  : byte
/// AA: Administrative Authority
  aa   : uint32
/// Rsvd: Reserved
  rsvd : uint16
/// RD: Routing Domain Identifier
  rd   : uint16
/// Area: Area Identifier
  area : uint16
/// ID: System Identifier
  id   : uint64
/// SEL: NSAP Selector
  sel  : byte } with
  member o.pickle = lazy (seq {
    yield  o.afi
    yield! o.idi  |> BitConverter.GetBytes |> Array.rev
    yield  o.dfi
    yield! o.aa   |> BitConverter.GetBytes |> Array.rev
    yield! o.rsvd |> BitConverter.GetBytes |> Array.rev
    yield! o.rd   |> BitConverter.GetBytes |> Array.rev
    yield! o.id   |> BitConverter.GetBytes |> Array.rev } |> Array.ofSeq)
  static member unpickle (stream : Stream) = 
    { afi  = readByte   stream
      idi  = readUInt16 stream
      dfi  = readByte   stream
      aa   = BitConverter.ToUInt32(readBytes stream 3L |> Array.rev,0)
      rsvd = readUInt16 stream
      rd   = readUInt16 stream
      area = readUInt16 stream
      id   = BitConverter.ToUInt64(readBytes stream 6L |> Array.rev,0)
      sel  = readByte stream }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream(bytes)
    NSAP.unpickle ms

(* 3.3.10. NULL RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                  <anything>                   /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// [RFC1035] 3.3.10. NULL RDATA format (Experimental). NULL records cause no additional section processing.  NULL RRs are not allowed in master files.  NULLs are used as placeholders in some experimental extensions of the DNS.
type NULL = {
/// ANYTHING: anything at all may be in the RDATA field so long as it is 65535 octets or less.
  anything : byte [] } with
  member o.pickle = lazy (o.anything)
  static member unpickle (stream : Stream) =
    rewind stream 2L
    let length = readUInt16 stream |> int64
    { anything = readBytes stream length }
  static member unpickle (bytes : byte []) =
    { anything = bytes }

(* 3.3.12. PTR RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   PTRDNAME                    /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:
*)
/// PTR records cause no additional section processing.  These RRs are used in special domains to point to some other location in the domain space. These records are simple data, and don't imply any special processing similar to that performed by CNAME, which identifies aliases.  See the description of the IN-ADDR.ARPA domain for an example.
type PTR = {
/// PTRDNAME: A <domain-name> which points to some location in the domain name space.
  ptrdname : domain_name } with
  member o.pickle = o.ptrdname.pickle
  static member unpickle (stream : Stream) = { ptrdname = domain_name.unpickle stream }
  static member unpickle (bytes : byte []) = { ptrdname = domain_name.unpickle bytes }

(* 4. The new DNS resource record for MIXER mapping rules: PX

   The definition of the new 'PX' DNS resource record is:

      class:        IN   (Internet)

      name:         PX   (pointer to X.400/RFC822 mapping information)

      value:        26

   The PX RDATA format is:

   +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
   |                  PREFERENCE                   |
   +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
   /                    MAP822                     /
   /                                               /
   +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
   /                    MAPX400                    /
   /                                               /
   +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// PX records cause no additional section processing. When we store in DNS a 'table1' or a 'gate1' entry, then <name> will be an X.400 mail domain name in DNS syntax (see sect. 4.2). When we store a 'table2' or a 'gate2' table entry, <name> will be an RFC822 mail domain name, including both fully qualified DNS domains and mail only domains (MX-only domains). All normal DNS conventions, like default values, wildcards, abbreviations and message compression, apply also for all the components of the PX RR. In particular <name>, MAP822 and MAPX400, as <domain-name> elements, must have the final "." (root) when they are fully qualified.
type PX = {
/// PREFERENCE: A 16 bit integer which specifies the preference given to this RR among others at the same owner.  Lower values are preferred;
  preference : uint16
/// MAP822: A <domain-name> element containing <rfc822-domain>, the RFC822 part of the MCGAM;
  map822     : domain_name
/// MAPX400: A <domain-name> element containing the value of <x400-in-domain-syntax> derived from the X.400 part of the MCGAM (see sect. 4.2);
  mapx400    : domain_name } with
  member o.pickle = lazy (seq {
    yield! o.preference |> BitConverter.GetBytes |> Array.rev
    yield! !!o.map822.pickle
    yield! !!o.mapx400.pickle } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    { preference = readUInt16 stream
      map822     = domain_name.unpickle stream
      mapx400    = domain_name.unpickle stream }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream(bytes)
    PX.unpickle ms

/// [RFC1183] 2.2. The Responsible Person RR. 
type RP = {
/// <mbox-dname>: is a domain name that specifies the mailbox for the responsible person.
  mbox  : domain_name
/// <txt-dname>: is a domain name for which TXT RR's exist.
  txt   : domain_name } with
  member o.pickle = lazy (seq {
    yield! !!o.mbox.pickle
    yield! !!o.txt.pickle } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    { mbox = domain_name.unpickle stream
      txt  = domain_name.unpickle stream }
  static member unipickle (bytes : byte []) =
    use ms = new MemoryStream()
    RP.unpickle ms

/// [RFC1183] 3.3. The Route Through RR
type RT = {
/// <preference>: is a 16 bit integer, representing the preference of the route.  Smaller numbers indicate more preferred routes.
  preference        : uint16
/// <intermediate-host>: is the domain name of a host which will serve as an intermediate in reaching the host specified by <owner>.  The DNS RRs associated with <intermediate-host> are expected to include at
  intermediate_host : domain_name } with
  member o.pickle = lazy (seq {
    yield! o.preference |> BitConverter.GetBytes |> Array.rev
    yield! !!o.intermediate_host.pickle } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    { preference = readUInt16 stream
      intermediate_host = domain_name.unpickle stream }
  static member unipckle (bytes : byte []) =
    use ms = new MemoryStream()
    RT.unpickle ms

(* [RFC2535] 4.1 SIG RDATA Format

                           1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
       0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |        type covered           |  algorithm    |     labels    |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |                         original TTL                          |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |                      signature expiration                     |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |                      signature inception                      |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |            key  tag           |                               |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+         signer's name         +
      |                                                               /
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-/
      /                                                               /
      /                            signature                          /
      /                                                               /
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

where: *)
/// [RFC2535] 2.3.1 The SIG Resource Record
type SIG = {
/// type covered: is the type of the other RRs covered by this SIG.
  type_covered         : uint16
/// algorithm: this octet is as described in [RFC2535] Section 3.2.
  algorithm            : byte
/// The "labels" octet is an unsigned count of how many labels there are in the original SIG RR owner name not counting the null label for root and not counting any initial "*" for a wildcard.
  labels               : byte
/// original_ttl: field is included in the RDATA portion to avoid (1) authentication problems that caching servers would otherwise cause by decrementing the real TTL field and (2) security problems that unscrupulous servers could otherwise cause by manipulating the real TTL field.  This original TTL is protected by the signature while the current TTL field is not.
  original_ttl         : uint32
/// The SIG is valid from the "signature inception" time until the "signature expiration" time.  Both are unsigned numbers of seconds since the start of 1 January 1970, GMT, ignoring leap seconds.  (See also Section 4.4.)  Ring arithmetic is used as for DNS SOA serial numbers [RFC 1982] which means that these times can never be more than about 68 years in the past or the future.  This means that these times are ambiguous modulo ~136.09 years.  However there is no security flaw because keys are required to be changed to new random keys by [RFC 2541] at least every five years.  This means that the probability that the same key is in use N*136.09 years later should be the same as the probability that a random guess will work. A SIG RR may have an expiration time numerically less than the inception time if the expiration time is near the 32 bit wrap around point and/or the signature is long lived.
  signature_expiration : uint32
/// The SIG is valid from the "signature inception" time until the "signature expiration" time.  Both are unsigned numbers of seconds since the start of 1 January 1970, GMT, ignoring leap seconds.  (See also Section 4.4.)  Ring arithmetic is used as for DNS SOA serial numbers [RFC 1982] which means that these times can never be more than about 68 years in the past or the future.  This means that these times are ambiguous modulo ~136.09 years.  However there is no security flaw because keys are required to be changed to new random keys by [RFC 2541] at least every five years.  This means that the probability that the same key is in use N*136.09 years later should be the same as the probability that a random guess will work. A SIG RR may have an expiration time numerically less than the inception time if the expiration time is near the 32 bit wrap around point and/or the signature is long lived.
  signature_inception  : uint32
/// is a two octet quantity that is used to efficiently select between multiple keys which may be applicable and thus check that a public key about to be used for the computationally expensive effort to check the signature is possibly valid.  For algorithm 1 (MD5/RSA) as defined in [RFC 2537], it is the next to the bottom two octets of the public key modulus needed to decode the signature field.  That is to say, the most significant 16 of the least significant 24 bits of the modulus in network (big endian) order. For all other algorithms, including private algorithms, it is calculated as a simple checksum of the KEY RR as described in Appendix C.
  key_tag              : uint16
/// The "signer's name" field is the domain name of the signer generating the SIG RR.  This is the owner name of the public KEY RR that can be used to verify the signature.  It is frequently the zone which contained the RRset being authenticated.  Which signers should be authorized to sign what is a significant resolver policy question as discussed in Section 6. The signer's name may be compressed with standard DNS name compression when being transmitted over the network.
  signers_name         : domain_name
/// The actual signature portion of the SIG RR binds the other RDATA fields to the RRset of the "type covered" RRs with that owner name and class.  This covered RRset is thereby authenticated.  How this data sequence is processed into the signature is algorithm dependent.  These algorithm dependent formats and procedures are described in separate documents ([RFC2535] Section 3.2)
  signature            : character_string } with
  member o.pickle = lazy (seq {
    yield! o.type_covered         |> BitConverter.GetBytes |> Array.rev
    yield  o.algorithm
    yield  o.labels
    yield! o.original_ttl         |> BitConverter.GetBytes |> Array.rev
    yield! o.signature_expiration |> BitConverter.GetBytes |> Array.rev
    yield! o.signature_inception  |> BitConverter.GetBytes |> Array.rev
    yield! o.key_tag              |> BitConverter.GetBytes |> Array.rev
    yield! !!o.signers_name.pickle
    yield! !!o.signature.pickle } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    { type_covered         = readUInt16 stream
      algorithm            = readByte   stream
      labels               = readByte   stream
      original_ttl         = readUInt32 stream
      signature_expiration = readUInt32 stream
      signature_inception  = readUInt32 stream
      key_tag              = readUInt16 stream
      signers_name         = domain_name.unpickle stream
      signature            = character_string.unpickle stream }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream()
    SIG.unpickle ms

(* 3.3.13. SOA RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                     MNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                     RNAME                     /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    SERIAL                     |
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    REFRESH                    |
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                     RETRY                     |
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    EXPIRE                     |
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    MINIMUM                    |
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// [RFC1035] 3.3.13. SOA RDATA format. SOA records cause no additional section processing. All times are in units of seconds. Most of these fields are pertinent only for name server maintenance operations.  However, MINIMUM is used in all query operations that retrieve RRs from a zone.  Whenever a RR is sent in a response to a query, the TTL field is set to the maximum of the TTL field from the RR and the MINIMUM field in the appropriate SOA.  Thus MINIMUM is a lower bound on the TTL field for all RRs in a zone.  Note that this use of MINIMUM should occur when the RRs are copied into the response and not when the zone is loaded from a master file or via a zone transfer.  The reason for this provison is to allow future dynamic update facilities to change the SOA RR with known semantics.
type SOA = {
/// MNAME: The <domain-name> of the name server that was the original or primary source of data for this zone.
  mname   : domain_name
/// RNAME: A <domain-name> which specifies the mailbox of the person responsible for this zone.
  rname   : domain_name
/// SERIAL: The unsigned 32 bit version number of the original copy of the zone.  Zone transfers preserve this value.  This value wraps and should be compared using sequence space arithmetic.
  serial  : uint32
/// REFRESH: A 32 bit time interval before the zone should be refreshed.
  refresh : int
/// RETRY: A 32 bit time interval that should elapse before a failed refresh should be retried.
  retry   : int
/// EXPIRE: A 32 bit time value that specifies the upper limit on the time interval that can elapse before the zone is no longer authoritative.
  expire  : int
/// MINIMUM: The unsigned 32 bit minimum TTL field that should be exported with any RR from this zone.
  minimum : uint32 } with
  member o.pickle = lazy (seq { 
    yield! !!o.mname.pickle
    yield! !!o.rname.pickle
    yield! o.serial  |> BitConverter.GetBytes |> Array.rev
    yield! o.refresh |> BitConverter.GetBytes |> Array.rev
    yield! o.expire  |> BitConverter.GetBytes |> Array.rev
    yield! o.minimum |> BitConverter.GetBytes |> Array.rev } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    { mname   = domain_name.unpickle stream
      rname   = domain_name.unpickle stream
      serial  = readUInt32 stream
      refresh = readInt32  stream
      retry   = readInt32  stream
      expire  = readInt32  stream
      minimum = readUInt32 stream  }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream(bytes)
    SOA.unpickle ms

/// [RFC2782] The SRV RR allows administrators to use several servers for a single domain, to move services from host to host with little fuss, and to designate some hosts as primary servers for a service and others as backups.
type SRV = { 
  priority : uint16
  weight   : uint16
  port     : uint16
  target   : domain_name } with
  member o.pickle = lazy (seq {
    yield! o.priority |> BitConverter.GetBytes |> Array.rev
    yield! o.weight   |> BitConverter.GetBytes |> Array.rev
    yield! o.port     |> BitConverter.GetBytes |> Array.rev
    yield! !!o.target.pickle } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    { priority = readUInt16 stream
      weight   = readUInt16 stream
      port     = readUInt16 stream
      target   = domain_name.unpickle stream }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream()
    SRV.unpickle ms

(* [RFC2930] 2. The TKEY Resource Record

   The TKEY resource record (RR) has the structure given below.  Its RR
   type code is 249.

      Field       Type         Comment
      -----       ----         -------

      NAME         domain      see description below
      TTYPE        u_int16_t   TKEY = 249
      CLASS        u_int16_t   ignored, SHOULD be 255 (ANY)
      TTL          u_int32_t   ignored, SHOULD be zero
      RDLEN        u_int16_t   size of RDATA
      RDATA:
       Algorithm:   domain
       Inception:   u_int32_t
       Expiration:  u_int32_t
       Mode:        u_int16_t
       Error:       u_int16_t
       Key Size:    u_int16_t
       Key Data:    octet-stream
       Other Size:  u_int16_t
       Other Data:  octet-stream  undefined by this specification

where: *)
/// [RFC2930] 2. the TKEY Resource Record
/// this needs an overhauling....
type TKEY = {
  name       : domain_name
  ttype      : uint16
  clss       : uint16
  ttl        : uint32
  rdlen      : uint16
  algorithm  : domain_name
  inception  : uint32
  expiration : uint32
  mode       : uint16
  error      : uint16
  key_size   : uint16
  key_data   : byte []
  other_size : uint16
  other_data : byte [] } with
  member o.pickle = lazy (seq {
    yield! !!o.name.pickle
    yield! o.ttype      |> BitConverter.GetBytes |> Array.rev
    yield! o.clss       |> BitConverter.GetBytes |> Array.rev
    yield! o.ttl        |> BitConverter.GetBytes |> Array.rev
    yield! o.rdlen      |> BitConverter.GetBytes |> Array.rev
    yield! !!o.algorithm.pickle
    yield! o.inception  |> BitConverter.GetBytes |> Array.rev
    yield! o.expiration |> BitConverter.GetBytes |> Array.rev
    yield! o.mode       |> BitConverter.GetBytes |> Array.rev
    yield! o.error      |> BitConverter.GetBytes |> Array.rev
    yield! o.key_size   |> BitConverter.GetBytes |> Array.rev
    yield! o.key_data
    yield! o.other_size |> BitConverter.GetBytes |> Array.rev
    yield! o.other_data } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    let name       = domain_name.unpickle stream
    let ttype      = readUInt16 stream
    let clss       = readUInt16 stream
    let ttl        = readUInt32 stream
    let rdlen      = readUInt16 stream
    let algorithm  = domain_name.unpickle stream
    let inception  = readUInt32 stream
    let expiration = readUInt32 stream
    let mode       = readUInt16 stream
    let error      = readUInt16 stream
    let key_size   = readUInt16 stream
    let key_data   = readBytes stream (int64 key_size)
    let other_size = readUInt16 stream
    let other_data = readBytes stream (int64 other_size)
    { name       = name
      ttype      = ttype
      clss       = clss
      ttl        = ttl
      rdlen      = rdlen
      algorithm  = algorithm
      inception  = inception
      expiration = expiration
      mode       = mode
      error      = error
      key_size   = key_size
      key_data   = key_data
      other_size = other_size
      other_data = other_data }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream()
    TKEY.unpickle ms

type TSIG = {
  algorithm   : domain_name
  time_signed : uint64
  fudge       : uint16
  mac_size    : uint16
  mac         : byte []
  original_id : uint16
  error       : uint16
  other_len   : uint16
  other_data  : byte [] } with
  member o.pickle = lazy (seq {
    yield! !!o.algorithm.pickle
    yield! o.time_signed |> BitConverter.GetBytes |> Array.skip 2 |> Array.rev
    yield! o.fudge       |> BitConverter.GetBytes |> Array.rev
    yield! o.mac_size    |> BitConverter.GetBytes |> Array.rev
    yield! o.mac
    yield! o.original_id |> BitConverter.GetBytes |> Array.rev
    yield! o.error       |> BitConverter.GetBytes |> Array.rev
    yield! o.other_len   |> BitConverter.GetBytes |> Array.rev
    yield! o.other_data } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    let algorithm   = domain_name.unpickle stream
    let time_signed = BitConverter.ToUInt64(readBytes stream 6L,0)
    let fudge       = readUInt16 stream
    let mac_size    = readUInt16 stream
    let mac         = readBytes stream (int64 mac_size)
    let original_id = readUInt16 stream
    let error       = readUInt16 stream
    let other_len   = readUInt16 stream
    let other_data  = readBytes stream (int64 other_len)
    { algorithm   = algorithm
      time_signed = time_signed
      fudge       = fudge
      mac_size    = mac_size
      mac         = mac
      original_id = original_id
      error       = error
      other_len   = other_len
      other_data  = other_data }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream()
    TSIG.unpickle ms

(* 3.3.14. TXT RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   TXT-DATA                    /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// TXT RRs are used to hold descriptive text.  The semantics of the text depends on the domain where it is found.
type TXT = {
/// TXT-DATA: One or more <character-string>s.
  txt : character_string } with
  member o.pickle = lazy (seq {
    yield! !!o.txt.pickle } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    { txt = character_string.unpickle stream }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream()
    TXT.unpickle ms

(* 3.4.2. WKS RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    ADDRESS                    |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |       PROTOCOL        |                       |
    +--+--+--+--+--+--+--+--+                       |
    |                                               |
    /                   <BIT MAP>                   /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where: *)
/// [RFCT1035] 3.4.2. WKS RDATA format. The WKS record is used to describe the well known services supported by a particular protocol on a particular internet address. The purpose of WKS RRs is to provide availability information for servers for TCP and UDP.  If a server supports both TCP and UDP, or has multiple Internet addresses, then multiple WKS RRs are used. WKS RRs cause no additional section processing.
type WKS = {
/// ADDRESS: An 32 bit Internet address
  address  : IPAddress
/// PROTOCOL: specifies An 8 bit IP protocol number, and the bit map has one bit per port of the specified protocol.  The first bit corresponds to port 0, the second to port 1, etc.  If the bit map does not include a bit for a protocol of interest, that bit is assumed zero.  The appropriate values and mnemonics for ports and protocols are specified in [RFC-1010]. For example, if PROTOCOL=TCP (6), the 26th bit corresponds to TCP port 25 (SMTP).  If this bit is set, a SMTP server should be listening on TCP port 25; if zero, SMTP service is not supported on the specified address.
  protocol : byte
/// <BIT MAP>: A variable length bit map.  The bit map must be a multiple of 8 bits long.
  bit_map  : byte [] } with
  member o.pickle = seq {
    yield! o.address.GetAddressBytes() |> Array.rev
    yield  o.protocol
    yield! o.bit_map } |> Array.ofSeq
  static member unpickle (stream : Stream) =
    rewind stream 2L
    let length = (readUInt16 stream |> int64) - 5L
    { address  = IPAddress(readBytes stream 4L)
      protocol = readByte stream
      bit_map  = readBytes stream length }
  static member unpickle (bytes : byte []) =
    use ms = new MemoryStream()
    WKS.unpickle ms

/// [RFC1183] 3.1. The X25 RR
type X25 = {
/// <PSDN-address>: identifies the PSDN (Public Switched Data Network) address in the X.121 [10] numbering plan associated with <owner>.
  psdn_address : character_string } with
  member o.pickle = lazy (seq {
    yield! !!o.psdn_address.pickle } |> Array.ofSeq)
  static member unpickle (stream : Stream) =
    { psdn_address = character_string.unpickle stream }
  static member unpickle (bytes : byte []) =
    { psdn_address = character_string.unpickle bytes }

type RData =
  | A     of A
  | AAAA  of AAAA
  | AFSDB of AFSDB
  | CNAME of CNAME
  | DNAME of DNAME
  | DS    of DS
  | KEY   of KEY
  | KX    of KX
  | LOC   of LOC
  | MB    of MB
  | MD    of MD
  | MF    of MF
  | MG    of MG
  | MINFO of MINFO
  | MR    of MR
  | MX    of MX
  | NAPTR of NAPTR
  | NS    of NS
  | NSAP  of NSAP
  | NULL  of NULL
  | PTR   of PTR
  | PX    of PX
  | RP    of RP
  | RT    of RT
  | SIG   of SIG
  | SOA   of SOA
  | SRV   of SRV
  | TKEY  of TKEY
  | TSIG  of TSIG
  | TXT   of TXT
  | WKS   of WKS
  | X25   of X25
  | Unknown of int * byte [] with
  member o.pickle = 
    match o with
    | A     x -> !!x.pickle
    | AAAA  x -> !!x.pickle
    | AFSDB x -> !!x.pickle
    | CNAME x -> !!x.pickle
    | DNAME x -> !!x.pickle
    | DS    x -> !!x.pickle
    | KEY   x -> !!x.pickle
    | KX    x -> !!x.pickle
    | LOC   x -> !!x.pickle
    | MB    x -> !!x.pickle
    | MD    x -> !!x.pickle
    | MF    x -> !!x.pickle
    | MG    x -> !!x.pickle
    | MINFO x -> !!x.pickle
    | MR    x -> !!x.pickle
    | MX    x -> !!x.pickle
    | NAPTR x -> !!x.pickle
    | NS    x -> !!x.pickle
    | NSAP  x -> !!x.pickle
    | NULL  x -> !!x.pickle
    | PTR   x -> !!x.pickle
    | PX    x -> !!x.pickle
    | RP    x -> !!x.pickle
    | RT    x -> !!x.pickle
    | SIG   x -> !!x.pickle
    | SOA   x -> !!x.pickle
    | SRV   x -> !!x.pickle
    | TKEY  x -> !!x.pickle
    | _       -> failwith "not yet implemented or unknown" 
  static member unpickle (stream : Stream) = function
    | RRType.A     -> A.unpickle     stream |> RData.A
    | RRType.AAAA  -> AAAA.unpickle  stream |> RData.AAAA
    | RRType.AFSDB -> AFSDB.unpickle stream |> RData.AFSDB
    | RRType.CNAME -> CNAME.unpickle stream |> RData.CNAME
    | RRType.DNAME -> DNAME.unpickle stream |> RData.DNAME
    | RRType.DS    -> DS.unpickle    stream |> RData.DS
    | RRType.KEY   -> KEY.unpickle   stream |> RData.KEY
    | RRType.KX    -> KX.unpickle    stream |> RData.KX
    | RRType.LOC   -> LOC.unpickle   stream |> RData.LOC
    | RRType.MB    -> MB.unpickle    stream |> RData.MB
    | RRType.MD    -> MD.unpickle    stream |> RData.MD
    | RRType.MF    -> MF.unpickle    stream |> RData.MF
    | RRType.MG    -> MG.unpickle    stream |> RData.MG
    | RRType.MINFO -> MINFO.unpickle stream |> RData.MINFO
    | RRType.MR    -> MR.unpickle    stream |> RData.MR
    | RRType.MX    -> MX.unpickle    stream |> RData.MX
    | RRType.NAPTR -> NAPTR.unpickle stream |> RData.NAPTR
    | RRType.NS    -> NS.unpickle    stream |> RData.NS
    | RRType.NSAP  -> NSAP.unpickle  stream |> RData.NSAP
    | RRType.NULL  -> NULL.unpickle  stream |> RData.NULL
    | RRType.PTR   -> PTR.unpickle   stream |> RData.PTR
    | RRType.PX    -> PX.unpickle    stream |> RData.PX
    | RRType.RP    -> RP.unpickle    stream |> RData.RP
    | RRType.RT    -> RT.unpickle    stream |> RData.RT
    | RRType.SIG   -> SIG.unpickle   stream |> RData.SIG
    | RRType.SOA   -> SOA.unpickle   stream |> RData.SOA
    | RRType.SRV   -> SRV.unpickle   stream |> RData.SRV
    | RRType.TKEY  -> TKEY.unpickle  stream |> RData.TKEY
    | e            -> printfn "%A at %A" e (stream.Position); Enum.unexpected e
  static member unpckle (rdata : RData) (bytes : byte []) =
    use ms = new MemoryStream()
    RData.unpickle ms
