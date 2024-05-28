// Decompiled with JetBrains decompiler
// Type: Popup00631Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup00631Menu : BackButtonMenuBase
{
  [SerializeField]
  private NGxScrollMasonry Scroll;
  [SerializeField]
  private UILabel popupTitle;

  public IEnumerator InitGachaDetail(string title, GachaDescriptionBodies[] bodys)
  {
    IEnumerator e = DetailController.Init(this.Scroll, title, bodys);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitGachaDetail(string title, CoinProductDetail[] bodys)
  {
    this.popupTitle.text = title;
    IEnumerator e = DetailController.Init(this.Scroll, "", bodys);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitGachaDetail(string title, CoinBonusDetail[] bodys)
  {
    this.popupTitle.text = title;
    IEnumerator e = DetailController.Init(this.Scroll, "", bodys);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitGachaDetail(string title, SimplePackDescription[] bodys)
  {
    this.popupTitle.text = title;
    IEnumerator e = DetailController.Init(this.Scroll, "", bodys);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitGachaDetail(string title, BeginnerPackDescription[] bodys)
  {
    this.popupTitle.text = title;
    IEnumerator e = DetailController.Init(this.Scroll, "", bodys);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitGachaDetail(string title, StepupPackDescription[] bodys)
  {
    this.popupTitle.text = title;
    IEnumerator e = DetailController.Init(this.Scroll, "", bodys);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitGachaDetail(string title, WeeklyPackDescription[] bodys)
  {
    this.popupTitle.text = title;
    IEnumerator e = DetailController.Init(this.Scroll, "", bodys);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitGachaDetail(string title, MonthlyPackDescription[] bodys)
  {
    this.popupTitle.text = title;
    IEnumerator e = DetailController.Init(this.Scroll, "", bodys);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitGachaDetail(string title, PackDescription[] bodys)
  {
    this.popupTitle.text = title;
    IEnumerator e = DetailController.Init(this.Scroll, "", bodys);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void IbtnNo()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    DetailController.Release();
  }

  public override void onBackButton() => this.IbtnNo();
}
