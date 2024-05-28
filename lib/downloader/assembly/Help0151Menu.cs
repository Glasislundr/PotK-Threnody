// Decompiled with JetBrains decompiler
// Type: Help0151Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Help0151Menu : BackButtonMenuBase
{
  [SerializeField]
  protected GameObject ScrollGrid;
  public NGxScroll scroll;
  public UIScrollView scrollview;
  private GameObject barPrefab;
  private const int FGGID_ID = 19;

  public virtual void Foreground()
  {
  }

  public virtual void IbtnHelp()
  {
  }

  public virtual void IbtnHelp2()
  {
  }

  public virtual void IbtnHelp3()
  {
  }

  public virtual void IbtnHelp4()
  {
  }

  public virtual void IbtnHelp5()
  {
  }

  public virtual void IbtnHelp6()
  {
  }

  public virtual void VScrollBar()
  {
  }

  public IEnumerator InitHelp()
  {
    Help0151Menu baseMenu = this;
    ((IEnumerable<HelpCategory>) MasterData.HelpCategoryList).OrderBy<HelpCategory, int>((Func<HelpCategory, int>) (x => x.priority));
    List<HelpHelp> categoryList = new List<HelpHelp>();
    Future<GameObject> prefabF = Res.Prefabs.help015_1.vscrollhelp1_682_33.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    baseMenu.barPrefab = prefabF.Result;
    categoryList.Clear();
    baseMenu.scroll.Clear();
    foreach (HelpCategory helpCategory in (IEnumerable<HelpCategory>) ((IEnumerable<HelpCategory>) MasterData.HelpCategoryList).Where<HelpCategory>((Func<HelpCategory, bool>) (x => x.priority > 0)).OrderByDescending<HelpCategory, int>((Func<HelpCategory, int>) (x => x.priority)))
    {
      if (helpCategory.ID != 19)
      {
        GameObject gameObject = baseMenu.barPrefab.Clone();
        baseMenu.scroll.Add(gameObject);
        Help0151Button component = gameObject.GetComponent<Help0151Button>();
        component.init((BackButtonMenuBase) baseMenu);
        component.setTextHelp01(helpCategory, false);
      }
    }
    baseMenu.CreateContactBar();
    baseMenu.scroll.grid.Reposition();
    baseMenu.scroll.scrollView.ResetPosition();
  }

  private void CreateContactBar()
  {
    GameObject gameObject = this.barPrefab.Clone();
    this.scroll.Add(gameObject);
    Help0151Button component = gameObject.GetComponent<Help0151Button>();
    component.setTextHelp01(Consts.GetInstance().HELP_CONTACT, true);
    EventDelegate.Set(component.button.onClick, new EventDelegate.Callback(this.ChangeContactScene));
  }

  private void ChangeContactScene() => Help0154Scene.ChangeScene(true);

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();
}
