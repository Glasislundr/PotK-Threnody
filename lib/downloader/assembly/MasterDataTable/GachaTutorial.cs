// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GachaTutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GachaTutorial
  {
    public int ID;
    public string name;
    public string description;
    public int _logic_id;
    public int payment_type_id_CommonPayType;
    public int? payment_id;
    public int payment_amount;
    public int _deck_id;
    public int _roll_count;
    public int? _limit;
    public int? _daily_limit;
    public int? _period_id_GachaTutorialPeriod;
    public int? max_roll_count;

    public static GachaTutorial Parse(MasterDataReader reader)
    {
      return new GachaTutorial()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        description = reader.ReadString(true),
        _logic_id = reader.ReadInt(),
        payment_type_id_CommonPayType = reader.ReadInt(),
        payment_id = reader.ReadIntOrNull(),
        payment_amount = reader.ReadInt(),
        _deck_id = reader.ReadInt(),
        _roll_count = reader.ReadInt(),
        _limit = reader.ReadIntOrNull(),
        _daily_limit = reader.ReadIntOrNull(),
        _period_id_GachaTutorialPeriod = reader.ReadIntOrNull(),
        max_roll_count = reader.ReadIntOrNull()
      };
    }

    public CommonPayType payment_type_id => (CommonPayType) this.payment_type_id_CommonPayType;

    public GachaTutorialPeriod _period_id
    {
      get
      {
        if (!this._period_id_GachaTutorialPeriod.HasValue)
          return (GachaTutorialPeriod) null;
        GachaTutorialPeriod periodId;
        if (!MasterData.GachaTutorialPeriod.TryGetValue(this._period_id_GachaTutorialPeriod.Value, out periodId))
          Debug.LogError((object) ("Key not Found: MasterData.GachaTutorialPeriod[" + (object) this._period_id_GachaTutorialPeriod.Value + "]"));
        return periodId;
      }
    }
  }
}
