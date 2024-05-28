// Decompiled with JetBrains decompiler
// Type: Quest00214Menu
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
public class Quest00214Menu : BackButtonMenuBase
{
  [SerializeField]
  private UISprite labelFilterButton;
  [SerializeField]
  private NGxScroll2 ScrollContainerChara;
  [SerializeField]
  private NGxScroll ScrollContainerCombi;
  [SerializeField]
  private NGTweenParts headerCombi;
  [SerializeField]
  private NGTweenParts headerChara;
  [SerializeField]
  private NGTweenParts headerCommon;
  [SerializeField]
  private UIButton CombiButton;
  [SerializeField]
  private UIButton CharaButton;
  [SerializeField]
  private GameObject dirBlankIcons;
  private Queue<GameObject> blankIcons;
  public Quest00214aMenu CharaQuest;
  private List<Quest00214DirCombi> allQuestCombiCells = new List<Quest00214DirCombi>();
  private List<Quest00214DirTrio> allQuestTrioCells = new List<Quest00214DirTrio>();
  private CharacterQuestFilter.Calculator charaFilter;
  private bool isSelect;
  private UnitIcon[] unitIcons = new UnitIcon[0];
  private Dictionary<UnitIcon, GameObject> dirClearIcon = new Dictionary<UnitIcon, GameObject>();
  private UnitIconInfo[] allUnitInfos = new UnitIconInfo[0];
  private UnitIconInfo[] sortedUnitInfos = new UnitIconInfo[0];
  private bool isEndScene;
  private bool isInitialize;
  private bool isWaitSort;
  private bool isUpdateIcon;
  private int iconWidth;
  private int iconHeight;
  private int iconColumnValue;
  private int iconRowValue;
  private int iconScreenValue;
  private int iconMaxValue;
  private float scrool_start_y;
  private GameObject unitPrefab;
  private HashSet<int> newCharacterM;
  private HashSet<int> clearedCharacterM;
  private Dictionary<UnitIconInfo, Quest00214Menu.SubInfo> dicSubInfo = new Dictionary<UnitIconInfo, Quest00214Menu.SubInfo>();
  private List<int> unitIdList = new List<int>();
  private PlayerCharacterQuestM[] characters;
  private Quest00214Menu.SelectMode selectMode;
  private GameObject popupReleaseConditonPrefab;
  private GameObject dirCombiPrefab;
  private GameObject dirTrioPrefab;
  private GameObject dirClearPrefab;
  private GameObject filterPrefab;
  private GameObject filterPartsPrefab;
  private Dictionary<int, UnitIcon.SpriteCache> storedSpriteCache;
  private CharacterQuestFilter filterPopup;

  public WebAPI.Response.QuestProgressCharacter apiResponse { get; set; }

  public bool isLoadedResources { get; private set; }

  public IEnumerator doLoadResources()
  {
    this.isLoadedResources = false;
    Future<GameObject> popupReleaseConditonF;
    if (Object.op_Equality((Object) this.popupReleaseConditonPrefab, (Object) null))
    {
      popupReleaseConditonF = Res.Prefabs.popup.popup_002_14__chara_quest_release_condition__anim_popup01.Load<GameObject>();
      yield return (object) popupReleaseConditonF.Wait();
      this.popupReleaseConditonPrefab = popupReleaseConditonF.Result;
      popupReleaseConditonF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.dirCombiPrefab, (Object) null))
    {
      popupReleaseConditonF = Res.Prefabs.quest002_14.dir_Conbi.Load<GameObject>();
      yield return (object) popupReleaseConditonF.Wait();
      this.dirCombiPrefab = popupReleaseConditonF.Result;
      popupReleaseConditonF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.dirTrioPrefab, (Object) null))
    {
      popupReleaseConditonF = Res.Prefabs.quest002_14.dir_Trio.Load<GameObject>();
      yield return (object) popupReleaseConditonF.Wait();
      this.dirTrioPrefab = popupReleaseConditonF.Result;
      popupReleaseConditonF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.unitPrefab, (Object) null))
    {
      popupReleaseConditonF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      yield return (object) popupReleaseConditonF.Wait();
      this.unitPrefab = popupReleaseConditonF.Result;
      popupReleaseConditonF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.dirClearPrefab, (Object) null))
    {
      popupReleaseConditonF = new ResourceObject("Prefabs/UnitIcon/CharacterQuest").Load<GameObject>();
      yield return (object) popupReleaseConditonF.Wait();
      this.dirClearPrefab = popupReleaseConditonF.Result;
      popupReleaseConditonF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.filterPrefab, (Object) null))
    {
      popupReleaseConditonF = new ResourceObject("Prefabs/popup/popup_Unit_Sort_CharaQue__anim_popup01").Load<GameObject>();
      yield return (object) popupReleaseConditonF.Wait();
      this.filterPrefab = popupReleaseConditonF.Result;
      popupReleaseConditonF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.filterPartsPrefab, (Object) null))
    {
      popupReleaseConditonF = new ResourceObject("Prefabs/unit004_6/ibtn_Popup_Group").Load<GameObject>();
      yield return (object) popupReleaseConditonF.Wait();
      this.filterPartsPrefab = popupReleaseConditonF.Result;
      popupReleaseConditonF = (Future<GameObject>) null;
    }
    yield return (object) this.CharaQuest.doLoadResources();
    this.isLoadedResources = true;
  }

  public IEnumerator InitCharacterQuestButton(
    int? unitOrQuestId = null,
    bool isCurrentCombi = false,
    bool isSameUnit = false)
  {
    Quest00214Menu quest00214Menu = this;
    quest00214Menu.isEndScene = false;
    quest00214Menu.selectMode = isCurrentCombi ? Quest00214Menu.SelectMode.Combi : Quest00214Menu.SelectMode.Character;
    NGTweenParts[] ngTweenPartsArray = new NGTweenParts[3]
    {
      quest00214Menu.headerChara,
      quest00214Menu.headerCombi,
      quest00214Menu.headerCommon
    };
    foreach (NGTweenParts ngTweenParts in ngTweenPartsArray)
    {
      ngTweenParts.resetActive(false);
      ((Component) ngTweenParts).GetComponent<UIRect>().alpha = 0.0f;
    }
    EventDelegate.Set(quest00214Menu.CharaQuest.onClose, new EventDelegate.Callback(quest00214Menu.onCloseQuestMenu));
    EventDelegate.Set(quest00214Menu.CharaQuest.onCloseFinished, new EventDelegate.Callback(quest00214Menu.onCloseFinishedQuestMenu));
    ((Component) quest00214Menu.CharaQuest).gameObject.SetActive(false);
    ((Component) quest00214Menu.ScrollContainerChara).gameObject.SetActive(true);
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<WebAPI.Response.QuestProgressCharacter> apiF = WebAPI.QuestProgressCharacter((Action<WebAPI.Response.UserError>) (error =>
    {
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    e = apiF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (apiF.Result != null)
    {
      WebAPI.SetLatestResponsedAt("QuestProgressCharacter");
      WebAPI.SetLatestResponsedAt("QuestProgressHarmony");
      quest00214Menu.apiResponse = apiF.Result;
      quest00214Menu.characters = SMManager.Get<PlayerCharacterQuestM[]>();
      quest00214Menu.InitializeStart();
      PlayerHarmonyQuestM[] harmonies = SMManager.Get<PlayerHarmonyQuestM[]>();
      PlayerCharacterQuestS[] playerCharacterQuestSArray = SMManager.Get<PlayerCharacterQuestS[]>().SelectReleased();
      PlayerHarmonyQuestS[] harmonyS = SMManager.Get<PlayerHarmonyQuestS[]>().SelectReleased();
      quest00214Menu.characters = ((IEnumerable<PlayerCharacterQuestM>) quest00214Menu.characters).OrderByDescending<PlayerCharacterQuestM, int>((Func<PlayerCharacterQuestM, int>) (x =>
      {
        QuestCharacterM questMId = x.quest_m_id;
        return questMId == null ? 0 : questMId.priority;
      })).ToArray<PlayerCharacterQuestM>();
      quest00214Menu.SetIconType();
      quest00214Menu.allUnitInfos = new UnitIconInfo[0];
      quest00214Menu.sortedUnitInfos = new UnitIconInfo[0];
      quest00214Menu.dirClearIcon.Clear();
      quest00214Menu.dicSubInfo.Clear();
      quest00214Menu.charaFilter = new CharacterQuestFilter.Calculator(Persist.characterQuestFilterInfo);
      quest00214Menu.setLabelFilterButton();
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      quest00214Menu.newCharacterM = new HashSet<int>();
      foreach (PlayerCharacterQuestS playerCharacterQuestS in playerCharacterQuestSArray)
      {
        int mQuestCharacterM = playerCharacterQuestS.quest_character_s.quest_m_QuestCharacterM;
        if (playerCharacterQuestS.is_new)
          quest00214Menu.newCharacterM.Add(mQuestCharacterM);
        if (playerCharacterQuestS.is_clear)
        {
          if (dictionary.ContainsKey(mQuestCharacterM))
            dictionary[mQuestCharacterM]++;
          else
            dictionary[mQuestCharacterM] = 1;
        }
      }
      quest00214Menu.clearedCharacterM = new HashSet<int>();
      foreach (KeyValuePair<int, int> keyValuePair in dictionary)
      {
        KeyValuePair<int, int> p = keyValuePair;
        if (p.Value == ((IEnumerable<QuestCharacterS>) MasterData.QuestCharacterSList).Count<QuestCharacterS>((Func<QuestCharacterS, bool>) (x => x.quest_m_QuestCharacterM == p.Key)))
          quest00214Menu.clearedCharacterM.Add(p.Key);
      }
      quest00214Menu.unitIdList = Enumerable.Range(0, quest00214Menu.characters.Length).Select<int, int>((Func<int, int>) (i => i)).ToList<int>();
      List<int> createUnitIdList = quest00214Menu.unitIdList.GetRange(0, quest00214Menu.characters.Length);
      while (!quest00214Menu.isLoadedResources)
        yield return (object) null;
      UnitIcon.ClearCache();
      quest00214Menu.CreateCharacterInfo(quest00214Menu.characters, createUnitIdList);
      yield return (object) quest00214Menu.doResetUnitIcons(false);
      quest00214Menu.storedSpriteCache = UnitIcon.CopyCache();
      QuestHarmonyS[] questHarmonySList = MasterData.QuestHarmonySList;
      PlayerHarmonyQuestM[] playerHarmonyQuestMArray = harmonies;
      for (int index = 0; index < playerHarmonyQuestMArray.Length; ++index)
      {
        PlayerHarmonyQuestM harmony = playerHarmonyQuestMArray[index];
        QuestHarmonyS questSTable = Array.Find<QuestHarmonyS>(questHarmonySList, (Predicate<QuestHarmonyS>) (x => x.quest_m_QuestHarmonyM == harmony.quest_m_id.ID));
        if (questSTable != null)
        {
          if (questSTable.target_unit2 == null)
          {
            GameObject gameObject = quest00214Menu.dirCombiPrefab.Clone();
            quest00214Menu.ScrollContainerCombi.Add(gameObject);
            Quest00214DirCombi questCell = gameObject.GetComponent<Quest00214DirCombi>();
            e = questCell.Init(questSTable, quest00214Menu.apiResponse.player_harmony_quests, new Action<int, int>(quest00214Menu.SelectHarmony));
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            quest00214Menu.allQuestCombiCells.Add(questCell);
            questCell = (Quest00214DirCombi) null;
          }
          else
          {
            GameObject gameObject = quest00214Menu.dirTrioPrefab.Clone();
            quest00214Menu.ScrollContainerCombi.Add(gameObject);
            Quest00214DirTrio questCellTrio = gameObject.GetComponent<Quest00214DirTrio>();
            e = questCellTrio.Init(questSTable, harmony.is_playable, quest00214Menu.apiResponse.player_harmony_quests, new Action<int, int[]>(quest00214Menu.SelectHarmony));
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            quest00214Menu.allQuestTrioCells.Add(questCellTrio);
            questCellTrio = (Quest00214DirTrio) null;
          }
        }
      }
      playerHarmonyQuestMArray = (PlayerHarmonyQuestM[]) null;
      quest00214Menu.ScrollContainerCombi.ResolvePosition();
      quest00214Menu.ScrollContainerChara.ResolvePosition();
      quest00214Menu.isSelect = false;
      quest00214Menu.IconEnable = true;
      if (unitOrQuestId.HasValue)
      {
        if (isCurrentCombi)
          quest00214Menu.setFocusHarmony(harmonies, harmonyS, unitOrQuestId.Value);
        else
          quest00214Menu.setFocusCharacter(unitOrQuestId.Value, isSameUnit);
      }
      yield return (object) null;
    }
  }

  private void setFocusCharacter(int Id, bool isSameUnit)
  {
    Func<UnitIconInfo, bool> checkFunc = isSameUnit ? (Func<UnitIconInfo, bool>) (x => x.unit.same_character_id == Id) : (Func<UnitIconInfo, bool>) (x => x.unit.ID == Id);
    int? nullable = ((IEnumerable<UnitIconInfo>) this.sortedUnitInfos).FirstIndexOrNull<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => checkFunc(x)));
    if (nullable.HasValue)
    {
      this.SelectCharacter(this.sortedUnitInfos[nullable.Value].unit.ID);
      this.ScrollContainerChara.ResolvePosition(nullable.Value, this.sortedUnitInfos.Length);
    }
    else
    {
      UnitIconInfo unitIconInfo = Array.Find<UnitIconInfo>(this.allUnitInfos, (Predicate<UnitIconInfo>) (x => checkFunc(x)));
      if (unitIconInfo == null)
        return;
      this.SelectCharacter(unitIconInfo.unit.ID);
    }
  }

  private void setFocusHarmony(
    PlayerHarmonyQuestM[] harmonyM,
    PlayerHarmonyQuestS[] harmonyS,
    int questId)
  {
    PlayerHarmonyQuestS playerHarmonyQuestS = Array.Find<PlayerHarmonyQuestS>(harmonyS, (Predicate<PlayerHarmonyQuestS>) (s => s._quest_harmony_s == questId));
    if (playerHarmonyQuestS == null)
      return;
    QuestHarmonyS qs = playerHarmonyQuestS.quest_harmony_s;
    if (qs.target_unit2_UnitUnit.HasValue)
      this.SelectHarmony(qs.unit_UnitUnit, new int[2]
      {
        qs.target_unit_UnitUnit,
        qs.target_unit2_UnitUnit.Value
      });
    else
      this.SelectHarmony(qs.unit_UnitUnit, qs.target_unit_UnitUnit);
    int? nullable = ((IEnumerable<PlayerHarmonyQuestM>) harmonyM).FirstIndexOrNull<PlayerHarmonyQuestM>((Func<PlayerHarmonyQuestM, bool>) (m => m._quest_m_id == qs.quest_m_QuestHarmonyM));
    if (!nullable.HasValue)
      return;
    this.ScrollContainerCombi.ResolvePosition(nullable.Value);
  }

  private void SelectCharacter(int unitId)
  {
    this.isSelect = true;
    this.IconEnable = false;
    this.setActiveHeader(false);
    ((Component) this.CharaQuest).gameObject.SetActive(true);
    this.StartCoroutine(this.CharaQuest.Init(unitId, this.apiResponse));
  }

  private void SelectHarmony(int unitId, int targetUnitId)
  {
    this.isSelect = true;
    this.CellEnable = false;
    this.setActiveHeader(false);
    ((Component) this.CharaQuest).gameObject.SetActive(true);
    int[] targetUnitIds = new int[1]{ targetUnitId };
    this.StartCoroutine(this.CharaQuest.Init(unitId, targetUnitIds, this.apiResponse, false));
  }

  private void SelectHarmony(int unitId, int[] targetUnitIds)
  {
    this.isSelect = true;
    this.CellEnable = false;
    this.setActiveHeader(false);
    ((Component) this.CharaQuest).gameObject.SetActive(true);
    this.StartCoroutine(this.CharaQuest.Init(unitId, targetUnitIds, this.apiResponse, true));
  }

  private void onCloseQuestMenu() => this.isSelect = false;

  private void onCloseFinishedQuestMenu()
  {
    this.setActiveHeader(true);
    if (this.selectMode == Quest00214Menu.SelectMode.Character)
      this.IconEnable = true;
    else
      this.CellEnable = true;
  }

  private bool IconEnable
  {
    set
    {
      foreach (UnitIcon unitIcon in this.unitIcons)
        ((Collider) unitIcon.buttonBoxCollider).enabled = value;
      ((Behaviour) this.CombiButton).enabled = value;
    }
  }

  private bool CellEnable
  {
    set
    {
      foreach (Quest00214DirCombi allQuestCombiCell in this.allQuestCombiCells)
        ((Collider) allQuestCombiCell.buttonBoxCollider).enabled = value;
      foreach (Quest00214DirTrio allQuestTrioCell in this.allQuestTrioCells)
        ((Collider) allQuestTrioCell.buttonBoxCollider).enabled = value;
      ((Behaviour) this.CharaButton).enabled = value;
    }
  }

  public void IbtnCombi()
  {
    this.selectMode = Quest00214Menu.SelectMode.Combi;
    this.changeHeader();
    ((Component) this.ScrollContainerCombi).gameObject.SetActive(true);
    ((Component) this.ScrollContainerChara).gameObject.SetActive(false);
  }

  public void IbtnChara()
  {
    this.selectMode = Quest00214Menu.SelectMode.Character;
    this.changeHeader();
    ((Component) this.ScrollContainerCombi).gameObject.SetActive(false);
    ((Component) this.ScrollContainerChara).gameObject.SetActive(true);
  }

  public void setActiveHeader(bool bAct)
  {
    if (this.selectMode == Quest00214Menu.SelectMode.Character)
      this.headerChara.isActive = bAct;
    else
      this.headerCombi.isActive = bAct;
    this.headerCommon.isActive = bAct;
  }

  private void changeHeader()
  {
    this.StopCoroutine("doDelayChangeHeader");
    if (this.selectMode == Quest00214Menu.SelectMode.Character)
      this.StartCoroutine("doDelayChangeHeader", (object) false);
    else
      this.StartCoroutine("doDelayChangeHeader", (object) true);
  }

  private IEnumerator doDelayChangeHeader(bool toCombi)
  {
    if (toCombi)
      this.headerChara.isActive = false;
    else
      this.headerCombi.isActive = false;
    yield return (object) new WaitForSeconds(0.3f);
    if (!this.isEndScene)
    {
      if (toCombi)
        this.headerCombi.isActive = true;
      else
        this.headerChara.isActive = true;
    }
  }

  public override void onBackButton()
  {
    if (((Component) this.CharaQuest).gameObject.activeSelf)
    {
      this.CharaQuest.onBackButton();
    }
    else
    {
      if (this.isSelect || this.IsPushAndSet())
        return;
      this.isEndScene = true;
      this.backScene();
    }
  }

  protected override void Update()
  {
    if (this.isEndScene)
      return;
    base.Update();
  }

  private void FixedUpdate()
  {
    if (this.isEndScene)
      return;
    this.ScrollUpdate();
  }

  private void InitializeStart()
  {
    this.isInitialize = false;
    this.ScrollContainerChara.Clear();
    this.ScrollContainerCombi.Clear();
  }

  public void InitializeEnd()
  {
    if (!this.isSelect)
    {
      if (this.selectMode == Quest00214Menu.SelectMode.Character)
      {
        this.headerChara.isActive = true;
        ((Component) this.ScrollContainerCombi).gameObject.SetActive(false);
        ((Component) this.ScrollContainerChara).gameObject.SetActive(true);
      }
      else
      {
        this.headerCombi.isActive = true;
        ((Component) this.ScrollContainerCombi).gameObject.SetActive(true);
        ((Component) this.ScrollContainerChara).gameObject.SetActive(false);
      }
      this.headerCommon.isActive = true;
    }
    else if (this.selectMode == Quest00214Menu.SelectMode.Character)
    {
      ((Component) this.ScrollContainerCombi).gameObject.SetActive(false);
      ((Component) this.ScrollContainerChara).gameObject.SetActive(true);
    }
    else
    {
      ((Component) this.ScrollContainerCombi).gameObject.SetActive(true);
      ((Component) this.ScrollContainerChara).gameObject.SetActive(false);
    }
    this.isInitialize = true;
  }

  public void SetIconType()
  {
    this.iconWidth = UnitIcon.Width;
    this.iconHeight = UnitIcon.Height;
    this.iconColumnValue = UnitIcon.ColumnValue;
    this.iconRowValue = UnitIcon.RowValue;
    this.iconScreenValue = UnitIcon.ScreenValue;
    this.iconMaxValue = UnitIcon.MaxValue;
  }

  private void ScrollUpdate()
  {
    if (this.isWaitSort || (!this.isInitialize || this.sortedUnitInfos.Length <= this.iconScreenValue) && !this.isUpdateIcon)
      return;
    int num1 = this.iconHeight * 2;
    float num2 = ((Component) this.ScrollContainerChara.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.sortedUnitInfos.Length - this.iconScreenValue - 1) / this.iconColumnValue * this.iconHeight);
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
        foreach (GameObject gameObject in this.ScrollContainerChara)
        {
          GameObject obj = gameObject;
          float num5 = obj.transform.localPosition.y + num2;
          if ((double) num5 > (double) num1)
          {
            int? nullable = ((IEnumerable<UnitIconInfo>) this.sortedUnitInfos).FirstIndexOrNull<UnitIconInfo>((Func<UnitIconInfo, bool>) (v => Object.op_Inequality((Object) v.icon, (Object) null) && Object.op_Equality((Object) ((Component) v.icon).gameObject, (Object) obj)));
            int info_index = nullable.HasValue ? nullable.Value + this.iconMaxValue : (this.sortedUnitInfos.Length + 4) / 5 * 5;
            if (nullable.HasValue && info_index < (this.sortedUnitInfos.Length + 4) / 5 * 5)
            {
              obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y - num4, 0.0f);
              if (info_index >= this.sortedUnitInfos.Length)
                obj.SetActive(false);
              else
                this.ScrollIconUpdate(info_index, unit_index);
              flag = true;
            }
          }
          else if ((double) num5 < -((double) num4 - (double) num1))
          {
            int num6 = this.iconMaxValue;
            if (!obj.activeSelf)
            {
              obj.SetActive(true);
              num6 = 0;
            }
            int? nullable = ((IEnumerable<UnitIconInfo>) this.sortedUnitInfos).FirstIndexOrNull<UnitIconInfo>((Func<UnitIconInfo, bool>) (v => Object.op_Inequality((Object) v.icon, (Object) null) && Object.op_Equality((Object) ((Component) v.icon).gameObject, (Object) obj)));
            int info_index = nullable.HasValue ? nullable.Value - num6 : -1;
            if (nullable.HasValue && info_index >= 0)
            {
              obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y + num4, 0.0f);
              this.ScrollIconUpdate(info_index, unit_index);
              flag = true;
            }
          }
          ++unit_index;
        }
        if (!flag)
          goto label_1;
      }
      while (!this.isUpdateIcon);
      this.isUpdateIcon = false;
    }
label_1:;
  }

  private void ScrollIconUpdate(int info_index, int unit_index)
  {
    this.ResetUnitIcon(unit_index);
    if (UnitIcon.IsCache(this.sortedUnitInfos[info_index].unit))
      this.CreateUnitIconCache(info_index, unit_index);
    else
      this.StartCoroutine(this.CreateUnitIcon(info_index, unit_index));
  }

  private void ResetUnitIcon(int index)
  {
    if (this.unitIcons.Length == 0)
      return;
    UnitIcon unitIcon = this.unitIcons[index];
    unitIcon.ResetUnit();
    ((Component) unitIcon).gameObject.SetActive(false);
    foreach (UnitIconInfo unitIconInfo in ((IEnumerable<UnitIconInfo>) this.sortedUnitInfos).Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (a => Object.op_Equality((Object) a.icon, (Object) unitIcon))))
      unitIconInfo.icon = (UnitIconBase) null;
  }

  private IEnumerator doResetUnitIcons(bool bLoadAsync = true)
  {
    Quest00214Menu quest00214Menu = this;
    quest00214Menu.resetUnitIconBlank();
    quest00214Menu.ScrollContainerChara.ResetScrollRange(quest00214Menu.iconWidth, quest00214Menu.iconHeight, quest00214Menu.sortedUnitInfos.Length);
    quest00214Menu.ScrollContainerChara.ResolvePosition();
    int maxIcon = Mathf.Min(quest00214Menu.iconMaxValue, quest00214Menu.sortedUnitInfos.Length);
    quest00214Menu.unitIcons = new UnitIcon[maxIcon];
    for (int index = 0; index < maxIcon; ++index)
      quest00214Menu.unitIcons[index] = quest00214Menu.restoreUnitIcon(index);
    if (quest00214Menu.sortedUnitInfos.Length != 0)
    {
      quest00214Menu.scrool_start_y = ((Component) quest00214Menu.ScrollContainerChara.scrollView).transform.localPosition.y;
      for (int i = 0; i < maxIcon; ++i)
      {
        if (UnitIcon.IsCache(quest00214Menu.sortedUnitInfos[i].unit))
          quest00214Menu.CreateUnitIconCache(i, i, false);
        else if (bLoadAsync)
          quest00214Menu.StartCoroutine(quest00214Menu.CreateUnitIcon(i, i, false));
        else
          yield return (object) quest00214Menu.CreateUnitIcon(i, i, false);
      }
    }
  }

  private UnitIcon restoreUnitIcon(int index)
  {
    GameObject gameObject = this.blankIcons.Dequeue();
    UnitIcon component = gameObject.GetComponent<UnitIcon>();
    this.dirClearIcon[component].SetActive(false);
    component.ResetUnit();
    this.ScrollContainerChara.Add(gameObject, this.iconWidth, this.iconHeight, index);
    return component;
  }

  private void resetUnitIconBlank()
  {
    if (this.blankIcons == null)
    {
      this.blankIcons = new Queue<GameObject>(this.iconMaxValue);
      for (int index = 0; index < this.iconMaxValue; ++index)
      {
        GameObject gameObject1 = this.unitPrefab.Clone(this.dirBlankIcons.transform);
        GameObject gameObject2 = this.dirClearPrefab.Clone(gameObject1.transform);
        UnitIcon component = gameObject1.GetComponent<UnitIcon>();
        component.princessType.DispPrincessType(false);
        gameObject1.SetActive(false);
        gameObject2.SetActive(false);
        this.blankIcons.Enqueue(gameObject1);
        this.dirClearIcon.Add(component, gameObject2);
      }
    }
    else
    {
      for (int index = 0; index < this.unitIcons.Length; ++index)
      {
        GameObject gameObject = ((Component) this.unitIcons[index]).gameObject;
        gameObject.SetActive(false);
        gameObject.transform.parent = this.dirBlankIcons.transform;
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.localPosition = Vector3.zero;
        this.blankIcons.Enqueue(gameObject);
      }
      foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
        allUnitInfo.icon = (UnitIconBase) null;
      this.ScrollContainerChara.Reset();
    }
  }

  private IEnumerator CreateUnitIcon(int info_index, int unit_index, bool bInitLink = true)
  {
    UnitIcon unitIcon = this.unitIcons[unit_index];
    UnitIconInfo info = this.sortedUnitInfos[info_index];
    if (bInitLink)
    {
      foreach (UnitIconInfo unitIconInfo in ((IEnumerable<UnitIconInfo>) this.allUnitInfos).Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (a => Object.op_Equality((Object) a.icon, (Object) unitIcon))))
        unitIconInfo.icon = (UnitIconBase) null;
    }
    if (info.is_unknown)
    {
      unitIcon.UnknownUnit();
    }
    else
    {
      UnitUnit unit = info.unit;
      IEnumerator e = unitIcon.SetUnit(unit, unit.GetElement(), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.resetInfo(info, unitIcon);
  }

  private void CreateUnitIconCache(int info_index, int unit_index, bool bInitLink = true)
  {
    UnitIcon unitIcon = this.unitIcons[unit_index];
    UnitIconInfo sortedUnitInfo = this.sortedUnitInfos[info_index];
    if (bInitLink)
    {
      foreach (UnitIconInfo unitIconInfo in ((IEnumerable<UnitIconInfo>) this.allUnitInfos).Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (a => Object.op_Equality((Object) a.icon, (Object) unitIcon))))
        unitIconInfo.icon = (UnitIconBase) null;
    }
    if (sortedUnitInfo.is_unknown)
      unitIcon.UnknownUnit();
    else
      unitIcon.SetUnitCache(sortedUnitInfo.unit, sortedUnitInfo.unit.GetElement());
    this.resetInfo(sortedUnitInfo, unitIcon);
  }

  private void resetInfo(UnitIconInfo info, UnitIcon icon)
  {
    info.icon = (UnitIconBase) icon;
    icon.Gray = info.gray;
    if (info.callback != null)
      icon.onClick = (Action<UnitIconBase>) (x => info.callback());
    else
      icon.onClick = (Action<UnitIconBase>) (_ => { });
    this.dirClearIcon[icon].SetActive(this.dicSubInfo[info].isClear);
    icon.NewUnit = info.is_new;
    icon.RarityCenter();
    icon.BottomModeValue = UnitIconBase.GetBottomMode(info.unit, (PlayerUnit) null);
    ((Component) icon).gameObject.SetActive(true);
  }

  private void CreateCharacterInfo(PlayerCharacterQuestM[] characters, List<int> createId)
  {
    this.allUnitInfos = new UnitIconInfo[createId.Count];
    int num = 0;
    foreach (int index in createId)
    {
      UnitIconInfo unitIconInfo = this.SetUnitIconInfo(new UnitIconInfo(), characters[index]);
      this.allUnitInfos[num++] = unitIconInfo;
    }
    this.sortedUnitInfos = this.charaFilter.filterBy(this.allUnitInfos, new List<int>());
  }

  private UnitIconInfo SetUnitIconInfo(UnitIconInfo iconInfo, PlayerCharacterQuestM questM)
  {
    UnitIconInfo unitIconInfo = iconInfo;
    bool isPlayable = questM.is_playable;
    QuestCharacterM quest_m = questM.quest_m_id;
    QuestCharacterMReleaseCondition questCondition = Array.Find<QuestCharacterMReleaseCondition>(MasterData.QuestCharacterMReleaseConditionList, (Predicate<QuestCharacterMReleaseCondition>) (x => x.quest_m_QuestCharacterM == quest_m.ID));
    UnitUnit unit = questM.unit_id;
    bool flag = this.newCharacterM.Contains(quest_m.ID);
    unitIconInfo.unit = unit;
    this.dicSubInfo[iconInfo] = new Quest00214Menu.SubInfo(this.clearedCharacterM.Contains(quest_m.ID));
    if (questCondition == null | isPlayable)
    {
      unitIconInfo.is_new = flag;
      if (!isPlayable)
        unitIconInfo.gray = true;
      unitIconInfo.callback = (Action) (() => this.SelectCharacter(unit.ID));
    }
    else
    {
      unitIconInfo.is_unknown = true;
      unitIconInfo.callback = (Action) (() => Singleton<PopupManager>.GetInstance().open(this.popupReleaseConditonPrefab).GetComponent<Quest00214PopupReleaseCondition>().Init(questCondition.required_condition));
    }
    return unitIconInfo;
  }

  public void onClickedSort()
  {
    if (this.IsPushAndSet())
      return;
    this.filterPopup = Singleton<PopupManager>.GetInstance().open(this.filterPrefab, isNonSe: true, isNonOpenAnime: true).GetComponent<CharacterQuestFilter>();
    this.filterPopup.Initialize(this.filterPartsPrefab, this.charaFilter, new Action(this.filtered), new Action(this.changeFilterSetting));
  }

  private void filtered()
  {
    this.setLabelFilterButton();
    Singleton<PopupManager>.GetInstance().dismiss();
    this.IsPush = true;
    Singleton<PopupManager>.GetInstance().open((GameObject) null);
    this.StartCoroutine(this.doFinishedFilter());
  }

  private IEnumerator doFinishedFilter()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Quest00214Menu quest00214Menu = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      Singleton<PopupManager>.GetInstance().dismiss();
      quest00214Menu.isWaitSort = false;
      quest00214Menu.IsPush = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    quest00214Menu.isWaitSort = true;
    quest00214Menu.sortedUnitInfos = quest00214Menu.charaFilter.filterBy(quest00214Menu.allUnitInfos);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) quest00214Menu.doResetUnitIcons();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void changeFilterSetting()
  {
    this.filterPopup.SetUnitNum(this.filterPopup.filterBy(this.allUnitInfos), this.allUnitInfos);
  }

  public void preLoadIcons()
  {
    if (this.storedSpriteCache != null && this.storedSpriteCache.Any<KeyValuePair<int, UnitIcon.SpriteCache>>())
      UnitIcon.RestoreCache(this.storedSpriteCache);
    this.storedSpriteCache = (Dictionary<int, UnitIcon.SpriteCache>) null;
    List<UnitUnit> source = new List<UnitUnit>(this.allUnitInfos.Length);
    for (int index = 0; index < this.allUnitInfos.Length; ++index)
    {
      UnitUnit unit = this.allUnitInfos[index].unit;
      if (!UnitIcon.IsCache(unit))
        source.Add(unit);
    }
    if (!source.Any<UnitUnit>())
      return;
    this.StartCoroutine("doPreLoadIcons", (object) source);
  }

  private IEnumerator doPreLoadIcons(List<UnitUnit> units)
  {
    int num = units.Count;
    for (int n = 0; n < num; ++n)
      yield return (object) UnitIcon.LoadSprite(units[n]);
  }

  private void setLabelFilterButton()
  {
    string str1 = "filter_";
    List<int> selectedGroupId1 = this.charaFilter.selectedGroupIDs[UnitGroupHead.group_all];
    List<int> selectedGroupId2 = this.charaFilter.selectedGroupIDs[UnitGroupHead.group_large];
    List<int> selectedGroupId3 = this.charaFilter.selectedGroupIDs[UnitGroupHead.group_small];
    List<int> selectedGroupId4 = this.charaFilter.selectedGroupIDs[UnitGroupHead.group_clothing];
    List<int> selectedGroupId5 = this.charaFilter.selectedGroupIDs[UnitGroupHead.group_generation];
    string str2;
    if (selectedGroupId1.Contains(1))
      str2 = str1 + "group_all";
    else if (selectedGroupId1.Contains(2))
    {
      str2 = str1 + "group_none";
    }
    else
    {
      int count1 = selectedGroupId2.Count;
      int count2 = selectedGroupId3.Count;
      int count3 = selectedGroupId4.Count;
      int count4 = selectedGroupId5.Count;
      str2 = count1 != 1 || count2 != 0 || count3 != 0 || count4 != 0 ? (count1 != 0 || count2 != 1 || count3 != 0 || count4 != 0 ? (count1 != 0 || count2 != 0 || count3 != 1 || count4 != 0 ? (count1 != 0 || count2 != 0 || count3 != 0 || count4 != 1 ? str1 + "group_multiple" : string.Format("generation_{0}", (object) selectedGroupId5[0])) : string.Format("dresses_{0}", (object) selectedGroupId4[0])) : string.Format("s_classification_{0}", (object) selectedGroupId3[0])) : string.Format("l_classification_{0}", (object) selectedGroupId2[0]);
    }
    UISpriteData sprite = this.labelFilterButton.atlas.GetSprite("slc_Label_" + str2 + "__GUI__unit_sort_label__unit_sort_label_prefab");
    if (sprite != null)
    {
      this.labelFilterButton.spriteName = sprite.name;
      ((UIWidget) this.labelFilterButton).width = sprite.width;
      ((UIWidget) this.labelFilterButton).height = sprite.height;
      ((Component) this.labelFilterButton).gameObject.SetActive(true);
    }
    else
      ((Component) this.labelFilterButton).gameObject.SetActive(false);
  }

  private enum SelectMode
  {
    Character,
    Combi,
  }

  private class SubInfo
  {
    public bool isClear { get; private set; }

    public SubInfo(bool bClear) => this.isClear = bClear;
  }
}
