// Decompiled with JetBrains decompiler
// Type: SheetGachaGetBonus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class SheetGachaGetBonus : MonoBehaviour
{
  public GameObject IconObject;
  public UILabel CompleteText;
  [SerializeField]
  private SheetGachaAnim anim;

  public IEnumerator Init(GachaG007PlayerPanel panel, Action endAction)
  {
    CreateIconObject target = this.IconObject.GetOrAddComponent<CreateIconObject>();
    if (panel.reward_type_id.HasValue)
    {
      IEnumerator e = target.CreateThumbnail((MasterDataTable.CommonRewardType) panel.reward_type_id.Value, panel.reward_id.HasValue ? panel.reward_id.Value : 0, panel.reward_quantity.HasValue ? panel.reward_quantity.Value : 0);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (panel.reward_type_id.Value == 1 || panel.reward_type_id.Value == 24)
      {
        target.GetIcon().GetComponent<UnitIcon>().setLevelText("1");
        target.GetIcon().GetComponent<UnitIcon>().ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
      }
      else if (panel.reward_type_id.Value != 3 && panel.reward_type_id.Value != 26 && panel.reward_type_id.Value != 35)
      {
        int num = panel.reward_type_id.Value;
      }
      this.CompleteText.SetTextLocalize(CommonRewardType.GetRewardName((MasterDataTable.CommonRewardType) panel.reward_type_id.Value, panel.reward_id.HasValue ? panel.reward_id.Value : 0, panel.reward_quantity.HasValue ? panel.reward_quantity.Value : 0));
      NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
      if (Object.op_Inequality((Object) instance, (Object) null))
        instance.playSE("SE_0546");
    }
    this.anim.Init(endAction);
  }
}
