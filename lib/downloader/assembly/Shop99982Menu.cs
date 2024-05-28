// Decompiled with JetBrains decompiler
// Type: Shop99982Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop99982Menu : BackButtonMenuBase
{
  private GameObject prefab99983;
  public string year;
  public string month;
  public string day;

  private void update()
  {
  }

  public void Init(string year, string month, string day)
  {
    this.year = year;
    this.month = month;
    this.day = day;
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    PurchaseBehavior.PopupDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public void IbtnPopupYes()
  {
    PurchaseBehavior.SendAge(int.Parse(this.year), int.Parse(this.month), int.Parse(this.day));
    PurchaseBehavior.PopupDismiss();
    PurchaseBehavior.PopupDismiss();
  }

  private IEnumerator popup99983()
  {
    string year = this.year.ToConverter();
    string month = this.month.ToConverter();
    string day = this.day.ToConverter();
    Future<GameObject> prefab = Res.Prefabs.popup.popup_999_8_3__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefab99983 = Singleton<PopupManager>.GetInstance().open(prefab.Result);
    this.prefab99983.GetComponent<SetPopupText>().SetText(string.Format(Consts.GetInstance().SHOP_99982_MENU, (object) year, (object) month, (object) day), false);
  }
}
