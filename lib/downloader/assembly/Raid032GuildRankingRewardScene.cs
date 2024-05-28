// Decompiled with JetBrains decompiler
// Type: Raid032GuildRankingRewardScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Raid032GuildRankingRewardScene : NGSceneBase
{
  private static readonly string DEFAULT_NAME = "raid032_RankingReward_conf";
  private Raid032GuildRankingRewardMenu rankingRewardMenu;
  private static int periodID = 0;

  public static void ChangeScene(int periodId, bool bstack = true)
  {
    Raid032GuildRankingRewardScene.periodID = periodId;
    Singleton<NGSceneManager>.GetInstance().changeScene(Raid032GuildRankingRewardScene.DEFAULT_NAME, bstack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Raid032GuildRankingRewardScene rankingRewardScene = this;
    if (Object.op_Equality((Object) rankingRewardScene.rankingRewardMenu, (Object) null))
      rankingRewardScene.rankingRewardMenu = ((Component) rankingRewardScene).gameObject.GetComponent<Raid032GuildRankingRewardMenu>();
    IEnumerator e = rankingRewardScene.rankingRewardMenu.Initalize(Raid032GuildRankingRewardScene.periodID);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene() => Singleton<CommonRoot>.GetInstance().isLoading = false;

  public override IEnumerator onEndSceneAsync()
  {
    Raid032GuildRankingRewardScene rankingRewardScene = this;
    float startTime = Time.time;
    while (!rankingRewardScene.isTweenFinished && (double) Time.time - (double) startTime < (double) rankingRewardScene.tweenTimeoutTime)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    rankingRewardScene.isTweenFinished = true;
    yield return (object) null;
    // ISSUE: reference to a compiler-generated method
    yield return (object) rankingRewardScene.\u003C\u003En__0();
  }
}
