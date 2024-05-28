// Decompiled with JetBrains decompiler
// Type: Popup004ExtraSkillEquipedByOthers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup004ExtraSkillEquipedByOthers : BackButtonMenuBase
{
  [SerializeField]
  private GameObject linkCharacter;
  [SerializeField]
  private UILabel txtDescription2;
  private Action<PlayerAwakeSkill, PlayerUnit, PlayerUnit> decideAction;
  private PlayerAwakeSkill targetSkill;
  private PlayerUnit targetUnit;
  private PlayerUnit nowEquipmentUnit;

  public IEnumerator Init(
    PlayerAwakeSkill skill,
    PlayerUnit nowEquipUnit,
    PlayerUnit targetUnit,
    Action<PlayerAwakeSkill, PlayerUnit, PlayerUnit> decideAct)
  {
    this.targetSkill = skill;
    this.targetUnit = targetUnit;
    this.nowEquipmentUnit = nowEquipUnit;
    this.decideAction = decideAct;
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIcon unitIconScript = prefabF.Result.CloneAndGetComponent<UnitIcon>(this.linkCharacter.transform);
    UnitUnit unit = nowEquipUnit.unit;
    e = unitIconScript.SetUnit(unit, unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitIconScript.SetRarities(nowEquipUnit);
    unitIconScript.setLevelText(nowEquipUnit);
    unitIconScript.PlayerUnit = nowEquipUnit;
    unitIconScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    this.txtDescription2.SetTextLocalize(Consts.Format(Consts.GetInstance().popup_004_ExtraSkillEquipByOthers_Description_text, (IDictionary) new Hashtable()
    {
      {
        (object) "name",
        (object) nowEquipUnit.unit.name
      }
    }));
  }

  public void IbtnDecide()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    if (this.decideAction == null)
      return;
    this.decideAction(this.targetSkill, this.nowEquipmentUnit, this.targetUnit);
  }

  public void IbtnCancle() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnCancle();
}
