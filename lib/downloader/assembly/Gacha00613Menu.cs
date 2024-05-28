// Decompiled with JetBrains decompiler
// Type: Gacha00613Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Gacha00613Menu : BackButtonMenuBase
{
  private const int ICON_WIDTH = 114;
  private const int ICON_HEIGHT = 136;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private GameObject dir_bonus;
  [SerializeField]
  private GameObject scrollContainerForBonus;
  [SerializeField]
  private UIScrollView scrollViewForBonus;
  [SerializeField]
  private UIGrid gridForBonus;
  [SerializeField]
  private GameObject dir_next;
  [SerializeField]
  private GameObject dir_ReChallenge;
  [SerializeField]
  private GameObject dir_GachaAgain;
  [SerializeField]
  private GameObject dir_next_ClickForDetail;
  [SerializeField]
  private GameObject dir_next_ClickForDetail_bonus;
  public Gacha00613Scene Scene;
  [SerializeField]
  private UIGrid resultItemGrid;
  [SerializeField]
  private UIScrollView resultItemScrollView;
  [SerializeField]
  private UIGrid resultItem100Grid;
  [SerializeField]
  private UIScrollView resultItem100ScrollView;
  [SerializeField]
  private UIGrid resultItem10Grid;
  [SerializeField]
  private UIScrollView resultItem10ScrollView;
  private GameObject UnitPrefab;
  private GameObject GearPrefab;
  private GameObject SupplyPrefab;
  private GameObject retryPopupPrefab;
  private int? remainingRetryCount;
  private DateTime? expiredAt;
  private bool isShowBonus;
  private GachaResultData.ResultData.AdditionalItem[] bonusItems;
  private GachaResultData.ResultData resultData;
  public bool isConfirmResult;
  private bool isBtnAction = true;

  public GameObject RetryPopupPrefab => this.retryPopupPrefab;

  public bool IsConfirmResult => this.isConfirmResult;

  public bool isWaitPopup { get; private set; }

  public bool IsBtnAction => this.isBtnAction;

  public void BtnActionEnable(bool enable)
  {
    this.isBtnAction = enable;
    foreach (Transform transform1 in ((Component) this.resultItemGrid).transform)
    {
      Transform transform2 = transform1.Find("button");
      if (Object.op_Inequality((Object) transform2, (Object) null))
        ((Component) transform2).gameObject.SetActive(enable);
    }
  }

  public void IbtnBack()
  {
    if (!this.isBtnAction || this.IsPushAndSet())
      return;
    if (this.dir_ReChallenge.activeSelf)
      this.showRetryPopup();
    else if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
    {
      Singleton<TutorialRoot>.GetInstance().ForceShowAdvice("newchapter_gacha2_tutorial", (Action) (() => Singleton<TutorialRoot>.GetInstance().CurrentAdvise()));
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      this.backScene();
    }
  }

  public override void onBackButton() => this.IbtnBack();

  public IEnumerator onEndSceneAsync()
  {
    foreach (Component component in ((Component) this.resultItemGrid).transform)
      Object.Destroy((Object) component.gameObject);
    ((Component) this.resultItemGrid).transform.Clear();
    this.dir_bonus.SetActive(false);
    yield break;
  }

  public IEnumerator CreateGetListAsync(GachaResultData.ResultData resultData)
  {
    Gacha00613Menu gacha00613Menu = this;
    gacha00613Menu.remainingRetryCount = resultData.remainingRetryCount;
    gacha00613Menu.expiredAt = resultData.expiredAt;
    IEnumerator e = gacha00613Menu.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (resultData.resultList.Length > 10)
    {
      gacha00613Menu.resultItemGrid = gacha00613Menu.resultItem100Grid;
      gacha00613Menu.resultItemScrollView = gacha00613Menu.resultItem100ScrollView;
      if (resultData.additionalItems.Length != 0)
        ((UIRect) ((Component) gacha00613Menu.resultItemScrollView).GetComponentInParent<UIWidget>()).bottomAnchor.target = ((Component) gacha00613Menu.dir_bonus.transform.Find("dir_Title")).gameObject.transform;
      else
        ((UIRect) ((Component) gacha00613Menu.resultItemScrollView).GetComponentInParent<UIWidget>()).bottomAnchor.target = ((Component) gacha00613Menu.dir_next.transform.Find("slc_ClickForDetail")).gameObject.transform;
    }
    else
    {
      gacha00613Menu.resultItemGrid = gacha00613Menu.resultItem10Grid;
      gacha00613Menu.resultItemScrollView = gacha00613Menu.resultItem10ScrollView;
    }
    ((UIRect) ((Component) gacha00613Menu.resultItemScrollView).GetComponentInParent<UIWidget>()).ResetAnchors();
    ((UIRect) ((Component) gacha00613Menu.resultItemScrollView).GetComponentInParent<UIWidget>()).UpdateAnchors();
    bool is_temporary = resultData.is_retry;
    bool is_ticket = resultData.is_ticket;
    int count = 0;
    GachaResultData.Result[] resultArray = resultData.GetResultData();
    for (int index = 0; index < resultArray.Length; ++index)
    {
      GachaResultData.Result result = resultArray[index];
      int id = is_temporary ? result.reward_id : result.reward_result_id;
      CommonRewardType crt = new CommonRewardType(result.reward_type_id, id, result.reward_result_quantity, result.is_new, result.is_reserves, is_temporary);
      e = crt.CreateIcon(((Component) gacha00613Menu.resultItemGrid).transform);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Gacha00613Icon hscroll = crt.GetIcon().AddComponent<Gacha00613Icon>();
      hscroll.Scene = ((Component) gacha00613Menu).gameObject.GetComponent<Gacha00613Scene>();
      hscroll.Number = count;
      if (crt.isUnit)
      {
        PlayerUnit[] playerUnits = new PlayerUnit[1]
        {
          crt.unit
        };
        e = crt.UnitIconScript.SetPlayerUnit(crt.unit, playerUnits, (PlayerUnit) null, false, false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        crt.UnitIconScript.setLevelText(crt.unit);
        crt.UnitIconScript.setBlinkUnityValue(crt.unit.unity_value, crt.unit.buildup_unity_value_f);
        crt.UnitIconScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
        gacha00613Menu.SetEvent(crt.UnitIconScript, (MonoBehaviour) hscroll);
      }
      if (crt.isMaterialUnit)
      {
        PlayerUnit unit = PlayerUnit.CreateByPlayerMaterialUnit(crt.materialUnit);
        PlayerUnit[] playerUnits = new PlayerUnit[1]{ unit };
        e = crt.UnitIconScript.SetPlayerUnit(unit, playerUnits, (PlayerUnit) null, false, false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        crt.UnitIconScript.setLevelText(unit);
        crt.UnitIconScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
        gacha00613Menu.SetEvent(crt.UnitIconScript, (MonoBehaviour) hscroll);
        if (is_temporary | is_ticket && crt.quantity_ > 1)
          crt.UnitIconScript.SetCounter(crt.quantity_, true);
        unit = (PlayerUnit) null;
      }
      if (crt.isGear)
      {
        e = crt.ItemIconScript.InitByPlayerItem(crt.gear);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        gacha00613Menu.SetEvent(crt.ItemIconScript, (MonoBehaviour) hscroll);
      }
      if (crt.isMaterialGear)
      {
        e = crt.ItemIconScript.InitByPlayerMaterialGear(crt.materialGear);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        gacha00613Menu.SetEvent(crt.ItemIconScript, (MonoBehaviour) hscroll);
        crt.ItemIconScript.EnableQuantity(0);
        if (is_ticket && crt.quantity_ > 1)
          crt.ItemIconScript.EnableQuantityBonus(crt.quantity_);
      }
      ++count;
      crt = (CommonRewardType) null;
      hscroll = (Gacha00613Icon) null;
    }
    resultArray = (GachaResultData.Result[]) null;
    gacha00613Menu.resultItemGrid.Reposition();
    gacha00613Menu.resultItemScrollView.ResetPosition();
    if (resultData.is_retry && Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
    {
      bool flag = gacha00613Menu.isConfirmResult && gacha00613Menu.bonusItems.Length != 0;
      gacha00613Menu.dir_bonus.SetActive(flag);
      gacha00613Menu.dir_next_ClickForDetail.SetActive(!flag);
      gacha00613Menu.dir_next_ClickForDetail_bonus.SetActive(flag);
      gacha00613Menu.dir_next.SetActive(false);
    }
    else
    {
      bool flag = resultData.GetAdditionalData().Length != 0;
      gacha00613Menu.dir_bonus.SetActive(flag);
      gacha00613Menu.dir_next_ClickForDetail.SetActive(!flag);
      gacha00613Menu.dir_next_ClickForDetail_bonus.SetActive(flag);
      gacha00613Menu.dir_next.SetActive(false);
      if (((Component) gacha00613Menu.gridForBonus).transform.childCount <= 0)
      {
        gacha00613Menu.bonusItems = resultData.GetAdditionalData();
        e = gacha00613Menu.SetBonus();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  public void SetEvent(UnitIcon UI, MonoBehaviour target)
  {
    UI.Button.onLongPress.Clear();
    UI.Button.onLongPress.Add(new EventDelegate(target, "IbtnIcon"));
    UI.Button.onClick.Clear();
    UI.Button.onClick.Add(new EventDelegate(target, "IbtnIcon"));
  }

  public void SetEvent(ItemIcon II, MonoBehaviour target)
  {
    II.gear.button.onLongPress.Clear();
    II.gear.button.onLongPress.Add(new EventDelegate(target, "IbtnIcon"));
    II.gear.button.onClick.Clear();
    II.gear.button.onClick.Add(new EventDelegate(target, "IbtnIcon"));
  }

  private IEnumerator ResourceLoad()
  {
    Future<GameObject> prefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.UnitPrefab, (Object) null))
    {
      prefabF = Singleton<NGGameDataManager>.GetInstance().IsSea ? Res.Prefabs.Sea.UnitIcon.normal_sea.Load<GameObject>() : Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.UnitPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.GearPrefab, (Object) null))
    {
      prefabF = Singleton<NGGameDataManager>.GetInstance().IsSea ? new ResourceObject("Prefabs/Sea/ItemIcon/prefab_sea").Load<GameObject>() : Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.GearPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.SupplyPrefab, (Object) null))
    {
      prefabF = Singleton<NGGameDataManager>.GetInstance().IsSea ? new ResourceObject("Prefabs/Sea/ItemIcon/prefab_sea").Load<GameObject>() : Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.SupplyPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.retryPopupPrefab, (Object) null))
    {
      prefabF = new ResourceObject("Prefabs/popup/popup_006_redrawn_gacha_confirm__anim_popup01").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.retryPopupPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  private IEnumerator SetBonus()
  {
    Gacha00613Menu gacha00613Menu = this;
    yield return (object) null;
    GameObject unknownUnit = gacha00613Menu.CreateUnknownUnit();
    GameObject unknownItem = gacha00613Menu.CreateUnknownItem();
    unknownUnit.SetActive(false);
    unknownItem.SetActive(false);
    GachaResultData.ResultData.AdditionalItem[] additionalItemArray = gacha00613Menu.bonusItems;
    for (int index = 0; index < additionalItemArray.Length; ++index)
    {
      GachaResultData.ResultData.AdditionalItem bonus_result = additionalItemArray[index];
      GameObject go = new GameObject("bonus");
      CreateIconObject icon = go.AddComponent<CreateIconObject>();
      go.transform.parent = ((Component) gacha00613Menu.gridForBonus).transform;
      IEnumerator e = icon.CreateThumbnail((MasterDataTable.CommonRewardType) bonus_result.reward_type_id, bonus_result.reward_result_id, bonus_result.reward_result_quantity, isButton: false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      go.transform.localScale = Vector3.one;
      switch ((MasterDataTable.CommonRewardType) bonus_result.reward_type_id)
      {
        case MasterDataTable.CommonRewardType.unit:
        case MasterDataTable.CommonRewardType.material_unit:
          bonus_result.unknownObject = unknownUnit.Clone(go.transform);
          bonus_result.gameObject = icon.GetIcon();
          UnitIcon component1 = icon.GetIcon().GetComponent<UnitIcon>();
          if (Object.op_Inequality((Object) component1, (Object) null))
          {
            component1.BottomModeValue = UnitIconBase.BottomMode.Level;
            component1.setLevelText("1");
          }
          Vector3 position1 = bonus_result.unknownObject.transform.position;
          bonus_result.unknownObject.transform.position = new Vector3(position1.x, position1.y + 0.01f, position1.z);
          Vector3 position2 = bonus_result.gameObject.transform.position;
          bonus_result.gameObject.transform.position = new Vector3(position2.x, position2.y + 0.01f, position2.z);
          break;
        default:
          bonus_result.unknownObject = unknownItem.Clone(go.transform);
          bonus_result.gameObject = icon.GetIcon();
          break;
      }
      UIWidget component2 = bonus_result.gameObject.GetComponent<UIWidget>();
      if (Object.op_Inequality((Object) component2, (Object) null))
        ((UIRect) component2).alpha = 0.0f;
      bonus_result.unknownObject.SetActive(true);
      bonus_result.gameObject.SetActive(false);
      go = (GameObject) null;
      icon = (CreateIconObject) null;
      bonus_result = (GachaResultData.ResultData.AdditionalItem) null;
    }
    additionalItemArray = (GachaResultData.ResultData.AdditionalItem[]) null;
    // ISSUE: method pointer
    gacha00613Menu.gridForBonus.onReposition = new UIGrid.OnReposition((object) gacha00613Menu, __methodptr(\u003CSetBonus\u003Eb__48_0));
    gacha00613Menu.gridForBonus.Reposition();
    Object.Destroy((Object) unknownUnit);
    Object.Destroy((Object) unknownItem);
  }

  private GameObject CreateUnknownUnit(Transform parent = null)
  {
    GameObject unknownUnit = this.UnitPrefab.Clone(parent);
    UnitIcon component = unknownUnit.GetComponent<UnitIcon>();
    component.BottomModeValue = UnitIconBase.BottomMode.Nothing;
    component.BackgroundModeValue = UnitIcon.BackgroundMode.PlayerShadow;
    component.Unknown = true;
    component.NewUnit = false;
    ((Component) ((Component) component).transform.Find("icon")).gameObject.SetActive(false);
    return unknownUnit;
  }

  private GameObject CreateUnknownItem(Transform parent = null)
  {
    GameObject unknownItem = this.GearPrefab.Clone(parent);
    ItemIcon component = unknownItem.GetComponent<ItemIcon>();
    component.SetEmpty(true);
    component.gear.unknown.SetActive(true);
    component.BottomModeValue = ItemIcon.BottomMode.Nothing;
    return unknownItem;
  }

  public IEnumerator OpenBonusIcon()
  {
    if (this.bonusItems != null && this.bonusItems.Length != 0 && !this.isShowBonus)
    {
      this.BtnActionEnable(false);
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
      bool isSkip = false;
      GameObject touchObj = this.CreateTouchObject((EventDelegate.Callback) (() => isSkip = true), ((Component) this.scrollViewForBonus).transform);
      GameObject touchObj2 = this.CreateTouchObject((EventDelegate.Callback) (() => isSkip = true));
      yield return (object) new WaitForSeconds(1f);
      for (int i = 0; i < this.bonusItems.Length; ++i)
      {
        GachaResultData.ResultData.AdditionalItem bonusItem = this.bonusItems[i];
        TweenAlpha tweenAlpha1 = bonusItem.unknownObject.AddComponent<TweenAlpha>();
        tweenAlpha1.to = 0.0f;
        tweenAlpha1.from = 1f;
        ((UITweener) tweenAlpha1).duration = 0.5f;
        bonusItem.gameObject.SetActive(true);
        TweenAlpha tweenAlpha2 = bonusItem.gameObject.AddComponent<TweenAlpha>();
        tweenAlpha2.to = 1f;
        tweenAlpha2.from = 0.0f;
        ((UITweener) tweenAlpha2).duration = 0.5f;
        if (!isSkip)
        {
          Singleton<NGSoundManager>.GetInstance().playSE("SE_1021", seChannel: i % 3);
          yield return (object) new WaitForSeconds(0.5f);
        }
      }
      Object.Destroy((Object) touchObj);
      Object.Destroy((Object) touchObj2);
      this.isShowBonus = true;
    }
  }

  private GameObject CreateTouchObject(EventDelegate.Callback callback, Transform parent = null)
  {
    Resolution currentResolution = Screen.currentResolution;
    GameObject touchObject = new GameObject("touch object");
    touchObject.transform.parent = parent ?? ((Component) this).transform;
    UIWidget uiWidget = touchObject.AddComponent<UIWidget>();
    uiWidget.depth = 1000;
    uiWidget.width = ((Resolution) ref currentResolution).height;
    uiWidget.height = ((Resolution) ref currentResolution).width;
    BoxCollider boxCollider = touchObject.AddComponent<BoxCollider>();
    ((Collider) boxCollider).isTrigger = true;
    boxCollider.size = new Vector3()
    {
      x = (float) ((Resolution) ref currentResolution).height,
      y = (float) ((Resolution) ref currentResolution).width,
      z = 1f
    };
    UIButton uiButton = touchObject.AddComponent<UIButton>();
    ((UIButtonColor) uiButton).tweenTarget = (GameObject) null;
    EventDelegate.Add(uiButton.onClick, callback);
    return touchObject;
  }

  private void showRetryPopup()
  {
    this.isWaitPopup = true;
    this.StartCoroutine(this.doShowRetryPopup());
  }

  private IEnumerator doShowRetryPopup()
  {
    Gacha00613Menu menu = this;
    menu.dir_next.SetActive(false);
    menu.dir_ReChallenge.SetActive(false);
    GameObject popup = menu.retryPopupPrefab.Clone();
    PopupRedrawnGachaConfirm script = popup.GetComponent<PopupRedrawnGachaConfirm>();
    popup.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = script.Initialize(menu, new Action(menu.\u003CdoShowRetryPopup\u003Eb__54_0), new Action(menu.\u003CdoShowRetryPopup\u003Eb__54_1), new Action(menu.\u003CdoShowRetryPopup\u003Eb__54_2), menu.remainingRetryCount, menu.expiredAt);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    menu.StartCoroutine(script.UpdateRemainTime());
  }

  private IEnumerator DecideGacha()
  {
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    int? remainingRetryCount = this.remainingRetryCount;
    int num = 0;
    if (remainingRetryCount.GetValueOrDefault() == num & remainingRetryCount.HasValue)
    {
      this.isConfirmResult = true;
    }
    else
    {
      if (this.expiredAt.HasValue)
      {
        DateTime? expiredAt = this.expiredAt;
        DateTime dateTime = ServerTime.NowAppTimeAddDelta();
        if ((expiredAt.HasValue ? (expiredAt.GetValueOrDefault() < dateTime ? 1 : 0) : 0) != 0)
        {
          this.isConfirmResult = true;
          goto label_13;
        }
      }
      bool popup_yes_no_select = false;
      PopupCommonYesNo.Show(Consts.GetInstance().GACHA_DECIDE_CONFIRM_TITLE, Consts.GetInstance().GACHA_DECIDE_CONFIRM_DESCRIPTION, (Action) (() =>
      {
        popup_yes_no_select = true;
        this.isConfirmResult = true;
      }), (Action) (() =>
      {
        popup_yes_no_select = true;
        this.isConfirmResult = false;
      }));
      while (!popup_yes_no_select)
        yield return (object) null;
    }
label_13:
    if (!this.isConfirmResult)
    {
      this.showRetryPopup();
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      this.isWaitPopup = false;
      GachaResultData.ResultData data = GachaResultData.GetInstance().GetData();
      int[] array = ((IEnumerable<PlayerUnit>) data.playerUnitReserves).Select<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x._unit)).ToArray<int>();
      bool is_error = false;
      Future<WebAPI.Response.GachaDecide> paramF = WebAPI.GachaDecide(array, data.gachaId, data.gachaName, (Action<WebAPI.Response.UserError>) (error =>
      {
        is_error = true;
        this.StartCoroutine(PopupCommon.Show(error.Code, error.Reason));
      }));
      IEnumerator e = paramF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (!is_error)
      {
        WebAPI.Response.GachaDecide result = paramF.Result;
        if (result != null)
        {
          List<GachaResultData.ResultData.AdditionalItem> additionalItemList = new List<GachaResultData.ResultData.AdditionalItem>();
          WebAPI.Response.GachaDecideAdditional_items[] additionalItems = result.additional_items;
          for (int index = 0; index < additionalItems.Length; ++index)
            additionalItemList.Add(new GachaResultData.ResultData.AdditionalItem()
            {
              reward_result_id = additionalItems[index].reward_id,
              reward_type_id = additionalItems[index].reward_type_id,
              reward_result_quantity = additionalItems[index].reward_quantity
            });
          this.bonusItems = additionalItemList.ToArray();
          GachaResultData.GetInstance().GetData().unlockQuests = result.unlock_quests;
          GachaResultData.GetInstance().GetData().playerCommonTicket = result.player_common_tickets;
        }
        if (this.bonusItems.Length != 0)
        {
          UIWidget bonusUiWidget = this.scrollContainerForBonus.GetComponent<UIWidget>();
          ((UIRect) bonusUiWidget).alpha = 0.0f;
          this.dir_bonus.SetActive(true);
          this.dir_next_ClickForDetail.SetActive(false);
          this.dir_next_ClickForDetail_bonus.SetActive(true);
          e = this.SetBonus();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          yield return (object) new WaitForEndOfFrame();
          ((UIRect) bonusUiWidget).alpha = 1f;
          bonusUiWidget = (UIWidget) null;
        }
        this.dir_next.SetActive(true);
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        e = this.Scene.ResultEffect();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private IEnumerator RetryGacha()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0, true);
    this.isWaitPopup = false;
    ((Component) Singleton<CommonRoot>.GetInstance().GetNormalHeaderComponent()).GetComponentInChildren<CommonHeaderExp>().OnPress(false);
    yield return (object) new WaitForSeconds(0.1f);
    GachaPlay gacha = GachaPlay.GetInstance();
    GachaResultData.ResultData data = GachaResultData.GetInstance().GetData();
    IEnumerator e = gacha.ChargeGacha(data.gachaName, data.rollCount, data.gachaId, data.gachaType, data.paymentAmount);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!gacha.isError)
    {
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
      Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_effect", false);
    }
  }

  public void onConfirmRetry()
  {
    if (this.IsPushAndSet())
      return;
    this.showRetryPopup();
  }

  public void SetRetryBtnActive(bool active)
  {
    this.dir_next.SetActive(!active);
    this.dir_ReChallenge.SetActive(active);
  }

  public void UpdateButtonStatus(GachaResultData.ResultData resultData)
  {
    this.resultData = resultData;
    if (resultData.gachaTicketData != null)
    {
      bool flag = this.CanPlayGachaAgain(resultData);
      this.dir_GachaAgain.SetActive(flag);
      this.dir_next.SetActive(!flag);
    }
    else
    {
      this.dir_GachaAgain.SetActive(false);
      this.dir_next.SetActive(true);
    }
  }

  private bool CanPlayGachaAgain(GachaResultData.ResultData resultData)
  {
    bool flag = false;
    PlayerGachaTicket playerGachaTicket = ((IEnumerable<PlayerGachaTicket>) SMManager.Get<Player>().gacha_tickets).FirstOrDefault<PlayerGachaTicket>((Func<PlayerGachaTicket, bool>) (x => x.ticket_id == resultData.gachaTicketData.gachaData.payment_id.Value));
    if (playerGachaTicket != null && playerGachaTicket.quantity >= resultData.gachaTicketData.gachaData.payment_amount)
      flag = true;
    return flag;
  }

  public void OnClickGachaAgain()
  {
    if (this.IsPushAndSet() || !this.TicketGachaCheckUnit() || !this.GachaCheckItem())
      return;
    this.InitPlayGeneralGachaTicket(this.resultData.gachaTicketData);
  }

  private bool TicketGachaCheckUnit()
  {
    if (!SMManager.Get<Player>().CheckCapMaxUnit())
      return true;
    this.StartCoroutine(PopupUtility._999_5_1(bStackScene: false));
    return false;
  }

  private bool GachaCheckItem()
  {
    if (!SMManager.Get<Player>().CheckMaxHavingGear())
      return true;
    this.StartCoroutine(PopupUtility._999_6_1(true, false));
    return false;
  }

  private void InitPlayGeneralGachaTicket(
    GachaResultData.ResultData.GachaTicketData gachaTicketData)
  {
    Popup006SliderSelectMenu menuPopup = Singleton<PopupManager>.GetInstance().open(gachaTicketData.popupPrefab).GetComponent<Popup006SliderSelectMenu>();
    menuPopup.Init(gachaTicketData.gachaData, (Action) (() => this.StartCoroutine(this.PlayTicket(menuPopup.currentPlayTime, gachaTicketData))));
  }

  private IEnumerator PlayTicket(
    int num,
    GachaResultData.ResultData.GachaTicketData gachaTicketData)
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    Singleton<CommonRoot>.GetInstance().loadingMode = 4;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    GachaPlay gacha = GachaPlay.GetInstance();
    IEnumerator e = gacha.TicketGacha(gachaTicketData.gachaName, num, gachaTicketData.gachaData, gachaTicketData.popupPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!gacha.isError)
      Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_effect", false);
  }
}
