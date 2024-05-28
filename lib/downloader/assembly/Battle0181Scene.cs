// Decompiled with JetBrains decompiler
// Type: Battle0181Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Battle0181Scene : NGSceneBase
{
  [SerializeField]
  private Battle0181Menu menu;
  [SerializeField]
  private NGDuelManager duel;
  public GameObject spdButton1x;
  public GameObject spdButton2x;
  public GameObject spdButton4x;
  public GameObject statusAttackBaseNormal;
  public GameObject statusAttackBaseColosseum;
  private Color orig_ambient;
  private GameObject gDL;
  private float origSpeed;
  private int settingSpeed;
  private bool is_colosseum_duel;
  private bool is_initial_scene;
  private bool enableSkip;

  public static void changeSceneForColossuem(DuelColosseumResult result, bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("battle018_1", (stack ? 1 : 0) != 0, (object) result);
  }

  public IEnumerator onStartSceneAsync(DuelColosseumResult result)
  {
    IEnumerator e = this.onStartSceneAsync(new DuelResult()
    {
      isColosseum = true,
      isPlayerAttack = result.isPlayerFirstAttacker,
      attack = result.isPlayerFirstAttacker ? result.player : result.opponent,
      attackAttackStatus = result.isPlayerFirstAttacker ? result.playerAttackStatus : result.opponentAttackStatus,
      colosseumNewAAS = result.isPlayerFirstAttacker ? result.colosseumNewPAS : result.colosseumNewOAS,
      colosseumAttackFirstAttack = result.isPlayerFirstAttacker ? result.colosseumPlayerFirstAttack : result.colosseumOpponentFirstAttack,
      attackDamage = result.firstAttackerDamage,
      attackFromDamage = result.firstAttackerFromDamage,
      attackDuelSupport = (IntimateDuelSupport) null,
      defense = !result.isPlayerFirstAttacker ? result.player : result.opponent,
      defenseAttackStatus = !result.isPlayerFirstAttacker ? result.playerAttackStatus : result.opponentAttackStatus,
      colosseumNewDAS = !result.isPlayerFirstAttacker ? result.colosseumNewPAS : result.colosseumNewOAS,
      colosseumDefenseFirstAttack = !result.isPlayerFirstAttacker ? result.colosseumPlayerFirstAttack : result.colosseumOpponentFirstAttack,
      defenseDamage = result.secondAttackerDamage,
      defenseFromDamage = result.secondAttackerFromDamage,
      defenseDuelSupport = (IntimateDuelSupport) null,
      isFirstBoss = false,
      isBossBattle = false,
      isDieAttack = result.isDieFirstAttacker,
      isDieDefense = result.isDieSecondAttacker,
      beforeAttakerAilmentEffectIDs = new int[0],
      beforeDefenderAilmentEffectIDs = new int[0],
      turns = result.turns,
      distance = 1
    }, new DuelEnvironment()
    {
      storys = (List<BL.Story>) null,
      stage = !result.isExploreChallenge ? new BL.Stage(501) : new BL.Stage(10001)
    });
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(DuelResult duelResult, DuelEnvironment duelEnv)
  {
    this.orig_ambient = RenderSettings.ambientLight;
    this.gDL = GameObject.Find("Directional light");
    ((Component) this.duel).gameObject.SetActive(true);
    if (this.is_initial_scene)
    {
      this.duel.ResetLight();
    }
    else
    {
      RenderSettings.ambientLight = new Color(1f, 1f, 1f);
      if (Object.op_Inequality((Object) this.gDL, (Object) null))
        this.gDL.SetActive(false);
      BL.UnitPosition attack = new BL.UnitPosition();
      BL.UnitPosition defense = new BL.UnitPosition();
      AttackStatus attackStatus = duelResult.playerAttackStatus();
      AttackStatus defenseStatus = duelResult.enemyAttackStatus();
      attack.unit = duelResult.playerUnit();
      defense.unit = duelResult.enemyUnit();
      IEnumerator e = this.menu.Init(attack, attackStatus, duelResult.playerColosseumFirstAttack(), defense, defenseStatus, duelResult.enemyColosseumFirstAttack(), duelResult.isColosseum, duelEnv.isDemoMode);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.menu.onStartSceneAsync(duelResult.isColosseum);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.duel.Initialize(duelResult, duelEnv);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      CommonRoot instance1 = Singleton<CommonRoot>.GetInstance();
      instance1.isActiveBackground3DCamera = false;
      if (duelEnv.isDemoMode)
        instance1.HideLoadingLayer();
      this.origSpeed = Time.timeScale;
      this.settingSpeed = 1;
      try
      {
        this.settingSpeed = Persist.duel.Data.speed;
      }
      catch (Exception ex)
      {
        Persist.duel.Delete();
        Persist.duel.Data = new Persist.Duel();
      }
      if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
        this.settingSpeed = 1;
      this.SetSpeed(this.settingSpeed);
      this.is_colosseum_duel = duelResult.isColosseum;
      if (Object.op_Inequality((Object) this.statusAttackBaseNormal, (Object) null))
        this.statusAttackBaseNormal.SetActive(!this.is_colosseum_duel);
      if (Object.op_Inequality((Object) this.statusAttackBaseColosseum, (Object) null))
        this.statusAttackBaseColosseum.SetActive(this.is_colosseum_duel);
      NGBattleManager instance2 = Singleton<NGBattleManager>.GetInstance();
      this.enableSkip = !instance2.isPvp && !instance2.isPvnpc || duelEnv.isDemoMode;
      this.is_initial_scene = true;
    }
  }

  private void SetSpeed(int speed)
  {
    this.settingSpeed = Mathf.Clamp(speed, 1, 3);
    this.spdButton1x.SetActive(this.settingSpeed == 1);
    this.spdButton2x.SetActive(this.settingSpeed == 2);
    this.spdButton4x.SetActive(this.settingSpeed == 3);
    Time.timeScale = (float) this.settingSpeed;
    if (Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
      return;
    ((UIButtonColor) this.spdButton1x.GetComponent<UIButton>()).isEnabled = false;
  }

  public override void onEndScene()
  {
    if (this.duel.isDuelEnd)
    {
      Singleton<CommonRoot>.GetInstance().isActiveBackground3DCamera = true;
      RenderSettings.ambientLight = this.orig_ambient;
      Singleton<NGSoundManager>.GetInstance().crossFadeCurrentBGM(2.5f, 0.0f);
      this.is_initial_scene = false;
      if (Object.op_Inequality((Object) this.gDL, (Object) null))
        this.gDL.SetActive(true);
    }
    if (Persist.duel.Data.speed != this.settingSpeed)
    {
      Persist.duel.Data.speed = this.settingSpeed;
      Persist.duel.Flush();
    }
    Time.timeScale = this.origSpeed;
  }

  public void onSpeedButtonClicked() => this.SetSpeed(this.nextSpeed());

  public void onSkipButtonClicked()
  {
    if (Object.op_Equality((Object) this.duel, (Object) null) || !this.enableSkip)
      return;
    this.duel.Skip();
  }

  private int nextSpeed()
  {
    if (1 == this.settingSpeed)
      return 2;
    return 2 == this.settingSpeed ? 3 : 1;
  }

  private void OnEnable()
  {
    this.duel.isWait = false;
    if (!this.is_initial_scene)
      return;
    this.SetSpeed(this.settingSpeed);
  }

  private void OnDisable() => this.duel.isWait = true;
}
