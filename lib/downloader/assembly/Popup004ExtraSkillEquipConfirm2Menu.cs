// Decompiled with JetBrains decompiler
// Type: Popup004ExtraSkillEquipConfirm2Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup004ExtraSkillEquipConfirm2Menu : BackButtonPopupBase
{
  [SerializeField]
  private ExtraSkillInfo beforeExtraSkillInfo;
  [SerializeField]
  private ExtraSkillInfo afterExtraSkillInfo;
  private Action<PlayerAwakeSkill, PlayerAwakeSkill, PlayerUnit> decideAction;
  private PlayerAwakeSkill beforeSkill;
  private PlayerAwakeSkill afterSkill;
  private PlayerUnit targetUnit;

  public IEnumerator Init(
    PlayerAwakeSkill beforeSkill,
    PlayerAwakeSkill afterSkill,
    PlayerUnit unit,
    Action<PlayerAwakeSkill, PlayerAwakeSkill, PlayerUnit> decideAct)
  {
    Popup004ExtraSkillEquipConfirm2Menu equipConfirm2Menu = this;
    equipConfirm2Menu.setTopObject(((Component) equipConfirm2Menu).gameObject);
    equipConfirm2Menu.beforeSkill = beforeSkill;
    equipConfirm2Menu.afterSkill = afterSkill;
    equipConfirm2Menu.targetUnit = unit;
    equipConfirm2Menu.decideAction = decideAct;
    IEnumerator e = equipConfirm2Menu.beforeExtraSkillInfo.Init(beforeSkill, beforeSkill.favorite, (Sprite) null, (GameObject) null);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = equipConfirm2Menu.afterExtraSkillInfo.Init(afterSkill, afterSkill.favorite, (Sprite) null, (GameObject) null);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void IbtnDecide()
  {
    if (this.decideAction != null)
      this.decideAction(this.beforeSkill, this.afterSkill, this.targetUnit);
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void IbtnCancle() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnCancle();
}
