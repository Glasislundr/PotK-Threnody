// Decompiled with JetBrains decompiler
// Type: EffectSkillAcquisition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class EffectSkillAcquisition : MonoBehaviour
{
  [Header("スキル")]
  [SerializeField]
  private GameObject objSkillBox_;
  [SerializeField]
  private UI2DSprite spriteSkill_;
  [SerializeField]
  private UILabel txtSkillName_;
  [SerializeField]
  private UILabel txtSkillDescription_;
  private BattleskillSkill skill_;

  public static Future<GameObject> createLoader()
  {
    return new ResourceObject("Prefabs/battle/Unit_Skill_Acquisition").Load<GameObject>();
  }

  public static IEnumerator doShow(
    PopupResultUnitBase popupBase,
    GameObject prefab,
    BattleskillSkill skill,
    bool autoDelete)
  {
    GameObject go = prefab.Clone(popupBase.lnkSkill.transform);
    EffectSkillAcquisition effect = go.GetComponent<EffectSkillAcquisition>();
    effect.initialize(skill);
    bool bWait = true;
    popupBase.onNext = (Action) (() =>
    {
      if (effect.isRunning)
        return;
      bWait = false;
    });
    while (bWait)
      yield return (object) null;
    if (autoDelete)
      Object.Destroy((Object) go);
  }

  public bool isRunning { get; private set; } = true;

  public void initialize(BattleskillSkill skill)
  {
    this.skill_ = skill;
    this.objSkillBox_.SetActive(false);
  }

  private IEnumerator Start()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EffectSkillAcquisition skillAcquisition = this;
    UIPanel panel;
    Future<Sprite> ld;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      skillAcquisition.spriteSkill_.sprite2D = ld.Result;
      skillAcquisition.txtSkillName_.SetTextLocalize(skillAcquisition.skill_.name);
      skillAcquisition.txtSkillDescription_.SetTextLocalize(skillAcquisition.skill_.description);
      ((UIRect) panel).alpha = 1f;
      skillAcquisition.StartCoroutine("doPlay");
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    panel = ((Component) skillAcquisition).GetComponent<UIPanel>();
    ((UIRect) panel).alpha = 0.0f;
    ld = skillAcquisition.skill_.LoadBattleSkillIcon();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) ld.Wait();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator doPlay()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1024");
    this.objSkillBox_.SetActive(true);
    yield return (object) new WaitForSeconds(1f);
    this.isRunning = false;
  }
}
