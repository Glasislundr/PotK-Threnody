// Decompiled with JetBrains decompiler
// Type: PopupCommonNoYes2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Linq;
using UnityEngine;

#nullable disable
public class PopupCommonNoYes2 : BackButtonMonoBehaiviour
{
  [SerializeField]
  protected UILabel title;
  [SerializeField]
  protected UILabel message;
  [SerializeField]
  private UILabel message02;
  private bool callbackAfterClose;
  private Action yes;
  private Action no;

  public void OnYes()
  {
    if (!this.callbackAfterClose)
      this.yes();
    Singleton<PopupManager>.GetInstance().dismiss();
    if (!this.callbackAfterClose)
      return;
    this.yes();
  }

  public void IbtnNo()
  {
    if (!this.callbackAfterClose)
      this.no();
    Singleton<PopupManager>.GetInstance().dismiss();
    if (!this.callbackAfterClose)
      return;
    this.no();
  }

  public override void onBackButton() => this.IbtnNo();

  public void SetMessageAligment(NGUIText.Alignment alignment)
  {
    this.message.alignment = alignment;
  }

  public void SetDelegate(Action yes = null, Action no = null)
  {
    this.yes = yes ?? new Action(PopupCommonNoYes2.defaultCallback);
    this.no = no ?? new Action(PopupCommonNoYes2.defaultCallback);
  }

  public static void Show(
    string title,
    string message,
    Action yes = null,
    Action no = null,
    NGUIText.Alignment alignment = 2,
    string messageB = null,
    NGUIText.Alignment alignmentB = 1,
    bool callbackAfterClose = false)
  {
    GameObject prefab = Resources.Load<GameObject>("Prefabs/popup_common_no_yes_2");
    PopupCommonNoYes2 component = Singleton<PopupManager>.GetInstance().open(prefab).GetComponent<PopupCommonNoYes2>();
    if (Object.op_Inequality((Object) ((Component) component).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) component).GetComponent<UIWidget>()).alpha = 0.0f;
    component.title.SetText(title);
    component.message.SetText(message);
    component.SetMessageAligment(alignment);
    int count1 = !string.IsNullOrEmpty(component.message.text) ? component.message.text.Count<char>((Func<char, bool>) (c => c == '\n')) + 1 : 0;
    if (!string.IsNullOrEmpty(messageB) && Object.op_Inequality((Object) component.message02, (Object) null))
    {
      ((Component) component.message02).gameObject.SetActive(true);
      component.message02.SetText(messageB);
      component.message02.alignment = alignmentB;
      int count2 = component.message02.text.Count<char>((Func<char, bool>) (c => c == '\n')) + 1;
      component.message.SetText(component.message.text + new string('\n', count2));
      component.message02.SetText(new string('\n', count1) + component.message02.text);
    }
    else if (count1 > 0 && count1 < 5)
      component.message.SetText(component.message.text + new string('\n', 5 - count1));
    component.callbackAfterClose = callbackAfterClose;
    component.SetDelegate(yes, no);
  }

  public static void defaultCallback()
  {
  }
}
