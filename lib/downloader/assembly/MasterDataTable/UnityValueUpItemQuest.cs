// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnityValueUpItemQuest
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
  public class UnityValueUpItemQuest
  {
    public int ID;
    public int material_unit_id_UnitUnit;
    public string quest_sids;
    public int quest_type_CommonQuestType;
    public bool is_skip_sortie;
    private static readonly int[] array_empty = new int[0];

    public static UnityValueUpItemQuest Parse(MasterDataReader reader)
    {
      return new UnityValueUpItemQuest()
      {
        ID = reader.ReadInt(),
        material_unit_id_UnitUnit = reader.ReadInt(),
        quest_sids = reader.ReadStringOrNull(true),
        quest_type_CommonQuestType = reader.ReadInt(),
        is_skip_sortie = reader.ReadBool()
      };
    }

    public UnitUnit material_unit_id
    {
      get
      {
        UnitUnit materialUnitId;
        if (!MasterData.UnitUnit.TryGetValue(this.material_unit_id_UnitUnit, out materialUnitId))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.material_unit_id_UnitUnit + "]"));
        return materialUnitId;
      }
    }

    public CommonQuestType quest_type => (CommonQuestType) this.quest_type_CommonQuestType;

    public int[] questSIDs
    {
      get
      {
        if (string.IsNullOrEmpty(this.quest_sids))
          return UnityValueUpItemQuest.array_empty;
        return ((IEnumerable<string>) this.quest_sids.Split(':')).Select<string, int>((Func<string, int>) (s =>
        {
          int result;
          if (int.TryParse(s.Replace(".0", ""), out result))
            return result;
          Debug.LogError((object) string.Format("Parse Failed! UnityValueUpItemQuest(ID={0} quest_sids=...\"{1}\")", (object) this.ID, (object) s));
          return 0;
        })).ToArray<int>();
      }
    }

    public static Dictionary<int, List<UnitUnit>> makeSkipSortieQuestUnityValueUp(
      IEnumerable<int> sids)
    {
      if (sids == null || !sids.Any<int>())
        return new Dictionary<int, List<UnitUnit>>();
      HashSet<int> intSet = new HashSet<int>(sids);
      Dictionary<int, List<UnitUnit>> dictionary = new Dictionary<int, List<UnitUnit>>();
      foreach (UnityValueUpItemQuest valueUpItemQuest in MasterData.UnityValueUpItemQuestList)
      {
        if (valueUpItemQuest.is_skip_sortie && !string.IsNullOrEmpty(valueUpItemQuest.quest_sids))
        {
          UnitUnit materialUnitId = valueUpItemQuest.material_unit_id;
          if (materialUnitId != null && materialUnitId.is_unity_value_up)
          {
            foreach (int questSiD in valueUpItemQuest.questSIDs)
            {
              if (intSet.Contains(questSiD))
              {
                List<UnitUnit> unitUnitList;
                if (!dictionary.TryGetValue(questSiD, out unitUnitList))
                {
                  unitUnitList = new List<UnitUnit>()
                  {
                    materialUnitId
                  };
                  dictionary.Add(questSiD, unitUnitList);
                }
                else if (!unitUnitList.Contains(materialUnitId))
                  unitUnitList.Add(materialUnitId);
              }
            }
          }
        }
      }
      return dictionary;
    }
  }
}
