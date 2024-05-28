// Decompiled with JetBrains decompiler
// Type: Raid032GuildRankingScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Raid032GuildRankingScene : NGSceneBase
{
  private static readonly string DEFAULT_NAME = "raid032_GuildRanking";
  private Raid032GuildRankingMenu rankingMenu;

  public static void ChangeScene(int id, bool bstack = true)
  {
    Singleton<NGSceneManager>.GetInstance().clearStack("raid032_GuildRanking");
    Singleton<NGSceneManager>.GetInstance().changeScene(Raid032GuildRankingScene.DEFAULT_NAME, (bstack ? 1 : 0) != 0, (object) id);
  }

  public IEnumerator onStartSceneAsync(int id)
  {
    Raid032GuildRankingScene guildRankingScene = this;
    if (Object.op_Equality((Object) guildRankingScene.rankingMenu, (Object) null))
      guildRankingScene.rankingMenu = ((Component) guildRankingScene).gameObject.GetComponent<Raid032GuildRankingMenu>();
    GuildRaid mMasterData = (GuildRaid) null;
    if (!MasterData.GuildRaid.TryGetValue(id, out mMasterData))
      Debug.LogError((object) ("There is no MasterData in local [ID:" + (object) id + "]"));
    IEnumerator e = guildRankingScene.rankingMenu.Initalize(mMasterData);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene(int id) => Singleton<CommonRoot>.GetInstance().isLoading = false;

  public override IEnumerator onEndSceneAsync()
  {
    Raid032GuildRankingScene guildRankingScene = this;
    float startTime = Time.time;
    while (!guildRankingScene.isTweenFinished && (double) Time.time - (double) startTime < (double) guildRankingScene.tweenTimeoutTime)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    guildRankingScene.isTweenFinished = true;
    yield return (object) null;
    // ISSUE: reference to a compiler-generated method
    yield return (object) guildRankingScene.\u003C\u003En__0();
  }
}
