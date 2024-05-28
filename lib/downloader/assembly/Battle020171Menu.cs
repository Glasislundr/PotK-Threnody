// Decompiled with JetBrains decompiler
// Type: Battle020171Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Battle020171Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtExplanation24;
  [SerializeField]
  protected UILabel TxtLvAfter28;
  [SerializeField]
  protected UILabel TxtLvbefore28;
  [SerializeField]
  protected UILabel TxtPlayername30;
  private Action onCallback;

  private void Start()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1022");
    Singleton<NGSoundManager>.GetInstance().PlayBgm("bgm016", 1);
  }

  private void OnDestroy() => Singleton<NGSoundManager>.GetInstance().StopBgm(1);

  public void SetLv(int before, int after)
  {
    this.TxtLvbefore28.SetTextLocalize(before);
    this.TxtLvAfter28.SetTextLocalize(after);
  }

  public void SetName(string name) => this.TxtPlayername30.SetTextLocalize(name);

  public void SetExplanetion(string str) => this.TxtExplanation24.SetTextLocalize(str);

  public void SetCallback(Action callback) => this.onCallback = callback;

  public virtual void IbtnScreen()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    if (this.onCallback == null)
      return;
    this.onCallback();
  }

  public override void onBackButton() => this.showBackKeyToast();
}
