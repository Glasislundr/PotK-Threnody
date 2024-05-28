// Decompiled with JetBrains decompiler
// Type: DuelColosseumSkillDisp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using UnityEngine;

#nullable disable
public class DuelColosseumSkillDisp : MonoBehaviour
{
  [SerializeField]
  private UILabel skill_name;
  [SerializeField]
  private UI2DSprite icon;

  public Future<Sprite> Init(BL.Skill skill)
  {
    this.skill_name.SetTextLocalize(skill.name);
    return skill.skill.LoadBattleSkillIcon().Then<Sprite>((Func<Sprite, Sprite>) (f => this.icon.sprite2D = f));
  }
}
