// Decompiled with JetBrains decompiler
// Type: Help0151Button
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Help0151Button : MonoBehaviour
{
  [SerializeField]
  private UILabel txtHelp01;
  [SerializeField]
  private GameObject btnHelp;
  [SerializeField]
  private GameObject btnContact;
  public UIButton button;
  private BackButtonMenuBase _baseMenu;
  private List<HelpHelp> subCategory = new List<HelpHelp>();

  public void init(BackButtonMenuBase baseMenu) => this._baseMenu = baseMenu;

  public void setTextHelp01(HelpCategory helpCategory, bool isContact)
  {
    if (isContact)
    {
      this.btnContact.gameObject.SetActive(true);
      this.btnHelp.gameObject.SetActive(false);
      this.button = this.btnContact.GetComponent<UIButton>();
    }
    else
    {
      this.btnContact.gameObject.SetActive(false);
      this.btnHelp.gameObject.SetActive(true);
      this.button = this.btnHelp.GetComponent<UIButton>();
    }
    this.txtHelp01.SetTextLocalize(helpCategory.name);
    this.subCategory = Help0151Button.getHelpList(helpCategory);
  }

  public static List<HelpHelp> getHelpList(HelpCategory helpCategory)
  {
    return ((IEnumerable<HelpHelp>) MasterData.HelpHelpList).Where<HelpHelp>((Func<HelpHelp, bool>) (x => x.category_HelpCategory == helpCategory.ID)).OrderByDescending<HelpHelp, int>((Func<HelpHelp, int>) (x => x.priority)).ToList<HelpHelp>();
  }

  public void setTextHelp01(string title, bool isContact)
  {
    if (isContact)
    {
      this.btnContact.gameObject.SetActive(true);
      this.btnHelp.gameObject.SetActive(false);
      this.button = this.btnContact.GetComponent<UIButton>();
    }
    else
    {
      this.btnContact.gameObject.SetActive(false);
      this.btnHelp.gameObject.SetActive(true);
      this.button = this.btnHelp.GetComponent<UIButton>();
    }
    this.txtHelp01.SetTextLocalize(title);
  }

  public void IbtnHelp()
  {
    if (this._baseMenu.IsPushAndSet())
      return;
    Help0152Scene.ChangeScene(true, this.subCategory);
  }
}
