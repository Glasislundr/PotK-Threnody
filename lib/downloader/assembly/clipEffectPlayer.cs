// Decompiled with JetBrains decompiler
// Type: clipEffectPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class clipEffectPlayer : MonoBehaviour
{
  private WeaponTrail _trail;
  private bool firstStep = true;
  private int countStep;
  private NGDuelUnit MyUnit;
  private BattleLandform mLandForm;
  protected UnitUnit _mUnit;
  private UnitUnit mDeteilUnit;
  private const int defaultFootstepSound = 5000;

  public WeaponTrail trail
  {
    get
    {
      if (Object.op_Equality((Object) this._trail, (Object) null))
        this._trail = ((Component) this).gameObject.GetComponentInChildren<WeaponTrail>();
      return this._trail;
    }
  }

  private NGDuelUnit EnemyUnit
  {
    get
    {
      return !Object.op_Inequality((Object) this.MyUnit, (Object) null) ? (NGDuelUnit) null : this.MyUnit.Enemy;
    }
  }

  private NGDuelManager duelManager
  {
    get
    {
      return !Object.op_Inequality((Object) this.MyUnit, (Object) null) ? (NGDuelManager) null : this.MyUnit.manager;
    }
  }

  private bool isDuel => Object.op_Inequality((Object) this.duelManager, (Object) null);

  private BattleLandform landformFlat => MasterData.BattleLandform[1];

  private UnitUnit mUnit
  {
    get
    {
      if (this._mUnit == null)
      {
        if (Object.op_Inequality((Object) this.MyUnit, (Object) null))
        {
          if (this.MyUnit.mMyUnitData != (BL.Unit) null)
            this._mUnit = this.MyUnit.mMyUnitData.unit;
        }
        else
        {
          BattleUnitParts componentInParent = ((Component) this).gameObject.GetComponentInParent<BattleUnitParts>();
          if (Object.op_Inequality((Object) componentInParent, (Object) null))
            this._mUnit = componentInParent.getUnitPosition().unit.unit;
        }
      }
      return this._mUnit;
    }
  }

  private BL.Unit blUnit
  {
    get
    {
      if (Object.op_Inequality((Object) this.MyUnit, (Object) null) && this.MyUnit.mMyUnitData != (BL.Unit) null)
        return this.MyUnit.mMyUnitData;
      BattleUnitParts componentInParent = ((Component) this).gameObject.GetComponentInParent<BattleUnitParts>();
      return !Object.op_Inequality((Object) componentInParent, (Object) null) ? (BL.Unit) null : componentInParent.getUnitPosition().unit;
    }
  }

  public UnitUnit DeteilUnit
  {
    get => this.mDeteilUnit;
    set => this.mDeteilUnit = value;
  }

  private void Start()
  {
    this.MyUnit = ((Component) this).gameObject.GetComponent<NGDuelUnit>();
    if (Object.op_Equality((Object) this.MyUnit, (Object) null) && Object.op_Inequality((Object) ((Component) this).transform.parent, (Object) null))
      this.MyUnit = ((Component) ((Component) this).transform.parent).gameObject.GetComponent<NGDuelUnit>();
    this._trail = ((Component) this).gameObject.GetComponentInChildren<WeaponTrail>();
  }

  public int lastPlaySound { get; private set; } = -1;

  protected virtual void playSound(string var)
  {
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    string str = var;
    if (var.Contains("FOOTSTEPS"))
    {
      int num = Mathf.RoundToInt(Time.timeScale);
      if (num < 1)
        num = 1;
      if (this.countStep++ % num != 0)
        return;
      if (this.mUnit == null)
      {
        this.lastPlaySound = instance.playSE("SE_5000");
      }
      else
      {
        string clip;
        if (this.firstStep)
        {
          this.firstStep = false;
          clip = this.mLandForm == null ? this.landformFlat.GetFootstep(this.mUnit).footstep1 : this.mLandForm.GetFootstep(this.mUnit).footstep1;
        }
        else
        {
          this.firstStep = true;
          clip = this.mLandForm == null ? this.landformFlat.GetFootstep(this.mUnit).footstep2 : this.mLandForm.GetFootstep(this.mUnit).footstep2;
        }
        instance.playSE(clip);
      }
    }
    else
    {
      string[] strArray = str.Split('.');
      if (!Object.op_Inequality((Object) null, (Object) instance) || !(str != ""))
        return;
      this.lastPlaySound = instance.playSE(strArray[0]);
    }
  }

  public void setGroundStatus(BattleLandform lf) => this.mLandForm = lf;

  public void playEffect(string str)
  {
    if (Object.op_Equality((Object) this.MyUnit, (Object) null))
      return;
    switch (str)
    {
      case "weapon_trail_on":
        if (!Object.op_Inequality((Object) this.trail, (Object) null))
          break;
        this.trail.On(((Component) this).transform, this.MyUnit);
        break;
      case "weapon_trail_off":
        if (!Object.op_Inequality((Object) this.trail, (Object) null))
          break;
        this.trail.Off();
        break;
      default:
        if (str.Contains("_locus_hit"))
        {
          BL.DuelTurn thisTurnDamage = this.MyUnit.thisTurnDamage;
          this.EnemyUnit.damaged(this.EnemyUnit.useDistance);
          if (thisTurnDamage == null || !thisTurnDamage.isHit)
            break;
          if (this.EnemyUnit.mMyUnitData.playerUnit.equippedGearOrInitial.kind.Enum == GearKindEnum.shield || this.EnemyUnit.mMyUnitData.playerUnit.equippedGear2 != (PlayerItem) null && this.EnemyUnit.mMyUnitData.playerUnit.equippedGear2.gear.kind.Enum == GearKindEnum.shield)
          {
            if (!thisTurnDamage.isCritical || !this.MyUnit.mIsLastAttack)
              break;
            this.StartCoroutine(this.EnemyUnit.playCriticalFlash());
            break;
          }
          if (thisTurnDamage.isCritical && this.MyUnit.mIsLastAttack)
            this.StartCoroutine(this.EnemyUnit.playCriticalFlash());
        }
        string[] strArray = str.Split(':');
        string target = "0";
        string parent_name = "";
        if (strArray == null || strArray.Length == 0)
          break;
        string effectName;
        if (strArray.Length == 1)
          effectName = strArray[0];
        else if (strArray.Length == 4)
        {
          effectName = strArray[0];
          target = strArray[1];
          parent_name = strArray[3];
        }
        else
        {
          effectName = strArray[0];
          target = strArray[1];
          if (target == "3")
            parent_name = strArray[2];
        }
        this.StartCoroutine(this.loadEffect(effectName, target, parent_name));
        break;
    }
  }

  private string changeEffect(string src)
  {
    string str = src;
    if (str.Equals("ef515_def_hit"))
    {
      if (Object.op_Inequality((Object) this.EnemyUnit, (Object) null))
      {
        foreach (BL.Skill skill in ((IEnumerable<BL.Skill>) this.EnemyUnit.thisTurn.invokeDefenderDuelSkills).Where<BL.Skill>((Func<BL.Skill, bool>) (x =>
        {
          BattleskillGenre? genre1 = x.genre1;
          BattleskillGenre battleskillGenre = BattleskillGenre.defense;
          return genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue && x.skill.duel_effect != null;
        })))
        {
          if (!string.IsNullOrEmpty(skill.skill.duel_effect.duel_effect_name))
          {
            str = skill.skill.duel_effect.duel_effect_name;
            break;
          }
        }
      }
      if (string.IsNullOrEmpty(str))
        str = "ef515_def_hit";
    }
    else if (str.Contains("_locus_hit") && Object.op_Inequality((Object) this.MyUnit, (Object) null))
    {
      DuelElementHitEffect elementHitEffect = this.MyUnit.GetElementHitEffect(src);
      if (elementHitEffect != null && !string.IsNullOrEmpty(elementHitEffect.change_effect_name))
        str = elementHitEffect.change_effect_name;
    }
    return str;
  }

  private IEnumerator loadEffect(string effectName, string target, string parent_name)
  {
    clipEffectPlayer clipEffectPlayer = this;
    if (!effectName.Contains("weapon_trail_"))
    {
      GameObject place = clipEffectPlayer.duelManager.mRoot3d;
      if (Object.op_Equality((Object) null, (Object) place))
        place = ((Component) clipEffectPlayer).gameObject;
      effectName = clipEffectPlayer.changeEffect(effectName);
      GameObject gameObject = Singleton<NGDuelDataManager>.GetInstance().GetPreloadDuelEffect(effectName, place.transform);
      if (Object.op_Equality((Object) gameObject, (Object) null))
      {
        Future<GameObject> go = new ResourceObject(string.Format("BattleEffects/duel/{0}", (object) effectName)).Load<GameObject>();
        IEnumerator e = go.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        gameObject = go.Result.Clone(place.transform);
        if (clipEffectPlayer.isDuel)
          Singleton<NGDuelDataManager>.GetInstance().AddDestroyList(gameObject);
        go = (Future<GameObject>) null;
      }
      if (Object.op_Inequality((Object) gameObject, (Object) null))
        clipEffectPlayer.goEffect(gameObject, effectName, target, parent_name);
      else
        Debug.LogError((object) ("clipEffectPlayer not load effectName:" + effectName));
    }
  }

  private void goEffect(GameObject goef, string kwd1, string kwd2, string kwd3)
  {
    goef.transform.localRotation = ((Component) this).gameObject.transform.rotation;
    switch (kwd2)
    {
      case "0":
        if (!string.IsNullOrEmpty(kwd3))
        {
          Transform transform = ((Component) this).gameObject.transform.GetChildInFind(kwd3);
          if (Object.op_Equality((Object) null, (Object) transform))
            transform = ((Component) this).gameObject.transform;
          goef.SetParent(((Component) transform).gameObject);
          break;
        }
        if (kwd1.Equals("ef515_def_hit") || kwd1.Equals("ef703_duel_holysheeld"))
        {
          Transform transform = !Object.op_Inequality((Object) this.MyUnit, (Object) null) ? ((Component) this).gameObject.transform.GetChildInFind("Bip") : this.MyUnit.mBipTransform;
          if (Object.op_Equality((Object) null, (Object) transform))
            transform = ((Component) this).gameObject.transform;
          goef.transform.localPosition = transform.position;
        }
        else
          goef.transform.localPosition = ((Component) this).gameObject.transform.position;
        goef.transform.parent = ((Component) this).gameObject.transform;
        break;
      case "1":
        if (!Object.op_Inequality((Object) this.EnemyUnit, (Object) null))
          break;
        if (!string.IsNullOrEmpty(kwd3))
        {
          Transform transform = ((Component) this.EnemyUnit).gameObject.transform.GetChildInFind(kwd3);
          if (Object.op_Equality((Object) null, (Object) transform))
            transform = ((Component) this.EnemyUnit).gameObject.transform;
          goef.transform.parent = transform;
          break;
        }
        Transform childInFind = ((Component) this.EnemyUnit).gameObject.transform.GetChildInFind("damagepoint_a");
        if (Object.op_Inequality((Object) null, (Object) childInFind))
        {
          goef.transform.localPosition = childInFind.position;
          break;
        }
        Transform transform1 = this.EnemyUnit.mBipTransform;
        if (Object.op_Equality((Object) null, (Object) transform1))
          transform1 = ((Component) this.EnemyUnit).gameObject.transform;
        goef.transform.localPosition = transform1.position;
        break;
      case "2":
        Transform transform2 = ((Component) this).gameObject.transform.GetChildInFind("weaponr");
        if (Object.op_Equality((Object) null, (Object) transform2))
          transform2 = ((Component) this).gameObject.transform;
        Transform transform3 = transform2.GetChild(0);
        if (Object.op_Equality((Object) null, (Object) transform3))
          transform3 = ((Component) this).gameObject.transform;
        goef.transform.localPosition = transform3.position;
        break;
      case "3":
        goef.SetParent(this.FindParentInRoot(kwd3));
        break;
      case "4":
        GameObject baseGameObject1 = this.MyUnit.baseGameObject;
        if (!string.IsNullOrEmpty(kwd3))
        {
          Transform transform4 = baseGameObject1.transform.GetChildInFind(kwd3);
          if (Object.op_Equality((Object) transform4, (Object) null))
            transform4 = baseGameObject1.transform;
          goef.SetParent(((Component) transform4).gameObject);
          break;
        }
        goef.SetParent(baseGameObject1);
        break;
      case "5":
        if (!Object.op_Inequality((Object) this.EnemyUnit, (Object) null))
          break;
        GameObject baseGameObject2 = this.EnemyUnit.baseGameObject;
        if (!string.IsNullOrEmpty(kwd3))
        {
          Transform transform5 = baseGameObject2.transform.GetChildInFind(kwd3);
          if (Object.op_Equality((Object) transform5, (Object) null))
            transform5 = baseGameObject2.transform;
          goef.SetParent(((Component) transform5).gameObject);
          break;
        }
        goef.SetParent(baseGameObject2);
        break;
    }
  }

  private GameObject FindParentInRoot(string name)
  {
    GameObject parentInRoot = this.duelManager.mRoot3d;
    if (Object.op_Equality((Object) parentInRoot, (Object) null))
      parentInRoot = ((Component) this).gameObject;
    if (!string.IsNullOrEmpty(name))
    {
      Transform transform = parentInRoot.transform.Find(name);
      if (Object.op_Inequality((Object) transform, (Object) null))
        parentInRoot = ((Component) transform).gameObject;
    }
    return parentInRoot;
  }

  public void shoot()
  {
    if (!Object.op_Inequality((Object) null, (Object) this.MyUnit))
      return;
    this.MyUnit.shootSomething();
  }

  public void shootready()
  {
    if (!Object.op_Inequality((Object) null, (Object) this.MyUnit))
      return;
    this.MyUnit.shootSomethingReady();
  }

  public void backstepStart()
  {
    if (!Object.op_Inequality((Object) null, (Object) this.MyUnit))
      return;
    this.MyUnit.playBackstepFromClip();
  }

  public void Attack1Start()
  {
    if (!Object.op_Inequality((Object) null, (Object) this.MyUnit))
      return;
    this.MyUnit.AtAttack1();
  }

  public void Attack2Start()
  {
    if (!Object.op_Inequality((Object) null, (Object) this.MyUnit))
      return;
    this.MyUnit.AtAttack2();
  }

  public void AttackSStart()
  {
    if (!Object.op_Inequality((Object) null, (Object) this.MyUnit))
      return;
    this.MyUnit.AtAttackS();
  }

  public void DodgeStart()
  {
    if (!Object.op_Inequality((Object) null, (Object) this.MyUnit))
      return;
    this.MyUnit.SetDodgeMode();
  }

  public void playVoiceCue(int Cue)
  {
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (!Object.op_Inequality((Object) null, (Object) instance))
      return;
    BL.Unit blUnit = this.blUnit;
    UnitVoicePattern voicePattern = blUnit != (BL.Unit) null ? blUnit.getVoicePattern() : (UnitVoicePattern) null;
    if (voicePattern == null && this.DeteilUnit != null)
      voicePattern = this.DeteilUnit.unitVoicePattern;
    if (voicePattern == null)
      return;
    instance.playVoiceByID(voicePattern, Cue);
  }

  public void HideWeapon()
  {
    if (!Object.op_Inequality((Object) null, (Object) this.MyUnit))
      return;
    this.MyUnit.SetActiveEquipeWeapon(false);
  }

  public void ShowWeapon()
  {
    if (!Object.op_Inequality((Object) null, (Object) this.MyUnit))
      return;
    this.MyUnit.SetActiveEquipeWeapon(true);
  }

  public void HideMap()
  {
    if (!Object.op_Inequality((Object) null, (Object) this.MyUnit))
      return;
    this.MyUnit.manager.SetActiveMap(false);
  }

  public void ShowMap()
  {
    if (!Object.op_Inequality((Object) null, (Object) this.MyUnit))
      return;
    this.MyUnit.manager.SetActiveMap(true);
  }

  public void HideShadow()
  {
    if (!Object.op_Inequality((Object) null, (Object) this.MyUnit))
      return;
    this.MyUnit.SetActiveShadow(false);
  }

  public void ShowShadow()
  {
    if (!Object.op_Inequality((Object) null, (Object) this.MyUnit))
      return;
    this.MyUnit.SetActiveShadow(true);
  }

  public void start_attack()
  {
    if (!Object.op_Inequality((Object) this.MyUnit, (Object) null))
      return;
    this.MyUnit.AddAttackMotionCount();
  }
}
