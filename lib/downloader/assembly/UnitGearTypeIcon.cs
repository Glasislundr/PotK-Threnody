// Decompiled with JetBrains decompiler
// Type: UnitGearTypeIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class UnitGearTypeIcon : BattleMonoBehaviour
{
  [SerializeField]
  protected UnitGearTypeIcon.ElementIcons[] gears;

  public void setUnit(BL.Unit unit)
  {
    ((IEnumerable<UnitGearTypeIcon.ElementIcons>) this.gears).ForEach<UnitGearTypeIcon.ElementIcons>((Action<UnitGearTypeIcon.ElementIcons>) (x => x.parent.SetActive(false)));
    UnitGearTypeIcon.ElementIcons gear = this.gears[unit.unit.kind.ID - 1];
    gear.parent.SetActive(true);
    int num = 1;
    BattleskillEffect battleskillEffect = ((IEnumerable<BL.Skill>) unit.duelSkills).SelectMany<BL.Skill, BattleskillEffect>((Func<BL.Skill, IEnumerable<BattleskillEffect>>) (x => (IEnumerable<BattleskillEffect>) x.skill.Effects)).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.effect_logic.Enum == BattleskillEffectLogicEnum.invest_element)).FirstOrDefault<BattleskillEffect>();
    if (battleskillEffect != null)
      num = (int) battleskillEffect.skill.element;
    ((IEnumerable<GameObject>) gear.icons).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
    gear.icons[num - 1].SetActive(true);
  }

  [Serializable]
  public struct ElementIcons
  {
    public GameObject parent;
    public GameObject[] icons;
  }
}
