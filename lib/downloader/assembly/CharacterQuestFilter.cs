// Decompiled with JetBrains decompiler
// Type: CharacterQuestFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/quest002_14/CharacterQuestFilter")]
public class CharacterQuestFilter : SortAndFilter
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UIGrid groupLargeGrid;
  [SerializeField]
  private Transform groupLargeTitle;
  [SerializeField]
  private UIGrid groupSmallGrid;
  [SerializeField]
  private Transform groupSmallTitle;
  [SerializeField]
  private UIGrid groupClothingGrid;
  [SerializeField]
  private Transform groupClothingTitle;
  [SerializeField]
  private UIGrid groupGenerationGrid;
  [SerializeField]
  private Transform groupGenerationTitle;
  [SerializeField]
  private UIScrollView groupButtonScrollView;
  [SerializeField]
  private GameObject[] DisplayList;
  [SerializeField]
  private UILabel unitGroupNum;
  private CharacterQuestFilter.Calculator calculator_;
  private Action onFilter;
  private Action onSample;
  private GameObject buttonPrefab;

  public Dictionary<UnitGroupHead, List<int>> selectedGroupIDs { get; private set; }

  private Dictionary<UnitGroupHead, List<int>> allGroupIDs => this.calculator_.allGroupIDs;

  private Persist<Persist.CharacterQuestFilterInfo> persist => this.calculator_.persist;

  private GameObject createGroupBtn(
    GameObject prefab,
    UnitGroupHead type,
    int id,
    string title,
    string spriteName)
  {
    GameObject groupBtn = Object.Instantiate<GameObject>(prefab);
    groupBtn.GetComponent<UnitSortAndFilterGroupButton>().Init(new Action<UnitGroupHead, int, bool>(this.switchGroupInfo), type, id, title, spriteName);
    return groupBtn;
  }

  private IEnumerator CreateGroupList()
  {
    CharacterQuestFilter characterQuestFilter = this;
    foreach (UnitGroupLargeCategory groupLargeCategory in ((IEnumerable<UnitGroupLargeCategory>) MasterData.UnitGroupLargeCategoryList).Where<UnitGroupLargeCategory>((Func<UnitGroupLargeCategory, bool>) (x =>
    {
      DateTime? startAt = x.start_at;
      DateTime dateTime = ServerTime.NowAppTime();
      return startAt.HasValue && startAt.GetValueOrDefault() < dateTime;
    })))
    {
      if (!string.IsNullOrEmpty(groupLargeCategory.name))
      {
        GameObject groupBtn = characterQuestFilter.createGroupBtn(characterQuestFilter.buttonPrefab, UnitGroupHead.group_large, groupLargeCategory.ID, groupLargeCategory.name, groupLargeCategory.GetSpriteName());
        groupBtn.transform.SetParent(((Component) characterQuestFilter.groupLargeGrid).transform);
        groupBtn.transform.localScale = Vector3.one;
        groupBtn.transform.localPosition = Vector3.zero;
      }
    }
    foreach (UnitGroupSmallCategory groupSmallCategory in ((IEnumerable<UnitGroupSmallCategory>) MasterData.UnitGroupSmallCategoryList).Where<UnitGroupSmallCategory>((Func<UnitGroupSmallCategory, bool>) (x =>
    {
      DateTime? startAt = x.start_at;
      DateTime dateTime = ServerTime.NowAppTime();
      return startAt.HasValue && startAt.GetValueOrDefault() < dateTime;
    })))
    {
      if (!string.IsNullOrEmpty(groupSmallCategory.name))
      {
        GameObject groupBtn = characterQuestFilter.createGroupBtn(characterQuestFilter.buttonPrefab, UnitGroupHead.group_small, groupSmallCategory.ID, groupSmallCategory.name, groupSmallCategory.GetSpriteName());
        groupBtn.transform.SetParent(((Component) characterQuestFilter.groupSmallGrid).transform);
        groupBtn.transform.localScale = Vector3.one;
        groupBtn.transform.localPosition = Vector3.zero;
      }
    }
    foreach (UnitGroupClothingCategory clothingCategory in ((IEnumerable<UnitGroupClothingCategory>) MasterData.UnitGroupClothingCategoryList).Where<UnitGroupClothingCategory>((Func<UnitGroupClothingCategory, bool>) (x =>
    {
      DateTime? startAt = x.start_at;
      DateTime dateTime = ServerTime.NowAppTime();
      return startAt.HasValue && startAt.GetValueOrDefault() < dateTime;
    })))
    {
      if (!string.IsNullOrEmpty(clothingCategory.name))
      {
        GameObject groupBtn = characterQuestFilter.createGroupBtn(characterQuestFilter.buttonPrefab, UnitGroupHead.group_clothing, clothingCategory.ID, clothingCategory.name, clothingCategory.GetSpriteName());
        groupBtn.transform.SetParent(((Component) characterQuestFilter.groupClothingGrid).transform);
        groupBtn.transform.localScale = Vector3.one;
        groupBtn.transform.localPosition = Vector3.zero;
      }
    }
    foreach (UnitGroupGenerationCategory generationCategory in ((IEnumerable<UnitGroupGenerationCategory>) MasterData.UnitGroupGenerationCategoryList).Where<UnitGroupGenerationCategory>((Func<UnitGroupGenerationCategory, bool>) (x =>
    {
      DateTime? startAt = x.start_at;
      DateTime dateTime = ServerTime.NowAppTime();
      return startAt.HasValue && startAt.GetValueOrDefault() < dateTime;
    })))
    {
      if (!string.IsNullOrEmpty(generationCategory.name))
      {
        GameObject groupBtn = characterQuestFilter.createGroupBtn(characterQuestFilter.buttonPrefab, UnitGroupHead.group_generation, generationCategory.ID, generationCategory.name, generationCategory.GetSpriteName());
        groupBtn.transform.SetParent(((Component) characterQuestFilter.groupGenerationGrid).transform);
        groupBtn.transform.localScale = Vector3.one;
        groupBtn.transform.localPosition = Vector3.zero;
      }
    }
    characterQuestFilter.removeIllegalGroupID();
    yield return (object) characterQuestFilter.StartCoroutine(characterQuestFilter.AdjustGroupButtonPosition());
  }

  private void removeIllegalGroupID()
  {
    UnitGroupHead[] unitGroupHeadArray = new UnitGroupHead[4]
    {
      UnitGroupHead.group_large,
      UnitGroupHead.group_small,
      UnitGroupHead.group_clothing,
      UnitGroupHead.group_generation
    };
    foreach (UnitGroupHead unitGroupHead in unitGroupHeadArray)
    {
      UnitGroupHead key = unitGroupHead;
      List<int> m = this.allGroupIDs[key];
      this.selectedGroupIDs[key] = this.selectedGroupIDs[key].Where<int>((Func<int, bool>) (x => m.Contains(x))).ToList<int>();
    }
  }

  private IEnumerator AdjustGroupButtonPosition()
  {
    this.groupLargeGrid.Reposition();
    this.groupSmallGrid.Reposition();
    this.groupClothingGrid.Reposition();
    this.groupGenerationGrid.Reposition();
    yield return (object) new WaitForSeconds(0.35f);
    float num1 = 0.0f;
    float num2 = 10f;
    this.groupLargeTitle.localPosition = new Vector3(this.groupLargeTitle.localPosition.x, num1, 0.0f);
    double num3 = (double) num1;
    Bounds relativeWidgetBounds1 = NGUIMath.CalculateRelativeWidgetBounds(this.groupLargeTitle);
    double num4 = (double) ((Bounds) ref relativeWidgetBounds1).size.y + 12.0;
    float num5 = (float) (num3 - num4);
    ((Component) this.groupLargeGrid).transform.localPosition = new Vector3(this.GetGridPositionX(this.groupLargeGrid), num5 - num2, 0.0f);
    double num6 = (double) num5;
    Bounds relativeWidgetBounds2 = NGUIMath.CalculateRelativeWidgetBounds(((Component) this.groupLargeGrid).transform);
    double y1 = (double) ((Bounds) ref relativeWidgetBounds2).size.y;
    float num7 = (float) (num6 - y1);
    this.groupSmallTitle.localPosition = new Vector3(this.groupSmallTitle.localPosition.x, num7, 0.0f);
    double num8 = (double) num7;
    Bounds relativeWidgetBounds3 = NGUIMath.CalculateRelativeWidgetBounds(this.groupSmallTitle);
    double y2 = (double) ((Bounds) ref relativeWidgetBounds3).size.y;
    float num9 = (float) (num8 - y2);
    ((Component) this.groupSmallGrid).transform.localPosition = new Vector3(this.GetGridPositionX(this.groupSmallGrid), num9 - num2, 0.0f);
    double num10 = (double) num9;
    Bounds relativeWidgetBounds4 = NGUIMath.CalculateRelativeWidgetBounds(((Component) this.groupSmallGrid).transform);
    double y3 = (double) ((Bounds) ref relativeWidgetBounds4).size.y;
    float num11 = (float) (num10 - y3);
    this.groupClothingTitle.localPosition = new Vector3(this.groupClothingTitle.localPosition.x, num11, 0.0f);
    double num12 = (double) num11;
    Bounds relativeWidgetBounds5 = NGUIMath.CalculateRelativeWidgetBounds(this.groupClothingTitle);
    double y4 = (double) ((Bounds) ref relativeWidgetBounds5).size.y;
    float num13 = (float) (num12 - y4);
    ((Component) this.groupClothingGrid).transform.localPosition = new Vector3(this.GetGridPositionX(this.groupClothingGrid), num13 - num2, 0.0f);
    double num14 = (double) num13;
    Bounds relativeWidgetBounds6 = NGUIMath.CalculateRelativeWidgetBounds(((Component) this.groupClothingGrid).transform);
    double y5 = (double) ((Bounds) ref relativeWidgetBounds6).size.y;
    float num15 = (float) (num14 - y5);
    this.groupGenerationTitle.localPosition = new Vector3(this.groupGenerationTitle.localPosition.x, num15, 0.0f);
    double num16 = (double) num15;
    Bounds relativeWidgetBounds7 = NGUIMath.CalculateRelativeWidgetBounds(this.groupGenerationTitle);
    double y6 = (double) ((Bounds) ref relativeWidgetBounds7).size.y;
    float num17 = (float) (num16 - y6);
    ((Component) this.groupGenerationGrid).transform.localPosition = new Vector3(this.GetGridPositionX(this.groupGenerationGrid), num17 - num2, 0.0f);
    this.groupButtonScrollView.ResetPosition();
  }

  private float GetGridPositionX(UIGrid grid)
  {
    float gridPositionX = 0.0f;
    if (((Component) grid).transform.childCount < 4)
      gridPositionX = (float) ((double) -(4 - ((Component) grid).transform.childCount) * (double) grid.cellWidth * 0.5);
    return gridPositionX;
  }

  public void Initialize(
    GameObject btnPrefab,
    CharacterQuestFilter.Calculator calculator,
    Action eventFilter,
    Action eventSample)
  {
    ((Component) this).GetComponent<UIRect>().alpha = 0.0f;
    this.calculator_ = calculator;
    this.selectedGroupIDs = CharacterQuestFilter.Calculator.duplicate(this.calculator_.selectedGroupIDs);
    this.buttonPrefab = btnPrefab;
    this.onFilter = eventFilter;
    this.onSample = eventSample;
  }

  private IEnumerator Start()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    CharacterQuestFilter characterQuestFilter = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      characterQuestFilter.setValueWindow();
      Singleton<PopupManager>.GetInstance().startOpenAnime(((Component) characterQuestFilter).gameObject);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) characterQuestFilter.CreateGroupList();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void setGroupTabLabel()
  {
    string text = "";
    List<int> selectedGroupId1 = this.selectedGroupIDs[UnitGroupHead.group_all];
    List<int> selectedGroupId2 = this.selectedGroupIDs[UnitGroupHead.group_large];
    List<int> selectedGroupId3 = this.selectedGroupIDs[UnitGroupHead.group_small];
    List<int> selectedGroupId4 = this.selectedGroupIDs[UnitGroupHead.group_clothing];
    List<int> selectedGroupId5 = this.selectedGroupIDs[UnitGroupHead.group_generation];
    if (selectedGroupId1.Contains(1))
      text = Consts.GetInstance().SORT_POPUP_LABEL_GROUP_ALL_TAB;
    else if (selectedGroupId1.Contains(2))
    {
      text = Consts.GetInstance().SORT_POPUP_LABEL_FILTER_OFF;
    }
    else
    {
      int count1 = selectedGroupId2.Count;
      int count2 = selectedGroupId3.Count;
      int count3 = selectedGroupId4.Count;
      int count4 = selectedGroupId5.Count;
      if (count1 == 1 && count2 == 0 && count3 == 0 && count4 == 0)
      {
        UnitGroupLargeCategory groupLargeCategory;
        if (MasterData.UnitGroupLargeCategory.TryGetValue(selectedGroupId2[0], out groupLargeCategory))
          text = groupLargeCategory.short_label_name;
      }
      else if (count1 == 0 && count2 == 1 && count3 == 0 && count4 == 0)
      {
        UnitGroupSmallCategory groupSmallCategory;
        if (MasterData.UnitGroupSmallCategory.TryGetValue(selectedGroupId3[0], out groupSmallCategory))
          text = groupSmallCategory.short_label_name;
      }
      else if (count1 == 0 && count2 == 0 && count3 == 1 && count4 == 0)
      {
        UnitGroupClothingCategory clothingCategory;
        if (MasterData.UnitGroupClothingCategory.TryGetValue(selectedGroupId4[0], out clothingCategory))
          text = clothingCategory.short_label_name;
      }
      else if (count1 == 0 && count2 == 0 && count3 == 0 && count4 == 1)
      {
        UnitGroupGenerationCategory generationCategory;
        if (MasterData.UnitGroupGenerationCategory.TryGetValue(selectedGroupId5[0], out generationCategory))
          text = generationCategory.short_label_name;
      }
      else
        text = Consts.GetInstance().SORT_POPUP_LABEL_GROUP_COMPLEX_TAB;
    }
    this.txtTitle.SetText(text);
    ((UIWidget) this.txtTitle).color = selectedGroupId1.Contains(2) ? new Color(1f, 1f, 1f) : new Color(1f, 1f, 0.0f);
  }

  private void setValueWindow()
  {
    this.setGroupTabLabel();
    this.onSample();
    List<Transform> transformList = new List<Transform>();
    transformList.AddRange(((Component) this.groupLargeGrid).transform.GetChildren());
    transformList.AddRange(((Component) this.groupSmallGrid).transform.GetChildren());
    transformList.AddRange(((Component) this.groupClothingGrid).transform.GetChildren());
    transformList.AddRange(((Component) this.groupGenerationGrid).transform.GetChildren());
    if (this.selectedGroupIDs[UnitGroupHead.group_all].Contains(1))
    {
      foreach (Component component1 in transformList)
      {
        UnitSortAndFilterGroupButton component2 = component1.GetComponent<UnitSortAndFilterGroupButton>();
        component2.isSelected = true;
        component2.SpriteColorGray(true);
        component2.TextColorGray(true);
      }
    }
    else if (this.selectedGroupIDs[UnitGroupHead.group_all].Contains(2))
    {
      foreach (Component component3 in transformList)
      {
        UnitSortAndFilterGroupButton component4 = component3.GetComponent<UnitSortAndFilterGroupButton>();
        component4.SpriteColorGray(false);
        component4.TextColorGray(false);
        component4.isSelected = false;
      }
    }
    else
    {
      foreach (Component component5 in transformList)
      {
        UnitSortAndFilterGroupButton component6 = component5.GetComponent<UnitSortAndFilterGroupButton>();
        component6.SpriteColorGray(false);
        component6.TextColorGray(false);
        component6.isSelected = false;
        if (this.selectedGroupIDs[component6.GroupType].Contains(component6.GroupID))
        {
          component6.isSelected = true;
          component6.SpriteColorGray(true);
          component6.TextColorGray(true);
        }
      }
    }
  }

  public void IbtnSelectAllGroup()
  {
    if (this.IsPush)
      return;
    this.addGroupInfo(UnitGroupHead.group_all, 1);
  }

  public void IbtnClearAllGroup()
  {
    if (this.IsPush)
      return;
    this.addGroupInfo(UnitGroupHead.group_all, 2);
  }

  public override void IbtnDicision()
  {
    if (this.IsPushAndSet())
      return;
    if (this.calculator_.Equals(this.selectedGroupIDs))
    {
      this.IsPush = false;
      this.IbtnClose();
    }
    else
    {
      this.SaveData();
      this.onFilter();
    }
  }

  private void switchGroupInfo(UnitGroupHead gType, int gID, bool bSelect)
  {
    if (this.IsPush)
      return;
    if (bSelect)
      this.addGroupInfo(gType, gID);
    else
      this.removeGroupInfo(gType, gID);
  }

  private void addGroupInfo(UnitGroupHead gType, int gID)
  {
    this.selectedGroupIDs[UnitGroupHead.group_all].Clear();
    List<int> selectedGroupId = this.selectedGroupIDs[gType];
    if (!selectedGroupId.Contains(gID))
      selectedGroupId.Add(gID);
    if (gType == UnitGroupHead.group_all)
    {
      switch (gID)
      {
        case 1:
          this.selectedGroupIDs[UnitGroupHead.group_large] = new List<int>((IEnumerable<int>) this.allGroupIDs[UnitGroupHead.group_large]);
          this.selectedGroupIDs[UnitGroupHead.group_small] = new List<int>((IEnumerable<int>) this.allGroupIDs[UnitGroupHead.group_small]);
          this.selectedGroupIDs[UnitGroupHead.group_clothing] = new List<int>((IEnumerable<int>) this.allGroupIDs[UnitGroupHead.group_clothing]);
          this.selectedGroupIDs[UnitGroupHead.group_generation] = new List<int>((IEnumerable<int>) this.allGroupIDs[UnitGroupHead.group_generation]);
          break;
        case 2:
          this.selectedGroupIDs[UnitGroupHead.group_large].Clear();
          this.selectedGroupIDs[UnitGroupHead.group_small].Clear();
          this.selectedGroupIDs[UnitGroupHead.group_clothing].Clear();
          this.selectedGroupIDs[UnitGroupHead.group_generation].Clear();
          break;
      }
    }
    else if (this.selectedGroupIDs[UnitGroupHead.group_large].Count == this.allGroupIDs[UnitGroupHead.group_large].Count && this.selectedGroupIDs[UnitGroupHead.group_small].Count == this.allGroupIDs[UnitGroupHead.group_small].Count && this.selectedGroupIDs[UnitGroupHead.group_clothing].Count == this.allGroupIDs[UnitGroupHead.group_clothing].Count && this.selectedGroupIDs[UnitGroupHead.group_generation].Count == this.allGroupIDs[UnitGroupHead.group_generation].Count)
    {
      this.addGroupInfo(UnitGroupHead.group_all, 1);
      return;
    }
    this.setValueWindow();
  }

  private void removeGroupInfo(UnitGroupHead gType, int gID)
  {
    this.selectedGroupIDs[UnitGroupHead.group_all].Clear();
    this.selectedGroupIDs[gType].Remove(gID);
    if (!this.selectedGroupIDs[UnitGroupHead.group_large].Any<int>() && !this.selectedGroupIDs[UnitGroupHead.group_small].Any<int>() && !this.selectedGroupIDs[UnitGroupHead.group_clothing].Any<int>() && !this.selectedGroupIDs[UnitGroupHead.group_generation].Any<int>())
      this.addGroupInfo(UnitGroupHead.group_all, 2);
    else
      this.setValueWindow();
  }

  public void SetUnitNum(UnitIconInfo[] aDisplays, UnitIconInfo[] aAlls)
  {
    this.SetUnitNum(aDisplays.Length, aAlls.Length);
  }

  public void SetUnitNum(int displaynum, int allnum)
  {
    this.unitGroupNum.SetText((displaynum <= 0 ? "[FF0000]" : "[FFFE27]") + (object) displaynum + "[-]/" + (object) allnum);
  }

  public override void SaveData()
  {
    this.calculator_.Reset(this.selectedGroupIDs);
    if (this.persist == null)
      return;
    this.persist.Data.groupIDs = CharacterQuestFilter.Calculator.duplicate(this.selectedGroupIDs);
    this.persist.Flush();
  }

  public UnitIconInfo[] filterBy(UnitIconInfo[] targets, List<int> regulars = null)
  {
    return this.calculator_.filterBy(targets, regulars, this.selectedGroupIDs);
  }

  public class Calculator
  {
    public readonly Dictionary<UnitGroupHead, List<int>> allGroupIDs;

    public Persist<Persist.CharacterQuestFilterInfo> persist { get; private set; }

    public Dictionary<UnitGroupHead, List<int>> selectedGroupIDs { get; private set; }

    public Calculator(Persist<Persist.CharacterQuestFilterInfo> filter)
    {
      this.persist = filter;
      this.Reset(this.persist.Data.groupIDs);
      this.allGroupIDs = UnitMenuBase.CreateAllGroupIDs();
      this.removeIllegalGroupID();
    }

    public Calculator()
    {
      this.selectedGroupIDs = Persist.CharacterQuestFilterInfo.createMapUnitGroupHead();
      this.allGroupIDs = UnitMenuBase.CreateAllGroupIDs();
    }

    private void removeIllegalGroupID()
    {
      UnitGroupHead[] unitGroupHeadArray = new UnitGroupHead[4]
      {
        UnitGroupHead.group_large,
        UnitGroupHead.group_small,
        UnitGroupHead.group_clothing,
        UnitGroupHead.group_generation
      };
      foreach (UnitGroupHead unitGroupHead in unitGroupHeadArray)
      {
        UnitGroupHead key = unitGroupHead;
        List<int> m = this.allGroupIDs[key];
        this.selectedGroupIDs[key] = this.selectedGroupIDs[key].Where<int>((Func<int, bool>) (x => m.Contains(x))).ToList<int>();
      }
    }

    public bool Equals(Dictionary<UnitGroupHead, List<int>> targets)
    {
      foreach (KeyValuePair<UnitGroupHead, List<int>> selectedGroupId in this.selectedGroupIDs)
      {
        List<int> target = targets[selectedGroupId.Key];
        if (selectedGroupId.Value.Count != target.Count)
          return false;
        foreach (int num in selectedGroupId.Value)
        {
          if (!target.Contains(num))
            return false;
        }
      }
      return true;
    }

    public void Reset(Dictionary<UnitGroupHead, List<int>> result)
    {
      this.selectedGroupIDs = CharacterQuestFilter.Calculator.duplicate(result);
    }

    public static Dictionary<UnitGroupHead, List<int>> duplicate(
      Dictionary<UnitGroupHead, List<int>> org)
    {
      return org.ToDictionary<KeyValuePair<UnitGroupHead, List<int>>, UnitGroupHead, List<int>>((Func<KeyValuePair<UnitGroupHead, List<int>>, UnitGroupHead>) (p => p.Key), (Func<KeyValuePair<UnitGroupHead, List<int>>, List<int>>) (p => p.Value.ToList<int>()));
    }

    public UnitIconInfo[] filterBy(
      UnitIconInfo[] targets,
      List<int> regulars = null,
      Dictionary<UnitGroupHead, List<int>> groupIds = null)
    {
      Dictionary<UnitGroupHead, List<int>> groupIDs = groupIds ?? this.selectedGroupIDs;
      if (groupIDs[UnitGroupHead.group_all].Any<int>())
        return ((IEnumerable<UnitIconInfo>) targets).ToArray<UnitIconInfo>();
      List<UnitIconInfo> unitIconInfoList = new List<UnitIconInfo>();
      Dictionary<int, UnitGroup> dictionary = ((IEnumerable<UnitGroup>) MasterData.UnitGroupList).ToDictionary<UnitGroup, int>((Func<UnitGroup, int>) (x => x.unit_id));
      if (regulars == null)
        regulars = new List<int>();
      for (int index = 0; index < targets.Length; ++index)
      {
        UnitIconInfo target = targets[index];
        UnitUnit unit = target.unit;
        if (regulars.Contains(unit.ID) || CharacterQuestFilter.Calculator.checkGroup(dictionary, unit, groupIDs, this.allGroupIDs))
          unitIconInfoList.Add(target);
      }
      return unitIconInfoList.ToArray();
    }

    public static bool checkGroup(
      Dictionary<int, UnitGroup> groupDic,
      UnitUnit unit,
      Dictionary<UnitGroupHead, List<int>> groupIDs,
      Dictionary<UnitGroupHead, List<int>> allGroupIDs)
    {
      UnitGroup unitGroup;
      if (!groupDic.TryGetValue(unit.ID, out unitGroup))
        return false;
      List<int> groupId1 = groupIDs[UnitGroupHead.group_large];
      int num1 = (!groupId1.Any<int>() ? 1 : (groupId1.Count == allGroupIDs[UnitGroupHead.group_large].Count ? 1 : 0)) != 0 ? 1 : (groupId1.Contains(unitGroup.group_large_category_id_UnitGroupLargeCategory) ? 1 : 0);
      List<int> groupId2 = groupIDs[UnitGroupHead.group_small];
      bool flag1 = !groupId2.Any<int>() || groupId2.Count == allGroupIDs[UnitGroupHead.group_small].Count || groupId2.Contains(unitGroup.group_small_category_id_UnitGroupSmallCategory);
      List<int> groupId3 = groupIDs[UnitGroupHead.group_clothing];
      bool flag2 = !groupId3.Any<int>() || groupId3.Count == allGroupIDs[UnitGroupHead.group_clothing].Count || groupId3.Contains(unitGroup.group_clothing_category_id_UnitGroupClothingCategory) || groupId3.Contains(unitGroup.group_clothing_category_id_2_UnitGroupClothingCategory);
      List<int> groupId4 = groupIDs[UnitGroupHead.group_generation];
      bool flag3 = !groupId4.Any<int>() || groupId4.Count == allGroupIDs[UnitGroupHead.group_generation].Count || groupId4.Contains(unitGroup.group_generation_category_id_UnitGroupGenerationCategory);
      int num2 = flag1 ? 1 : 0;
      return (num1 & num2 & (flag2 ? 1 : 0) & (flag3 ? 1 : 0)) != 0;
    }
  }
}
