// Decompiled with JetBrains decompiler
// Type: Unit0046Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeckOrganization;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit0046/Menu")]
public class Unit0046Menu : BackButtonMenuBase
{
  private DateTime serverTime = DateTime.MinValue;
  protected const float LINK_HEIGHT = 100f;
  protected const float LINK_DEFHEIGHT = 136f;
  protected const float scale = 0.7352941f;
  [SerializeField]
  protected UILabel TxtTitle;
  public GameObject TopObject;
  public GameObject MiddleObject;
  public UILabel TxtCostValue;
  public UILabel TxtCombat;
  public UILabel TxtTeamNum;
  public NGHorizontalScrollParts indicator;
  private PlayerDeck[] playerDecks;
  private GameObject prefabResult;
  private PlayerDeck SelectDeck;
  public GameObject[] playerItems;
  public UIButton[] buttons = new UIButton[5];
  public Transform[] ui3DModelTransfrom;
  public GameObject[] ui3DModelLoadDummy;
  public List<UI3DModel> ui3DModels;
  public List<GameObject> Models = new List<GameObject>();
  public GameObject ui3DModelPrefab;
  private int cost_max;
  public bool play;
  public bool coroutine;
  public bool stop;
  private int old_indicator_select = -1;
  public GameObject backButton;
  public CoroutineData<bool> modelChange;
  public ColosseumUtility.Info info;
  private bool ChangeBackScene;
  private int? lastDeckNumberOnEndScene;
  private GameObject LeaderSkillPopupPrefab;
  [SerializeField]
  private List<SupplyItem> SupplyItems = new List<SupplyItem>();
  [SerializeField]
  private List<SupplyItem> SaveDeck = new List<SupplyItem>();
  private GameObject detailPopup;
  [NonSerialized]
  public Unit0046Scene.LimitationData limitationData;
  [SerializeField]
  private int SELECT_MIN = 1;
  [SerializeField]
  private int SELECT_MAX = 5;
  [SerializeField]
  private Unit0046CustomDeckTutorial buttonLockControl;
  private Coroutine[] ModelLoader = new Coroutine[5];
  private bool isAutoOrganization;
  private int m_windowHeight;
  private int m_windowWidth;
  private RenderTextureRecoveryUtil util;

  public DateTime SevertTime
  {
    get => this.serverTime;
    set => this.serverTime = value;
  }

  public Unit0046Scene.From fromScene { get; set; }

  public bool isSea { get; set; }

  private IEnumerator WaitScrollSe()
  {
    yield return (object) new WaitForSeconds(0.3f);
    this.indicator.SeEnable = true;
  }

  private IEnumerator ShouldStop()
  {
    Unit0046Menu behaviour = this;
    while (behaviour.modelChange == null || behaviour.modelChange.Running)
      yield return (object) null;
    behaviour.modelChange.Stop();
    behaviour.modelChange = behaviour.StartCoroutine<bool>(behaviour.ModelChange());
  }

  private void SetTeamInfo(PlayerDeck deck)
  {
    this.TxtCostValue.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_0046_COST, (IDictionary) new Hashtable()
    {
      {
        (object) "now",
        (object) deck.cost
      },
      {
        (object) "max",
        (object) this.cost_max
      }
    }));
    int combat = 0;
    ((IEnumerable<PlayerUnit>) deck.player_units).ForEach<PlayerUnit>((Action<PlayerUnit>) (x =>
    {
      if (!(x != (PlayerUnit) null))
        return;
      combat += Judgement.NonBattleParameter.FromPlayerUnit(x).Combat;
    }));
    this.TxtCombat.SetTextLocalize(combat.ToString());
    this.TxtTeamNum.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_0046_MENU, (IDictionary) new Hashtable()
    {
      {
        (object) "num",
        (object) (this.indicator.selected + 1)
      }
    }));
  }

  protected override void Update()
  {
    if (this.playerDecks == null || this.old_indicator_select < 0 || this.indicator.selected >= this.playerDecks.Length || this.indicator.selected < 0)
      return;
    base.Update();
    if (this.SelectDeck == null || this.playerDecks[this.indicator.selected] != this.SelectDeck)
    {
      this.SelectDeck = this.playerDecks[this.indicator.selected];
      this.charaActive(this.SelectDeck);
      if (this.old_indicator_select != this.indicator.selected)
      {
        this.StopCoroutine("ShouldStop");
        this.StartCoroutine("ShouldStop");
      }
      this.SetTeamInfo(this.SelectDeck);
    }
    this.util.FixRenderTexture();
    if (this.m_windowHeight == 0 || this.m_windowWidth == 0)
    {
      this.m_windowHeight = Screen.height;
      this.m_windowWidth = Screen.width;
    }
    else
    {
      if (this.m_windowHeight == Screen.height && this.m_windowWidth == Screen.width)
        return;
      this.m_windowHeight = Screen.height;
      this.m_windowWidth = Screen.width;
      this.StopCoroutine("ShouldStop");
      this.StartCoroutine("ShouldStop");
    }
  }

  public IEnumerator Init3DModelPrefab()
  {
    Unit0046Menu behaviour = this;
    Future<GameObject> prefabF = Res.Prefabs.gacha006_8.slc_3DModel.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    behaviour.ui3DModelPrefab = prefabF.Result;
    for (int index = 0; index < behaviour.ui3DModelTransfrom.Length; ++index)
    {
      UI3DModel component = behaviour.ui3DModelPrefab.Clone(behaviour.ui3DModelTransfrom[index]).GetComponent<UI3DModel>();
      component.lightOn = index == 0;
      behaviour.ui3DModels.Add(component);
    }
    while (behaviour.indicator.selected == -1)
      yield return (object) null;
    behaviour.modelChange = behaviour.StartCoroutine<bool>(behaviour.ModelChange());
  }

  private void InitOrSaveRenderTexture()
  {
    if (Object.op_Equality((Object) this.util, (Object) null))
    {
      Debug.Log((object) "Init RenderTextureRecoveryUtil.");
      this.util = ((Component) this).GetComponent<RenderTextureRecoveryUtil>();
    }
    if (!Object.op_Inequality((Object) this.util, (Object) null))
      return;
    Debug.Log((object) "Save RenderTexture Info.");
    this.util.SaveRenderTexture();
  }

  public IEnumerator ModelChange()
  {
    Unit0046Menu unit0046Menu = this;
    for (int index = 0; index < unit0046Menu.buttons.Length; ++index)
    {
      if (index < unit0046Menu.ui3DModels.Count)
        unit0046Menu.ui3DModels[index].Remove();
      unit0046Menu.ui3DModelLoadDummy[index].SetActive(unit0046Menu.playerDecks[unit0046Menu.indicator.selected].player_units[index] != (PlayerUnit) null);
    }
    unit0046Menu.old_indicator_select = unit0046Menu.indicator.selected;
    unit0046Menu.stop = true;
    yield return (object) unit0046Menu.StartCoroutine("OneProcessDestroy");
    unit0046Menu.coroutine = true;
    unit0046Menu.stop = false;
    for (int i = 0; i < unit0046Menu.buttons.Length; ++i)
    {
      if (unit0046Menu.ui3DModels.Count > i)
      {
        if (unit0046Menu.stop)
        {
          unit0046Menu.coroutine = false;
          yield break;
        }
        else
        {
          UI3DModel ui3DModel = unit0046Menu.ui3DModels[i];
          GameObject gameObject = (GameObject) null;
          ui3DModel.widget.depth = 18 - i;
          if (unit0046Menu.playerDecks[unit0046Menu.indicator.selected].player_units[i] != (PlayerUnit) null)
          {
            if (Object.op_Inequality((Object) ui3DModel.model_creater_, (Object) null))
              ui3DModel.model_creater_.BaseModel = (GameObject) null;
            if (unit0046Menu.ModelLoader[i] != null)
              unit0046Menu.StopCoroutine(unit0046Menu.ModelLoader[i]);
            unit0046Menu.ModelLoader[i] = unit0046Menu.StartCoroutine(ui3DModel.UnitEdit(unit0046Menu.playerDecks[unit0046Menu.indicator.selected].player_units[i]));
            while (Object.op_Equality((Object) ui3DModel.model_creater_.BaseModel, (Object) null))
            {
              if (unit0046Menu.stop)
              {
                unit0046Menu.StopCoroutine(unit0046Menu.ModelLoader[i]);
                unit0046Menu.ModelLoader[i] = (Coroutine) null;
                unit0046Menu.coroutine = false;
                yield break;
              }
              else
                yield return (object) null;
            }
            gameObject = ui3DModel.model_creater_.BaseModel;
            unit0046Menu.ui3DModelLoadDummy[i].SetActive(false);
            unit0046Menu.ModelLoader[i] = (Coroutine) null;
          }
          unit0046Menu.Models.Add(gameObject);
          ((Component) ui3DModel.ModelCamera).transform.localPosition = new Vector3((float) (unit0046Menu.indicator.selected * 1000), (float) (i * 1000), 0.0f);
          ui3DModel = (UI3DModel) null;
        }
      }
    }
    unit0046Menu.coroutine = false;
    unit0046Menu.InitOrSaveRenderTexture();
  }

  private IEnumerator OneProcessDestroy()
  {
    Unit0046Menu unit0046Menu = this;
    if (ModelUnits.Instance.ModelList != null)
    {
      for (int index = 0; index < ModelUnits.Instance.ModelList.Count; ++index)
      {
        if (Object.op_Implicit((Object) ModelUnits.Instance.ModelList[index]))
          unit0046Menu.StartCoroutine("StartDestroyPoolableObject", (object) ModelUnits.Instance.ModelList[index].gameObject);
      }
    }
    yield return (object) null;
  }

  private IEnumerator StartDestroyPoolableObject(GameObject go)
  {
    while (Object.op_Implicit((Object) go))
    {
      ObjectPoolController.Destroy(go);
      if (!go.activeSelf)
        break;
      yield return (object) null;
    }
  }

  public IEnumerator InitPlayerDecks(PlayerDeck[] _playerDecks)
  {
    Unit0046Menu unit0046Menu = this;
    if (unit0046Menu.modelChange != null)
    {
      unit0046Menu.modelChange.Stop();
      unit0046Menu.modelChange = (CoroutineData<bool>) null;
    }
    unit0046Menu.old_indicator_select = -1;
    unit0046Menu.SelectDeck = (PlayerDeck) null;
    unit0046Menu.playerDecks = (PlayerDeck[]) null;
    unit0046Menu.Clear3DModel();
    unit0046Menu.cost_max = SMManager.Get<Player>().max_cost;
    yield return (object) null;
    unit0046Menu.playerDecks = _playerDecks;
    Future<GameObject> prefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) unit0046Menu.prefabResult, (Object) null))
    {
      prefabF = (Future<GameObject>) null;
      prefabF = !unit0046Menu.isSea ? new ResourceObject("Prefabs/unit004_6/indicator").Load<GameObject>() : new ResourceObject("Prefabs/unit004_6_sea/indicator_sea").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit0046Menu.prefabResult = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) unit0046Menu.LeaderSkillPopupPrefab, (Object) null))
    {
      prefabF = PopupSkillDetails.createPrefabLoader(unit0046Menu.isSea);
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit0046Menu.LeaderSkillPopupPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    unit0046Menu.indicator.destroyParts(false);
    // ISSUE: reference to a compiler-generated method
    e = ((IEnumerable<PlayerDeck>) unit0046Menu.playerDecks).Select<PlayerDeck, IEnumerator>(new Func<PlayerDeck, IEnumerator>(unit0046Menu.\u003CInitPlayerDecks\u003Eb__65_1)).WaitAll();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0046Menu.indicator.SeEnable = false;
    yield return (object) new WaitForEndOfFrame();
    unit0046Menu.indicator.SeEnable = true;
    unit0046Menu.indicator.resetScrollView();
    int idx;
    switch (unit0046Menu.fromScene)
    {
      case Unit0046Scene.From.Versus:
        idx = unit0046Menu.lastDeckNumberOnEndScene.HasValue ? unit0046Menu.lastDeckNumberOnEndScene.Value : Persist.versusDeckOrganized.Data.number;
        break;
      case Unit0046Scene.From.Colosseum:
        idx = unit0046Menu.lastDeckNumberOnEndScene.HasValue ? unit0046Menu.lastDeckNumberOnEndScene.Value : Persist.colosseumDeckOrganized.Data.number;
        break;
      default:
        idx = unit0046Menu.isSea ? Persist.seaDeckOrganized.Data.number : Persist.deckOrganized.Data.number;
        break;
    }
    unit0046Menu.indicator.setItemPositionQuick(idx);
    unit0046Menu.SetTeamInfo(unit0046Menu.playerDecks[unit0046Menu.indicator.selected]);
    // ISSUE: reference to a compiler-generated method
    ((IEnumerable<UIButton>) unit0046Menu.buttons).ForEach<UIButton>(new Action<UIButton>(unit0046Menu.\u003CInitPlayerDecks\u003Eb__65_0));
    if (Object.op_Implicit((Object) unit0046Menu.buttonLockControl))
      unit0046Menu.buttonLockControl.startUnlockAndTutorial();
    unit0046Menu.StartCoroutine(unit0046Menu.WaitScrollSe());
  }

  private IEnumerator AddDeck(PlayerDeck playerDeck, GameObject prefab)
  {
    GameObject gameObject = this.indicator.instantiateParts(prefab);
    Player player = SMManager.Get<Player>();
    IEnumerator e = gameObject.GetComponent<Unit0046Indicator>().InitPlayerDeck(player, playerDeck, this.LeaderSkillPopupPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.InitOrSaveRenderTexture();
  }

  private IEnumerator UpdateDeck(int index)
  {
    PlayerDeck playerDeck = this.playerDecks[this.indicator.selected];
    GameObject gameObject = this.indicator.GridChild(index);
    Player player = SMManager.Get<Player>();
    IEnumerator e = gameObject.GetComponent<Unit0046Indicator>().InitPlayerDeck(player, playerDeck, this.LeaderSkillPopupPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.InitOrSaveRenderTexture();
  }

  public IEnumerator InitPlayerItems(PlayerItem[] items, GameObject itemIcon)
  {
    for (int i = 0; i < this.playerItems.Length; ++i)
    {
      this.playerItems[i].transform.Clear();
      ItemIcon n = itemIcon.CloneAndGetComponent<ItemIcon>(this.playerItems[i].transform);
      ((Component) n).transform.localScale = new Vector3()
      {
        x = 0.7352941f,
        y = 0.7352941f
      };
      if (i < items.Length)
      {
        IEnumerator e = n.InitByPlayerItem(items[i]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        int temp = i;
        n.onClick = (Action<ItemIcon>) (supplyicon => this.StartCoroutine(this.setDetailPopup(items[temp].supply.ID)));
      }
      else
      {
        n.SetModeSupply();
        n.SetEmpty(true);
      }
      n = (ItemIcon) null;
    }
  }

  public void charaActive(PlayerDeck pDeck)
  {
    int index = 0;
    QuestScoreBonusTimetable[] array = ((IEnumerable<QuestScoreBonusTimetable>) SMManager.Get<QuestScoreBonusTimetable[]>()).Where<QuestScoreBonusTimetable>((Func<QuestScoreBonusTimetable, bool>) (x => x.start_at < this.SevertTime && x.end_at > this.SevertTime)).ToArray<QuestScoreBonusTimetable>();
    UnitBonus[] activeUnitBonus = UnitBonus.getActiveUnitBonus(ServerTime.NowAppTime());
    foreach (PlayerUnit playerUnit in pDeck.player_units)
    {
      if (playerUnit == (PlayerUnit) null)
        ((Component) this.buttons[index]).GetComponent<Unit0046Character>().CharaSetActive(false, (string) null, false);
      else
        ((Component) this.buttons[index]).GetComponent<Unit0046Character>().CharaSetActive(true, playerUnit.SpecialEffectType((IEnumerable<QuestScoreBonusTimetable>) array, (IEnumerable<UnitBonus>) activeUnitBonus), playerUnit.unit.GetPiece);
      ++index;
    }
  }

  public void CharacterChange(int num)
  {
    if (this.IsPushAndSet())
      return;
    Unit0046Menu.OneFormationInfo info = new Unit0046Menu.OneFormationInfo();
    info.playerDeck = this.playerDecks[this.indicator.selected];
    info.num = num;
    ModelUnits.Instance.DestroyModelUnits();
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    Unit00468Scene.changeScene004682(true, info);
  }

  public void DetailSceneChange(int num)
  {
    if (this.playerDecks[this.indicator.selected].player_units[num] == (PlayerUnit) null)
      this.CharacterChange(num);
    else
      Unit0042Scene.changeScene(true, this.playerDecks[this.indicator.selected].player_units[num], this.playerDecks[this.indicator.selected].player_units);
  }

  public void OnDisable() => this.Clear3DModel();

  private void Clear3DModel()
  {
    this.ui3DModels.ForEach((Action<UI3DModel>) (obj =>
    {
      if (!Object.op_Inequality((Object) obj, (Object) null))
        return;
      if (Object.op_Inequality((Object) obj.model_creater_, (Object) null))
        obj.DestroyModelCamera();
      if (!Object.op_Inequality((Object) ((Component) obj).gameObject, (Object) null) || !(((Object) ((Component) obj).gameObject).name == "slc_3DModel(Clone)"))
        return;
      Object.Destroy((Object) ((Component) obj).gameObject);
    }));
    ModelUnits.Instance.DestroyModelUnits();
    this.ui3DModels.Clear();
    this.Models.Clear();
  }

  public void onEndScene()
  {
    switch (this.fromScene)
    {
      case Unit0046Scene.From.Versus:
        if (this.ChangeBackScene)
          break;
        this.lastDeckNumberOnEndScene = new int?(Mathf.Clamp(this.indicator.selected, 0, this.playerDecks.Length - 1));
        break;
      case Unit0046Scene.From.Colosseum:
        if (this.ChangeBackScene)
          break;
        this.lastDeckNumberOnEndScene = new int?(Mathf.Clamp(this.indicator.selected, 0, this.playerDecks.Length - 1));
        break;
      default:
        if (this.isSea)
        {
          Persist.seaDeckOrganized.Data.number = Mathf.Clamp(this.indicator.selected, 0, this.playerDecks.Length - 1);
          Persist.seaDeckOrganized.Flush();
          break;
        }
        Persist.deckOrganized.Data.number = Mathf.Clamp(this.indicator.selected, 0, this.playerDecks.Length - 1);
        Persist.deckOrganized.Flush();
        break;
    }
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    switch (this.fromScene)
    {
      case Unit0046Scene.From.Versus:
        this.saveVersusDeck(Mathf.Clamp(this.indicator.selected, 0, this.playerDecks.Length - 1));
        this.ChangeBackScene = true;
        Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
        this.backScene();
        break;
      case Unit0046Scene.From.Colosseum:
        Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
        this.saveColosseumDeck(Mathf.Clamp(this.indicator.selected, 0, this.playerDecks.Length - 1));
        this.ChangeBackScene = true;
        Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
        Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
        Colosseum0234Scene.ChangeScene(this.info);
        break;
      default:
        if (this.isSea)
        {
          if (this.fromScene == Unit0046Scene.From.Mypage)
          {
            Sea030_questScene.ChangeScene(false, false);
            break;
          }
          this.backScene();
          break;
        }
        this.backScene();
        break;
    }
  }

  private void saveVersusDeck(int deckNumber)
  {
    if (this.checkEmptyDeck(deckNumber))
      return;
    Persist<Persist.VersusDeckOrganized> versusDeckOrganized = Persist.versusDeckOrganized;
    Persist.VersusDeckOrganized data = versusDeckOrganized.Data;
    if (data.number == deckNumber && !data.isCustom)
      return;
    data.number = deckNumber;
    data.isCustom = false;
    versusDeckOrganized.Flush();
  }

  private void saveColosseumDeck(int deckNumber)
  {
    if (this.checkEmptyDeck(deckNumber))
      return;
    Persist<Persist.ColosseumDeckOrganized> colosseumDeckOrganized = Persist.colosseumDeckOrganized;
    Persist.ColosseumDeckOrganized data = colosseumDeckOrganized.Data;
    if (data.number == deckNumber && !data.isCustom)
      return;
    data.number = deckNumber;
    data.isCustom = false;
    colosseumDeckOrganized.Flush();
  }

  private bool checkEmptyDeck(int deckNumber)
  {
    PlayerDeck playerDeck = SMManager.Get<PlayerDeck[]>()[deckNumber];
    return playerDeck.player_unit_ids.IsNullOrEmpty<int?>() || !((IEnumerable<int?>) playerDeck.player_unit_ids).Any<int?>((Func<int?, bool>) (i => i.IsValid()));
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnEquip()
  {
    if (this.IsPushAndSet())
      return;
    PlayerDeck playerDeck = this.playerDecks[this.indicator.selected];
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    Unit00468Scene.changeScene00468(true, playerDeck);
  }

  public virtual void IbtnItemedit()
  {
    if (this.IsPushAndSet())
      return;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    this.SupplyItems = ((IEnumerable<SupplyItem>) SupplyItem.Merge(((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllSupplies()).ToList<PlayerItem>().ToArray(), ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllBattleSupplies()).ToArray<PlayerItem>())).ToList<SupplyItem>();
    this.SaveDeck = this.SupplyItems.Copy();
    Quest00210aScene.ChangeScene(true, new Quest00210Menu.Param()
    {
      SupplyItems = this.SupplyItems,
      SaveDeck = this.SaveDeck,
      removeButton = false,
      limitedOnly = false,
      mode = Quest00210Scene.Mode.Quest
    });
  }

  private IEnumerator setDetailPopup(int itemid)
  {
    IEnumerator e;
    if (Object.op_Equality((Object) this.detailPopup, (Object) null))
    {
      Future<GameObject> prefabF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.detailPopup = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    GameObject popup = Singleton<PopupManager>.GetInstance().open(this.detailPopup);
    popup.SetActive(false);
    e = popup.GetComponent<Shop00742Menu>().Init(MasterDataTable.CommonRewardType.supply, itemid);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
  }

  public void IbtnAutoOrganization()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.coOpenPopupAutoOrganization());
  }

  public void IbtnRentalEdit() => Unit004_RentalUnit_Edit_Scene.ChangeScene();

  private IEnumerator coOpenPopupAutoOrganization()
  {
    Unit0046Menu unit0046Menu = this;
    unit0046Menu.isAutoOrganization = true;
    bool bok = false;
    CommonElement element = CommonElement.none;
    IEnumerator waitPopup = Unit0046ConfirmAutoOrganization.doPopup(unit0046Menu.limitationData != null ? unit0046Menu.limitationData.description_ : (string) null, (Action<CommonElement>) (_element =>
    {
      bok = true;
      element = _element;
    }));
    while (waitPopup.MoveNext())
      yield return waitPopup.Current;
    if (!bok)
    {
      unit0046Menu.isAutoOrganization = false;
    }
    else
    {
      List<DeckOrganization.Filter> filters = new List<DeckOrganization.Filter>();
      if ((unit0046Menu.limitationData == null || unit0046Menu.limitationData.limitations_ == null ? 0 : (unit0046Menu.limitationData.limitations_.Length != 0 ? 1 : 0)) != 0)
      {
        int filterno = 0;
        foreach (QuestLimitationBase limitation in unit0046Menu.limitationData.limitations_)
        {
          DeckOrganization.Filter filter = limitation.createFilter(filterno);
          if (filter != null)
          {
            filters.Add(filter);
            ++filterno;
          }
        }
      }
      PlayerUnit[] array1 = SMManager.Get<PlayerUnit[]>();
      if (unit0046Menu.isSea)
        array1 = ((IEnumerable<PlayerUnit>) array1).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsSea)).ToArray<PlayerUnit>();
      PlayerUnit[] array2 = ((IEnumerable<PlayerUnit>) array1).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.cost <= 75)).ToArray<PlayerUnit>();
      if (element != CommonElement.none)
        array2 = ((IEnumerable<PlayerUnit>) array2).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.GetElement() == element)).ToArray<PlayerUnit>();
      Creator deckCreator = new Creator((PlayerUnit[]) null, array2, filters, unit0046Menu.SELECT_MIN, unit0046Menu.SELECT_MAX, unit0046Menu.cost_max);
      IEnumerator e = deckCreator.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (deckCreator.isSuccess)
      {
        if (!unit0046Menu.equalDeck(unit0046Menu.SelectDeck, deckCreator.result_))
          unit0046Menu.StartCoroutine(unit0046Menu.coUpdateDeck(deckCreator.result_));
        else
          unit0046Menu.isAutoOrganization = false;
      }
      else
      {
        bool bwait = true;
        ModalWindow.Show(Consts.GetInstance().UNIT_0046_AUTODECK_ERROR_TITLE, Consts.GetInstance().UNIT_0046_AUTODECK_ERROR_MESSAGE, (Action) (() => bwait = false));
        while (bwait)
          yield return (object) null;
        unit0046Menu.isAutoOrganization = false;
      }
    }
  }

  private bool equalDeck(PlayerDeck a, List<PlayerUnit> b)
  {
    int[] array1 = ((IEnumerable<int?>) a.player_unit_ids).Where<int?>((Func<int?, bool>) (d => d.HasValue)).Select<int?, int>((Func<int?, int>) (d => d.Value)).OrderBy<int, int>((Func<int, int>) (i => i)).ToArray<int>();
    int[] array2 = b.Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => u != (PlayerUnit) null)).Select<PlayerUnit, int>((Func<PlayerUnit, int>) (u => u.id)).OrderBy<int, int>((Func<int, int>) (i => i)).ToArray<int>();
    if (array1.Length != array2.Length)
      return false;
    for (int index = 0; index < array1.Length; ++index)
    {
      if (array1[index] != array2[index])
        return false;
    }
    return true;
  }

  private IEnumerator coUpdateDeck(List<PlayerUnit> newdeck)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    bool isFailed = false;
    IEnumerator e1;
    if (this.isSea)
    {
      e1 = WebAPI.SeaDeckEdit(this.SelectDeck.deck_number, newdeck.Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => u != (PlayerUnit) null)).Select<PlayerUnit, int>((Func<PlayerUnit, int>) (u => u.id)).ToArray<int>(), (Action<WebAPI.Response.UserError>) (e =>
      {
        isFailed = true;
        if (string.Equals(e.Code, "SEA000"))
          this.StartCoroutine(PopupUtility.SeaError(e));
        else
          WebAPI.DefaultUserErrorCallback(e);
      })).Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
    else
    {
      e1 = WebAPI.DeckEdit(this.SelectDeck.deck_type_id, this.SelectDeck.deck_number, newdeck.Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => u != (PlayerUnit) null)).Select<PlayerUnit, int>((Func<PlayerUnit, int>) (u => u.id)).ToArray<int>(), (Action<WebAPI.Response.UserError>) (e =>
      {
        isFailed = true;
        WebAPI.DefaultUserErrorCallback(e);
      })).Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (!isFailed)
    {
      Singleton<NGSceneManager>.GetInstance().RebootScene();
      this.isAutoOrganization = false;
    }
  }

  public void IbtnEditCustomDeck()
  {
    if (this.IsPushAndSet())
      return;
    if (this.fromScene == Unit0046Scene.From.Colosseum)
      EditCustomDeckScene.changeScene(this.info);
    else
      EditCustomDeckScene.changeScene(false);
  }

  public new bool IsPushAndSet()
  {
    if (this.isAutoOrganization || this.IsPush)
      return true;
    this.IsPush = true;
    return false;
  }

  public class OneFormationInfo
  {
    public int num;

    public PlayerDeck playerDeck { get; set; }
  }
}
