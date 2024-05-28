// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleLandform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleLandform
  {
    private static GameGlobalVariable<AssocList<int, BattleLandformIncr>> incrDic = GameGlobalVariable<AssocList<int, BattleLandformIncr>>.Null();
    public int ID;
    public int base_id;
    public string name;
    public int in_out_BattleInOutSide;
    public int footstep_type_BattleLandformFootstepType;
    public int tag1;
    public int tag2;
    public int tag3;
    public string description;

    private static int MakeIncrKey(BattleLandform landform, UnitMoveType move_type)
    {
      return (int) ((landform.ID << 8) + move_type);
    }

    public static void CacheClear()
    {
      BattleLandform.incrDic = GameGlobalVariable<AssocList<int, BattleLandformIncr>>.Null();
    }

    public BattleLandformIncr GetIncr(UnitMoveType move_type)
    {
      if (BattleLandform.incrDic.Get() == null)
      {
        AssocList<int, BattleLandformIncr> v = new AssocList<int, BattleLandformIncr>();
        BattleLandform.incrDic.Reset(v);
        foreach (BattleLandformIncr battleLandformIncr in MasterData.BattleLandformIncrList)
          v.Add(BattleLandform.MakeIncrKey(battleLandformIncr.landform, battleLandformIncr.move_type), battleLandformIncr);
      }
      return BattleLandform.incrDic.Get()[BattleLandform.MakeIncrKey(this, move_type)];
    }

    public BattleLandformIncr GetIncr(BL.Unit unit) => this.GetIncr(unit.job.move_type);

    public BattleLandformIncr GetDisplayIncr() => this.GetIncr(UnitMoveType.keihohei);

    public BattleLandformIncr[] GetAllIncr()
    {
      return ((IEnumerable<BattleLandformIncr>) MasterData.BattleLandformIncrList).Where<BattleLandformIncr>((Func<BattleLandformIncr, bool>) (x => x.landform_BattleLandform == this.ID)).ToArray<BattleLandformIncr>();
    }

    public BattleUnitLandformFootstep GetFootstep(UnitUnit unit)
    {
      int targetId = unit.footstep_type_UnitFootstepType;
      int myId = this.footstep_type_BattleLandformFootstepType;
      return ((IEnumerable<BattleUnitLandformFootstep>) MasterData.BattleUnitLandformFootstepList).Single<BattleUnitLandformFootstep>((Func<BattleUnitLandformFootstep, bool>) (x => x.unit_footstep_type_UnitFootstepType == targetId && x.landform_footstep_type_BattleLandformFootstepType == myId));
    }

    public bool HasTag(int tag) => this.tag1 == tag || this.tag2 == tag || this.tag3 == tag;

    public int baseID => this.base_id != 0 ? this.base_id : this.ID;

    public static BattleLandform Parse(MasterDataReader reader)
    {
      return new BattleLandform()
      {
        ID = reader.ReadInt(),
        base_id = reader.ReadInt(),
        name = reader.ReadString(true),
        in_out_BattleInOutSide = reader.ReadInt(),
        footstep_type_BattleLandformFootstepType = reader.ReadInt(),
        tag1 = reader.ReadInt(),
        tag2 = reader.ReadInt(),
        tag3 = reader.ReadInt(),
        description = reader.ReadString(true)
      };
    }

    public BattleInOutSide in_out => (BattleInOutSide) this.in_out_BattleInOutSide;

    public BattleLandformFootstepType footstep_type
    {
      get
      {
        BattleLandformFootstepType footstepType;
        if (!MasterData.BattleLandformFootstepType.TryGetValue(this.footstep_type_BattleLandformFootstepType, out footstepType))
          Debug.LogError((object) ("Key not Found: MasterData.BattleLandformFootstepType[" + (object) this.footstep_type_BattleLandformFootstepType + "]"));
        return footstepType;
      }
    }
  }
}
