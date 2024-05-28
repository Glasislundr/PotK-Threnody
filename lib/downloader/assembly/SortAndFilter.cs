// Decompiled with JetBrains decompiler
// Type: SortAndFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class SortAndFilter : BackButtonMenuBase
{
  protected Action sortAction;

  public virtual void Initialize(Action SortAction) => this.sortAction = SortAction;

  public void IbtnFilter<Type>(List<Type> list, Type e, SortAndFilterButton button)
  {
    if (list.Contains(e))
      list.Remove(e);
    else
      list.Add(e);
    this.GrayCheck<Type>(list, e, button);
  }

  public void GrayCheck<Type>(List<Type> list, Type e, SortAndFilterButton button)
  {
    if (list.Contains(e))
      button.SpriteColorGray(true);
    else
      button.SpriteColorGray(false);
  }

  public virtual void IbtnOrderBuySort(SortAndFilter.SORT_TYPE_ORDER_BUY sort)
  {
  }

  public virtual void IbtnAllFilterSelect()
  {
  }

  public virtual void IbtnFilterClear()
  {
  }

  public virtual void IbtnDicision()
  {
  }

  public virtual void IbtnClose() => Singleton<PopupManager>.GetInstance().onDismiss();

  public void IbtnNo() => this.IbtnClose();

  public override void onBackButton() => this.IbtnNo();

  public virtual void IbtnOrder()
  {
  }

  public virtual void IbtnOrderDec()
  {
  }

  public virtual void SaveData()
  {
  }

  public enum SORT_TYPE_ORDER_BUY
  {
    ASCENDING,
    DESCENDING,
  }

  public enum SORT_TYPE
  {
    NEW,
    RARE,
    NUMBER,
  }
}
