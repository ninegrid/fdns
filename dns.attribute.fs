namespace thinkhard.Protocol.Dns

open thinkhard

type DescriptionAttribute (s : string) = 
  inherit System.Attribute ()
  member self.Description = s

type NameAttribute (s : string) =
  inherit System.Attribute ()
  member self.Name = s
