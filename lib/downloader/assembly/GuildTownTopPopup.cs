// Decompiled with JetBrains decompiler
// Type: GuildTownTopPopup
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
public class GuildTownTopPopup : BackButtonMonoBehaiviour
{
  private Guild0282Menu guildMapMenu;
  private Guild0282MemberBaseMenu memberBaseMenu;
  [SerializeField]
  private GameObject dir_player;
  [SerializeField]
  private GameObject dir_guildmember;
  [SerializeField]
  private UILabel lblMapNamePlayer;
  [SerializeField]
  private UILabel lblMapNameGuildmember;
  [SerializeField]
  private UILabel lblMedalNum;
  [SerializeField]
  private UIButton btnEdit;
  [SerializeField]
  private UI2DSprite iconMedal;
  [SerializeField]
  private UIButton btnTestBattle;
  [SerializeField]
  private GuildTownTopPopup.MapInfo mapInfoPlayer;
  [SerializeField]
  private GuildTownTopPopup.MapInfo mapInfoMember;
  private GameObject mapChipPrefab;
  private GameObject mapStoragePrefab;
  private GameObject facilityStoragePrefab;
  private GameObject facilityDetailPopup;
  private GameObject facilitySellNumEnterPopup;
  private UIScrollView scrollView;
  private UIGrid grid;
  private UICenterOnChild centerOnChild;
  private NGHorizontalScrollParts scrollParts;
  private bool isPush;
  private int defaultSlotNo;
  private int currentSlotNo;
  private GameObject currentObject;
  private PlayerGuildTownSlot[] townSlotList;
  private PlayerGuildFacility[] facilityList;
  private GuildTownTopPopup.MapInfo mapInfo;
  private bool includeTrap;
  private Action eventFinished;

  private void OnEnable()
  {
    this.isPush = false;
    this.lblMedalNum.SetTextLocalize(PlayerAffiliation.Current.guild_medal);
    ((UIWidget) this.lblMedalNum).color = new Color(1f, 1f, 1f, 1f);
    ((UIWidget) this.iconMedal).color = new Color(1f, 1f, 1f, 1f);
  }

  private IEnumerator ResourceLoad()
  {
    Future<GameObject> f;
    IEnumerator e;
    if (Object.op_Equality((Object) this.mapChipPrefab, (Object) null))
    {
      f = new ResourceObject("Prefabs/guild_town/MapChipContainer").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mapChipPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.mapStoragePrefab, (Object) null))
    {
      f = new ResourceObject("Prefabs/map_edit031/dir_map_storage").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mapStoragePrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.facilityStoragePrefab, (Object) null))
    {
      f = new ResourceObject("Prefabs/map_edit031/dir_facility_storage").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.facilityStoragePrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.facilityDetailPopup, (Object) null))
    {
      f = new ResourceObject("Prefabs/popup/popup_028_facility_option__anim_popup01").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.facilityDetailPopup = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.facilitySellNumEnterPopup, (Object) null))
    {
      f = new ResourceObject("Prefabs/popup/popup_028_guild_sell_numEnter__anim_popup01").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.facilitySellNumEnterPopup = f.Result;
      f = (Future<GameObject>) null;
    }
  }

  private IEnumerator CommonInit(
    PlayerGuildTownSlot[] townSlots,
    PlayerGuildFacility[] facilities,
    int default_town_slot_number,
    bool includeTrap)
  {
    IEnumerator e = this.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.townSlotList = townSlots;
    this.facilityList = facilities;
    this.includeTrap = includeTrap;
    for (int i = 0; i < townSlots.Length; ++i)
    {
      GuildTownMapScroll component = this.mapChipPrefab.Clone(((Component) this.grid).transform).GetComponent<GuildTownMapScroll>();
      List<Tuple<int, int>> positionList = new List<Tuple<int, int>>();
      for (int index = 0; index < townSlots[i].facilities_data.Length; ++index)
      {
        MapFacility mapFacility;
        if (MasterData.MapFacility.TryGetValue(townSlots[i].facilities_data[index].master_id, out mapFacility) && (this.includeTrap || mapFacility.category_id != 4))
          positionList.Add(new Tuple<int, int>(townSlots[i].facilities_data[index].x, townSlots[i].facilities_data[index].y));
      }
      e = component.InitializeAsync(townSlots[i].master.stage_id, !this.dir_guildmember.activeSelf, i, positionList, this.memberBaseMenu);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    ((Behaviour) this.scrollParts).enabled = true;
    this.defaultSlotNo = default_town_slot_number;
    this.currentSlotNo = default_town_slot_number;
    Transform child = ((Component) this.grid).transform.GetChild(this.currentSlotNo);
    this.currentObject = Object.op_Inequality((Object) child, (Object) null) ? ((Component) child).gameObject : (GameObject) null;
  }

  private void onFinished()
  {
    this.UpdateMapInfo();
    if (this.eventFinished == null)
      return;
    this.eventFinished();
    this.eventFinished = (Action) null;
  }

  private void UpdateMapInfo()
  {
    for (int i = 0; i < ((Component) this.grid).transform.childCount; i++)
    {
      if (((Component) this.grid).transform.GetChild(i).Equals((object) this.centerOnChild.centeredObject.transform))
      {
        this.mapInfo.lblMapName.SetTextLocalize(this.townSlotList[i].master.name);
        int num = 0;
        for (int j = 0; j < this.townSlotList[i].facilities_data.Length; j++)
        {
          PlayerGuildFacility playerGuildFacility = ((IEnumerable<PlayerGuildFacility>) this.facilityList).FirstOrDefault<PlayerGuildFacility>((Func<PlayerGuildFacility, bool>) (x => x._master == this.townSlotList[i].facilities_data[j].master_id));
          if (playerGuildFacility != null)
            num += playerGuildFacility.unit.cost;
        }
        this.mapInfo.lblCostValue.SetTextLocalize(string.Format("{0}/{1}", (object) num, (object) this.townSlotList[i].master.cost_capacity));
        this.mapInfo.lblSlotNum.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_TOWN_SLOT_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "num",
            (object) (this.townSlotList[i].slot_number + 1)
          }
        }));
        this.currentSlotNo = this.townSlotList[i].slot_number;
        this.currentObject = this.centerOnChild.centeredObject;
        if (i == this.defaultSlotNo)
        {
          this.mapInfo.slc_guild_battleSet_icon_on.SetActive(true);
          this.mapInfo.slc_guild_battleSet_icon_off.SetActive(false);
          break;
        }
        this.mapInfo.slc_guild_battleSet_icon_on.SetActive(false);
        this.mapInfo.slc_guild_battleSet_icon_off.SetActive(true);
        break;
      }
    }
  }

  private IEnumerator RefreshMapchipInfo()
  {
    GuildTownTopPopup guildTownTopPopup = this;
    guildTownTopPopup.townSlotList = SMManager.Get<PlayerGuildTownSlot[]>();
    guildTownTopPopup.facilityList = SMManager.Get<PlayerGuildFacility[]>();
    for (int i = 0; i < guildTownTopPopup.townSlotList.Length; ++i)
    {
      GuildTownMapScroll component = ((Component) ((Component) guildTownTopPopup.grid).transform.GetChild(i)).gameObject.GetComponent<GuildTownMapScroll>();
      if (!Object.op_Equality((Object) component, (Object) null))
      {
        List<Tuple<int, int>> positionList = new List<Tuple<int, int>>();
        for (int index = 0; index < guildTownTopPopup.townSlotList[i].facilities_data.Length; ++index)
        {
          MapFacility mapFacility;
          if (MasterData.MapFacility.TryGetValue(guildTownTopPopup.townSlotList[i].facilities_data[index].master_id, out mapFacility) && (guildTownTopPopup.includeTrap || mapFacility.category_id != 4))
            positionList.Add(new Tuple<int, int>(guildTownTopPopup.townSlotList[i].facilities_data[index].x, guildTownTopPopup.townSlotList[i].facilities_data[index].y));
        }
        IEnumerator e = component.InitializeAsync(guildTownTopPopup.townSlotList[i].master.stage_id, !guildTownTopPopup.dir_guildmember.activeSelf, i, positionList, guildTownTopPopup.memberBaseMenu);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (PlayerAffiliation.Current.default_town_slot_number == i)
          guildTownTopPopup.defaultSlotNo = i;
      }
    }
    // ISSUE: reference to a compiler-generated method
    guildTownTopPopup.focusCurrentMap(GuildUtil.last_edit_slot_no, new Action(guildTownTopPopup.\u003CRefreshMapchipInfo\u003Eb__36_0));
  }

  private IEnumerator ShowMapStoragePopup()
  {
    GameObject popup = this.mapStoragePrefab.Clone();
    popup.SetActive(false);
    IEnumerator e = popup.GetComponent<GuildMapStoragePopup>().InitializeAsync(this.guildMapMenu, SMManager.Get<PlayerGuildTown[]>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    this.guildMapMenu.openPopup(popup, GuildUtil.GvGPopupState.MapList, true, true);
    this.isPush = false;
  }

  private IEnumerator ShowFacilityStoragePopup()
  {
    GuildTownTopPopup guildTownTopPopup = this;
    GameObject popup = guildTownTopPopup.facilityStoragePrefab.Clone(guildTownTopPopup.guildMapMenu.dyn_battle_edit.transform);
    guildTownTopPopup.guildMapMenu.openPopup(popup, GuildUtil.GvGPopupState.FacilityList, true, true, true);
    MapEditFacilityStorage script = popup.GetComponent<MapEditFacilityStorage>();
    script.isLayoutEdit_ = false;
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = script.initialize(SMManager.Get<PlayerGuildFacility[]>(), new Action<PlayerGuildFacility>(guildTownTopPopup.\u003CShowFacilityStoragePopup\u003Eb__38_0), new Action(guildTownTopPopup.\u003CShowFacilityStoragePopup\u003Eb__38_1), (Dictionary<int, int>) null, int.MaxValue, int.MaxValue);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    while (!script.isInitialized_)
      yield return (object) null;
    guildTownTopPopup.guildMapMenu.startOpenAnime(popup);
    guildTownTopPopup.isPush = false;
  }

  private IEnumerator onSelectFacility(PlayerGuildFacility facility)
  {
    GuildTownTopPopup guildTownTopPopup = this;
    GameObject popup = guildTownTopPopup.facilityDetailPopup.Clone();
    popup.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = popup.GetComponent<PopupFacilityDetailMenu>().InitializeAsync(guildTownTopPopup.guildMapMenu, facility, guildTownTopPopup.facilitySellNumEnterPopup, new Action<int, int, int>(guildTownTopPopup.\u003ConSelectFacility\u003Eb__39_0));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
  }

  private IEnumerator onSellFacility(int amount, int facilityID, int totalMedal)
  {
    GuildTownTopPopup guildTownTopPopup = this;
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.GuildtownSellFacility> sellFacility = WebAPI.GuildtownSellFacility(amount, facilityID, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
    IEnumerator e1 = sellFacility.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    MapEditFacilityStorage componentInChildren = guildTownTopPopup.guildMapMenu.dyn_battle_edit.GetComponentInChildren<MapEditFacilityStorage>();
    if (Object.op_Inequality((Object) componentInChildren, (Object) null))
      componentInChildren.UpdatePossession(facilityID);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (sellFacility.Result != null)
    {
      guildTownTopPopup.lblMedalNum.SetTextLocalize(PlayerAffiliation.Current.guild_medal);
      // ISSUE: reference to a compiler-generated method
      ((MonoBehaviour) guildTownTopPopup.memberBaseMenu).StartCoroutine(PopupCommon.Show(Consts.GetInstance().POPUP_GUILD_FACILITY_SELL_RESULT_TITLE, Consts.Format(Consts.GetInstance().POPUP_GUILD_FACILITY_SELL_RESULT_DESC, (IDictionary) new Hashtable()
      {
        {
          (object) "num",
          (object) totalMedal
        }
      }), new Action(guildTownTopPopup.\u003ConSellFacility\u003Eb__40_1)));
    }
  }

  public void focusCurrentMap(int slotNo, Action callbackFinished = null)
  {
    if (Object.op_Equality((Object) this.centerOnChild, (Object) null))
      return;
    this.eventFinished = callbackFinished;
    // ISSUE: method pointer
    this.centerOnChild.onFinished = new SpringPanel.OnFinished((object) this, __methodptr(onFinished));
    this.scrollParts.setItemPosition(slotNo);
    this.scrollParts.setItemPositionQuick(slotNo);
  }

  public IEnumerator InitializeAsync(Guild0282Menu menu, Guild0282MemberBaseMenu memberMenu)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) new WaitForEndOfFrame();
    this.guildMapMenu = menu;
    this.memberBaseMenu = memberMenu;
    this.dir_player.SetActive(true);
    this.dir_guildmember.SetActive(false);
    PlayerGuildTownSlot[] townSlots = SMManager.Get<PlayerGuildTownSlot[]>();
    if (townSlots == null)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
    else
    {
      this.scrollView = this.dir_player.GetComponentInChildren<UIScrollView>();
      this.grid = this.dir_player.GetComponentInChildren<UIGrid>();
      this.centerOnChild = this.dir_player.GetComponentInChildren<UICenterOnChild>();
      this.scrollParts = this.dir_player.GetComponentInChildren<NGHorizontalScrollParts>();
      if (Object.op_Equality((Object) this.scrollView, (Object) null) || Object.op_Equality((Object) this.grid, (Object) null) || Object.op_Equality((Object) this.centerOnChild, (Object) null) || Object.op_Equality((Object) this.scrollParts, (Object) null))
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
      else
      {
        ((Behaviour) this.scrollParts).enabled = false;
        this.mapInfo = this.mapInfoPlayer;
        IEnumerator e = this.CommonInit(townSlots, SMManager.Get<PlayerGuildFacility[]>(), PlayerAffiliation.Current.default_town_slot_number, true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.lblMedalNum.SetTextLocalize(PlayerAffiliation.Current.guild_medal);
        if (PlayerAffiliation.Current.guild.gvg_status == GvgStatus.fighting)
        {
          ((Behaviour) this.btnEdit).enabled = false;
          ((UIButtonColor) this.btnEdit).isEnabled = false;
        }
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
    }
  }

  public IEnumerator InitializeAsync(
    Guild0282Menu menu,
    Guild0282MemberBaseMenu memberMenu,
    WebAPI.Response.GuildtownShow guildTownShow)
  {
    this.guildMapMenu = menu;
    this.memberBaseMenu = memberMenu;
    this.dir_player.SetActive(false);
    this.dir_guildmember.SetActive(true);
    if (guildTownShow == null)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
    else
    {
      PlayerGuildTownSlot[] guildTownSlots = guildTownShow.guild_town_slots;
      if (guildTownSlots == null)
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
      else
      {
        this.scrollView = this.dir_guildmember.GetComponentInChildren<UIScrollView>();
        this.grid = this.dir_guildmember.GetComponentInChildren<UIGrid>();
        this.centerOnChild = this.dir_guildmember.GetComponentInChildren<UICenterOnChild>();
        this.scrollParts = this.dir_guildmember.GetComponentInChildren<NGHorizontalScrollParts>();
        if (Object.op_Equality((Object) this.scrollView, (Object) null) || Object.op_Equality((Object) this.grid, (Object) null) || Object.op_Equality((Object) this.centerOnChild, (Object) null) || Object.op_Equality((Object) this.scrollParts, (Object) null))
        {
          Singleton<CommonRoot>.GetInstance().isLoading = false;
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        }
        else
        {
          this.mapInfo = this.mapInfoMember;
          IEnumerator e = this.CommonInit(guildTownSlots, guildTownShow.guild_facilities, guildTownShow.default_town_slot_number, false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          ((Behaviour) this.btnTestBattle).enabled = PlayerAffiliation.Current.guild.gvg_status != GvgStatus.fighting;
          ((UIButtonColor) this.btnTestBattle).isEnabled = PlayerAffiliation.Current.guild.gvg_status != GvgStatus.fighting;
          Singleton<CommonRoot>.GetInstance().isLoading = false;
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        }
      }
    }
  }

  public void editButton()
  {
    if (this.isPush)
      return;
    this.isPush = true;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    this.guildMapMenu.SetGvgPopup(GuildUtil.GvGPopupState.TownTop, (Action) (() => this.StartCoroutine(this.RefreshMapchipInfo())));
    MapEdit031TopScene.changeScene(true, this.currentSlotNo, true);
    this.isPush = false;
  }

  public void mapStorageButton()
  {
    if (this.isPush)
      return;
    this.isPush = true;
    this.StartCoroutine(this.ShowMapStoragePopup());
  }

  public void facilityStorageButton()
  {
    if (this.isPush)
      return;
    this.isPush = true;
    ((MonoBehaviour) this.guildMapMenu).StartCoroutine(this.ShowFacilityStoragePopup());
  }

  public void shopButton()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    this.guildMapMenu.SetGvgPopup(GuildUtil.GvGPopupState.None, (Action) null);
    Guild028ShopScene.ChangeScene(true);
  }

  public void testBattleButton()
  {
    if (this.isPush || PlayerAffiliation.Current.guild.gvg_status == GvgStatus.fighting)
      return;
    this.isPush = true;
    ((MonoBehaviour) this.guildMapMenu).StartCoroutine(this.memberBaseMenu.ShowBattlePreparationPopup(this.currentSlotNo, true));
  }

  public override void onBackButton()
  {
    if (this.isPush)
      return;
    this.isPush = true;
    this.guildMapMenu.closePopup();
  }

  public void setDefenceTownButton()
  {
    if (PlayerAffiliation.Current.guild.gvg_status == GvgStatus.fighting || PlayerAffiliation.Current.guild.gvg_status == GvgStatus.aggregating || Object.op_Equality((Object) this.scrollView, (Object) null) || Object.op_Equality((Object) this.centerOnChild, (Object) null) || Object.op_Inequality((Object) this.currentObject, (Object) this.centerOnChild.centeredObject) || this.scrollView.isDragging || this.currentSlotNo == this.defaultSlotNo || this.isPush)
      return;
    this.isPush = true;
    this.StartCoroutine(this.doSetDefenceTown());
  }

  private IEnumerator doSetDefenceTown()
  {
    ((Behaviour) this.scrollView).enabled = false;
    int selectedSlot = this.currentSlotNo;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Future<WebAPI.Response.GuildtownSelect> future = WebAPI.GuildtownSelect(selectedSlot, (Action<WebAPI.Response.UserError>) (err =>
    {
      WebAPI.DefaultUserErrorCallback(err);
      if (err.Code.Equals("GVG002"))
        return;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (future.Result != null)
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      Future<GameObject> ldPopup = Res.Prefabs.popup.popup_028_guild_common_ok__anim_popup01.Load<GameObject>();
      e = ldPopup.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bool bWait = true;
      if (Object.op_Inequality((Object) ldPopup.Result, (Object) null))
      {
        GuildOkPopup component = Singleton<PopupManager>.GetInstance().open(ldPopup.Result).GetComponent<GuildOkPopup>();
        Consts instance = Consts.GetInstance();
        string titleSelectedDefence = instance.POPUP_GUILD_TOWN_TITLE_SELECTED_DEFENCE;
        string message = Consts.Format(instance.POPUP_GUILD_TOWN_MESSAGE_SELECTED_DEFENCE, (IDictionary) new Hashtable()
        {
          {
            (object) "slot",
            (object) (selectedSlot + 1)
          }
        });
        Vector2? size = new Vector2?();
        Action ok = (Action) (() => bWait = false);
        component.Initialize(titleSelectedDefence, message, size, ok);
      }
      else
        bWait = false;
      this.defaultSlotNo = selectedSlot;
      this.UpdateMapInfo();
      while (bWait)
        yield return (object) null;
      ldPopup = (Future<GameObject>) null;
      ((Behaviour) this.scrollView).enabled = true;
      this.isPush = false;
    }
  }

  [Serializable]
  public class MapInfo
  {
    public UILabel lblMapName;
    public UILabel lblSlotNum;
    public UILabel lblCostValue;
    public GameObject slc_guild_battleSet_icon_on;
    public GameObject slc_guild_battleSet_icon_off;
  }
}
