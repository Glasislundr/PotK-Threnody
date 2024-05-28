// Decompiled with JetBrains decompiler
// Type: BattleUI01PopupAilmentsDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BattleUI01PopupAilmentsDetail : BattleBackButtonMenuBase
{
  public UIGrid grid;
  public UIScrollView scrollview;
  private GameObject dirAilmentsDetailPrefab;

  public IEnumerator Init(IEnumerable<BattleFuncs.InvestSkill> skills)
  {
    Future<GameObject> dirAilmentsDetailPrefabF = Res.Prefabs.battle.dir_Ailments_Detail.Load<GameObject>();
    IEnumerator e = dirAilmentsDetailPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dirAilmentsDetailPrefab = dirAilmentsDetailPrefabF.Result;
    this.scrollview.ResetPosition();
    foreach (BattleFuncs.InvestSkill skill in skills)
    {
      e = this.dirAilmentsDetailPrefab.Clone(((Component) this.grid).transform).GetComponent<BattleUI01DirAilmentsDetail>().Init(skill);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public override void onBackButton() => this.IbtnNo();

  public void IbtnNo() => this.battleManager.popupDismiss();
}
