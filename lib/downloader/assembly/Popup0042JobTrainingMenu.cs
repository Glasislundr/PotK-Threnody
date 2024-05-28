// Decompiled with JetBrains decompiler
// Type: Popup0042JobTrainingMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Popup0042JobTrainingMenu : BackButtonMenuBase
{
  private GameObject dirJobTrainingMenuPrefab;
  [SerializeField]
  private UIGrid buttonsGrid;
  [SerializeField]
  private UIScrollView scrollview;
  private GameObject button;
  private GameObject attackMethodDialog;
  private GameObject attackMethodDialogPrefab;

  public IEnumerator Init(UnitUnit unit, int playerJobId)
  {
    bool isSea = Singleton<NGGameDataManager>.GetInstance().IsSea;
    Future<GameObject> loader = (Future<GameObject>) null;
    loader = isSea ? Res.Prefabs.unit004_2_sea.dir_JobTrainingMenu_sea.Load<GameObject>() : Res.Prefabs.unit004_2.dir_JobTrainingMenu.Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (loader != null)
    {
      this.button = loader.Result;
      loader = isSea ? Res.Prefabs.unit004_2_sea.AttackMethodDialog_sea.Load<GameObject>() : Res.Prefabs.unit004_2.AttackMethodDialog.Load<GameObject>();
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (loader != null)
      {
        this.attackMethodDialogPrefab = loader.Result;
        IEnumerable<AttackMethod> attackMethods = ((IEnumerable<AttackMethod>) MasterData.AttackMethodList).Where<AttackMethod>((Func<AttackMethod, bool>) (x => x.unit == unit && x.job.ID == playerJobId));
        if (attackMethods != null)
        {
          foreach (AttackMethod attackMethod in attackMethods)
          {
            BattleskillSkill skill = attackMethod.skill;
            if (skill != null)
            {
              GearAttackClassification attackClass = GearAttackClassification.none;
              if (skill.skill_type == BattleskillSkillType.magic)
              {
                attackClass = GearAttackClassification.magic;
              }
              else
              {
                BattleskillEffect battleskillEffect = ((IEnumerable<BattleskillEffect>) skill.Effects).FirstOrDefault<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.attack_classification));
                if (battleskillEffect != null)
                  attackClass = (GearAttackClassification) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.attack_classification_id);
              }
              e = this.SetBtnFunction(skill.name, true, skill, attackClass);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
            }
          }
        }
      }
    }
  }

  public IEnumerator ResetScroll()
  {
    this.buttonsGrid.Reposition();
    yield return (object) new WaitForEndOfFrame();
    this.scrollview.ResetPosition();
  }

  private IEnumerator CreateButton(string txt, bool active, Action callFunction)
  {
    GameObject btn = this.button.Clone(((Component) this.buttonsGrid).transform);
    btn.SetActive(false);
    DetailJobTraining component = btn.GetComponent<DetailJobTraining>();
    component.dragScrollView.scrollView = this.scrollview;
    IEnumerator e = component.Init(txt, active, (Action) (() => callFunction()));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    btn.SetActive(true);
  }

  private IEnumerator SetBtnFunction(
    string btnTitle,
    bool active,
    BattleskillSkill skill,
    GearAttackClassification attackClass)
  {
    IEnumerator e = this.CreateButton(btnTitle, active, (Action) (() => this.ShowCallDialogPrefab(skill, attackClass)));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void ShowCallDialogPrefab(BattleskillSkill skill, GearAttackClassification attackClass)
  {
    if (Object.op_Equality((Object) this.attackMethodDialog, (Object) null))
    {
      this.attackMethodDialog = this.attackMethodDialogPrefab.Clone(((Component) this).transform);
      this.attackMethodDialog.GetComponentInChildren<UIPanel>().depth += 30;
      this.attackMethodDialog.SetActive(false);
    }
    DetailAttackMenuDialog componentInChildren = this.attackMethodDialog.GetComponentInChildren<DetailAttackMenuDialog>();
    if (Object.op_Equality((Object) componentInChildren, (Object) null))
      return;
    componentInChildren.setSkillProperty(false);
    componentInChildren.setData(skill, "", attackClass);
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    componentInChildren.Show(true);
    this.attackMethodDialog.SetActive(true);
  }

  public virtual void IbtnOK() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnOK();
}
