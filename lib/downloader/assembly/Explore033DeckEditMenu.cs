// Decompiled with JetBrains decompiler
// Type: Explore033DeckEditMenu
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
public class Explore033DeckEditMenu : Unit00468Menu
{
  [SerializeField]
  private GameObject mCostLabelParts;
  private Explore033DeckEditScene.Mode mMode;
  private GameObject mConfirmPopupPrefab;
  private bool mDeckEdited;
  private ExploreDataManager.PopupStateType mPopupStateLog;

  protected override void updateTxtCostValue(int cost = 0)
  {
    this.totalCost = cost;
    this.txtCostValue.SetTextLocalize(this.totalCost.ToString() + "/" + (object) this.maxCost);
    int num1 = ((UIButtonColor) this.ibtnYes).isEnabled ? 1 : 0;
    bool flag = false;
    if (this.mMode == Explore033DeckEditScene.Mode.Challenge && this.selectedUnitIcons.Count < 3)
      flag = true;
    ((UIButtonColor) this.ibtnYes).isEnabled = this.totalCost > 0 && !flag;
    int num2 = ((UIButtonColor) this.ibtnYes).isEnabled ? 1 : 0;
    if (num1 == num2)
      return;
    ((UIWidget) ((Component) ((Component) this.ibtnYes).transform.GetChild(0)).GetComponent<UISprite>()).color = this.totalCost > 0 ? Color.white : Color.gray;
  }

  public IEnumerator InitializeAsync(Explore033DeckEditScene.Mode mode)
  {
    Explore033DeckEditMenu explore033DeckEditMenu = this;
    explore033DeckEditMenu.mMode = mode;
    explore033DeckEditMenu.isNonCheckOverkillers = true;
    IEnumerator e = explore033DeckEditMenu.loadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    explore033DeckEditMenu.SetIconType(UnitMenuBase.IconType.Normal);
    e = explore033DeckEditMenu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    explore033DeckEditMenu.serverTime = ServerTime.NowAppTime();
    ((IEnumerable<GameObject>) explore033DeckEditMenu.linkCharacters).ForEach<GameObject>((Action<GameObject>) (v => v.transform.Clear()));
    explore033DeckEditMenu.updateTxtCostValue(0);
    PlayerUnit[] source = SMManager.Get<PlayerUnit[]>();
    PlayerUnit[] deck = (PlayerUnit[]) null;
    switch (explore033DeckEditMenu.mMode)
    {
      case Explore033DeckEditScene.Mode.Explore:
        explore033DeckEditMenu.maxCost = int.MaxValue;
        explore033DeckEditMenu.mCostLabelParts.SetActive(false);
        deck = Singleton<ExploreDataManager>.GetInstance().GetExploreUnits();
        break;
      case Explore033DeckEditScene.Mode.Challenge:
        explore033DeckEditMenu.maxCost = SMManager.Get<Player>().max_cost;
        explore033DeckEditMenu.mCostLabelParts.SetActive(true);
        deck = Singleton<ExploreDataManager>.GetInstance().GetChallengeDeck().player_units;
        break;
    }
    PlayerUnit[] array = ((IEnumerable<PlayerUnit>) source).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>();
    explore033DeckEditMenu.InitializeInfo((IEnumerable<PlayerUnit>) array, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit00468SortAndFilter, false, false, false, true, true, (Action) (() => this.InitializeAllUnitInfosExtend(deck)));
    e = explore033DeckEditMenu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = explore033DeckEditMenu.CreateBottomInformationObject();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = explore033DeckEditMenu.DisplaySelectUnit();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    explore033DeckEditMenu.UpdateInfomation();
    explore033DeckEditMenu.mPopupStateLog = Singleton<ExploreDataManager>.GetInstance().mPopupState;
    Singleton<ExploreDataManager>.GetInstance().InitReopenPopupState();
    explore033DeckEditMenu.InitializeEnd();
  }

  public IEnumerator onBackSceneAsync(Explore033DeckEditScene.Mode mode)
  {
    Explore033DeckEditMenu explore033DeckEditMenu = this;
    PlayerUnit[] source = SMManager.Get<PlayerUnit[]>();
    yield return (object) explore033DeckEditMenu.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) source).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>());
    explore033DeckEditMenu.UpdateInfomation();
  }

  private IEnumerator loadResources()
  {
    if (Object.op_Equality((Object) this.mConfirmPopupPrefab, (Object) null))
    {
      Future<GameObject> loader = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/explore033_Top/Explore_SearcherDeckEdit_Confirm");
      IEnumerator e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mConfirmPopupPrefab = loader.Result;
      loader = (Future<GameObject>) null;
    }
  }

  private void InitializeAllUnitInfosExtend(PlayerUnit[] deck)
  {
    bool updateIndex = this.selectedUnitIcons.Count == 0;
    UnitIconInfo[] array = this.selectedUnitIcons.ToArray();
    this.selectedUnitIcons.Clear();
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      UnitIconInfo info = allUnitInfo;
      if (updateIndex)
      {
        info.select = -1;
        info.for_battle = false;
        int? nullable = ((IEnumerable<PlayerUnit>) deck).FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (a => a != (PlayerUnit) null && a.id == info.playerUnit.id));
        if (nullable.HasValue)
        {
          info.gray = true;
          info.select = nullable.Value;
          info.for_battle = true;
          this.selectedUnitIcons.Add(info);
        }
      }
      else
      {
        info.select = -1;
        info.for_battle = false;
        if (((IEnumerable<PlayerUnit>) deck).FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (a => a != (PlayerUnit) null && a.id == info.playerUnit.id)).HasValue)
          info.for_battle = true;
        UnitIconInfo unitIconInfo = ((IEnumerable<UnitIconInfo>) array).FirstOrDefault<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.playerUnit.id == info.playerUnit.id));
        if (unitIconInfo != null)
        {
          info.select = unitIconInfo.select;
          this.selectedUnitIcons.Add(info);
        }
      }
    }
    this.ReflectionSelectUnit();
    this.CreateSelectUnitList(updateIndex);
    this.updateTxtCostValue(this.GetUsedCost());
  }

  protected override IEnumerator CreateUnitIcon(
    int info_index,
    int unit_index,
    PlayerUnit baseUnit = null)
  {
    Explore033DeckEditMenu explore033DeckEditMenu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = explore033DeckEditMenu.\u003C\u003En__0(info_index, unit_index, baseUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    explore033DeckEditMenu.CreateUnitIconAction(info_index, unit_index);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index, baseUnit);
    this.CreateUnitIconAction(info_index, unit_index);
  }

  protected override void CreateUnitIconAction(int info_index, int unit_index)
  {
    base.CreateUnitIconAction(info_index, unit_index);
    UnitIconBase unitIcon = this.allUnitIcons[unit_index];
    unitIcon.onLongPress = (Action<UnitIconBase>) (x =>
    {
      Singleton<NGSceneManager>.GetInstance().clearStack();
      Unit0042Scene.changeScene(true, unitIcon.PlayerUnit, this.getUnits(), this.isMaterial);
    });
  }

  protected override void Select(UnitIconBase unitIcon)
  {
    if (!unitIcon.Selected && ((UnitIcon) unitIcon).BreakWeapon)
      return;
    base.Select(unitIcon);
  }

  protected override void IconGraySetting(UnitIconBase unitIcon, UnitIconInfo info)
  {
    bool flag = this.CheckWeaponBreak(info.playerUnit);
    ((UnitIcon) unitIcon).BreakWeapon = flag;
    if (flag)
    {
      info.gray = true;
      unitIcon.Gray = true;
    }
    else
      base.IconGraySetting(unitIcon, info);
  }

  private bool CheckWeaponBreak(PlayerUnit playerUnit)
  {
    bool flag = false;
    if (playerUnit.equippedGear != (PlayerItem) null)
      flag |= playerUnit.equippedGear.broken;
    if (playerUnit.equippedGear2 != (PlayerItem) null)
      flag |= playerUnit.equippedGear2.broken;
    if (playerUnit.equippedGear3 != (PlayerItem) null)
      flag |= playerUnit.equippedGear3.broken;
    return flag;
  }

  protected override IEnumerator DeckEditAsync()
  {
    Explore033DeckEditMenu explore033DeckEditMenu = this;
    int[] player_unit_ids = explore033DeckEditMenu.selectedUnitIcons.OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.select)).Select<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.playerUnit.id)).ToArray<int>();
    int total_combat = explore033DeckEditMenu.selectedUnitIcons.Sum<UnitIconInfo>((Func<UnitIconInfo, int>) (x => Judgement.NonBattleParameter.FromPlayerUnit(x.playerUnit).Combat));
    ExploreDataManager dataManager = Singleton<ExploreDataManager>.GetInstance();
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    if (Object.op_Inequality((Object) Singleton<ExploreSceneManager>.GetInstanceOrNull(), (Object) null))
    {
      dataManager.CleanLog();
      Singleton<ExploreSceneManager>.GetInstance().SetReloadDirty();
      yield return (object) Singleton<NGSceneManager>.GetInstance().destroyLoadedScenesImmediate();
    }
    int deckIndex = 1;
    switch (explore033DeckEditMenu.mMode)
    {
      case Explore033DeckEditScene.Mode.Explore:
        deckIndex = 1;
        Singleton<ExploreLotteryCore>.GetInstance().SetDeckEditedDirty();
        break;
      case Explore033DeckEditScene.Mode.Challenge:
        deckIndex = 2;
        break;
    }
    yield return (object) dataManager.LoadSuspendData(true);
    yield return (object) dataManager.ResumeExplore();
    bool saveFailed = false;
    yield return (object) dataManager.EditDeck(deckIndex, player_unit_ids, total_combat, (Action) (() => saveFailed = true));
    if (saveFailed)
    {
      explore033DeckEditMenu.OnDeckEditFailed();
    }
    else
    {
      explore033DeckEditMenu.mDeckEdited = true;
      explore033DeckEditMenu.backScene();
    }
  }

  private void OnDeckEditFailed()
  {
    ExploreSceneManager instanceOrNull = Singleton<ExploreSceneManager>.GetInstanceOrNull();
    if (Object.op_Inequality((Object) instanceOrNull, (Object) null))
      instanceOrNull.SetReloadDirty();
    Singleton<ExploreDataManager>.GetInstance().InitReopenPopupState();
    MypageScene.ChangeScene();
  }

  public override void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    switch (this.mMode)
    {
      case Explore033DeckEditScene.Mode.Explore:
        Singleton<PopupManager>.GetInstance().open(this.mConfirmPopupPrefab).GetComponent<PopupCommonNoYes>().SetDelegate((Action) (() => this.StartCoroutine(this.DeckEditAsync())));
        break;
      case Explore033DeckEditScene.Mode.Challenge:
        this.StartCoroutine(this.DeckEditAsync());
        break;
    }
  }

  public override void IbtnClearS()
  {
    if (this.IsPush)
      return;
    foreach (UnitIconBase allUnitIcon in this.allUnitIcons)
    {
      if (Object.op_Inequality((Object) allUnitIcon, (Object) null) && allUnitIcon.PlayerUnit != (PlayerUnit) null)
      {
        this.Deselect(allUnitIcon);
        if (((UnitIcon) allUnitIcon).BreakWeapon)
          allUnitIcon.Gray = true;
        else
          allUnitIcon.Gray = false;
      }
    }
    this.allUnitInfos.ForEach((Action<UnitIconInfo>) (v => v.select = -1));
    this.displayUnitInfos.ForEach((Action<UnitIconInfo>) (v => v.select = -1));
    this.selectedUnitIcons.Clear();
    this.UpdateInfomation();
  }

  protected override void backScene()
  {
    Singleton<ExploreDataManager>.GetInstance().mPopupState = this.mPopupStateLog;
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    if (instance.backScene())
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = -1;
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = -1;
    instance.destroyCurrentScene();
    instance.clearStack();
    Explore033TopScene.changeScene(false, !this.mDeckEdited);
  }
}
