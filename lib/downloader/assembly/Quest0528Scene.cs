// Decompiled with JetBrains decompiler
// Type: Quest0528Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest0528Scene : NGSceneBase
{
  public Quest0528Menu menu;
  private bool isScript;
  private bool isInit;

  public static void changeScene(bool stack)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Singleton<NGSceneManager>.GetInstance().changeScene("quest052_8", stack);
  }

  public static void changeScene(bool stack, EarthExtraQuest quest)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Singleton<NGSceneManager>.GetInstance().changeScene("quest052_8", (stack ? 1 : 0) != 0, (object) quest);
  }

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public IEnumerator onStartSceneAsync()
  {
    this.isScript = false;
    int? script = Singleton<EarthDataManager>.GetInstance().questProgress.currentEpisode.script;
    if (script.HasValue && !Singleton<EarthDataManager>.GetInstance().questProgress.isRead)
    {
      this.isScript = true;
      Story0093Scene.changeScene009_3(false, script.Value);
      Singleton<EarthDataManager>.GetInstance().questProgress.isRead = true;
    }
    else
    {
      IEnumerator e = this.LoadBackGround();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.menu.Init(Singleton<EarthDataManager>.GetInstance().questProgress.currentEpisode.stage);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator onStartSceneAsync(EarthExtraQuest quest)
  {
    if (!this.isInit)
    {
      this.isInit = true;
      Singleton<EarthDataManager>.GetInstance().ForceDeselectEventQuestBattleIndex();
    }
    IEnumerator e = this.LoadBackGround();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.menu.Init(quest);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene()
  {
    if (this.isScript)
    {
      Singleton<CommonRoot>.GetInstance().isActiveHeader = false;
      Singleton<CommonRoot>.GetInstance().isActiveFooter = false;
      Singleton<CommonRoot>.GetInstance().isActiveBackground = false;
    }
    else
      Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public void onStartScene(EarthExtraQuest quest) => this.onStartScene();

  private IEnumerator LoadBackGround()
  {
    IEnumerator e = ((Component) this).GetComponent<BGChange>().EarthBGprefabCreate(Singleton<EarthDataManager>.GetInstance().questProgress.currentEpisode.background_name);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
