// Decompiled with JetBrains decompiler
// Type: Mypage00176Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Mypage00176Menu : BackButtonMenuBase
{
  [SerializeField]
  protected GameObject[] BtnProtected;
  [SerializeField]
  public UIButton BtnDelete;
  [SerializeField]
  protected UILabel TxtDescription;
  [SerializeField]
  protected UILabel TxtPopuptitle;
  private Mypage0017Menu menu0017;
  private PlayerPresent present;

  public IEnumerator Init(PlayerPresent presents, Mypage0017Menu menu)
  {
    this.menu0017 = menu;
    this.present = presents;
    this.TxtPopuptitle.SetTextLocalize(this.present.title);
    this.TxtDescription.SetTextLocalize(presents.message);
    yield break;
  }

  private IEnumerator DeleteDialog()
  {
    Mypage00176Menu mypage00176Menu = this;
    Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_001_7_9__anim_popup01.Load<GameObject>();
    IEnumerator e = popupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = popupPrefabF.Result;
    Popup00179Menu component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Popup00179Menu>();
    mypage00176Menu.StartCoroutine(component.Init(mypage00176Menu.present, mypage00176Menu.menu0017));
  }

  public virtual void IbtnPopupDelete() => this.StartCoroutine(this.DeleteDialog());

  public virtual void IbtnPopupOk()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnPopupOk();
}
