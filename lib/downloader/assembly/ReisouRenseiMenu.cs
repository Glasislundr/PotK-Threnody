// Decompiled with JetBrains decompiler
// Type: ReisouRenseiMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ReisouRenseiMenu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel m_NameLabel;
  [SerializeField]
  private UI2DSprite m_RaritySprite;
  [SerializeField]
  private GearKindIcon m_GearTypeIcon;
  [SerializeField]
  private List<UIWidget> m_SkillIconParentList;
  public UIScrollBar slider;
  public GameObject tapLabelObj;
  public UIDragScrollView uiDrag;
  public UIGrid grid;
  private List<BattleSkillIcon> m_SkillIconPrefabList;
  [SerializeField]
  private List<ReisouRenseiMenu.MaterialIcon> m_MaterialIconList;
  [SerializeField]
  private SpreadColorButton m_DrillingBtn;
  [SerializeField]
  private UILabel m_ZenyLabel;
  [SerializeField]
  private UILabel reisouSkill1;
  [SerializeField]
  private UILabel reisouSkill2;
  [SerializeField]
  protected GameObject DirReisou;
  [SerializeField]
  protected GameObject DynReisouIcon;
  [SerializeField]
  protected GameObject DynReisouIconForBase;
  [SerializeField]
  protected ReisouRenseiMenu.ReisouExpGauge HolyReisouExpGauge;
  [SerializeField]
  protected ReisouRenseiMenu.ReisouExpGauge ChaosReisouExpGauge;
  protected PlayerItem reisouInfo;
  protected PlayerItem reisouInfoAfter;
  protected ItemIcon reisouIcon;
  protected ItemIcon reisouIconForBase;
  private GameObject m_ItemIconPrefab;
  private GameObject m_SkillIconPrefab;
  private GameObject m_CheckMaterialPopupPrefabMini;
  private const int CheckMaterialPopupMiniIconNum = 5;
  private static GameObject reisouPopupDualSkillPrefab;
  private static GameObject reisouPopupPrefab;
  private static GameObject reisouIconPrefab;
  private GameCore.ItemInfo m_After;
  private List<InventoryItem> m_Materials = new List<InventoryItem>();
  private ReisouRenseiScene m_ParentScene;
  private WebAPI.Response.ItemGearBulkDrillingConfirm response;
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
  private bool isReisouPopupOpen;
  protected const int holyParamSkillDispCntMax = 12;
  protected const int chaosParamSkillDispCntMax = 3;

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
    Future<GameObject> popupPrefabMiniF = new ResourceObject("Prefabs/popup/popup_005_9_1__anim_reisou_rensei").Load<GameObject>();
    e = popupPrefabMiniF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.m_CheckMaterialPopupPrefabMini = popupPrefabMiniF.Result;
    Future<GameObject> iconPrefab = new ResourceObject("Prefabs/UnitGUIs/dir_Bugu1").Load<GameObject>();
    e = iconPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ReisouRenseiMenu.reisouIconPrefab = iconPrefab.Result;
    for (int index = 0; index < 99; ++index)
    {
      ReisouRenseiIcon component = ReisouRenseiMenu.reisouIconPrefab.Clone(((Component) this.grid).transform).GetComponent<ReisouRenseiIcon>();
      this.m_MaterialIconList.Add(new ReisouRenseiMenu.MaterialIcon()
      {
        m_Parent = component.m_Parent,
        m_AddMaterialObject = component.m_AddMaterialObject
      });
    }
    this.grid.Reposition();
    Singleton<NGGameDataManager>.GetInstance().lastReferenceItemID = -1;
  }

  public IEnumerator onStartAsync(
    PlayerItem item,
    List<InventoryItem> materials,
    WebAPI.Response.ItemGearBulkDrillingConfirm response,
    ReisouRenseiScene scene,
    bool canDriling = true)
  {
    this.m_ParentScene = scene;
    this.tapLabelObj.SetActive(true);
    ((Behaviour) ((Component) ((UIProgressBar) this.slider).foregroundWidget).GetComponent<UISprite>()).enabled = false;
    ((Behaviour) this.uiDrag).enabled = false;
    this.response = response;
    this.reisouInfoAfter = (PlayerItem) null;
    this.reisouInfo = (PlayerItem) null;
    if (item != (PlayerItem) null)
    {
      this.reisouInfo = item;
      this.m_NameLabel.SetTextLocalize(item.name);
      this.SetRarity(item.gear, this.m_RaritySprite);
      this.reisouInfo.GetPlayerMythologyGearStatus();
    }
    if (response?.player_item != (PlayerItem) null)
    {
      this.reisouInfoAfter = response.player_item;
      if (response?.player_mythology_gear_status != null)
        this.reisouInfoAfter.SetPlayerMythologyGearStatusCache(response.player_mythology_gear_status);
    }
    yield return (object) this.setDispParam();
    this.m_Materials.Clear();
    if (materials != null)
    {
      this.m_Materials = materials;
      ((Behaviour) this.uiDrag).enabled = materials.Count > 0;
      ((Behaviour) ((Component) ((UIProgressBar) this.slider).foregroundWidget).GetComponent<UISprite>()).enabled = materials.Count > 0;
      this.tapLabelObj.SetActive(materials.Count <= 0);
      for (int i = 0; i < this.m_MaterialIconList.Count; ++i)
      {
        if (i < this.m_Materials.Count)
        {
          InventoryItem material = this.m_Materials[i];
          if (material != null)
          {
            if (Object.op_Equality((Object) this.m_MaterialIconList[i].m_ItemIcon, (Object) null))
              this.m_MaterialIconList[i].m_ItemIcon = this.m_ItemIconPrefab.CloneAndGetComponent<ItemIcon>(this.m_MaterialIconList[i].m_Parent.transform);
            IEnumerator e = this.m_MaterialIconList[i].m_ItemIcon.InitByItemInfo(material.Item);
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
      for (int index = 0; index < this.m_MaterialIconList.Count; ++index)
      {
        if (Object.op_Inequality((Object) this.m_MaterialIconList[index].m_ItemIcon, (Object) null))
          ((Component) this.m_MaterialIconList[index].m_ItemIcon).gameObject.SetActive(false);
        this.m_MaterialIconList[index].m_AddMaterialObject.SetActive(true);
      }
    }
    ((UIButtonColor) this.m_DrillingBtn).isEnabled = ((materials == null ? 0 : (materials.Count > 0 ? 1 : 0)) & (canDriling ? 1 : 0)) != 0;
    Transform child = ((Component) this.uiDrag).transform.GetChild(0);
    if (Object.op_Inequality((Object) child, (Object) null))
    {
      Vector4 baseClipRegion = ((Component) child).GetComponent<UIPanel>().baseClipRegion;
      ((Component) child).GetComponent<UIPanel>().baseClipRegion = new Vector4(baseClipRegion.x, 0.0f, baseClipRegion.z, baseClipRegion.w);
    }
  }

  private IEnumerator setDispParam()
  {
    IEnumerator e;
    if (this.response == null)
    {
      e = this.SetNoResponse();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      this.m_After = new GameCore.ItemInfo(this.response.player_item);
      e = this.SetResponse(this.response);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    yield return (object) this.SetReisouInfo();
  }

  private IEnumerator SetReisouInfo()
  {
    ReisouRenseiMenu reisouRenseiMenu = this;
    PlayerItem equipGear = Array.Find<PlayerItem>(SMManager.Get<PlayerItem[]>(), (Predicate<PlayerItem>) (x => x.equipped_reisou_player_gear_id == this.reisouInfo.id && x.isWeapon()));
    PlayerItem dispReisouInfo = reisouRenseiMenu.reisouInfo;
    if (reisouRenseiMenu.reisouInfoAfter != (PlayerItem) null)
      dispReisouInfo = reisouRenseiMenu.reisouInfoAfter;
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
    {
      reisouRenseiMenu.DirReisou.SetActive(false);
    }
    else
    {
      foreach (Component component in reisouRenseiMenu.DynReisouIcon.transform)
        Object.Destroy((Object) component.gameObject);
      foreach (Component component in reisouRenseiMenu.DynReisouIconForBase.transform)
        Object.Destroy((Object) component.gameObject);
      if (Object.op_Inequality((Object) reisouRenseiMenu.reisouIcon, (Object) null))
        Object.Destroy((Object) reisouRenseiMenu.reisouIcon);
      if (dispReisouInfo == (PlayerItem) null)
      {
        reisouRenseiMenu.DirReisou.SetActive(false);
      }
      else
      {
        reisouRenseiMenu.DirReisou.SetActive(true);
        reisouRenseiMenu.reisouIcon = reisouRenseiMenu.m_ItemIconPrefab.CloneAndGetComponent<ItemIcon>(reisouRenseiMenu.DynReisouIcon.transform);
        IEnumerator e = reisouRenseiMenu.reisouIcon.InitByPlayerItem(reisouRenseiMenu.reisouInfo);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        reisouRenseiMenu.reisouIcon.setEquipReisouDisp();
        GameCore.ItemInfo dispInfoTemp = new GameCore.ItemInfo(dispReisouInfo);
        reisouRenseiMenu.reisouIcon.onClick = (Action<ItemIcon>) (x => this.OpenReisouDetailPopup(dispInfoTemp, dispReisouInfo, new Action(this.cbRemoveReisou)));
        reisouRenseiMenu.EnableLongPressEventReisou(dispInfoTemp, reisouRenseiMenu.reisouIcon, dispReisouInfo, new Action(reisouRenseiMenu.cbRemoveReisou));
        if (equipGear != (PlayerItem) null)
        {
          reisouRenseiMenu.reisouIconForBase = reisouRenseiMenu.m_ItemIconPrefab.CloneAndGetComponent<ItemIcon>(reisouRenseiMenu.DynReisouIconForBase.transform);
          e = reisouRenseiMenu.reisouIconForBase.InitByPlayerItem(equipGear);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          reisouRenseiMenu.reisouIconForBase.setEquipReisouDisp();
          reisouRenseiMenu.reisouIconForBase.gear.dynReisouEffect.transform.Clear();
          reisouRenseiMenu.reisouIconForBase.EnableLongPressEvent();
        }
        if (reisouRenseiMenu.reisouInfo.gear.isHolyReisou())
        {
          reisouRenseiMenu.setReisouGaugeExp(dispReisouInfo, reisouRenseiMenu.reisouInfo, reisouRenseiMenu.HolyReisouExpGauge);
          reisouRenseiMenu.ChaosReisouExpGauge.DirReisou.SetActive(false);
        }
        else if (reisouRenseiMenu.reisouInfo.gear.isChaosReisou())
        {
          reisouRenseiMenu.HolyReisouExpGauge.DirReisou.SetActive(false);
          reisouRenseiMenu.setReisouGaugeExp(dispReisouInfo, reisouRenseiMenu.reisouInfo, reisouRenseiMenu.ChaosReisouExpGauge);
        }
        else
        {
          GearReisouFusion fusionMineRecipe = reisouRenseiMenu.reisouInfo.GetReisouFusionMineRecipe();
          PlayerItem dispReisouItem1 = new PlayerItem(fusionMineRecipe.holy_ID, dispReisouInfo.GetPlayerMythologyGearStatus());
          PlayerItem baseReisouItem1 = new PlayerItem(fusionMineRecipe.holy_ID, reisouRenseiMenu.reisouInfo.GetPlayerMythologyGearStatus());
          reisouRenseiMenu.setReisouGaugeExp(dispReisouItem1, baseReisouItem1, reisouRenseiMenu.HolyReisouExpGauge);
          PlayerItem dispReisouItem2 = new PlayerItem(fusionMineRecipe.chaos_ID, dispReisouInfo.GetPlayerMythologyGearStatus());
          PlayerItem baseReisouItem2 = new PlayerItem(fusionMineRecipe.chaos_ID, reisouRenseiMenu.reisouInfo.GetPlayerMythologyGearStatus());
          reisouRenseiMenu.setReisouGaugeExp(dispReisouItem2, baseReisouItem2, reisouRenseiMenu.ChaosReisouExpGauge);
        }
        if (reisouRenseiMenu.reisouInfo.gear.isHolyReisou())
          reisouRenseiMenu.setHolyParamSkillDetail(dispReisouInfo);
        else if (reisouRenseiMenu.reisouInfo.gear.isChaosReisou())
        {
          reisouRenseiMenu.setChaosParamSkillDetail(dispReisouInfo, false, (GearGear) null);
        }
        else
        {
          GearReisouFusion fusionMineRecipe = dispReisouInfo.GetReisouFusionMineRecipe();
          PlayerItem playerItem1 = new PlayerItem(fusionMineRecipe.holy_ID, MasterDataTable.CommonRewardType.gear);
          PlayerItem playerItem2 = new PlayerItem(fusionMineRecipe.chaos_ID, MasterDataTable.CommonRewardType.gear);
          PlayerMythologyGearStatus mythologyGearStatus = dispReisouInfo.GetPlayerMythologyGearStatus();
          playerItem1.gear_level = mythologyGearStatus.holy_gear_level;
          playerItem1.gear_level_limit = mythologyGearStatus.holy_gear_level_limit;
          playerItem1.gear_exp = mythologyGearStatus.holy_gear_exp;
          playerItem1.gear_exp_next = mythologyGearStatus.holy_gear_exp_next;
          playerItem2.gear_level = mythologyGearStatus.chaos_gear_level;
          playerItem2.gear_level_limit = mythologyGearStatus.chaos_gear_level_limit;
          playerItem2.gear_exp = mythologyGearStatus.chaos_gear_exp;
          playerItem2.gear_exp_next = mythologyGearStatus.chaos_gear_exp_next;
          reisouRenseiMenu.setHolyParamSkillDetail(playerItem1);
          reisouRenseiMenu.setChaosParamSkillDetail(playerItem2, true, fusionMineRecipe.holy_ID);
        }
      }
    }
  }

  private void setReisouGaugeExp(
    PlayerItem dispReisouItem,
    PlayerItem baseReisouItem,
    ReisouRenseiMenu.ReisouExpGauge reisouExpGauge)
  {
    reisouExpGauge.DirReisou.SetActive(true);
    string str = dispReisouItem.gear_level.ToString();
    if (dispReisouItem.gear_level > baseReisouItem.gear_level)
      str = ReisouRenseiMenu.COLOR_TAG_GREEN.F((object) str);
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

  public void cbRemoveReisou() => this.StartCoroutine(this.cbRemoveReisouAsync());

  protected IEnumerator cbRemoveReisouAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    ReisouRenseiMenu reisouRenseiMenu = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      reisouRenseiMenu.backScene();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    reisouRenseiMenu.reisouInfoAfter = (PlayerItem) null;
    reisouRenseiMenu.reisouInfo = (PlayerItem) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) reisouRenseiMenu.setDispParam();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
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
        if (Object.op_Equality((Object) ReisouRenseiMenu.reisouPopupDualSkillPrefab, (Object) null))
        {
          popupPrefabF = new ResourceObject("Prefabs/UnitGUIs/PopupReisouSkillDetails_DualSkill").Load<GameObject>();
          e = popupPrefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          ReisouRenseiMenu.reisouPopupDualSkillPrefab = popupPrefabF.Result;
          popupPrefabF = (Future<GameObject>) null;
        }
        popup = ReisouRenseiMenu.reisouPopupDualSkillPrefab.Clone();
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
        if (Object.op_Equality((Object) ReisouRenseiMenu.reisouPopupPrefab, (Object) null))
        {
          popupPrefabF = new ResourceObject("Prefabs/UnitGUIs/PopupReisouSkillDetails").Load<GameObject>();
          e = popupPrefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          ReisouRenseiMenu.reisouPopupPrefab = popupPrefabF.Result;
          popupPrefabF = (Future<GameObject>) null;
        }
        popup = ReisouRenseiMenu.reisouPopupPrefab.Clone();
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

  private void SetRarity(GearGear gear, UI2DSprite dstSprite)
  {
    if (Object.op_Equality((Object) dstSprite, (Object) null))
      return;
    ((Component) dstSprite).gameObject.SetActive(false);
    if (gear.rarity.index <= 0)
      return;
    Sprite sprite = Resources.Load<Sprite>(ReisouRenseiMenu.spriteNameTBL[gear.rarity.index - 1]);
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

  private IEnumerator SetNoResponse()
  {
    this.m_GearTypeIcon.Init(this.reisouInfo.gear.kind, this.reisouInfo.GetElement());
    this.m_ZenyLabel.SetTextLocalize(0);
    yield return (object) null;
  }

  private IEnumerator SetResponse(
    WebAPI.Response.ItemGearBulkDrillingConfirm response)
  {
    Consts.GetInstance();
    PlayerItem playerItem = response.player_item;
    this.m_GearTypeIcon.Init(playerItem.gear.kind, playerItem.GetElement());
    this.m_ZenyLabel.SetTextLocalize(response.consume_money);
    yield return (object) null;
  }

  protected void setHolyParamSkillDetail(PlayerItem playerItem)
  {
    Judgement.GearParameter gearParam = Judgement.GearParameter.FromPlayerGear(playerItem);
    Queue<string> paramTextQueue = new Queue<string>();
    this.SetHolyParamSkillStrings(paramTextQueue, playerItem, gearParam);
    this.reisouSkill1.SetText("");
    this.reisouSkill1.fontSize = paramTextQueue.Count > 1 ? 15 : 18;
    int num = 0;
    while (paramTextQueue.Count > 0)
    {
      UILabel reisouSkill1 = this.reisouSkill1;
      reisouSkill1.text = reisouSkill1.text + paramTextQueue.Dequeue() + "\n";
      ++num;
    }
  }

  protected void SetHolyParamSkillStrings(
    Queue<string> paramTextQueue,
    PlayerItem playerItem,
    Judgement.GearParameter gearParam)
  {
    if (gearParam.Hp > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_HP.F((object) gearParam.Hp);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.Strength > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_POWER.F((object) gearParam.Strength);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.Intelligence > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_MAGIC_POWER.F((object) gearParam.Intelligence);
      paramTextQueue.Enqueue(str);
    }
    int vitalityIncremental = playerItem.vitality_incremental;
    if (vitalityIncremental > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_VITALITY.F((object) vitalityIncremental);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.Mind > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_MIND.F((object) gearParam.Mind);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.Agility > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_AGILITY.F((object) gearParam.Agility);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.Dexterity > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_DEXTERITY.F((object) gearParam.Dexterity);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.Luck > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_LUCK.F((object) gearParam.Luck);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.PhysicalPower > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_PHY_POW.F((object) gearParam.PhysicalPower);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.MagicalPower > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_MAG_POW.F((object) gearParam.MagicalPower);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.PhysicalDefense > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_PHY_DEF.F((object) gearParam.PhysicalDefense);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.MagicDefense > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_MAG_DEF.F((object) gearParam.MagicDefense);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.Hit > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_HIT.F((object) gearParam.Hit);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.Critical > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_CRITICAL.F((object) gearParam.Critical);
      paramTextQueue.Enqueue(str);
    }
    int evasion = playerItem.evasion;
    if (evasion <= 0)
      return;
    string str1 = Consts.GetInstance().UNIT_0044_HOLY_REISOU_EVASION.F((object) evasion);
    paramTextQueue.Enqueue(str1);
  }

  protected void setChaosParamSkillDetail(PlayerItem playerItem, bool isShinwa, GearGear gear)
  {
    Queue<string> paramTextQueue = new Queue<string>();
    if (!isShinwa)
      this.SetChaosParamSkillStrings(paramTextQueue, playerItem, playerItem.reisouRankIncr);
    else
      this.SetChaosParamSkillStringsForMythology(paramTextQueue, playerItem, playerItem.reisouRankIncr, gear);
    this.reisouSkill2.SetText("");
    this.reisouSkill2.fontSize = paramTextQueue.Count > 1 ? 15 : 18;
    int num = 0;
    while (paramTextQueue.Count > 0)
    {
      UILabel reisouSkill2 = this.reisouSkill2;
      reisouSkill2.text = reisouSkill2.text + paramTextQueue.Dequeue() + "\n";
      ++num;
    }
  }

  protected void SetChaosParamSkillStrings(
    Queue<string> paramTextQueue,
    PlayerItem playerItem,
    ReisouRankIncr rankIncr)
  {
    if (rankIncr.hp_incremental > 0 && rankIncr.hp_incremental != 100)
    {
      double num = (double) rankIncr.hp_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_HP.F((object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.strength_incremental > 0 && rankIncr.strength_incremental != 100)
    {
      double num = (double) rankIncr.strength_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_POWER.F((object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.intelligence_incremental > 0 && rankIncr.intelligence_incremental != 100)
    {
      double num = (double) rankIncr.intelligence_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MAGIC_POWER.F((object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.vitality_incremental > 0 && rankIncr.vitality_incremental != 100)
    {
      double num = (double) rankIncr.vitality_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_VITALITY.F((object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.mind_incremental > 0 && rankIncr.mind_incremental != 100)
    {
      double num = (double) rankIncr.mind_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MIND.F((object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.agility_incremental > 0 && rankIncr.agility_incremental != 100)
    {
      double num = (double) rankIncr.agility_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_AGILITY.F((object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.dexterity_incremental > 0 && rankIncr.dexterity_incremental != 100)
    {
      double num = (double) rankIncr.dexterity_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_DEXTERITY.F((object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.lucky_incremental > 0 && rankIncr.lucky_incremental != 100)
    {
      double num = (double) rankIncr.lucky_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_LUCK.F((object) num);
      paramTextQueue.Enqueue(str);
    }
    int num1 = 0;
    if (playerItem.gear.attack_type == GearAttackType.physical)
      num1 = rankIncr.power;
    if (num1 > 0 && num1 != 100)
    {
      double num2 = (double) num1 / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_PHY_POW.F((object) num2);
      paramTextQueue.Enqueue(str);
    }
    int num3 = 0;
    if (playerItem.gear.attack_type == GearAttackType.magic)
      num3 = rankIncr.power;
    if (num3 > 0 && num3 != 100)
    {
      double num4 = (double) num3 / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MAG_POW.F((object) num4);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.physical_defense > 0 && rankIncr.physical_defense != 100)
    {
      double num5 = (double) rankIncr.physical_defense / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_PHY_DEF.F((object) num5);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.magic_defense > 0 && rankIncr.magic_defense != 100)
    {
      double num6 = (double) rankIncr.magic_defense / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MAG_DEF.F((object) num6);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.hit > 0 && rankIncr.hit != 100)
    {
      double num7 = (double) rankIncr.hit / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_HIT.F((object) num7);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.critical > 0 && rankIncr.critical != 100)
    {
      double num8 = (double) rankIncr.critical / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_CRITICAL.F((object) num8);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.evasion <= 0 || rankIncr.evasion == 100)
      return;
    double num9 = (double) rankIncr.evasion / 100.0;
    string str1 = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_EVASION.F((object) num9);
    paramTextQueue.Enqueue(str1);
  }

  protected void SetChaosParamSkillStringsForMythology(
    Queue<string> paramTextQueue,
    PlayerItem playerItem,
    ReisouRankIncr rankIncr,
    GearGear holyReisou)
  {
    if (rankIncr.hp_incremental > 0 && rankIncr.hp_incremental != 100)
    {
      double num = (double) rankIncr.hp_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_HP_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.strength_incremental > 0 && rankIncr.strength_incremental != 100)
    {
      double num = (double) rankIncr.strength_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_POWER_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.intelligence_incremental > 0 && rankIncr.intelligence_incremental != 100)
    {
      double num = (double) rankIncr.intelligence_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MAGIC_POWER_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.vitality_incremental > 0 && rankIncr.vitality_incremental != 100)
    {
      double num = (double) rankIncr.vitality_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_VITALITY_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.mind_incremental > 0 && rankIncr.mind_incremental != 100)
    {
      double num = (double) rankIncr.mind_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MIND_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.agility_incremental > 0 && rankIncr.agility_incremental != 100)
    {
      double num = (double) rankIncr.agility_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_AGILITY_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.dexterity_incremental > 0 && rankIncr.dexterity_incremental != 100)
    {
      double num = (double) rankIncr.dexterity_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_DEXTERITY_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.lucky_incremental > 0 && rankIncr.lucky_incremental != 100)
    {
      double num = (double) rankIncr.lucky_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_LUCK_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num);
      paramTextQueue.Enqueue(str);
    }
    int num1 = 0;
    if (playerItem.gear.attack_type == GearAttackType.physical)
      num1 = rankIncr.power;
    if (num1 > 0 && num1 != 100)
    {
      double num2 = (double) num1 / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_PHY_POW_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num2);
      paramTextQueue.Enqueue(str);
    }
    int num3 = 0;
    if (playerItem.gear.attack_type == GearAttackType.magic)
      num3 = rankIncr.power;
    if (num3 > 0 && num3 != 100)
    {
      double num4 = (double) num3 / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MAG_POW_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num4);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.physical_defense > 0 && rankIncr.physical_defense != 100)
    {
      double num5 = (double) rankIncr.physical_defense / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_PHY_DEF_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num5);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.magic_defense > 0 && rankIncr.magic_defense != 100)
    {
      double num6 = (double) rankIncr.magic_defense / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MAG_DEF_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num6);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.hit > 0 && rankIncr.hit != 100)
    {
      double num7 = (double) rankIncr.hit / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_HIT_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num7);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.critical > 0 && rankIncr.critical != 100)
    {
      double num8 = (double) rankIncr.critical / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_CRITICAL_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num8);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.evasion <= 0 || rankIncr.evasion == 100)
      return;
    double num9 = (double) rankIncr.evasion / 100.0;
    string str1 = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_EVASION_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num9);
    paramTextQueue.Enqueue(str1);
  }

  public void ibtnChangeMaterial()
  {
    if (this.IsPushAndSet())
      return;
    GameCore.ItemInfo target = new GameCore.ItemInfo(this.reisouInfo);
    if (this.reisouInfo.gear_level == this.reisouInfo.gear_level_limit)
      ReisouRenseiSelectScene.ChangeScene(false, this.m_Materials, target, true);
    else
      ReisouRenseiSelectScene.ChangeScene(false, this.m_Materials, target, false);
  }

  public void ibtnDrilling()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.OpenMaterialCheckPopup());
  }

  private IEnumerator OpenMaterialCheckPopup()
  {
    ReisouRenseiMenu reisouRenseiMenu = this;
    GameObject obj = reisouRenseiMenu.m_CheckMaterialPopupPrefabMini.Clone();
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = obj.GetComponent<Popup00591Menu>().Init((GameCore.ItemInfo) null, reisouRenseiMenu.m_After, reisouRenseiMenu.reisouInfo, reisouRenseiMenu.reisouInfoAfter, reisouRenseiMenu.m_Materials, new Action(reisouRenseiMenu.\u003COpenMaterialCheckPopup\u003Eb__63_0), reisouRenseiMenu.m_ItemIconPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(obj, isCloned: true);
  }

  private IEnumerator drilling()
  {
    float visibleTime = 3f;
    int reisouJewel = SMManager.Get<Player>().reisou_jewel;
    int[] array1 = this.m_Materials.ToMaterialId().ToArray();
    int[] array2 = this.m_Materials.ToGearId().ToArray();
    int[] array3 = this.m_Materials.ToMaterialCounts().ToArray();
    Future<WebAPI.Response.ItemGearBulkDrilling> feature = WebAPI.ItemGearBulkDrilling(this.reisouInfo.id, array2, array1, array3);
    IEnumerator e = feature.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PlayerItem playerItem = feature.Result.player_item;
    this.m_ParentScene.SetResetFlag();
    e = this.m_ParentScene.onBackSceneAsync(playerItem, (List<InventoryItem>) null);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<NGMessageUI>.GetInstance().SetMessageByTime(Consts.GetInstance().POPUP_004_TITLE_REISOU_DRILLING_FINNISH, visibleTime, colorType: NGMessageUI.ColorType.Gray, enabledSe: true);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    if (this.m_ParentScene.IsDrillingMax)
    {
      visibleTime = 2f;
      Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    }
    if (this.tapLabelObj.activeSelf)
    {
      this.tapLabelObj.SetActive(false);
      yield return (object) new WaitForSeconds(visibleTime);
      this.tapLabelObj.SetActive(true);
    }
    if (this.m_ParentScene.IsDrillingMax)
    {
      Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
      this.callBackScene();
    }
  }

  public override void onBackButton() => this.ibtnBack();

  public void ibtnBack()
  {
    Singleton<NGGameDataManager>.GetInstance().isReisouScene = false;
    this.backScene();
  }

  public void callBackScene() => this.backScene();

  protected override void backScene()
  {
    if (Singleton<NGSceneManager>.GetInstance().backScene())
      return;
    Bugu00526Scene.ChangeScene(false);
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
