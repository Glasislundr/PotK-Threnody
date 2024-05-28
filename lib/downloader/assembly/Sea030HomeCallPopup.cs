// Decompiled with JetBrains decompiler
// Type: Sea030HomeCallPopup
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
public class Sea030HomeCallPopup : BackButtonMonoBehaiviour
{
  [SerializeField]
  private NGxScroll2 scroll;
  [SerializeField]
  private int iconWidth = 114;
  [SerializeField]
  private int iconHeight = 136;
  [SerializeField]
  private int iconColumnValue = 5;
  [SerializeField]
  private int iconRowValue = 8;
  [SerializeField]
  private int iconScreenValue = 25;
  [SerializeField]
  private int iconMaxValue = 40;
  public UISprite titleLabel;
  private const string defaultTitle = "slc_txt_Who_calls_ibtn.png__GUI__sea_home__sea_home_prefab";
  private const string madogiTitle = "slc_txt_madomagi_Who_calls_ibtn.png__GUI__sea_home__sea_home_prefab";
  private bool isInitialize;
  private bool isUpdateIcon;
  private float scrool_start_y;
  private PlayerUnit[] playerUnitList;
  private List<UnitIconBase> allUnitIcons = new List<UnitIconBase>();
  private List<Sea030HomeCallPopup.UnitInfo> displayUnitInfos = new List<Sea030HomeCallPopup.UnitInfo>();
  private Action<SeaHomeManager.UnitConrtolleData> clickCallback;
  private Action backCallback;
  private bool isShow;

  public bool IsShow => this.isShow;

  public bool IsInitialize => this.isInitialize;

  public IEnumerator UpdateUnitIcon()
  {
    int[] playerUnitIds = ((IEnumerable<PlayerUnit>) this.playerUnitList).Select<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id)).ToArray<int>();
    Dictionary<int, PlayerUnit> playerUnitDict = new Dictionary<int, PlayerUnit>();
    foreach (PlayerUnit playerUnit in ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => ((IEnumerable<int>) playerUnitIds).Contains<int>(x.id))))
      playerUnitDict[playerUnit.id] = playerUnit;
    for (int index = 0; index < this.playerUnitList.Length; ++index)
      this.playerUnitList[index] = playerUnitDict[this.playerUnitList[index].id];
    for (int i = 0; i < this.allUnitIcons.Count; ++i)
    {
      UnitIcon unitIcon = (UnitIcon) this.allUnitIcons[i];
      if (unitIcon.PlayerUnit != (PlayerUnit) null)
      {
        IEnumerator e = unitIcon.SetPlayerUnit(playerUnitDict[unitIcon.PlayerUnit.id], this.playerUnitList, (PlayerUnit) null, false, false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Trust);
        unitIcon.BottomModeValue = UnitIconBase.BottomMode.TrustValue;
        ((Component) unitIcon.RarityStar).gameObject.SetActive(false);
      }
      else
        unitIcon.ShowBottomInfos(UnitSortAndFilter.SORT_TYPES.Trust);
      unitIcon = (UnitIcon) null;
    }
  }

  public void UpdateUnitData()
  {
    int[] playerUnitIds = ((IEnumerable<PlayerUnit>) this.playerUnitList).Select<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id)).ToArray<int>();
    Dictionary<int, PlayerUnit> dictionary = new Dictionary<int, PlayerUnit>();
    foreach (PlayerUnit playerUnit in ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => ((IEnumerable<int>) playerUnitIds).Contains<int>(x.id))))
      dictionary[playerUnit.id] = playerUnit;
    for (int index = 0; index < this.playerUnitList.Length; ++index)
      this.playerUnitList[index] = dictionary[this.playerUnitList[index].id];
  }

  public bool VerifyUnitLength()
  {
    int[] playerUnitIds = ((IEnumerable<PlayerUnit>) this.playerUnitList).Select<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id)).ToArray<int>();
    PlayerUnit[] array = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => ((IEnumerable<int>) playerUnitIds).Contains<int>(x.id))).ToArray<PlayerUnit>();
    return playerUnitIds.Length == array.Length;
  }

  public IEnumerator Init(
    SeaHomeManager.UnitConrtolleData[] unitDatas,
    Action<SeaHomeManager.UnitConrtolleData> callback,
    Action backButton,
    bool isShowGuestDebug = false)
  {
    this.isShow = false;
    this.clickCallback = callback;
    this.backCallback = backButton;
    this.scrool_start_y = ((Component) this.scroll.scrollView).transform.localPosition.y;
    Future<GameObject> futureUnitIcon = Res.Prefabs.Sea.UnitIcon.normal_sea.Load<GameObject>();
    IEnumerator e = futureUnitIcon.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = futureUnitIcon.Result;
    this.displayUnitInfos = ((IEnumerable<SeaHomeManager.UnitConrtolleData>) unitDatas).OrderBy<SeaHomeManager.UnitConrtolleData, int>((Func<SeaHomeManager.UnitConrtolleData, int>) (x => !(x.PlayerUnit == (PlayerUnit) null) ? 1 : 0)).Select<SeaHomeManager.UnitConrtolleData, Sea030HomeCallPopup.UnitInfo>((Func<SeaHomeManager.UnitConrtolleData, Sea030HomeCallPopup.UnitInfo>) (x => new Sea030HomeCallPopup.UnitInfo(x, x.PlayerUnit != (PlayerUnit) null))).ToList<Sea030HomeCallPopup.UnitInfo>();
    foreach (IEnumerable<PlayerUnit> source in (IEnumerable<IGrouping<int, PlayerUnit>>) ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsSea && !((IEnumerable<SeaHomeManager.UnitConrtolleData>) unitDatas).Any<SeaHomeManager.UnitConrtolleData>((Func<SeaHomeManager.UnitConrtolleData, bool>) (y => y.PlayerUnit == x)))).GroupBy<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.unit.same_character_id)).OrderByDescending<IGrouping<int, PlayerUnit>, int>((Func<IGrouping<int, PlayerUnit>, int>) (x => !x.Any<PlayerUnit>((Func<PlayerUnit, bool>) (y => y.favorite)) ? 0 : 1)).ThenByDescending<IGrouping<int, PlayerUnit>, float>((Func<IGrouping<int, PlayerUnit>, float>) (x => x.Max<PlayerUnit>((Func<PlayerUnit, float>) (y => y.trust_rate)))).ThenBy<IGrouping<int, PlayerUnit>, int>((Func<IGrouping<int, PlayerUnit>, int>) (x => x.Key)))
    {
      foreach (PlayerUnit playerUnit in (IEnumerable<PlayerUnit>) source.OrderByDescending<PlayerUnit, int>((Func<PlayerUnit, int>) (x => !x.favorite ? 0 : 1)).ThenByDescending<PlayerUnit, float>((Func<PlayerUnit, float>) (x => x.trust_rate)).ThenByDescending<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.unit.rarity.index)).ThenBy<PlayerUnit, DateTime>((Func<PlayerUnit, DateTime>) (x => x.created_at)))
        this.displayUnitInfos.Add(new Sea030HomeCallPopup.UnitInfo(new SeaHomeManager.UnitConrtolleData(playerUnit.unit.ID, playerUnit), false));
    }
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
    foreach (Sea030HomeCallPopup.UnitInfo displayUnitInfo in this.displayUnitInfos)
    {
      if (!(displayUnitInfo.unitData.PlayerUnit == (PlayerUnit) null))
        playerUnitList.Add(displayUnitInfo.unitData.PlayerUnit);
    }
    this.playerUnitList = playerUnitList.ToArray();
    if (Object.op_Inequality((Object) result, (Object) null))
    {
      for (int count = this.allUnitIcons.Count; count < Mathf.Min(this.iconMaxValue, this.displayUnitInfos.Count); ++count)
      {
        GameObject gameObject = Object.Instantiate<GameObject>(result);
        UnitIconBase component = gameObject.GetComponent<UnitIconBase>();
        component.unit = this.displayUnitInfos[count].unitData.Unit;
        this.displayUnitInfos[count].icon = gameObject;
        this.allUnitIcons.Add(component);
      }
    }
    for (int index = 0; index < Mathf.Min(this.iconMaxValue, this.displayUnitInfos.Count); ++index)
      this.scroll.Add(((Component) this.allUnitIcons[index]).gameObject, this.iconWidth, this.iconHeight);
    for (int index = 0; index < Mathf.Min(this.iconMaxValue, this.displayUnitInfos.Count); ++index)
      this.ResetUnitIcon(index);
    for (int i = 0; i < Mathf.Min(this.iconMaxValue, this.displayUnitInfos.Count); ++i)
    {
      e = this.CreateUnitIcon(i, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.scroll.CreateScrollPoint(this.iconHeight, this.displayUnitInfos.Count);
    this.scroll.ResolvePosition();
    this.titleLabel.spriteName = "slc_txt_Who_calls_ibtn.png__GUI__sea_home__sea_home_prefab";
    for (int index = 0; index < this.displayUnitInfos.Count; ++index)
    {
      if (this.IsMadogiUnit(this.displayUnitInfos[index].unitData.UnitID))
      {
        this.titleLabel.spriteName = "slc_txt_madomagi_Who_calls_ibtn.png__GUI__sea_home__sea_home_prefab";
        break;
      }
    }
    this.isInitialize = true;
  }

  private bool IsMadogiUnit(int UnitID)
  {
    foreach (KeyValuePair<int, UnitComponent> keyValuePair in MasterData.UnitComponent)
    {
      if (keyValuePair.Value.UnitID == UnitID)
        return true;
    }
    return false;
  }

  private List<SeaHomeManager.UnitConrtolleData> SetDebugGuestUnit()
  {
    List<SeaHomeManager.UnitConrtolleData> source = new List<SeaHomeManager.UnitConrtolleData>();
    foreach (UnitUnit unitUnit in ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).ToList<UnitUnit>())
    {
      UnitUnit target = unitUnit;
      if (!source.Any<SeaHomeManager.UnitConrtolleData>((Func<SeaHomeManager.UnitConrtolleData, bool>) (x => x.Unit.character == target.character)) && target.IsSea && target.character.category != UnitCategory.enemy)
      {
        source.Add(new SeaHomeManager.UnitConrtolleData(target.ID, (PlayerUnit) null));
        UnitCharacter character = target.character;
      }
    }
    return source;
  }

  public void StartLoadThum() => this.StartCoroutine(this.LoadObjectSprite());

  public override void onBackButton()
  {
    this.backCallback();
    this.Hide();
  }

  public void Show()
  {
    ((Component) this).gameObject.SetActive(true);
    UITweener component = ((Component) this).gameObject.GetComponent<UITweener>();
    if (Object.op_Inequality((Object) component, (Object) null))
    {
      component.ResetToBeginning();
      component.PlayForward();
    }
    this.isShow = true;
  }

  public void ScrollReset() => this.scroll.scrollView.ResetPosition();

  public void Hide()
  {
    this.isShow = false;
    UITweener component = ((Component) this).gameObject.GetComponent<UITweener>();
    if (Object.op_Inequality((Object) component, (Object) null))
      component.PlayReverse();
    ((Component) this).gameObject.SetActive(false);
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  private void ResetUnitIcon(int index)
  {
    if (this.allUnitIcons == null || this.allUnitIcons.Count == 0)
      return;
    UnitIconBase unitIcon = this.allUnitIcons[index];
    ((UnitIcon) unitIcon).ResetUnit();
    ((Component) unitIcon).gameObject.SetActive(false);
    this.displayUnitInfos.Where<Sea030HomeCallPopup.UnitInfo>((Func<Sea030HomeCallPopup.UnitInfo, bool>) (a => Object.op_Equality((Object) a.icon, (Object) ((Component) unitIcon).gameObject))).ForEach<Sea030HomeCallPopup.UnitInfo>((Action<Sea030HomeCallPopup.UnitInfo>) (b => b.icon = (GameObject) null));
  }

  private void ScrollIconUpdate(int info_index, int unit_index)
  {
    this.ResetUnitIcon(unit_index);
    if (UnitIcon.IsCache(this.displayUnitInfos[unit_index].unitData.Unit))
      this.CreateUnitIconCache(info_index, unit_index);
    else
      this.StartCoroutine(this.CreateUnitIcon(info_index, unit_index));
  }

  private void ScrollUpdate()
  {
    if ((!this.isInitialize || this.displayUnitInfos.Count <= this.iconScreenValue) && !this.isUpdateIcon)
      return;
    int num1 = this.iconHeight * 2;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.displayUnitInfos.Count - this.iconScreenValue - 1) / this.iconColumnValue * this.iconHeight);
    float num4 = (float) (this.iconHeight * this.iconRowValue);
    if ((double) num2 < 0.0)
      num2 = 0.0f;
    if ((double) num2 > (double) num3)
      num2 = num3;
    while (true)
    {
      do
      {
        bool flag = false;
        int unit_index = 0;
        foreach (GameObject gameObject in this.scroll)
        {
          GameObject unit = gameObject;
          float num5 = unit.transform.localPosition.y + num2;
          if ((double) num5 > (double) num1)
          {
            int? nullable = this.displayUnitInfos.FirstIndexOrNull<Sea030HomeCallPopup.UnitInfo>((Func<Sea030HomeCallPopup.UnitInfo, bool>) (x => Object.op_Equality((Object) x.icon, (Object) unit)));
            int info_index = nullable.HasValue ? nullable.Value + this.iconMaxValue : (this.displayUnitInfos.Count + 4) / 5 * 5;
            if (nullable.HasValue && info_index < (this.displayUnitInfos.Count + 4) / 5 * 5)
            {
              unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y - num4, 0.0f);
              if (info_index >= this.displayUnitInfos.Count)
                unit.SetActive(false);
              else
                this.ScrollIconUpdate(info_index, unit_index);
              flag = true;
            }
          }
          else if ((double) num5 < -((double) num4 - (double) num1))
          {
            int num6 = this.iconMaxValue;
            if (!unit.activeSelf)
            {
              unit.SetActive(true);
              num6 = 0;
            }
            int? nullable = this.displayUnitInfos.FirstIndexOrNull<Sea030HomeCallPopup.UnitInfo>((Func<Sea030HomeCallPopup.UnitInfo, bool>) (x => Object.op_Equality((Object) x.icon, (Object) unit)));
            int info_index = nullable.HasValue ? nullable.Value - num6 : -1;
            if (nullable.HasValue && info_index >= 0)
            {
              unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y + num4, 0.0f);
              this.ScrollIconUpdate(info_index, unit_index);
              flag = true;
            }
          }
          ++unit_index;
        }
        if (!flag)
          goto label_27;
      }
      while (!this.isUpdateIcon);
      this.isUpdateIcon = false;
    }
label_27:;
  }

  private IEnumerator CreateUnitIcon(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    UnitIconBase unitIcon = this.allUnitIcons[unit_index];
    this.displayUnitInfos.Where<Sea030HomeCallPopup.UnitInfo>((Func<Sea030HomeCallPopup.UnitInfo, bool>) (a => Object.op_Equality((Object) a.icon, (Object) unitIcon))).ForEach<Sea030HomeCallPopup.UnitInfo>((Action<Sea030HomeCallPopup.UnitInfo>) (b => b.icon = (GameObject) null));
    unitIcon.SetCounter(0);
    this.displayUnitInfos[info_index].icon = ((Component) unitIcon).gameObject;
    UnitIcon uIcon = (UnitIcon) unitIcon;
    IEnumerator e;
    if (this.displayUnitInfos[info_index].unitData.PlayerUnit == (PlayerUnit) null)
    {
      UnitUnit unit = this.displayUnitInfos[info_index].unitData.Unit;
      e = unitIcon.SetUnit(unit, unit.GetElement());
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      uIcon.ShowBottomInfos(UnitSortAndFilter.SORT_TYPES.Trust);
      uIcon.setTrustText();
      uIcon.princessType.DispPrincessType(false);
      uIcon.Favorite = false;
      uIcon.SetSeaGuest();
      uIcon.ResetPlayerUnit();
    }
    else
    {
      PlayerUnit playerUnit = this.displayUnitInfos[info_index].unitData.PlayerUnit;
      e = unitIcon.SetPlayerUnit(playerUnit, new PlayerUnit[1]
      {
        playerUnit
      }, baseUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      uIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Trust);
      uIcon.BottomModeValue = UnitIconBase.BottomMode.TrustValue;
      uIcon.princessType.DispPrincessType(true);
      uIcon.Favorite = playerUnit.favorite;
      if (this.displayUnitInfos[info_index].pickup)
        uIcon.SetSeaPickup();
      else
        uIcon.HideSeaIcon();
      playerUnit = (PlayerUnit) null;
    }
    unitIcon.Button.onLongPress.Clear();
    unitIcon.Button.onClick.Clear();
    unitIcon.onClick = (Action<UnitIconBase>) (_ =>
    {
      this.clickCallback(this.displayUnitInfos[info_index].unitData);
      this.Hide();
    });
    EventDelegate.Set(unitIcon.Button.onLongPress, (EventDelegate.Callback) (() =>
    {
      PlayerUnit playerUnit = this.displayUnitInfos[info_index].unitData.PlayerUnit;
      if (playerUnit == (PlayerUnit) null)
        this.StartCoroutine(PopupCommon.Show(Consts.GetInstance().POPUP_030_SEA_MYPAGE_CALL_TITLE, Consts.GetInstance().POPUP_030_SEA_MYPAGE_CALL_MESSAGE));
      else
        Unit0042Scene.changeScene(true, playerUnit, this.playerUnitList);
    }));
    ((Component) unitIcon.RarityStar).gameObject.SetActive(false);
    unitIcon.ForBattle = false;
    unitIcon.CanAwake = false;
    unitIcon.Equip = false;
    uIcon.SpecialIconType = -1;
    uIcon.SpecialIcon = false;
    unitIcon.SetCounter(0);
    unitIcon.SetSelectionCounter(0);
    unitIcon.SelectMarker = false;
    unitIcon.Deselect();
    ((Component) unitIcon).gameObject.SetActive(true);
  }

  private void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    UnitIconBase unitIcon = this.allUnitIcons[unit_index];
    this.displayUnitInfos.Where<Sea030HomeCallPopup.UnitInfo>((Func<Sea030HomeCallPopup.UnitInfo, bool>) (a => Object.op_Equality((Object) a.icon, (Object) unitIcon))).ForEach<Sea030HomeCallPopup.UnitInfo>((Action<Sea030HomeCallPopup.UnitInfo>) (b => b.icon = (GameObject) null));
    unitIcon.SetCounter(0);
    this.displayUnitInfos[info_index].icon = ((Component) unitIcon).gameObject;
    UnitIcon unitIcon1 = (UnitIcon) unitIcon;
    if (this.displayUnitInfos[info_index].unitData.PlayerUnit == (PlayerUnit) null)
    {
      UnitUnit unit = this.displayUnitInfos[info_index].unitData.Unit;
      unitIcon1.SetUnitCache(unit, unit.GetElement());
      unitIcon1.ShowBottomInfos(UnitSortAndFilter.SORT_TYPES.Trust);
      unitIcon1.setTrustText();
      unitIcon1.princessType.DispPrincessType(false);
      unitIcon1.Favorite = false;
      unitIcon1.SetSeaGuest();
      unitIcon1.ResetPlayerUnit();
    }
    else
    {
      PlayerUnit playerUnit = this.displayUnitInfos[info_index].unitData.PlayerUnit;
      unitIcon1.SetPlayerUnitCache(playerUnit, new PlayerUnit[1]
      {
        playerUnit
      });
      unitIcon1.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Trust);
      unitIcon1.princessType.DispPrincessType(true);
      unitIcon1.Favorite = playerUnit.favorite;
      if (this.displayUnitInfos[info_index].pickup)
        unitIcon1.SetSeaPickup();
      else
        unitIcon1.HideSeaIcon();
    }
    ((Component) unitIcon.RarityStar).gameObject.SetActive(false);
    unitIcon.Button.onLongPress.Clear();
    unitIcon.Button.onClick.Clear();
    unitIcon.onClick = (Action<UnitIconBase>) (_ =>
    {
      this.clickCallback(this.displayUnitInfos[info_index].unitData);
      this.Hide();
    });
    EventDelegate.Set(unitIcon.Button.onLongPress, (EventDelegate.Callback) (() =>
    {
      PlayerUnit playerUnit = this.displayUnitInfos[info_index].unitData.PlayerUnit;
      if (playerUnit == (PlayerUnit) null)
        this.StartCoroutine(PopupCommon.Show(Consts.GetInstance().POPUP_030_SEA_MYPAGE_CALL_TITLE, Consts.GetInstance().POPUP_030_SEA_MYPAGE_CALL_MESSAGE));
      else
        Unit0042Scene.changeScene(true, playerUnit, this.playerUnitList);
    }));
    unitIcon.ForBattle = false;
    unitIcon.CanAwake = false;
    unitIcon.Equip = false;
    unitIcon1.SpecialIconType = -1;
    unitIcon1.SpecialIcon = false;
    unitIcon.SetCounter(0);
    unitIcon.SetSelectionCounter(0);
    unitIcon.SelectMarker = false;
    unitIcon.Deselect();
    ((Component) unitIcon).gameObject.SetActive(true);
  }

  private IEnumerator LoadObjectSprite()
  {
    if (this.displayUnitInfos.Count > this.iconMaxValue)
    {
      yield return (object) null;
      for (int i = this.iconMaxValue; i < this.displayUnitInfos.Count; ++i)
      {
        IEnumerator e = UnitIcon.LoadSprite(this.displayUnitInfos[i].unitData.Unit);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private class UnitInfo
  {
    public GameObject icon;
    public SeaHomeManager.UnitConrtolleData unitData;
    public bool pickup;

    public UnitInfo(SeaHomeManager.UnitConrtolleData data, bool pickup)
    {
      this.unitData = data;
      this.icon = (GameObject) null;
      this.pickup = pickup;
    }
  }
}
