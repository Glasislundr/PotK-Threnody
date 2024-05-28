// Decompiled with JetBrains decompiler
// Type: Gacha0063hindicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Gacha0063hindicator : NGMenuBase
{
  [SerializeField]
  protected UILabel TxtGachaPt;
  [SerializeField]
  protected UILabel TxtGachaTicket;
  [SerializeField]
  protected UIPanel Uipanel;
  public List<GachaButton> gachaButton;
  public List<GachaButton> compensationGachaButton;
  public List<GameObject> dirNormalObject;
  public List<GameObject> dirCompensationObject;
  public GachaButton singleGachaButton;
  public GachaButton singleCompensationGachaButton;
  public GachaButton singleGachaButtonEx;
  public UI2DSprite singleGachaButtonExIcon;
  public GachaButton singleCompensationGachaButtonEx;
  public GameObject singleCompensationGachaButtonExObject;
  private GachaModule gachaModule;

  public GachaModule GachaModule
  {
    get => this.gachaModule;
    set => this.gachaModule = value;
  }

  public Gacha0063Menu Menu { get; set; }

  public int PrefabCount { get; set; }

  public int GachaNumber => this.gachaModule == null ? -1 : this.gachaModule.number;

  public virtual void InitGachaModuleGacha(
    Gacha0063Menu gacha0063Menu,
    GachaModule gachaModule,
    DateTime serverTime,
    UIScrollView scrollView,
    int prefabCount)
  {
  }

  public virtual void InitGachaModuleGacha(
    Gacha0063Menu menu,
    GachaModuleGacha gacha,
    GachaModule gachaModule)
  {
  }

  public virtual void ScrollCenterOnFinished()
  {
  }

  public virtual IEnumerator Set(GameObject detailPopup)
  {
    yield break;
  }

  public virtual IEnumerator TextureSet()
  {
    yield break;
  }

  public virtual void TextureClear()
  {
  }

  public virtual void PlayAnim()
  {
  }

  public virtual void EndAnim()
  {
  }

  public virtual void IbtnBuyKiseki()
  {
  }

  public virtual void IbtnGachaCharge()
  {
  }

  public virtual void IbtnGachaPt01()
  {
  }

  public virtual void IbtnGachaPt02()
  {
  }

  public virtual void IbtnGachaTicket01()
  {
  }

  public virtual void IbtnGachaTicket02()
  {
  }

  public virtual void IbtnGetList01()
  {
  }

  public virtual void IbtnGetList02()
  {
  }

  public virtual void IbtnUnitlist()
  {
  }
}
