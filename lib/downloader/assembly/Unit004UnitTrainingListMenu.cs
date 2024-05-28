// Decompiled with JetBrains decompiler
// Type: Unit004UnitTrainingListMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/Training/BaseUnitSelectMenu")]
public class Unit004UnitTrainingListMenu : UnitMenuBase
{
  public UIButton LumpToutaBtn;
  public UIButton RegressionBtn;
  private UnitSortAndFilter filter;
  private GameObject SaveMemorySlotSelectPrefab;
  private PlayerUnit[] playerUnits;

  public bool isInit { get; private set; }

  public IEnumerator Initalize(
    PlayerDeck playerDeck,
    PlayerUnit[] playerUnits,
    int max_cost,
    bool isEquip)
  {
    Unit004UnitTrainingListMenu trainingListMenu = this;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    trainingListMenu.serverTime = ServerTime.NowAppTime();
    trainingListMenu.IsRecord = true;
    playerUnits = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>();
    if (PlayerUnitTransMigrateMemoryList.Current == null && Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
    {
      e = WebAPI.UnitListTransmigrateMemory().Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    trainingListMenu.InitializeInfoEx((IEnumerable<PlayerUnit>) playerUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit004UnitTrainingListSortAndFilter, false, false, true, true, true, false);
    e = trainingListMenu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    trainingListMenu.InitializeEnd();
    List<UnitIcon> unitIconList = new List<UnitIcon>();
    Future<GameObject> prefabF = (Future<GameObject>) null;
    if (Object.op_Equality((Object) trainingListMenu.SaveMemorySlotSelectPrefab, (Object) null))
    {
      prefabF = Res.Prefabs.popup.popup_004_save_memory_slot_select__anim_popup01.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      trainingListMenu.SaveMemorySlotSelectPrefab = prefabF.Result;
    }
    trainingListMenu.LumpToutaBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => Singleton<NGSceneManager>.GetInstance().changeScene("unit004_LumpTouta", true))));
    trainingListMenu.RegressionBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => Unit004RegressionScene.changeScene())));
    Player player = SMManager.Get<Player>();
    trainingListMenu.SetTextPosession(playerUnits, player);
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
      Singleton<TutorialRoot>.GetInstance().CurrentAdvise();
    if (trainingListMenu.lastReferenceUnitID != -1)
    {
      yield return (object) null;
      trainingListMenu.resolveScrollPosition(trainingListMenu.lastReferenceUnitID);
      yield return (object) null;
      trainingListMenu.setLastReference();
    }
    trainingListMenu.isInit = true;
  }

  public void SetTextPosession(PlayerUnit[] playerUnits, Player player)
  {
    this.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) playerUnits.Length, (object) player.max_units));
  }

  protected override IEnumerator CreateUnitIcon(
    int info_index,
    int unit_index,
    PlayerUnit baseUnit = null)
  {
    IEnumerator e = base.CreateUnitIcon(info_index, unit_index, baseUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.CreateUnitIconAction(info_index, unit_index);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index);
    this.CreateUnitIconAction(info_index, unit_index);
  }

  private void CreateUnitIconAction(int info_index, int unit_index)
  {
    UnitIconBase allUnitIcon = this.allUnitIcons[unit_index];
    UnitIconInfo displayUnitInfo = this.displayUnitInfos[info_index];
    ((Component) allUnitIcon).gameObject.GetComponent<UnitIcon>().SetIconBoxCollider(true);
    allUnitIcon.onClick = (Action<UnitIconBase>) (ui =>
    {
      if (ui.Selected)
        return;
      Unit004TrainingScene.changeScene(true, ui.PlayerUnit);
    });
  }

  protected override IEnumerator ShowSortAndFilterPopup()
  {
    Unit004UnitTrainingListMenu menu = this;
    if (!Singleton<PopupManager>.GetInstance().isOpen)
    {
      if (Object.op_Equality((Object) menu.filter, (Object) null))
      {
        IEnumerator coroutine = menu.CreateUnitSortAndFilterPrefab();
        yield return (object) menu.StartCoroutine(coroutine);
        menu.filter = (UnitSortAndFilter) coroutine.Current;
        IEnumerator e = ((Component) menu.filter).GetComponent<UnitSortAndFilter>().Initialize(new Action<SortInfo>(((UnitMenuBase) menu).Sort), menu.persist, (UnitMenuBase) menu, menu.isDispOnlyNormalUnit, menu.isFriendSupport);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        // ISSUE: reference to a compiler-generated method
        menu.filter.SortFilterUnitNum = new Action<SortInfo>(menu.\u003CShowSortAndFilterPopup\u003Eb__14_0);
        coroutine = (IEnumerator) null;
      }
      menu.filter.SetUnitNum(menu.displayUnitInfos, menu.allUnitInfos);
      ((Component) menu.filter).gameObject.SetActive(false);
      Singleton<PopupManager>.GetInstance().open(((Component) menu.filter).gameObject, isCloned: true);
      ((Component) menu.filter).gameObject.SetActive(true);
    }
    else
      menu.IsPush = false;
  }

  private IEnumerator CreateUnitSortAndFilterPrefab()
  {
    if (Object.op_Equality((Object) this.filter, (Object) null))
    {
      Future<GameObject> sortPopupPrefabF = Res.Prefabs.popup.popup_Unit_Sort__anim_popup01.Load<GameObject>();
      IEnumerator e = sortPopupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.filter = sortPopupPrefabF.Result.Clone(Singleton<CommonRoot>.GetInstance().LoadTmpObj.transform).GetComponent<UnitSortAndFilter>();
      sortPopupPrefabF = (Future<GameObject>) null;
    }
    yield return (object) this.filter;
  }

  public void IbtnRecord()
  {
    this.StartCoroutine(Singleton<PopupManager>.GetInstance().open(this.SaveMemorySlotSelectPrefab).GetComponent<Unit00499SaveMemorySlotSelect>().Initialize((PlayerUnit) null, new Action(this.RecordEndUpdate)));
  }

  public void RecordEndUpdate() => Unit004UnitTrainingListScene.changeScene(false);

  public override void onBackButton() => this.onClickedClose();

  public void onClickedClose()
  {
    if (Singleton<CommonRoot>.GetInstance().isLoading || this.IsPushAndSet())
      return;
    this.backScene();
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }
}
