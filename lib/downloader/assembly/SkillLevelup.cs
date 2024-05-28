// Decompiled with JetBrains decompiler
// Type: SkillLevelup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SkillLevelup : MonoBehaviour
{
  private const int SKILL_KIND = 4;
  private Unit004813Menu menu;
  [SerializeField]
  private GameObject[] skillSlots;
  [SerializeField]
  private UI2DSprite[] skillSlotSprites;
  [SerializeField]
  private UILabel[] txtSkillName;
  [SerializeField]
  private UILabel[] txtLvbefore;
  [SerializeField]
  private UILabel[] txtLvafter;

  public void SetSkillSlots(
    Unit004813Menu m,
    PlayerUnit beforeUnit,
    PlayerUnit afterUnit,
    Dictionary<int, Sprite> dicSkillIcons,
    List<LevelupSkill> levelupSkills)
  {
    this.menu = m;
    Array.ForEach<GameObject>(this.skillSlots, (Action<GameObject>) (gameObject => gameObject.SetActive(false)));
    if (levelupSkills.Count <= 0)
      return;
    int count = 0;
    levelupSkills.ForEach((Action<LevelupSkill>) (lus =>
    {
      if (lus.afterLevel <= lus.beforeLevel || count >= 4)
        return;
      this.skillSlots[count].SetActive(true);
      this.txtSkillName[count].SetTextLocalize(lus.skill.name);
      this.txtLvbefore[count].SetTextLocalize(lus.beforeLevel);
      this.txtLvafter[count].SetTextLocalize(lus.afterLevel);
      this.skillSlotSprites[count].sprite2D = dicSkillIcons[lus.skill.ID];
      ++count;
    }));
  }

  public void StartTween()
  {
    foreach (TweenAlpha componentsInChild in ((Component) this).GetComponentsInChildren<TweenAlpha>())
    {
      if (((UITweener) componentsInChild).tweenGroup == 0)
      {
        ((UITweener) componentsInChild).ResetToBeginning();
        ((UITweener) componentsInChild).PlayForward();
      }
    }
  }

  public void EndTween()
  {
    foreach (TweenAlpha componentsInChild in ((Component) this).GetComponentsInChildren<TweenAlpha>())
    {
      if (((UITweener) componentsInChild).tweenGroup == 1)
      {
        ((UITweener) componentsInChild).ResetToBeginning();
        ((UITweener) componentsInChild).PlayForward();
      }
    }
  }

  public void TweenFinish() => this.menu.StartNextTween();
}
