// Decompiled with JetBrains decompiler
// Type: Raid032RankingRewardConfScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Raid032RankingRewardConfScene : NGSceneBase
{
  [SerializeField]
  private Raid032RankingRewardConfMenu menu;

  public static void ChangeScene(bool stack, GuildRaid raid)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("raid032_PlayerRankingReward_conf", (stack ? 1 : 0) != 0, (object) raid);
  }

  public IEnumerator onStartSceneAsync(GuildRaid raid)
  {
    IEnumerator e = this.menu.Init(raid);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene(GuildRaid raid) => Singleton<CommonRoot>.GetInstance().isLoading = false;
}
