// Decompiled with JetBrains decompiler
// Type: Battle01SkillTitle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class Battle01SkillTitle : NGBattleMenuBase
{
  [SerializeField]
  private GameObject title_skill;
  [SerializeField]
  private GameObject title_secrets;

  public void setSkill(BL.Skill skill)
  {
    if (skill.isOugi)
    {
      this.title_skill.SetActive(false);
      this.title_secrets.SetActive(true);
    }
    else
    {
      this.title_skill.SetActive(true);
      this.title_secrets.SetActive(false);
    }
  }
}
