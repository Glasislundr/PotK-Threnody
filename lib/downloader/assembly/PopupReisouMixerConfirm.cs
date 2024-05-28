// Decompiled with JetBrains decompiler
// Type: PopupReisouMixerConfirm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PopupReisouMixerConfirm : BackButtonMonoBehaiviour
{
  [SerializeField]
  private UILabel m_Warning;
  [SerializeField]
  private UIGrid m_Grid;
  private Action m_YesCallback;

  public IEnumerator Init(List<InventoryItem> materials, Action yesCallback, GameObject iconPrefab = null)
  {
    IEnumerator e;
    if (Object.op_Equality((Object) iconPrefab, (Object) null))
    {
      Future<GameObject> ItemIconF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = ItemIconF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      iconPrefab = ItemIconF.Result;
      ItemIconF = (Future<GameObject>) null;
    }
    this.m_YesCallback = yesCallback;
    foreach (InventoryItem item in materials)
    {
      ItemIcon icon = iconPrefab.CloneAndGetComponent<ItemIcon>(((Component) this.m_Grid).transform);
      e = icon.InitByItemInfo(item.Item);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      icon.SetRenseiIcon(false, false);
      icon.SetRenseiMaterialNum(item.Item.isTempSelectedCount ? item.Item.tempSelectedCount : 1);
      icon = (ItemIcon) null;
    }
    this.m_Grid.repositionNow = true;
  }

  public void IbtnYes()
  {
    if (this.m_YesCallback == null)
      return;
    this.m_YesCallback();
  }

  public void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();
}
