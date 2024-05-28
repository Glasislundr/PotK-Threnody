// Decompiled with JetBrains decompiler
// Type: Popup004ExtraSkillEquipConfirm1Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup004ExtraSkillEquipConfirm1Menu : BackButtonPopupBase
{
  [SerializeField]
  private ExtraSkillInfo extraSkillInfo;
  private Action<PlayerAwakeSkill, PlayerUnit> decideAction;
  private PlayerAwakeSkill targetSkill;
  private PlayerUnit targetUnit;
  private GameObject skillDetailPrefab;

  public IEnumerator Init(
    PlayerAwakeSkill skill,
    PlayerUnit unit,
    Action<PlayerAwakeSkill, PlayerUnit> decideAct)
  {
    Popup004ExtraSkillEquipConfirm1Menu equipConfirm1Menu = this;
    equipConfirm1Menu.setTopObject(((Component) equipConfirm1Menu).gameObject);
    equipConfirm1Menu.targetSkill = skill;
    equipConfirm1Menu.targetUnit = unit;
    equipConfirm1Menu.decideAction = decideAct;
    IEnumerator e = equipConfirm1Menu.extraSkillInfo.Init(skill, skill.favorite, (Sprite) null, (GameObject) null);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) equipConfirm1Menu.skillDetailPrefab, (Object) null))
    {
      Future<GameObject> loader = PopupSkillDetails.createPrefabLoader(false);
      yield return (object) loader.Wait();
      equipConfirm1Menu.skillDetailPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
  }

  public void IbtnDecide()
  {
    if (this.IsPushAndSet())
      return;
    if (this.decideAction != null)
      this.decideAction(this.targetSkill, this.targetUnit);
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void IbtnCancle()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnCancle();

  public void onClickedSkillZoom()
  {
    if (this.targetSkill == null || Object.op_Equality((Object) this.skillDetailPrefab, (Object) null) || this.IsPushAndSet())
      return;
    PopupSkillDetails.show(this.skillDetailPrefab, PopupSkillDetails.Param.createBySkillView(this.targetSkill), onClosed: (Action) (() => this.IsPush = false));
  }
}
