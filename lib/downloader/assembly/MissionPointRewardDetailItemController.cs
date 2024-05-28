// Decompiled with JetBrains decompiler
// Type: MissionPointRewardDetailItemController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class MissionPointRewardDetailItemController : MonoBehaviour
{
  [SerializeField]
  private UILabel title;
  [SerializeField]
  private GameObject icon;
  [SerializeField]
  private GameObject objUnderline;

  public IEnumerator Init(
    PointReward pointRewardData,
    bool bDisableDragScrollView = false,
    bool bDisableUnderline = false)
  {
    MissionPointRewardDetailItemController detailItemController = this;
    if (bDisableDragScrollView)
    {
      ((Behaviour) ((Component) detailItemController).GetComponent<UIDragScrollView>()).enabled = false;
      ((Collider) ((Component) detailItemController).GetComponent<BoxCollider>()).enabled = false;
    }
    if (bDisableUnderline)
      detailItemController.objUnderline.SetActive(false);
    detailItemController.title.SetTextLocalize(pointRewardData.reward_title);
    CreateIconObject target = detailItemController.icon.GetOrAddComponent<CreateIconObject>();
    IEnumerator e = target.CreateThumbnail(pointRewardData.reward_type, pointRewardData.reward_id, pointRewardData.reward_quantity);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) target.GetIcon(), (Object) null))
    {
      foreach (UIButton componentsInChild in target.GetIcon().GetComponentsInChildren<UIButton>())
      {
        ((Behaviour) componentsInChild).enabled = false;
        BoxCollider component = ((Component) componentsInChild).GetComponent<BoxCollider>();
        if (Object.op_Inequality((Object) component, (Object) null))
          ((Collider) component).enabled = false;
      }
    }
  }
}
