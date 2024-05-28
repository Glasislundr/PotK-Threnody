// Decompiled with JetBrains decompiler
// Type: Quest0528Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest0528Menu : BackButtonMenuBase
{
  private static readonly string chipExt = ".png__GUI__battle_mapchip__battle_mapchip_prefab";
  [SerializeField]
  private GameObject map_grid;
  [SerializeField]
  private NGHorizontalScrollParts unitInfoScroll;
  [SerializeField]
  private GameObject[] bottmoDir;
  [SerializeField]
  private UILabel txtWinningCondition;
  [SerializeField]
  private UILabel txtTotalTeamNum;
  [SerializeField]
  private UIButton ibtnBattleStart0028;
  private Quest0528Menu.BottomStatus bottomStatus;
  private GameObject battleMapChipSpritePrefab;
  private GameObject numPrefab;
  private GameObject normalPrefab;
  private GameObject gearKindPrefab;
  private GameObject unitDetailInfoPrefab;
  private GameObject battleSkillIconPrefab;
  private GameObject unitStatusPrefab;
  private GameObject skillDialogPrefab;
  private List<Quest0528Menu.FieldUnitInfo> fieldUnitInfoList = new List<Quest0528Menu.FieldUnitInfo>();
  private int unitInfoScrollPosition;
  private bool isSeEnableInit;
  private EarthExtraQuest extraQuest;

  private void SetScrollSeEnable(bool enable)
  {
    if (enable)
    {
      this.unitInfoScroll.SeEnable = false;
      this.StartCoroutine(this.WaitScrollSe(this.unitInfoScroll));
    }
    else
      this.unitInfoScroll.SeEnable = false;
  }

  private IEnumerator WaitScrollSe(NGHorizontalScrollParts obj)
  {
    yield return (object) new WaitForSeconds(0.5f);
    obj.SeEnable = true;
  }

  protected override void Update()
  {
    if (this.bottomStatus != Quest0528Menu.BottomStatus.BattleInfo || this.unitInfoScrollPosition == this.unitInfoScroll.selected)
      return;
    int index = this.unitInfoScroll.selected;
    if (this.unitInfoScroll.selected == -1)
      index = 0;
    this.EnableMapChipEffect(index);
    this.unitInfoScrollPosition = this.unitInfoScroll.selected;
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().clearStack();
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    Mypage051Scene.ChangeScene(false);
  }

  public void onBtnOrganization()
  {
    if (this.IsPushAndSet())
      return;
    int maxNum = -1;
    if (this.extraQuest != null)
      maxNum = this.extraQuest.stage.Players.Length;
    Quest0529Scene.ChangeScene(true, maxNum);
  }

  public void onBtnQuestStart()
  {
    if (this.IsPushAndSet())
      return;
    if (this.extraQuest == null)
      this.StartCoroutine(Singleton<EarthDataManager>.GetInstance().BattleInitStory());
    else
      this.StartCoroutine(Singleton<EarthDataManager>.GetInstance().BattleInitExtra(this.extraQuest));
  }

  public void onBtnDetai()
  {
    this.bottomStatus = Quest0528Menu.BottomStatus.BattleInfo;
    ((IEnumerable<GameObject>) this.bottmoDir).ToggleOnce((int) this.bottomStatus);
    this.EnableMapChipEffect(this.unitInfoScrollPosition);
    if (this.isSeEnableInit)
      return;
    this.isSeEnableInit = true;
    this.SetScrollSeEnable(true);
  }

  public void onBtnDetailClose()
  {
    this.bottomStatus = Quest0528Menu.BottomStatus.BattleStart;
    ((IEnumerable<GameObject>) this.bottmoDir).ToggleOnce((int) this.bottomStatus);
    this.DisableMapChipEffect();
  }

  public override void onBackButton() => this.IbtnBack();

  private void EnableMapChipEffect(int index)
  {
    this.fieldUnitInfoList.ForEach((Action<Quest0528Menu.FieldUnitInfo>) (x => x.mapchip.StopSelectAnim()));
    this.fieldUnitInfoList[index].mapchip.StartSelectAnim();
  }

  private void DisableMapChipEffect()
  {
    this.fieldUnitInfoList.ForEach((Action<Quest0528Menu.FieldUnitInfo>) (x => x.mapchip.StopSelectAnim()));
  }

  public IEnumerator Init(BattleStage stage)
  {
    ((UIButtonColor) this.ibtnBattleStart0028).isEnabled = true;
    if (this.extraQuest != null && Singleton<EarthDataManager>.GetInstance().GetEventQuestDeckCharacterCount() == 0)
      ((UIButtonColor) this.ibtnBattleStart0028).isEnabled = false;
    IEnumerator e = MasterData.LoadBattleStageEnemy(stage);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = MasterData.LoadBattleMapLandform(stage.map);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.txtWinningCondition.SetTextLocalize(stage.victory_condition.name);
    this.txtTotalTeamNum.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_0528_MAX_LABEL, (IDictionary) new Hashtable()
    {
      {
        (object) "Max",
        (object) stage.Players.Length
      }
    }));
    e = this.setupMapChipsWithInfoIndicator(stage);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.isSeEnableInit = false;
    this.SetScrollSeEnable(false);
  }

  public IEnumerator Init(EarthExtraQuest quest)
  {
    this.extraQuest = quest;
    IEnumerator e = this.Init(this.extraQuest.stage);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private Quest0528Menu.FieldUnitInfo getFieldPlayerUnit(
    EarthCharacter[] characters,
    BattleStage stage,
    int mapx,
    int mapy)
  {
    Quest0528Menu.FieldUnitInfo fieldPlayerUnit = (Quest0528Menu.FieldUnitInfo) null;
    BattleStagePlayer player = ((IEnumerable<BattleStagePlayer>) stage.Players).FirstOrDefault<BattleStagePlayer>((Func<BattleStagePlayer, bool>) (x => x.initial_coordinate_x - 1 == mapx && x.initial_coordinate_y - 1 == mapy));
    if (player != null)
    {
      fieldPlayerUnit = new Quest0528Menu.FieldUnitInfo();
      fieldPlayerUnit.deck_position = player.deck_position;
      fieldPlayerUnit.unit = (PlayerUnit) null;
      fieldPlayerUnit.unitType = BL.ForceID.player;
      fieldPlayerUnit.mapchip = (Quest0528MenuMapChipNum) null;
      EarthCharacter earthCharacter = ((IEnumerable<EarthCharacter>) characters).FirstOrDefault<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.battleIndex == player.deck_position));
      if (earthCharacter != null)
        fieldPlayerUnit.unit = earthCharacter.GetPlayerUnit();
    }
    return fieldPlayerUnit;
  }

  private Quest0528Menu.FieldUnitInfo getFieldEnemyUnit(BattleStage stage, int mapx, int mapy)
  {
    Quest0528Menu.FieldUnitInfo fieldEnemyUnit = (Quest0528Menu.FieldUnitInfo) null;
    BattleStageEnemy enemy = ((IEnumerable<BattleStageEnemy>) stage.Enemies).FirstOrDefault<BattleStageEnemy>((Func<BattleStageEnemy, bool>) (x => x.initial_coordinate_x - 1 == mapx && x.initial_coordinate_y - 1 == mapy));
    if (enemy != null && enemy.reinforcement == null)
    {
      fieldEnemyUnit = new Quest0528Menu.FieldUnitInfo();
      fieldEnemyUnit.deck_position = -1;
      fieldEnemyUnit.unitType = BL.ForceID.enemy;
      fieldEnemyUnit.unit = PlayerUnit.FromEnemy(enemy);
    }
    return fieldEnemyUnit;
  }

  private int getFieldPanel(BattleStage stage, int mapx, int mapy)
  {
    int fieldPanel = 1;
    int map_offset_x = stage.map_offset_x + mapx;
    int map_offset_y = stage.map_offset_y + mapy;
    BattleMapLandform battleMapLandform = Array.Find<BattleMapLandform>(MasterData.BattleMapLandformList, (Predicate<BattleMapLandform>) (x => stage.map_BattleMap == x.map_BattleMap && x.coordinate_x == map_offset_x && x.coordinate_y == map_offset_y));
    if (battleMapLandform != null)
      fieldPanel = battleMapLandform.landform_BattleLandform;
    return fieldPanel;
  }

  private IEnumerator setupMapChipsWithInfoIndicator(BattleStage stage)
  {
    this.fieldUnitInfoList.Clear();
    Future<GameObject> prefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.battleMapChipSpritePrefab, (Object) null))
    {
      prefabF = Res.Battle_Mapchip.BattleMapChipSprite.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.battleMapChipSpritePrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.numPrefab, (Object) null))
    {
      prefabF = Res.Prefabs.quest052_8.dir_team_num_for_mapchip.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.numPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    this.map_grid.transform.Clear();
    this.fieldUnitInfoList.Clear();
    int mapHeight = stage.map_height;
    int mapWidth = stage.map_width;
    UIWidget inParents = NGUITools.FindInParents<UIWidget>(this.map_grid);
    float num = (float) inParents.height / (float) mapHeight;
    int size = (int) Mathf.Min((float) inParents.width / (float) mapWidth, num);
    EarthCharacter[] characters = Singleton<EarthDataManager>.GetInstance().characters;
    for (int mapy = mapHeight - 1; mapy >= 0; --mapy)
    {
      for (int mapx = 0; mapx < mapWidth; ++mapx)
      {
        int number = 0;
        GameObject numPrefab = (GameObject) null;
        BL.ForceID unitType = BL.ForceID.player;
        Quest0528Menu.FieldUnitInfo fieldUnitInfo = this.getFieldPlayerUnit(characters, stage, mapx, mapy);
        string name;
        if (fieldUnitInfo != null)
        {
          name = "slc_mapchip_50";
          number = fieldUnitInfo.deck_position;
          numPrefab = this.numPrefab;
        }
        else
        {
          fieldUnitInfo = this.getFieldEnemyUnit(stage, mapx, mapy);
          if (fieldUnitInfo != null)
          {
            name = "slc_mapchip_51";
            unitType = BL.ForceID.enemy;
            numPrefab = this.numPrefab;
          }
          else
          {
            name = "slc_mapchip_" + (object) this.getFieldPanel(stage, mapx, mapy);
            unitType = BL.ForceID.player;
          }
        }
        Quest0528MenuMapChipNum quest0528MenuMapChipNum = this.cloneMapChip(name, size, this.battleMapChipSpritePrefab, number, unitType, numPrefab);
        if (fieldUnitInfo != null && fieldUnitInfo.unit != (PlayerUnit) null)
        {
          fieldUnitInfo.mapchip = quest0528MenuMapChipNum;
          this.fieldUnitInfoList.Add(fieldUnitInfo);
        }
      }
    }
    this.fieldUnitInfoList = this.fieldUnitInfoList.Where<Quest0528Menu.FieldUnitInfo>((Func<Quest0528Menu.FieldUnitInfo, bool>) (x => x.unitType != 0)).OrderByDescending<Quest0528Menu.FieldUnitInfo, BL.ForceID>((Func<Quest0528Menu.FieldUnitInfo, BL.ForceID>) (x => x.unitType)).ThenBy<Quest0528Menu.FieldUnitInfo, int>((Func<Quest0528Menu.FieldUnitInfo, int>) (x => x.deck_position)).ThenBy<Quest0528Menu.FieldUnitInfo, int>((Func<Quest0528Menu.FieldUnitInfo, int>) (x => x.unit.id)).ToList<Quest0528Menu.FieldUnitInfo>();
    UIGrid component = this.map_grid.GetComponent<UIGrid>();
    component.arrangement = (UIGrid.Arrangement) 0;
    component.maxPerLine = mapWidth;
    component.cellHeight = (float) size;
    component.cellWidth = (float) size;
    inParents.width = size * mapWidth;
    inParents.height = size * mapHeight;
    component.repositionNow = true;
    e = this.createBattleUnitInfo(this.fieldUnitInfoList);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private Quest0528MenuMapChipNum cloneMapChip(
    string name,
    int size,
    GameObject prefab,
    int number,
    BL.ForceID unitType,
    GameObject numPrefab)
  {
    Quest0528MenuMapChipNum quest0528MenuMapChipNum = (Quest0528MenuMapChipNum) null;
    UISprite component = prefab.CloneAndGetComponent<UISprite>(this.map_grid);
    component.spriteName = name + Quest0528Menu.chipExt;
    ((UIWidget) component).width = size;
    ((UIWidget) component).height = size;
    if (Object.op_Inequality((Object) numPrefab, (Object) null))
    {
      quest0528MenuMapChipNum = numPrefab.CloneAndGetComponent<Quest0528MenuMapChipNum>(((Component) component).gameObject);
      quest0528MenuMapChipNum.Init(number, size, unitType);
    }
    return quest0528MenuMapChipNum;
  }

  private IEnumerator createBattleUnitInfo(List<Quest0528Menu.FieldUnitInfo> unitInfoList)
  {
    Future<GameObject> prefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.unitStatusPrefab, (Object) null))
    {
      prefabF = Res.Prefabs.quest052_8.PlayerStatus.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.unitStatusPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.normalPrefab, (Object) null))
    {
      prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.normalPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.gearKindPrefab, (Object) null))
    {
      prefabF = Res.Icons.GearKindIcon.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.gearKindPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.unitDetailInfoPrefab, (Object) null))
    {
      prefabF = Res.Prefabs.quest052_8.PlayerStatusDetail.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.unitDetailInfoPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.battleSkillIconPrefab, (Object) null))
    {
      prefabF = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.battleSkillIconPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.skillDialogPrefab, (Object) null))
    {
      prefabF = Res.Prefabs.battle017_11_1_1.SkillDetailDialog.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.skillDialogPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    ((IEnumerable<GameObject>) this.bottmoDir).ToggleOnce(1);
    this.unitInfoScroll.destroyParts();
    this.unitInfoScrollPosition = 0;
    foreach (Quest0528Menu.FieldUnitInfo unitInfo in unitInfoList)
    {
      if (unitInfo.unit != (PlayerUnit) null)
      {
        e = this.unitInfoScroll.instantiateParts(this.unitStatusPrefab).GetComponent<Quest0528PlayerStatus>().Init(this.normalPrefab, this.gearKindPrefab, this.unitDetailInfoPrefab, this.battleSkillIconPrefab, this.skillDialogPrefab, unitInfo);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    this.unitInfoScroll.resetScrollView();
    ((IEnumerable<GameObject>) this.bottmoDir).ToggleOnce((int) this.bottomStatus);
  }

  public class FieldUnitInfo
  {
    public PlayerUnit unit;
    public int deck_position;
    public BL.ForceID unitType;
    public Quest0528MenuMapChipNum mapchip;
  }

  private enum BottomStatus
  {
    BattleStart,
    BattleInfo,
  }
}
