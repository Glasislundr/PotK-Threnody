// Decompiled with JetBrains decompiler
// Type: Tower029ManualMenu
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
public class Tower029ManualMenu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel sceneTitle;
  [SerializeField]
  private NGxScrollMasonry Scroll;

  public IEnumerator InitializeAsync()
  {
    this.sceneTitle.SetTextLocalize(Consts.GetInstance().TOWER_MANUAL_TITLE);
    TowerHowto towerHowto = ((IEnumerable<TowerHowto>) MasterData.TowerHowtoList).FirstOrDefault<TowerHowto>((Func<TowerHowto, bool>) (x => x.kind == 1));
    TowerHowto[] array = ((IEnumerable<TowerHowto>) MasterData.TowerHowtoList).Where<TowerHowto>((Func<TowerHowto, bool>) (x => x.kind >= 2)).ToArray<TowerHowto>();
    IEnumerator e = DetailController.Init(this.Scroll, towerHowto == null ? string.Empty : towerHowto.body, array);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void onBackButton() => this.backScene();
}
