// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitMaterialExclusion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitMaterialExclusion
  {
    public int ID;
    public int material_UnitUnit;
    public int exclusion_type_UnitMaterialExclusionType;
    public string values;
    private static readonly Dictionary<UnitMaterialExclusionType, HashSet<int>> dicEmpty = new Dictionary<UnitMaterialExclusionType, HashSet<int>>(0);

    public static UnitMaterialExclusion Parse(MasterDataReader reader)
    {
      return new UnitMaterialExclusion()
      {
        ID = reader.ReadInt(),
        material_UnitUnit = reader.ReadInt(),
        exclusion_type_UnitMaterialExclusionType = reader.ReadInt(),
        values = reader.ReadString(true)
      };
    }

    public UnitUnit material
    {
      get
      {
        UnitUnit material;
        if (!MasterData.UnitUnit.TryGetValue(this.material_UnitUnit, out material))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.material_UnitUnit + "]"));
        return material;
      }
    }

    public UnitMaterialExclusionType exclusion_type
    {
      get => (UnitMaterialExclusionType) this.exclusion_type_UnitMaterialExclusionType;
    }

    public static Dictionary<UnitMaterialExclusionType, HashSet<int>> getExclusions(
      int materialUnitID)
    {
      IEnumerable<UnitMaterialExclusion> source = ((IEnumerable<UnitMaterialExclusion>) MasterData.UnitMaterialExclusionList).Where<UnitMaterialExclusion>((Func<UnitMaterialExclusion, bool>) (x => x.material_UnitUnit == materialUnitID));
      return !source.Any<UnitMaterialExclusion>() ? UnitMaterialExclusion.dicEmpty : source.GroupBy<UnitMaterialExclusion, UnitMaterialExclusionType>((Func<UnitMaterialExclusion, UnitMaterialExclusionType>) (y => y.exclusion_type)).ToDictionary<IGrouping<UnitMaterialExclusionType, UnitMaterialExclusion>, UnitMaterialExclusionType, HashSet<int>>((Func<IGrouping<UnitMaterialExclusionType, UnitMaterialExclusion>, UnitMaterialExclusionType>) (k => k.Key), (Func<IGrouping<UnitMaterialExclusionType, UnitMaterialExclusion>, HashSet<int>>) (v => new HashSet<int>(v.SelectMany<UnitMaterialExclusion, int>((Func<UnitMaterialExclusion, IEnumerable<int>>) (vv => (IEnumerable<int>) vv.values.CommaSeparatedToInts())))));
    }

    public static bool checkExclusion(
      UnitUnit target,
      Dictionary<UnitMaterialExclusionType, HashSet<int>> dicExclusions)
    {
      if (dicExclusions == null)
        return false;
      foreach (KeyValuePair<UnitMaterialExclusionType, HashSet<int>> dicExclusion in dicExclusions)
      {
        switch (dicExclusion.Key)
        {
          case UnitMaterialExclusionType.character_id:
            if (dicExclusion.Value.Contains(target.character_UnitCharacter))
              return true;
            continue;
          case UnitMaterialExclusionType.same_character_id:
            if (dicExclusion.Value.Contains(target.same_character_id))
              return true;
            continue;
          case UnitMaterialExclusionType.unit_id:
            if (dicExclusion.Value.Contains(target.ID))
              return true;
            continue;
          default:
            continue;
        }
      }
      return false;
    }
  }
}
