// Decompiled with JetBrains decompiler
// Type: RankingInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using UnityEngine;

#nullable disable
public class RankingInfo
{
  public Quest00229MenuParts scroll;
  public GameObject TextObject;

  public QuestScoreRankingPlayer player { get; set; }

  public RankingInfo TempCopy()
  {
    RankingInfo rankingInfo = (RankingInfo) this.MemberwiseClone();
    rankingInfo.scroll = (Quest00229MenuParts) null;
    return rankingInfo;
  }
}
