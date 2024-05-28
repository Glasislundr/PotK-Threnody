// Decompiled with JetBrains decompiler
// Type: Shop99981Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop99981Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UIInput nen;
  [SerializeField]
  protected UIInput getsu;
  [SerializeField]
  protected UIInput hi;
  [SerializeField]
  private UILabel TxtDescription01;
  [SerializeField]
  private UILabel TxtDescription02;
  [SerializeField]
  private UILabel TxtDescription04;
  [SerializeField]
  private UILabel TxtDescription06;
  [SerializeField]
  private UILabel TxtPopuptitle;
  private Action onCancel;

  public void SetOnCancel(Action callback) => this.onCancel = callback;

  private void Awake()
  {
    this.nen.caretColor = Color.black;
    this.getsu.caretColor = Color.black;
    this.hi.caretColor = Color.black;
  }

  private void update()
  {
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    PurchaseBehavior.PopupDismiss();
    PurchaseBehavior.PopupDismiss();
    this.onCancel();
  }

  public override void onBackButton() => this.IbtnNo();

  public void IbtnPopupDecide()
  {
    if (this.IsPushAndSet())
      return;
    string str1 = this.nen.value;
    string str2 = this.getsu.value;
    string day = this.hi.value;
    if (DateCheck.YearCheck(str1) && DateCheck.MonthCheck(str2))
    {
      if (DateTime.TryParse(str1 + "/" + str2 + "/" + day, out DateTime _))
      {
        this.StartCoroutine(this.popup99982(str1, str2, day));
        return;
      }
    }
    PurchaseBehavior.ShowPopupWithMessage(Consts.GetInstance().SHOP_99981_MENU_01, Consts.GetInstance().SHOP_99981_MENU_02);
    this.IsPush = false;
  }

  public void Shop99981Visible(bool b)
  {
    ((Component) ((Component) this).transform.Find("MainPanel")).gameObject.SetActive(b);
  }

  private IEnumerator popup99982(string year, string month, string day)
  {
    Shop99981Menu shop99981Menu = this;
    Future<GameObject> prefab = Res.Prefabs.popup.popup_999_8_2__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = PurchaseBehavior.PopupOpen(prefab.Result);
    gameObject.GetComponent<SetPopupText>().SetText(string.Format(Consts.GetInstance().SHOP_99981_MENU_03, (object) year, (object) month, (object) day));
    gameObject.GetComponent<Shop99982Menu>().Init(year, month, day);
    shop99981Menu.IsPush = false;
  }
}
