// Decompiled with JetBrains decompiler
// Type: Quest00210aScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System.Collections;

#nullable disable
public class Quest00210aScene : NGSceneBase
{
  public Quest00210aMenu menu;

  public static void ChangeScene(bool stack, Quest00210Menu.Param param, bool fromTower = false)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_10a", (stack ? 1 : 0) != 0, (object) param, (object) fromTower);
  }

  public static void ChangeScene(
    bool stack,
    Quest00210Menu.Param param,
    int[] selectedUnitIds,
    TowerProgress progress,
    TowerUtil.SequenceType type)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_10a", (stack ? 1 : 0) != 0, (object) param, (object) selectedUnitIds, (object) progress, (object) type);
  }

  public static void ChangeScene(
    bool stack,
    Quest00210Menu.Param param,
    int[] selectedUnitIds,
    PlayerCorps progress,
    CorpsUtil.SequenceType type)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_10a", (stack ? 1 : 0) != 0, (object) param, (object) selectedUnitIds, (object) progress, (object) type);
  }

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public IEnumerator onStartSceneAsync(Quest00210Menu.Param param, bool fromTower = false)
  {
    Quest00210aScene quest00210aScene = this;
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
      quest00210aScene.bgmName = "bgm104";
    else if (Singleton<CommonRoot>.GetInstance().headerType == CommonRoot.HeaderType.Tower)
    {
      quest00210aScene.bgmFile = TowerUtil.BgmFile;
      quest00210aScene.bgmName = TowerUtil.BgmName;
    }
    IEnumerator e = quest00210aScene.menu.InitSupplyList(param, fromTower);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    Quest00210Menu.Param param,
    int[] selectedUnitIds,
    TowerProgress progress,
    TowerUtil.SequenceType type)
  {
    Quest00210aScene quest00210aScene = this;
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
      quest00210aScene.bgmName = "bgm104";
    else if (Singleton<CommonRoot>.GetInstance().headerType == CommonRoot.HeaderType.Tower)
    {
      quest00210aScene.bgmFile = TowerUtil.BgmFile;
      quest00210aScene.bgmName = TowerUtil.BgmName;
    }
    quest00210aScene.menu.SetTowerInfo(selectedUnitIds, progress, type);
    IEnumerator e = quest00210aScene.menu.InitSupplyList(param, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    Quest00210Menu.Param param,
    int[] selectedUnitIds,
    PlayerCorps progress,
    CorpsUtil.SequenceType type)
  {
    Quest00210aScene quest00210aScene = this;
    CorpsSetting corpsSetting;
    MasterData.CorpsSetting.TryGetValue(progress.corps_id, out corpsSetting);
    if (corpsSetting != null && !string.IsNullOrEmpty(corpsSetting.bgm_file))
    {
      quest00210aScene.bgmFile = corpsSetting.bgm_file;
      quest00210aScene.bgmName = corpsSetting.bgm_name;
    }
    quest00210aScene.menu.SetCorpsInfo(selectedUnitIds, progress, type);
    IEnumerator e = quest00210aScene.menu.InitSupplyList(param, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void onStartScene(Quest00210Menu.Param param, bool fromTower = false)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = true;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public virtual void onStartScene(
    Quest00210Menu.Param param,
    int[] selectedUnitIds,
    TowerProgress progress,
    TowerUtil.SequenceType type)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = true;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public virtual void onStartScene(
    Quest00210Menu.Param param,
    int[] selectedUnitIds,
    PlayerCorps progress,
    CorpsUtil.SequenceType type)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = true;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public override void onEndScene() => this.menu.onEndScene();
}
