// Decompiled with JetBrains decompiler
// Type: DetailMenuScrollViewInfoOption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class DetailMenuScrollViewInfoOption : DetailMenuScrollViewBase
{
  [SerializeField]
  private DetailMenuScrollViewInfo menu_;
  [SerializeField]
  [Tooltip("武器表示")]
  private DetailMenuScrollViewInfo.WeaponRow[] weapons_;

  public override IEnumerator initAsync(
    PlayerUnit playerUnit,
    bool limitMode,
    bool isMaterial,
    GameObject[] prefabs)
  {
    if (this.weapons_ != null && this.weapons_.Length != 0)
    {
      int index1 = 0;
      IAttackMethod[] iattackMethodArray = playerUnit.battleOptionAttacks;
      for (int index2 = 0; index2 < iattackMethodArray.Length; ++index2)
      {
        IAttackMethod attack = iattackMethodArray[index2];
        if (this.weapons_.Length > index1)
        {
          IEnumerator e = this.menu_.initializeOptionAttack(this.weapons_[index1++], attack);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else
          break;
      }
      iattackMethodArray = (IAttackMethod[]) null;
      for (; index1 < this.weapons_.Length; ++index1)
        this.weapons_[index1].top_.SetActive(false);
    }
  }
}
