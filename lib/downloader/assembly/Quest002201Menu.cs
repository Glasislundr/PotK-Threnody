// Decompiled with JetBrains decompiler
// Type: Quest002201Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/QuestExtra/UnityValueUpSoloQuestMenu")]
public class Quest002201Menu : UnitMenuBase
{
  [SerializeField]
  private UILabel txtTitle_;
  private int Lid_;
  private int Mid_;
  private Dictionary<int, List<UnitUnit>> dicUnityValueUp_;
  private PlayerExtraQuestS[] playerQuests_;
  private PlayerUnit[] playerUnits_;
  private bool isLoadedResources_;
  private Action<GameObject[], NGMenuBase> openPopupUnityDetail_;
  private PlayerUnit current_;

  public Future<GameObject>[] unityDetailPrefabs { get; private set; }

  private PlayerExtraQuestS[] playerQuests
  {
    get
    {
      if (this.playerQuests_ != null)
        return this.playerQuests_;
      this.playerQuests_ = ((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).S(this.Lid_, this.Mid_, true);
      return this.playerQuests_;
    }
  }

  public IEnumerator doInitialize(int Lid, int Mid, bool bReset)
  {
    Quest002201Menu quest002201Menu = this;
    if (((quest002201Menu.Lid_ != Lid ? 1 : (quest002201Menu.Mid_ != Mid ? 1 : 0)) | (bReset ? 1 : 0)) != 0)
    {
      quest002201Menu.dicUnityValueUp_ = (Dictionary<int, List<UnitUnit>>) null;
      quest002201Menu.playerQuests_ = (PlayerExtraQuestS[]) null;
      quest002201Menu.playerUnits_ = (PlayerUnit[]) null;
    }
    quest002201Menu.Lid_ = Lid;
    quest002201Menu.Mid_ = Mid;
    Player player = Player.Current;
    quest002201Menu.isLoadedResources_ = false;
    quest002201Menu.StartCoroutine(quest002201Menu.doLoadResources());
    yield return (object) quest002201Menu.Initialize();
    quest002201Menu.SetIconType(UnitMenuBase.IconType.Normal);
    PlayerUnit[] units = quest002201Menu.getPlayerUnits();
    PlayerExtraQuestS[] quests = quest002201Menu.playerQuests;
    quest002201Menu.InitializeInfoEx((IEnumerable<PlayerUnit>) units, (IEnumerable<PlayerMaterialUnit>) null, Persist.quest002201SortAndFilter, false, false, true, true, true, false);
    yield return (object) quest002201Menu.CreateUnitIcon();
    quest002201Menu.txtTitle_.SetTextLocalize(quests == null || quests.Length == 0 ? string.Empty : ((IEnumerable<PlayerExtraQuestS>) quests).First<PlayerExtraQuestS>().quest_extra_s.quest_m.name);
    quest002201Menu.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) units.Length, (object) player.max_units));
    quest002201Menu.InitializeEnd();
    while (!quest002201Menu.isLoadedResources_)
      yield return (object) null;
    quest002201Menu.current_ = (PlayerUnit) null;
    if (quest002201Menu.lastReferenceUnitID != -1)
    {
      yield return (object) null;
      quest002201Menu.resolveScrollPosition(quest002201Menu.lastReferenceUnitID);
      yield return (object) null;
      // ISSUE: reference to a compiler-generated method
      quest002201Menu.selectedUnit(Array.Find<PlayerUnit>(units, new Predicate<PlayerUnit>(quest002201Menu.\u003CdoInitialize\u003Eb__15_0)));
      quest002201Menu.setLastReference();
    }
  }

  protected override void SortAndSetIcons(
    UnitSortAndFilter.SORT_TYPES type,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool isBattleFirst,
    bool isTowerEntry)
  {
    if (this.isInitialize)
      this.deselectedUnit();
    base.SortAndSetIcons(type, order, isBattleFirst, isTowerEntry);
  }

  public override IEnumerator UpdateInfoAndScroll(
    PlayerUnit[] playerUnits,
    PlayerMaterialUnit[] playerMaterialUnits = null)
  {
    int refUnitID = Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID;
    this.deselectedUnit();
    yield return (object) base.UpdateInfoAndScroll(playerUnits);
    if (refUnitID != -1)
      this.selectedUnit(Array.Find<PlayerUnit>(this.getPlayerUnits(), (Predicate<PlayerUnit>) (x => x.id == refUnitID)));
  }

  private IEnumerator doLoadResources()
  {
    if (this.unityDetailPrefabs == null)
    {
      this.unityDetailPrefabs = PopupUnityValueDetail.createLoaders(false);
      Future<GameObject>[] futureArray = this.unityDetailPrefabs;
      for (int index = 0; index < futureArray.Length; ++index)
        yield return (object) futureArray[index].Wait();
      futureArray = (Future<GameObject>[]) null;
    }
    this.isLoadedResources_ = true;
  }

  public PlayerUnit[] getPlayerUnits(bool bReset = false)
  {
    if (this.Lid_ == 0 || this.Mid_ == 0)
      return new PlayerUnit[0];
    if (this.dicUnityValueUp_ == null)
      this.dicUnityValueUp_ = UnityValueUpItemQuest.makeSkipSortieQuestUnityValueUp(((IEnumerable<PlayerExtraQuestS>) this.playerQuests).Select<PlayerExtraQuestS, int>((Func<PlayerExtraQuestS, int>) (x => x._quest_extra_s)));
    if (this.playerUnits_ == null | bReset)
    {
      List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
      foreach (PlayerUnit playerUnit in SMManager.Get<PlayerUnit[]>())
      {
        PlayerUnit pu = playerUnit;
        if (this.dicUnityValueUp_.Any<KeyValuePair<int, List<UnitUnit>>>((Func<KeyValuePair<int, List<UnitUnit>>, bool>) (x => x.Value.Any<UnitUnit>((Func<UnitUnit, bool>) (y => y.FindValueUpPattern(pu.unit, (Func<UnitFamily[]>) (() => pu.Families)) != null)))))
          playerUnitList.Add(pu);
      }
      this.playerUnits_ = playerUnitList.ToArray();
    }
    return this.playerUnits_;
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
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
    this.createUnitIconAction(unit_index);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index);
    this.createUnitIconAction(unit_index);
  }

  private void createUnitIconAction(int unit_index)
  {
    UnitIconBase unitIcon = this.allUnitIcons[unit_index];
    unitIcon.SelectMarker = this.current_ != (PlayerUnit) null && unitIcon.PlayerUnit == this.current_;
    unitIcon.Gray = this.checkAllClear(unitIcon.PlayerUnit);
    unitIcon.onClick = (Action<UnitIconBase>) (_ => this.onClickIcon(unitIcon.PlayerUnit));
  }

  private void selectedUnit(PlayerUnit unit)
  {
    if (this.current_ == unit)
      return;
    this.deselectedUnit();
    UnitIconBase unitIconBase = this.allUnitIcons.FirstOrDefault<UnitIconBase>((Func<UnitIconBase, bool>) (x => x.PlayerUnit == unit));
    if (Object.op_Inequality((Object) unitIconBase, (Object) null))
      unitIconBase.SelectMarker = true;
    this.current_ = unit;
  }

  private void deselectedUnit()
  {
    if (this.current_ == (PlayerUnit) null)
      return;
    UnitIconBase unitIconBase = this.allUnitIcons.FirstOrDefault<UnitIconBase>((Func<UnitIconBase, bool>) (x => x.PlayerUnit == this.current_));
    if (Object.op_Inequality((Object) unitIconBase, (Object) null))
      unitIconBase.SelectMarker = false;
    this.current_ = (PlayerUnit) null;
  }

  private bool checkAllClear(PlayerUnit unit)
  {
    bool flag = true;
    foreach (PlayerExtraQuestS playerQuest in this.playerQuests)
    {
      List<UnitUnit> source;
      if (!this.dicUnityValueUp_.TryGetValue(playerQuest._quest_extra_s, out source))
        flag = false;
      else if (source.Any<UnitUnit>((Func<UnitUnit, bool>) (x => x.FindValueUpPattern(unit.unit, (Func<UnitFamily[]>) (() => unit.Families)) != null)) && playerQuest.remain_battle_count.HasValue)
      {
        int? remainBattleCount = playerQuest.remain_battle_count;
        if (0 < remainBattleCount.GetValueOrDefault() & remainBattleCount.HasValue)
          flag = false;
      }
    }
    return flag;
  }

  private void onClickIcon(PlayerUnit unit)
  {
    bool bUpdate = false;
    this.openPopupUnityDetail_ = (Action<GameObject[], NGMenuBase>) ((prefabs, menu) =>
    {
      if (prefabs == null)
        return;
      if (bUpdate)
      {
        unit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == unit.id));
        if (unit == (PlayerUnit) null)
          return;
      }
      Quest002201Menu questMenu = menu as Quest002201Menu;
      if (Object.op_Inequality((Object) questMenu, (Object) null))
        questMenu.selectedUnit(unit);
      PopupUnityValueDetail.showSoloQuests(prefabs[0], prefabs[1], (float) unit.unity_value, unit.buildup_unity_value_f, unit, (Action<Action>) (endWait =>
      {
        bUpdate = true;
        NGGameDataManager instance3 = Singleton<NGGameDataManager>.GetInstance();
        NGSceneManager instance4 = Singleton<NGSceneManager>.GetInstance();
        object[] args = instance4.GetSavedChangeSceneParam().args;
        instance4.ReplaceSavedChangeSceneParamArgs(new object[3]
        {
          args[0],
          args[1],
          (object) unit.id
        });
        instance3.setSceneChangeLog(instance4.exportSceneChangeLog());
        instance3.OpenPopup = this.openPopupUnityDetail_;
        if (endWait != null)
          endWait();
        if (!Object.op_Inequality((Object) questMenu, (Object) null))
          return;
        this.deselectedUnit();
      }), (Action<PopupUtility.SceneTo>) (_ => { }));
    });
    this.openPopupUnityDetail_(((IEnumerable<Future<GameObject>>) this.unityDetailPrefabs).Select<Future<GameObject>, GameObject>((Func<Future<GameObject>, GameObject>) (x => x.Result)).ToArray<GameObject>(), (NGMenuBase) this);
  }

  public void preOpenUnityPopup() => this.IsPush = true;

  public override void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.deselectedUnit();
    QuestExtraS master = ((IEnumerable<PlayerExtraQuestS>) this.playerQuests).FirstOrDefault<PlayerExtraQuestS>()?.quest_extra_s;
    QuestExtra.SeekType? seekType = master?.seek_type;
    if (seekType.HasValue)
    {
      switch (seekType.GetValueOrDefault())
      {
        case QuestExtra.SeekType.M:
          QuestExtraM questM = master.quest_m;
          QuestExtraLL questLl = questM.quest_ll;
          if (questLl == null)
          {
            Quest00217Scene.ChangeScene(false, questM.category_QuestExtraCategory);
            return;
          }
          Quest00218Scene.backOrChangeScene(questLl.ID, new int?(master.ID));
          return;
        case QuestExtra.SeekType.L:
          QuestScoreCampaignProgress[] source = SMManager.Get<QuestScoreCampaignProgress[]>();
          if (source != null && ((IEnumerable<QuestScoreCampaignProgress>) source).Any<QuestScoreCampaignProgress>((Func<QuestScoreCampaignProgress, bool>) (x => x.quest_extra_l == master.quest_l_QuestExtraL)))
          {
            Quest00226Scene.ChangeScene(master.ID, false);
            return;
          }
          Quest00219Scene.ChangeScene(master.ID, false);
          return;
      }
    }
    this.backScene();
  }
}
