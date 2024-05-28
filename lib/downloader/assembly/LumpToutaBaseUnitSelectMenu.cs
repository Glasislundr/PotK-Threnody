// Decompiled with JetBrains decompiler
// Type: LumpToutaBaseUnitSelectMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/LumpTouta/BaseUnitSelectMenu")]
public class LumpToutaBaseUnitSelectMenu : UnitSelectMenuBase
{
  public const int MAX_SELECT_BASE_UNIT = 30;
  [SerializeField]
  private UIPanel scrollPanel;
  [SerializeField]
  private UIWidget scrollBarWidget;
  [SerializeField]
  private UILabel useZeny;
  [SerializeField]
  private UILabel selectedCount;
  [SerializeField]
  private SpreadColorButton DecisionButton;
  private List<PlayerUnit> selectedBasePlayerUnits = new List<PlayerUnit>();
  private List<List<UnitIconInfo>> selectedMaterialUnitIconInfos = new List<List<UnitIconInfo>>();
  private Dictionary<PlayerUnit, List<PlayerUnit>> allSamePlayerUnits = new Dictionary<PlayerUnit, List<PlayerUnit>>();

  public IEnumerator Init()
  {
    LumpToutaBaseUnitSelectMenu baseUnitSelectMenu = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    yield return (object) null;
    baseUnitSelectMenu.selectedCount.text = "0" + string.Format("/{0}", (object) 30);
    ((UIRect) baseUnitSelectMenu.scrollPanel).bottomAnchor.absolute = 120;
    ((UIRect) baseUnitSelectMenu.scrollBarWidget).bottomAnchor.absolute = 140;
    ((UIButtonColor) baseUnitSelectMenu.DecisionButton).isEnabled = false;
    baseUnitSelectMenu.isMaterial = true;
    IEnumerator e = baseUnitSelectMenu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    List<PlayerUnit> baseUnitPlayerList = new List<PlayerUnit>();
    e = baseUnitSelectMenu.CreateBasePlayerUnits(baseUnitPlayerList);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    baseUnitSelectMenu.SetIconType(UnitMenuBase.IconType.Normal);
    Persist<Persist.UnitSortAndFilterInfo> toutaSortAndFilter = Persist.unitLumpToutaSortAndFilter;
    toutaSortAndFilter.Data.setInitSortType(UnitSortAndFilter.SORT_TYPES.PossessionNumber);
    baseUnitSelectMenu.InitializeInfo((IEnumerable<PlayerUnit>) baseUnitPlayerList.ToArray(), (IEnumerable<PlayerMaterialUnit>) null, toutaSortAndFilter, false, false, true, true, false);
    e = baseUnitSelectMenu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    baseUnitSelectMenu.InitializeEnd();
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  private IEnumerator CreateBasePlayerUnits(List<PlayerUnit> baseUnitPlayerList)
  {
    LumpToutaBaseUnitSelectMenu baseUnitSelectMenu = this;
    float unityMax = (float) PlayerUnit.GetUnityValueMax();
    PlayerUnit[] baseUnits = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit && (double) x.unityTotal < (double) unityMax)).ToArray<PlayerUnit>();
    int frameBalanceCount = 0;
    Dictionary<int, List<PlayerUnit>> unityMaterialUnits = new Dictionary<int, List<PlayerUnit>>();
    PlayerUnit[] playerUnitArray = baseUnits;
    int index;
    for (index = 0; index < playerUnitArray.Length; ++index)
    {
      PlayerUnit playerUnit = playerUnitArray[index];
      if (!baseUnitSelectMenu.IsExclusionUnitForLumpToutaMaterial(playerUnit))
      {
        UnitUnit unit = playerUnit.unit;
        if (!unityMaterialUnits.ContainsKey(unit.same_character_id))
          unityMaterialUnits.Add(unit.same_character_id, new List<PlayerUnit>());
        unityMaterialUnits[unit.same_character_id].Add(playerUnit);
      }
      ++frameBalanceCount;
      if (frameBalanceCount % 30 == 0)
        yield return (object) null;
    }
    playerUnitArray = (PlayerUnit[]) null;
    Tuple<PlayerMaterialUnit, int[]>[] hasMaterialUnits = LumpToutaBaseUnitSelectMenu.getUnityValueUpMaterials();
    playerUnitArray = baseUnits;
    for (index = 0; index < playerUnitArray.Length; ++index)
    {
      PlayerUnit baseUnitKey = playerUnitArray[index];
      UnitUnit buu = baseUnitKey.unit;
      int sameCharacterId = buu.same_character_id;
      List<PlayerUnit> source1 = new List<PlayerUnit>();
      List<PlayerUnit> source2;
      if (unityMaterialUnits.TryGetValue(sameCharacterId, out source2))
        source1 = source2.Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != baseUnitKey)).ToList<PlayerUnit>();
      baseUnitSelectMenu.allSamePlayerUnits.Add(baseUnitKey, source1);
      if (source1.Any<PlayerUnit>())
        baseUnitPlayerList.Add(baseUnitKey);
      else if (((IEnumerable<Tuple<PlayerMaterialUnit, int[]>>) hasMaterialUnits).Any<Tuple<PlayerMaterialUnit, int[]>>((Func<Tuple<PlayerMaterialUnit, int[]>, bool>) (o => ((IEnumerable<int>) o.Item2).Contains<int>(buu.ID))))
        baseUnitPlayerList.Add(baseUnitKey);
      ++frameBalanceCount;
      if (frameBalanceCount % 200 == 0)
        yield return (object) null;
    }
    playerUnitArray = (PlayerUnit[]) null;
  }

  public static Tuple<PlayerMaterialUnit, int[]>[] getUnityValueUpMaterials()
  {
    DateTime nowDateTime = ServerTime.NowAppTimeAddDelta();
    return ((IEnumerable<PlayerMaterialUnit>) SMManager.Get<PlayerMaterialUnit[]>()).Where<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => LumpToutaBaseUnitSelectMenu.checkAvailability(x, nowDateTime))).Select<PlayerMaterialUnit, Tuple<PlayerMaterialUnit, int[]>>((Func<PlayerMaterialUnit, Tuple<PlayerMaterialUnit, int[]>>) (y => Tuple.Create<PlayerMaterialUnit, int[]>(y, ((IEnumerable<UnityValueUpPattern>) y.unit.UnityValueUpPatterns).Where<UnityValueUpPattern>((Func<UnityValueUpPattern, bool>) (yy => yy.unit_UnitUnit.HasValue)).Select<UnityValueUpPattern, int>((Func<UnityValueUpPattern, int>) (zz => zz.unit_UnitUnit.Value)).ToArray<int>()))).ToArray<Tuple<PlayerMaterialUnit, int[]>>();
  }

  private static bool checkAvailability(PlayerMaterialUnit pmu, DateTime now)
  {
    if (pmu.quantity <= 0)
      return false;
    UnitUnit unit = pmu.unit;
    return unit.is_unity_value_up && (!unit.expire_date_UnitExpireDate.HasValue || !(unit.expire_date.end_at <= now)) && ((IEnumerable<UnityValueUpPattern>) unit.UnityValueUpPatterns).Select<UnityValueUpPattern, UnitUnit>((Func<UnityValueUpPattern, UnitUnit>) (x => x.unit)).Where<UnitUnit>((Func<UnitUnit, bool>) (y => y != null)).Select<UnitUnit, int>((Func<UnitUnit, int>) (z => z.same_character_id)).Distinct<int>().Count<int>() == 1;
  }

  protected override void Select(UnitIconBase selectUnitIcon)
  {
    if (!selectUnitIcon.Selected && this.selectedBasePlayerUnits.Count >= 30)
      return;
    int sameCharacterId = selectUnitIcon.unit.same_character_id;
    List<UnitIconInfo> sameAllUnitInfos = new List<UnitIconInfo>();
    List<UnitIconBase> sameAllUnitIcons = new List<UnitIconBase>();
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      if (allUnitInfo.playerUnit != selectUnitIcon.PlayerUnit && allUnitInfo.unit.same_character_id == sameCharacterId)
      {
        sameAllUnitInfos.Add(allUnitInfo);
        foreach (UnitIconBase allUnitIcon in this.allUnitIcons)
        {
          if (allUnitIcon.PlayerUnit == allUnitInfo.playerUnit)
            sameAllUnitIcons.Add(allUnitIcon);
        }
      }
    }
    if (selectUnitIcon.Selected)
    {
      this.UnSelect(selectUnitIcon);
      this.LumpUnSelect(selectUnitIcon, sameAllUnitInfos, sameAllUnitIcons);
    }
    else
    {
      this.OnSelect(selectUnitIcon);
      this.LumpOnSelect(selectUnitIcon, sameAllUnitInfos, sameAllUnitIcons);
    }
    this.UpdateBottomInfo();
  }

  private void LumpOnSelect(
    UnitIconBase selectUnitIcon,
    List<UnitIconInfo> sameAllUnitInfos,
    List<UnitIconBase> sameAllUnitIcons)
  {
    this.selectedBasePlayerUnits.Add(selectUnitIcon.PlayerUnit);
    this.selectedMaterialUnitIconInfos.Add(sameAllUnitInfos);
    foreach (UnitIconInfo sameAllUnitInfo in sameAllUnitInfos)
      sameAllUnitInfo.select = 0;
    foreach (UnitIconBase sameAllUnitIcon in sameAllUnitIcons)
    {
      if (sameAllUnitIcon.Selected)
        this.UnSelect(sameAllUnitIcon);
      else
        this.OnSelect(sameAllUnitIcon);
      sameAllUnitIcon.HideCheckIcon();
    }
    if (this.selectedBasePlayerUnits.Count < 30)
      return;
    this.SelectWhiteAndNoSelectGray(true);
  }

  private void LumpUnSelect(
    UnitIconBase selectUnitIcon,
    List<UnitIconInfo> sameAllUnitInfos,
    List<UnitIconBase> sameAllUnitIcons)
  {
    int index1 = -1;
    for (int index2 = 0; index2 < this.selectedBasePlayerUnits.Count; ++index2)
    {
      if (this.selectedBasePlayerUnits[index2].unit.same_character_id == selectUnitIcon.unit.same_character_id)
      {
        index1 = index2;
        break;
      }
    }
    bool flag = false;
    if (this.selectedBasePlayerUnits.Count == 30)
      flag = true;
    this.selectedBasePlayerUnits.RemoveAt(index1);
    this.selectedMaterialUnitIconInfos.RemoveAt(index1);
    foreach (UnitIconInfo sameAllUnitInfo in sameAllUnitInfos)
      sameAllUnitInfo.select = -1;
    foreach (UnitIconBase sameAllUnitIcon in sameAllUnitIcons)
    {
      if (sameAllUnitIcon.Selected)
        this.UnSelect(sameAllUnitIcon);
      else
        this.OnSelect(sameAllUnitIcon);
    }
    if (!flag)
      return;
    this.SelectWhiteAndNoSelectGray(false);
  }

  private void SelectWhiteAndNoSelectGray(bool b)
  {
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
    foreach (PlayerUnit selectedBasePlayerUnit in this.selectedBasePlayerUnits)
      playerUnitList.Add(selectedBasePlayerUnit);
    foreach (List<UnitIconInfo> materialUnitIconInfo in this.selectedMaterialUnitIconInfos)
    {
      foreach (UnitIconInfo unitIconInfo in materialUnitIconInfo)
        playerUnitList.Add(unitIconInfo.playerUnit);
    }
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      if (playerUnitList.Contains(allUnitInfo.playerUnit))
      {
        if (Object.op_Inequality((Object) allUnitInfo.icon, (Object) null))
        {
          if (b)
            allUnitInfo.icon.Gray = false;
          else
            allUnitInfo.icon.Gray = true;
        }
      }
      else if (Object.op_Inequality((Object) allUnitInfo.icon, (Object) null))
      {
        if (b)
          allUnitInfo.icon.Gray = true;
        else
          allUnitInfo.icon.Gray = false;
      }
    }
  }

  public override void OnSelect(UnitIconBase unitIcon)
  {
    unitIcon.SelectByCheckIcon();
    this.UnitInfoUpdates(unitIcon);
  }

  public override void UpdateSelectIcon()
  {
    foreach (UnitIconInfo selectedUnitIcon in this.selectedUnitIcons)
    {
      UnitIconInfo unitInfoDisplay = this.GetUnitInfoDisplay(selectedUnitIcon.playerUnit);
      if (unitInfoDisplay != null && Object.op_Inequality((Object) unitInfoDisplay.icon, (Object) null))
      {
        if (!this.selectedBasePlayerUnits.Contains(selectedUnitIcon.playerUnit))
          unitInfoDisplay.icon.HideCheckIcon();
        else
          unitInfoDisplay.icon.SelectByCheckIcon();
      }
    }
  }

  protected override void CreateUnitIconAction(int info_index, int unit_index)
  {
    UnitIconBase allUnitIcon = this.allUnitIcons[unit_index];
    UnitIconInfo info = this.displayUnitInfos[info_index];
    if (this.selectedBasePlayerUnits.Count >= 30)
    {
      if (info.select == 0 && this.selectedBasePlayerUnits.Contains(allUnitIcon.PlayerUnit))
        allUnitIcon.SelectByCheckIcon(false);
      else if (info.select == 0)
      {
        allUnitIcon.SelectByCheckIcon(false);
        allUnitIcon.HideCheckIcon();
      }
      else
        allUnitIcon.Gray = true;
    }
    else if (info.select == 0 && this.selectedBasePlayerUnits.Contains(allUnitIcon.PlayerUnit))
      allUnitIcon.SelectByCheckIcon();
    else if (info.select == 0)
    {
      allUnitIcon.SelectByCheckIcon();
      allUnitIcon.HideCheckIcon();
    }
    else
      allUnitIcon.Gray = false;
    allUnitIcon.onClick = (Action<UnitIconBase>) (ui => this.Select(ui));
    allUnitIcon.onLongPress = (Action<UnitIconBase>) (x => Unit0042Scene.changeSceneEvolutionUnit(true, info.playerUnit, this.getUnits(), true, IsToutaPlusNoEnable: true));
  }

  public override bool SelectedUnitIsMax() => false;

  private void UpdateBottomInfo()
  {
    long useMoney = 0;
    float unityValueMax = (float) PlayerUnit.GetUnityValueMax();
    foreach (PlayerUnit selectedBasePlayerUnit in this.selectedBasePlayerUnits)
    {
      float unityTotal = selectedBasePlayerUnit.unityTotal;
      float num = 0.0f;
      List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
      bool flag = false;
      foreach (PlayerUnit playerUnit in (IEnumerable<PlayerUnit>) this.allSamePlayerUnits[selectedBasePlayerUnit].OrderBy<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.unit.rarity.index)))
      {
        if (!flag)
        {
          if (playerUnitList.Count < 30)
          {
            if (!this.IsExclusionUnitForLumpToutaMaterial(playerUnit))
            {
              ++num;
              if ((double) num + (double) unityTotal >= (double) unityValueMax)
                flag = true;
              playerUnitList.Add(playerUnit);
            }
          }
          else
            break;
        }
        else
          break;
      }
      useMoney += CalcUnitCompose.priceCompose(selectedBasePlayerUnit, playerUnitList.ToArray());
    }
    if (useMoney >= SMManager.Get<Player>().money)
      ((UIWidget) this.useZeny).color = Color.red;
    else
      ((UIWidget) this.useZeny).color = Color.white;
    this.useZeny.SetTextLocalize(useMoney.ToString());
    if (this.selectedBasePlayerUnits.Count >= 30)
      ((UIWidget) this.selectedCount).color = Color.red;
    else
      ((UIWidget) this.selectedCount).color = Color.white;
    this.selectedCount.text = this.selectedBasePlayerUnits.Count.ToString() + string.Format("/{0}", (object) 30);
    this.SetDecisionButton(useMoney);
  }

  private void SetDecisionButton(long useMoney)
  {
    if (this.selectedBasePlayerUnits.Count <= 0)
      ((UIButtonColor) this.DecisionButton).isEnabled = false;
    else if (useMoney >= SMManager.Get<Player>().money)
      ((UIButtonColor) this.DecisionButton).isEnabled = false;
    else
      ((UIButtonColor) this.DecisionButton).isEnabled = true;
  }

  public override void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_LumpTouta_Confirmation", true, (object) this.selectedBasePlayerUnits, (object) this.allSamePlayerUnits);
  }

  public override void IbtnClearS()
  {
    base.IbtnClearS();
    this.selectedBasePlayerUnits.Clear();
    this.selectedMaterialUnitIconInfos.Clear();
    this.UpdateBottomInfo();
  }
}
