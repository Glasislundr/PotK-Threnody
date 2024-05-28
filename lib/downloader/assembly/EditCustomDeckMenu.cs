// Decompiled with JetBrains decompiler
// Type: EditCustomDeckMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CustomDeck/Menu")]
public class EditCustomDeckMenu : BackButtonMenuBase
{
  [SerializeField]
  [Tooltip("デッキ名")]
  private UILabel txtName_;
  [SerializeField]
  [Tooltip("デッキ名最大文字数")]
  private int maxLengthName_ = 10;
  [SerializeField]
  [Tooltip("総合戦闘力")]
  private UILabel txtCombat_;
  [SerializeField]
  [Tooltip("総コスト")]
  private UILabel txtCost_;
  [SerializeField]
  private NGHorizontalScrollParts scroll_;
  private UIScrollView scrollView_;
  private UIGrid grid_;
  [SerializeField]
  [Tooltip("共通ジョブ表記")]
  private string[] jobRankLabels_;
  [SerializeField]
  private UILabel txtLeaderSkillName_;
  [SerializeField]
  private UILabel txtLeaderSkillDescription_;
  [SerializeField]
  private UIButton btnLeaderSkillDetail_;
  [SerializeField]
  private GameObject objDecision_;
  private EditCustomDeckMenu.EditMode editMode_;
  private GameObject prefabPanel_;
  private GameObject prefabSkillDetail_;
  private GameObject prefabEditName_;
  private GameObject prefabSwapUnit_;
  private GameObject prefabEditUnit_;
  private GameObject prefabReisouDetail_;
  private GameObject prefabReisouDualDetail_;
  private UIButton btnDecision_;
  private Modified<PlayerUnit[]> modifiedUnits_;
  private Modified<PlayerItem[]> modifiedGears_;
  private Modified<PlayerAwakeSkill[]> modifiedAwakeSkills_;
  private const int DISPLAY_OBJECT_MAX = 4;
  private int? leaderSkillId_;
  private PopupSkillDetails.Param leaderSkillParam_;
  private bool isUpdated_;
  private int numEntity_ = 4;
  private int numDeck_ = 10;
  private Dictionary<GameObject, EditCustomDeckMenu.UnitPage> dicPage_;
  private Queue<EditCustomDeckMenu.UnitPage> blankPages_ = new Queue<EditCustomDeckMenu.UnitPage>();
  private UIDragScrollView[] dragScrollViews_;
  private EditCustomDeckMenu.UnitNode[] nodes_;
  private PlayerCustomDeck[] decks_;
  private EditCustomDeckPanel currentPanel_;
  private const int MAX_LOAD_PAGE = 3;
  private float? oldScrollViewLocalX_;

  private UIScrollView scrollView
  {
    get
    {
      return !Object.op_Implicit((Object) this.scrollView_) ? (this.scrollView_ = this.scroll_.uiScrollView) : this.scrollView_;
    }
  }

  private UIGrid grid
  {
    get
    {
      return !Object.op_Implicit((Object) this.grid_) ? (this.grid_ = ((Component) this.scrollView).GetComponentInChildren<UIGrid>()) : this.grid_;
    }
  }

  public string[] jobRankLabels => this.jobRankLabels_;

  public string fromScene { get; set; }

  public EditCustomDeckScene.Sortie sortie { get; set; }

  public int[] usedIds { get; set; }

  public ColosseumUtility.Info colosseumInfo { get; set; }

  public GameObject prefabUnitIcon { get; private set; }

  public GameObject prefabItemIcon { get; private set; }

  public GameObject prefabSkillIcon { get; private set; }

  private void checkMakeData()
  {
    if (this.modifiedUnits_ == null)
    {
      this.modifiedUnits_ = SMManager.Observe<PlayerUnit[]>();
      this.modifiedGears_ = SMManager.Observe<PlayerItem[]>();
      this.modifiedAwakeSkills_ = SMManager.Observe<PlayerAwakeSkill[]>();
      this.modifiedUnits_.NotifyChanged();
    }
    if (!this.modifiedUnits_.Changed && !this.modifiedGears_.Changed && !this.modifiedAwakeSkills_.Changed)
      return;
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    Util.resetPlayerUnits(instance.restoreUnits);
    Util.resetPlayerGears(instance.restoreGears);
    this.modifiedUnits_.Commit();
    this.modifiedGears_.Commit();
    this.modifiedAwakeSkills_.Commit();
  }

  public PlayerUnit[] playerUnits
  {
    get
    {
      this.checkMakeData();
      return SMManager.Get<PlayerUnit[]>();
    }
  }

  public PlayerItem[] playerGears
  {
    get
    {
      this.checkMakeData();
      return SMManager.Get<PlayerItem[]>();
    }
  }

  public PlayerAwakeSkill[] awakeSkills => SMManager.Get<PlayerAwakeSkill[]>();

  public IEnumerator doLoadResources()
  {
    Future<GameObject> ld = new ResourceObject("Prefabs/quest002_8/dir_party_Custom").Load<GameObject>();
    IEnumerator e = ld.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabPanel_ = ld.Result;
    ld = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    e = ld.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabUnitIcon = ld.Result;
    ld = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    e = ld.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabItemIcon = ld.Result;
    ld = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
    e = ld.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabSkillIcon = ld.Result;
    ld = PopupSkillDetails.createPrefabLoader(false);
    e = ld.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabSkillDetail_ = ld.Result;
    ld = PopupEditDeckName.createPrefabLoader();
    e = ld.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabEditName_ = ld.Result;
    ld = new ResourceObject("Prefabs/quest002_8/popup_CustomChangeOrder").Load<GameObject>();
    e = ld.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabSwapUnit_ = ld.Result;
    ld = new ResourceObject("Prefabs/quest002_8/popup_CustomSettings").Load<GameObject>();
    e = ld.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabEditUnit_ = ld.Result;
    ld = new ResourceObject("Prefabs/UnitGUIs/PopupReisouSkillDetails").Load<GameObject>();
    e = ld.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabReisouDetail_ = ld.Result;
    ld = new ResourceObject("Prefabs/UnitGUIs/PopupReisouSkillDetails_DualSkill").Load<GameObject>();
    e = ld.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabReisouDualDetail_ = ld.Result;
  }

  public IEnumerator onStartSceneAsync(bool bBackScene, int deckNumber)
  {
    EditCustomDeckMenu m = this;
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    EditCustomDeckMenu.\u003C\u003Ec__DisplayClass68_0 cDisplayClass680 = new EditCustomDeckMenu.\u003C\u003Ec__DisplayClass68_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass680.\u003C\u003E4__this = this;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass680.deckNumber = deckNumber;
    m.isInitializing = true;
    if (!bBackScene)
    {
      m.editMode_ = EditCustomDeckMenu.EditMode.None;
      NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
      m.decks_ = instance.customDecks;
      m.numDeck_ = m.decks_.Length;
      m.numEntity_ = Mathf.Min(m.numDeck_, 4);
      m.scroll_.SeEnable = false;
      if (m.dicPage_ != null)
      {
        foreach (KeyValuePair<GameObject, EditCustomDeckMenu.UnitPage> keyValuePair in m.dicPage_)
        {
          if (Object.op_Inequality((Object) keyValuePair.Key, (Object) null))
            Object.Destroy((Object) keyValuePair.Key);
        }
        m.dicPage_.Clear();
      }
      else
        m.dicPage_ = new Dictionary<GameObject, EditCustomDeckMenu.UnitPage>(m.numEntity_);
      bool flag;
      switch (m.sortie)
      {
        case EditCustomDeckScene.Sortie.Normal:
          // ISSUE: reference to a compiler-generated field
          if (cDisplayClass680.deckNumber < 0)
          {
            // ISSUE: reference to a compiler-generated field
            cDisplayClass680.deckNumber = Persist.deckOrganized.Data.customNumber;
          }
          flag = false;
          break;
        case EditCustomDeckScene.Sortie.Versus:
          // ISSUE: reference to a compiler-generated field
          if (cDisplayClass680.deckNumber < 0)
          {
            // ISSUE: reference to a compiler-generated field
            cDisplayClass680.deckNumber = Persist.versusDeckOrganized.Data.customNumber;
          }
          flag = true;
          break;
        case EditCustomDeckScene.Sortie.Colosseum:
          // ISSUE: reference to a compiler-generated field
          if (cDisplayClass680.deckNumber < 0)
          {
            // ISSUE: reference to a compiler-generated field
            cDisplayClass680.deckNumber = Persist.colosseumDeckOrganized.Data.customNumber;
          }
          flag = true;
          break;
        case EditCustomDeckScene.Sortie.GuildRaid:
          // ISSUE: reference to a compiler-generated field
          if (cDisplayClass680.deckNumber < 0)
          {
            // ISSUE: reference to a compiler-generated field
            cDisplayClass680.deckNumber = Persist.guildRaidLastSortie.Data.customDeckNumber;
          }
          flag = false;
          break;
        default:
          // ISSUE: reference to a compiler-generated field
          if (cDisplayClass680.deckNumber < 0)
          {
            // ISSUE: reference to a compiler-generated field
            cDisplayClass680.deckNumber = 0;
          }
          flag = false;
          break;
      }
      if (Object.op_Inequality((Object) m.objDecision_, (Object) null))
      {
        m.objDecision_.SetActive(flag);
        m.btnDecision_ = flag ? m.objDecision_.GetComponent<UIButton>() : (UIButton) null;
      }
      m.scroll_.destroyParts(false);
      EditCustomDeckMenu.UnitNode node = m.createNode();
      m.nodes_ = new EditCustomDeckMenu.UnitNode[m.numDeck_];
      for (int index = 0; index < m.nodes_.Length; ++index)
        m.nodes_[index] = m.createNode(node);
      Object.Destroy((Object) node.center);
      // ISSUE: method pointer
      m.grid.onReposition = new UIGrid.OnReposition((object) cDisplayClass680, __methodptr(\u003ConStartSceneAsync\u003Eb__0));
      m.grid.repositionNow = true;
      yield return (object) null;
      m.blankPages_.Clear();
      for (int index = 0; index < m.numEntity_; ++index)
      {
        GameObject key = m.prefabPanel_.Clone(m.nodes_[index].center.transform);
        EditCustomDeckPanel component = key.GetComponent<EditCustomDeckPanel>();
        component.setLinks(m);
        component.setBlanks();
        key.SetActive(false);
        EditCustomDeckMenu.UnitPage unitPage = new EditCustomDeckMenu.UnitPage(component);
        m.blankPages_.Enqueue(unitPage);
        m.dicPage_.Add(key, unitPage);
      }
      // ISSUE: reference to a compiler-generated method
      m.currentIndex = ((IEnumerable<PlayerCustomDeck>) m.decks_).FirstIndexOrNull<PlayerCustomDeck>(new Func<PlayerCustomDeck, bool>(cDisplayClass680.\u003ConStartSceneAsync\u003Eb__1)) ?? 0;
      yield return (object) m.doCreatePage(m.currentIndex);
      ((Component) m.scrollView).transform.localPosition = new Vector3(-m.grid.cellWidth * (float) m.currentIndex, 0.0f, 0.0f);
      m.oldScrollViewLocalX_ = new float?();
      m.editParam = (PlayerCustomDeckUnit_parameter_list) null;
    }
    else if (m.isUpdated_)
    {
      m.isInitializing = true;
      IEnumerator e;
      switch (m.editMode_)
      {
        case EditCustomDeckMenu.EditMode.UnitSingle:
        case EditCustomDeckMenu.EditMode.UnitMulti:
        case EditCustomDeckMenu.EditMode.UnitSwap:
          e = m.doSaveCustomDeck();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
      }
      // ISSUE: reference to a compiler-generated method
      e = m.dicPage_.Values.FirstOrDefault<EditCustomDeckMenu.UnitPage>(new Func<EditCustomDeckMenu.UnitPage, bool>(cDisplayClass680.\u003ConStartSceneAsync\u003Eb__2)).panel.doInitialize(m.currentDeck);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    m.updateDeckInfo();
    m.editMode_ = EditCustomDeckMenu.EditMode.None;
    m.isInitializing = false;
    m.isUpdated_ = false;
    if (m.editParam != null)
    {
      m.IsPush = true;
      m.showPopupEditor();
    }
  }

  private PlayerUnit leaderUnit
  {
    get
    {
      int id = ((IEnumerable<int>) this.currentDeck.player_unit_ids).First<int>();
      return id == 0 ? (PlayerUnit) null : Array.Find<PlayerUnit>(this.playerUnits, (Predicate<PlayerUnit>) (x => x.id == id));
    }
  }

  private void updateDeckInfo()
  {
    this.currentPanel_ = (EditCustomDeckPanel) null;
    this.currentDeck = this.decks_[this.currentIndex];
    this.txtName_.SetTextLocalize(this.currentDeck.name);
    this.setCurrentDeckInfo();
  }

  private void setCurrentDeckInfo()
  {
    PlayerUnit[] playerUnits = this.currentDeck.createPlayerUnits(this.playerUnits, this.playerGears, this.awakeSkills);
    this.txtCombat_.SetTextLocalize(((IEnumerable<PlayerUnit>) playerUnits).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => x.combat)));
    int num = ((IEnumerable<PlayerUnit>) playerUnits).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => x.cost));
    int maxCost = Player.Current.max_cost;
    bool flag = num <= maxCost;
    if (flag)
      this.txtCost_.SetTextLocalize(string.Format("{0}/{1}", (object) num, (object) maxCost));
    else
      this.txtCost_.SetTextLocalize(string.Format("[ff0000]{0}[-]/{1}", (object) num, (object) maxCost));
    PlayerUnitLeader_skills leaderSkill = this.leaderUnit?.leader_skill;
    int skillId = leaderSkill != null ? leaderSkill.skill_id : 0;
    if (!this.leaderSkillId_.HasValue || this.leaderSkillId_.Value != skillId)
    {
      this.leaderSkillId_ = new int?(skillId);
      if (leaderSkill != null)
      {
        this.leaderSkillParam_ = new PopupSkillDetails.Param(leaderSkill);
        BattleskillSkill skill = leaderSkill.skill;
        this.txtLeaderSkillName_.SetTextLocalize(skill.name);
        this.txtLeaderSkillDescription_.SetTextLocalize(skill.description);
        ((UIButtonColor) this.btnLeaderSkillDetail_).isEnabled = true;
      }
      else
      {
        this.leaderSkillParam_ = (PopupSkillDetails.Param) null;
        this.txtLeaderSkillName_.SetTextLocalize("");
        this.txtLeaderSkillDescription_.SetTextLocalize("");
        ((UIButtonColor) this.btnLeaderSkillDetail_).isEnabled = false;
      }
    }
    if (!Object.op_Implicit((Object) this.btnDecision_))
      return;
    ((UIButtonColor) this.btnDecision_).isEnabled = flag && ((IEnumerable<int>) this.currentDeck.player_unit_ids).Any<int>((Func<int, bool>) (i => i != 0));
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.doFinishEdit());
  }

  private IEnumerator doFinishEdit()
  {
    EditCustomDeckMenu editCustomDeckMenu = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    IEnumerator e = Singleton<NGGameDataManager>.GetInstance().doFinalizeEditCustomDeck();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    editCustomDeckMenu.backScene();
  }

  protected override void backScene()
  {
    switch (this.sortie)
    {
      case EditCustomDeckScene.Sortie.Normal:
        this.saveDeckNumber();
        break;
      case EditCustomDeckScene.Sortie.GuildRaid:
        this.saveGuildRaidDeckNumber();
        break;
    }
    switch (this.fromScene)
    {
      case "colosseum023_4":
        this.saveColosseumDeckNumber();
        Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
        Colosseum0234Scene.ChangeScene(this.colosseumInfo);
        return;
      case "versus026_2":
      case "versus026_10":
        this.saveVersusDeckNumber();
        Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
        break;
      case "unit004_6_0822":
        switch (this.sortie)
        {
          case EditCustomDeckScene.Sortie.Versus:
            Unit0046Scene.changeSceneVersus(false);
            return;
          case EditCustomDeckScene.Sortie.Colosseum:
            Unit0046Scene.changeScene(false, this.colosseumInfo);
            return;
          default:
            Unit0046Scene.changeScene(false);
            return;
        }
    }
    base.backScene();
  }

  private int validLastDeckNumber
  {
    get
    {
      int deckNumber = this.currentDeck.deck_number;
      if (!((IEnumerable<int>) this.currentDeck.player_unit_ids).Any<int>((Func<int, bool>) (i => i != 0)))
        deckNumber = this.decks_[((IEnumerable<PlayerCustomDeck>) this.decks_).FirstIndexOrNull<PlayerCustomDeck>((Func<PlayerCustomDeck, bool>) (x => ((IEnumerable<int>) x.player_unit_ids).Any<int>((Func<int, bool>) (i => i != 0)))) ?? 0].deck_number;
      return deckNumber;
    }
  }

  private void saveDeckNumber()
  {
    int validLastDeckNumber = this.validLastDeckNumber;
    Persist<Persist.DeckOrganized> deckOrganized = Persist.deckOrganized;
    if (deckOrganized.Data.customNumber == validLastDeckNumber)
      return;
    deckOrganized.Data.customNumber = validLastDeckNumber;
    deckOrganized.Flush();
  }

  private void saveGuildRaidDeckNumber()
  {
    int validLastDeckNumber = this.validLastDeckNumber;
    Persist<Persist.GuildRaidLastSortie> guildRaidLastSortie = Persist.guildRaidLastSortie;
    if (guildRaidLastSortie.Data.customDeckNumber == validLastDeckNumber)
      return;
    guildRaidLastSortie.Data.customDeckNumber = validLastDeckNumber;
    guildRaidLastSortie.Flush();
  }

  private void saveColosseumDeckNumber(bool bSelectedCustom = true)
  {
    if (this.checkEmpty(this.currentDeck))
      return;
    int deckNumber = this.currentDeck.deck_number;
    Persist<Persist.ColosseumDeckOrganized> colosseumDeckOrganized = Persist.colosseumDeckOrganized;
    Persist.ColosseumDeckOrganized data = colosseumDeckOrganized.Data;
    if (data.customNumber == deckNumber && data.isCustom == bSelectedCustom)
      return;
    data.customNumber = deckNumber;
    data.isCustom = bSelectedCustom;
    colosseumDeckOrganized.Flush();
  }

  private void saveVersusDeckNumber(bool bSelectedCustom = true)
  {
    if (this.checkEmpty(this.currentDeck))
      return;
    int deckNumber = this.currentDeck.deck_number;
    Persist<Persist.VersusDeckOrganized> versusDeckOrganized = Persist.versusDeckOrganized;
    Persist.VersusDeckOrganized data = versusDeckOrganized.Data;
    if (data.customNumber == deckNumber && data.isCustom == bSelectedCustom)
      return;
    data.customNumber = deckNumber;
    data.isCustom = bSelectedCustom;
    versusDeckOrganized.Flush();
  }

  private bool checkEmpty(PlayerCustomDeck target)
  {
    return !((IEnumerable<int>) target.player_unit_ids).Any<int>((Func<int, bool>) (i => i.IsValid()));
  }

  private void playSeOK() => Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1002");

  private void playSeNG() => Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1004");

  public void onClickedEditName()
  {
    if (this.IsPushAndSet())
      return;
    PopupEditDeckName.show(this.prefabEditName_, this.txtName_.text, this.maxLengthName_, new Action<string>(this.onSetName), (Action) (() => { }));
  }

  private void onSetName(string text) => this.StartCoroutine(this.doSaveDeckName(text));

  private IEnumerator doSaveDeckName(string nextName)
  {
    this.editMode_ = EditCustomDeckMenu.EditMode.DeckName;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    Future<WebAPI.Response.DeckEditCustomDeckName> wApi = WebAPI.DeckEditCustomDeckName(this.currentDeck.deck_type_id, nextName, this.currentDeck.deck_number);
    IEnumerator e = wApi.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.editMode_ = EditCustomDeckMenu.EditMode.None;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    if (wApi.Result != null)
    {
      this.currentDeck.name = nextName;
      this.txtName_.SetTextLocalize(nextName);
    }
  }

  public void onClickedSwapUnit()
  {
    GameObject[] objIcons = this.currentPanel.objIcons;
    if (objIcons == null || this.IsPushAndSet())
      return;
    PopupSwapCustomDeckUnit.show(this.prefabSwapUnit_, objIcons, ((IEnumerable<PlayerCustomDeckUnit_parameter_list>) this.currentDeck.unit_parameter_list).Select<PlayerCustomDeckUnit_parameter_list, bool>((Func<PlayerCustomDeckUnit_parameter_list, bool>) (x => x.checkBlank())).ToArray<bool>(), new Action<int, int>(this.onSwapUnit), (Action) (() => { }));
  }

  private void onSwapUnit(int a, int b)
  {
    if (a == b)
      return;
    this.currentDeck.swap(a, b);
    this.callbackSwapPosition(a, b);
    this.isUpdated_ = true;
    this.editMode_ = EditCustomDeckMenu.EditMode.UnitSwap;
    this.StartCoroutine(this.onStartSceneAsync(true, -1));
  }

  public void onClickedSelectUnits()
  {
    if (this.IsPushAndSet())
      return;
    Unit00468Scene.changeScene(true, new EditUnitParam(this.currentDeck, this.playerUnitsMergedCurrentDeck, this.usedIds, new EditUnitParam.Multi()
    {
      setUnits = new Action<int[]>(this.onSetUnits)
    }));
  }

  private void onSetUnits(int[] unitIds)
  {
    this.isUpdated_ = this.currentDeck.updateUnits(unitIds, this.playerUnits, this.playerGears, this.awakeSkills, new Action<int, int>(this.callbackSwapPosition));
    this.editMode_ = EditCustomDeckMenu.EditMode.UnitMulti;
  }

  private void callbackSwapPosition(int aIndex, int bIndex)
  {
    this.currentPanel.callbackSwapPosition(aIndex, bIndex);
  }

  public void onClickedSelectUnit(int index)
  {
    if (this.IsPushAndSet())
      return;
    this.playSeOK();
    Unit00468Scene.changeScene(true, new EditUnitParam(this.currentDeck, this.playerUnitsMergedCurrentDeck, this.usedIds, new EditUnitParam.Single()
    {
      index = index,
      setUnit = new Action<EditUnitParam.Single, int>(this.onSetUnit)
    }));
  }

  private PlayerUnit[] playerUnitsMergedCurrentDeck
  {
    get
    {
      PlayerUnit[] deckUnits = this.currentDeck.createPlayerUnits(this.playerUnits, this.playerGears, this.awakeSkills);
      deckUnits = ((IEnumerable<PlayerUnit>) deckUnits).Concat<PlayerUnit>((IEnumerable<PlayerUnit>) this.currentDeck.createOverkillers(this.playerUnits)).ToArray<PlayerUnit>();
      return deckUnits.Length != 0 ? ((IEnumerable<PlayerUnit>) this.playerUnits).Select<PlayerUnit, PlayerUnit>((Func<PlayerUnit, PlayerUnit>) (x =>
      {
        PlayerUnit playerUnit = Array.Find<PlayerUnit>(deckUnits, (Predicate<PlayerUnit>) (y => y.id == x.id));
        return (object) playerUnit != null ? playerUnit : x;
      })).ToArray<PlayerUnit>() : this.playerUnits;
    }
  }

  private void onSetUnit(EditUnitParam.Single s, int id)
  {
    this.isUpdated_ = this.currentDeck.updateUnit(s.index, id, this.playerUnits, this.playerGears, this.awakeSkills, new Action<int, int>(this.callbackSwapPosition));
    this.editMode_ = EditCustomDeckMenu.EditMode.UnitSingle;
  }

  public void onLongPressedUnit(int index)
  {
    int playerUnitId = this.currentDeck.unit_parameter_list[index].player_unit_id;
    if (playerUnitId != 0)
    {
      if (this.IsPushAndSet())
        return;
      this.playSeOK();
      PlayerUnit[] playerUnits = this.currentDeck.createPlayerUnits(this.playerUnits, this.playerGears, this.awakeSkills);
      Unit0042Scene.changeSceneCustomDeck(true, Array.Find<PlayerUnit>(playerUnits, (Predicate<PlayerUnit>) (x => x.id == playerUnitId)), playerUnits);
    }
    else
      this.playSeNG();
  }

  public void onClickedEdit(int no)
  {
    PlayerCustomDeckUnit_parameter_list unitParameter = this.currentDeck.unit_parameter_list[no];
    if (unitParameter.player_unit_id == 0)
    {
      this.playSeNG();
    }
    else
    {
      if (this.IsPushAndSet())
        return;
      this.showPopupEditor(unitParameter);
    }
  }

  private void showPopupEditor(PlayerCustomDeckUnit_parameter_list param = null)
  {
    if (param != null)
    {
      this.editParam = param;
      this.editParams = new PlayerCustomDeckUnit_parameter_list[this.currentDeck.unit_parameter_list.Length];
      this.editParams[this.editParam.index] = this.editParam;
    }
    PopupEditCustomDeck.show(this.prefabEditUnit_, this, ((IEnumerable<PlayerCustomDeckUnit_parameter_list>) this.currentDeck.unit_parameter_list).Where<PlayerCustomDeckUnit_parameter_list>((Func<PlayerCustomDeckUnit_parameter_list, bool>) (x => x.player_unit_id != 0)).ToArray<PlayerCustomDeckUnit_parameter_list>(), this.editParam.index, new Action(this.onCloseEdit), param == null);
  }

  public void changeEditTarget(PlayerCustomDeckUnit_parameter_list next)
  {
    if (this.editParam == next)
      return;
    this.editParam = next;
    this.editParams[this.editParam.index] = this.editParam;
  }

  private void onCloseEdit()
  {
    if (((IEnumerable<PlayerCustomDeckUnit_parameter_list>) this.editParams).Any<PlayerCustomDeckUnit_parameter_list>((Func<PlayerCustomDeckUnit_parameter_list, bool>) (x => x != null && x.isModified)))
    {
      this.editMode_ = EditCustomDeckMenu.EditMode.Popup;
      this.StartCoroutine(this.doSaveCustomDeck());
    }
    this.editParam = (PlayerCustomDeckUnit_parameter_list) null;
    this.editParams = (PlayerCustomDeckUnit_parameter_list[]) null;
  }

  private IEnumerator doSaveCustomDeck()
  {
    bool bShowLoading;
    switch (this.editMode_)
    {
      case EditCustomDeckMenu.EditMode.UnitSingle:
      case EditCustomDeckMenu.EditMode.UnitMulti:
        bShowLoading = false;
        break;
      default:
        bShowLoading = true;
        break;
    }
    if (bShowLoading)
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    PlayerCustomDeck playerCustomDeck = this.currentDeck.cloneBySave(this.playerUnits);
    SaveDeckUnitIds[] saveParams = playerCustomDeck.createSaveParams(5);
    Future<WebAPI.Response.DeckEditCustomDeck> wApi = WebAPI.DeckEditCustomDeck(playerCustomDeck.deck_type_id, playerCustomDeck.deck_number, playerCustomDeck.player_unit_ids, saveParams[0].skill, saveParams[1].skill, saveParams[2].skill, saveParams[3].skill, saveParams[4].skill, saveParams[0].gears, saveParams[1].gears, saveParams[2].gears, saveParams[3].gears, saveParams[4].gears, saveParams[0].job, saveParams[1].job, saveParams[2].job, saveParams[3].job, saveParams[4].job, saveParams[0].overkillers, saveParams[1].overkillers, saveParams[2].overkillers, saveParams[3].overkillers, saveParams[4].overkillers, saveParams[0].reisous, saveParams[1].reisous, saveParams[2].reisous, saveParams[3].reisous, saveParams[4].reisous);
    IEnumerator e = wApi.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.editMode_ = EditCustomDeckMenu.EditMode.None;
    if (bShowLoading)
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    if (wApi.Result != null)
      this.currentDeck.clearModified();
  }

  public void onClickedGear(PlayerCustomDeckUnit_parameter_list param, int slotNo)
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    PlayerItem[] playerGears = this.playerGears;
    PlayerUnit playerUnit = param.createPlayerUnit(this.playerUnits, playerGears, this.awakeSkills);
    PlayerItem[] tmpGears = new PlayerItem[3]
    {
      playerUnit.equippedGear,
      playerUnit.equippedGear2,
      playerUnit.equippedGear3
    };
    PlayerItem[] playerItemArray = new PlayerItem[3]
    {
      playerUnit.equippedReisou,
      playerUnit.equippedReisou2,
      playerUnit.equippedReisou3
    };
    for (int index = 0; index < tmpGears.Length; ++index)
    {
      if (tmpGears[index] != (PlayerItem) null && playerItemArray[index] != (PlayerItem) null)
        tmpGears[index].equipped_reisou_player_gear_id = playerItemArray[index].id;
    }
    Unit0044Scene.changeScene(true, new EditGearParam(this.currentDeck, playerUnit, this.currentDeck.createGearReference(this.playerUnits), param.index, slotNo, ((IEnumerable<PlayerItem>) playerGears).Where<PlayerItem>((Func<PlayerItem, bool>) (z => z.gear != null)).Select<PlayerItem, PlayerItem>((Func<PlayerItem, PlayerItem>) (x =>
    {
      PlayerItem playerItem = Array.Find<PlayerItem>(tmpGears, (Predicate<PlayerItem>) (y => y != (PlayerItem) null && y.id == x.id));
      return (object) playerItem != null ? playerItem : x;
    })).ToArray<PlayerItem>(), ((IEnumerable<int>) param.player_reisou_ids).ToArray<int>(), new Action<int, int, int>(this.onSetGear)));
  }

  private void onSetGear(int unitIndex, int slotNo, int gearId)
  {
    this.isUpdated_ |= this.currentDeck.setGear(this.playerGears, unitIndex, slotNo, gearId);
  }

  public void onLongPressedGear(PlayerCustomDeckUnit_parameter_list param, int slotNo)
  {
    PlayerItem[] playerGears = this.playerGears;
    PlayerItem gear = param.getGear(playerGears, slotNo);
    if (gear == (PlayerItem) null)
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    Unit00443Scene.changeSceneCustomDeck(gear, param.createPlayerUnit(this.playerUnits), param.getReisou(playerGears, slotNo));
  }

  public void onClickedReisou(PlayerCustomDeckUnit_parameter_list param, int slotNo)
  {
    if (param.getGearId(slotNo) == 0)
    {
      this.playSeNG();
    }
    else
    {
      Singleton<PopupManager>.GetInstance().dismiss();
      PlayerItem[] playerGears = this.playerGears;
      Unit0044ReisouScene.changeScene(true, new EditReisouParam(this.currentDeck, param.createGear(playerGears, slotNo, true), this.currentDeck.createReisouReference(this.playerUnits, this.playerGears), param.index, slotNo, playerGears, new Action<int, int, int>(this.onSetReisou)));
    }
  }

  private void onSetReisou(int unitIndex, int slotNo, int reisouId)
  {
    this.isUpdated_ |= this.currentDeck.setReisou(unitIndex, slotNo, reisouId);
  }

  public void onLongPressedReisou(PlayerCustomDeckUnit_parameter_list param, int slotNo)
  {
    PlayerItem[] playerGears = this.playerGears;
    PlayerItem reisou = param.getReisou(playerGears, slotNo);
    if (reisou == (PlayerItem) null)
      return;
    this.StartCoroutine(this.doPopupReisouDetail(reisou, param.getGear(playerGears, slotNo)));
  }

  private IEnumerator doPopupReisouDetail(PlayerItem reisou, PlayerItem gear)
  {
    GameObject go;
    IEnumerator e;
    if (reisou.gear.isMythologyReisou())
    {
      go = this.prefabReisouDualDetail_.Clone();
      go.SetActive(false);
      e = go.GetComponent<PopupReisouDetailsDualSkill>().Init(new GameCore.ItemInfo(reisou), (PlayerItem) null, true, (Action) null, false, false, false, gear);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      go.SetActive(true);
      Singleton<PopupManager>.GetInstance().open(go, isCloned: true);
      yield return (object) null;
      go.GetComponent<PopupReisouDetailsDualSkill>().scrollResetPosition();
      go = (GameObject) null;
    }
    else
    {
      go = this.prefabReisouDetail_.Clone();
      go.SetActive(false);
      e = go.GetComponent<PopupReisouDetails>().Init(new GameCore.ItemInfo(reisou), customGearBase: gear);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      go.SetActive(true);
      Singleton<PopupManager>.GetInstance().open(go, isCloned: true);
      yield return (object) null;
      go.GetComponent<PopupReisouDetails>().scrollResetPosition();
      go = (GameObject) null;
    }
  }

  public void onSetJob(int unitIndex, int jobId, bool[] bClearGears)
  {
    this.isUpdated_ |= this.currentDeck.setJob(unitIndex, jobId, bClearGears);
    if (!this.isUpdated_)
      return;
    this.setCurrentDeckInfo();
    this.StartCoroutine(this.doUpdatedJob(this.currentDeck.unit_parameter_list[unitIndex]));
  }

  private IEnumerator doUpdatedJob(PlayerCustomDeckUnit_parameter_list param)
  {
    IEnumerator e = this.currentPanel.doUpdatedJob(param);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onClickedOverkillers(PlayerCustomDeckUnit_parameter_list param, int slotNo)
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    Singleton<NGGameDataManager>.GetInstance().currentCustomDeck = this.currentDeck;
    PlayerUnit[] playerUnits = this.playerUnits;
    PlayerUnit playerUnit = param.createPlayerUnit(playerUnits, this.playerGears, this.awakeSkills);
    Unit004OverkillersSlotUnitSelectScene.changeScene(new EditOverkillersParam(this.currentDeck, playerUnit, this.currentDeck.createOverkillersReference(playerUnits), param.index, slotNo, playerUnit.getOverkillersUnit(playerUnits, slotNo), playerUnits, new Action<int, int, int>(this.onSetOverkillers)));
  }

  private void onSetOverkillers(int unitIndex, int slotNo, int unitId)
  {
    this.isUpdated_ |= this.currentDeck.setOverkillers(unitIndex, slotNo, unitId);
  }

  public void onLongPressedOverkillers(PlayerCustomDeckUnit_parameter_list param, int slotNo)
  {
    PlayerUnit[] playerUnits = this.playerUnits;
    int overkillersId = param.over_killers_ids[slotNo];
    PlayerUnit targetUnit = overkillersId != 0 ? Array.Find<PlayerUnit>(playerUnits, (Predicate<PlayerUnit>) (x => x.id == overkillersId)) : (PlayerUnit) null;
    if (targetUnit == (PlayerUnit) null)
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    Unit0042Scene.changeOverkillersUnitDetail(param.createPlayerUnit(playerUnits), targetUnit);
  }

  public void onClickedAwakeSkill(PlayerCustomDeckUnit_parameter_list param)
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    Unit004ExtraskillEquipListScene.changeScene(true, new EditAwakeSkillParam(this.currentDeck, param.createPlayerUnit(this.playerUnits, awakeSkills: this.awakeSkills), this.currentDeck.createAwakeSkillReference(this.playerUnits), param.index, this.awakeSkills, new Action<int, int>(this.onSetAwakeSkill)));
  }

  private void onSetAwakeSkill(int unitIndex, int skillId)
  {
    this.isUpdated_ |= this.currentDeck.setAwakeSkill(unitIndex, skillId);
  }

  public void onLongPressedAwakeSkill(PlayerCustomDeckUnit_parameter_list param)
  {
    PlayerAwakeSkill s = param.awake_skill_id != 0 ? Array.Find<PlayerAwakeSkill>(this.awakeSkills, (Predicate<PlayerAwakeSkill>) (x => x.id == param.awake_skill_id)) : (PlayerAwakeSkill) null;
    if (s == null)
      return;
    PopupSkillDetails.show(this.prefabSkillDetail_, new PopupSkillDetails.Param(s));
  }

  public void onClickedLeaderSkillDetail()
  {
    if (this.IsPushAndSet())
      return;
    PopupSkillDetails.show(this.prefabSkillDetail_, this.leaderSkillParam_);
  }

  public bool isInitializing { get; private set; } = true;

  public int currentIndex { get; private set; } = -1;

  public PlayerCustomDeck currentDeck { get; private set; }

  public PlayerCustomDeckUnit_parameter_list editParam { get; private set; }

  public PlayerCustomDeckUnit_parameter_list[] editParams { get; private set; }

  private EditCustomDeckPanel currentPanel
  {
    get
    {
      return !Object.op_Implicit((Object) this.currentPanel_) ? (this.currentPanel_ = this.dicPage_.Values.First<EditCustomDeckMenu.UnitPage>((Func<EditCustomDeckMenu.UnitPage, bool>) (x => x.index == this.currentIndex)).panel) : this.currentPanel_;
    }
  }

  private EditCustomDeckMenu.UnitNode createNode()
  {
    EditCustomDeckMenu.UnitNode node = new EditCustomDeckMenu.UnitNode()
    {
      center = new GameObject("node")
    };
    node.center.transform.localScale = Vector3.one;
    node.center.transform.localPosition = Vector3.zero;
    node.center.transform.localRotation = Quaternion.identity;
    GameObject self = new GameObject("target");
    self.SetParent(node.center);
    node.target = self.transform;
    return node;
  }

  private EditCustomDeckMenu.UnitNode createNode(EditCustomDeckMenu.UnitNode original)
  {
    EditCustomDeckMenu.UnitNode node = new EditCustomDeckMenu.UnitNode()
    {
      center = this.scroll_.instantiateParts(original.center, false)
    };
    node.target = node.center.transform.GetChild(0);
    return node;
  }

  private bool createPage(int unitIdx)
  {
    if (this.dicPage_.Any<KeyValuePair<GameObject, EditCustomDeckMenu.UnitPage>>((Func<KeyValuePair<GameObject, EditCustomDeckMenu.UnitPage>, bool>) (x => x.Value.index == unitIdx)))
      return true;
    if (!this.blankPages_.Any<EditCustomDeckMenu.UnitPage>())
      return false;
    this.StartCoroutine(this.doCreatePage(this.blankPages_.Dequeue(), unitIdx));
    return true;
  }

  private IEnumerator doCreatePage(int unitIdx)
  {
    if (!this.dicPage_.Any<KeyValuePair<GameObject, EditCustomDeckMenu.UnitPage>>((Func<KeyValuePair<GameObject, EditCustomDeckMenu.UnitPage>, bool>) (x => x.Value.index == unitIdx)) && this.blankPages_.Any<EditCustomDeckMenu.UnitPage>())
      yield return (object) this.doCreatePage(this.blankPages_.Dequeue(), unitIdx);
  }

  private IEnumerator doCreatePage(EditCustomDeckMenu.UnitPage page, int index)
  {
    page.isWaitLoad = true;
    page.index = index;
    EditCustomDeckPanel panel = page.panel;
    GameObject gameObject = ((Component) panel).gameObject;
    gameObject.SetActive(true);
    gameObject.SetParent(this.nodes_[index].center);
    IEnumerator e = panel.doInitialize(this.decks_[index]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    page.isWaitLoad = false;
  }

  private void updateBlank()
  {
    foreach (EditCustomDeckMenu.UnitPage unitPage in this.dicPage_.Values)
    {
      if (!unitPage.isWaitLoad && Mathf.Abs(unitPage.index - this.currentIndex) > 1 && !this.blankPages_.Contains(unitPage))
      {
        unitPage.panel.setBlanks();
        ((Component) unitPage.panel).gameObject.SetActive(false);
        unitPage.index = -1;
        this.blankPages_.Enqueue(unitPage);
      }
    }
  }

  protected override void Update()
  {
    base.Update();
    this.updateScroll();
  }

  private void updateScroll()
  {
    if (this.isInitializing)
      return;
    int center = this.currentIndex;
    bool flag = !this.oldScrollViewLocalX_.HasValue;
    float x = ((Component) this.scrollView).transform.localPosition.x;
    float dir = this.oldScrollViewLocalX_.HasValue ? this.oldScrollViewLocalX_.Value - x : 0.0f;
    if (flag || (double) dir != 0.0)
    {
      int cellWidth = (int) this.grid.cellWidth;
      int num = cellWidth / 2;
      int numDeck = this.numDeck_;
      center = Mathf.Clamp((int) Mathf.Abs((x - (float) num) / (float) cellWidth), 0, this.numDeck_ - 1);
      this.oldScrollViewLocalX_ = new float?(x);
      if (flag)
        this.scroll_.SeEnable = true;
    }
    if (!flag && this.currentIndex == center)
      return;
    List<int> loadReserves = this.createLoadReserves(center, dir);
    this.currentIndex = center;
    this.updateDeckInfo();
    this.updateBlank();
    for (int index = 0; index < loadReserves.Count; ++index)
    {
      if (!this.createPage(loadReserves[index]))
      {
        this.StartCoroutine(this.doWaitCreatePage(loadReserves.Skip<int>(index).ToList<int>()));
        break;
      }
    }
  }

  private IEnumerator doWaitCreatePage(List<int> request)
  {
    int num = this.scrollView.isDragging ? 1 : 0;
    this.setEnabledScrollViews(false);
    if (num != 0)
      this.scrollView.Press(false);
    yield return (object) null;
    while (this.dicPage_.Any<KeyValuePair<GameObject, EditCustomDeckMenu.UnitPage>>((Func<KeyValuePair<GameObject, EditCustomDeckMenu.UnitPage>, bool>) (kv => kv.Value.isWaitLoad)))
      yield return (object) null;
    this.updateBlank();
    foreach (int unitIdx in request)
      yield return (object) this.doCreatePage(unitIdx);
    this.setEnabledScrollViews(true);
  }

  private UIDragScrollView[] dragScrollViews
  {
    get
    {
      if (this.dragScrollViews_ != null)
        return this.dragScrollViews_;
      this.dragScrollViews_ = ((IEnumerable<UIDragScrollView>) ((Component) this).GetComponentsInChildren<UIDragScrollView>()).Where<UIDragScrollView>((Func<UIDragScrollView, bool>) (x => Object.op_Equality((Object) x.scrollView, (Object) this.scrollView))).ToArray<UIDragScrollView>();
      return this.dragScrollViews_;
    }
  }

  private void setEnabledScrollViews(bool bEnabled)
  {
    foreach (Behaviour dragScrollView in this.dragScrollViews)
      dragScrollView.enabled = bEnabled;
  }

  private List<int> createLoadReserves(int center, float dir = 0.0f)
  {
    int capacity = Mathf.Min(3, this.numDeck_);
    List<int> loadReserves = new List<int>(capacity)
    {
      center
    };
    if ((double) dir == 0.0)
    {
      int num1 = center + 1;
      if (num1 < this.numDeck_ && loadReserves.Count < capacity)
        loadReserves.Add(num1);
      int num2 = center - 1;
      if (num2 >= 0 && loadReserves.Count < capacity)
        loadReserves.Add(num2);
    }
    else if ((double) dir > 0.0)
    {
      int num3 = center + 1;
      if (num3 < this.numDeck_ && loadReserves.Count < capacity)
        loadReserves.Add(num3);
      int num4 = num3 + 1;
      if (num4 < this.numDeck_ && loadReserves.Count < capacity)
        loadReserves.Add(num4);
    }
    else
    {
      int num5 = center - 1;
      if (num5 >= 0 && loadReserves.Count < capacity)
        loadReserves.Add(num5);
      int num6 = num5 - 1;
      if (num6 >= 0 && loadReserves.Count < capacity)
        loadReserves.Add(num6);
    }
    return loadReserves;
  }

  private enum EditMode
  {
    None,
    DeckName,
    UnitSingle,
    UnitMulti,
    UnitSwap,
    Popup,
  }

  private class UnitPage
  {
    public int index;
    public bool isWaitLoad;

    public EditCustomDeckPanel panel { get; private set; }

    public UnitPage(EditCustomDeckPanel p)
    {
      this.panel = p;
      this.index = -1;
      this.isWaitLoad = false;
    }
  }

  private class UnitNode
  {
    public GameObject center;
    public Transform target;
  }
}
