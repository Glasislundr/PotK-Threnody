// Decompiled with JetBrains decompiler
// Type: SM_PlayerQuestKeyExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class SM_PlayerQuestKeyExtension
{
  public static int AmountHavingTargetKey(
    this PlayerQuestKey[] self,
    int entity_id,
    MasterDataTable.CommonRewardType entity_type)
  {
    int num = 0;
    if (entity_type == MasterDataTable.CommonRewardType.quest_key && self != null)
    {
      PlayerQuestKey playerQuestKey = ((IEnumerable<PlayerQuestKey>) self).FirstOrDefault<PlayerQuestKey>((Func<PlayerQuestKey, bool>) (k => k.quest_key_id == entity_id));
      if (playerQuestKey != null)
        num = playerQuestKey.quantity;
    }
    return num;
  }
}
