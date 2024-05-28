// Decompiled with JetBrains decompiler
// Type: CorpsQuestUnitSelectionScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CorpsQuest/UnitSelectionScene")]
public class CorpsQuestUnitSelectionScene : NGSceneBase
{
  private CorpsQuestUnitSelectionMenu menu;
  private CorpsSetting setting;

  public static void ChangeScene(
    PlayerCorps corps,
    CorpsUtil.UnitSelectionMode mode,
    CorpsUtil.SequenceType type)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("CorpsQuest_unit_selection", true, (object) corps, (object) mode, (object) type);
  }

  public override IEnumerator onInitSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    CorpsQuestUnitSelectionScene unitSelectionScene = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    unitSelectionScene.menu = unitSelectionScene.menuBase as CorpsQuestUnitSelectionMenu;
    return false;
  }

  public IEnumerator onStartSceneAsync(
    PlayerCorps corps,
    CorpsUtil.UnitSelectionMode mode,
    CorpsUtil.SequenceType type,
    int lastReferenceUnitId)
  {
    if (lastReferenceUnitId == -1)
      lastReferenceUnitId = Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID;
    this.menu.setLastReference(lastReferenceUnitId);
    IEnumerator e = this.onStartSceneAsync(corps, mode, type);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onBackSceneAsync(
    PlayerCorps corps,
    CorpsUtil.UnitSelectionMode mode,
    CorpsUtil.SequenceType type,
    int lastReferenceUnitId)
  {
    IEnumerator e = this.onBackSceneAsync(corps, mode, type);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    PlayerCorps corps,
    CorpsUtil.UnitSelectionMode mode,
    CorpsUtil.SequenceType type)
  {
    CorpsQuestUnitSelectionScene unitSelectionScene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    MasterData.CorpsSetting.TryGetValue(corps.corps_id, out unitSelectionScene.setting);
    IEnumerator e = unitSelectionScene.menu.OnStartSceneAsync(corps, mode, type);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (unitSelectionScene.setting != null && !string.IsNullOrEmpty(unitSelectionScene.setting.bgm_file))
    {
      unitSelectionScene.bgmFile = unitSelectionScene.setting.bgm_file;
      unitSelectionScene.bgmName = unitSelectionScene.setting.bgm_name;
    }
  }

  public IEnumerator onBackSceneAsync(
    PlayerCorps corps,
    CorpsUtil.UnitSelectionMode mode,
    CorpsUtil.SequenceType type)
  {
    CorpsQuestUnitSelectionScene unitSelectionScene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    IEnumerator e = unitSelectionScene.menu.OnBackSceneAsync(mode, type);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (unitSelectionScene.setting != null && !string.IsNullOrEmpty(unitSelectionScene.setting.bgm_file))
    {
      unitSelectionScene.bgmFile = unitSelectionScene.setting.bgm_file;
      unitSelectionScene.bgmName = unitSelectionScene.setting.bgm_name;
    }
  }

  public void onStartScene(
    PlayerCorps corps,
    CorpsUtil.UnitSelectionMode mode,
    CorpsUtil.SequenceType type,
    int lastReferenceUnitId)
  {
    this.onStartScene(corps, mode, type);
  }

  public void onStartScene(
    PlayerCorps corps,
    CorpsUtil.UnitSelectionMode mode,
    CorpsUtil.SequenceType type)
  {
    this.menu.onStartScene(mode);
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }
}
