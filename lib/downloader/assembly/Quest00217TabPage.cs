// Decompiled with JetBrains decompiler
// Type: Quest00217TabPage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/quest002_17/TabPage")]
public class Quest00217TabPage : MonoBehaviour
{
  private List<Quest00217TabPage.Cell> cells_ = new List<Quest00217TabPage.Cell>();
  [SerializeField]
  private SpreadColorButton mTabButton;
  [SerializeField]
  private GameObject mScrollContainer;
  [SerializeField]
  private bool isFirstResetContainerAlpha;
  [SerializeField]
  private UIScrollView mScrollView;
  [SerializeField]
  private UIGrid mGrid;
  private Quest00217Menu mQuestMenu;
  private Quest002SideStoryMenu mSideQuestMenu;
  private bool isResetedContainerAlpha;

  public int ID { get; private set; }

  public List<Quest00217TabPage.RequestParam> RequestList { get; private set; } = new List<Quest00217TabPage.RequestParam>();

  public List<Quest00217TabPage.RequestParam> RequestLateList { get; private set; } = new List<Quest00217TabPage.RequestParam>();

  public void Init(Quest00217Menu menu, int id)
  {
    this.mQuestMenu = menu;
    this.ID = id;
    // ISSUE: method pointer
    this.mGrid.onReposition = new UIGrid.OnReposition((object) this, __methodptr(onGridReposition));
    this.ClearRequest();
  }

  public void Init(Quest002SideStoryMenu menu, int id)
  {
    this.mSideQuestMenu = menu;
    this.ID = id;
    // ISSUE: method pointer
    this.mGrid.onReposition = new UIGrid.OnReposition((object) this, __methodptr(onGridReposition));
    this.ClearRequest();
  }

  public void AddRequest(Quest00217Scroll.Parameter param, GameObject prefab)
  {
    Quest00217TabPage.RequestParam requestParam = new Quest00217TabPage.RequestParam()
    {
      IsCreated = param.isClearedToday,
      QuestLockID = param.entryConditionID,
      Param = (object) param,
      Prefab = prefab
    };
    if (!requestParam.IsCreated)
      this.RequestList.Add(requestParam);
    else
      this.RequestLateList.Add(requestParam);
  }

  public void AddRequest(EventInfo param, GameObject prefab)
  {
    this.RequestList.Add(new Quest00217TabPage.RequestParam()
    {
      Param = (object) param,
      Prefab = prefab
    });
  }

  public void AddRequest(SM.TowerPeriod param, GameObject prefab)
  {
    this.RequestList.Add(new Quest00217TabPage.RequestParam()
    {
      Param = (object) param,
      Prefab = prefab
    });
  }

  public void AddRequest(CorpsPeriod param, GameObject prefab)
  {
    this.RequestList.Add(new Quest00217TabPage.RequestParam()
    {
      Param = (object) param,
      Prefab = prefab
    });
  }

  public void AddRequest(GameObject prefab)
  {
    this.RequestList.Add(new Quest00217TabPage.RequestParam()
    {
      Prefab = prefab
    });
  }

  public void ClearRequest()
  {
    this.RequestList.Clear();
    this.RequestLateList.Clear();
  }

  public void SetGridReposition() => this.mScrollView.ResetPosition();

  private void onGridReposition() => this.mScrollView.ResetPosition();

  public void SetButtonActive(bool active)
  {
    if (active)
      this.setTabButtonColor(Color.white);
    else
      this.setTabButtonColor(Color.gray);
  }

  public void SetPageActive(bool active, bool bOperateAlpha = false)
  {
    this.SetButtonActive(active);
    bOperateAlpha = ((bOperateAlpha ? 1 : 0) & (!this.isFirstResetContainerAlpha ? 0 : (!this.isResetedContainerAlpha ? 1 : 0))) != 0;
    this.mScrollContainer.gameObject.SetActive(active | bOperateAlpha);
    if (!active & bOperateAlpha)
    {
      this.mScrollContainer.GetComponent<UIRect>().alpha = 0.0f;
    }
    else
    {
      if (!active || !this.isFirstResetContainerAlpha || this.isResetedContainerAlpha)
        return;
      this.isResetedContainerAlpha = true;
      this.mScrollContainer.GetComponent<UIRect>().alpha = 1f;
    }
  }

  public void AddItem(GameObject obj) => this.setItem(obj);

  public void RepositionGrid() => this.mGrid.Reposition();

  public void UpdateTime(DateTime serverTime)
  {
    foreach (Quest00217Scroll quest00217Scroll in this.getItems())
      quest00217Scroll.SetTime(serverTime, quest00217Scroll.RankingEventTerm);
  }

  public void OnPushTab() => this.mQuestMenu.OnPushPageTab(this.ID);

  public void OnPushTabWithSideQuest() => this.mSideQuestMenu.OnPushPageTab(this.ID);

  private List<Quest00217Scroll> getItems() => this.getItems<Quest00217Scroll>();

  private void setTabButtonColor(Color color)
  {
    ((UIButtonColor) this.mTabButton).defaultColor = ((UIButtonColor) this.mTabButton).pressed = ((UIButtonColor) this.mTabButton).hover = color;
    this.mTabButton.SetTweenColor(false, 0.0f, color);
    foreach (UIWidget componentsInChild in ((Component) this.mTabButton).GetComponentsInChildren<UISprite>())
      componentsInChild.color = color;
  }

  private List<T> getItems<T>() where T : MonoBehaviour
  {
    return this.cells_.Select<Quest00217TabPage.Cell, T>((Func<Quest00217TabPage.Cell, T>) (i => i.item.GetComponent<T>())).Where<T>((Func<T, bool>) (c => Object.op_Implicit((Object) (object) c))).ToList<T>();
  }

  private int setItem(GameObject item)
  {
    if (Object.op_Equality((Object) item, (Object) null))
      return -1;
    Transform transform = item.transform;
    transform.parent = ((Component) this.mGrid).gameObject.transform;
    transform.localPosition = Vector3.zero;
    transform.localRotation = Quaternion.identity;
    transform.localScale = Vector3.one;
    int count = this.cells_.Count;
    this.cells_.Add(new Quest00217TabPage.Cell(item));
    return count;
  }

  public class Cell
  {
    public GameObject item { get; private set; }

    public Cell(GameObject go) => this.item = go;
  }

  public class RequestParam
  {
    public bool IsCreated;
    public int QuestLockID;
    public object Param;
    public GameObject Prefab;
  }
}
