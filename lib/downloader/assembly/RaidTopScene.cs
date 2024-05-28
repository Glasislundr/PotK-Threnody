// Decompiled with JetBrains decompiler
// Type: RaidTopScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class RaidTopScene : NGSceneBase
{
  [SerializeField]
  private RaidTopMenu menu;
  private bool playStory;

  public static void ChangeScene()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("raid_top", Singleton<NGSceneManager>.GetInstance().sceneName != "raid_top");
  }

  public static void ChangeSceneBattleFinish(bool isStack = true)
  {
    bool flag = true;
    Singleton<NGSceneManager>.GetInstance().changeScene("raid_top", (isStack ? 1 : 0) != 0, (object) flag);
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.onStartSceneAsync(false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(bool fromBattle)
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    this.setPlayStory();
    if (!this.playStory)
    {
      IEnumerator e = this.menu.InitializeAsync(fromBattle);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private void setPlayStory()
  {
    Persist.eventStoryPlay.Data.SetReserveList(StoryPlaybackEventPlay.GetPlayIDList(ServerTime.NowAppTime(), this.sceneName), this.sceneName);
    this.playStory = Persist.eventStoryPlay.Data.PlayEventScript(this.sceneName, 0);
  }

  public void onStartScene()
  {
    if (this.menu.isFailedInit || this.playStory)
      return;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    this.ShowAdvice();
    this.StartCoroutine(this.menu.playSceneStartEffect());
  }

  public void onStartScene(bool fromBattle) => this.onStartScene();

  private void ShowAdvice()
  {
    Singleton<TutorialRoot>.GetInstance().ShowAdvice("guild028_1_raid_tutorial");
  }

  public override IEnumerator onEndSceneAsync()
  {
    RaidTopScene raidTopScene = this;
    float startTime = Time.time;
    while (!raidTopScene.isTweenFinished && (double) Time.time - (double) startTime < (double) raidTopScene.tweenTimeoutTime)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    raidTopScene.isTweenFinished = true;
    yield return (object) null;
    // ISSUE: reference to a compiler-generated method
    yield return (object) raidTopScene.\u003C\u003En__0();
  }

  public override IEnumerator onDestroySceneAsync()
  {
    GuildUtil.gvgPopupState = GuildUtil.GvGPopupState.None;
    GuildUtil.gvgDeckAttack = (GvgDeck) null;
    GuildUtil.gvgDeckDefense = (GvgDeck) null;
    GuildUtil.gvgFriendDefense = (GvgReinforcement) null;
    return base.onDestroySceneAsync();
  }
}
