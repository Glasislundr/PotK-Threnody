// Decompiled with JetBrains decompiler
// Type: Unit004ReincarnationPage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnitTraining;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/Training/ReincarnationPage")]
public class Unit004ReincarnationPage : Unit004TrainingPage
{
  private bool isResetRecord_;
  private GameObject changeCntlPrefab_;
  private GameObject transPopupPrefab_;
  private GameObject recordPopupPrefab_;
  private long versionPlayerUnits_;
  private Unit004ReincarnationPage.TransMode transMode;
  [SerializeField]
  private Unit004TrainingUnitStatus beforeUnit_;
  private Unit00499ReincarnationChange reincarnationChangeCtrl_;
  private Unit00499UnitStatus afterUnit_;
  private Unit00499UnitStatus recordUnit_;
  [SerializeField]
  private GameObject dialogBox_;
  [SerializeField]
  private UILabel txtMaterialName_;
  [SerializeField]
  private UILabel txtMaterialPlace_;
  [SerializeField]
  private UIButton ibtn_Save_Memory_;
  [SerializeField]
  private GameObject dirTransmigration_;
  [SerializeField]
  private GameObject comShortage_;
  [SerializeField]
  private GameObject[] comShortages_;
  [SerializeField]
  private UILabel txtShortageLevel_;
  [SerializeField]
  private UILabel[] lnkTransUnitsPossessionLabel_;
  [SerializeField]
  private GameObject[] lnkTransUnits_;
  [SerializeField]
  private UIButton transBtn_;
  [SerializeField]
  private UIButton recordBtn_;
  private Unit004TrainingPage.ErrorFlag errFlags_;
  private GameObject statusUpPrefab_;
  private GameObject saveMemorySlotSelectPrefab_;
  private GameObject memorySlotOverwritePrefab_;
  private GameObject memorySlotListPrefab_;
  [SerializeField]
  private Transform afterUnitBase_;
  private UnitUnit baseUnitUnit_;
  private UnitUnit targetUnitUnit_;
  private PlayerUnit recordPlayerUnit_;
  private UnitTransmigrationPattern transmigratePattern_;
  private int price_;
  private UnitUnit[] transmigrateUnits_;
  private List<int> materialUnitIds_;
  private List<int> materialMaterialUnitIds_;
  private bool isActivePopup_;

  public override TrainingType page => TrainingType.Reincarnation;

  protected override IEnumerator doInitialize()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004ReincarnationPage reincarnationPage = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      reincarnationPage.isResetBase_ = false;
      reincarnationPage.isResetRecord_ = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    reincarnationPage.isResetBase_ = true;
    reincarnationPage.isResetRecord_ = true;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) reincarnationPage.Init();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  protected override IEnumerator loadResources()
  {
    Unit004ReincarnationPage reincarnationPage = this;
    yield return (object) reincarnationPage.doLoadCommonPrefab();
    Future<GameObject> prefabF;
    if (Object.op_Equality((Object) reincarnationPage.changeCntlPrefab_, (Object) null))
    {
      prefabF = Res.Prefabs.unit004_9_9.dir_Reincarnation_Chenge_Status_anim.Load<GameObject>();
      yield return (object) prefabF.Wait();
      reincarnationPage.changeCntlPrefab_ = prefabF.Result;
    }
    if (Object.op_Equality((Object) reincarnationPage.statusUpPrefab_, (Object) null))
    {
      prefabF = Res.Prefabs.unit004_9_9.dir_Uppt.Load<GameObject>();
      yield return (object) prefabF.Wait();
      reincarnationPage.statusUpPrefab_ = prefabF.Result;
    }
    if (Object.op_Equality((Object) reincarnationPage.saveMemorySlotSelectPrefab_, (Object) null))
    {
      prefabF = Res.Prefabs.popup.popup_004_save_memory_slot_select__anim_popup01.Load<GameObject>();
      yield return (object) prefabF.Wait();
      reincarnationPage.saveMemorySlotSelectPrefab_ = prefabF.Result;
    }
    if (Object.op_Equality((Object) reincarnationPage.memorySlotOverwritePrefab_, (Object) null))
    {
      prefabF = Res.Prefabs.popup.popup_004_memory_slot_overwrite__anim_popup01.Load<GameObject>();
      yield return (object) prefabF.Wait();
      reincarnationPage.memorySlotOverwritePrefab_ = prefabF.Result;
    }
    if (Object.op_Equality((Object) reincarnationPage.transPopupPrefab_, (Object) null))
    {
      prefabF = Res.Prefabs.popup.popup_004_13_1__anim_popup01.Load<GameObject>();
      yield return (object) prefabF.Wait();
      reincarnationPage.transPopupPrefab_ = prefabF.Result;
    }
    if (Object.op_Equality((Object) reincarnationPage.recordPopupPrefab_, (Object) null))
    {
      prefabF = Res.Prefabs.popup.popup_004_13_1__anim_popup01.Load<GameObject>();
      yield return (object) prefabF.Wait();
      reincarnationPage.recordPopupPrefab_ = prefabF.Result;
    }
  }

  protected override void preChangeTarget(Ingredients targetNext, bool bModifiedBase)
  {
  }

  protected override IEnumerator doChange(bool modifiedBase)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004ReincarnationPage reincarnationPage = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      reincarnationPage.isResetBase_ = false;
      reincarnationPage.isResetRecord_ = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    reincarnationPage.isResetBase_ = modifiedBase;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) reincarnationPage.Init();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  protected override void onEnabledEffectFinished()
  {
    base.onEnabledEffectFinished();
    this.StartCoroutine(this.doWaitShowAdvice());
  }

  private IEnumerator doWaitShowAdvice()
  {
    Unit004ReincarnationPage reincarnationPage = this;
    while (Singleton<CommonRoot>.GetInstance().isLoading)
      yield return (object) null;
    reincarnationPage.showAdvice("integralnoah_reincarnation_tutorial");
  }

  protected override bool isDisabledAfterUnitIconButton
  {
    get => this.errFlags_.IsOn(Unit004TrainingPage.ErrorFlag.Level);
  }

  private PlayerUnit baseUnit => this.target_.baseUnit;

  private PlayerUnit targetUnit => this.target_.estimatesReincarnation;

  private IEnumerator Init()
  {
    Unit004ReincarnationPage reincarnationPage = this;
    if (!reincarnationPage.isResetBase_)
    {
      if (reincarnationPage.isResetRecord_)
        yield return (object) reincarnationPage.doInitAfter();
      reincarnationPage.setEnabledBottomButtons(reincarnationPage.price_);
    }
    else
    {
      int num1 = (int) reincarnationPage.errFlags_.Reset();
      reincarnationPage.baseUnitUnit_ = reincarnationPage.baseUnit.unit;
      reincarnationPage.transmigratePattern_ = reincarnationPage.baseUnitUnit_.TransmigratePattern;
      reincarnationPage.transmigrateUnits_ = reincarnationPage.baseUnitUnit_.TransmigrateUnits;
      reincarnationPage.versionPlayerUnits_ = SMManager.Revision<PlayerUnit[]>();
      reincarnationPage.materialUnitIds_ = reincarnationPage.getMaterialUnitIDs(reincarnationPage.baseUnit, SMManager.Get<PlayerUnit[]>(), reincarnationPage.transmigrateUnits_);
      reincarnationPage.materialMaterialUnitIds_ = reincarnationPage.getMaterialMaterialUnitIDs(SMManager.Get<PlayerMaterialUnit[]>(), reincarnationPage.transmigrateUnits_);
      if (Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
      {
        UnitRarity rarity = reincarnationPage.baseUnitUnit_.rarity;
        if (reincarnationPage.baseUnit.level < rarity.reincarnation_level)
        {
          int num2 = (int) reincarnationPage.errFlags_.On(Unit004TrainingPage.ErrorFlag.Level);
          reincarnationPage.txtShortageLevel_.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT004TRAINING_SHORTAGE_LEVEL, (IDictionary) new Hashtable()
          {
            {
              (object) "level",
              (object) rarity.reincarnation_level
            }
          }));
        }
        if (reincarnationPage.errFlags_.IsOn(Unit004TrainingPage.ErrorFlag.Any))
        {
          reincarnationPage.target_.estimatesReincarnation = reincarnationPage.baseUnit;
        }
        else
        {
          Future<WebAPI.Response.UnitTransmigrateParameter> wapp = WebAPI.UnitTransmigrateParameter(reincarnationPage.baseUnit.id, reincarnationPage.transmigratePattern_.ID, (Action<WebAPI.Response.UserError>) (e =>
          {
            WebAPI.DefaultUserErrorCallback(e);
            Singleton<NGSceneManager>.GetInstance().changeScene("mypage", false);
          }));
          yield return (object) wapp.Wait();
          if (wapp.Result == null)
          {
            yield break;
          }
          else
          {
            reincarnationPage.target_.estimatesReincarnation = wapp.Result.target_player_unit;
            reincarnationPage.targetUnit.id = -1;
            reincarnationPage.targetUnitUnit_ = reincarnationPage.targetUnit.unit;
            wapp = (Future<WebAPI.Response.UnitTransmigrateParameter>) null;
          }
        }
      }
      else
        reincarnationPage.target_.estimatesReincarnation = Singleton<TutorialRoot>.GetInstance().Resume.after_transmigrate_player_unit;
      reincarnationPage.dialogBox_.SetActive(false);
      reincarnationPage.dirTransmigration_.SetActive(true);
      while (reincarnationPage.isLoadingResources)
        yield return (object) null;
      yield return (object) reincarnationPage.doInitTransmigrationUnits(reincarnationPage.lnkTransUnitsPossessionLabel_, reincarnationPage.lnkTransUnits_);
      SMManager.Get<Player>();
      NGGameDataManager.Boost boostInfo = Singleton<NGGameDataManager>.GetInstance().BoostInfo;
      Decimal num3 = boostInfo == null ? 1.0M : boostInfo.DiscountUnitTransmigrate;
      reincarnationPage.price_ = (int) ((Decimal) reincarnationPage.transmigratePattern_.price * num3);
      reincarnationPage.setCostZeny((long) reincarnationPage.price_);
      yield return (object) reincarnationPage.doInitBefore();
      yield return (object) reincarnationPage.doInitAfter();
      reincarnationPage.setEnabledBottomButtons(reincarnationPage.price_);
    }
  }

  private void showTransmigrationPopup()
  {
    this.isActivePopup_ = true;
    Singleton<PopupManager>.GetInstance().open(this.transPopupPrefab_).GetComponent<Popup004131Menu>().Init(new Action<bool>(this.onResultTransmigration));
  }

  private void showRecordTransmigrationPopup()
  {
    this.isActivePopup_ = true;
    Singleton<PopupManager>.GetInstance().open(this.recordPopupPrefab_).GetComponent<Popup004131Menu>().Init(new Action<bool>(this.onResultRecord), true);
  }

  private void onResultTransmigration(bool bYes)
  {
    this.isActivePopup_ = false;
    if (!bYes)
      return;
    this.StartCoroutine(this.doTransmigration(false));
  }

  private void onResultRecord(bool bYes)
  {
    this.isActivePopup_ = false;
    if (!bYes)
      return;
    this.StartCoroutine(this.doTransmigration(true));
  }

  private IEnumerator doTransmigration(bool isRecod)
  {
    Unit004ReincarnationPage reincarnationPage = this;
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Future<WebAPI.Response.UnitTransmigrate> paramF = WebAPI.UnitTransmigrate(reincarnationPage.baseUnit.id, isRecod, reincarnationPage.materialMaterialUnitIds_.ToArray(), reincarnationPage.materialUnitIds_.ToArray(), reincarnationPage.baseUnit.unit.TransmigratePattern.ID, (Action<WebAPI.Response.UserError>) (error =>
      {
        WebAPI.DefaultUserErrorCallback(error);
        Singleton<NGSceneManager>.GetInstance().changeScene("mypage", false);
      }));
      IEnumerator e = paramF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (paramF.Result != null)
      {
        Singleton<NGGameDataManager>.GetInstance().corps_player_unit_ids = new HashSet<int>((IEnumerable<int>) paramF.Result.corps_player_unit_ids);
        e = OnDemandDownload.WaitLoadHasUnitResource(false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        PlayerUnit result = (PlayerUnit) null;
        foreach (PlayerUnit playerUnit in SMManager.Get<PlayerUnit[]>())
        {
          if (playerUnit.id == reincarnationPage.baseUnit.id)
          {
            result = playerUnit;
            break;
          }
        }
        List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
        foreach (GameObject lnkTransUnit in reincarnationPage.lnkTransUnits_)
        {
          UnitIcon componentInChildren = ((Component) lnkTransUnit.GetComponent<UI2DSprite>()).GetComponentInChildren<UnitIcon>();
          if (componentInChildren.PlayerUnit != (PlayerUnit) null)
            playerUnitList.Add(componentInChildren.PlayerUnit);
        }
        PrincesEvolutionParam princesEvolutionParam = new PrincesEvolutionParam();
        princesEvolutionParam.materiaqlUnits = playerUnitList;
        princesEvolutionParam.is_new = false;
        princesEvolutionParam.baseUnit = reincarnationPage.baseUnit;
        princesEvolutionParam.resultUnit = result;
        princesEvolutionParam.mode = Unit00499Scene.Mode.Transmigration;
        reincarnationPage.setBackSceneFromResult(result);
        unit00497Scene.ChangeScene(true, princesEvolutionParam);
        paramF = (Future<WebAPI.Response.UnitTransmigrate>) null;
      }
    }
  }

  private void DrawTransButton()
  {
    ((Component) this.transBtn_).gameObject.SetActive(true);
    ((Component) this.recordBtn_).gameObject.SetActive(false);
    this.setErrorStatus(false);
    this.transMode = Unit004ReincarnationPage.TransMode.Trans;
  }

  private void DrawRecordButton()
  {
    ((Component) this.transBtn_).gameObject.SetActive(false);
    ((Component) this.recordBtn_).gameObject.SetActive(true);
    this.setErrorStatus(true);
    this.transMode = Unit004ReincarnationPage.TransMode.Record;
  }

  private IEnumerator doInitBefore()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004ReincarnationPage reincarnationPage = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      reincarnationPage.beforeUnit_.SetStatusText(reincarnationPage.baseUnit);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) reincarnationPage.initUnitIcon(reincarnationPage.beforeUnit_.lnkUnit, reincarnationPage.baseUnit, true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator doInitAfter()
  {
    Unit004ReincarnationPage reincarnationPage = this;
    if (Object.op_Equality((Object) reincarnationPage.afterUnit_, (Object) null))
    {
      GameObject gameObject = reincarnationPage.changeCntlPrefab_.Clone(reincarnationPage.afterUnitBase_);
      reincarnationPage.reincarnationChangeCtrl_ = gameObject.GetComponent<Unit00499ReincarnationChange>();
      reincarnationPage.reincarnationChangeCtrl_.SetAction(new Action(reincarnationPage.DrawTransButton), new Action(reincarnationPage.DrawRecordButton));
      reincarnationPage.afterUnit_ = reincarnationPage.reincarnationChangeCtrl_.AfterUnit;
      reincarnationPage.afterUnit_.mode = Unit00499Scene.Mode.Transmigration;
      reincarnationPage.recordUnit_ = reincarnationPage.reincarnationChangeCtrl_.RecordUnit;
      if (PlayerUnitTransMigrateMemoryList.Current == null && Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
      {
        IEnumerator e = WebAPI.UnitListTransmigrateMemory().Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    PlayerUnit target = reincarnationPage.targetUnit;
    yield return (object) reincarnationPage.initUnitIcon(reincarnationPage.afterUnit_.linkUnit, target, false);
    int base_id = reincarnationPage.baseUnit.id;
    List<PlayerUnit> source = PlayerTransmigrateMemoryPlayerUnitIds.Current != null ? PlayerTransmigrateMemoryPlayerUnitIds.Current.PlayerUnits() : new List<PlayerUnit>();
    reincarnationPage.recordPlayerUnit_ = source.FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.id == base_id));
    if (reincarnationPage.recordPlayerUnit_ != (PlayerUnit) null)
    {
      if (reincarnationPage.errFlags_.IsOff(Unit004TrainingPage.ErrorFlag.Level))
      {
        ((Component) reincarnationPage.afterUnit_).gameObject.SetActive(true);
        ((Component) reincarnationPage.recordUnit_).gameObject.SetActive(true);
        reincarnationPage.reincarnationChangeCtrl_.SetActiveChangeButton(true);
        reincarnationPage.DrawTransButton();
        reincarnationPage.reincarnationChangeCtrl_.AfterFront();
      }
      else
      {
        ((Component) reincarnationPage.recordUnit_).gameObject.SetActive(true);
        ((Component) reincarnationPage.afterUnit_).gameObject.SetActive(false);
        reincarnationPage.reincarnationChangeCtrl_.SetActiveChangeButton(false);
        reincarnationPage.DrawRecordButton();
        reincarnationPage.reincarnationChangeCtrl_.RecordFront();
      }
      yield return (object) reincarnationPage.initUnitIcon(reincarnationPage.recordUnit_.linkUnit, reincarnationPage.recordPlayerUnit_, false, afterNo: Unit004TrainingPage.SpriteNo.Memory);
    }
    else
    {
      reincarnationPage.reincarnationChangeCtrl_.SetActiveChangeButton(false);
      ((Component) reincarnationPage.afterUnit_).gameObject.SetActive(true);
      ((Component) reincarnationPage.recordUnit_).gameObject.SetActive(false);
      reincarnationPage.DrawTransButton();
      reincarnationPage.reincarnationChangeCtrl_.AfterFront();
    }
    reincarnationPage.afterUnit_.SetStatusText(target, true);
    if (reincarnationPage.recordPlayerUnit_ != (PlayerUnit) null)
      reincarnationPage.recordUnit_.SetStatusTextMemory(reincarnationPage.recordPlayerUnit_);
    reincarnationPage.SetTransUpStatus(reincarnationPage.afterUnit_.TransUpParameter[0], reincarnationPage.statusUpPrefab_, target.hp.transmigrate - reincarnationPage.baseUnit.hp.transmigrate);
    reincarnationPage.SetTransUpStatus(reincarnationPage.afterUnit_.TransUpParameter[1], reincarnationPage.statusUpPrefab_, target.strength.transmigrate - reincarnationPage.baseUnit.strength.transmigrate);
    reincarnationPage.SetTransUpStatus(reincarnationPage.afterUnit_.TransUpParameter[3], reincarnationPage.statusUpPrefab_, target.vitality.transmigrate - reincarnationPage.baseUnit.vitality.transmigrate);
    reincarnationPage.SetTransUpStatus(reincarnationPage.afterUnit_.TransUpParameter[2], reincarnationPage.statusUpPrefab_, target.intelligence.transmigrate - reincarnationPage.baseUnit.intelligence.transmigrate);
    reincarnationPage.SetTransUpStatus(reincarnationPage.afterUnit_.TransUpParameter[4], reincarnationPage.statusUpPrefab_, target.mind.transmigrate - reincarnationPage.baseUnit.mind.transmigrate);
    reincarnationPage.SetTransUpStatus(reincarnationPage.afterUnit_.TransUpParameter[5], reincarnationPage.statusUpPrefab_, target.agility.transmigrate - reincarnationPage.baseUnit.agility.transmigrate);
    reincarnationPage.SetTransUpStatus(reincarnationPage.afterUnit_.TransUpParameter[6], reincarnationPage.statusUpPrefab_, target.dexterity.transmigrate - reincarnationPage.baseUnit.dexterity.transmigrate);
    reincarnationPage.SetTransUpStatus(reincarnationPage.afterUnit_.TransUpParameter[7], reincarnationPage.statusUpPrefab_, target.lucky.transmigrate - reincarnationPage.baseUnit.lucky.transmigrate);
  }

  private IEnumerator doInitTransmigrationUnits(UILabel[] label, GameObject[] objects)
  {
    Unit004ReincarnationPage reincarnationPage = this;
    foreach (GameObject gameObject in objects)
      gameObject.transform.Clear();
    UnitUnit[] materials = reincarnationPage.transmigrateUnits_;
    Dictionary<int, Queue<PlayerUnit>> playerUnitDic = reincarnationPage.makePlayerUnitMaterials(reincarnationPage.transmigrateUnits_, reincarnationPage.baseUnit.id);
    int cnt = 0;
    List<PlayerUnit> materialUnitList = new List<PlayerUnit>();
    for (int n = 0; n < objects.Length; ++n)
    {
      GameObject gameObject = objects[n];
      UnitUnit unit = n < materials.Length ? materials[n] : (UnitUnit) null;
      UnitIcon component = reincarnationPage.unitIconPrefab_.CloneAndGetComponent<UnitIcon>(gameObject.transform);
      IEnumerator enumerator;
      if (unit != null)
      {
        Queue<PlayerUnit> playerUnitQueue;
        if (playerUnitDic.TryGetValue(unit.ID, out playerUnitQueue))
        {
          PlayerUnit playerUnit = playerUnitQueue.Dequeue();
          if (playerUnitQueue.Count == 0)
            playerUnitDic.Remove(unit.ID);
          materialUnitList.Add(playerUnit);
          enumerator = component.SetMaterialUnit(playerUnit, false, (PlayerUnit[]) null);
          if (playerUnit.favorite)
          {
            int num = (int) reincarnationPage.errFlags_.On(Unit004TrainingPage.ErrorFlag.Favorite);
          }
        }
        else
        {
          enumerator = component.SetUnit(unit, unit.GetElement(), true);
          component.setLevelText("1");
          int num = (int) reincarnationPage.errFlags_.On(Unit004TrainingPage.ErrorFlag.Material);
        }
        ++cnt;
      }
      else
      {
        enumerator = component.SetPlayerUnit((PlayerUnit) null, (PlayerUnit[]) null, (PlayerUnit) null, true, false);
        component.BackgroundModeValue = UnitIcon.BackgroundMode.PlayerShadow;
      }
      yield return (object) enumerator;
    }
    PlayerUnit[] source = SMManager.Get<PlayerUnit[]>();
    PlayerMaterialUnit[] array = SMManager.Get<PlayerMaterialUnit[]>();
    int index = 0;
    foreach (GameObject gameObject in objects)
    {
      UnitIcon icon = ((Component) gameObject.GetComponent<UI2DSprite>()).GetComponentInChildren<UnitIcon>();
      if (index < cnt)
      {
        ((Behaviour) icon.Button).enabled = true;
        ((Collider) icon.buttonBoxCollider).enabled = true;
        int num = 0;
        if (icon.unit.IsMaterialUnit)
        {
          PlayerMaterialUnit playerMaterialUnit = Array.Find<PlayerMaterialUnit>(array, (Predicate<PlayerMaterialUnit>) (x => icon.Unit.ID == x._unit));
          if (playerMaterialUnit != null)
            num = playerMaterialUnit.quantity;
        }
        else
          num = ((IEnumerable<PlayerUnit>) source).Count<PlayerUnit>((Func<PlayerUnit, bool>) (x => icon.Unit.ID == x._unit));
        if (icon.Unit.ID == reincarnationPage.baseUnitUnit_.ID)
          --num;
        label[index].SetTextLocalize(Consts.Format(Consts.GetInstance().unit_004_9_9_possession_text, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) num
          }
        }));
        ((Component) label[index]).gameObject.SetActive(true);
        icon.RarityCenter();
        // ISSUE: reference to a compiler-generated method
        icon.onClick = new Action<UnitIconBase>(reincarnationPage.\u003CdoInitTransmigrationUnits\u003Eb__63_1);
        icon.SetButtonDetailEvent(icon.PlayerUnit, materialUnitList.ToArray());
      }
      else
      {
        ((Behaviour) icon.Button).enabled = false;
        ((Collider) icon.buttonBoxCollider).enabled = false;
        icon.SetEmpty();
        icon.setSilhouette(false);
        ((Component) label[index]).gameObject.SetActive(false);
        gameObject.GetComponentInChildren<UIButton>().onClick.Clear();
      }
      ++index;
    }
  }

  private void SetTransUpStatus(GameObject dst, GameObject go, int value)
  {
    dst.transform.Clear();
    dst.SetActive(false);
    if (value <= 0)
      return;
    dst.SetActive(true);
    go.CloneAndGetComponent<UnitTransAddStatus>(dst.transform).Init(value);
  }

  private void setEnabledBottomButtons(int money)
  {
    Player player = SMManager.Get<Player>();
    if ((long) money > player.money)
    {
      int num = (int) this.errFlags_.On(Unit004TrainingPage.ErrorFlag.Money);
    }
    ((UIButtonColor) this.transBtn_).isEnabled = !this.errFlags_.Any();
    ((UIButtonColor) this.recordBtn_).isEnabled = this.recordPlayerUnit_ != (PlayerUnit) null && this.errFlags_.IsOff(Unit004TrainingPage.ErrorFlag.Money | Unit004TrainingPage.ErrorFlag.Material | Unit004TrainingPage.ErrorFlag.Favorite);
    ((UIButtonColor) this.ibtn_Save_Memory_).isEnabled = this.baseUnit.level == this.baseUnit.max_level;
  }

  private void setErrorStatus(bool bRecord)
  {
    int index = -1;
    if (this.errFlags_.IsOn(Unit004TrainingPage.ErrorFlag.Money))
      index = 0;
    if (this.errFlags_.IsOn(Unit004TrainingPage.ErrorFlag.Material))
      index = 1;
    if (!bRecord && this.errFlags_.IsOn(Unit004TrainingPage.ErrorFlag.Level))
      index = 2;
    if (this.errFlags_.IsOn(Unit004TrainingPage.ErrorFlag.Favorite))
      index = 3;
    if (index >= 0)
    {
      ((IEnumerable<GameObject>) this.comShortages_).ToggleOnce(index);
      this.comShortage_.SetActive(true);
    }
    else
      this.comShortage_.SetActive(false);
  }

  private void ShowMaterialQuestInfo(UnitUnit materail)
  {
    int num = !this.dialogBox_.activeInHierarchy ? 1 : 0;
    this.dialogBox_.SetActive(true);
    if (num != 0)
    {
      UITweener[] tweeners = NGTween.findTweeners(this.dialogBox_, true);
      NGTween.playTweens(tweeners, NGTween.Kind.START_END);
      NGTween.playTweens(tweeners, NGTween.Kind.START);
      foreach (UITweener uiTweener in tweeners)
        uiTweener.onFinished.Clear();
    }
    this.txtMaterialName_.SetText(materail.name);
    UnitMaterialQuestInfo materialQuestInfo = ((IEnumerable<UnitMaterialQuestInfo>) MasterData.UnitMaterialQuestInfoList).SingleOrDefault<UnitMaterialQuestInfo>((Func<UnitMaterialQuestInfo, bool>) (x => x.unit_id == materail.ID));
    if (materialQuestInfo == null)
      this.txtMaterialPlace_.SetText("");
    else
      this.txtMaterialPlace_.SetText(materialQuestInfo.long_desc);
  }

  private UITweener[] endTweensMaterialQuestInfo(bool isForce = false)
  {
    if (!this.dialogBox_.activeInHierarchy)
      return (UITweener[]) null;
    UITweener[] tweeners = NGTween.findTweeners(this.dialogBox_, true);
    if (!isForce && ((IEnumerable<UITweener>) tweeners).Any<UITweener>((Func<UITweener, bool>) (x => ((Behaviour) x).enabled)))
      return (UITweener[]) null;
    NGTween.playTweens(tweeners, NGTween.Kind.START_END, true);
    NGTween.playTweens(tweeners, NGTween.Kind.END);
    return tweeners;
  }

  public void HideMaterialQuestInfo()
  {
    UITweener[] tweens = this.endTweensMaterialQuestInfo();
    if (tweens == null)
      return;
    NGTween.setOnTweenFinished(tweens, (MonoBehaviour) this, "hideDialogBox");
  }

  private void hideDialogBox() => this.dialogBox_.SetActive(false);

  public void IbtnRecord()
  {
    if (PlayerTransmigrateMemoryPlayerUnitIds.Current != null && PlayerTransmigrateMemoryPlayerUnitIds.Current.PlayerUnits().Any<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.id == this.baseUnit.id)))
    {
      if (Object.op_Equality((Object) this.memorySlotOverwritePrefab_, (Object) null))
        return;
      Singleton<PopupManager>.GetInstance().open(this.memorySlotOverwritePrefab_).GetComponent<Unit00499SaveMemoryOverwrite>().Initialize(this.baseUnit, new Action(this.recordEndUpdate));
    }
    else
    {
      if (Object.op_Equality((Object) this.saveMemorySlotSelectPrefab_, (Object) null))
        return;
      this.StartCoroutine(Singleton<PopupManager>.GetInstance().open(this.saveMemorySlotSelectPrefab_).GetComponent<Unit00499SaveMemorySlotSelect>().Initialize(this.baseUnit, new Action(this.recordEndUpdate)));
    }
  }

  private void recordEndUpdate()
  {
    this.isResetRecord_ = true;
    Ingredients ingredients = new Ingredients(TrainingType.Reincarnation);
    int base_id = this.baseUnit.id;
    ingredients.baseUnit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == base_id));
    if (this.versionPlayerUnits_ == SMManager.Revision<PlayerUnit[]>())
      ingredients.estimatesReincarnation = this.target_.estimatesReincarnation;
    this.StartCoroutine(this.mainMenu_.doReset(ingredients));
  }

  public void IbtnTrans()
  {
    if (this.errFlags_.Any() || this.IsPushAndSet())
      return;
    this.StartCoroutine(this.IbtnTransAsync());
  }

  private IEnumerator IbtnTransAsync()
  {
    if (this.baseUnit.tower_is_entry || this.baseUnit.corps_is_entry)
    {
      bool isRejected = false;
      yield return (object) PopupManager.StartTowerEntryUnitWarningPopupProc((Action<bool>) (selected => isRejected = !selected), true);
      if (isRejected)
        yield break;
    }
    this.showTransmigrationPopup();
  }

  public void IbtnRecordTrans()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.IbtnRecordTransAsync());
  }

  private IEnumerator IbtnRecordTransAsync()
  {
    Unit004ReincarnationPage reincarnationPage = this;
    if (reincarnationPage.baseUnit.tower_is_entry || reincarnationPage.baseUnit.corps_is_entry)
    {
      bool isRejected = false;
      yield return (object) PopupManager.StartTowerEntryUnitWarningPopupProc((Action<bool>) (selected => isRejected = !selected), true);
      if (isRejected)
        yield break;
    }
    if (reincarnationPage.baseUnit.is_memory_over)
      ModalWindow.ShowYesNo(Consts.GetInstance().MEMORY_LOAD_ALERT_TITLE, Consts.GetInstance().MEMORY_LOAD_ALERT_DESCRIPTION, new Action(reincarnationPage.showRecordTransmigrationPopup), (Action) (() => { }));
    else
      reincarnationPage.showRecordTransmigrationPopup();
  }

  private enum TransBonusIndex
  {
    HP,
    ATK,
    MAG,
    DEF,
    MEN,
    SPD,
    TEC,
    LUC,
    MAX,
  }

  private enum TransMode
  {
    Trans,
    Record,
  }
}
