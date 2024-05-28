// Decompiled with JetBrains decompiler
// Type: Tower029UnitSelectionScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Tower029UnitSelectionScene : NGSceneBase
{
  [SerializeField]
  private Tower029UnitSelectionMenu menu;
  private bool isFirstInit = true;

  public static void ChangeScene(
    TowerUtil.UnitSelectionMode mode,
    TowerProgress progress,
    TowerUtil.SequenceType type)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("tower029_unit_selection", true, (object) mode, (object) progress, (object) type);
  }

  public IEnumerator onStartSceneAsync(
    TowerUtil.UnitSelectionMode mode,
    TowerProgress progress,
    TowerUtil.SequenceType type,
    int lastReferenceUnitId)
  {
    if (this.isFirstInit)
    {
      if (lastReferenceUnitId == -1)
        lastReferenceUnitId = Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID;
      this.menu.setLastReference(lastReferenceUnitId);
    }
    yield return (object) this.onStartSceneAsync(mode, progress, type);
  }

  public IEnumerator onStartSceneAsync(
    TowerUtil.UnitSelectionMode mode,
    TowerProgress progress,
    TowerUtil.SequenceType type)
  {
    Tower029UnitSelectionScene unitSelectionScene = this;
    unitSelectionScene.isFirstInit = false;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    IEnumerator e = unitSelectionScene.menu.InitializeAsync(mode, progress, type);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitSelectionScene.bgmFile = TowerUtil.BgmFile;
    unitSelectionScene.bgmName = TowerUtil.BgmName;
  }

  public void onStartScene(
    TowerUtil.UnitSelectionMode mode,
    TowerProgress progress,
    TowerUtil.SequenceType type,
    int lastReferenceUnitId)
  {
    this.onStartScene(mode, progress, type);
  }

  public void onStartScene(
    TowerUtil.UnitSelectionMode mode,
    TowerProgress progress,
    TowerUtil.SequenceType type)
  {
    this.menu.onStartScene(mode);
  }

  public override void onEndScene() => Singleton<CommonRoot>.GetInstance().isLoading = true;
}
