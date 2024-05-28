// Decompiled with JetBrains decompiler
// Type: Tower029TopScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Tower029TopScene : NGSceneBase
{
  [SerializeField]
  private Tower029TopMenu menu;
  private bool playStory;

  public static void ChangeScene(bool isStack, bool forceInitialize = false)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("tower029_top", (isStack ? 1 : 0) != 0, (object) forceInitialize);
  }

  public IEnumerator onStartSceneAsync(bool forceInitialize = false)
  {
    Tower029TopScene tower029TopScene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    Persist.eventStoryPlay.Data.SetReserveList(StoryPlaybackEventPlay.GetPlayIDList(ServerTime.NowAppTime(), tower029TopScene.sceneName), tower029TopScene.sceneName);
    tower029TopScene.playStory = Persist.eventStoryPlay.Data.PlayEventScript(tower029TopScene.sceneName, 0);
    if (!tower029TopScene.playStory)
    {
      IEnumerator e = tower029TopScene.menu.InitializeAsync(forceInitialize);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      tower029TopScene.bgmFile = TowerUtil.BgmFile;
      tower029TopScene.bgmName = TowerUtil.BgmName;
    }
  }

  public void onStartScene(bool forceInitialize = false)
  {
    if (this.playStory)
      return;
    this.menu.StartScene();
  }

  public override IEnumerator onDestroySceneAsync()
  {
    TowerUtil.towerDeckUnits = (TowerDeckUnit[]) null;
    TowerUtil.TowerPlayer = (TowerPlayer) null;
    Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    return base.onDestroySceneAsync();
  }

  public override void onEndScene()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    this.menu.onEndScene();
  }
}
