// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GachaTutorialDeckEntity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GachaTutorialDeckEntity
  {
    public int ID;
    public int deck_id_GachaTutorialDeck;
    public int reward_type_id_CommonRewardType;
    public int reward_id;
    public int? reward_quantity;
    public int _appearance;
    public bool is_pickup;

    public static GachaTutorialDeckEntity Parse(MasterDataReader reader)
    {
      return new GachaTutorialDeckEntity()
      {
        ID = reader.ReadInt(),
        deck_id_GachaTutorialDeck = reader.ReadInt(),
        reward_type_id_CommonRewardType = reader.ReadInt(),
        reward_id = reader.ReadInt(),
        reward_quantity = reader.ReadIntOrNull(),
        _appearance = reader.ReadInt(),
        is_pickup = reader.ReadBool()
      };
    }

    public GachaTutorialDeck deck_id
    {
      get
      {
        GachaTutorialDeck deckId;
        if (!MasterData.GachaTutorialDeck.TryGetValue(this.deck_id_GachaTutorialDeck, out deckId))
          Debug.LogError((object) ("Key not Found: MasterData.GachaTutorialDeck[" + (object) this.deck_id_GachaTutorialDeck + "]"));
        return deckId;
      }
    }

    public CommonRewardType reward_type_id
    {
      get => (CommonRewardType) this.reward_type_id_CommonRewardType;
    }
  }
}
