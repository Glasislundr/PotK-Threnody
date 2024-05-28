// Decompiled with JetBrains decompiler
// Type: MasterDataTable.EarthShopContent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class EarthShopContent
  {
    public int ID;
    public int article_EarthShopArticle;
    public int entity_type_CommonRewardType;
    public int entity_id;
    public int quantity;
    public bool upper_limit_check;
    public int upper_limit_count;

    public static EarthShopContent Parse(MasterDataReader reader)
    {
      return new EarthShopContent()
      {
        ID = reader.ReadInt(),
        article_EarthShopArticle = reader.ReadInt(),
        entity_type_CommonRewardType = reader.ReadInt(),
        entity_id = reader.ReadInt(),
        quantity = reader.ReadInt(),
        upper_limit_check = reader.ReadBool(),
        upper_limit_count = reader.ReadInt()
      };
    }

    public EarthShopArticle article
    {
      get
      {
        EarthShopArticle article;
        if (!MasterData.EarthShopArticle.TryGetValue(this.article_EarthShopArticle, out article))
          Debug.LogError((object) ("Key not Found: MasterData.EarthShopArticle[" + (object) this.article_EarthShopArticle + "]"));
        return article;
      }
    }

    public CommonRewardType entity_type => (CommonRewardType) this.entity_type_CommonRewardType;

    public int GetPossessionNum()
    {
      int possessionNum = 0;
      switch (this.entity_type)
      {
        case CommonRewardType.unit:
          possessionNum = Singleton<EarthDataManager>.GetInstance().GetPlayerUnits().AmountHavingTargetUnit(this.entity_id, this.entity_type);
          break;
        case CommonRewardType.supply:
        case CommonRewardType.gear:
          possessionNum = SMManager.Get<PlayerItem[]>().AmountHavingTargetItem(this.entity_id, this.entity_type);
          break;
        case CommonRewardType.quest_key:
          Earth.EarthQuestKey earthQuestKey = ((IEnumerable<Earth.EarthQuestKey>) Singleton<EarthDataManager>.GetInstance().GetQuestKeys()).FirstOrDefault<Earth.EarthQuestKey>((Func<Earth.EarthQuestKey, bool>) (x => x.ID == this.entity_id));
          if (earthQuestKey != null)
          {
            possessionNum = earthQuestKey.quantity;
            break;
          }
          break;
      }
      return possessionNum;
    }
  }
}
