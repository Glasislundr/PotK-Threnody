// Decompiled with JetBrains decompiler
// Type: Raid032MyRankingHierarchy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Raid032MyRankingHierarchy : Raid032MyRankingStatus
{
  [SerializeField]
  private GameObject objNew;
  [SerializeField]
  private UILabel txtHierarchy;
  [SerializeField]
  private UILabel txtPoint;
  private bool[] isNewFlags;

  public IEnumerator coInitialize(
    int hierarchy,
    Raid032MyRankingStatus.StatusEnum status1st,
    bool[] newflags,
    int?[] statusvalues,
    int point)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Raid032MyRankingHierarchy rankingHierarchy = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    rankingHierarchy.setStatusValues(statusvalues);
    rankingHierarchy.txtHierarchy.SetTextLocalize(hierarchy);
    rankingHierarchy.txtPoint.SetTextLocalize(point);
    rankingHierarchy.isNewFlags = newflags == null || newflags.Length != 2 ? (bool[]) null : ((IEnumerable<bool>) newflags).ToArray<bool>();
    rankingHierarchy.changeStatus(status1st);
    return false;
  }

  public void changeStatus(Raid032MyRankingStatus.StatusEnum estatus)
  {
    this.changeDrawStatus(estatus);
    if (Object.op_Equality((Object) this.objNew, (Object) null))
      return;
    this.objNew.SetActive(this.isNewFlags != null && (Raid032MyRankingStatus.StatusEnum) this.isNewFlags.Length > estatus && this.isNewFlags[(int) estatus]);
  }
}
