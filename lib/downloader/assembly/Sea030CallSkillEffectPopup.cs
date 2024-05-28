// Decompiled with JetBrains decompiler
// Type: Sea030CallSkillEffectPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Sea030CallSkillEffectPopup : MonoBehaviour
{
  [SerializeField]
  private UILabel text;
  [SerializeField]
  private Animator popupAnima;

  public void SetData(CallCharacter master)
  {
    this.text.SetText(string.Format("{0}の誓約スキル\n[FFFF00]{1}[-]\nが解放されました", (object) ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.same_character_id == master.same_character_id)).ToArray<UnitUnit>()[0].name, (object) MasterData.BattleskillSkill[master.call_skill_id].name));
  }

  public void onOkButton() => this.popupAnima.SetTrigger("IsOut");

  public void PopupActiveSe()
  {
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (!Object.op_Inequality((Object) instance, (Object) null))
      return;
    instance.playSE("SE_1044");
  }

  public void AnimationEnd() => Singleton<NGSceneManager>.GetInstance().backScene();
}
