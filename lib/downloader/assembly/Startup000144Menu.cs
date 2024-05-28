// Decompiled with JetBrains decompiler
// Type: Startup000144Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using UnityEngine;

#nullable disable
public class Startup000144Menu : BackButtonMonoBehaiviour
{
  [SerializeField]
  protected UILabel TxtDescription01;
  [SerializeField]
  protected UILabel TxtDescription02;
  [SerializeField]
  protected UILabel TxtDescription03;
  [SerializeField]
  protected UILabel TxtDescription04;
  [SerializeField]
  protected UILabel TxtPopuptitle;
  [SerializeField]
  protected UILabel TxtREADME;
  public UIButton ibtn_ok;
  [SerializeField]
  private GameObject description03;
  [SerializeField]
  private GameObject description04;
  private Action onOkButton;
  private bool isPush;

  public Action OnOkButton
  {
    get => this.onOkButton;
    set => this.onOkButton = value;
  }

  private bool IsPushAndSet()
  {
    if (this.isPush)
      return true;
    this.isPush = true;
    return false;
  }

  public void InitScene(PlayerLoginBonus loginBonus)
  {
    this.TxtPopuptitle.SetText(loginBonus.loginbonus.name);
    this.TxtDescription01.SetTextLocalize(loginBonus.rewards[0].client_reward_message);
    ((Component) this.TxtDescription01).gameObject.SetActive(true);
    ((Component) this.TxtDescription02).gameObject.SetActive(false);
    ((Component) this.TxtDescription03).gameObject.SetActive(false);
    ((Component) this.TxtDescription04).gameObject.SetActive(false);
    Singleton<NGGameDataManager>.GetInstance().loginBonuses.Remove(loginBonus);
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet() || this.onOkButton == null)
      return;
    this.onOkButton();
  }
}
