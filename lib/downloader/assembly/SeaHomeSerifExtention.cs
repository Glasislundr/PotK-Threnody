// Decompiled with JetBrains decompiler
// Type: SeaHomeSerifExtention
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class SeaHomeSerifExtention
{
  public static SeaHomeSerif[] GetSerifs(
    this IEnumerable<SeaHomeSerif> self,
    DateTime now,
    UnitUnit unit,
    bool hasUnit,
    int trust)
  {
    IEnumerable<SeaHomeSerif> source = self.Where<SeaHomeSerif>((Func<SeaHomeSerif, bool>) (x =>
    {
      if (x.time_zone != null && (x.time_zone == null || !x.time_zone.WithIn(now)))
        return false;
      if (!x.trust_provision.HasUnit() && !hasUnit)
        return true;
      return x.trust_provision.HasUnit() & hasUnit && x.trust_provision.WithIn(trust);
    }));
    return source.Any<SeaHomeSerif>((Func<SeaHomeSerif, bool>) (x => x.same_character_id_UnitUnit.HasValue && x.same_character_id_UnitUnit.Value == unit.same_character_id)) ? source.Where<SeaHomeSerif>((Func<SeaHomeSerif, bool>) (x => x.same_character_id_UnitUnit.HasValue && x.same_character_id_UnitUnit.Value == unit.same_character_id)).ToArray<SeaHomeSerif>() : source.Where<SeaHomeSerif>((Func<SeaHomeSerif, bool>) (x => x.character_id.HasValue && x.character_id.Value == unit.character_UnitCharacter)).ToArray<SeaHomeSerif>();
  }
}
