// Decompiled with JetBrains decompiler
// Type: BattleTerrainSkillEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class BattleTerrainSkillEffect : MonoBehaviour
{
  [SerializeField]
  private GameObject mActivateEffect;
  [SerializeField]
  private GameObject mLoopEffect;
  [SerializeField]
  private float mWaitLoopEffSec = 1f;
  private HashSet<int> mTargetLandTags = new HashSet<int>();
  private bool isActive;
  private Coroutine mActiveLoopHandle;

  private void Awake()
  {
    this.mActivateEffect.SetActive(false);
    this.mLoopEffect.SetActive(false);
  }

  public void SetCompatibleSkillByEffective(BL.Unit unit)
  {
    this.mTargetLandTags.Clear();
    this.AddCompatibleSkill(unit.skillEffects.Where(BattleskillSkillType.passive, baseSkillOnly: true, remainOnly: true));
  }

  public void AddCompatibleSkill(IEnumerable<BattleskillSkill> skills)
  {
    foreach (BattleskillSkill skill in skills)
      this.AddCompatibleSkill(skill);
  }

  public void AddCompatibleSkill(BattleskillSkill skill)
  {
    foreach (BattleskillEffect effect in skill.Effects)
    {
      BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(effect);
      if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.land_tag1))
      {
        for (BattleskillEffectLogicArgumentEnum key = BattleskillEffectLogicArgumentEnum.land_tag1; key <= BattleskillEffectLogicArgumentEnum.land_tag10; ++key)
        {
          int num = packedSkillEffect.GetInt(key);
          if (num != 0)
            this.mTargetLandTags.Add(num);
          else
            break;
        }
      }
    }
  }

  public void resetIllegalActiveStatus()
  {
    if (!this.isActive || this.mLoopEffect.activeSelf || this.mActiveLoopHandle != null)
      return;
    this.isActive = false;
  }

  public void UpdateState(BL.Panel panel)
  {
    if (this.isCompatiblePanel(panel))
    {
      if (this.isActive)
        return;
      this.isActive = true;
      this.mActivateEffect.SetActive(Singleton<NGBattleManager>.GetInstance().isBattleEnable);
      this.stopWaitForLoopEffect();
      this.mActiveLoopHandle = this.StartCoroutine(this.activeLoopEffect(this.mWaitLoopEffSec));
    }
    else
    {
      this.stopWaitForLoopEffect();
      this.isActive = false;
      this.mActivateEffect.SetActive(false);
      this.mLoopEffect.SetActive(false);
    }
  }

  private bool isCompatiblePanel(BL.Panel panel)
  {
    BattleLandform landform = panel.landform;
    return this.mTargetLandTags.Contains(landform.tag1) || this.mTargetLandTags.Contains(landform.tag2) || this.mTargetLandTags.Contains(landform.tag3) || panel.hasLandTag(false, this.mTargetLandTags.ToArray<int>());
  }

  private void stopWaitForLoopEffect()
  {
    if (this.mActiveLoopHandle == null)
      return;
    this.StopCoroutine(this.mActiveLoopHandle);
    this.mActiveLoopHandle = (Coroutine) null;
  }

  private IEnumerator activeLoopEffect(float waitSec)
  {
    yield return (object) new WaitForSeconds(waitSec);
    this.mActivateEffect.SetActive(false);
    this.mLoopEffect.SetActive(true);
    this.mActiveLoopHandle = (Coroutine) null;
  }
}
