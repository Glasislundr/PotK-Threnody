// Decompiled with JetBrains decompiler
// Type: Shop00718Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class Shop00718Menu : BackButtonMenuBase
{
  [SerializeField]
  private UITextList txtList;
  [SerializeField]
  protected UILabel TxtPopuptitle;
  [SerializeField]
  protected UILabel TxtPopuptitle2;
  [SerializeField]
  protected UILabel TxtREADME;
  private int m_windowHeight;
  private int m_windowWidth;

  public void setData() => this.txtList.Add(Consts.GetInstance().COMMISSION_ON_TRADE);

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  protected override void Update()
  {
    if (this.m_windowHeight == 0 || this.m_windowWidth == 0)
    {
      this.m_windowHeight = Screen.height;
      this.m_windowWidth = Screen.width;
    }
    else if (this.m_windowHeight != Screen.height || this.m_windowWidth != Screen.width)
    {
      Debug.Log((object) "Window size change detected.");
      this.txtList.Clear();
      this.setData();
      this.m_windowHeight = Screen.height;
      this.m_windowWidth = Screen.width;
    }
    base.Update();
  }
}
