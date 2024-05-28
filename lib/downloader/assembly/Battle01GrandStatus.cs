// Decompiled with JetBrains decompiler
// Type: Battle01GrandStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01GrandStatus : NGBattleMenuBase
{
  [SerializeField]
  private UILabel place;
  [SerializeField]
  private UILabel hit;
  [SerializeField]
  private UILabel pDefense;
  [SerializeField]
  private UILabel mDefense;
  [SerializeField]
  private UILabel stay;
  [SerializeField]
  private GameObject descriptionRoot;
  [SerializeField]
  private UILabel description;
  [SerializeField]
  private NGTweenParts descriptionTween;
  [SerializeField]
  private UILabel[] landformTagLabels;
  private Transform[] skillLandformTagLabelAnchorDefaultTarget;
  private float[] skillLandformTagLabelAnchorDefaultRelative;
  private int[] skillLandformTagLabelAnchorDefaultAbsolute;
  private BL.BattleModified<BL.ClassValue<BL.Panel>> modified;

  private void OnEnable()
  {
    if (this.env == null || this.env.core.fieldCurrent.value == null)
      return;
    this.updateTagLabels(this.env.core.fieldCurrent.value);
  }

  private string numberString(int v) => v <= 0 ? string.Concat((object) v) : "+" + (object) v;

  private string percentString(int v) => v <= 0 ? v.ToString() + "%" : "+" + (object) v + "%";

  public override IEnumerator onInitAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01GrandStatus battle01GrandStatus = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battle01GrandStatus.modified = BL.Observe<BL.ClassValue<BL.Panel>>(battle01GrandStatus.env.core.fieldCurrent);
    return false;
  }

  public void setText(UILabel label, int v, float? ratio)
  {
    if (ratio.HasValue)
    {
      int v1 = Mathf.RoundToInt((float) ((double) ratio.Value * 100.0 - 100.0));
      this.setText(label, this.percentString(v1));
    }
    else
      this.setText(label, this.numberString(v));
  }

  protected override void LateUpdate_Battle()
  {
    if (!this.modified.isChangedOnce() || this.env.core.fieldCurrent.value == null)
      return;
    BattleLandform landform = this.env.core.fieldCurrent.value.landform;
    BattleLandformIncr displayIncr = landform.GetDisplayIncr();
    this.setText(this.place, landform.name);
    this.setText(this.hit, displayIncr.hit_incr, displayIncr.hit_ratio_incr);
    this.setText(this.pDefense, displayIncr.physical_defense_incr, displayIncr.physical_defense_ratio_incr);
    this.setText(this.mDefense, displayIncr.magic_defense_incr, displayIncr.magic_defense_ratio_incr);
    this.setText(this.stay, displayIncr.evasion_incr, displayIncr.evasion_ratio_incr);
    this.updateTagLabels(this.env.core.fieldCurrent.value);
    BL.Unit unit = this.env.core.unitCurrent.unit;
    if (unit == (BL.Unit) null || this.env.core.currentUnitPosition != null && unit.isView && !this.env.unitResource[unit].unitParts_.isMoving)
    {
      this.SetTextLandformDescription(landform.description);
    }
    else
    {
      if (this.env.core.currentUnitPosition == null || !unit.isView || !this.env.unitResource[unit].unitParts_.isMoving)
        return;
      this.descriptionTween.isActive = false;
    }
  }

  private void SetTextLandformDescription(string descriptionText)
  {
    if (Object.op_Equality((Object) this.descriptionTween, (Object) null))
      return;
    if (string.IsNullOrEmpty(descriptionText))
    {
      this.descriptionTween.isActive = false;
    }
    else
    {
      this.setText(this.description, descriptionText);
      this.descriptionTween.isActive = true;
    }
  }

  private void updateTagLabels(BL.Panel panel)
  {
    if (this.landformTagLabels == null || this.landformTagLabels.Length < 4)
      return;
    BL.SkillEffect skillEffect = panel.getSkillEffects().value.Find((Predicate<BL.SkillEffect>) (x => x.effect.EffectLogic.Enum == BattleskillEffectLogicEnum.invest_land_tag));
    int num = skillEffect != null ? skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.land_tag) : 0;
    if (this.skillLandformTagLabelAnchorDefaultTarget == null)
    {
      this.skillLandformTagLabelAnchorDefaultTarget = new Transform[4];
      this.skillLandformTagLabelAnchorDefaultTarget[0] = ((UIRect) this.landformTagLabels[3]).leftAnchor.target;
      this.skillLandformTagLabelAnchorDefaultTarget[1] = ((UIRect) this.landformTagLabels[3]).rightAnchor.target;
      this.skillLandformTagLabelAnchorDefaultTarget[2] = ((UIRect) this.landformTagLabels[3]).bottomAnchor.target;
      this.skillLandformTagLabelAnchorDefaultTarget[3] = ((UIRect) this.landformTagLabels[3]).topAnchor.target;
      this.skillLandformTagLabelAnchorDefaultRelative = new float[4];
      this.skillLandformTagLabelAnchorDefaultRelative[0] = ((UIRect) this.landformTagLabels[3]).leftAnchor.relative;
      this.skillLandformTagLabelAnchorDefaultRelative[1] = ((UIRect) this.landformTagLabels[3]).rightAnchor.relative;
      this.skillLandformTagLabelAnchorDefaultRelative[2] = ((UIRect) this.landformTagLabels[3]).bottomAnchor.relative;
      this.skillLandformTagLabelAnchorDefaultRelative[3] = ((UIRect) this.landformTagLabels[3]).topAnchor.relative;
      this.skillLandformTagLabelAnchorDefaultAbsolute = new int[4];
      this.skillLandformTagLabelAnchorDefaultAbsolute[0] = ((UIRect) this.landformTagLabels[3]).leftAnchor.absolute;
      this.skillLandformTagLabelAnchorDefaultAbsolute[1] = ((UIRect) this.landformTagLabels[3]).rightAnchor.absolute;
      this.skillLandformTagLabelAnchorDefaultAbsolute[2] = ((UIRect) this.landformTagLabels[3]).bottomAnchor.absolute;
      this.skillLandformTagLabelAnchorDefaultAbsolute[3] = ((UIRect) this.landformTagLabels[3]).topAnchor.absolute;
    }
    BattleLandform landform = panel.landform;
    int[] numArray = new int[4]
    {
      landform.tag1,
      landform.tag2,
      landform.tag3,
      num
    };
    UILabel uiLabel = (UILabel) null;
    for (int index = 0; index < 4; ++index)
    {
      BattleLandformTag battleLandformTag;
      if (numArray[index] != 0 && MasterData.BattleLandformTag.TryGetValue(numArray[index], out battleLandformTag))
      {
        ((Component) this.landformTagLabels[index]).gameObject.SetActive(true);
        this.setText(this.landformTagLabels[index], battleLandformTag.type);
        if (index == 3)
        {
          UILabel landformTagLabel = this.landformTagLabels[3];
          if (Object.op_Inequality((Object) uiLabel, (Object) null))
          {
            ((UIRect) landformTagLabel).leftAnchor.target = ((UIRect) uiLabel).leftAnchor.target;
            ((UIRect) landformTagLabel).rightAnchor.target = ((UIRect) uiLabel).rightAnchor.target;
            ((UIRect) landformTagLabel).topAnchor.target = ((UIRect) uiLabel).topAnchor.target;
            ((UIRect) landformTagLabel).bottomAnchor.target = ((UIRect) uiLabel).bottomAnchor.target;
            ((UIRect) landformTagLabel).leftAnchor.relative = ((UIRect) uiLabel).leftAnchor.relative;
            ((UIRect) landformTagLabel).rightAnchor.relative = ((UIRect) uiLabel).rightAnchor.relative;
            ((UIRect) landformTagLabel).topAnchor.relative = ((UIRect) uiLabel).topAnchor.relative;
            ((UIRect) landformTagLabel).bottomAnchor.relative = ((UIRect) uiLabel).bottomAnchor.relative;
            ((UIRect) landformTagLabel).leftAnchor.absolute = ((UIRect) uiLabel).leftAnchor.absolute;
            ((UIRect) landformTagLabel).rightAnchor.absolute = ((UIRect) uiLabel).rightAnchor.absolute;
            ((UIRect) landformTagLabel).topAnchor.absolute = ((UIRect) uiLabel).topAnchor.absolute;
            ((UIRect) landformTagLabel).bottomAnchor.absolute = ((UIRect) uiLabel).bottomAnchor.absolute;
          }
          else
          {
            ((UIRect) landformTagLabel).leftAnchor.target = this.skillLandformTagLabelAnchorDefaultTarget[0];
            ((UIRect) landformTagLabel).rightAnchor.target = this.skillLandformTagLabelAnchorDefaultTarget[1];
            ((UIRect) landformTagLabel).bottomAnchor.target = this.skillLandformTagLabelAnchorDefaultTarget[2];
            ((UIRect) landformTagLabel).topAnchor.target = this.skillLandformTagLabelAnchorDefaultTarget[3];
            ((UIRect) landformTagLabel).leftAnchor.relative = this.skillLandformTagLabelAnchorDefaultRelative[0];
            ((UIRect) landformTagLabel).rightAnchor.relative = this.skillLandformTagLabelAnchorDefaultRelative[1];
            ((UIRect) landformTagLabel).bottomAnchor.relative = this.skillLandformTagLabelAnchorDefaultRelative[2];
            ((UIRect) landformTagLabel).topAnchor.relative = this.skillLandformTagLabelAnchorDefaultRelative[3];
            ((UIRect) landformTagLabel).leftAnchor.absolute = this.skillLandformTagLabelAnchorDefaultAbsolute[0];
            ((UIRect) landformTagLabel).rightAnchor.absolute = this.skillLandformTagLabelAnchorDefaultAbsolute[1];
            ((UIRect) landformTagLabel).bottomAnchor.absolute = this.skillLandformTagLabelAnchorDefaultAbsolute[2];
            ((UIRect) landformTagLabel).topAnchor.absolute = this.skillLandformTagLabelAnchorDefaultAbsolute[3];
          }
          ((UIRect) landformTagLabel).ResetAnchors();
          ((UIRect) landformTagLabel).UpdateAnchors();
        }
        ((UIWidget) this.landformTagLabels[index]).MakePixelPerfect();
      }
      else
      {
        if (Object.op_Equality((Object) uiLabel, (Object) null))
          uiLabel = this.landformTagLabels[index];
        this.setText(this.landformTagLabels[index], "");
        ((Component) this.landformTagLabels[index]).gameObject.SetActive(false);
      }
      ((UIWidget) this.landformTagLabels[index]).SetDirty();
    }
  }
}
