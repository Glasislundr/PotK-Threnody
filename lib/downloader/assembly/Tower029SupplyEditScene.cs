// Decompiled with JetBrains decompiler
// Type: Tower029SupplyEditScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Tower029SupplyEditScene : NGSceneBase
{
  [SerializeField]
  private Tower029SupplyEditMenu menu;

  public static void ChangeScene(
    int[] selectedUnitIds,
    TowerProgress progress,
    TowerUtil.SequenceType type)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("tower029_supplies_edit", true, (object) selectedUnitIds, (object) progress, (object) type);
  }

  public static void ChangeScene(List<SupplyItem> supplyItems)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("tower029_supplies_edit", false, (object) supplyItems);
  }

  public IEnumerator onStartSceneAsync(
    int[] selectedUnitIds,
    TowerProgress progress,
    TowerUtil.SequenceType type)
  {
    Tower029SupplyEditScene tower029SupplyEditScene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    List<PlayerItem> list1 = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllSupplies()).ToList<PlayerItem>();
    List<PlayerItem> list2 = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllTowerSupplies()).ToList<PlayerItem>();
    IEnumerator e = tower029SupplyEditScene.menu.InitializeAsync(selectedUnitIds, ((IEnumerable<SupplyItem>) SupplyItem.Merge(list1.ToArray(), list2.ToArray())).ToList<SupplyItem>(), progress, type);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    tower029SupplyEditScene.bgmFile = TowerUtil.BgmFile;
    tower029SupplyEditScene.bgmName = TowerUtil.BgmName;
  }

  public IEnumerator onStartSceneAsync(List<SupplyItem> supplyItems)
  {
    Tower029SupplyEditScene tower029SupplyEditScene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    IEnumerator e = tower029SupplyEditScene.menu.InitializeAsync((int[]) null, supplyItems, (TowerProgress) null, TowerUtil.SequenceType.None);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    tower029SupplyEditScene.bgmFile = TowerUtil.BgmFile;
    tower029SupplyEditScene.bgmName = TowerUtil.BgmName;
  }

  public void onStartScene(
    int[] selectedUnitIds,
    TowerProgress progress,
    TowerUtil.SequenceType type)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public void onStartScene(List<SupplyItem> supplyItems)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public override void onEndScene() => Singleton<CommonRoot>.GetInstance().isLoading = true;
}
