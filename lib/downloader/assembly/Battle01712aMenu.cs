// Decompiled with JetBrains decompiler
// Type: Battle01712aMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01712aMenu : NGBattleMenuBase
{
  [SerializeField]
  private UILabel txt_name;
  [SerializeField]
  private UILabel txt_balloon;

  private void Awake()
  {
    foreach (Component componentsInChild in ((Component) this).GetComponentsInChildren<Battle01712aCharaView>(true))
      componentsInChild.gameObject.SetActive(false);
  }

  private IEnumerator doSetSprite(UnitUnit unit, int job_id, int? metamorId)
  {
    Battle01712aMenu battle01712aMenu = this;
    Future<Sprite> f = unit.LoadSpriteLarge(!metamorId.HasValue || metamorId.Value == 0 ? job_id : metamorId.Value, 1f);
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = f.Result;
    foreach (Battle01712aCharaView componentsInChild in ((Component) battle01712aMenu).GetComponentsInChildren<Battle01712aCharaView>(true))
    {
      componentsInChild.setSprite(result);
      ((Component) componentsInChild).gameObject.SetActive(true);
    }
  }

  public void setUnit(BL.Unit unit)
  {
    SkillMetamorphosis metamorphosis = unit.metamorphosis;
    int metamorphosisId = metamorphosis != null ? metamorphosis.metamorphosis_id : 0;
    UnitUnit unit1 = unit.unit;
    this.setText(this.txt_name, unit1.getName(metamorphosisId));
    this.StartCoroutine(this.doSetSprite(unit.unit, unit.playerUnit.job_id, metamorphosisId != 0 ? new int?(metamorphosisId) : new int?()));
    if (!Object.op_Inequality((Object) this.txt_balloon, (Object) null))
      return;
    UnitVoicePattern voicePattern = unit1.getVoicePattern(metamorphosisId);
    this.setText(this.txt_balloon, voicePattern?.dead_message ?? string.Empty);
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    instance.playSE("SE_2022");
    instance.playVoiceByID(voicePattern, 76);
  }
}
