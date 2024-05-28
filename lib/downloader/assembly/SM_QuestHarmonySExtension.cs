// Decompiled with JetBrains decompiler
// Type: SM_QuestHarmonySExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class SM_QuestHarmonySExtension
{
  public static PlayerHarmonyQuestS[] SelectReleased(this PlayerHarmonyQuestS[] self)
  {
    return ((IEnumerable<PlayerHarmonyQuestS>) self).Where<PlayerHarmonyQuestS>((Func<PlayerHarmonyQuestS, bool>) (questDetail => QuestCharacterS.CheckIsReleased(questDetail.quest_harmony_s.start_at))).ToArray<PlayerHarmonyQuestS>();
  }
}
