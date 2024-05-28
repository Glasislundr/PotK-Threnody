// Decompiled with JetBrains decompiler
// Type: Popup00173Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup00173Menu : NGMenuBase
{
  public UILabel TxtPopupTitle;
  public UILabel TxtDescription1;
  public UILabel TxtDescription2;
  public UIButton IBtnOk;
  private bool Present;
  private PlayerPresent[] receiveList;

  public IEnumerator Init(PlayerPresent[] presents, int noReceiveCount = 0, bool isPresent = false)
  {
    this.Present = isPresent;
    this.receiveList = presents;
    string text1;
    string text2;
    string text3;
    if (this.Present)
    {
      text1 = Consts.GetInstance().POPUP_00173_PRESENT_TITLE;
      text2 = Consts.Format(Consts.GetInstance().POPUP_00173_PRESENT_TEXT1, (IDictionary) new Hashtable()
      {
        {
          (object) "Count",
          (object) noReceiveCount
        }
      });
      text3 = Consts.GetInstance().POPUP_00173_PRESENT_TEXT2;
    }
    else
    {
      text1 = Consts.GetInstance().POPUP_00173_NOPRESENT_TITLE;
      text2 = Consts.Format(Consts.GetInstance().POPUP_00173_NOPRESENT_TEXT1, (IDictionary) new Hashtable()
      {
        {
          (object) "Count",
          (object) presents.Length
        }
      });
      text3 = Consts.GetInstance().POPUP_00173_NOPRESENT_TEXT2;
    }
    this.TxtPopupTitle.SetTextLocalize(text1);
    this.TxtDescription1.SetTextLocalize(text2);
    this.TxtDescription2.SetTextLocalize(text3);
    yield break;
  }

  private IEnumerator ShowPopup()
  {
    Popup00173Menu popup00173Menu = this;
    Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_001_7_1__anim_popup01.Load<GameObject>();
    IEnumerator e = popupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = popupPrefabF.Result;
    Mypage00171Menu component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Mypage00171Menu>();
    popup00173Menu.StartCoroutine(component.Init(popup00173Menu.receiveList));
  }

  public virtual void IbtnOk()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    if (!this.Present)
      return;
    this.StartCoroutine(this.ShowPopup());
  }
}
