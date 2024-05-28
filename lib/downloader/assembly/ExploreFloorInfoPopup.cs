// Decompiled with JetBrains decompiler
// Type: ExploreFloorInfoPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Explore;
using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class ExploreFloorInfoPopup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel mFloorNameLbl;
  [SerializeField]
  private UIGrid mWeakPointGridElement;
  [SerializeField]
  private UIGrid mWeakPointGridGearKind;
  [SerializeField]
  private UIGrid mWeakPointGridPrincessType;
  [Space(8f)]
  [SerializeField]
  private NGxScroll mRewardScroll;
  [Space(8f)]
  [SerializeField]
  private GameObject mElementIconPrefab;
  [SerializeField]
  private GameObject mGearKindIconPrefab;
  [SerializeField]
  private GameObject mPrincessTypeIconPrefab;
  private GameObject mCreateIconPrefab;

  public IEnumerator Initialize(int floorId)
  {
    ExploreFloorInfoPopup exploreFloorInfoPopup = this;
    exploreFloorInfoPopup.IsPush = true;
    IEnumerator e = exploreFloorInfoPopup.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ExploreFloor floorData = MasterData.ExploreFloor[floorId];
    e = MasterData.LoadExploreEnemy(floorData.floor);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    List<ExploreEnemy> list = ((IEnumerable<ExploreEnemy>) MasterData.ExploreEnemyList).Where<ExploreEnemy>((Func<ExploreEnemy, bool>) (x => x.period_id == floorData.period_id)).ToList<ExploreEnemy>();
    MasterDataCache.Unload("ExploreEnemy");
    List<int> floorDropDeck = new List<int>();
    foreach (ExploreDropTable exploreDropTable in ((IEnumerable<ExploreDropTable>) MasterData.ExploreDropTableList).Where<ExploreDropTable>((Func<ExploreDropTable, bool>) (x => x.deck_id == floorData.drop_deck_id)))
      floorDropDeck.Add(exploreDropTable.drop_reward_id);
    exploreFloorInfoPopup.mFloorNameLbl.SetTextLocalize(floorData.name);
    exploreFloorInfoPopup.SetupWeakPointIcon(list);
    e = exploreFloorInfoPopup.SetupDropRewardIcon(list, floorDropDeck);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    exploreFloorInfoPopup.IsPush = false;
  }

  private IEnumerator LoadResources()
  {
    if (Object.op_Equality((Object) this.mCreateIconPrefab, (Object) null))
    {
      Future<GameObject> prefabF = new ResourceObject("Prefabs/RewardIcon/createIconObject").Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mCreateIconPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  private void SetupWeakPointIcon(List<ExploreEnemy> floorEnemyList)
  {
    HashSet<CommonElement> commonElementSet = new HashSet<CommonElement>();
    HashSet<GearKindEnum> gearKindEnumSet = new HashSet<GearKindEnum>();
    HashSet<UnitTypeEnum> unitTypeEnumSet = new HashSet<UnitTypeEnum>();
    foreach (WeakPoint weakPoint in floorEnemyList.Select<ExploreEnemy, WeakPoint>((Func<ExploreEnemy, WeakPoint>) (x => x.WeakPoint)))
    {
      commonElementSet.UnionWith((IEnumerable<CommonElement>) weakPoint.Element);
      gearKindEnumSet.UnionWith((IEnumerable<GearKindEnum>) weakPoint.Gearkind);
      unitTypeEnumSet.UnionWith((IEnumerable<UnitTypeEnum>) weakPoint.PrincessType);
    }
    foreach (CommonElement element in commonElementSet)
    {
      CommonElementIcon component = this.mElementIconPrefab.CloneAndGetComponent<CommonElementIcon>(((Component) this.mWeakPointGridElement).transform);
      component.Init(element);
      UI2DSprite componentInChildren = ((Component) component).GetComponentInChildren<UI2DSprite>();
      ((UIWidget) componentInChildren).width = 46;
      ((UIWidget) componentInChildren).height = 42;
      ((UIWidget) componentInChildren).depth = 100;
    }
    this.mWeakPointGridElement.Reposition();
    foreach (GearKindEnum kind in gearKindEnumSet)
    {
      GearKindButtonIcon component = this.mGearKindIconPrefab.CloneAndGetComponent<GearKindButtonIcon>(((Component) this.mWeakPointGridGearKind).transform);
      component.Init(kind);
      UI2DSprite componentInChildren = ((Component) component).GetComponentInChildren<UI2DSprite>();
      ((UIWidget) componentInChildren).width = 46;
      ((UIWidget) componentInChildren).height = 42;
      ((UIWidget) componentInChildren).depth = 100;
    }
    this.mWeakPointGridGearKind.Reposition();
    foreach (UnitTypeEnum unitType in unitTypeEnumSet)
    {
      PrincessTypeIcon component = this.mPrincessTypeIconPrefab.CloneAndGetComponent<PrincessTypeIcon>(((Component) this.mWeakPointGridPrincessType).transform);
      component.SetPrincessType(unitType);
      ((UIWidget) ((Component) component).GetComponentInChildren<UISprite>()).depth = 100;
    }
    this.mWeakPointGridPrincessType.Reposition();
  }

  private IEnumerator SetupDropRewardIcon(
    List<ExploreEnemy> floorEnemyList,
    List<int> floorDropDeck)
  {
    ExploreFloorInfoPopup exploreFloorInfoPopup = this;
    ((UIRect) exploreFloorInfoPopup.mRewardScroll.scrollView.panel).alpha = 0.0f;
    foreach (int floorDropReward in exploreFloorInfoPopup.GetFloorDropRewards(floorEnemyList, floorDropDeck))
    {
      IEnumerator e = exploreFloorInfoPopup.CreateRewardIcon(floorDropReward);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    // ISSUE: method pointer
    exploreFloorInfoPopup.mRewardScroll.GridReposition(new UIGrid.OnReposition((object) exploreFloorInfoPopup, __methodptr(\u003CSetupDropRewardIcon\u003Eb__12_0)));
  }

  private IEnumerable<int> GetFloorDropRewards(
    List<ExploreEnemy> floorEnemyList,
    List<int> floorDropDeck)
  {
    HashSet<int> intSet = new HashSet<int>();
    foreach (SortedDictionary<int, int> sortedDictionary in floorEnemyList.Select<ExploreEnemy, SortedDictionary<int, int>>((Func<ExploreEnemy, SortedDictionary<int, int>>) (x => x.DropDeck)))
    {
      foreach (KeyValuePair<int, int> keyValuePair in sortedDictionary)
        intSet.Add(keyValuePair.Value);
    }
    foreach (int num in floorDropDeck)
      intSet.Add(num);
    foreach (int floorDropReward in intSet)
      yield return floorDropReward;
  }

  private IEnumerator CreateRewardIcon(int rewardId)
  {
    ExploreDropReward exploreDropReward = MasterData.ExploreDropReward[rewardId];
    GameObject gameObject = this.mCreateIconPrefab.Clone();
    this.mRewardScroll.Add(gameObject);
    CreateIconObject createIcon = gameObject.gameObject.GetComponent<CreateIconObject>();
    IEnumerator e = createIcon.CreateThumbnail(exploreDropReward.reward_type, exploreDropReward.reward_id, exploreDropReward.reward_quantity, isButton: false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) createIcon).transform.localScale = new Vector3(0.8f, 0.8f, 1f);
    UnitIcon component = createIcon.GetIcon().GetComponent<UnitIcon>();
    if (Object.op_Inequality((Object) component, (Object) null))
      component.RarityCenter();
    createIcon.addComponentUniqueIconDragScrollView();
  }

  public void OnCloseButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.OnCloseButton();
}
