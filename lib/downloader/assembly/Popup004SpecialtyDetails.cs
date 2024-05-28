// Decompiled with JetBrains decompiler
// Type: Popup004SpecialtyDetails
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Popup004SpecialtyDetails : BackButtonMonoBehaiviour
{
  [SerializeField]
  private List<UILabel> lblParameterList;
  [SerializeField]
  private UILabel lblName;
  [SerializeField]
  private UILabel lblDesc;
  private bool isPush;

  private bool isPushAndSet()
  {
    if (this.isPush)
      return true;
    this.isPush = true;
    return false;
  }

  public IEnumerator Init(ComposeMaxUnityValueSetting values)
  {
    // ISSUE: reference to a compiler-generated field
    int num1 = this.\u003C\u003E1__state;
    Popup004SpecialtyDetails specialtyDetails = this;
    if (num1 != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (Object.op_Inequality((Object) ((Component) specialtyDetails).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) specialtyDetails).GetComponent<UIWidget>()).alpha = 0.0f;
    specialtyDetails.lblName.SetTextLocalize(values.name);
    specialtyDetails.lblDesc.SetTextLocalize(values.description);
    List<string> stringList = new List<string>()
    {
      values.hp_compose_add_max,
      values.strength_compose_add_max,
      values.intelligence_compose_add_max,
      values.vitality_compose_add_max,
      values.mind_compose_add_max,
      values.agility_compose_add_max,
      values.dexterity_compose_add_max,
      values.lucky_compose_add_max
    };
    for (int index = 0; index < stringList.Count; ++index)
    {
      int num2;
      if (string.IsNullOrEmpty(stringList[index]))
        num2 = 0;
      else
        num2 = stringList[index].Split(',').Length;
      specialtyDetails.lblParameterList[index].SetTextLocalize(string.Format("+{0}", (object) num2));
    }
    return false;
  }

  public override void onBackButton()
  {
    if (this.isPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
