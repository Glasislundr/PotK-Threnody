// Decompiled with JetBrains decompiler
// Type: Tower029RankingHierarchy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Tower029RankingHierarchy : Tower029RankingStatus
{
  [SerializeField]
  private GameObject objNew_;
  [SerializeField]
  private UILabel txtHierarchy_;
  [SerializeField]
  private UILabel txtPoint_;
  private bool[] isNewFlags_;

  public IEnumerator coInitialize(
    int hierarchy,
    Tower029RankingStatus.StatusEnum status1st,
    bool[] newflags,
    int?[] statusvalues,
    int point)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Tower029RankingHierarchy rankingHierarchy = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    rankingHierarchy.setStatusValues(statusvalues);
    rankingHierarchy.txtHierarchy_.SetTextLocalize(hierarchy);
    rankingHierarchy.txtPoint_.SetTextLocalize(point);
    rankingHierarchy.isNewFlags_ = newflags == null || newflags.Length != 3 ? (bool[]) null : ((IEnumerable<bool>) newflags).ToArray<bool>();
    rankingHierarchy.changeStatus(status1st);
    return false;
  }

  public void changeStatus(Tower029RankingStatus.StatusEnum estatus)
  {
    this.changeDrawStatus(estatus);
    if (Object.op_Equality((Object) this.objNew_, (Object) null))
      return;
    this.objNew_.SetActive(this.isNewFlags_ != null && (Tower029RankingStatus.StatusEnum) this.isNewFlags_.Length > estatus && this.isNewFlags_[(int) estatus]);
  }
}
