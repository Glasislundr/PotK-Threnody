// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BoostBonusGearDrilling
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
  public class BoostBonusGearDrilling
  {
    private int[] gearIds_;
    public int ID;
    public int period_id_BoostPeriod;
    public int kind_GearKind;
    public string gear_ids;
    public float boot_rate;
    public float increase_price;

    public bool isAllTargets => this.kind_GearKind == 9999 && string.IsNullOrEmpty(this.gear_ids);

    public int[] gearIds
    {
      get
      {
        if (this.gearIds_ != null || string.IsNullOrEmpty(this.gear_ids))
          return this.gearIds_;
        this.gearIds_ = ((IEnumerable<string>) this.gear_ids.Split(':')).Select<string, int>((Func<string, int>) (x =>
        {
          int result;
          if (!int.TryParse(x, out result))
            Debug.LogError((object) string.Format("「武具錬成」\"武具IDリスト\"列の変換に失敗しました！(ID={0}, Value={1})", (object) this.ID, (object) x));
          return result;
        })).ToArray<int>();
        return this.gearIds_;
      }
    }

    public static BoostBonusGearDrilling Parse(MasterDataReader reader)
    {
      return new BoostBonusGearDrilling()
      {
        ID = reader.ReadInt(),
        period_id_BoostPeriod = reader.ReadInt(),
        kind_GearKind = reader.ReadInt(),
        gear_ids = reader.ReadStringOrNull(true),
        boot_rate = reader.ReadFloat(),
        increase_price = reader.ReadFloat()
      };
    }

    public BoostPeriod period_id
    {
      get
      {
        BoostPeriod periodId;
        if (!MasterData.BoostPeriod.TryGetValue(this.period_id_BoostPeriod, out periodId))
          Debug.LogError((object) ("Key not Found: MasterData.BoostPeriod[" + (object) this.period_id_BoostPeriod + "]"));
        return periodId;
      }
    }

    public GearKind kind
    {
      get
      {
        GearKind kind;
        if (!MasterData.GearKind.TryGetValue(this.kind_GearKind, out kind))
          Debug.LogError((object) ("Key not Found: MasterData.GearKind[" + (object) this.kind_GearKind + "]"));
        return kind;
      }
    }
  }
}
