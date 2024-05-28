// Decompiled with JetBrains decompiler
// Type: Bugu0059Menu
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
public class Bugu0059Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel m_NameLabel;
  [SerializeField]
  private UI2DSprite m_RaritySprite;
  [SerializeField]
  private GearKindIcon m_GearTypeIcon;
  [SerializeField]
  private UI2DSprite m_ThumSprite;
  [SerializeField]
  private UILabel m_RankLabel;
  [SerializeField]
  private UILabel m_NextRankToLabel;
  [SerializeField]
  private GameObject m_GaugeBeforeNextRankToObject;
  [SerializeField]
  private GameObject m_GaugeAfterNextRankToObject;
  [SerializeField]
  private List<UIWidget> m_SkillIconParentList;
  [SerializeField]
  private List<GameObject> m_SkillEvolutionObjectList;
  [SerializeField]
  private List<GameObject> m_SkillAcquireObjectList;
  public UIScrollBar slider;
  public GameObject tapLabelObj;
  public UIDragScrollView uiDrag;
  private List<BattleSkillIcon> m_SkillIconPrefabList;
  [SerializeField]
  private Bugu0059Menu.Status m_AtkStatus;
  [SerializeField]
  private Bugu0059Menu.Status m_MatkStatus;
  [SerializeField]
  private Bugu0059Menu.Status m_DefStatus;
  [SerializeField]
  private Bugu0059Menu.Status m_MdefStatus;
  [SerializeField]
  private Bugu0059Menu.Status m_HitStatus;
  [SerializeField]
  private Bugu0059Menu.Status m_CrtStatus;
  [SerializeField]
  private Bugu0059Menu.Status m_EvaStatus;
  [SerializeField]
  private List<Bugu0059Menu.MaterialIcon> m_MaterialIconList;
  [SerializeField]
  private SpreadColorButton m_DrillingBtn;
  [SerializeField]
  private UILabel m_ZenyLabel;
  [SerializeField]
  protected GameObject DirReisou;
  [SerializeField]
  protected GameObject DynReisouIcon;
  [SerializeField]
  protected Bugu0059Menu.ReisouExpGauge HolyReisouExpGauge;
  [SerializeField]
  protected Bugu0059Menu.ReisouExpGauge ChaosReisouExpGauge;
  [SerializeField]
  private UIWidget widgetScrollTop;
  protected PlayerItem reisouInfo;
  protected PlayerItem reisouInfoAfter;
  protected ItemIcon reisouIcon;
  private GameObject m_ItemIconPrefab;
  private GameObject m_SkillIconPrefab;
  private GameObject m_CheckMaterialPopupPrefab;
  private GameObject m_CheckMaterialPopupPrefabMini;
  private const int CheckMaterialPopupMiniIconNum = 5;
  private static GameObject reisouPopupDualSkillPrefab;
  private static GameObject reisouPopupPrefab;
  private GameCore.ItemInfo m_Base;
  private GameCore.ItemInfo m_After;
  private List<InventoryItem> m_Materials = new List<InventoryItem>();
  private Bugu0059Scene m_ParentScene;
  private WebAPI.Response.ItemGearBulkDrillingConfirm response;
  private ScrollViewSpecifyBounds scrollView_;
  private bool isReisouPopupOpen;
  private static readonly string COLOR_TAG_GREEN = "[00ff00]{0}[-]";
  private static readonly string COLOR_TAG_RED = "[ff0000]{0}[-]";
  private static readonly string COLOR_TAG_YELLOW = "[ffff00]{0}[-]";
  private static string[] spriteNameTBL = new string[7]
  {
    "Prefabs/ItemIcon/Materials/s_star1",
    "Prefabs/ItemIcon/Materials/s_star2",
    "Prefabs/ItemIcon/Materials/s_star3",
    "Prefabs/ItemIcon/Materials/s_star4",
    "Prefabs/ItemIcon/Materials/s_star5",
    "Prefabs/ItemIcon/Materials/s_star6",
    "Prefabs/ItemIcon/Materials/s_star7"
  };

  public IEnumerator onInitAsync()
  {
    Future<GameObject> ItemIconF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = ItemIconF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.m_ItemIconPrefab = ItemIconF.Result;
    Future<GameObject> SkillIconF = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
    e = SkillIconF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.m_SkillIconPrefab = SkillIconF.Result;
    this.m_SkillIconPrefabList = new List<BattleSkillIcon>();
    for (int index = 0; index < this.m_SkillIconParentList.Count; ++index)
      this.m_SkillIconPrefabList.Add((BattleSkillIcon) null);
    Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_005_9_1__anim_popup01.Load<GameObject>();
    e = popupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.m_CheckMaterialPopupPrefab = popupPrefabF.Result;
    Future<GameObject> popupPrefabMiniF = Res.Prefabs.popup.popup_005_9_2__anim_popup01.Load<GameObject>();
    e = popupPrefabMiniF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.m_CheckMaterialPopupPrefabMini = popupPrefabMiniF.Result;
    Singleton<NGGameDataManager>.GetInstance().lastReferenceItemID = -1;
  }

  public IEnumerator onStartAsync(
    GameCore.ItemInfo baseGear,
    List<InventoryItem> materials,
    WebAPI.Response.ItemGearBulkDrillingConfirm response,
    Bugu0059Scene scene,
    bool canDriling = true)
  {
    this.m_ParentScene = scene;
    this.m_Base = baseGear;
    Future<Sprite> basicSpriteF = baseGear.gear.LoadSpriteBasic();
    IEnumerator e = basicSpriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.m_ThumSprite.sprite2D = basicSpriteF.Result;
    this.m_NameLabel.SetTextLocalize(baseGear.Name());
    this.SetRarity(baseGear.gear, this.m_RaritySprite);
    this.tapLabelObj.SetActive(true);
    float num1 = baseGear.gearExp + baseGear.gearExpNext > 0 ? (float) baseGear.gearExp / (float) (baseGear.gearExp + baseGear.gearExpNext) : 0.0f;
    if ((double) num1 != 0.0)
    {
      this.m_GaugeBeforeNextRankToObject.SetActive(true);
      this.m_GaugeBeforeNextRankToObject.transform.localScale = new Vector3(num1, 1f, 1f);
    }
    else
      this.m_GaugeBeforeNextRankToObject.SetActive(false);
    this.response = response;
    this.reisouInfoAfter = (PlayerItem) null;
    this.reisouInfo = (PlayerItem) null;
    if (baseGear.reisou != (PlayerItem) null)
    {
      this.reisouInfo = baseGear.reisou;
      this.reisouInfo.GetPlayerMythologyGearStatus();
    }
    if (response?.player_reisou_item != (PlayerItem) null)
    {
      this.reisouInfoAfter = response.player_reisou_item;
      if (response?.player_mythology_gear_status != null)
        this.reisouInfoAfter.SetPlayerMythologyGearStatusCache(response.player_mythology_gear_status);
    }
    yield return (object) this.setDispParam();
    this.scrollView_ = this.uiDrag.scrollView as ScrollViewSpecifyBounds;
    if (materials != null)
    {
      this.m_Materials = materials;
      this.tapLabelObj.SetActive(!materials.Any<InventoryItem>());
      int limitRest = this.m_Base.playerItem.gear_level_limit_max - this.m_Base.playerItem.gear_level_limit;
      for (int i = 0; i < this.m_MaterialIconList.Count; ++i)
      {
        if (i < this.m_Materials.Count)
        {
          InventoryItem material = this.m_Materials[i];
          if (material != null)
          {
            if (Object.op_Equality((Object) this.m_MaterialIconList[i].m_ItemIcon, (Object) null))
              this.m_MaterialIconList[i].m_ItemIcon = this.m_ItemIconPrefab.CloneAndGetComponent<ItemIcon>(this.m_MaterialIconList[i].m_Parent.transform);
            e = this.m_MaterialIconList[i].m_ItemIcon.InitByItemInfo(material.Item);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            this.m_MaterialIconList[i].m_ItemIcon.HideCounter();
            if (!material.Item.isTempSelectedCount)
              this.m_MaterialIconList[i].m_ItemIcon.SetRenseiMaterialNum(1);
            else
              this.m_MaterialIconList[i].m_ItemIcon.SetRenseiMaterialNum(material.Item.tempSelectedCount);
            this.m_MaterialIconList[i].m_ItemIcon.SetRenseiIcon(false);
            if ((material.Item.isSupply || material.Item.isExchangable || material.Item.isCompse ? 1 : (material.Item.isWeaponMaterial ? 1 : 0)) != 0)
              this.m_MaterialIconList[i].m_ItemIcon.SetRenseiMaterialCount(material.Item.quantity);
            else
              this.m_MaterialIconList[i].m_ItemIcon.SetRenseiMaterialCount(0);
            ((Component) this.m_MaterialIconList[i].m_ItemIcon).gameObject.SetActive(true);
            if (limitRest > 0 && GearGear.CanSpecialDrill(this.m_Base.gear, material.Item.gear))
            {
              this.m_MaterialIconList[i].m_ItemIcon.SetRenseiMaxUpMark(true);
              --limitRest;
            }
            else
              this.m_MaterialIconList[i].m_ItemIcon.SetRenseiMaxUpMark(false);
            this.m_MaterialIconList[i].m_AddMaterialObject.SetActive(false);
          }
          material = (InventoryItem) null;
        }
        else
        {
          if (Object.op_Inequality((Object) this.m_MaterialIconList[i].m_ItemIcon, (Object) null))
            ((Component) this.m_MaterialIconList[i].m_ItemIcon).gameObject.SetActive(false);
          this.m_MaterialIconList[i].m_AddMaterialObject.SetActive(true);
        }
      }
    }
    else
    {
      this.m_Materials.Clear();
      for (int index = 0; index < this.m_MaterialIconList.Count; ++index)
      {
        if (Object.op_Inequality((Object) this.m_MaterialIconList[index].m_ItemIcon, (Object) null))
          ((Component) this.m_MaterialIconList[index].m_ItemIcon).gameObject.SetActive(false);
        this.m_MaterialIconList[index].m_AddMaterialObject.SetActive(true);
      }
    }
    ((UIButtonColor) this.m_DrillingBtn).isEnabled = this.m_Materials.Any<InventoryItem>() & canDriling;
    if (Object.op_Inequality((Object) this.scrollView_, (Object) null))
    {
      this.scrollView_.ClearBounds();
      int maxPerLine = ((Component) this.scrollView_).GetComponentInChildren<UIGrid>().maxPerLine;
      int count = this.m_Materials.Count;
      List<int> intList = new List<int>(3) { 0 };
      if (count >= maxPerLine)
      {
        intList.Add(maxPerLine - 1);
        int num2 = count - 1;
        if (num2 / maxPerLine > 0)
          intList.Add(num2);
      }
      UIWidget[] uiWidgetArray = new UIWidget[1]
      {
        this.widgetScrollTop
      };
      foreach (int index in intList)
      {
        Bugu0059Menu.MaterialIcon materialIcon = this.m_MaterialIconList[index];
        this.scrollView_.AddBounds((index == 0 ? (IEnumerable<UIWidget>) uiWidgetArray : (IEnumerable<UIWidget>) ((Component) materialIcon.m_ItemIcon).GetComponentsInChildren<UIWidget>()).Where<UIWidget>((Func<UIWidget, bool>) (x => ((Behaviour) x).enabled && ((Component) x).gameObject.activeInHierarchy)));
      }
      yield return (object) null;
      this.scrollView_.ResetPosition();
    }
  }

  private IEnumerator setDispParam()
  {
    IEnumerator e;
    if (this.response == null)
    {
      e = this.SetNoResponse(this.m_Base);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      this.m_After = new GameCore.ItemInfo(this.response.player_item);
      e = this.SetResponse(this.m_Base, this.response);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    yield return (object) this.SetReisouInfo();
  }

  private IEnumerator SetReisouInfo()
  {
    Bugu0059Menu bugu0059Menu = this;
    PlayerItem dispReisouInfo = bugu0059Menu.reisouInfo;
    if (bugu0059Menu.reisouInfoAfter != (PlayerItem) null)
      dispReisouInfo = bugu0059Menu.reisouInfoAfter;
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
    {
      bugu0059Menu.DirReisou.SetActive(false);
    }
    else
    {
      foreach (Component component in bugu0059Menu.DynReisouIcon.transform)
        Object.Destroy((Object) component.gameObject);
      if (Object.op_Inequality((Object) bugu0059Menu.reisouIcon, (Object) null))
        Object.Destroy((Object) bugu0059Menu.reisouIcon);
      if (dispReisouInfo == (PlayerItem) null)
      {
        bugu0059Menu.DirReisou.SetActive(false);
      }
      else
      {
        bugu0059Menu.DirReisou.SetActive(true);
        bugu0059Menu.reisouIcon = bugu0059Menu.m_ItemIconPrefab.CloneAndGetComponent<ItemIcon>(bugu0059Menu.DynReisouIcon.transform);
        IEnumerator e = bugu0059Menu.reisouIcon.InitByPlayerItem(bugu0059Menu.reisouInfo);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        bugu0059Menu.reisouIcon.setEquipReisouDisp();
        GameCore.ItemInfo dispInfoTemp = new GameCore.ItemInfo(dispReisouInfo);
        bugu0059Menu.reisouIcon.onClick = (Action<ItemIcon>) (x => this.OpenReisouDetailPopup(dispInfoTemp, dispReisouInfo, new Action(this.cbRemoveReisou)));
        bugu0059Menu.EnableLongPressEventReisou(dispInfoTemp, bugu0059Menu.reisouIcon, dispReisouInfo, new Action(bugu0059Menu.cbRemoveReisou));
        if (bugu0059Menu.reisouInfo.gear.isHolyReisou())
        {
          bugu0059Menu.setReisouGaugeExp(dispReisouInfo, bugu0059Menu.reisouInfo, bugu0059Menu.HolyReisouExpGauge);
          bugu0059Menu.ChaosReisouExpGauge.DirReisou.SetActive(false);
        }
        else if (bugu0059Menu.reisouInfo.gear.isChaosReisou())
        {
          bugu0059Menu.HolyReisouExpGauge.DirReisou.SetActive(false);
          bugu0059Menu.setReisouGaugeExp(dispReisouInfo, bugu0059Menu.reisouInfo, bugu0059Menu.ChaosReisouExpGauge);
        }
        else
        {
          GearReisouFusion fusionMineRecipe = bugu0059Menu.reisouInfo.GetReisouFusionMineRecipe();
          PlayerItem dispReisouItem1 = new PlayerItem(fusionMineRecipe.holy_ID, dispReisouInfo.GetPlayerMythologyGearStatus());
          PlayerItem baseReisouItem1 = new PlayerItem(fusionMineRecipe.holy_ID, bugu0059Menu.reisouInfo.GetPlayerMythologyGearStatus());
          bugu0059Menu.setReisouGaugeExp(dispReisouItem1, baseReisouItem1, bugu0059Menu.HolyReisouExpGauge);
          PlayerItem dispReisouItem2 = new PlayerItem(fusionMineRecipe.chaos_ID, dispReisouInfo.GetPlayerMythologyGearStatus());
          PlayerItem baseReisouItem2 = new PlayerItem(fusionMineRecipe.chaos_ID, bugu0059Menu.reisouInfo.GetPlayerMythologyGearStatus());
          bugu0059Menu.setReisouGaugeExp(dispReisouItem2, baseReisouItem2, bugu0059Menu.ChaosReisouExpGauge);
        }
      }
    }
  }

  public void EnableLongPressEventReisou(
    GameCore.ItemInfo item,
    ItemIcon icon,
    PlayerItem playerItem,
    Action removeCallback = null)
  {
    ((Component) icon.gear.button).gameObject.SetActive(true);
    EventDelegate.Set(icon.gear.button.onLongPress, (EventDelegate.Callback) (() => this.OpenReisouDetailPopup(item, playerItem, removeCallback)));
  }

  public void OpenReisouDetailPopup(GameCore.ItemInfo item, PlayerItem playerItem, Action removeCallback = null)
  {
    if (this.isReisouPopupOpen)
      return;
    this.isReisouPopupOpen = true;
    this.StartCoroutine(this.OpenReisouDetailPopupAsync(item, playerItem, removeCallback));
  }

  public IEnumerator OpenReisouDetailPopupAsync(
    GameCore.ItemInfo item,
    PlayerItem playerItem,
    Action removeCallback = null)
  {
    if (item == null)
    {
      this.isReisouPopupOpen = false;
    }
    else
    {
      GameObject popup;
      Future<GameObject> popupPrefabF;
      IEnumerator e;
      if (item.gear.isMythologyReisou())
      {
        if (this.response?.player_mythology_gear_status != null)
          playerItem.SetPlayerMythologyGearStatusCache(this.response.player_mythology_gear_status);
        if (Object.op_Equality((Object) Bugu0059Menu.reisouPopupDualSkillPrefab, (Object) null))
        {
          popupPrefabF = new ResourceObject("Prefabs/UnitGUIs/PopupReisouSkillDetails_DualSkill").Load<GameObject>();
          e = popupPrefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          Bugu0059Menu.reisouPopupDualSkillPrefab = popupPrefabF.Result;
          popupPrefabF = (Future<GameObject>) null;
        }
        popup = Bugu0059Menu.reisouPopupDualSkillPrefab.Clone();
        PopupReisouDetailsDualSkill script = popup.GetComponent<PopupReisouDetailsDualSkill>();
        popup.SetActive(false);
        e = script.Init(item, playerItem, true, removeCallback, false, false, false, (PlayerItem) null);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        popup.SetActive(true);
        Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
        yield return (object) null;
        script.scrollResetPosition();
        popup = (GameObject) null;
        script = (PopupReisouDetailsDualSkill) null;
      }
      else
      {
        if (Object.op_Equality((Object) Bugu0059Menu.reisouPopupPrefab, (Object) null))
        {
          popupPrefabF = new ResourceObject("Prefabs/UnitGUIs/PopupReisouSkillDetails").Load<GameObject>();
          e = popupPrefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          Bugu0059Menu.reisouPopupPrefab = popupPrefabF.Result;
          popupPrefabF = (Future<GameObject>) null;
        }
        popup = Bugu0059Menu.reisouPopupPrefab.Clone();
        PopupReisouDetails script = popup.GetComponent<PopupReisouDetails>();
        popup.SetActive(false);
        e = script.Init(item, playerItem, removeCallback: removeCallback);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        popup.SetActive(true);
        Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
        yield return (object) null;
        script.scrollResetPosition();
        popup = (GameObject) null;
        script = (PopupReisouDetails) null;
      }
      yield return (object) null;
      this.isReisouPopupOpen = false;
    }
  }

  private void setReisouGaugeExp(
    PlayerItem dispReisouItem,
    PlayerItem baseReisouItem,
    Bugu0059Menu.ReisouExpGauge reisouExpGauge)
  {
    reisouExpGauge.DirReisou.SetActive(true);
    string str = dispReisouItem.gear_level.ToString();
    if (dispReisouItem.gear_level > baseReisouItem.gear_level)
      str = Bugu0059Menu.COLOR_TAG_GREEN.F((object) str);
    reisouExpGauge.TxtReisouRank.SetTextLocalize(Consts.GetInstance().UNIT_00443_REISOU_RANK.F((object) str, (object) baseReisouItem.gear_level_limit));
    if (dispReisouItem.gear_level > baseReisouItem.gear_level)
      reisouExpGauge.TxtReisouNextRank.SetTextLocalize(Consts.Format(Consts.GetInstance().BUGU_0059_REMAIN, (IDictionary) new Hashtable()
      {
        {
          (object) "remain",
          (object) 0
        }
      }));
    else
      reisouExpGauge.TxtReisouNextRank.SetTextLocalize(Consts.Format(Consts.GetInstance().BUGU_0059_REMAIN, (IDictionary) new Hashtable()
      {
        {
          (object) "remain",
          (object) dispReisouItem.gear_exp_next
        }
      }));
    int width1 = ((UIWidget) reisouExpGauge.SlcReisouGauge).width;
    float num1 = (float) baseReisouItem.gear_exp / (float) (baseReisouItem.gear_exp + baseReisouItem.gear_exp_next);
    if ((double) num1 == 0.0 || baseReisouItem.gear_exp + baseReisouItem.gear_exp_next == 0)
    {
      ((Component) reisouExpGauge.SlcReisouGauge).gameObject.SetActive(false);
    }
    else
    {
      ((Component) reisouExpGauge.SlcReisouGauge).gameObject.SetActive(true);
      if ((double) num1 > 1.0)
        num1 = 1f;
      ((Component) reisouExpGauge.SlcReisouGauge).transform.localScale = new Vector3(num1, 1f, 1f);
    }
    if (dispReisouItem.gear_exp > baseReisouItem.gear_exp || dispReisouItem.gear_level > baseReisouItem.gear_level)
    {
      int width2 = ((UIWidget) reisouExpGauge.SlcReisouGaugeAfter).width;
      float num2 = (float) dispReisouItem.gear_exp / (float) (dispReisouItem.gear_exp + dispReisouItem.gear_exp_next);
      if (dispReisouItem.gear_level > baseReisouItem.gear_level)
        num2 = 1f;
      if ((double) num2 == 0.0)
      {
        ((Component) reisouExpGauge.SlcReisouGaugeAfter).gameObject.SetActive(false);
      }
      else
      {
        ((Component) reisouExpGauge.SlcReisouGaugeAfter).gameObject.SetActive(true);
        ((Component) reisouExpGauge.SlcReisouGaugeAfter).transform.localScale = new Vector3(num2, 1f, 1f);
      }
    }
    else
      ((Component) reisouExpGauge.SlcReisouGaugeAfter).gameObject.SetActive(false);
  }

  private IEnumerator SetNoResponse(GameCore.ItemInfo baseGear)
  {
    Consts instance = Consts.GetInstance();
    this.m_GearTypeIcon.Init(baseGear.gear.kind, baseGear.GetElement());
    this.m_RankLabel.SetTextLocalize(Consts.Format(instance.BUGU_0059_RANK, (IDictionary) new Hashtable()
    {
      {
        (object) "now",
        (object) baseGear.gearLevel
      },
      {
        (object) "max",
        (object) baseGear.gearLevelLimit
      }
    }));
    this.m_NextRankToLabel.SetTextLocalize(Consts.Format(instance.BUGU_0059_REMAIN, (IDictionary) new Hashtable()
    {
      {
        (object) "remain",
        (object) baseGear.gearExpNext
      }
    }));
    this.m_GaugeAfterNextRankToObject.SetActive(false);
    this.m_SkillEvolutionObjectList.ForEach((Action<GameObject>) (x => x.SetActive(false)));
    this.m_SkillAcquireObjectList.ForEach((Action<GameObject>) (x => x.SetActive(false)));
    Judgement.GearParameter lhs = Judgement.GearParameter.FromPlayerGear(baseGear);
    if (this.reisouInfo != (PlayerItem) null)
    {
      Judgement.GearParameter rhs = Judgement.GearParameter.FromPlayerGear(this.reisouInfo);
      lhs = Judgement.GearParameter.AddReisou(lhs, rhs);
    }
    this.SetStatus(this.m_AtkStatus, lhs.PhysicalPower);
    this.SetStatus(this.m_MatkStatus, lhs.MagicalPower);
    this.SetStatus(this.m_DefStatus, lhs.PhysicalDefense);
    this.SetStatus(this.m_MdefStatus, lhs.MagicDefense);
    this.SetStatus(this.m_HitStatus, lhs.Hit);
    this.SetStatus(this.m_CrtStatus, lhs.Critical);
    this.SetStatus(this.m_EvaStatus, lhs.Evasion);
    this.m_ZenyLabel.SetTextLocalize(0);
    for (int i = 0; i < this.m_SkillIconParentList.Count; ++i)
    {
      if (i < this.m_Base.skills.Length)
      {
        ((Component) this.m_SkillIconParentList[i]).gameObject.SetActive(true);
        if (Object.op_Equality((Object) this.m_SkillIconPrefabList[i], (Object) null))
        {
          BattleSkillIcon component = this.m_SkillIconPrefab.CloneAndGetComponent<BattleSkillIcon>(((Component) this.m_SkillIconParentList[i]).transform);
          this.m_SkillIconPrefabList[i] = component;
        }
        IEnumerator e = this.m_SkillIconPrefabList[i].Init(this.m_Base.skills[i].skill);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.m_SkillIconPrefabList[i].SetDepth(this.m_SkillIconParentList[i].depth);
      }
      else if (Object.op_Inequality((Object) this.m_SkillIconPrefabList[i], (Object) null))
        ((Component) this.m_SkillIconPrefabList[i]).gameObject.SetActive(false);
    }
  }

  private IEnumerator SetResponse(
    GameCore.ItemInfo baseGear,
    WebAPI.Response.ItemGearBulkDrillingConfirm response)
  {
    Consts instance = Consts.GetInstance();
    PlayerItem drillingGear = response.player_item;
    this.m_GearTypeIcon.Init(drillingGear.gear.kind, drillingGear.GetElement());
    string str1 = drillingGear.gear_level.ToString();
    if (drillingGear.gear_level > baseGear.gearLevel)
      str1 = Bugu0059Menu.COLOR_TAG_GREEN.F((object) str1);
    if (drillingGear.gear_level < baseGear.gearLevel)
      str1 = Bugu0059Menu.COLOR_TAG_RED.F((object) str1);
    string str2 = drillingGear.gear_level_limit.ToString();
    if (drillingGear.gear_level_limit > baseGear.gearLevelLimit)
      str2 = Bugu0059Menu.COLOR_TAG_GREEN.F((object) str2);
    if (drillingGear.gear_level_limit < baseGear.gearLevelLimit)
      str2 = Bugu0059Menu.COLOR_TAG_RED.F((object) str2);
    this.m_RankLabel.SetTextLocalize(Consts.Format(instance.BUGU_0059_RANK, (IDictionary) new Hashtable()
    {
      {
        (object) "now",
        (object) str1
      },
      {
        (object) "max",
        (object) str2
      }
    }));
    this.m_GaugeAfterNextRankToObject.SetActive(true);
    if (drillingGear.gear_level > baseGear.gearLevel)
    {
      this.m_NextRankToLabel.SetTextLocalize(Consts.Format(instance.BUGU_0059_REMAIN, (IDictionary) new Hashtable()
      {
        {
          (object) "remain",
          (object) 0
        }
      }));
      this.m_GaugeAfterNextRankToObject.transform.localScale = Vector3.one;
    }
    else
    {
      this.m_NextRankToLabel.SetTextLocalize(Consts.Format(instance.BUGU_0059_REMAIN, (IDictionary) new Hashtable()
      {
        {
          (object) "remain",
          (object) drillingGear.gear_exp_next
        }
      }));
      this.m_GaugeAfterNextRankToObject.transform.localScale = new Vector3((float) drillingGear.gear_exp / (float) (drillingGear.gear_exp + drillingGear.gear_exp_next), 1f, 1f);
    }
    Judgement.GearParameter lhs1 = Judgement.GearParameter.FromPlayerGear(drillingGear);
    Judgement.GearParameter lhs2 = Judgement.GearParameter.FromPlayerGear(baseGear);
    if (this.reisouInfoAfter != (PlayerItem) null)
    {
      Judgement.GearParameter rhs = Judgement.GearParameter.FromPlayerGear(this.reisouInfoAfter);
      lhs1 = Judgement.GearParameter.AddReisou(lhs1, rhs);
    }
    if (this.reisouInfo != (PlayerItem) null)
    {
      Judgement.GearParameter rhs = Judgement.GearParameter.FromPlayerGear(this.reisouInfo);
      lhs2 = Judgement.GearParameter.AddReisou(lhs2, rhs);
    }
    this.SetStatus(this.m_AtkStatus, lhs1.PhysicalPower, lhs1.PhysicalPower - lhs2.PhysicalPower);
    this.SetStatus(this.m_MatkStatus, lhs1.MagicalPower, lhs1.MagicalPower - lhs2.MagicalPower);
    this.SetStatus(this.m_DefStatus, lhs1.PhysicalDefense, lhs1.PhysicalDefense - lhs2.PhysicalDefense);
    this.SetStatus(this.m_MdefStatus, lhs1.MagicDefense, lhs1.MagicDefense - lhs2.MagicDefense);
    this.SetStatus(this.m_HitStatus, lhs1.Hit, lhs1.Hit - lhs2.Hit);
    this.SetStatus(this.m_CrtStatus, lhs1.Critical, lhs1.Critical - lhs2.Critical);
    this.SetStatus(this.m_EvaStatus, lhs1.Evasion, lhs1.Evasion - lhs2.Evasion);
    this.m_ZenyLabel.SetTextLocalize(response.consume_money);
    this.m_SkillEvolutionObjectList.ForEach((Action<GameObject>) (x => x.SetActive(false)));
    this.m_SkillAcquireObjectList.ForEach((Action<GameObject>) (x => x.SetActive(false)));
    for (int i = 0; i < this.m_SkillIconParentList.Count; ++i)
    {
      if (i < drillingGear.skills.Length)
      {
        ((Component) this.m_SkillIconParentList[i]).gameObject.SetActive(true);
        if (Object.op_Equality((Object) this.m_SkillIconPrefabList[i], (Object) null))
        {
          BattleSkillIcon component = this.m_SkillIconPrefab.CloneAndGetComponent<BattleSkillIcon>(((Component) this.m_SkillIconParentList[i]).transform);
          this.m_SkillIconPrefabList[i] = component;
        }
        ((Component) this.m_SkillIconPrefabList[i]).gameObject.SetActive(true);
        IEnumerator e = this.m_SkillIconPrefabList[i].Init(drillingGear.skills[i].skill);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.m_SkillIconPrefabList[i].SetDepth(this.m_SkillIconParentList[i].depth);
        if (this.m_Base.skills.Length <= i)
          this.m_SkillAcquireObjectList[i].SetActive(true);
        else if (drillingGear.skills[i].ID != this.m_Base.skills[i].ID && this.m_Base.skills[i].release_rank < drillingGear.skills[i].release_rank)
          this.m_SkillEvolutionObjectList[i].SetActive(true);
      }
      else if (Object.op_Inequality((Object) this.m_SkillIconPrefabList[i], (Object) null))
        ((Component) this.m_SkillIconPrefabList[i]).gameObject.SetActive(false);
    }
  }

  private void SetStatus(Bugu0059Menu.Status status, int baseParam, int upParam = 0)
  {
    if (upParam > 0)
      status.m_ParameterLabel.SetTextLocalize(Bugu0059Menu.COLOR_TAG_YELLOW.F((object) baseParam));
    else if (upParam < 0)
      status.m_ParameterLabel.SetTextLocalize(Bugu0059Menu.COLOR_TAG_RED.F((object) baseParam));
    else
      status.m_ParameterLabel.SetTextLocalize(baseParam);
    if (upParam <= 0)
    {
      status.m_ParameterUpObject.SetActive(false);
    }
    else
    {
      status.m_ParameterUpObject.SetActive(true);
      status.m_ParameterUpLabel.SetTextLocalize(upParam);
    }
  }

  public void cbRemoveReisou() => this.StartCoroutine(this.cbRemoveReisouAsync());

  protected IEnumerator cbRemoveReisouAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Bugu0059Menu bugu0059Menu = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      bugu0059Menu.backScene();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    bugu0059Menu.reisouInfoAfter = (PlayerItem) null;
    bugu0059Menu.reisouInfo = (PlayerItem) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) bugu0059Menu.setDispParam();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void ibtnChangeMaterial()
  {
    if (this.IsPushAndSet())
      return;
    if (this.m_Base.gearLevel == this.m_Base.gearLevelLimit)
      Bugu00527Scene.ChangeScene(false, this.m_Materials, this.m_Base, true);
    else
      Bugu00527Scene.ChangeScene(false, this.m_Materials, this.m_Base, false);
  }

  public void ibtnDrilling()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.OpenMaterialCheckPopup());
  }

  private IEnumerator OpenMaterialCheckPopup()
  {
    Bugu0059Menu bugu0059Menu = this;
    GameObject obj = bugu0059Menu.m_Materials.Count <= 5 ? bugu0059Menu.m_CheckMaterialPopupPrefabMini.Clone() : bugu0059Menu.m_CheckMaterialPopupPrefab.Clone();
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = obj.GetComponent<Popup00591Menu>().Init(bugu0059Menu.m_Base, bugu0059Menu.m_After, bugu0059Menu.reisouInfo, bugu0059Menu.reisouInfoAfter, bugu0059Menu.m_Materials, new Action(bugu0059Menu.\u003COpenMaterialCheckPopup\u003Eb__68_0), bugu0059Menu.m_ItemIconPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(obj, isCloned: true);
  }

  private IEnumerator drilling()
  {
    int reisouJewelBefore = SMManager.Get<Player>().reisou_jewel;
    int[] array1 = this.m_Materials.ToMaterialId().ToArray();
    int[] array2 = this.m_Materials.ToGearId().ToArray();
    int[] array3 = this.m_Materials.ToMaterialCounts().ToArray();
    Future<WebAPI.Response.ItemGearBulkDrilling> feature = WebAPI.ItemGearBulkDrilling(this.m_Base.itemID, array2, array1, array3);
    IEnumerator e = feature.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    WebAPI.Response.ItemGearBulkDrilling result = feature.Result;
    PlayerItem playerItem = result.player_item;
    List<GameCore.ItemInfo> list = this.m_Materials.Select<InventoryItem, GameCore.ItemInfo>((Func<InventoryItem, GameCore.ItemInfo>) (x => x.Item)).ToList<GameCore.ItemInfo>();
    list.Add(new GameCore.ItemInfo(playerItem));
    bool flag = playerItem.isLimitMax() && playerItem.isLevelMax();
    if (playerItem.isReisouSet)
    {
      PlayerItem equipReisou = playerItem.equipReisou;
      flag = ((flag ? 1 : 0) & (!equipReisou.isLimitMax() ? 0 : (equipReisou.isLevelMax() ? 1 : 0))) != 0;
    }
    int addReisouJewel = result.player.reisou_jewel - reisouJewelBefore;
    Bugu00539Scene.ChangeScene(true, new Bugu00539ChangeSceneParam(list, false, result.animation_pattern, this.m_Base, this.reisouInfo, flag ? (Action) (() =>
    {
      NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
      if (instance.backScene("bugu005_drilling_base"))
        return;
      instance.changeScene(Singleton<CommonRoot>.GetInstance().startScene, false);
    }) : (Action) null, addReisouJewel));
    this.m_ParentScene.SetResetFlag();
  }

  public void ibtnBack()
  {
    if (this.IsPushAndSet())
      return;
    if (this.m_Base != null)
    {
      Singleton<NGGameDataManager>.GetInstance().lastReferenceItemID = !Singleton<NGGameDataManager>.GetInstance().isReisouScene || !(this.m_Base.reisou != (PlayerItem) null) ? this.m_Base.itemID : this.m_Base.reisou.id;
      Singleton<NGGameDataManager>.GetInstance().isReisouScene = false;
    }
    this.backScene();
  }

  public override void onBackButton() => this.ibtnBack();

  public void callBackScene() => this.backScene();

  protected override void backScene()
  {
    if (Singleton<NGSceneManager>.GetInstance().backScene())
      return;
    Bugu00526Scene.ChangeScene(false);
  }

  private void SetRarity(GearGear gear, UI2DSprite dstSprite)
  {
    if (Object.op_Equality((Object) dstSprite, (Object) null))
      return;
    ((Component) dstSprite).gameObject.SetActive(false);
    if (gear.rarity.index <= 0)
      return;
    Sprite sprite = Resources.Load<Sprite>(Bugu0059Menu.spriteNameTBL[gear.rarity.index - 1]);
    if (!Object.op_Inequality((Object) sprite, (Object) null))
      return;
    dstSprite.sprite2D = sprite;
    UI2DSprite ui2Dsprite = dstSprite;
    Rect textureRect1 = sprite.textureRect;
    int width = (int) ((Rect) ref textureRect1).width;
    Rect textureRect2 = sprite.textureRect;
    int height = (int) ((Rect) ref textureRect2).height;
    ((UIWidget) ui2Dsprite).SetDimensions(width, height);
    ((Component) dstSprite).gameObject.SetActive(true);
  }

  [Serializable]
  public struct Status
  {
    [SerializeField]
    public UILabel m_ParameterLabel;
    [SerializeField]
    public GameObject m_ParameterUpObject;
    [SerializeField]
    public UILabel m_ParameterUpLabel;
  }

  [Serializable]
  public class MaterialIcon
  {
    [SerializeField]
    public GameObject m_Parent;
    [SerializeField]
    public GameObject m_AddMaterialObject;
    [HideInInspector]
    public ItemIcon m_ItemIcon;
  }

  [Serializable]
  public struct ReisouExpGauge
  {
    [SerializeField]
    public GameObject DirReisou;
    [SerializeField]
    public UILabel TxtReisouRank;
    [SerializeField]
    public UILabel TxtReisouNextRank;
    [SerializeField]
    public UISprite SlcReisouGauge;
    [SerializeField]
    public UISprite SlcReisouGaugeAfter;
  }
}
