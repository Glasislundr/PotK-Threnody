// Decompiled with JetBrains decompiler
// Type: Unit00443Menu
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
public class Unit00443Menu : EquipmentDetailMenuBase
{
  [SerializeField]
  [Tooltip("ここから先(シーン)には行かせない")]
  private bool isTerminal;
  [SerializeField]
  public UI2DSprite DynWeaponIll;
  public UILabel TxtTitle;
  [SerializeField]
  protected Transform DynWeaponModel;
  [SerializeField]
  protected GameObject charaThum;
  [SerializeField]
  protected GameObject DirAddStauts;
  [SerializeField]
  protected GameObject remainingManaSeedContainer;
  [SerializeField]
  protected UILabel remainingManaSeedLabel;
  public UIButton nowFavorite;
  public UIButton yetFavorite;
  public Transform TopStarPos;
  public NGHorizontalScrollParts indicator;
  public Unit00443indicator indicatorPage1;
  public Unit00443indicatorDirection indicatorPage2;
  public UIGrid grid;
  public GameCore.ItemInfo RetentionGear;
  public UIWidget ZoomBuguSprite;
  private UnitIcon uniticon;
  protected Unit004431Menu.Param sendParam = new Unit004431Menu.Param();
  [SerializeField]
  public UI2DSprite rarityStarsIcon;
  [SerializeField]
  protected GameObject DirReisou;
  [SerializeField]
  protected GameObject DynReisouIcon;
  [SerializeField]
  protected GameObject SlcAddReisou;
  [SerializeField]
  protected UIButton BtnReisou;
  [SerializeField]
  protected Unit00443Menu.ReisouExpGauge HolyReisouExpGauge;
  [SerializeField]
  protected Unit00443Menu.ReisouExpGauge ChaosReisouExpGauge;
  protected GameCore.ItemInfo reisouInfo;
  protected ItemIcon reisouIcon;
  protected GameObject itemIconPrefab;
  protected GameObject reisouPopupDualSkillPrefab;
  protected GameObject reisouPopupPrefab;
  protected bool is_for_reisou;
  private readonly int DISPLAY_OBJECT_MAX = 4;
  private int objectCnt;
  private int currentIndex;
  private List<InventoryItem> sortGearList;
  private GameObject buguDetail;
  private List<GameObject> objectList;
  private GameObject[] detailObject;
  public NGxScroll scrollObj;
  private Dictionary<GameObject, Unit00443BuguList> detailPrefabDict;
  private Unit00443BuguList currentBuguDatail;
  private int currentInfoPageIndex;
  private int chacheCount;
  [SerializeField]
  private GameObject rightArrows;
  [SerializeField]
  private GameObject leftArrows;
  private bool isArrowBtns = true;
  private bool isScrollViewDragStarts;
  private int scrollStartCurrents;
  [SerializeField]
  private UICenterOnChild centerOnChilds;
  private bool firstInit;
  private bool isLimited;
  protected PlayerUnit equiptargets;
  private const float WAIT_STATUS_CHANGE = 0.5f;
  private float waitStatusChange;
  public int countChangedSetting;
  private Dictionary<int, bool> firstSetting = new Dictionary<int, bool>();
  private Dictionary<int, bool> changeSetting = new Dictionary<int, bool>();
  private const int reisouGaugeWidth = 86;
  private int centerStatus = -1;
  private bool isRefesh;
  private bool isRefreshing;
  private int m_windowHeight;
  private int m_windowWidth;
  private float? oldScrollViewLocalX;

  public bool IsTerminal => this.isTerminal;

  public CustomDeck.GearInfo customGearInfo { get; set; }

  protected void SetTitleText(string gearName)
  {
    ((Component) this.TxtTitle).gameObject.SetActive(true);
    this.TxtTitle.SetText(gearName);
  }

  public IEnumerator Init(
    GameCore.ItemInfo targetGear,
    bool limited = false,
    bool is_for_reisou = false,
    List<InventoryItem> sortGears = null)
  {
    Unit00443Menu unit00443Menu = this;
    if (unit00443Menu.customGearInfo != null)
      targetGear = new GameCore.ItemInfo(unit00443Menu.customGearInfo.gear);
    if (unit00443Menu.firstInit)
    {
      unit00443Menu.RetentionGear = targetGear;
      unit00443Menu.sortGearList[unit00443Menu.currentIndex].Item = targetGear;
      unit00443Menu.SetChangeActiveComponent(true);
      unit00443Menu.SetMenuInformation(unit00443Menu.currentIndex);
    }
    else
    {
      unit00443Menu.RetentionGear = targetGear;
      if (sortGears != null)
      {
        if (sortGears[0].removeButton)
          sortGears.RemoveAt(0);
        unit00443Menu.sortGearList = sortGears;
      }
      else
        unit00443Menu.sortGearList = new List<InventoryItem>()
        {
          new InventoryItem(targetGear)
        };
      if (Object.op_Inequality((Object) unit00443Menu.scrollObj, (Object) null))
        ((Behaviour) unit00443Menu.scrollObj.scrollView).enabled = unit00443Menu.sortGearList.Count > 1;
      unit00443Menu.CreateFirstFavoriteSetting();
      unit00443Menu.isLimited = limited;
      foreach (Component component in unit00443Menu.charaThum.transform)
        Object.Destroy((Object) component.gameObject);
      if (unit00443Menu.sortGearList != null)
      {
        int index = Array.FindIndex<InventoryItem>(unit00443Menu.sortGearList.ToArray(), (Predicate<InventoryItem>) (x => x.Item.itemID == targetGear.itemID));
        unit00443Menu.currentIndex = index;
      }
      if (unit00443Menu.customGearInfo == null)
      {
        PlayerUnit[] playerUnitArray = SMManager.Get<PlayerUnit[]>();
        unit00443Menu.equiptargets = (PlayerUnit) null;
        foreach (PlayerUnit playerUnit in playerUnitArray)
        {
          if (((IEnumerable<int?>) playerUnit.equip_gear_ids).Contains<int?>(new int?(targetGear.itemID)))
          {
            unit00443Menu.equiptargets = playerUnit;
            break;
          }
        }
      }
      else
        unit00443Menu.equiptargets = unit00443Menu.customGearInfo.playerUnit;
      IEnumerator e = unit00443Menu.indicatorPage1.LoadPrefabs();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Future<GameObject> ItemIconF;
      if (Object.op_Equality((Object) unit00443Menu.itemIconPrefab, (Object) null))
      {
        ItemIconF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
        e = ItemIconF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        unit00443Menu.itemIconPrefab = ItemIconF.Result;
        ItemIconF = (Future<GameObject>) null;
      }
      if (Object.op_Equality((Object) unit00443Menu.buguDetail, (Object) null))
      {
        ItemIconF = Res.Prefabs.unit004_4_3.bugu_detail_list.Load<GameObject>();
        e = ItemIconF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        unit00443Menu.buguDetail = ItemIconF.Result;
        ItemIconF = (Future<GameObject>) null;
      }
      unit00443Menu.objectCnt = unit00443Menu.sortGearList.Count;
      if (unit00443Menu.objectCnt > unit00443Menu.DISPLAY_OBJECT_MAX)
        unit00443Menu.objectCnt = unit00443Menu.DISPLAY_OBJECT_MAX;
      unit00443Menu.currentInfoPageIndex = 0;
      unit00443Menu.chacheCount = 0;
      unit00443Menu.objectList = new List<GameObject>();
      unit00443Menu.detailObject = new GameObject[unit00443Menu.objectCnt];
      unit00443Menu.detailPrefabDict = new Dictionary<GameObject, Unit00443BuguList>();
      for (int index = 0; index < unit00443Menu.objectCnt; ++index)
      {
        unit00443Menu.detailObject[index] = Object.Instantiate<GameObject>(unit00443Menu.buguDetail);
        unit00443Menu.objectList.Add(unit00443Menu.detailObject[index]);
        unit00443Menu.scrollObj.Add(unit00443Menu.detailObject[index]);
        Unit00443BuguList component = unit00443Menu.detailObject[index].GetComponent<Unit00443BuguList>();
        unit00443Menu.detailPrefabDict.Add(unit00443Menu.detailObject[index], component);
      }
      yield return (object) null;
      unit00443Menu.scrollObj.ResolvePosition();
      ((Component) unit00443Menu.scrollObj.scrollView).transform.localPosition = new Vector3(-unit00443Menu.scrollObj.grid.cellWidth * (float) unit00443Menu.currentIndex, 0.0f, 0.0f);
      foreach (GameObject key in unit00443Menu.detailObject)
        unit00443Menu.detailPrefabDict[key].SetContainerPosition(unit00443Menu.scrollObj.scrollView.panel.GetViewSize().y);
      yield return (object) null;
      e = unit00443Menu.CreatePages(unit00443Menu.currentIndex);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) null;
      int start = unit00443Menu.currentIndex - 1 < 0 ? 0 : unit00443Menu.currentIndex - 1;
      int end = unit00443Menu.currentIndex + 1 >= unit00443Menu.sortGearList.Count ? unit00443Menu.sortGearList.Count - 1 : unit00443Menu.currentIndex + 1;
      for (int i = start; i <= end; ++i)
      {
        if (i != unit00443Menu.currentIndex)
        {
          e = unit00443Menu.CreatePages(i);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      unit00443Menu.SetMenuInformation(unit00443Menu.currentIndex);
      unit00443Menu.CenterOnChild(unit00443Menu.currentIndex);
      yield return (object) null;
      if (Object.op_Inequality((Object) unit00443Menu.nowFavorite, (Object) null))
      {
        ((Component) unit00443Menu.nowFavorite).gameObject.SetActive(targetGear.favorite);
        ((Component) unit00443Menu.yetFavorite).gameObject.SetActive(!targetGear.favorite);
      }
      unit00443Menu.StartCoroutine(unit00443Menu.WaitScrollSe());
      if (Object.op_Inequality((Object) unit00443Menu.remainingManaSeedContainer, (Object) null))
        unit00443Menu.remainingManaSeedContainer.SetActive(false);
      if (targetGear.gear != null && targetGear.gear.kind.Enum == GearKindEnum.accessories && targetGear.gear.disappearance_type_GearDisappearanceType == 1)
      {
        unit00443Menu.remainingManaSeedContainer.SetActive(true);
        unit00443Menu.remainingManaSeedLabel.SetTextLocalize(targetGear.gearAccessoryRemainingAmount);
      }
      for (int index = 0; index < unit00443Menu.objectList.Count; ++index)
        unit00443Menu.objectList[index].transform.localPosition = end < unit00443Menu.sortGearList.Count - 1 ? new Vector3(unit00443Menu.scrollObj.grid.cellWidth * (float) (end + index + 1), 0.0f, 0.0f) : new Vector3(unit00443Menu.scrollObj.grid.cellWidth * (float) (start - (index + 1)), 0.0f, 0.0f);
      unit00443Menu.firstInit = true;
    }
  }

  private IEnumerator SetReisouInfo(GameCore.ItemInfo targetGear, bool limited)
  {
    Unit00443Menu unit00443Menu = this;
    unit00443Menu.reisouInfo = (GameCore.ItemInfo) null;
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth)
    {
      if (Object.op_Inequality((Object) unit00443Menu.DirReisou, (Object) null))
        unit00443Menu.DirReisou.SetActive(false);
    }
    else
    {
      if (unit00443Menu.customGearInfo != null)
      {
        if (unit00443Menu.customGearInfo.reisou != (PlayerItem) null)
          unit00443Menu.reisouInfo = new GameCore.ItemInfo(unit00443Menu.customGearInfo.reisou);
      }
      else if (targetGear.reisou != (PlayerItem) null)
        unit00443Menu.reisouInfo = new GameCore.ItemInfo(targetGear.reisou);
      foreach (Component component in unit00443Menu.DynReisouIcon.transform)
        Object.Destroy((Object) component.gameObject);
      if (Object.op_Inequality((Object) unit00443Menu.reisouIcon, (Object) null))
        Object.Destroy((Object) unit00443Menu.reisouIcon);
      if (unit00443Menu.reisouInfo == null)
      {
        unit00443Menu.InitReisouEmpty();
      }
      else
      {
        unit00443Menu.DirReisou.SetActive(true);
        ((Component) unit00443Menu.BtnReisou).gameObject.SetActive(false);
        unit00443Menu.reisouIcon = unit00443Menu.itemIconPrefab.CloneAndGetComponent<ItemIcon>(unit00443Menu.DynReisouIcon.transform);
        IEnumerator e = unit00443Menu.reisouIcon.InitByItemInfo(unit00443Menu.reisouInfo);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        unit00443Menu.reisouIcon.setEquipReisouDisp();
        if (unit00443Menu.customGearInfo == null)
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          unit00443Menu.reisouIcon.onClick = !(unit00443Menu.is_for_reisou | limited) ? new Action<ItemIcon>(unit00443Menu.\u003CSetReisouInfo\u003Eb__73_1) : new Action<ItemIcon>(unit00443Menu.\u003CSetReisouInfo\u003Eb__73_0);
          unit00443Menu.reisouIcon.EnableLongPressEventReisou(removeCallback: new Action(unit00443Menu.cbRemoveReisou));
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          unit00443Menu.reisouIcon.onClick = new Action<ItemIcon>(unit00443Menu.\u003CSetReisouInfo\u003Eb__73_2);
          unit00443Menu.reisouIcon.EnableLongPressEventReisou(customGearBase: unit00443Menu.customGearInfo.gear);
        }
        if (unit00443Menu.reisouInfo.gear.isHolyReisou())
        {
          unit00443Menu.setReisouGaugeExp(unit00443Menu.reisouInfo.gearLevel, unit00443Menu.reisouInfo.gearLevelLimit, unit00443Menu.reisouInfo.gearExp, unit00443Menu.reisouInfo.gearExpNext, unit00443Menu.HolyReisouExpGauge);
          unit00443Menu.ChaosReisouExpGauge.DirReisou.SetActive(false);
        }
        else if (unit00443Menu.reisouInfo.gear.isChaosReisou())
        {
          unit00443Menu.HolyReisouExpGauge.DirReisou.SetActive(false);
          unit00443Menu.setReisouGaugeExp(unit00443Menu.reisouInfo.gearLevel, unit00443Menu.reisouInfo.gearLevelLimit, unit00443Menu.reisouInfo.gearExp, unit00443Menu.reisouInfo.gearExpNext, unit00443Menu.ChaosReisouExpGauge);
        }
        else
        {
          PlayerMythologyGearStatus mythologyGearStatus = unit00443Menu.reisouInfo.playerItem.GetPlayerMythologyGearStatus();
          unit00443Menu.setReisouGaugeExp(mythologyGearStatus.holy_gear_level, mythologyGearStatus.holy_gear_level_limit, mythologyGearStatus.holy_gear_exp, mythologyGearStatus.holy_gear_exp_next, unit00443Menu.HolyReisouExpGauge);
          unit00443Menu.setReisouGaugeExp(mythologyGearStatus.chaos_gear_level, mythologyGearStatus.chaos_gear_level_limit, mythologyGearStatus.chaos_gear_exp, mythologyGearStatus.chaos_gear_exp_next, unit00443Menu.ChaosReisouExpGauge);
        }
      }
    }
  }

  private IEnumerator SetWeaponImage(GameCore.ItemInfo targetGear)
  {
    Future<Sprite> spriteF = targetGear.gear.LoadSpriteBasic();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.DynWeaponIll.sprite2D = spriteF.Result;
    UI2DSprite dynWeaponIll1 = this.DynWeaponIll;
    Rect textureRect1 = spriteF.Result.textureRect;
    int num1 = Mathf.FloorToInt(((Rect) ref textureRect1).width);
    ((UIWidget) dynWeaponIll1).width = num1;
    UI2DSprite dynWeaponIll2 = this.DynWeaponIll;
    Rect textureRect2 = spriteF.Result.textureRect;
    int num2 = Mathf.FloorToInt(((Rect) ref textureRect2).height);
    ((UIWidget) dynWeaponIll2).height = num2;
    ((Component) this.DynWeaponIll).transform.localScale = Vector2.op_Implicit(new Vector2(0.8f, 0.8f));
  }

  private IEnumerator SetEquipUnit(PlayerUnit equiptargets, GameCore.ItemInfo targetGear, bool limited)
  {
    Unit00443Menu unit00443Menu = this;
    IEnumerator e;
    if (Object.op_Equality((Object) unit00443Menu.uniticon, (Object) null))
    {
      Future<GameObject> iconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = iconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject gameObject = iconPrefabF.Result.Clone(unit00443Menu.charaThum.transform);
      unit00443Menu.uniticon = gameObject.GetComponent<UnitIcon>();
      iconPrefabF = (Future<GameObject>) null;
    }
    if (equiptargets == (PlayerUnit) null)
    {
      unit00443Menu.uniticon.SetEmpty();
      if (!targetGear.broken)
      {
        unit00443Menu.uniticon.SelectUnit = true;
        // ISSUE: reference to a compiler-generated method
        unit00443Menu.uniticon.onClick = new Action<UnitIconBase>(unit00443Menu.\u003CSetEquipUnit\u003Eb__75_0);
      }
      else
        unit00443Menu.uniticon.onClick = (Action<UnitIconBase>) (_ => { });
    }
    else
    {
      e = unit00443Menu.uniticon.SetUnit(equiptargets, equiptargets.GetElement(), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit00443Menu.uniticon.setBottom(equiptargets);
      unit00443Menu.uniticon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
      unit00443Menu.uniticon.setLevelText(equiptargets);
      unit00443Menu.uniticon.princessType.SetPrincessType(equiptargets);
      // ISSUE: reference to a compiler-generated method
      unit00443Menu.uniticon.onClick = new Action<UnitIconBase>(unit00443Menu.\u003CSetEquipUnit\u003Eb__75_2);
      unit00443Menu.uniticon.SelectUnit = false;
    }
    if (limited || unit00443Menu.customGearInfo != null)
    {
      unit00443Menu.uniticon.SelectUnit = false;
      unit00443Menu.uniticon.onClick = (Action<UnitIconBase>) (_ => { });
      unit00443Menu.uniticon.SetIconBoxCollider(false);
    }
  }

  private void CreateFirstFavoriteSetting()
  {
    this.countChangedSetting = 0;
    this.firstSetting.Clear();
    this.changeSetting.Clear();
    foreach (InventoryItem inventoryItem in this.sortGearList.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.isWeapon || x.Item.isSupply)))
    {
      this.firstSetting.Add(inventoryItem.Item.itemID, inventoryItem.Item.favorite);
      this.changeSetting.Add(inventoryItem.Item.itemID, inventoryItem.Item.favorite);
    }
  }

  protected void SelectReisou(GameCore.ItemInfo item)
  {
    Unit0044ReisouScene.ChangeScene(true, this.RetentionGear, this.reisouInfo);
  }

  protected void InitReisouEmpty()
  {
    this.DirReisou.SetActive(true);
    ((Component) this.BtnReisou).gameObject.SetActive(true);
    this.HolyReisouExpGauge.DirReisou.SetActive(false);
    this.ChaosReisouExpGauge.DirReisou.SetActive(false);
    if (this.is_for_reisou || this.customGearInfo != null)
    {
      ((UIButtonColor) this.BtnReisou).isEnabled = false;
      this.SlcAddReisou.SetActive(false);
    }
    else
    {
      ((UIButtonColor) this.BtnReisou).isEnabled = true;
      this.SlcAddReisou.SetActive(true);
    }
  }

  protected void setReisouGaugeExp(
    int gear_level,
    int gear_level_limit,
    int gear_exp,
    int gear_exp_next,
    Unit00443Menu.ReisouExpGauge reisouExpGauge)
  {
    reisouExpGauge.DirReisou.SetActive(true);
    reisouExpGauge.TxtReisouRank.SetTextLocalize(Consts.GetInstance().UNIT_00443_REISOU_RANK.F((object) gear_level, (object) gear_level_limit));
    reisouExpGauge.TxtReisouNextRank.SetTextLocalize(Consts.Format(Consts.GetInstance().BUGU_0059_REMAIN, (IDictionary) new Hashtable()
    {
      {
        (object) "remain",
        (object) gear_exp_next
      }
    }));
    float num = 86f * ((float) gear_exp / (float) (gear_exp_next + gear_exp));
    if ((double) num == 0.0 || gear_exp_next + gear_exp == 0)
    {
      ((Component) reisouExpGauge.SlcReisouGauge).gameObject.SetActive(false);
    }
    else
    {
      ((Component) reisouExpGauge.SlcReisouGauge).gameObject.SetActive(true);
      ((UIWidget) reisouExpGauge.SlcReisouGauge).width = (int) num;
    }
  }

  public void cbRemoveReisou()
  {
    this.reisouInfo = (GameCore.ItemInfo) null;
    foreach (Component component in this.DynReisouIcon.transform)
      Object.Destroy((Object) component.gameObject);
    if (Object.op_Inequality((Object) this.reisouIcon, (Object) null))
      Object.Destroy((Object) this.reisouIcon);
    this.InitReisouEmpty();
  }

  protected IEnumerator setTexture(Future<Sprite> src, UI2DSprite to)
  {
    return src.Then<Sprite>((Func<Sprite, Sprite>) (sprite => to.sprite2D = sprite)).Wait();
  }

  public void UpdateSetting(int id, bool flg)
  {
    if (this.changeSetting[id] == flg)
      return;
    this.changeSetting[id] = flg;
    ++this.countChangedSetting;
  }

  public void changeFavorite()
  {
    ((Component) this.yetFavorite).gameObject.SetActive(((Component) this.nowFavorite).gameObject.activeSelf);
    ((Component) this.nowFavorite).gameObject.SetActive(!((Component) this.yetFavorite).gameObject.activeSelf);
    this.UpdateSetting(this.RetentionGear.itemID, !this.RetentionGear.favorite);
    this.RetentionGear.favorite = !this.RetentionGear.favorite;
  }

  public virtual IEnumerator FavoriteAPI()
  {
    if (this.countChangedSetting != 0)
    {
      this.countChangedSetting = 0;
      List<int> source1 = new List<int>();
      List<int> source2 = new List<int>();
      foreach (KeyValuePair<int, bool> keyValuePair in this.firstSetting)
      {
        bool flag = this.changeSetting[keyValuePair.Key];
        if (flag != keyValuePair.Value)
        {
          if (flag)
            source1.Add(keyValuePair.Key);
          else
            source2.Add(keyValuePair.Key);
        }
      }
      if (source1.Any<int>() || source2.Any<int>())
      {
        foreach (KeyValuePair<int, bool> keyValuePair in this.changeSetting)
          this.firstSetting[keyValuePair.Key] = keyValuePair.Value;
        int[] array1 = source1.ToArray();
        int[] array2 = source2.ToArray();
        if (!this.isTerminal)
        {
          IEnumerator f = WebAPI.ItemGearFavorite(((IEnumerable<int>) array1).ToArray<int>(), ((IEnumerable<int>) array2).ToArray<int>(), (Action<WebAPI.Response.UserError>) (error => WebAPI.DefaultUserErrorCallback(error))).Wait();
          while (f.MoveNext())
            yield return f.Current;
          f = (IEnumerator) null;
        }
      }
    }
  }

  public virtual void choiceUnitChangeScene()
  {
    Unit00468Scene.changeScene004431(true, this.sendParam);
  }

  protected IEnumerator WaitScrollSe()
  {
    yield return (object) new WaitForSeconds(0.3f);
    this.indicator.SeEnable = true;
  }

  public virtual void EndScene()
  {
    foreach (Component component in this.charaThum.transform)
      Object.Destroy((Object) component.gameObject);
    this.SetChangeActiveComponent(false);
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    if (this.RetentionGear != null)
      Singleton<NGGameDataManager>.GetInstance().lastReferenceItemID = this.RetentionGear.itemID;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnFavoriteOff()
  {
  }

  public virtual void IbtnFavoriteOn()
  {
  }

  public virtual void IbtnZoom() => Unit00446Scene.changeScene(true, this.RetentionGear.gear);

  public void IbtnReisou()
  {
    if (this.IsPushAndSet())
      return;
    Unit0044ReisouScene.ChangeScene(true, this.RetentionGear, this.reisouInfo);
  }

  protected override void Update()
  {
    if (!this.firstInit || this.isRefreshing)
      return;
    if (this.m_windowHeight == 0 || this.m_windowWidth == 0)
    {
      this.m_windowHeight = Screen.height;
      this.m_windowWidth = Screen.width;
    }
    else if (this.m_windowHeight != Screen.height || this.m_windowWidth != Screen.width)
    {
      Debug.Log((object) "Window size change detected.");
      this.StartCoroutine(this.indicatorPage2.Init(this.RetentionGear));
      this.m_windowHeight = Screen.height;
      this.m_windowWidth = Screen.width;
    }
    base.Update();
    this.UpdateCurrentItem();
  }

  public IEnumerator CreatePages(int gearIndex, bool isChangePage = false)
  {
    Unit00443Menu m = this;
    GameObject go = m.objectList[0];
    Unit00443BuguList d = m.detailPrefabDict[go];
    m.objectList.RemoveAt(0);
    Vector3 gridPos = ((Component) m.scrollObj.grid).transform.localPosition;
    go.transform.localPosition = new Vector3(m.scrollObj.grid.cellWidth * (float) gearIndex, 0.0f, 0.0f);
    yield return (object) null;
    IEnumerator e = d.Init(m, m.sortGearList[gearIndex].Item.gear, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    d.index = gearIndex;
    go.transform.localPosition = new Vector3(m.scrollObj.grid.cellWidth * (float) gearIndex, 0.0f, 0.0f);
    ((Component) m.scrollObj.grid).transform.localPosition = gridPos;
    if (PerformanceConfig.GetInstance().IsLowMemory && m.chacheCount > 0)
    {
      GC.Collect();
      Singleton<ResourceManager>.GetInstance().ClearCache();
      Resources.UnloadUnusedAssets();
      m.chacheCount = 0;
    }
    if (isChangePage)
      ++m.chacheCount;
  }

  private void UpdateCurrentItem()
  {
    int num1 = this.currentIndex;
    if ((double) ((Component) this.scrollObj.scrollView).transform.localPosition.x < 0.0)
    {
      int num2 = (int) Mathf.Abs((((Component) this.scrollObj.scrollView).transform.localPosition.x - this.scrollObj.grid.cellWidth / 2f) / this.scrollObj.grid.cellWidth);
      num1 = num2 <= this.sortGearList.Count ? num2 : this.sortGearList.Count - 1;
    }
    if (this.currentIndex != num1)
    {
      int num3 = this.currentIndex < num1 ? 1 : 0;
      bool flag = true;
      this.currentIndex = num1;
      if (this.currentIndex < 0)
      {
        this.currentIndex = 0;
        flag = false;
      }
      if (this.currentIndex >= this.sortGearList.Count)
      {
        this.currentIndex = this.sortGearList.Count - 1;
        flag = false;
      }
      this.SetMenuInformation(this.currentIndex);
      this.UpdateObjectList();
      if (num3 != 0)
      {
        if (this.currentIndex < this.sortGearList.Count - 1)
          this.StartCoroutine(this.CreatePages(this.currentIndex + 1, true));
      }
      else if (this.currentIndex > 0)
        this.StartCoroutine(this.CreatePages(this.currentIndex - 1, true));
      if (flag)
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1005");
    }
    if (this.scrollObj.scrollView.isDragging)
    {
      if (this.isScrollViewDragStarts)
        return;
      this.isScrollViewDragStarts = true;
      this.scrollStartCurrents = this.currentIndex;
    }
    else
    {
      if (this.isScrollViewDragStarts && this.scrollStartCurrents == this.currentIndex)
      {
        int currentIndex = this.currentIndex;
        double num4 = -(double) this.scrollObj.grid.cellWidth * (double) this.currentIndex;
        float num5 = this.scrollObj.grid.cellWidth * 0.25f;
        float num6 = (float) num4 - num5;
        float num7 = (float) num4 + num5;
        if ((double) ((Component) this.scrollObj.scrollView).transform.localPosition.x <= (double) num6)
          ++currentIndex;
        else if ((double) ((Component) this.scrollObj.scrollView).transform.localPosition.x >= (double) num7)
          --currentIndex;
        this.CenterOnChild(currentIndex <= this.sortGearList.Count ? currentIndex : this.sortGearList.Count - 1);
      }
      this.isScrollViewDragStarts = false;
    }
  }

  private void SetMenuInformation(int idx)
  {
    if (idx < 0 || idx > this.sortGearList.Count - 1)
      return;
    foreach (GameObject key in this.detailObject)
    {
      Unit00443BuguList unit00443BuguList = this.detailPrefabDict[key];
      if (unit00443BuguList.index == idx)
      {
        this.currentBuguDatail = unit00443BuguList;
        break;
      }
    }
    this.RetentionGear = this.sortGearList[idx].Item;
    this.currentBuguDatail.SetGearInformation();
    if (Object.op_Inequality((Object) this.rightArrows, (Object) null))
      this.rightArrows.SetActive(true);
    if (Object.op_Inequality((Object) this.leftArrows, (Object) null))
      this.leftArrows.SetActive(true);
    if (idx == 0 && Object.op_Inequality((Object) this.leftArrows, (Object) null))
      this.leftArrows.SetActive(false);
    if (idx == this.sortGearList.Count - 1 && Object.op_Inequality((Object) this.rightArrows, (Object) null))
      this.rightArrows.SetActive(false);
    this.StartCoroutine(this.RefreshInfo());
  }

  private IEnumerator RefreshInfo()
  {
    Unit00443Menu unit00443Menu = this;
    unit00443Menu.isRefreshing = true;
    if (unit00443Menu.customGearInfo == null)
    {
      PlayerUnit[] playerUnitArray = SMManager.Get<PlayerUnit[]>();
      unit00443Menu.equiptargets = (PlayerUnit) null;
      foreach (PlayerUnit playerUnit in playerUnitArray)
      {
        if (((IEnumerable<int?>) playerUnit.equip_gear_ids).Contains<int?>(new int?(unit00443Menu.RetentionGear.itemID)))
        {
          unit00443Menu.equiptargets = playerUnit;
          break;
        }
      }
    }
    IEnumerator e = unit00443Menu.SetIncrementalParameter(unit00443Menu.RetentionGear, unit00443Menu.DirAddStauts);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = unit00443Menu.SetEquipUnit(unit00443Menu.equiptargets, unit00443Menu.RetentionGear, unit00443Menu.isLimited);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = unit00443Menu.SetReisouInfo(unit00443Menu.RetentionGear, unit00443Menu.isLimited);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00443Menu.indicatorPage1.Init(unit00443Menu.RetentionGear, unit00443Menu.reisouInfo);
    e = unit00443Menu.indicatorPage2.Init(unit00443Menu.RetentionGear);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00443Menu.sendParam.gearId = unit00443Menu.RetentionGear.itemID;
    unit00443Menu.sendParam.gearKindId = unit00443Menu.RetentionGear.gear.kind_GearKind;
    if (Object.op_Inequality((Object) unit00443Menu.remainingManaSeedContainer, (Object) null))
      unit00443Menu.remainingManaSeedContainer.SetActive(false);
    if (unit00443Menu.RetentionGear.gear != null && unit00443Menu.RetentionGear.gear.kind.Enum == GearKindEnum.accessories && unit00443Menu.RetentionGear.gear.disappearance_type_GearDisappearanceType == 1)
    {
      unit00443Menu.remainingManaSeedContainer.SetActive(true);
      unit00443Menu.remainingManaSeedLabel.SetTextLocalize(unit00443Menu.RetentionGear.gearAccessoryRemainingAmount);
    }
    if (Object.op_Inequality((Object) unit00443Menu.nowFavorite, (Object) null))
    {
      ((Component) unit00443Menu.nowFavorite).gameObject.SetActive(unit00443Menu.RetentionGear.favorite);
      ((Component) unit00443Menu.yetFavorite).gameObject.SetActive(!unit00443Menu.RetentionGear.favorite);
    }
    unit00443Menu.isRefreshing = false;
  }

  private void CenterOnChild(int num)
  {
    if (num < 0)
      return;
    foreach (GameObject key in this.detailObject)
    {
      if (this.detailPrefabDict[key].index == num)
      {
        this.centerOnChilds.CenterOn(key.transform);
        break;
      }
    }
  }

  private void UpdateObjectList()
  {
    foreach (GameObject key in this.detailObject)
    {
      int index = this.detailPrefabDict[key].index;
      if ((index < this.currentIndex - 1 || index > this.currentIndex + 1) && !this.objectList.Contains(key))
        this.objectList.Add(key);
    }
  }

  public void IbtnLeftArrows()
  {
    if (!this.isArrowBtns)
      return;
    this.isArrowBtns = false;
    this.StartCoroutine(this.IsArrowBtnOns());
    int num = this.currentIndex - 1;
    if (num < 0)
      return;
    this.CenterOnChild(num);
  }

  public void IbtnRightArrows()
  {
    if (!this.isArrowBtns)
      return;
    this.isArrowBtns = false;
    this.StartCoroutine(this.IsArrowBtnOns());
    int num = this.currentIndex + 1;
    if (num > this.sortGearList.Count - 1)
      return;
    this.CenterOnChild(num);
  }

  protected IEnumerator IsArrowBtnOns()
  {
    yield return (object) new WaitForSeconds(0.2f);
    this.isArrowBtns = true;
  }

  private void SetChangeActiveComponent(bool isActive)
  {
    if (Object.op_Inequality((Object) this.rightArrows, (Object) null))
      this.rightArrows.SetActive(isActive);
    if (!Object.op_Inequality((Object) this.leftArrows, (Object) null))
      return;
    this.leftArrows.SetActive(isActive);
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
  }
}
