// Decompiled with JetBrains decompiler
// Type: Unit004JobAfter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class Unit004JobAfter : MonoBehaviour
{
  [SerializeField]
  private UILabel txtJobDescription;
  [SerializeField]
  private UILabel txtJobDescriptionNoBtn;
  [SerializeField]
  private GameObject[] dirModeRoot;
  [Header("group 1")]
  [SerializeField]
  private UILabel txtJobNameGrp1;
  [SerializeField]
  private UILabel txtActualLevelgrp1;
  [SerializeField]
  private UILabel txtNextLevelLeft;
  [SerializeField]
  private UILabel txtNextLevelRight;
  [Header("group 2")]
  [SerializeField]
  private UILabel txtJobNameGrp2;
  [SerializeField]
  private UILabel txtActualLevelgrp2;
  [SerializeField]
  private GameObject[] dynSkillGenreIcons2;
  [Header("group 3")]
  [SerializeField]
  private UILabel txtJobNameGrp3;
  [SerializeField]
  private UILabel txtActualLevelgrp3;
  [SerializeField]
  private GameObject[] dynSkillGenreIcons3;
  [SerializeField]
  private GameObject objSkillZoom;
  private Action popupSkillDetail;
  private GameObject skillDetailPrefab;

  public IEnumerator Init(
    int groupMode,
    PlayerUnitJob_abilities jobAbility,
    PlayerUnitJob_abilities beforeAbility = null,
    bool bActiveSkillZoom = false)
  {
    JobCharacteristics master = jobAbility.master;
    if (master != null)
    {
      BattleskillSkill skill = master.skill;
      this.objSkillZoom.SetActive(false);
      Future<GameObject> loader;
      if (bActiveSkillZoom)
      {
        loader = PopupSkillDetails.createPrefabLoader(Singleton<NGGameDataManager>.GetInstance().IsSea);
        yield return (object) loader.Wait();
        this.skillDetailPrefab = loader.Result;
        loader = (Future<GameObject>) null;
      }
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      {
        groupMode = Math.Min(groupMode, 2);
        if (bActiveSkillZoom)
        {
          this.txtJobDescription.SetTextLocalize(skill.description);
        }
        else
        {
          ((Component) this.txtJobDescription).gameObject.SetActive(false);
          this.txtJobDescriptionNoBtn.SetTextLocalize(skill.description);
          ((Component) this.txtJobDescriptionNoBtn).gameObject.SetActive(true);
        }
      }
      else
        this.txtJobDescription.SetTextLocalize(skill.description);
      ((IEnumerable<GameObject>) this.dirModeRoot).ToggleOnce(groupMode - 1);
      IEnumerator e;
      switch (groupMode)
      {
        case 1:
          int num = beforeAbility != null ? beforeAbility.level : jobAbility.level;
          int nextLevel = beforeAbility != null ? jobAbility.level : jobAbility.level + 1;
          this.txtJobNameGrp1.SetTextLocalize(skill.name);
          this.txtActualLevelgrp1.SetTextLocalize(Consts.Format(Consts.GetInstance().extra_skillI_thumb_skill_level_text, (IDictionary) new Hashtable()
          {
            {
              (object) "level",
              (object) num
            },
            {
              (object) "max",
              (object) skill.upper_level
            }
          }));
          if (bActiveSkillZoom)
          {
            this.popupSkillDetail = (Action) (() => PopupSkillDetails.show(this.skillDetailPrefab, new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.JobAbility, new int?(nextLevel))));
            this.objSkillZoom.SetActive(true);
          }
          this.txtNextLevelLeft.SetText(nextLevel.ToString());
          this.txtNextLevelRight.SetText("/" + skill.upper_level.ToString());
          break;
        case 2:
          this.txtJobNameGrp2.SetTextLocalize(skill.name);
          UILabel txtActualLevelgrp2 = this.txtActualLevelgrp2;
          string thumbSkillLevelText = Consts.GetInstance().extra_skillI_thumb_skill_level_text;
          Hashtable args;
          if (skill.upper_level <= 0)
          {
            args = new Hashtable()
            {
              {
                (object) "level",
                jobAbility.level > 0 ? (object) jobAbility.level.ToString() : (object) Consts.GetInstance().SKILL_LEVEL_NONE
              },
              {
                (object) "max",
                (object) Consts.GetInstance().SKILL_LEVEL_NONE
              }
            };
          }
          else
          {
            args = new Hashtable();
            args.Add((object) "level", (object) jobAbility.level);
            args.Add((object) "max", (object) skill.upper_level);
          }
          string text = Consts.Format(thumbSkillLevelText, (IDictionary) args);
          txtActualLevelgrp2.SetTextLocalize(text);
          if (bActiveSkillZoom)
          {
            this.popupSkillDetail = (Action) (() => PopupSkillDetails.show(this.skillDetailPrefab, new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.JobAbility, new int?(jobAbility.level))));
            this.objSkillZoom.SetActive(true);
          }
          loader = Res.Icons.SkillGenreIcon.Load<GameObject>();
          e = loader.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          GameObject result1 = loader.Result;
          this.createGenreIcon(result1, this.dynSkillGenreIcons2[0].transform).GetComponent<SkillGenreIcon>().Init(skill.genre1);
          this.createGenreIcon(result1, this.dynSkillGenreIcons2[1].transform).GetComponent<SkillGenreIcon>().Init(skill.genre2);
          loader = (Future<GameObject>) null;
          break;
        case 3:
          this.txtJobNameGrp3.SetTextLocalize(skill.name);
          this.txtActualLevelgrp3.SetTextLocalize(skill.upper_level > 0 ? jobAbility.level.ToString() : Consts.GetInstance().SKILL_LEVEL_NONE);
          if (bActiveSkillZoom)
          {
            this.popupSkillDetail = (Action) (() => PopupSkillDetails.show(this.skillDetailPrefab, new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.JobAbility, new int?(jobAbility.level))));
            this.objSkillZoom.SetActive(true);
          }
          loader = Res.Icons.SkillGenreIcon.Load<GameObject>();
          e = loader.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          GameObject result2 = loader.Result;
          this.createGenreIcon(result2, this.dynSkillGenreIcons3[0].transform).GetComponent<SkillGenreIcon>().Init(skill.genre1);
          this.createGenreIcon(result2, this.dynSkillGenreIcons3[1].transform).GetComponent<SkillGenreIcon>().Init(skill.genre2);
          loader = (Future<GameObject>) null;
          break;
      }
    }
  }

  private GameObject createGenreIcon(GameObject prefab, Transform trans)
  {
    GameObject genreIcon = prefab.Clone(trans);
    UI2DSprite componentInChildren = genreIcon.GetComponentInChildren<UI2DSprite>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return genreIcon;
    UI2DSprite ui2Dsprite = componentInChildren;
    ((UIWidget) ui2Dsprite).depth = ((UIWidget) ui2Dsprite).depth + 150;
    return genreIcon;
  }

  public void onClickedSkillZoom() => this.popupSkillDetail();
}
