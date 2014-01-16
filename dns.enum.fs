namespace thinkhard.Protocol.Dns

open thinkhard


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module Help =
  let Description (t : 'a) : DescriptionAttribute = Attribute.pick t
  let Name        (t : 'a) : NameAttribute        = Attribute.pick t

/// 3.2.2. TYPE values
///
/// TYPE fields are used in resource records.
/// Note that these types are a subset of QTYPEs.
///        Unassigned   32770-65279  
///        Private use  65280-65534
/// Note: In [RFC1002], two types are defined.  It is not clear that these
/// are in use, though if so their assignment is in conflict.
///        NB        32    NetBIOS general Name Service
///        NBSTAT    33    NetBIOS NODE STATUS
type RRType =
  /// an IPV4 host address [RFC1035]
  | [<Description("an IPV4 host address [RFC1035]")>]
    [<Name("A")>]
    A = 1us
  /// an authoritative name server [RFC1035]
  | [<Description("an authoritative name server [RFC1035]")>]
    [<Name("NS")>] 
    NS = 2us
  /// a mail destination (Obsolete - use MX) [RFC1035]
  | [<Description("a mail destination (Obsolete - use MX) [RFC1035]")>]
    [<Name("MD")>] 
    MD = 3us
  /// a mail forwarder (Obsolete - use MX) [RFC1035]
  | [<Description("a mail forwarder (Obsolete - use MX) [RFC1035]")>]
    [<Name("MF")>] 
    MF = 4us
  /// the canonical name for an alias [RFC1035]
  | [<Description("the canonical name for an alias [RFC1035]")>]
    [<Name("CNAME")>] 
    CNAME = 5us
  /// marks the start of a zone of authority [RFC1035]
  | [<Description("marks the start of a zone of authority [RFC1035]")>]
    [<Name("SOA")>] 
    SOA = 6us
  /// a mailbox domain name (EXPERIMENTAL) [RFC1035]
  | [<Description("a mailbox domain name (EXPERIMENTAL) [RFC1035]")>]
    [<Name("MB")>] 
    MB = 7us
  /// a mail group member (EXPERIMENTAL) [RFC1035]
  | [<Description("a mail group member (EXPERIMENTAL) [RFC1035]")>]
    [<Name("MG")>] 
    MG = 8us
  /// a mail rename domain name (EXPERIMENTAL) [RFC1035]
  | [<Description("a mail rename domain name (EXPERIMENTAL) [RFC1035]")>]
    [<Name("MR")>] 
    MR = 9us
  /// a null RR (EXPERIMENTAL) [RFC1035]
  | [<Description("a null RR (EXPERIMENTAL) [RFC1035]")>]
    [<Name("NULL")>] 
    NULL = 10us
  /// a well known service description [RFC1035]
  | [<Description("a well known service description [RFC1035]")>]
    [<Name("WKS")>] 
    WKS = 11us
  /// a domain name pointer [RFC1035]
  | [<Description("a domain name pointer [RFC1035]")>]
    [<Name("PTR")>] 
    PTR = 12us
  /// host information [RFC1035]
  | [<Description("host information [RFC1035]")>]
    [<Name("HINFO")>] 
    HINFO = 13us
  /// mailbox or mail list information [RFC1035]
  | [<Description("mailbox or mail list information [RFC1035]")>]
    [<Name("MINFO")>] 
    MINFO = 14us
  /// mail exchange [RFC1035]
  | [<Description("mail exchange [RFC1035]")>]
    [<Name("MX")>] 
    MX = 15us
  /// text strings [RFC1035]
  | [<Description("text strings [RFC1035]")>]
    [<Name("TXT")>] 
    TXT = 16us
  /// for Responsible Person 
  | [<Description("for Responsible Person")>]
    [<Name("RP")>] 
    RP = 17us
  /// for AFS Data Base location [RFC1183][RFC5864]
  | [<Description("for AFS Data Base location [RFC1183][RFC5864]")>]
    [<Name("AFSDB")>] 
    AFSDB = 18us
  /// for X.25 address [RFC1183]
  | [<Description("for X.25 address [RFC1183]")>]
    [<Name("X25")>] 
    X25 = 19us
  /// for ISDN address [RFC1183]
  | [<Description("for ISDN address [RFC1183]")>]
    [<Name("ISDN")>] 
    ISDN = 20us
  /// for Route Through [RFC1183]
  | [<Description("for Route Through [RFC1183]")>]
    [<Name("RT")>] 
    RT = 21us
  /// Network service access point address (NSAP style A record) [RFC1706]
  | [<Description("Network service access point address (NSAP style A record) [RFC1706]")>]
    [<Name("NSAP")>] 
    NSAP = 22us
  /// for domain name pointer, NSAP style [RFC1348]
  | [<Description("for domain name pointer, NSAP style [RFC1348]")>]
    [<Name("NSAP-PTR")>] 
    NSAP_PTR = 23us
  /// Cryptographic public key signature [RFC4034][RFC3755][RFC2931][RFC2535]
  | [<Description("Cryptographic public key signature [RFC4034][RFC3755][RFC2931][RFC2535]")>]
    [<Name("SIG")>] 
    SIG = 24us
  /// Public key as used in DNSSEC [RFC4034][RFC3755][RFC2535]
  | [<Description("Public key as used in DNSSEC [RFC4034][RFC3755][RFC2535]")>]
    [<Name("KEY")>] 
    KEY = 25us
  /// Pointer to X.400 mail mapping information [RFC2163]
  | [<Description("Pointer to X.400 mail mapping information [RFC2163]")>]
    [<Name("PX")>] 
    PX = 26us
  /// Geographical position [RFC1712] (Obsolete)
  | [<Description("Geographical position [RFC1712] (Obsolete)")>]
    [<Name("GPOS")>] 
    GPOS = 27us
  /// an IPV6 host address [RFC3596]
  | [<Description("an IPV6 host address [RFC3596]")>]
    [<Name("AAAA")>] 
    AAAA = 28us
  /// Location information [RFC1876]
  | [<Description("Location information [RFC1876]")>]
    [<Name("LOC")>] 
    LOC = 29us
  /// Next Domain [RFC2065][RC2535] (Obsolete)
  | [<Description("Next Domain [RFC2065][RC2535] (Obsolete)")>]
    [<Name("NXT")>] 
    NXT = 30us
  /// Endpoint Identifier [Patton]
  | [<Description("Endpoint Identifier [Patton]")>]
    [<Name("EID")>] 
    EID = 31us
  /// Nimrod Locator [Patton]
  | [<Description("Nimrod Locator [Patton]")>]
    [<Name("NIMLOC")>] 
    NIMLOC = 32us
  /// Server Selection [RFC2782]
  | [<Description("Server Selection [RFC2782]")>]
    [<Name("SRV")>] 
    SRV = 33us
  /// ATM Address [ATMDOC]
  | [<Description("ATM Address [ATMDOC]")>]
    [<Name("ATMA")>] 
    ATMA = 34us
  /// Naming Authority Pointer [RFC2915][RFC2168][RFC3403]
  | [<Description("Naming Authority Pointer [RFC2915][RFC2168][RFC3403]")>]
    [<Name("NAPTR")>] 
    NAPTR = 35us
  /// Key Exchanger [RFC2230]
  | [<Description("Key Exchanger [RFC2230]")>]
    [<Name("KX")>] 
    KX = 36us
  /// CERT [RFC2538]
  | [<Description("CERT [RFC2538]")>]
    [<Name("CERT")>] 
    CERT = 37us
  /// IPv6 address [RFC3363][RFC2874][RFC3226] (Experimental)
  | [<Description("IPv6 address [RFC3363][RFC2874][RFC3226] (Experimental)")>]
    [<Name("A6")>] 
    A6 = 38us
  /// A way to provide aliases for a whole domain, not just a single domain name as with CNAME. [RFC2672]
  | [<Description("A way to provide aliases for a whole domain, not just a single domain name as with CNAME. [RFC2672]")>]
    [<Name("DNAME")>] 
    DNAME = 39us
  /// SINK [Eastlake]
  | [<Description("SINK [Eastlake]")>]
    [<Name("SINK")>] 
    SINK = 40us
  /// OPT [RFC2671]
  | [<Description("OPT [RFC2671]")>]
    [<Name("OPT")>] 
    OPT = 41us
  /// APL [RFC3123]
  | [<Description("APL [RFC3123]")>]
    [<Name("APL")>] 
    APL = 42us
  /// Delegation Signer [RFC3658]
  | [<Description("Delegation Signer [RFC3658]")>]
    [<Name("DS")>] 
    DS = 43us
  /// SSH Key Fingerprint [RFC4255]
  | [<Description("SSH Key Fingerprint [RFC4255]")>]
    [<Name("SSHFP")>] 
    SSHFP = 44us
  /// IPSECKEY [RFC4025]
  | [<Description("IPSECKEY [RFC4025]")>]
    [<Name("IPSECKEY")>] 
    IPSECKEY = 45us
  /// RRSIG [RFC4034][RFC3755]
  | [<Description("RRSIG [RFC4034][RFC3755]")>]
    [<Name("RRSIG")>] 
    RRSIG = 46us
  /// NSEC [RFC4034][RFC3755]
  | [<Description("NSEC [RFC4034][RFC3755]")>]
    [<Name("NSEC")>] 
    NSEC = 47us
  /// DNSKEY [RFC4034][RFC3755]
  | [<Description("DNSKEY [RFC4034][RFC3755]")>]
    [<Name("DNSKEY")>] 
    DNSKEY = 48us
  /// DHCID [RFC4701]
  | [<Description("DHCID [RFC4701]")>]
    [<Name("DHCID")>] 
    DHCID = 49us
  /// NSEC3 [RFC5155]
  | [<Description("NSEC3 [RFC5155]")>]
    [<Name("NSEC3")>] 
    NSEC3 = 50us
  /// NSEC3PARAM [RFC5155]
  | [<Description("NSEC3PARAM [RFC5155]")>]
    [<Name("NSEC3PARAM")>] 
    NSEC3PARAM = 51us
  /// Host Identity Protocol  [ietf-hip-dns-09.txt][RFC5205]
  | [<Description("Host Identity Protocol  [ietf-hip-dns-09.txt][RFC5205]")>]
    [<Name("HIP")>] 
    HIP = 55us
  /// NINFO [Reid]
  | [<Description("NINFO [Reid]")>]
    [<Name("NINFO")>] 
    NINFO = 56us
  /// RKEY [Reid]
  | [<Description("RKEY [Reid]")>]
    [<Name("RKEY")>] 
    RKEY = 57us
  /// Trust Anchor LINK [Wijngaards]
  | [<Description("Trust Anchor LINK [Wijngaards]")>]
    [<Name("TALINK")>] 
    TALINK = 58us
  /// Child DS [Barwood]
  | [<Description("Child DS [Barwood]")>]
    [<Name("CDS")>] 
    CDS = 59us
  /// SPF [RFC4408]
  | [<Description("SPF [RFC4408]")>]
    [<Name("SPF")>] 
    SPF = 99us
  /// IANA-Reserved
  | [<Description("IANA-Reserved")>]
    [<Name("UINFO")>] 
    UINFO = 100us
  /// IANA-Reserved
  | [<Description("IANA-Reserved")>]
    [<Name("UID")>] 
    UID = 101us
  /// IANA-Reserved
  | [<Description("IANA-Reserved")>]
    [<Name("GID")>] 
    GID = 102us
  /// IANA-Reserved
  | [<Description("IANA-Reserved")>]
    [<Name("UNSPEC")>] 
    UNSPEC = 103us
  /// Transaction key [RFC2930]
  | [<Description("Transaction key [RFC2930]")>]
    [<Name("TKEY")>] 
    TKEY = 249us
  /// Transaction signature [RFC2845]
  | [<Description("Transaction signature [RFC2845]")>]
    [<Name("TSIG")>] 
    TSIG = 250us
  /// incremental transfer [RFC1995]
  | [<Description("incremental transfer [RFC1995]")>]
    [<Name("IXFR")>] 
    IXFR = 251us
  /// transfer of an entire zone [RFC1035][RFC5936]
  | [<Description("transfer of an entire zone [RFC1035][RFC5936]")>]
    [<Name("AXFR")>] 
    AXFR = 252us
  /// mailbox-related RRs (MB, MG or MR) [RFC1035]
  | [<Description("mailbox-related RRs (MB, MG or MR) [RFC1035]")>]
    [<Name("MAILB")>] 
    MAILB = 232us
  /// mail agent RRs (Obsolete - see MX) [RFC1035]
  | [<Description("mail agent RRs (Obsolete - see MX) [RFC1035]")>]
    [<Name("MAILA")>] 
    MAILA = 254us
  /// * - a request for all records [RFC1035]
  | [<Description("* - a request for all records [RFC1035]")>]
    [<Name("ANY")>] 
    ANY = 255us
  /// URI [Faltstrom]
  | [<Description("URI [Faltstrom]")>]
    [<Name("URI")>] 
    URI = 256us
  /// Certification Authority Authorization [Hallam-Baker]
  | [<Description("Certification Authority Authorization [Hallam-Baker]")>]
    [<Name("CAA")>] 
    CAA = 257us
  /// DNSSEC Trust Authorities [Weiler]
  | [<Description("DNSSEC Trust Authorities [Weiler]")>]
    [<Name("TA")>] 
    TA = 32768us
  /// DNSSEC Lookaside Validation [RFC4431]
  | [<Description("DNSSEC Lookaside Validation [RFC4431]")>]
    [<Name("DLV")>] 
    DLV = 32769us
  /// Reserved
  | [<Description("Reserved")>]
    [<Name("Reserved")>] 
    Reserved = 65535us

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module RR = 
  let t = typeof<RRType>
  let Description : DescriptionAttribute = Attribute.pick t
  let Name        : NameAttribute        = Attribute.pick t

/// 3.2.3. QTYPE values
///
/// QTYPE fields appear in the question part of a query.  QTYPES are a
/// superset of TYPEs, hence all TYPEs are valid QTYPEs.  In addition, the
/// following QTYPEs are defined: IFXR, AXFR, MAILB, MAILA, ANY
type QType =
  /// an IPV4 host address [RFC1035]
  | [<Description("an IPV4 host address [RFC1035]")>]
    [<Name("A")>]
    A = 1us
  /// an authoritative name server [RFC1035]
  | [<Description("an authoritative name server [RFC1035]")>]
    [<Name("NS")>] 
    NS = 2us
  /// a mail destination (Obsolete - use MX) [RFC1035]
  | [<Description("a mail destination (Obsolete - use MX) [RFC1035]")>]
    [<Name("MD")>] 
    MD = 3us
  /// a mail forwarder (Obsolete - use MX) [RFC1035]
  | [<Description("a mail forwarder (Obsolete - use MX) [RFC1035]")>]
    [<Name("MF")>] 
    MF = 4us
  /// the canonical name for an alias [RFC1035]
  | [<Description("the canonical name for an alias [RFC1035]")>]
    [<Name("CNAME")>] 
    CNAME = 5us
  /// marks the start of a zone of authority [RFC1035]
  | [<Description("marks the start of a zone of authority [RFC1035]")>]
    [<Name("SOA")>] 
    SOA = 6us
  /// a mailbox domain name (EXPERIMENTAL) [RFC1035]
  | [<Description("a mailbox domain name (EXPERIMENTAL) [RFC1035]")>]
    [<Name("MB")>] 
    MB = 7us
  /// a mail group member (EXPERIMENTAL) [RFC1035]
  | [<Description("a mail group member (EXPERIMENTAL) [RFC1035]")>]
    [<Name("MG")>] 
    MG = 8us
  /// a mail rename domain name (EXPERIMENTAL) [RFC1035]
  | [<Description("a mail rename domain name (EXPERIMENTAL) [RFC1035]")>]
    [<Name("MR")>] 
    MR = 9us
  /// a null RR (EXPERIMENTAL) [RFC1035]
  | [<Description("a null RR (EXPERIMENTAL) [RFC1035]")>]
    [<Name("NULL")>] 
    NULL = 10us
  /// a well known service description [RFC1035]
  | [<Description("a well known service description [RFC1035]")>]
    [<Name("WKS")>] 
    WKS = 11us
  /// a domain name pointer [RFC1035]
  | [<Description("a domain name pointer [RFC1035]")>]
    [<Name("PTR")>] 
    PTR = 12us
  /// host information [RFC1035]
  | [<Description("host information [RFC1035]")>]
    [<Name("HINFO")>] 
    HINFO = 13us
  /// mailbox or mail list information [RFC1035]
  | [<Description("mailbox or mail list information [RFC1035]")>]
    [<Name("MINFO")>] 
    MINFO = 14us
  /// mail exchange [RFC1035]
  | [<Description("mail exchange [RFC1035]")>]
    [<Name("MX")>] 
    MX = 15us
  /// text strings [RFC1035]
  | [<Description("text strings [RFC1035]")>]
    [<Name("TXT")>] 
    TXT = 16us
  /// for Responsible Person 
  | [<Description("for Responsible Person")>]
    [<Name("RP")>] 
    RP = 17us
  /// for AFS Data Base location [RFC1183][RFC5864]
  | [<Description("for AFS Data Base location [RFC1183][RFC5864]")>]
    [<Name("AFSDB")>] 
    AFSDB = 18us
  /// for X.25 address [RFC1183]
  | [<Description("for X.25 address [RFC1183]")>]
    [<Name("X25")>] 
    X25 = 19us
  /// for ISDN address [RFC1183]
  | [<Description("for ISDN address [RFC1183]")>]
    [<Name("ISDN")>] 
    ISDN = 20us
  /// for Route Through [RFC1183]
  | [<Description("for Route Through [RFC1183]")>]
    [<Name("RT")>] 
    RT = 21us
  /// Network service access point address (NSAP style A record) [RFC1706]
  | [<Description("Network service access point address (NSAP style A record) [RFC1706]")>]
    [<Name("NSAP")>] 
    NSAP = 22us
  /// for domain name pointer, NSAP style [RFC1348]
  | [<Description("for domain name pointer, NSAP style [RFC1348]")>]
    [<Name("NSAP-PTR")>] 
    NSAP_PTR = 23us
  /// Cryptographic public key signature [RFC4034][RFC3755][RFC2931][RFC2535]
  | [<Description("Cryptographic public key signature [RFC4034][RFC3755][RFC2931][RFC2535]")>]
    [<Name("SIG")>] 
    SIG = 24us
  /// Public key as used in DNSSEC [RFC4034][RFC3755][RFC2535]
  | [<Description("Public key as used in DNSSEC [RFC4034][RFC3755][RFC2535]")>]
    [<Name("KEY")>] 
    KEY = 25us
  /// Pointer to X.400 mail mapping information [RFC2163]
  | [<Description("Pointer to X.400 mail mapping information [RFC2163]")>]
    [<Name("PX")>] 
    PX = 26us
  /// Geographical position [RFC1712] (Obsolete)
  | [<Description("Geographical position [RFC1712] (Obsolete)")>]
    [<Name("GPOS")>] 
    GPOS = 27us
  /// an IPV6 host address [RFC3596]
  | [<Description("an IPV6 host address [RFC3596]")>]
    [<Name("AAAA")>] 
    AAAA = 28us
  /// Location information [RFC1876]
  | [<Description("Location information [RFC1876]")>]
    [<Name("LOC")>] 
    LOC = 29us
  /// Next Domain [RFC2065][RC2535] (Obsolete)
  | [<Description("Next Domain [RFC2065][RC2535] (Obsolete)")>]
    [<Name("NXT")>] 
    NXT = 30us
  /// Endpoint Identifier [Patton]
  | [<Description("Endpoint Identifier [Patton]")>]
    [<Name("EID")>] 
    EID = 31us
  /// Nimrod Locator [Patton]
  | [<Description("Nimrod Locator [Patton]")>]
    [<Name("NIMLOC")>] 
    NIMLOC = 32us
  /// Server Selection [RFC2782]
  | [<Description("Server Selection [RFC2782]")>]
    [<Name("SRV")>] 
    SRV = 33us
  /// ATM Address [ATMDOC]
  | [<Description("ATM Address [ATMDOC]")>]
    [<Name("ATMA")>] 
    ATMA = 34us
  /// Naming Authority Pointer [RFC2915][RFC2168][RFC3403]
  | [<Description("Naming Authority Pointer [RFC2915][RFC2168][RFC3403]")>]
    [<Name("NAPTR")>] 
    NAPTR = 35us
  /// Key Exchanger [RFC2230]
  | [<Description("Key Exchanger [RFC2230]")>]
    [<Name("KX")>] 
    KX = 36us
  /// CERT [RFC2538]
  | [<Description("CERT [RFC2538]")>]
    [<Name("CERT")>] 
    CERT = 37us
  /// IPv6 address [RFC3363][RFC2874][RFC3226] (Experimental)
  | [<Description("IPv6 address [RFC3363][RFC2874][RFC3226] (Experimental)")>]
    [<Name("A6")>] 
    A6 = 38us
  /// A way to provide aliases for a whole domain, not just a single domain name as with CNAME. [RFC2672]
  | [<Description("A way to provide aliases for a whole domain, not just a single domain name as with CNAME. [RFC2672]")>]
    [<Name("DNAME")>] 
    DNAME = 39us
  /// SINK [Eastlake]
  | [<Description("SINK [Eastlake]")>]
    [<Name("SINK")>] 
    SINK = 40us
  /// OPT [RFC2671]
  | [<Description("OPT [RFC2671]")>]
    [<Name("OPT")>] 
    OPT = 41us
  /// APL [RFC3123]
  | [<Description("APL [RFC3123]")>]
    [<Name("APL")>] 
    APL = 42us
  /// Delegation Signer [RFC3658]
  | [<Description("Delegation Signer [RFC3658]")>]
    [<Name("DS")>] 
    DS = 43us
  /// SSH Key Fingerprint [RFC4255]
  | [<Description("SSH Key Fingerprint [RFC4255]")>]
    [<Name("SSHFP")>] 
    SSHFP = 44us
  /// IPSECKEY [RFC4025]
  | [<Description("IPSECKEY [RFC4025]")>]
    [<Name("IPSECKEY")>] 
    IPSECKEY = 45us
  /// RRSIG [RFC4034][RFC3755]
  | [<Description("RRSIG [RFC4034][RFC3755]")>]
    [<Name("RRSIG")>] 
    RRSIG = 46us
  /// NSEC [RFC4034][RFC3755]
  | [<Description("NSEC [RFC4034][RFC3755]")>]
    [<Name("NSEC")>] 
    NSEC = 47us
  /// DNSKEY [RFC4034][RFC3755]
  | [<Description("DNSKEY [RFC4034][RFC3755]")>]
    [<Name("DNSKEY")>] 
    DNSKEY = 48us
  /// DHCID [RFC4701]
  | [<Description("DHCID [RFC4701]")>]
    [<Name("DHCID")>] 
    DHCID = 49us
  /// NSEC3 [RFC5155]
  | [<Description("NSEC3 [RFC5155]")>]
    [<Name("NSEC3")>] 
    NSEC3 = 50us
  /// NSEC3PARAM [RFC5155]
  | [<Description("NSEC3PARAM [RFC5155]")>]
    [<Name("NSEC3PARAM")>] 
    NSEC3PARAM = 51us
  /// Host Identity Protocol  [ietf-hip-dns-09.txt][RFC5205]
  | [<Description("Host Identity Protocol  [ietf-hip-dns-09.txt][RFC5205]")>]
    [<Name("HIP")>] 
    HIP = 55us
  /// NINFO [Reid]
  | [<Description("NINFO [Reid]")>]
    [<Name("NINFO")>] 
    NINFO = 56us
  /// RKEY [Reid]
  | [<Description("RKEY [Reid]")>]
    [<Name("RKEY")>] 
    RKEY = 57us
  /// Trust Anchor LINK [Wijngaards]
  | [<Description("Trust Anchor LINK [Wijngaards]")>]
    [<Name("TALINK")>] 
    TALINK = 58us
  /// Child DS [Barwood]
  | [<Description("Child DS [Barwood]")>]
    [<Name("CDS")>] 
    CDS = 59us
  /// SPF [RFC4408]
  | [<Description("SPF [RFC4408]")>]
    [<Name("SPF")>] 
    SPF = 99us
  /// IANA-Reserved
  | [<Description("IANA-Reserved")>]
    [<Name("UINFO")>] 
    UINFO = 100us
  /// IANA-Reserved
  | [<Description("IANA-Reserved")>]
    [<Name("UID")>] 
    UID = 101us
  /// IANA-Reserved
  | [<Description("IANA-Reserved")>]
    [<Name("GID")>] 
    GID = 102us
  /// IANA-Reserved
  | [<Description("IANA-Reserved")>]
    [<Name("UNSPEC")>] 
    UNSPEC = 103us
  /// Transaction key [RFC2930]
  | [<Description("Transaction key [RFC2930]")>]
    [<Name("TKEY")>] 
    TKEY = 249us
  /// Transaction signature [RFC2845]
  | [<Description("Transaction signature [RFC2845]")>]
    [<Name("TSIG")>] 
    TSIG = 250us
  /// incremental transfer [RFC1995]
  | [<Description("incremental transfer [RFC1995]")>]
    [<Name("IXFR")>] 
    IXFR = 251us
  /// transfer of an entire zone [RFC1035][RFC5936]
  | [<Description("transfer of an entire zone [RFC1035][RFC5936]")>]
    [<Name("AXFR")>] 
    AXFR = 252us
  /// mailbox-related RRs (MB, MG or MR) [RFC1035]
  | [<Description("mailbox-related RRs (MB, MG or MR) [RFC1035]")>]
    [<Name("MAILB")>] 
    MAILB = 232us
  /// mail agent RRs (Obsolete - see MX) [RFC1035]
  | [<Description("mail agent RRs (Obsolete - see MX) [RFC1035]")>]
    [<Name("MAILA")>] 
    MAILA = 254us
  /// * - a request for all records [RFC1035]
  | [<Description("* - a request for all records [RFC1035]")>]
    [<Name("ANY")>] 
    ANY = 255us
  /// URI [Faltstrom]
  | [<Description("URI [Faltstrom]")>]
    [<Name("URI")>] 
    URI = 256us
  /// Certification Authority Authorization [Hallam-Baker]
  | [<Description("Certification Authority Authorization [Hallam-Baker]")>]
    [<Name("CAA")>] 
    CAA = 257us
  /// DNSSEC Trust Authorities [Weiler]
  | [<Description("DNSSEC Trust Authorities [Weiler]")>]
    [<Name("TA")>] 
    TA = 32768us
  /// DNSSEC Lookaside Validation [RFC4431]
  | [<Description("DNSSEC Lookaside Validation [RFC4431]")>]
    [<Name("DLV")>] 
    DLV = 32769us
  /// Reserved
  | [<Description("Reserved")>]
    [<Name("Reserved")>] 
    Reserved = 65535us
 
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module QType = begin
  end 

/// 3.2.4. CLASS values
///
/// CLASS fields appear in resource records.  The following CLASS mnemonics
/// and values are defined:
type RRClass =
  /// the Internet
  | [<Description("the Internet")>]
    [<Name("IN")>] 
    IN = 1us
  /// the CSNET class (Obsolete - used only for examples in some obsolete RFCs)
  | [<Description("the CSNET class (Obsolete - used only for examples in some obsolete RFCs)")>]
    [<Name("CS")>] 
    CS = 2us
  /// the CHAOS class
  | [<Description("the CHAOS class")>]
    [<Name("CH")>] 
    CH = 3us
  /// Hesiod [Dyer 87]
  | [<Description("Hesiod [Dyer 87]")>]
    [<Name("HS")>] 
    HS = 4us
    
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module RRClass = begin
  end

/// 3.2.5. QCLASS values
///
/// QCLASS fields appear in the question section of a query.  QCLASS values
/// are a superset of CLASS values; every CLASS is a valid QCLASS.  In
/// addition to CLASS values, the following QCLASSes are defined:
type QClass =
  /// the Internet
  | [<Description("the Internet")>]
    [<Name("IN")>] 
    IN = 1us
  /// the CSNET class (Obsolete - used only for examples in some obsolete RFCs)
  | [<Description("the CSNET class (Obsolete - used only for examples in some obsolete RFCs)")>]
    [<Name("CS")>] 
    CS = 2us
  /// the CHAOS class
  | [<Description("the CHAOS class")>]
    [<Name("CH")>] 
    CH = 3us
  /// Hesiod [Dyer 87]
  | [<Description("Hesiod [Dyer 87]")>]
    [<Name("HS")>] 
    HS = 4us
  // any class
  | [<Description("any class")>]
    [<Name("ANY")>] 
    ANY = 255us
 
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module QClass = begin
  end


/// Response code - this 4 bit field is set as part of responses.  
///          23-3840       available for assignment
///          3841-4095     Private Use
///          4096-65535    available for assignment
type RCode =
  /// No Error [RFC1035]
  | [<Description("No Error [RFC1035]")>]
    [<Name("NoError")>] 
    NoError = 0uy
  /// Format Error [RFC1035]
  | [<Description("Format Error [RFC1035]")>]
    [<Name("FormErr")>] 
    FormErr = 1uy
  /// Server Failure [RFC1035]
  | [<Description("Server Failure [RFC1035]")>]
    [<Name("ServFail")>] 
    ServFail = 2uy
  /// Non-Existent Domain [RFC1035]
  | [<Description("Non-Existent Domain [RFC1035]")>]
    [<Name("NXDomain")>] 
    NXDomain = 3uy
  /// Not Implemented [RFC1035]
  | [<Description("Not Implemented [RFC1035]")>]
    [<Name("NotImp")>] 
    NotImp = 4uy
  /// Query Refused [RFC1035]
  | [<Description("Query Refused [RFC1035]")>]
    [<Name("Refused")>] 
    Refused = 5uy
  /// Name Exists when it should not [RFC2136]
  | [<Description("Name Exists when it should not [RFC2136]")>]
    [<Name("YXDomain")>] 
    YXDomain = 6uy
  /// RR Set Exists when it should not [RFC2136]
  | [<Description("RR Set Exists when it should not [RFC2136]")>]
    [<Name("YXRRSet")>] 
    YXRRSet = 7uy
  /// RR Set that should exist does not [RFC2136]
  | [<Description("RR Set that should exist does not [RFC2136]")>]
    [<Name("NXRRSet")>] 
    NXRRSet = 8uy
  /// Server Not Authoritative for zone [RFC2136]
  | [<Description("Server Not Authoritative for zone [RFC2136]")>]
    [<Name("NotAuth")>] 
    NotAuth = 9uy
  /// Name not contained in zone [RFC2136]
  | [<Description("Name not contained in zone [RFC2136]")>]
    [<Name("NotZone")>] 
    NotZone = 10uy
  /// Reserved
  | [<Description("Reserved")>]
    [<Name("Reserved")>] 
    RESERVED11 = 11uy
  /// Reserved
  | [<Description("Reserved")>]
    [<Name("Reserved")>] 
    RESERVED12 = 12uy
  /// Reserved
  | [<Description("Reserved")>]
    [<Name("Reserved")>] 
    RESERVED13 = 13uy
  /// Reserved
  | [<Description("Reserved")>]
    [<Name("Reserved")>] 
    RESERVED14 = 14uy
  /// Reserved
  | [<Description("Reserved")>]
    [<Name("Reserved")>] 
    RESERVED15 = 15uy
  /// Bad OPT Version [RFC2671]
  | [<Description("Bad OPT Version [RFC2671]")>]
    [<Name("BADVERSSIG")>] 
    BADVERSSIG = 16uy

  (* WHAT THE HELL IS THIS SHIT?
  /// TSIG Signature Failure [RFC2845]
  /// Key not recognized [RFC2845]
  | [<Description("TSIG Signature Failure [RFC2845] Key not recognized [RFC2845]")>]
    [<Name("BADKEY")>] 
    BADKEY = 17us
  /// Signature out of time window [RFC2845]
  | [<Description("Signature out of time window [RFC2845]")>]
    [<Name("BADTIME")>] 
    BADTIME = 18us
  /// Bad TKEY Mode [RFC2930]
  | [<Description("Bad TKEY Mode [RFC2930]")>]
    [<Name("BADMODE")>] 
    BADMODE = 19us
  /// Duplicate key name [RFC2930]
  | [<Description("Duplicate key name [RFC2930]")>]
    [<Name("BADNAME")>] 
    BADNAME = 20us
  /// Algorithm not supported [RFC2930]
  | [<Description("Algorithm not supported [RFC2930]")>]
    [<Name("BADALG")>] 
    BADALG = 21us
  /// Bad Truncation [RFC4635]
  | [<Description("Bad Truncation [RFC4635]")>]
    [<Name("BADTRUNC")>] 
    BADTRUNC = 22us
  /// Reserved, can be allocated by Standards Action [RFC6195]
  | [<Description("Reserved, can be allocated by Standards Action [RFC6195]")>]
    [<Name("Reserved")>] 
    RESERVED = 65535us
  *)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module RCode = begin
  end

/// A four bit field that specifies kind of query in this
/// message.  This value is set by the originator of a query
/// and copied into the response.
type OPCode =
  | [<Description("")>]
    [<Name("")>] 
    Query = 0uy
  // OpCode Retired (previously IQUERY - No further [RFC3425]
  | [<Description("OpCode Retired (previously IQUERY - No further [RFC3425]")>]
    [<Name("")>] 
    IQUERY = 1uy
  // a server status request (STATUS) RFC1035
  | [<Description("a server status request (STATUS) RFC1035")>]
    [<Name("")>] 
    Status = 2uy
  // IANA
  | [<Description("IANA")>]
    [<Name("")>] 
    RESERVED3 = 3uy
  // RFC1996
  | [<Description("RFC1996")>]
    [<Name("")>] 
    Notify = 4uy
  // RFC2136
  | [<Description("RFC2136")>]
    [<Name("")>] 
    Update = 5uy
  | [<Description("")>]
    [<Name("")>] 
    RESERVED6 = 6uy
  | [<Description("")>]
    [<Name("")>] 
    RESERVED7 = 7uy
  | [<Description("")>]
    [<Name("")>] 
    RESERVED8 = 8uy
  | [<Description("")>]
    [<Name("")>] 
    RESERVED9 = 9uy
  | [<Description("")>]
    [<Name("")>] 
    RESERVED10 = 10uy
  | [<Description("")>]
    [<Name("")>] 
    RESERVED11 = 11uy
  | [<Description("")>]
    [<Name("")>] 
    RESERVED12 = 12uy
  | [<Description("")>]
    [<Name("")>] 
    RESERVED13 = 13uy
  | [<Description("")>]
    [<Name("")>] 
    RESERVED14 = 14uy
  | [<Description("")>]
    [<Name("")>] 
    RESERVED15 = 15uy
  
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module OPCode = begin
  end
