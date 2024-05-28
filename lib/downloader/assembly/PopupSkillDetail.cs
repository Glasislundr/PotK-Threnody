// Decompiled with JetBrains decompiler
// Type: PopupSkillDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class PopupSkillDetail : MonoBehaviour
{
  [SerializeField]
  [Tooltip("タイトル")]
  private UILabel txtTitle_;
  [SerializeField]
  [Tooltip("スキル名")]
  private UILabel txtName_;
  [SerializeField]
  [Tooltip("メインアイコン")]
  private UI2DSprite icon_;
  [SerializeField]
  [Tooltip("ジャンルアイコン左側")]
  private UI2DSprite iconGenreL_;
  [SerializeField]
  [Tooltip("ジャンルアイコン右側")]
  private UI2DSprite iconGenreR_;
  [SerializeField]
  [Tooltip("Lv?/?")]
  private PopupSkillDetail.LevelObject level_;
  [SerializeField]
  [Tooltip("最大Lv?")]
  private PopupSkillDetail.LevelObject maxLevel_;
  [SerializeField]
  [Tooltip("説明")]
  private UILabel txtDescription_;
  [SerializeField]
  [Tooltip("下部メッセージ")]
  private UILabel txtBottom_;
  [SerializeField]
  private UILabel txtBottom2_;

  public int index { get; private set; } = -1;

  public void resetParam(
    Dictionary<UnitParameter.SkillGroup, string> dicTitle,
    PopupSkillDetails.Param param,
    int pos)
  {
    this.index = pos;
    if (param.skill == null)
    {
      this.txtDescription_.SetTextLocalize(string.Empty);
      this.txtBottom_.SetTextLocalize(string.Empty);
    }
    else
    {
      string str;
      this.txtTitle_.SetTextLocalize(dicTitle.TryGetValue(param.group, out str) ? str : string.Empty);
      this.txtName_.SetTextLocalize(param.skill.name);
      switch (param.group)
      {
        case UnitParameter.SkillGroup.Leader:
          this.level_.setActive(true);
          this.level_.setValue(0, 0);
          this.maxLevel_.setActive(false);
          break;
        case UnitParameter.SkillGroup.SEA:
          this.level_.setActive(true);
          this.level_.setValue(param.level, Consts.GetInstance().SEA_SKILL_LEVEL_DENOMINATOR);
          this.maxLevel_.setActive(false);
          break;
        default:
          if (param.level.HasValue)
          {
            this.level_.setActive(true);
            this.level_.setValue(param.level.Value, param.skill.upper_level);
            this.maxLevel_.setActive(false);
            break;
          }
          this.level_.setActive(false);
          this.maxLevel_.setActive(true);
          this.maxLevel_.setValue(param.skill.upper_level);
          break;
      }
      this.txtDescription_.SetTextLocalize(param.skill.description);
      if (string.IsNullOrEmpty(param.message))
        this.txtBottom_.SetTextLocalize(string.Empty);
      else if (!param.message.StartsWith("\n"))
        this.txtBottom_.SetTextLocalize("\n" + param.message);
      else
        this.txtBottom_.SetTextLocalize(param.message);
      if (string.IsNullOrEmpty(param.remainingSkillAcquisition))
        this.txtBottom2_.SetTextLocalize(string.Empty);
      else
        this.txtBottom2_.SetTextLocalize(param.remainingSkillAcquisition);
    }
  }

  public void resetIcons(Sprite icon, Sprite genreL, Sprite genreR)
  {
    this.setSprite(this.icon_, icon);
    this.setSprite(this.iconGenreL_, genreL);
    this.setSprite(this.iconGenreR_, genreR);
  }

  private void setSprite(UI2DSprite uiSprite, Sprite sprite)
  {
    if (Object.op_Inequality((Object) sprite, (Object) null))
    {
      uiSprite.sprite2D = sprite;
      ((Component) uiSprite).gameObject.SetActive(true);
    }
    else
      ((Component) uiSprite).gameObject.SetActive(false);
  }

  [Serializable]
  private class LevelObject
  {
    [SerializeField]
    [Tooltip("先頭位置")]
    private GameObject top;
    [SerializeField]
    [Tooltip("数値")]
    private UILabel txtNumber;

    public void setActive(bool bActive) => this.top.SetActive(bActive);

    public void setValue(int v) => this.txtNumber.SetTextLocalize(v > 0 ? v.ToString() : "-");

    public void setValue(int numer, int denom)
    {
      this.txtNumber.SetTextLocalize((denom > 0 || numer > 0 ? numer.ToString() : "-") + "/" + (denom <= 0 ? "-" : denom.ToString()));
    }

    public void setValue(int? numer, string denom)
    {
      this.txtNumber.SetTextLocalize((numer.HasValue ? numer.Value.ToString() : "-") + "/" + denom);
    }
  }
}
