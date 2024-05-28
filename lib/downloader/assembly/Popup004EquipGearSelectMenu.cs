// Decompiled with JetBrains decompiler
// Type: Popup004EquipGearSelectMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup004EquipGearSelectMenu : BackButtonMonoBehaiviour
{
  [SerializeField]
  private CreateIconObject firstGear;
  [SerializeField]
  private CreateIconObject secondGear;
  private bool isPush;
  private GameObject gearIconPrefab;
  private PlayerUnit playerUnit;

  private bool isPushAndSet()
  {
    if (this.isPush)
      return true;
    this.isPush = true;
    return false;
  }

  private IEnumerator setEmptyGearIcon(Transform parent, Action<ItemIcon> onClick)
  {
    Future<GameObject> gearPrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = gearPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ItemIcon component = gearPrefabF.Result.Clone(parent).GetComponent<ItemIcon>();
    component.SetEmpty(true);
    component.gear.favorite.SetActive(false);
    ((Component) component.gear.button).gameObject.SetActive(true);
    component.onClick = onClick;
  }

  public IEnumerator Init(PlayerUnit playerUnit)
  {
    Popup004EquipGearSelectMenu equipGearSelectMenu = this;
    if (Object.op_Inequality((Object) ((Component) equipGearSelectMenu).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) equipGearSelectMenu).GetComponent<UIWidget>()).alpha = 0.0f;
    equipGearSelectMenu.playerUnit = playerUnit;
    PlayerItem equippedGear = playerUnit.equippedGear;
    PlayerItem gear2 = playerUnit.equippedGear2;
    IEnumerator e;
    if (equippedGear != (PlayerItem) null)
    {
      e = equipGearSelectMenu.firstGear.CreateThumbnail(MasterDataTable.CommonRewardType.gear, equippedGear.gear.ID);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      // ISSUE: reference to a compiler-generated method
      equipGearSelectMenu.firstGear.GetIcon().GetComponent<ItemIcon>().onClick = new Action<ItemIcon>(equipGearSelectMenu.\u003CInit\u003Eb__7_0);
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      e = equipGearSelectMenu.setEmptyGearIcon(((Component) equipGearSelectMenu.firstGear).transform, new Action<ItemIcon>(equipGearSelectMenu.\u003CInit\u003Eb__7_2));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (gear2 != (PlayerItem) null)
    {
      e = equipGearSelectMenu.secondGear.CreateThumbnail(MasterDataTable.CommonRewardType.gear, gear2.gear.ID);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      // ISSUE: reference to a compiler-generated method
      equipGearSelectMenu.secondGear.GetIcon().GetComponent<ItemIcon>().onClick = new Action<ItemIcon>(equipGearSelectMenu.\u003CInit\u003Eb__7_1);
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      e = equipGearSelectMenu.setEmptyGearIcon(((Component) equipGearSelectMenu.secondGear).transform, new Action<ItemIcon>(equipGearSelectMenu.\u003CInit\u003Eb__7_3));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    yield return (object) null;
  }

  public void gear1Button()
  {
    if (this.isPushAndSet())
      return;
    Unit0044Scene.ChangeScene(true, this.playerUnit, 1);
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void gear2Button()
  {
    if (this.isPushAndSet())
      return;
    Unit0044Scene.ChangeScene(true, this.playerUnit, 2);
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton()
  {
    if (this.isPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
