// Decompiled with JetBrains decompiler
// Type: Coloseum02342Menu
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
public class Coloseum02342Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtAttack;
  [SerializeField]
  protected UILabel TxtAttackOwn;
  [SerializeField]
  protected UILabel TxtCharaname;
  [SerializeField]
  protected UILabel TxtCharaname_ElementOn;
  [SerializeField]
  protected UILabel TxtCriticalEnemy;
  [SerializeField]
  protected UILabel TxtCriticalOwn;
  [SerializeField]
  protected UILabel TxtDexterityEnemy;
  [SerializeField]
  protected UILabel TxtDexterityOwn;
  [SerializeField]
  protected UILabel TxtEnemy;
  [SerializeField]
  protected UILabel TxtEnemyHonor;
  [SerializeField]
  protected UILabel TxtHPEnemy;
  [SerializeField]
  protected UILabel TxtHPOwn;
  [SerializeField]
  protected UILabel TxtOwn;
  [SerializeField]
  protected UILabel TxtOwnHonor;
  [SerializeField]
  protected UILabel TxtUnitNameEnemy;
  [SerializeField]
  protected UILabel TxtUnitNameEnemy_ElementOn;
  [SerializeField]
  protected UILabel TxtWeaponNameEnemy1;
  [SerializeField]
  protected UILabel TxtWeaponNameEnemy2;
  [SerializeField]
  protected UILabel TxtWeaponNameOwn1;
  [SerializeField]
  protected UILabel TxtWeaponNameOwn2;
  [SerializeField]
  private UISprite slcCountryOwn;
  [SerializeField]
  private UISprite slcCountryEnemy;
  [SerializeField]
  protected TweenAlpha tweenAlphaFirstWeaponOwn;
  [SerializeField]
  protected TweenAlpha tweenAlphaSecondWeaponOwn;
  [SerializeField]
  protected TweenAlpha tweenAlphaFirstWeaponEnemy;
  [SerializeField]
  protected TweenAlpha tweenAlphaSecondWeaponEnemy;
  [SerializeField]
  private bool DEBUG_LOSE_PARTICLE_OWN;
  [SerializeField]
  private bool DEBUG_LOSE_PARTICLE_ENEMY;
  protected ColosseumUtility.Info info;
  protected GameCore.ColosseumResult result;
  protected Gladiator gladiator;
  private int nextBattleType;
  private int finalDuelCount;
  private int dueleCount = -1;
  private int resultIndex = -1;
  private int resultEffectType = -1;
  protected List<UnitIcon> playerUnitIconC;
  protected List<UnitIcon> enemyUnitIconC;
  public bool initialized;
  private GearKindIcon playerElementIcon;
  private GearKindIcon enemyElementIcon;
  protected bool isSkip;
  protected bool SkipPermission;
  private GearKindIcon playerGearKindIcon1;
  private GearKindIcon playerGearKindIcon2;
  private GearKindIcon enemyGearKindIcon1;
  private GearKindIcon enemyGearKindIcon2;
  protected BL.Unit player;
  protected BL.Unit opponent;
  private BL.Unit lastPlayer;
  private BL.Unit lastOpponent;
  private const int TWEEN_LOSE_ENEMY = 16;
  private const int TWEEN_LOSE_OWN = 15;
  private const int TWEEN_ID_OUT = 14;
  private const int TWEEN_ID_IN = 0;
  private const float DELAY = 1f;
  private const float DELAY_HALF = 0.5f;
  private const float DELAY_QUARTER = 0.25f;
  private float WAIT_LIMIT = 10f;
  private float waitTime;
  [SerializeField]
  private GameObject[] playerAttackValues;
  [SerializeField]
  private GameObject[] enemyAttackValues;
  [SerializeField]
  private GameObject[] playerAttackAdvantage;
  [SerializeField]
  private GameObject[] enemyAttackAdvantage;
  [SerializeField]
  private GameObject[] playerUnitIcon;
  private List<GameObject> playerMatchUpIcons;
  [SerializeField]
  private GameObject[] enemyUnitIcon;
  private List<GameObject> enemyMatchUpIcons;
  [SerializeField]
  private GameObject[] rounds;
  [SerializeField]
  private GameObject[] battleType;
  [SerializeField]
  private GameObject[] battleTypeDecoration;
  [SerializeField]
  private GameObject criticalBuffOwn;
  [SerializeField]
  private GameObject dexterityBuffOwn;
  [SerializeField]
  private GameObject attackBuffOwn;
  [SerializeField]
  private GameObject statusBuffOwn;
  [SerializeField]
  private GameObject criticalBuffEnemy;
  [SerializeField]
  private GameObject dexterityBuffEnemy;
  [SerializeField]
  private GameObject attackBuffEnemy;
  [SerializeField]
  private GameObject statusBuffEnemy;
  [SerializeField]
  private GameObject dirAnimResult;
  [SerializeField]
  protected GameObject dirResult;
  protected Animator[] resultAnimations = new Animator[2];
  [SerializeField]
  private GameObject dirAnimation;
  private GameObject loseAnimation;
  [SerializeField]
  protected GameObject roundButton;
  [SerializeField]
  private GameObject roundButtonAnimate;
  [SerializeField]
  private GameObject playerDuelCharacter;
  [SerializeField]
  private GameObject enemyDuelCharacter;
  [SerializeField]
  private GameObject middle;
  protected global::BattleResult battleResults;
  [SerializeField]
  private UI2DSprite[] Emblems;
  [SerializeField]
  private UI2DSprite playerKind1;
  [SerializeField]
  private UI2DSprite playerKind2;
  [SerializeField]
  private UI2DSprite enemyKind1;
  [SerializeField]
  private UI2DSprite enemyKind2;
  [SerializeField]
  private GameObject dirBonusOwn;
  [SerializeField]
  private GameObject dirBonusEnemy;
  [SerializeField]
  private GameObject linkPlayerElement;
  [SerializeField]
  private GameObject linkEnemyElement;
  [SerializeField]
  private UILabel txtBonusOwn;
  [SerializeField]
  private UILabel txtBonusEnemy;
  [SerializeField]
  protected UIButton ibtnResult;
  [SerializeField]
  private Coloseum02342Scene scene;
  [SerializeField]
  protected GameObject BtnSkip;
  private Coloseum02342Menu.States state;
  private Coloseum02342Menu.States nextState;
  protected string loseAnimPath = "Prefabs/colosseum/colosseum023-4-4/023-4-4_lose";
  protected string winAnimPath = "Prefabs/colosseum/colosseum023-4-4/023-4-4_win";
  protected string battleResultAnimPath = "Prefabs/colosseum/colosseum023-4-4/dir_BattleResults";
  protected NGSoundManager SM;

  private void Awake() => this.SM = Singleton<NGSoundManager>.GetInstance();

  public IEnumerator Initialize(
    ColosseumUtility.Info info,
    GameCore.ColosseumResult result,
    Gladiator gladiator,
    int duelCount)
  {
    this.SetColosseumData(info, result, gladiator);
    IEnumerator e = this.LoadResource(duelCount);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (this.nextBattleType == 1)
      this.battleTypeDecoration[0].gameObject.SetActive(true);
    else if (this.nextBattleType == 2)
      this.battleTypeDecoration[1].gameObject.SetActive(true);
    this.initialized = true;
  }

  public void SetColosseumData(
    ColosseumUtility.Info info,
    GameCore.ColosseumResult result,
    Gladiator gladiator)
  {
    this.info = info;
    this.result = result;
    this.finalDuelCount = result.duelResult.Length;
    this.nextBattleType = info.next_battle_type;
    this.gladiator = gladiator;
  }

  public void SetColosseumData(GameCore.ColosseumResult result, Gladiator gladiator)
  {
    this.info = (ColosseumUtility.Info) null;
    this.result = result;
    this.finalDuelCount = result.duelResult.Length;
    this.nextBattleType = 0;
    this.gladiator = gladiator;
  }

  public IEnumerator LoadResource(int duelCount)
  {
    Coloseum02342Menu receiver = this;
    Future<GameObject> loseAnimationPrefab;
    IEnumerator e;
    if (Object.op_Equality((Object) receiver.loseAnimation, (Object) null))
    {
      loseAnimationPrefab = Res.Prefabs.colosseum_unit_lose.Load<GameObject>();
      e = loseAnimationPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      receiver.loseAnimation = loseAnimationPrefab.Result.Clone(receiver.dirAnimation.transform);
      loseAnimationPrefab = (Future<GameObject>) null;
    }
    receiver.loseAnimation.SetActive(false);
    if (Object.op_Equality((Object) receiver.resultAnimations[0], (Object) null))
    {
      loseAnimationPrefab = new ResourceObject(receiver.loseAnimPath).Load<GameObject>();
      e = loseAnimationPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      receiver.resultAnimations[0] = loseAnimationPrefab.Result.Clone(receiver.dirAnimResult.transform).GetComponent<Animator>();
      loseAnimationPrefab = (Future<GameObject>) null;
    }
    ((Component) receiver.resultAnimations[0]).gameObject.SetActive(false);
    if (Object.op_Equality((Object) receiver.resultAnimations[1], (Object) null))
    {
      loseAnimationPrefab = new ResourceObject(receiver.winAnimPath).Load<GameObject>();
      e = loseAnimationPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      receiver.resultAnimations[1] = loseAnimationPrefab.Result.Clone(receiver.dirAnimResult.transform).GetComponent<Animator>();
      loseAnimationPrefab = (Future<GameObject>) null;
    }
    ((Component) receiver.resultAnimations[1]).gameObject.SetActive(false);
    if (Object.op_Equality((Object) receiver.battleResults, (Object) null))
    {
      loseAnimationPrefab = new ResourceObject(receiver.battleResultAnimPath).Load<GameObject>();
      e = loseAnimationPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject gameObject = loseAnimationPrefab.Result.Clone(receiver.dirResult.transform);
      receiver.battleResults = gameObject.GetComponent<global::BattleResult>();
      receiver.battleResults.Initialize(new Action(receiver.Replay));
      loseAnimationPrefab = (Future<GameObject>) null;
    }
    NGTween.setOnTweenFinishedIncludeDelay(NGTween.findTweenersGroup(((Component) receiver).gameObject, 14, true), (MonoBehaviour) receiver, "DoAdvanceNextDuel");
    Future<GameObject> unitPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    e = unitPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject unitPrefab = unitPrefabF.Result;
    Future<GameObject> prefabF = Res.Icons.GearKindIcon.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    if (Object.op_Equality((Object) receiver.playerGearKindIcon1, (Object) null))
      receiver.playerGearKindIcon1 = result.Clone(((Component) receiver.playerKind1).gameObject.transform).GetComponent<GearKindIcon>();
    if (Object.op_Equality((Object) receiver.playerGearKindIcon2, (Object) null))
      receiver.playerGearKindIcon2 = result.Clone(((Component) receiver.playerKind2).gameObject.transform).GetComponent<GearKindIcon>();
    if (Object.op_Equality((Object) receiver.enemyGearKindIcon1, (Object) null))
      receiver.enemyGearKindIcon1 = result.Clone(((Component) receiver.enemyKind1).gameObject.transform).GetComponent<GearKindIcon>();
    if (Object.op_Equality((Object) receiver.enemyGearKindIcon2, (Object) null))
      receiver.enemyGearKindIcon2 = result.Clone(((Component) receiver.enemyKind2).gameObject.transform).GetComponent<GearKindIcon>();
    if (Object.op_Equality((Object) receiver.playerElementIcon, (Object) null))
      receiver.playerElementIcon = result.Clone(receiver.linkPlayerElement.transform).GetComponent<GearKindIcon>();
    if (Object.op_Equality((Object) receiver.enemyElementIcon, (Object) null))
      receiver.enemyElementIcon = result.Clone(receiver.linkEnemyElement.gameObject.transform).GetComponent<GearKindIcon>();
    e = receiver.SetUnit(unitPrefab, duelCount);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Modified<Player> modified = SMManager.Observe<Player>();
    receiver.TxtOwn.SetText(modified.Value.name.ToConverter());
    receiver.TxtEnemy.SetText(receiver.gladiator.name.ToConverter());
    int currentEmblemId = modified.Value.current_emblem_id;
    int emblemIdEnemy = receiver.gladiator.current_emblem_id;
    e = receiver.SetEmblem(receiver.Emblems[0], currentEmblemId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = receiver.SetEmblem(receiver.Emblems[1], emblemIdEnemy);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void InitKindIcon(GameObject weaponPrefab)
  {
    if (Object.op_Equality((Object) this.playerGearKindIcon1, (Object) null))
      this.playerGearKindIcon1 = weaponPrefab.Clone(((Component) this.playerKind1).gameObject.transform).GetComponent<GearKindIcon>();
    if (Object.op_Equality((Object) this.playerGearKindIcon2, (Object) null))
      this.playerGearKindIcon2 = weaponPrefab.Clone(((Component) this.playerKind2).gameObject.transform).GetComponent<GearKindIcon>();
    if (Object.op_Equality((Object) this.enemyGearKindIcon1, (Object) null))
      this.enemyGearKindIcon1 = weaponPrefab.Clone(((Component) this.enemyKind1).gameObject.transform).GetComponent<GearKindIcon>();
    if (!Object.op_Equality((Object) this.enemyGearKindIcon2, (Object) null))
      return;
    this.enemyGearKindIcon2 = weaponPrefab.Clone(((Component) this.enemyKind2).gameObject.transform).GetComponent<GearKindIcon>();
  }

  public void StartToBeginning(int index)
  {
    this.dueleCount = index;
    this.resultIndex = this.dueleCount - 1;
    this.state = Coloseum02342Menu.States.DEFAULT;
    this.nextState = Coloseum02342Menu.States.DEFAULT;
    this.loseAnimation.SetActive(false);
    foreach (var data in ((IEnumerable<GameObject>) this.rounds).Select((s, i) => new
    {
      i = i,
      s = s
    }))
      data.s.SetActive(data.i == index);
    if (index >= this.finalDuelCount)
      this.ChangeState(Coloseum02342Menu.States.FINAL_DUEL);
    else if (index > 0)
    {
      this.lastPlayer = this.result.duelResult[this.dueleCount - 1].player;
      this.lastOpponent = this.result.duelResult[this.dueleCount - 1].opponent;
      this.player = this.result.duelResult[this.dueleCount].player;
      this.opponent = this.result.duelResult[this.dueleCount].opponent;
      if (this.lastPlayer == (BL.Unit) null || this.lastOpponent == (BL.Unit) null)
        this.ChangeState(Coloseum02342Menu.States.WALKOVER);
      else
        this.ChangeState(Coloseum02342Menu.States.DUEL);
    }
    else
    {
      this.player = this.result.duelResult[this.dueleCount].player;
      this.opponent = this.result.duelResult[this.dueleCount].opponent;
      this.ChangeState(Coloseum02342Menu.States.FIRST_DUEL);
    }
    this.SetMatchupIcon(this.playerMatchUpIcons, this.resultIndex, false);
    this.SetMatchupIcon(this.enemyMatchUpIcons, this.resultIndex, false);
  }

  protected override void Update()
  {
    if (!this.initialized)
      return;
    base.Update();
    if (this.roundButton.activeSelf)
    {
      this.waitTime += Time.deltaTime;
      if ((double) this.WAIT_LIMIT < (double) this.waitTime)
        this.IbtnStartBattle();
    }
    switch (this.state)
    {
      case Coloseum02342Menu.States.FIRST_DUEL:
        this.FirstDuel();
        break;
      case Coloseum02342Menu.States.FINAL_DUEL:
        this.FinalDuel();
        break;
      case Coloseum02342Menu.States.DUEL:
        this.Duel();
        break;
      case Coloseum02342Menu.States.WALKOVER:
        this.Walkover();
        break;
      case Coloseum02342Menu.States.DUEL_RESULT:
        this.DuelResult();
        break;
    }
    if (this.state == this.nextState)
      return;
    this.state = this.nextState;
    switch (this.state)
    {
      case Coloseum02342Menu.States.FIRST_DUEL:
        this.StartCoroutine(this.StartFirstDuel());
        break;
      case Coloseum02342Menu.States.FINAL_DUEL:
        this.StartCoroutine(this.StartFinalDuel());
        break;
      case Coloseum02342Menu.States.DUEL:
        this.StartCoroutine(this.StartDuel());
        break;
      case Coloseum02342Menu.States.WALKOVER:
        this.StartCoroutine(this.StartWalkover());
        break;
      case Coloseum02342Menu.States.DUEL_RESULT:
        this.StartDuelResult();
        break;
    }
  }

  private void ChangeState(Coloseum02342Menu.States nextState) => this.nextState = nextState;

  private IEnumerator StartFirstDuel()
  {
    this.AnimateTargets(40, false);
    this.AnimateTargets(41, false);
    this.AnimateTargets(42, false);
    this.AnimateTargets(43, false);
    this.AnimateTargets(44, false);
    IEnumerator e = this.AdvanceNextDuel();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator StartDuel()
  {
    Coloseum02342Menu coloseum02342Menu = this;
    coloseum02342Menu.AnimateTargets(14, false);
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = coloseum02342Menu.UnitBattleResultAnimation(coloseum02342Menu.resultIndex, new Action(coloseum02342Menu.\u003CStartDuel\u003Eb__124_0));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator StartWalkover()
  {
    Coloseum02342Menu coloseum02342Menu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = coloseum02342Menu.UnitBattleResultAnimation(coloseum02342Menu.resultIndex, new Action(coloseum02342Menu.\u003CStartWalkover\u003Eb__125_0));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator StartFinalDuel()
  {
    Coloseum02342Menu coloseum02342Menu = this;
    coloseum02342Menu.AnimateTargets(14, false);
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = coloseum02342Menu.UnitBattleResultAnimation(coloseum02342Menu.resultIndex, new Action(coloseum02342Menu.\u003CStartFinalDuel\u003Eb__126_0));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    coloseum02342Menu.ChangeState(Coloseum02342Menu.States.DUEL_RESULT);
  }

  private void StartDuelResult()
  {
  }

  private void FirstDuel()
  {
  }

  private void Duel()
  {
  }

  private void Walkover()
  {
  }

  private void FinalDuel()
  {
  }

  private void DuelResult()
  {
  }

  private void ShowWinOrLoss()
  {
    if (this.result.isWin())
    {
      this.resultEffectType = 1;
      this.SM.playSE("SE_1505");
    }
    else
    {
      this.resultEffectType = 0;
      this.SM.playSE("SE_1506");
    }
    foreach (Component resultAnimation in this.resultAnimations)
      resultAnimation.gameObject.SetActive(false);
    ((Component) this.resultAnimations[this.resultEffectType]).gameObject.SetActive(true);
    ((Component) this.ibtnResult).gameObject.SetActive(true);
    if (!Object.op_Inequality((Object) this.battleResults, (Object) null))
      return;
    this.battleResults.dispReplay(true);
  }

  private IEnumerator SetUnitCharacter(DuelColosseumResult result)
  {
    foreach (Component component in this.playerDuelCharacter.transform)
      component.gameObject.SetActive(false);
    IEnumerator e;
    if (result.player != (BL.Unit) null)
    {
      e = this.LoadDuelist(result.player.unit, result.player.playerUnit.job_id, this.playerDuelCharacter, Res.GUI._009_3_sozai.mask_Chara_L);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    foreach (Component component in this.enemyDuelCharacter.transform)
      component.gameObject.SetActive(false);
    if (result.opponent != (BL.Unit) null)
    {
      e = this.LoadDuelist(result.opponent.unit, result.opponent.playerUnit.job_id, this.enemyDuelCharacter, Res.GUI._009_3_sozai.mask_Chara_R);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator LoadDuelist(
    UnitUnit unit,
    int job_id,
    GameObject original,
    ResourceObject resource)
  {
    Future<GameObject> future = unit.LoadColosseum();
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject go = future.Result.Clone(original.transform);
    TweenColor tweenColor = go.AddComponent<TweenColor>();
    TweenColor component = original.GetComponent<TweenColor>();
    tweenColor.from = component.from;
    tweenColor.to = component.to;
    ((UITweener) tweenColor).duration = ((UITweener) component).duration;
    ((UITweener) tweenColor).delay = ((UITweener) component).delay;
    ((UITweener) tweenColor).tweenGroup = ((UITweener) component).tweenGroup;
    ((UITweener) tweenColor).ignoreTimeScale = ((UITweener) component).ignoreTimeScale;
    ((UITweener) tweenColor).animationCurve = ((UITweener) component).animationCurve;
    ((UITweener) tweenColor).style = ((UITweener) component).style;
    ((Behaviour) ((Component) tweenColor).GetComponent<TweenColor>()).enabled = false;
    e = unit.SetLargeSpriteWithMask(job_id, go, resource.Load<Texture2D>(), 0);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator UnitBattleResultAnimation(int index, Action CallBack = null)
  {
    Coloseum02342Menu coloseum02342Menu = this;
    foreach (GameObject playerMatchUpIcon in coloseum02342Menu.playerMatchUpIcons)
      playerMatchUpIcon.SetActive(false);
    foreach (GameObject enemyMatchUpIcon in coloseum02342Menu.enemyMatchUpIcons)
      enemyMatchUpIcon.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    CallBack = CallBack ?? new Action(coloseum02342Menu.\u003CUnitBattleResultAnimation\u003Eb__136_0);
    UnitIcon.ColosseumResult[] colosseumResultArray = coloseum02342Menu.ChangeResultEnum(coloseum02342Menu.result.duelResult[index]);
    UnitIcon.ColosseumResult ownResult = colosseumResultArray[0];
    UnitIcon.ColosseumResult opponentResult = colosseumResultArray[1];
    yield return (object) new WaitForSeconds(0.5f);
    coloseum02342Menu.playerUnitIconC[index].SetColosseumResult(ownResult, 5f, 0.3f);
    coloseum02342Menu.enemyUnitIconC[index].SetColosseumResult(opponentResult, 5f, 0.3f, CallBack);
    string clip = "";
    switch (ownResult)
    {
      case UnitIcon.ColosseumResult.WIN:
        clip = "SE_1507";
        break;
      case UnitIcon.ColosseumResult.DROW:
        clip = "SE_1509";
        break;
      case UnitIcon.ColosseumResult.LOSE:
        clip = "SE_1508";
        break;
    }
    coloseum02342Menu.SM.playSE(clip);
  }

  private IEnumerator BattleResult()
  {
    DuelColosseumResult resultOne = this.result.duelResult[this.resultIndex];
    for (int index = 0; index < this.finalDuelCount; ++index)
    {
      this.playerUnitIconC[index].SetColosseumResultAlphaLoop();
      this.enemyUnitIconC[index].SetColosseumResultAlphaLoop();
    }
    UnitIcon.ColosseumResult[] unitIconResult = this.ChangeResultEnum(resultOne);
    if (this.DEBUG_LOSE_PARTICLE_OWN || resultOne.player != (BL.Unit) null && unitIconResult[0] == UnitIcon.ColosseumResult.LOSE)
    {
      this.loseAnimation.transform.right = new Vector3(-0.6640633f, 0.0f);
      this.loseAnimation.SetActive(true);
      this.SM.playSE("SE_1511");
      this.AnimateTargets(15);
      yield return (object) new WaitForSeconds(1f);
    }
    if (this.DEBUG_LOSE_PARTICLE_ENEMY || resultOne.opponent != (BL.Unit) null && unitIconResult[1] == UnitIcon.ColosseumResult.LOSE)
    {
      this.loseAnimation.transform.right = Vector3.zero;
      this.loseAnimation.SetActive(true);
      this.SM.playSE("SE_1511");
      this.AnimateTargets(16);
      yield return (object) new WaitForSeconds(1f);
    }
  }

  private IEnumerator BattleResultForSkipDuel()
  {
    IEnumerator e = this.BattleResult();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) new WaitForSeconds(0.25f);
    this.AnimateTargets(14);
  }

  private IEnumerator BattleResultForNextDuel()
  {
    IEnumerator e = this.BattleResult();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) new WaitForSeconds(0.25f);
    this.AnimateTargets(14);
  }

  private IEnumerator BattleResultForFinal()
  {
    IEnumerator e = this.BattleResult();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) new WaitForSeconds(0.25f);
    if (Object.op_Inequality((Object) this.battleResults, (Object) null))
    {
      this.dirResult.gameObject.SetActive(true);
      this.battleResults.SetBattleResult(this.result);
    }
    yield return (object) new WaitForSeconds(0.5f);
    this.ShowWinOrLoss();
  }

  private void DoAdvanceNextDuel() => this.StartCoroutine(this.AdvanceNextDuel());

  private IEnumerator AdvanceNextDuel()
  {
    if (!this.isSkip)
      Singleton<NGDuelDataManager>.GetInstance().StartBackGroundPreload(this.result.duelResult[this.dueleCount]);
    this.SetMatchupIcon(this.playerMatchUpIcons, this.dueleCount);
    this.SetMatchupIcon(this.enemyMatchUpIcons, this.dueleCount);
    IEnumerator e = this.UpdateDuelist(this.dueleCount);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.AnimateTargets(0);
    if (this.isSkip)
    {
      this.IbtnStartBattle();
    }
    else
    {
      e = this.StartRoundButton(0.5f);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator SetUnit(GameObject unitPrefab, int duelCount)
  {
    this.playerMatchUpIcons = new List<GameObject>();
    this.enemyMatchUpIcons = new List<GameObject>();
    this.playerUnitIconC = new List<UnitIcon>();
    this.enemyUnitIconC = new List<UnitIcon>();
    for (int i = 0; i < this.finalDuelCount; ++i)
    {
      UnitIcon unitPlayer = this.InitializeUnitIcon(unitPrefab, this.playerUnitIcon[i]);
      IEnumerator e = this.SetUnitIcon(this.result.duelResult[i].player, unitPlayer, this.playerUnitIconC);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      UnitIcon unitEnemy = this.InitializeUnitIcon(unitPrefab, this.enemyUnitIcon[i]);
      e = this.SetUnitIcon(this.result.duelResult[i].opponent, unitEnemy, this.enemyUnitIconC);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (i < duelCount - 1)
      {
        UnitIcon.ColosseumResult[] colosseumResultArray = this.ChangeResultEnum(this.result.duelResult[i]);
        unitPlayer.SetColosseumResult(colosseumResultArray[0]);
        unitEnemy.SetColosseumResult(colosseumResultArray[1]);
      }
      unitPlayer.Gray = true;
      unitEnemy.Gray = true;
      e = this.LoadMatchIcon(this.playerUnitIcon[i], Res.Prefabs.colosseum.colosseum023_4_2.dir_match_own, this.playerMatchUpIcons);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.LoadMatchIcon(this.enemyUnitIcon[i], Res.Prefabs.colosseum.colosseum023_4_2.dir_match_enemy, this.enemyMatchUpIcons);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitPlayer = (UnitIcon) null;
      unitEnemy = (UnitIcon) null;
    }
    this.SetGrayOut(this.playerUnitIconC, this.enemyUnitIconC, duelCount);
  }

  private void SetGrayOut(List<UnitIcon> listPlayer, List<UnitIcon> listEnemy, int targetIndex)
  {
    foreach (var data in listPlayer.Select((s, i) => new
    {
      s = s,
      i = i
    }))
      data.s.Gray = data.i != targetIndex;
    foreach (var data in listEnemy.Select((s, i) => new
    {
      s = s,
      i = i
    }))
      data.s.Gray = data.i != targetIndex;
  }

  private void SetMatchupIcon(List<GameObject> list, int index, bool withAnimate = true)
  {
    foreach (var data in list.Select((s, i) => new
    {
      s = s,
      i = i
    }))
    {
      if (data.i == index)
      {
        data.s.SetActive(true);
        this.playerUnitIcon[data.i].GetComponent<UIPanel>().depth = 3;
        this.enemyUnitIcon[data.i].GetComponent<UIPanel>().depth = 3;
        this.SetGrayOut(this.playerUnitIconC, this.enemyUnitIconC, data.i);
        if (withAnimate)
        {
          switch (data.i)
          {
            case 0:
              this.AnimateTargets(40);
              this.AnimateTargets(41, isReverse: true);
              this.AnimateTargets(42, isReverse: true);
              this.AnimateTargets(43, isReverse: true);
              this.AnimateTargets(44, isReverse: true);
              continue;
            case 1:
              this.AnimateTargets(41);
              this.AnimateTargets(40, isReverse: true);
              this.AnimateTargets(42, isReverse: true);
              this.AnimateTargets(43, isReverse: true);
              this.AnimateTargets(44, isReverse: true);
              continue;
            case 2:
              this.AnimateTargets(42);
              this.AnimateTargets(41, isReverse: true);
              this.AnimateTargets(40, isReverse: true);
              this.AnimateTargets(43, isReverse: true);
              this.AnimateTargets(44, isReverse: true);
              continue;
            case 3:
              this.AnimateTargets(43);
              this.AnimateTargets(41, isReverse: true);
              this.AnimateTargets(42, isReverse: true);
              this.AnimateTargets(40, isReverse: true);
              this.AnimateTargets(44, isReverse: true);
              continue;
            case 4:
              this.AnimateTargets(44);
              this.AnimateTargets(41, isReverse: true);
              this.AnimateTargets(42, isReverse: true);
              this.AnimateTargets(43, isReverse: true);
              this.AnimateTargets(40, isReverse: true);
              continue;
            default:
              continue;
          }
        }
      }
      else
      {
        data.s.SetActive(false);
        this.playerUnitIcon[data.i].GetComponent<UIPanel>().depth = 2;
        this.enemyUnitIcon[data.i].GetComponent<UIPanel>().depth = 2;
      }
    }
  }

  private IEnumerator LoadMatchIcon(GameObject go, ResourceObject ro, List<GameObject> list)
  {
    Future<GameObject> matchPrefab = ro.Load<GameObject>();
    IEnumerator e = matchPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    list.Add(matchPrefab.Result.Clone(go.transform));
  }

  private UnitIcon InitializeUnitIcon(GameObject unitPrefab, GameObject unit)
  {
    foreach (Component component in unit.transform)
      Object.Destroy((Object) component.gameObject);
    return unitPrefab.Clone(unit.transform).GetComponent<UnitIcon>();
  }

  private IEnumerator SetUnitIcon(BL.Unit unit, UnitIcon icon, List<UnitIcon> list)
  {
    if (unit == (BL.Unit) null)
    {
      icon.SetEmpty();
    }
    else
    {
      IEnumerator e = icon.setSimpleUnit(unit.playerUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      icon.setLevelText(unit.playerUnit);
      icon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    }
    list.Add(icon);
  }

  private void SetBuff(GameObject go, int before, int after)
  {
    GameObject[] array = go.transform.GetChildren().Select<Transform, GameObject>((Func<Transform, GameObject>) (x => ((Component) x).gameObject)).ToArray<GameObject>();
    if (after < before)
    {
      array[0].SetActive(true);
      array[1].SetActive(false);
    }
    else if (after > before)
    {
      array[0].SetActive(false);
      array[1].SetActive(true);
    }
    else
    {
      array[0].SetActive(false);
      array[1].SetActive(false);
    }
  }

  private void SetBattleParam(int duelCount)
  {
    duelCount = Mathf.Min(duelCount, 4);
    DuelColosseumResult duelColosseumResult = this.result.duelResult[duelCount];
    bool flag = duelColosseumResult.player != (BL.Unit) null && duelColosseumResult.opponent != (BL.Unit) null;
    if (duelColosseumResult.player != (BL.Unit) null)
    {
      ((Component) this.playerElementIcon).gameObject.SetActive(true);
      this.playerElementIcon.Init(duelColosseumResult.player.unit.kind, duelColosseumResult.player.GetElement());
      ((Component) this.TxtCharaname).gameObject.SetActive(false);
      ((Component) this.TxtCharaname_ElementOn).gameObject.SetActive(true);
      this.TxtCharaname_ElementOn.SetTextLocalize(duelColosseumResult.player.playerUnit.unit.name);
      if (Object.op_Inequality((Object) this.slcCountryOwn, (Object) null))
      {
        ((Component) this.slcCountryOwn).gameObject.SetActive(false);
        if (duelColosseumResult.player.unit.country_attribute.HasValue)
        {
          ((Component) this.slcCountryOwn).gameObject.SetActive(true);
          duelColosseumResult.player.unit.SetCuntrySpriteName(ref this.slcCountryOwn);
        }
      }
      this.TxtWeaponNameOwn1.SetTextLocalize(this.GetWeaponName(duelColosseumResult.player.playerUnit));
      this.TxtHPOwn.SetTextLocalize(duelColosseumResult.player.hp.ToLocalizeNumberText());
      float num1 = 0.0f;
      if (flag && duelColosseumResult.playerAttackStatus != null)
      {
        num1 = duelColosseumResult.playerAttackStatus.duelParameter.DamageRate;
        duelColosseumResult.playerAttackStatus.duelParameter.DamageRate *= duelColosseumResult.playerAttackStatus.elementAttackRate * duelColosseumResult.playerAttackStatus.attackClassificationRate * duelColosseumResult.playerAttackStatus.normalDamageRate;
      }
      this.TxtAttackOwn.SetTextLocalize(!flag || duelColosseumResult.playerAttackStatus == null ? "-" : Mathf.Max(Mathf.FloorToInt(duelColosseumResult.playerAttackStatus.originalAttack), 1).ToLocalizeNumberText());
      if (flag && duelColosseumResult.playerAttackStatus != null)
        duelColosseumResult.playerAttackStatus.duelParameter.DamageRate = num1;
      this.TxtDexterityOwn.SetTextLocalize(!flag || duelColosseumResult.playerAttackStatus == null ? "-" : duelColosseumResult.playerAttackStatus.dexerityDisplay().ToLocalizeNumberText() + "％");
      this.TxtCriticalOwn.SetTextLocalize(!flag || duelColosseumResult.playerAttackStatus == null ? "-" : duelColosseumResult.playerAttackStatus.criticalDisplay().ToLocalizeNumberText() + "％");
      this.SetGearNameAnimation(duelColosseumResult.player.playerUnit, this.playerGearKindIcon1, this.playerGearKindIcon2, this.TxtWeaponNameOwn1, this.TxtWeaponNameOwn2, this.tweenAlphaFirstWeaponOwn, this.tweenAlphaSecondWeaponOwn);
      int num2 = !flag || duelColosseumResult.playerAttackStatus == null ? 1 : NC.Clamp(1, 4, duelColosseumResult.playerAttackStatus.attackCount);
      if (duelColosseumResult.player.playerUnit.getDualWieldSkillData() != null)
        num2 /= duelColosseumResult.player.playerUnit.normalAttackCount;
      for (int index = 0; index < this.playerAttackValues.Length; ++index)
        this.playerAttackValues[index].SetActive(flag && index + 2 == num2);
      if (flag && duelColosseumResult.playerAttackStatus != null && duelColosseumResult.playerBeforeBonusParam != null)
      {
        this.SetBuff(this.statusBuffOwn, duelColosseumResult.playerBeforeBonusParam.HP, duelColosseumResult.player.hp);
        this.SetBuff(this.attackBuffOwn, duelColosseumResult.playerBeforeBonusParam.attack, duelColosseumResult.playerAttackStatus.attack);
        this.SetBuff(this.dexterityBuffOwn, duelColosseumResult.playerBeforeBonusParam.dexerityDisplay, duelColosseumResult.playerAttackStatus.dexerityDisplay());
        this.SetBuff(this.criticalBuffOwn, duelColosseumResult.playerBeforeBonusParam.criticalDisplay, duelColosseumResult.playerAttackStatus.criticalDisplay());
        this.SetValueColor(this.TxtAttackOwn, duelColosseumResult.playerBeforeBonusParam.attack, duelColosseumResult.playerAttackStatus.attack);
        this.SetValueColor(this.TxtDexterityOwn, duelColosseumResult.playerBeforeBonusParam.dexerityDisplay, duelColosseumResult.playerAttackStatus.dexerityDisplay());
        this.SetValueColor(this.TxtCriticalOwn, duelColosseumResult.playerBeforeBonusParam.criticalDisplay, duelColosseumResult.playerAttackStatus.criticalDisplay());
      }
      else
      {
        this.SetBuff(this.criticalBuffOwn, 0, 0);
        this.SetBuff(this.dexterityBuffOwn, 0, 0);
        this.SetBuff(this.attackBuffOwn, 0, 0);
        this.SetBuff(this.statusBuffOwn, 0, 0);
        this.SetValueColor(this.TxtAttackOwn);
        this.SetValueColor(this.TxtDexterityOwn);
        this.SetValueColor(this.TxtCriticalOwn);
      }
    }
    else
    {
      this.TxtCharaname.SetTextLocalize("-");
      this.TxtCharaname_ElementOn.SetTextLocalize("-");
      if (Object.op_Inequality((Object) this.slcCountryOwn, (Object) null))
        ((Component) this.slcCountryOwn).gameObject.SetActive(false);
      this.TxtHPOwn.SetTextLocalize("-");
      this.TxtWeaponNameOwn1.SetTextLocalize("");
      this.TxtWeaponNameOwn2.SetTextLocalize("");
      this.TxtAttackOwn.SetTextLocalize("-");
      this.TxtDexterityOwn.SetTextLocalize("-");
      this.TxtCriticalOwn.SetTextLocalize("-");
      ((IEnumerable<GameObject>) this.playerAttackValues).ForEach<GameObject>((Action<GameObject>) (v => v.SetActive(false)));
      ((UIWidget) this.TxtAttackOwn).color = new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue);
      this.SetBuff(this.criticalBuffOwn, 0, 0);
      this.SetBuff(this.dexterityBuffOwn, 0, 0);
      this.SetBuff(this.attackBuffOwn, 0, 0);
      this.SetBuff(this.statusBuffOwn, 0, 0);
      this.SetValueColor(this.TxtAttackOwn);
      this.SetValueColor(this.TxtDexterityOwn);
      this.SetValueColor(this.TxtCriticalOwn);
      ((Component) this.playerElementIcon).gameObject.SetActive(false);
      ((Component) ((Component) this.playerKind1).GetComponentInChildren<GearKindIcon>(true)).GetComponent<UI2DSprite>().sprite2D = (Sprite) null;
      ((Component) ((Component) this.playerKind2).GetComponentInChildren<GearKindIcon>(true)).GetComponent<UI2DSprite>().sprite2D = (Sprite) null;
    }
    if (duelColosseumResult.opponent != (BL.Unit) null)
    {
      ((Component) this.enemyElementIcon).gameObject.SetActive(true);
      this.enemyElementIcon.Init(duelColosseumResult.opponent.unit.kind, duelColosseumResult.opponent.GetElement());
      ((Component) this.TxtUnitNameEnemy).gameObject.SetActive(false);
      ((Component) this.TxtUnitNameEnemy_ElementOn).gameObject.SetActive(true);
      this.TxtUnitNameEnemy_ElementOn.SetText(duelColosseumResult.opponent.playerUnit.unit.name);
      if (Object.op_Inequality((Object) this.slcCountryEnemy, (Object) null))
      {
        ((Component) this.slcCountryEnemy).gameObject.SetActive(false);
        if (duelColosseumResult.opponent.unit.country_attribute.HasValue)
        {
          ((Component) this.slcCountryEnemy).gameObject.SetActive(true);
          duelColosseumResult.opponent.unit.SetCuntrySpriteName(ref this.slcCountryEnemy);
        }
      }
      this.TxtHPEnemy.SetTextLocalize(duelColosseumResult.opponent.hp.ToLocalizeNumberText());
      float num3 = 0.0f;
      if (flag && duelColosseumResult.opponentAttackStatus != null)
      {
        num3 = duelColosseumResult.opponentAttackStatus.duelParameter.DamageRate;
        duelColosseumResult.opponentAttackStatus.duelParameter.DamageRate *= duelColosseumResult.opponentAttackStatus.elementAttackRate * duelColosseumResult.opponentAttackStatus.attackClassificationRate * duelColosseumResult.playerAttackStatus.normalDamageRate;
      }
      this.TxtAttack.SetTextLocalize(!flag || duelColosseumResult.opponentAttackStatus == null ? "-" : Mathf.Max(Mathf.FloorToInt(duelColosseumResult.opponentAttackStatus.originalAttack), 1).ToLocalizeNumberText());
      if (flag && duelColosseumResult.opponentAttackStatus != null)
        duelColosseumResult.opponentAttackStatus.duelParameter.DamageRate = num3;
      this.TxtDexterityEnemy.SetTextLocalize(!flag || duelColosseumResult.opponentAttackStatus == null ? "-" : duelColosseumResult.opponentAttackStatus.dexerityDisplay().ToLocalizeNumberText() + "％");
      this.TxtCriticalEnemy.SetTextLocalize(!flag || duelColosseumResult.opponentAttackStatus == null ? "-" : duelColosseumResult.opponentAttackStatus.criticalDisplay().ToLocalizeNumberText() + "％");
      this.SetGearNameAnimation(duelColosseumResult.opponent.playerUnit, this.enemyGearKindIcon1, this.enemyGearKindIcon2, this.TxtWeaponNameEnemy1, this.TxtWeaponNameEnemy2, this.tweenAlphaFirstWeaponEnemy, this.tweenAlphaSecondWeaponEnemy);
      int num4 = !flag || duelColosseumResult.opponentAttackStatus == null ? 1 : NC.Clamp(1, 4, duelColosseumResult.opponentAttackStatus.attackCount);
      if (duelColosseumResult.opponent.playerUnit.getDualWieldSkillData() != null)
        num4 /= duelColosseumResult.opponent.playerUnit.normalAttackCount;
      for (int index = 0; index < this.enemyAttackValues.Length; ++index)
        this.enemyAttackValues[index].SetActive(flag && index + 2 == num4);
      if (flag && duelColosseumResult.opponentAttackStatus != null && duelColosseumResult.opponentBeforeBonusParam != null)
      {
        this.SetBuff(this.statusBuffEnemy, duelColosseumResult.opponentBeforeBonusParam.HP, duelColosseumResult.opponent.hp);
        this.SetBuff(this.attackBuffEnemy, duelColosseumResult.opponentBeforeBonusParam.attack, duelColosseumResult.opponentAttackStatus.attack);
        this.SetBuff(this.dexterityBuffEnemy, duelColosseumResult.opponentBeforeBonusParam.dexerityDisplay, duelColosseumResult.opponentAttackStatus.dexerityDisplay());
        this.SetBuff(this.criticalBuffEnemy, duelColosseumResult.opponentBeforeBonusParam.criticalDisplay, duelColosseumResult.opponentAttackStatus.criticalDisplay());
        this.SetValueColor(this.TxtAttack, duelColosseumResult.opponentBeforeBonusParam.attack, duelColosseumResult.opponentAttackStatus.attack);
        this.SetValueColor(this.TxtDexterityEnemy, duelColosseumResult.opponentBeforeBonusParam.dexerityDisplay, duelColosseumResult.opponentAttackStatus.dexerityDisplay());
        this.SetValueColor(this.TxtCriticalEnemy, duelColosseumResult.opponentBeforeBonusParam.criticalDisplay, duelColosseumResult.opponentAttackStatus.criticalDisplay());
      }
      else
      {
        this.SetBuff(this.criticalBuffEnemy, 0, 0);
        this.SetBuff(this.dexterityBuffEnemy, 0, 0);
        this.SetBuff(this.attackBuffEnemy, 0, 0);
        this.SetBuff(this.statusBuffEnemy, 0, 0);
        this.SetValueColor(this.TxtAttack);
        this.SetValueColor(this.TxtDexterityEnemy);
        this.SetValueColor(this.TxtCriticalEnemy);
      }
    }
    else
    {
      this.TxtUnitNameEnemy.SetTextLocalize("-");
      this.TxtUnitNameEnemy_ElementOn.SetText("-");
      if (Object.op_Inequality((Object) this.slcCountryEnemy, (Object) null))
        ((Component) this.slcCountryEnemy).gameObject.SetActive(false);
      this.TxtHPEnemy.SetTextLocalize("-");
      this.TxtWeaponNameEnemy1.SetTextLocalize("");
      this.TxtWeaponNameEnemy2.SetTextLocalize("");
      this.TxtAttack.SetTextLocalize("-");
      this.TxtDexterityEnemy.SetTextLocalize("-");
      this.TxtCriticalEnemy.SetTextLocalize("-");
      ((IEnumerable<GameObject>) this.enemyAttackValues).ForEach<GameObject>((Action<GameObject>) (v => v.SetActive(false)));
      ((UIWidget) this.TxtAttack).color = new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue);
      this.SetBuff(this.criticalBuffEnemy, 0, 0);
      this.SetBuff(this.dexterityBuffEnemy, 0, 0);
      this.SetBuff(this.attackBuffEnemy, 0, 0);
      this.SetBuff(this.statusBuffEnemy, 0, 0);
      this.SetValueColor(this.TxtAttack);
      this.SetValueColor(this.TxtDexterityEnemy);
      this.SetValueColor(this.TxtCriticalEnemy);
      ((Component) this.enemyElementIcon).gameObject.SetActive(false);
      ((Component) ((Component) this.enemyKind1).GetComponentInChildren<GearKindIcon>(true)).GetComponent<UI2DSprite>().sprite2D = (Sprite) null;
      ((Component) ((Component) this.enemyKind2).GetComponentInChildren<GearKindIcon>(true)).GetComponent<UI2DSprite>().sprite2D = (Sprite) null;
    }
    ((IEnumerable<GameObject>) this.playerAttackAdvantage).ForEach<GameObject>((Action<GameObject>) (v => v.SetActive(false)));
    ((IEnumerable<GameObject>) this.enemyAttackAdvantage).ForEach<GameObject>((Action<GameObject>) (v => v.SetActive(false)));
    if (!flag)
      return;
    switch (this.CheckAdvantage(duelColosseumResult.player.playerUnit.unit.kind_GearKind, duelColosseumResult.opponent.playerUnit.unit.kind_GearKind))
    {
      case Coloseum02342Menu.Advantage.HIGHER:
        this.playerAttackAdvantage[0].SetActive(true);
        this.enemyAttackAdvantage[1].SetActive(true);
        break;
      case Coloseum02342Menu.Advantage.LOWER:
        this.playerAttackAdvantage[1].SetActive(true);
        this.enemyAttackAdvantage[0].SetActive(true);
        break;
    }
  }

  private void SetValueColor(UILabel label, int before = 0, int after = 0)
  {
    if (before > after)
      ((UIWidget) label).color = new Color((float) byte.MaxValue, 0.0f, 0.0f);
    else if (before < after)
      ((UIWidget) label).color = new Color((float) byte.MaxValue, (float) byte.MaxValue, 0.0f);
    else
      ((UIWidget) label).color = new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue);
  }

  private Coloseum02342Menu.Advantage CheckAdvantage(int attack, int defense)
  {
    GearKindCorrelations kindCorrelations = ((IEnumerable<GearKindCorrelations>) MasterData.GearKindCorrelationsList).SingleOrDefault<GearKindCorrelations>((Func<GearKindCorrelations, bool>) (x => x.attacker.ID == attack && x.defender.ID == defense));
    if (kindCorrelations == null)
      return Coloseum02342Menu.Advantage.NONE;
    return !kindCorrelations.is_advantage ? Coloseum02342Menu.Advantage.LOWER : Coloseum02342Menu.Advantage.HIGHER;
  }

  private UnitIcon.ColosseumResult[] ChangeResultEnum(DuelColosseumResult result)
  {
    return result.judgment == 0 ? new UnitIcon.ColosseumResult[2]
    {
      UnitIcon.ColosseumResult.DROW,
      UnitIcon.ColosseumResult.DROW
    } : (result.judgment == 1 ? new UnitIcon.ColosseumResult[2]
    {
      UnitIcon.ColosseumResult.WIN,
      UnitIcon.ColosseumResult.LOSE
    } : new UnitIcon.ColosseumResult[2]
    {
      UnitIcon.ColosseumResult.LOSE,
      UnitIcon.ColosseumResult.WIN
    });
  }

  private IEnumerator StartRoundButton(float delay)
  {
    Coloseum02342Menu coloseum02342Menu = this;
    if ((double) delay != 0.0)
      yield return (object) new WaitForSeconds(delay);
    if (!coloseum02342Menu.isSkip)
    {
      coloseum02342Menu.waitTime = 0.0f;
      coloseum02342Menu.roundButton.SetActive(true);
      coloseum02342Menu.roundButtonAnimate.transform.localScale = new Vector3(0.0f, 0.05f, 1f);
      iTween.ScaleTo(coloseum02342Menu.roundButtonAnimate, iTween.Hash(new object[10]
      {
        (object) "x",
        (object) 1f,
        (object) "time",
        (object) 0.05f,
        (object) "easetype",
        (object) (iTween.EaseType) 3,
        (object) "oncomplete",
        (object) "StartRoundButtonY",
        (object) "oncompletetarget",
        (object) ((Component) coloseum02342Menu).gameObject
      }));
    }
  }

  private void StartRoundButtonY()
  {
    iTween.ScaleTo(this.roundButtonAnimate, iTween.Hash(new object[6]
    {
      (object) "y",
      (object) 1f,
      (object) "time",
      (object) 0.2f,
      (object) "easetype",
      (object) (iTween.EaseType) 3
    }));
    this.BtnSkip.SetActive(true);
    this.SkipPermission = true;
  }

  public virtual void IbtnStartBattle()
  {
    this.roundButton.SetActive(false);
    this.SkipPermission = false;
    this.BtnSkip.SetActive(false);
    this.SM.playSE("SE_1030");
    if (this.player == (BL.Unit) null || this.opponent == (BL.Unit) null || this.isSkip)
      this.scene.Reinitialize();
    else
      this.StartCoroutine(this.ChangeDuelScene());
  }

  protected IEnumerator ChangeDuelScene()
  {
    NGDuelManager.actScreen();
    yield return (object) new WaitForSeconds(0.4f);
    yield return (object) new WaitWhile((Func<bool>) (() => Singleton<NGDuelDataManager>.GetInstance().IsBackgroundPreloading()));
    Battle0181Scene.changeSceneForColossuem(this.result.duelResult[this.dueleCount], true);
  }

  private void AnimateTargets(int id, bool isPlay = true, bool isReverse = false)
  {
    ((IEnumerable<UITweener>) ((Component) this).gameObject.GetComponentsInChildren<UITweener>()).Where<UITweener>((Func<UITweener, bool>) (v => v.tweenGroup == id)).ForEach<UITweener>((Action<UITweener>) (v2 =>
    {
      if (isReverse)
      {
        v2.PlayReverse();
      }
      else
      {
        v2.ResetToBeginning();
        if (!isPlay)
          return;
        v2.PlayForward();
      }
    }));
  }

  public IEnumerator UpdateDuelist(int index)
  {
    DuelColosseumResult resultOne = this.result.duelResult[Mathf.Min(index, 4)];
    this.AnimateTargets(15, false);
    this.AnimateTargets(16, false);
    IEnumerator e = this.SetUnitCharacter(resultOne);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SetBattleParam(index);
    this.SetBonus(resultOne.playerActiveBonus, this.dirBonusOwn, this.txtBonusOwn);
    this.SetBonus(resultOne.opponentActiveBonus, this.dirBonusEnemy, this.txtBonusEnemy);
  }

  private void SetBonus(Bonus[] bonus, GameObject go, UILabel label)
  {
    if (bonus != null && bonus.Length != 0)
    {
      new BonusDisplay().Set(bonus, label, (UILabel) null);
      go.SetActive(true);
    }
    else
      go.SetActive(false);
  }

  public virtual void GoToResult()
  {
    Colosseum02346Scene.ChangeScene(this.info, this.result, this.gladiator);
  }

  private IEnumerator SetEmblem(UI2DSprite emblem, int id = 0)
  {
    Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(id);
    IEnumerator e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    emblem.sprite2D = sprF.Result;
  }

  public void IbtnBack()
  {
  }

  public override void onBackButton() => this.showBackKeyToast();

  public void StartSkipMode()
  {
    if (!this.SkipPermission)
      return;
    this.isSkip = true;
    this.IbtnStartBattle();
  }

  public virtual void Replay()
  {
    this.isSkip = false;
    this.BtnSkip.SetActive(true);
    this.ResetUnitIcons(this.playerUnitIconC);
    this.ResetUnitIcons(this.enemyUnitIconC);
    ((Component) this.resultAnimations[0]).gameObject.SetActive(false);
    ((Component) this.resultAnimations[1]).gameObject.SetActive(false);
    ((Component) this.ibtnResult).gameObject.SetActive(false);
    if (Object.op_Inequality((Object) this.battleResults, (Object) null))
    {
      this.battleResults.dispReplay(false);
      this.dirResult.gameObject.SetActive(false);
    }
    this.scene.ReplayScene();
  }

  protected void ResetUnitIcons(List<UnitIcon> unitIcons)
  {
    unitIcons.ForEach((Action<UnitIcon>) (x => x.SetColosseumResult(UnitIcon.ColosseumResult.NONE)));
    foreach (Component unitIcon in unitIcons)
    {
      foreach (Object componentsInChild in unitIcon.GetComponentsInChildren<iTween>())
        Object.Destroy(componentsInChild);
    }
  }

  private string GetWeaponName(PlayerUnit unit)
  {
    string weaponName = string.Empty;
    if (unit.equippedGear != (PlayerItem) null)
      weaponName = unit.equippedGearName;
    else if (unit.equippedGear3 != (PlayerItem) null)
      weaponName = unit.equippedGearName3;
    else if (unit.equippedGear2 != (PlayerItem) null)
      weaponName = unit.equippedGearName2;
    return weaponName;
  }

  private CommonElement GetWeaponElement(PlayerUnit unit)
  {
    return !(unit.equippedGear != (PlayerItem) null) ? (!(unit.equippedGear2 != (PlayerItem) null) ? unit.equippedGearOrInitial.GetElement() : unit.equippedGear2.GetElement()) : unit.equippedGear.GetElement();
  }

  private GearKind GetWeaponKind(PlayerUnit unit)
  {
    return !(unit.equippedGear != (PlayerItem) null) ? unit.equippedGear2OrInitial.kind : unit.equippedGearOrInitial.kind;
  }

  private void SetGearNameAnimation(
    PlayerUnit pu,
    GearKindIcon kindIcon1,
    GearKindIcon kindIcon2,
    UILabel weapon1,
    UILabel weapon2,
    TweenAlpha tweenAlphaFirstWeapon,
    TweenAlpha tweenAlphaSecondWeapon)
  {
    if (Object.op_Equality((Object) tweenAlphaFirstWeapon, (Object) null) || Object.op_Equality((Object) tweenAlphaSecondWeapon, (Object) null))
      return;
    PlayerItem equippedGear = pu.equippedGear;
    PlayerItem equippedGear2 = pu.equippedGear2;
    ((Component) tweenAlphaFirstWeapon).gameObject.SetActive(true);
    ((Component) tweenAlphaSecondWeapon).gameObject.SetActive(equippedGear2 != (PlayerItem) null);
    ((Behaviour) tweenAlphaFirstWeapon).enabled = false;
    ((Behaviour) tweenAlphaSecondWeapon).enabled = false;
    ((Component) kindIcon1).gameObject.SetActive(false);
    ((Component) kindIcon2).gameObject.SetActive(false);
    if (equippedGear != (PlayerItem) null && equippedGear2 != (PlayerItem) null)
    {
      ((UITweener) tweenAlphaFirstWeapon).ResetToBeginning();
      ((UITweener) tweenAlphaSecondWeapon).ResetToBeginning();
      ((UITweener) tweenAlphaFirstWeapon).PlayForward();
      ((UITweener) tweenAlphaSecondWeapon).PlayForward();
      weapon1.SetTextLocalize(equippedGear.name);
      weapon2.SetTextLocalize(equippedGear2.name);
      ((Component) kindIcon1).gameObject.SetActive(true);
      kindIcon1.Init(equippedGear.gear.kind, equippedGear.gear.GetElement());
      ((Component) kindIcon2).gameObject.SetActive(true);
      kindIcon2.Init(equippedGear2.gear.kind, equippedGear2.gear.GetElement());
    }
    else if (pu.unit.awake_unit_flag)
    {
      if (equippedGear == (PlayerItem) null && equippedGear2 != (PlayerItem) null)
      {
        ((Component) tweenAlphaSecondWeapon).gameObject.SetActive(false);
        ((UIRect) ((Component) tweenAlphaFirstWeapon).GetComponent<UIWidget>()).alpha = 1f;
        weapon1.SetTextLocalize(equippedGear2.name);
        ((Component) kindIcon1).gameObject.SetActive(true);
        kindIcon1.Init(equippedGear2.gear.kind, equippedGear2.gear.GetElement());
      }
      else if (equippedGear != (PlayerItem) null && equippedGear2 == (PlayerItem) null)
      {
        ((Component) tweenAlphaSecondWeapon).gameObject.SetActive(false);
        ((UIRect) ((Component) tweenAlphaFirstWeapon).GetComponent<UIWidget>()).alpha = 1f;
        weapon1.SetTextLocalize(equippedGear.name);
        ((Component) kindIcon1).gameObject.SetActive(true);
        kindIcon1.Init(equippedGear.gear.kind, equippedGear.gear.GetElement());
      }
      else
      {
        GearGear initialGear = pu.initial_gear;
        ((UIRect) ((Component) tweenAlphaFirstWeapon).GetComponent<UIWidget>()).alpha = 1f;
        ((UIRect) ((Component) tweenAlphaSecondWeapon).GetComponent<UIWidget>()).alpha = 0.0f;
        weapon1.SetTextLocalize(initialGear.name);
        ((Component) kindIcon1).gameObject.SetActive(true);
        kindIcon1.Init(initialGear.kind, initialGear.GetElement());
      }
    }
    else
    {
      string empty = string.Empty;
      GearGear gearGear;
      string name;
      if (equippedGear == (PlayerItem) null)
      {
        gearGear = pu.initial_gear;
        name = pu.initial_gear.name;
      }
      else
      {
        gearGear = equippedGear.gear;
        name = equippedGear.name;
      }
      ((UIRect) ((Component) tweenAlphaFirstWeapon).GetComponent<UIWidget>()).alpha = 1f;
      ((UIRect) ((Component) tweenAlphaSecondWeapon).GetComponent<UIWidget>()).alpha = 0.0f;
      weapon1.SetTextLocalize(name);
      ((Component) kindIcon1).gameObject.SetActive(true);
      kindIcon1.Init(gearGear.kind, gearGear.GetElement());
    }
  }

  private enum Advantage
  {
    NONE,
    HIGHER,
    LOWER,
  }

  private enum DuelEffects
  {
    STAYED,
    RUNKUP,
    RUNKDOW,
  }

  private enum ResultEffects
  {
    LOSE,
    WIN,
  }

  private enum States
  {
    DEFAULT,
    FIRST_DUEL,
    FINAL_DUEL,
    DUEL,
    WALKOVER,
    DUEL_RESULT,
  }
}
