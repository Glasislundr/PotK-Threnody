// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SlotS001MedalDeckEntity
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
  public class SlotS001MedalDeckEntity
  {
    public int ID;
    public int deck_id;
    public int reward_type_id_CommonRewardType;
    public int reward_id;
    public int? reward_quantity;

    public static SlotS001MedalDeckEntity Parse(MasterDataReader reader)
    {
      return new SlotS001MedalDeckEntity()
      {
        ID = reader.ReadInt(),
        deck_id = reader.ReadInt(),
        reward_type_id_CommonRewardType = reader.ReadInt(),
        reward_id = reader.ReadInt(),
        reward_quantity = reader.ReadIntOrNull()
      };
    }

    public CommonRewardType reward_type_id
    {
      get => (CommonRewardType) this.reward_type_id_CommonRewardType;
    }

    public static SlotS001MedalDeckEntity[] getRewards(int deckId)
    {
      SlotS001MedalDeckEntity[] medalDeckEntityList = MasterData.SlotS001MedalDeckEntityList;
      HashSet<int> rewardIds = new HashSet<int>(((IEnumerable<SlotS001MedalDeckEntity>) medalDeckEntityList).Where<SlotS001MedalDeckEntity>((Func<SlotS001MedalDeckEntity, bool>) (x => x.reward_type_id == CommonRewardType.deck && x.deck_id == deckId)).Select<SlotS001MedalDeckEntity, int>((Func<SlotS001MedalDeckEntity, int>) (y => y.reward_id)));
      return ((IEnumerable<SlotS001MedalDeckEntity>) medalDeckEntityList).Where<SlotS001MedalDeckEntity>((Func<SlotS001MedalDeckEntity, bool>) (x => x.reward_type_id != CommonRewardType.deck && rewardIds.Contains(x.deck_id))).ToArray<SlotS001MedalDeckEntity>();
    }
  }
}
