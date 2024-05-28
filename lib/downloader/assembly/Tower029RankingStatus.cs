// Decompiled with JetBrains decompiler
// Type: Tower029RankingStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Tower029RankingStatus : MonoBehaviour
{
  [SerializeField]
  protected Tower029RankingStatus.InsStatus[] status_ = new Tower029RankingStatus.InsStatus[3];

  protected void setStatusValues(int?[] values)
  {
    if (this.status_ == null || values == null || this.status_.Length != values.Length)
      return;
    for (int index = 0; index < this.status_.Length; ++index)
    {
      if (values[index].HasValue && this.status_[index] != null && !Object.op_Equality((Object) this.status_[index].txtValue_, (Object) null))
        this.status_[index].txtValue_.SetTextLocalize(values[index].Value);
    }
  }

  protected void changeDrawStatus(Tower029RankingStatus.StatusEnum estatus = Tower029RankingStatus.StatusEnum.Num, int? intValue = null)
  {
    if (this.status_ == null)
      return;
    foreach (Tower029RankingStatus.InsStatus statu in this.status_)
    {
      if (statu != null && !Object.op_Equality((Object) statu.top_, (Object) null))
      {
        statu.top_.SetActive(statu.status_ == estatus);
        if (statu.top_.activeSelf && intValue.HasValue && Object.op_Inequality((Object) statu.txtValue_, (Object) null))
          statu.txtValue_.SetTextLocalize(intValue.Value);
      }
    }
  }

  public enum StatusEnum
  {
    Speed,
    Technic,
    Damage,
    Num,
  }

  [Serializable]
  public class InsStatus
  {
    public Tower029RankingStatus.StatusEnum status_ = Tower029RankingStatus.StatusEnum.Num;
    public GameObject top_;
    public UILabel txtValue_;
  }
}
