// Decompiled with JetBrains decompiler
// Type: MasterDataTable.OverkillersGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class OverkillersGroup
  {
    public int ID;
    public int child_unit_id;
    public string parent_unit_id;
    public string parameter_no;
    public bool is_for_all_units;
    private int[] parent_unit_ids_;
    private int[] parameter_no_list_;

    public static OverkillersGroup Parse(MasterDataReader reader)
    {
      return new OverkillersGroup()
      {
        ID = reader.ReadInt(),
        child_unit_id = reader.ReadInt(),
        parent_unit_id = reader.ReadStringOrNull(true),
        parameter_no = reader.ReadStringOrNull(true),
        is_for_all_units = reader.ReadBool()
      };
    }

    public int[] parent_unit_ids
    {
      get
      {
        return this.parent_unit_ids_ ?? (this.parent_unit_ids_ = this.stringToIntegers(this.parent_unit_id));
      }
    }

    public int[] parameter_no_list
    {
      get
      {
        return this.parameter_no_list_ ?? (this.parameter_no_list_ = this.stringToIntegers(this.parameter_no));
      }
    }

    private int[] stringToIntegers(string str)
    {
      if (string.IsNullOrEmpty(str))
        return new int[0];
      double result;
      return ((IEnumerable<string>) str.Split(',')).Select<string, int>((Func<string, int>) (s => !double.TryParse(s.Trim(), out result) ? 0 : (int) Math.Floor(result))).ToArray<int>();
    }

    public static List<OverkillersGroup> getOverkillersGroupsByParent(int parent_unit_id)
    {
      List<OverkillersGroup> overkillersGroupsByParent = new List<OverkillersGroup>();
      foreach (OverkillersGroup overkillersGroup in MasterData.OverkillersGroupList)
      {
        foreach (int parentUnitId in overkillersGroup.parent_unit_ids)
        {
          if (parent_unit_id == parentUnitId)
          {
            overkillersGroupsByParent.Add(overkillersGroup);
            break;
          }
        }
      }
      return overkillersGroupsByParent;
    }

    public static bool IsForAllUnits(int child_unit_id)
    {
      return ((IEnumerable<OverkillersGroup>) MasterData.OverkillersGroupList).Any<OverkillersGroup>((Func<OverkillersGroup, bool>) (x => x.is_for_all_units && x.child_unit_id == child_unit_id));
    }
  }
}
