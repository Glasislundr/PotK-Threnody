// Decompiled with JetBrains decompiler
// Type: Unit004ReincarnationTypeUnitSelectionMenu
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
public class Unit004ReincarnationTypeUnitSelectionMenu : UnitMenuBase
{
  [SerializeField]
  private GameObject dirNoUnit;
  private UnitTypeTicket ticket_;
  private PlayerUnit selectedPlayerUnit;

  public bool isPopupOpen { get; set; }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public virtual IEnumerator Init(
    Player player,
    PlayerUnit[] playerUnits,
    bool isEquip,
    UnitTypeTicket ticket)
  {
    Unit004ReincarnationTypeUnitSelectionMenu unitSelectionMenu = this;
    unitSelectionMenu.ticket_ = ticket;
    yield return (object) unitSelectionMenu.Initialize();
    PlayerUnit[] filteringUnits = unitSelectionMenu.getFilteringUnits(playerUnits);
    unitSelectionMenu.InitializeInfo((IEnumerable<PlayerUnit>) filteringUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit004ReincarnationTypeAndFilter, isEquip, false, true, true, false);
    yield return (object) unitSelectionMenu.CreateUnitIcon();
    Singleton<PopupManager>.GetInstance().closeAll();
    unitSelectionMenu.isPopupOpen = false;
    unitSelectionMenu.lastReferenceUnitID = -1;
    unitSelectionMenu.InitializeEnd();
  }

  public IEnumerator reload(PlayerUnit[] playerUnits)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004ReincarnationTypeUnitSelectionMenu unitSelectionMenu = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    PlayerUnit[] filteringUnits = unitSelectionMenu.getFilteringUnits(playerUnits);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) unitSelectionMenu.UpdateInfoAndScroll(filteringUnits);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private int convertParamString(string s)
  {
    return !s.Contains(".") ? Convert.ToInt32(s) : Convert.ToInt32(Convert.ToDouble(s));
  }

  private PlayerUnit[] getFilteringUnits(PlayerUnit[] playerUnits)
  {
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
    List<PlayerUnit> usableUnitList = this.getUsableUnitList(((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)));
    UnitTypeTicket unitTypeTicket = MasterData.UnitTypeTicket[this.ticket_.ID];
    string[] strArray = unitTypeTicket.ticketTypeParam.Split(',');
    List<int> idList = new List<int>();
    foreach (string s in strArray)
    {
      if (!string.IsNullOrEmpty(s))
      {
        int num = this.convertParamString(s);
        idList.Add(num);
      }
    }
    switch (unitTypeTicket.ticketTypeID)
    {
      case UnitTypeTicketType.all:
        playerUnitList = usableUnitList;
        break;
      case UnitTypeTicketType.character:
        playerUnitList = this.getCharacterFilterList(usableUnitList, idList);
        break;
      case UnitTypeTicketType.unit:
        playerUnitList = this.getUnitFilterList(usableUnitList, idList);
        break;
      case UnitTypeTicketType.kind:
        playerUnitList = this.getKindFilterList(usableUnitList, idList);
        break;
      case UnitTypeTicketType.group_clothing:
        playerUnitList = this.getGroupClothingFilterList(usableUnitList, idList);
        break;
      case UnitTypeTicketType.group_generation:
        playerUnitList = this.getGroupGenerationFilterList(usableUnitList, idList);
        break;
      case UnitTypeTicketType.group_large:
        playerUnitList = this.getGroupLargeFilterList(usableUnitList, idList);
        break;
      case UnitTypeTicketType.group_small:
        playerUnitList = this.getGroupSmallFilterList(usableUnitList, idList);
        break;
      case UnitTypeTicketType.element:
        playerUnitList = this.getElementFilterList(usableUnitList, idList);
        break;
      case UnitTypeTicketType.unit_type_category:
        playerUnitList = this.getUnitTypeFilterList(usableUnitList, idList);
        break;
      case UnitTypeTicketType.job:
        playerUnitList = this.getJobFilterList(usableUnitList, idList);
        break;
      case UnitTypeTicketType.same_character:
        playerUnitList = this.getSameCharacterFilterList(usableUnitList, idList);
        break;
    }
    this.DoUnitTypeTicketExclude(playerUnitList);
    this.dirNoUnit.SetActive(!playerUnitList.Any<PlayerUnit>());
    return playerUnitList.ToArray();
  }

  private List<PlayerUnit> getUsableUnitList(IEnumerable<PlayerUnit> srcUnitList)
  {
    List<PlayerUnit> usableUnitList = new List<PlayerUnit>();
    UnitTypeTicketUnusable[] ticketUnusableList = MasterData.UnitTypeTicketUnusableList;
    if (((IEnumerable<UnitTypeTicketUnusable>) ticketUnusableList).Any<UnitTypeTicketUnusable>())
    {
      foreach (PlayerUnit srcUnit in srcUnitList)
      {
        PlayerUnit unit = srcUnit;
        if (((IEnumerable<UnitTypeTicketUnusable>) ticketUnusableList).FirstOrDefault<UnitTypeTicketUnusable>((Func<UnitTypeTicketUnusable, bool>) (x => x.ID == unit.unit.character_UnitCharacter)) == null)
          usableUnitList.Add(unit);
      }
    }
    else
    {
      foreach (PlayerUnit srcUnit in srcUnitList)
        usableUnitList.Add(srcUnit);
    }
    return usableUnitList;
  }

  private List<PlayerUnit> getCharacterFilterList(List<PlayerUnit> srcUnitList, List<int> idList)
  {
    List<PlayerUnit> characterFilterList = new List<PlayerUnit>();
    foreach (PlayerUnit srcUnit in srcUnitList)
    {
      PlayerUnit unit = srcUnit;
      if (idList.FirstOrDefault<int>((Func<int, bool>) (x => x == unit.unit.character_UnitCharacter)) > 0)
        characterFilterList.Add(unit);
    }
    return characterFilterList;
  }

  private List<PlayerUnit> getUnitFilterList(List<PlayerUnit> srcUnitList, List<int> idList)
  {
    List<PlayerUnit> unitFilterList = new List<PlayerUnit>();
    foreach (PlayerUnit srcUnit in srcUnitList)
    {
      PlayerUnit unit = srcUnit;
      if (idList.FirstOrDefault<int>((Func<int, bool>) (x => x == unit.unit.ID)) > 0)
        unitFilterList.Add(unit);
    }
    return unitFilterList;
  }

  private List<PlayerUnit> getKindFilterList(List<PlayerUnit> srcUnitList, List<int> idList)
  {
    List<PlayerUnit> kindFilterList = new List<PlayerUnit>();
    foreach (PlayerUnit srcUnit in srcUnitList)
    {
      PlayerUnit unit = srcUnit;
      if (idList.FirstOrDefault<int>((Func<int, bool>) (x => x == unit.unit.kind.ID)) > 0)
        kindFilterList.Add(unit);
    }
    return kindFilterList;
  }

  private List<PlayerUnit> getGroupClothingFilterList(
    List<PlayerUnit> srcUnitList,
    List<int> idList)
  {
    List<PlayerUnit> clothingFilterList = new List<PlayerUnit>();
    foreach (PlayerUnit srcUnit in srcUnitList)
    {
      UnitGroup groupInfo = this.getGroupInfo(srcUnit.unit.ID);
      if (groupInfo != null && idList.FirstOrDefault<int>((Func<int, bool>) (x => x == groupInfo.group_clothing_category_id_UnitGroupClothingCategory || x == groupInfo.group_clothing_category_id_2_UnitGroupClothingCategory)) > 0)
        clothingFilterList.Add(srcUnit);
    }
    return clothingFilterList;
  }

  private List<PlayerUnit> getGroupGenerationFilterList(
    List<PlayerUnit> srcUnitList,
    List<int> idList)
  {
    List<PlayerUnit> generationFilterList = new List<PlayerUnit>();
    foreach (PlayerUnit srcUnit in srcUnitList)
    {
      UnitGroup groupInfo = this.getGroupInfo(srcUnit.unit.ID);
      if (groupInfo != null && idList.FirstOrDefault<int>((Func<int, bool>) (x => x == groupInfo.group_generation_category_id_UnitGroupGenerationCategory)) > 0)
        generationFilterList.Add(srcUnit);
    }
    return generationFilterList;
  }

  private List<PlayerUnit> getGroupLargeFilterList(List<PlayerUnit> srcUnitList, List<int> idList)
  {
    List<PlayerUnit> groupLargeFilterList = new List<PlayerUnit>();
    foreach (PlayerUnit srcUnit in srcUnitList)
    {
      UnitGroup groupInfo = this.getGroupInfo(srcUnit.unit.ID);
      if (groupInfo != null && idList.FirstOrDefault<int>((Func<int, bool>) (x => x == groupInfo.group_large_category_id_UnitGroupLargeCategory)) > 0)
        groupLargeFilterList.Add(srcUnit);
    }
    return groupLargeFilterList;
  }

  private List<PlayerUnit> getGroupSmallFilterList(List<PlayerUnit> srcUnitList, List<int> idList)
  {
    List<PlayerUnit> groupSmallFilterList = new List<PlayerUnit>();
    foreach (PlayerUnit srcUnit in srcUnitList)
    {
      UnitGroup groupInfo = this.getGroupInfo(srcUnit.unit.ID);
      if (groupInfo != null && idList.FirstOrDefault<int>((Func<int, bool>) (x => x == groupInfo.group_small_category_id_UnitGroupSmallCategory)) > 0)
        groupSmallFilterList.Add(srcUnit);
    }
    return groupSmallFilterList;
  }

  private UnitGroup getGroupInfo(int unit_id)
  {
    UnitGroup groupInfo = (UnitGroup) null;
    Dictionary<int, UnitGroup> dictionary = ((IEnumerable<UnitGroup>) MasterData.UnitGroupList).ToDictionary<UnitGroup, int>((Func<UnitGroup, int>) (x => x.unit_id));
    if (dictionary == null)
      return (UnitGroup) null;
    dictionary.TryGetValue(unit_id, out groupInfo);
    return groupInfo;
  }

  private List<PlayerUnit> getElementFilterList(List<PlayerUnit> srcUnitList, List<int> idList)
  {
    List<PlayerUnit> elementFilterList = new List<PlayerUnit>();
    foreach (PlayerUnit srcUnit in srcUnitList)
    {
      int element = (int) srcUnit.unit.GetElement();
      if (idList.FirstOrDefault<int>((Func<int, bool>) (x => x == element)) > 0)
        elementFilterList.Add(srcUnit);
    }
    return elementFilterList;
  }

  private List<PlayerUnit> getUnitTypeFilterList(List<PlayerUnit> srcUnitList, List<int> idList)
  {
    List<PlayerUnit> unitTypeFilterList = new List<PlayerUnit>();
    foreach (PlayerUnit srcUnit in srcUnitList)
    {
      PlayerUnit unit = srcUnit;
      if (idList.FirstOrDefault<int>((Func<int, bool>) (x => x == unit._unit_type)) > 0)
        unitTypeFilterList.Add(unit);
    }
    return unitTypeFilterList;
  }

  private List<PlayerUnit> getJobFilterList(List<PlayerUnit> srcUnitList, List<int> idList)
  {
    List<PlayerUnit> jobFilterList = new List<PlayerUnit>();
    foreach (PlayerUnit srcUnit in srcUnitList)
    {
      PlayerUnit unit = srcUnit;
      if (idList.FirstOrDefault<int>((Func<int, bool>) (x => x == unit.unit.job_UnitJob)) > 0)
        jobFilterList.Add(unit);
    }
    return jobFilterList;
  }

  private List<PlayerUnit> getSameCharacterFilterList(
    List<PlayerUnit> srcUnitList,
    List<int> idList)
  {
    List<PlayerUnit> characterFilterList = new List<PlayerUnit>();
    foreach (PlayerUnit srcUnit in srcUnitList)
    {
      PlayerUnit unit = srcUnit;
      if (idList.FirstOrDefault<int>((Func<int, bool>) (x => x == unit.unit.same_character_id)) > 0)
        characterFilterList.Add(unit);
    }
    return characterFilterList;
  }

  private void DoUnitTypeTicketExclude(List<PlayerUnit> playerUnits)
  {
    UnitTypeTicketExclude typeTicketExclude = ((IEnumerable<UnitTypeTicketExclude>) MasterData.UnitTypeTicketExcludeList).FirstOrDefault<UnitTypeTicketExclude>((Func<UnitTypeTicketExclude, bool>) (x => x.ticket_id == this.ticket_));
    if (typeTicketExclude == null)
      return;
    string[] strArray = typeTicketExclude.ticket_type_param.Split(',');
    HashSet<int> source = new HashSet<int>();
    foreach (string s in strArray)
    {
      if (!string.IsNullOrEmpty(s))
      {
        int num = this.convertParamString(s);
        source.Add(num);
      }
    }
    HashSet<int> excludePlayerUnitIds = new HashSet<int>();
    foreach (PlayerUnit playerUnit in playerUnits)
    {
      UnitUnit unit = playerUnit.unit;
      int id = playerUnit.id;
      UnitGroup groupInfo;
      switch (typeTicketExclude.ticket_type_id)
      {
        case UnitTypeTicketType.all:
          excludePlayerUnitIds.Add(id);
          continue;
        case UnitTypeTicketType.character:
          if (source.Contains(unit.character_UnitCharacter))
          {
            excludePlayerUnitIds.Add(id);
            continue;
          }
          continue;
        case UnitTypeTicketType.unit:
          if (source.Contains(unit.ID))
          {
            excludePlayerUnitIds.Add(id);
            continue;
          }
          continue;
        case UnitTypeTicketType.kind:
          if (source.Contains(unit.kind.ID))
          {
            excludePlayerUnitIds.Add(id);
            continue;
          }
          continue;
        case UnitTypeTicketType.group_clothing:
          groupInfo = this.getGroupInfo(unit.ID);
          if (groupInfo != null && source.FirstOrDefault<int>((Func<int, bool>) (x => x == groupInfo.group_clothing_category_id_UnitGroupClothingCategory || x == groupInfo.group_clothing_category_id_2_UnitGroupClothingCategory)) > 0)
          {
            excludePlayerUnitIds.Add(id);
            continue;
          }
          continue;
        case UnitTypeTicketType.group_generation:
          groupInfo = this.getGroupInfo(unit.ID);
          if (groupInfo != null && source.Contains(groupInfo.group_generation_category_id_UnitGroupGenerationCategory))
          {
            excludePlayerUnitIds.Add(id);
            continue;
          }
          continue;
        case UnitTypeTicketType.group_large:
          groupInfo = this.getGroupInfo(unit.ID);
          if (groupInfo != null && source.Contains(groupInfo.group_large_category_id_UnitGroupLargeCategory))
          {
            excludePlayerUnitIds.Add(id);
            continue;
          }
          continue;
        case UnitTypeTicketType.group_small:
          groupInfo = this.getGroupInfo(unit.ID);
          if (groupInfo != null && source.Contains(groupInfo.group_small_category_id_UnitGroupSmallCategory))
          {
            excludePlayerUnitIds.Add(id);
            continue;
          }
          continue;
        case UnitTypeTicketType.element:
          int element = (int) unit.GetElement();
          if (source.Contains(element))
          {
            excludePlayerUnitIds.Add(id);
            continue;
          }
          continue;
        case UnitTypeTicketType.unit_type_category:
          if (source.Contains(playerUnit._unit_type))
          {
            excludePlayerUnitIds.Add(id);
            continue;
          }
          continue;
        case UnitTypeTicketType.job:
          if (source.Contains(unit.job_UnitJob))
          {
            excludePlayerUnitIds.Add(id);
            continue;
          }
          continue;
        case UnitTypeTicketType.same_character:
          if (source.Contains(unit.same_character_id))
          {
            excludePlayerUnitIds.Add(id);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
    playerUnits.RemoveAll((Predicate<PlayerUnit>) (x => excludePlayerUnitIds.Contains(x.id)));
  }

  private void ChangeScene(UnitIconBase unitIcon)
  {
    if (this.isPopupOpen)
      return;
    if (unitIcon.PlayerUnit != (PlayerUnit) null)
    {
      this.lastReferenceUnitID = unitIcon.PlayerUnit.id;
      this.lastReferenceUnitIndex = this.GetUnitInfoDisplayIndex(unitIcon.PlayerUnit);
      this.selectedPlayerUnit = unitIcon.PlayerUnit;
      this.StartCoroutine(this.openPopupTypeSelection(unitIcon.PlayerUnit));
      this.isPopupOpen = true;
    }
    else
      Debug.LogWarning((object) "PlayerUnit Null : Unit004ReincarnationTypeUnitSelectionMenu");
  }

  private IEnumerator openPopupTypeSelection(PlayerUnit PlayerUnit)
  {
    Unit004ReincarnationTypeUnitSelectionMenu menu = this;
    Future<GameObject> prefabF = new ResourceObject("Prefabs/unit004_Reincarnation_Type/popup_004_Reincarnation_Type_exchange_confirmation__anim_popup01").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    e = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Unit004ReincarnationTypeTicketPopupExchangeMenu>().coInitialize(menu.ticket_, PlayerUnit, menu, new Action<UnitTypeEnum, bool>(menu.OnUseSelected));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void OnUseSelected(UnitTypeEnum unitType, bool isDecision)
  {
    if (!isDecision)
      return;
    if (unitType == UnitTypeEnum.random)
      this.StartCoroutine(this.openPopupReincarnationTypeRandom());
    else
      Unit004ReincarnationTypeScene.changeScene(true, this.ticket_, this.selectedPlayerUnit, unitType);
  }

  private IEnumerator openPopupReincarnationTypeRandom()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004ReincarnationTypeUnitSelectionMenu unitSelectionMenu = this;
    Future<GameObject> prefabF2;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      Singleton<PopupManager>.GetInstance().open(prefabF2.Result).GetComponent<Popup004ReincarnationTypeMenu>().Init(unitSelectionMenu.selectedPlayerUnit.unit_type.name, "ランダム", new Action(unitSelectionMenu.reincarnationTypeRandom));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    prefabF2 = new ResourceObject("Prefabs/unit004_Reincarnation_Type/popup_004_Reincarnation_Type__anim_popup01").Load<GameObject>();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) prefabF2.Wait();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void reincarnationTypeRandom() => this.StartCoroutine(this.doReincarnationTypeRandom());

  private IEnumerator doReincarnationTypeRandom()
  {
    Unit004ReincarnationTypeUnitSelectionMenu unitSelectionMenu = this;
    if (unitSelectionMenu.selectedPlayerUnit.tower_is_entry || unitSelectionMenu.selectedPlayerUnit.corps_is_entry)
    {
      bool bReject = true;
      yield return (object) PopupManager.StartTowerEntryUnitWarningPopupProc((Action<bool>) (bYes => bReject = !bYes));
      if (bReject)
      {
        unitSelectionMenu.isPopupOpen = false;
        yield break;
      }
    }
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.UnittypeticketSpend> paramF = WebAPI.UnittypeticketSpend(unitSelectionMenu.selectedPlayerUnit.id, unitSelectionMenu.ticket_.ID, 0, (Action<WebAPI.Response.UserError>) (error =>
    {
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    yield return (object) paramF.Wait();
    if (paramF.Result != null)
    {
      Singleton<NGGameDataManager>.GetInstance().corps_player_unit_ids = new HashSet<int>((IEnumerable<int>) paramF.Result.corps_player_unit_ids);
      yield return (object) OnDemandDownload.WaitLoadHasUnitResource(false);
      // ISSUE: reference to a compiler-generated method
      PlayerUnit playerUnit = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).FirstOrDefault<PlayerUnit>(new Func<PlayerUnit, bool>(unitSelectionMenu.\u003CdoReincarnationTypeRandom\u003Eb__31_1));
      unit00497Scene.ChangeScene(false, new PrincesEvolutionParam()
      {
        materiaqlUnits = new List<PlayerUnit>(),
        is_new = false,
        baseUnit = unitSelectionMenu.selectedPlayerUnit,
        resultUnit = playerUnit,
        mode = Unit00499Scene.Mode.ReincarnationType
      });
      Singleton<NGSceneManager>.GetInstance().destroyScene("unit004_Reincarnation_Type");
    }
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
    this.allUnitIcons[unit_index].onClick = (Action<UnitIconBase>) (ui => this.ChangeScene(ui));
  }
}
