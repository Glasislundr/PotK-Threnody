// Decompiled with JetBrains decompiler
// Type: Versus02611Menu
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
public class Versus02611Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private NGxScroll scroll;
  private bool is_initialized;

  public IEnumerator Init(WebAPI.Response.PvpBoot pvpInfo)
  {
    if (!this.is_initialized)
    {
      this.txtTitle.SetText(Consts.GetInstance().VERSUS_002611TITLE);
      this.scroll.Clear();
      Future<GameObject> scF = Res.Prefabs.versus026_11.dir_Rank_List.Load<GameObject>();
      IEnumerator e = scF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      IOrderedEnumerable<PvpClassKind> classList = ((IEnumerable<PvpClassKind>) MasterData.PvpClassKindList).OrderByDescending<PvpClassKind, int>((Func<PvpClassKind, int>) (x => x.weight));
      foreach (PvpClassKind pvpClassKind in (IEnumerable<PvpClassKind>) classList)
      {
        Versus02611ClassList component = scF.Result.Clone().GetComponent<Versus02611ClassList>();
        this.scroll.Add(((Component) component).gameObject);
        e = component.Init(pvpClassKind.ID, pvpClassKind.name, pvpInfo.current_class, pvpInfo.best_class + 1 < pvpClassKind.ID, pvpInfo.best_class);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      this.scroll.ResolvePosition(classList.Count<PvpClassKind>() - pvpInfo.current_class);
      this.is_initialized = true;
    }
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();
}
