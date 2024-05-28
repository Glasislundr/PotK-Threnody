// Decompiled with JetBrains decompiler
// Type: GuildChatStampSelectViewController
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
public class GuildChatStampSelectViewController : MonoBehaviour
{
  [SerializeField]
  private GuildChatManager guildChatManager;
  [SerializeField]
  private UISprite previewImage;
  [SerializeField]
  private GameObject previewArea;
  [SerializeField]
  private UIScrollView stampScrollView;
  [SerializeField]
  private UIScrollView stampGroupScrollView;
  [SerializeField]
  private SpringPanel springPanel;
  [SerializeField]
  private SpringPanel stampGroupSpringPanel;
  [SerializeField]
  private Transform container;
  [SerializeField]
  private GameObject noAvailableStampCaution;
  [SerializeField]
  private GameObject stampItemList;
  [SerializeField]
  private Transform headPlaceholder;
  [SerializeField]
  private Transform tailPlaceholder;
  [SerializeField]
  private GameObject stampGroupItemList;
  [SerializeField]
  private UIGrid stampGroupItemContainer;
  private int currentSelectedStampGroupID;
  private int currentSelectedStampID;
  private Dictionary<int, List<int>> allStampData = new Dictionary<int, List<int>>();
  private Dictionary<int, int> stampGroupIconIDs = new Dictionary<int, int>();
  private int[] availableStampGroupIDs = new int[0];
  private List<int> currentSelectedStampGroup = new List<int>();
  private const int bufferedStampItemCount = 20;
  private const int stampListColumnCount = 4;
  private float stampSelectItemHeight;
  private float stampSelectItemWidth;
  private float stampGroupSelectItemWidth;
  private List<GuildChatStampSelectItemController> bufferedStampSelectItems;
  private List<GuildChatStampGroupSelectItemController> stampGroupSelectItems;
  private bool shouldUpdateStampList = true;
  public bool isStampSelectViewOpened;
  public bool isFirstTimeOpeningStampSelectView = true;

  private void Awake()
  {
  }

  private void Start()
  {
  }

  private void Update() => this.CheckAndUpdateStampList();

  private void CheckAndUpdateStampList()
  {
    if ((double) this.stampScrollView.panel.finalClipRegion.y - (double) this.stampScrollView.panel.finalClipRegion.w > (double) ((Component) this.headPlaceholder).transform.localPosition.y)
    {
      ((Component) this.stampScrollView).transform.localPosition = new Vector3(((Component) this.stampScrollView).transform.localPosition.x, (float) -((double) ((Component) this.headPlaceholder).transform.localPosition.y + (double) this.stampScrollView.panel.finalClipRegion.w - 1.0), ((Component) this.stampScrollView).transform.localPosition.z);
      this.stampScrollView.panel.clipOffset = new Vector2(-((Component) this.stampScrollView).transform.localPosition.x, -((Component) this.stampScrollView).transform.localPosition.y);
    }
    else if ((double) ((Component) this.tailPlaceholder).transform.localPosition.y > (double) this.stampScrollView.panel.finalClipRegion.y)
    {
      ((Component) this.stampScrollView).transform.localPosition = new Vector3(((Component) this.stampScrollView).transform.localPosition.x, (float) -((double) ((Component) this.tailPlaceholder).transform.localPosition.y + 1.0), ((Component) this.stampScrollView).transform.localPosition.z);
      this.stampScrollView.panel.clipOffset = new Vector2(-((Component) this.stampScrollView).transform.localPosition.x, -((Component) this.stampScrollView).transform.localPosition.y);
    }
    if (this.availableStampGroupIDs.Length == 0)
    {
      this.noAvailableStampCaution.SetActive(true);
      this.stampGroupItemList.SetActive(false);
      this.stampItemList.SetActive(false);
    }
    else
    {
      this.noAvailableStampCaution.SetActive(false);
      this.stampGroupItemList.SetActive(true);
      this.stampItemList.SetActive(true);
      if (this.shouldUpdateStampList)
      {
        int num1 = Mathf.FloorToInt(((Component) this.stampScrollView).transform.localPosition.y / this.stampSelectItemHeight) * 4;
        int num2 = Mathf.FloorToInt((((Component) this.stampScrollView).transform.localPosition.y + this.stampScrollView.panel.finalClipRegion.w) / this.stampSelectItemHeight) * 4 + 4 - 1;
        if (num1 < 0)
          num1 = 0;
        if (num2 > this.currentSelectedStampGroup.Count - 1)
          num2 = this.currentSelectedStampGroup.Count - 1;
        List<int> intList = new List<int>();
        for (int index = num1; index <= num2; ++index)
        {
          int currentItemID = this.currentSelectedStampGroup[index];
          intList.Add(currentItemID);
          if (!this.bufferedStampSelectItems.Any<GuildChatStampSelectItemController>((Func<GuildChatStampSelectItemController, bool>) (x => x.stampID == currentItemID)))
          {
            GuildChatStampSelectItemController selectItemController = this.bufferedStampSelectItems.FirstOrDefault<GuildChatStampSelectItemController>((Func<GuildChatStampSelectItemController, bool>) (x => x.stampID == 0));
            if (Object.op_Inequality((Object) selectItemController, (Object) null))
            {
              selectItemController.InitializeGuildChatStampItem(currentItemID);
              ((Component) selectItemController).transform.localScale = Vector3.one;
              ((Component) selectItemController).transform.localPosition = new Vector3((float) (index % 4) * this.stampSelectItemWidth, (float) -(index / 4) * this.stampSelectItemHeight, 0.0f);
              ((Component) selectItemController).GetComponent<UIDragScrollView>().scrollView = this.stampScrollView;
              ((Component) selectItemController).gameObject.SetActive(true);
            }
            else
              Debug.Log((object) "<color=red>Available buffered stamp select item not found!!!</color>");
          }
        }
        for (int index = 0; index < this.bufferedStampSelectItems.Count; ++index)
        {
          GuildChatStampSelectItemController bufferedStampSelectItem = this.bufferedStampSelectItems[index];
          if (!intList.Contains(bufferedStampSelectItem.stampID))
            bufferedStampSelectItem.Clear();
        }
        this.shouldUpdateStampList = false;
      }
      if (!Vector3.op_Inequality(this.stampScrollView.currentMomentum, Vector3.zero) && !((Behaviour) this.springPanel).isActiveAndEnabled)
        return;
      this.shouldUpdateStampList = true;
    }
  }

  private void RefreshStampData()
  {
    GuildStampGroup[] stampGroups = MasterData.GuildStampGroupList;
    GuildStamp[] guildStampList = MasterData.GuildStampList;
    if (!this.isFirstTimeOpeningStampSelectView)
      this.isFirstTimeOpeningStampSelectView = PlayerAffiliation.Current.stamp_groups.Length != this.availableStampGroupIDs.Length || !((IEnumerable<int>) PlayerAffiliation.Current.stamp_groups).All<int>((Func<int, bool>) (x => ((IEnumerable<int>) this.availableStampGroupIDs).Contains<int>(x)));
    this.allStampData.Clear();
    this.stampGroupIconIDs.Clear();
    for (int i = 0; i < stampGroups.Length; i++)
    {
      List<int> list = ((IEnumerable<GuildStamp>) guildStampList).Where<GuildStamp>((Func<GuildStamp, bool>) (x => x.groupID.ID == stampGroups[i].ID)).Select<GuildStamp, int>((Func<GuildStamp, int>) (x => x.ID)).ToList<int>();
      this.allStampData.Add(stampGroups[i].ID, list);
      int iconId = stampGroups[i].iconID;
      this.stampGroupIconIDs.Add(stampGroups[i].ID, iconId);
    }
    this.availableStampGroupIDs = (int[]) PlayerAffiliation.Current.stamp_groups.Clone();
    if (this.availableStampGroupIDs.Length != 0)
    {
      if (this.isFirstTimeOpeningStampSelectView)
        this.currentSelectedStampGroupID = this.availableStampGroupIDs[0];
      this.currentSelectedStampGroup = this.allStampData[this.currentSelectedStampGroupID];
    }
    this.currentSelectedStampID = 0;
    this.shouldUpdateStampList = true;
  }

  private void InitializeBufferedStampList()
  {
    if (this.bufferedStampSelectItems == null)
    {
      this.StartCoroutine(this.InitializeBufferedStampListCoroutine());
    }
    else
    {
      foreach (GuildChatStampSelectItemController bufferedStampSelectItem in this.bufferedStampSelectItems)
        bufferedStampSelectItem.Clear();
    }
  }

  private IEnumerator InitializeBufferedStampListCoroutine()
  {
    while (Object.op_Equality((Object) Singleton<CommonRoot>.GetInstance().guildChatManager.stampSelectItemPrefab, (Object) null))
    {
      Debug.Log((object) "<color=yellow>The prefab of stampe select item is not ready.</color>");
      yield return (object) null;
    }
    GameObject selectItemPrefab = Singleton<CommonRoot>.GetInstance().guildChatManager.stampSelectItemPrefab;
    this.bufferedStampSelectItems = new List<GuildChatStampSelectItemController>();
    for (int index = 0; index < 20; ++index)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(selectItemPrefab);
      GuildChatStampSelectItemController component = gameObject.GetComponent<GuildChatStampSelectItemController>();
      gameObject.SetActive(false);
      gameObject.transform.SetParent(this.container);
      this.bufferedStampSelectItems.Add(component);
    }
    this.stampSelectItemWidth = (float) selectItemPrefab.GetComponent<UIWidget>().width;
    this.stampSelectItemHeight = (float) selectItemPrefab.GetComponent<UIWidget>().height;
  }

  private void ResetStampSelectViewStatus(bool shouldResetScrollViewPosition)
  {
    this.currentSelectedStampID = 0;
    this.previewArea.SetActive(false);
    if (!shouldResetScrollViewPosition)
      return;
    this.ResetStampScrollView();
  }

  private void ResetStampScrollView()
  {
    this.shouldUpdateStampList = true;
    ((Behaviour) this.springPanel).enabled = false;
    UIPanel component = ((Component) this.stampScrollView).GetComponent<UIPanel>();
    ((Component) this.stampScrollView).transform.localPosition = new Vector3(0.0f, -component.clipSoftness.y, 0.0f);
    component.clipOffset = new Vector2(-((Component) this.stampScrollView).transform.localPosition.x, -((Component) this.stampScrollView).transform.localPosition.y);
    this.headPlaceholder.localPosition = Vector3.zero;
    float num = (float) Mathf.CeilToInt((float) this.currentSelectedStampGroup.Count / 4f) * this.stampSelectItemHeight;
    this.tailPlaceholder.localPosition = (double) num <= (double) component.finalClipRegion.w ? new Vector3(0.0f, -component.finalClipRegion.w, 0.0f) : new Vector3(0.0f, -num, 0.0f);
    this.stampScrollView.UpdateScrollbars(true);
  }

  public void SelectStampGroup(int stampGroupID)
  {
    this.currentSelectedStampGroupID = stampGroupID;
    this.currentSelectedStampGroup = this.allStampData[this.currentSelectedStampGroupID];
    foreach (GuildChatStampSelectItemController bufferedStampSelectItem in this.bufferedStampSelectItems)
      bufferedStampSelectItem.Clear();
    this.ResetStampSelectViewStatus(true);
    int num1 = 0;
    UIWidget uiWidget = (UIWidget) null;
    for (int index = 0; index < this.stampGroupSelectItems.Count; ++index)
    {
      this.stampGroupSelectItems[index].SetSelected(this.stampGroupSelectItems[index].stampGroupID == stampGroupID);
      if (this.stampGroupSelectItems[index].stampGroupID == stampGroupID)
      {
        num1 = index;
        uiWidget = ((Component) this.stampGroupSelectItems[index]).GetComponent<UIWidget>();
      }
    }
    if (!this.stampGroupScrollView.shouldMoveHorizontally)
      return;
    Matrix4x4 worldToLocalMatrix1 = ((Component) this.stampGroupScrollView).transform.worldToLocalMatrix;
    Vector3 vector3_1 = ((Matrix4x4) ref worldToLocalMatrix1).MultiplyPoint(((UIRect) uiWidget).worldCorners[0]);
    Vector3 vector3_2 = Vector3.op_Addition(((UIRect) this.stampGroupScrollView.panel).localCorners[0], new Vector3(this.stampGroupScrollView.panel.clipSoftness.x, 0.0f, 0.0f));
    Matrix4x4 worldToLocalMatrix2 = ((Component) this.stampGroupScrollView).transform.worldToLocalMatrix;
    Vector3 vector3_3 = ((Matrix4x4) ref worldToLocalMatrix2).MultiplyPoint(((UIRect) uiWidget).worldCorners[2]);
    Vector3 vector3_4 = Vector3.op_Subtraction(((UIRect) this.stampGroupScrollView.panel).localCorners[2], new Vector3(this.stampGroupScrollView.panel.clipSoftness.x, 0.0f, 0.0f));
    float num2 = vector3_1.x - vector3_2.x;
    float num3 = vector3_4.x - vector3_3.x;
    if (num1 > 0 && (double) num2 > (double) -uiWidget.width && (double) num2 < (double) uiWidget.width)
    {
      this.stampGroupSpringPanel.target = new Vector3(((Component) this.stampGroupScrollView).transform.localPosition.x + ((float) uiWidget.width - num2), ((Component) this.stampGroupScrollView).transform.localPosition.y, ((Component) this.stampGroupScrollView).transform.localPosition.z);
      ((Behaviour) this.stampGroupSpringPanel).enabled = true;
    }
    else if (num1 < this.availableStampGroupIDs.Length - 1 && (double) num3 >= (double) -uiWidget.width && (double) num3 < (double) uiWidget.width)
    {
      this.stampGroupSpringPanel.target = new Vector3(((Component) this.stampGroupScrollView).transform.localPosition.x - ((float) uiWidget.width - num3), ((Component) this.stampGroupScrollView).transform.localPosition.y, ((Component) this.stampGroupScrollView).transform.localPosition.z);
      ((Behaviour) this.stampGroupSpringPanel).enabled = true;
    }
    else if (num1 == 0)
    {
      this.stampGroupSpringPanel.target = new Vector3(((Component) this.stampGroupScrollView).transform.localPosition.x + (vector3_2.x - vector3_1.x), ((Component) this.stampGroupScrollView).transform.localPosition.y, ((Component) this.stampGroupScrollView).transform.localPosition.z);
      ((Behaviour) this.stampGroupSpringPanel).enabled = true;
    }
    else
    {
      if (num1 != this.availableStampGroupIDs.Length - 1)
        return;
      this.stampGroupSpringPanel.target = new Vector3(((Component) this.stampGroupScrollView).transform.localPosition.x - (vector3_3.x - vector3_4.x), ((Component) this.stampGroupScrollView).transform.localPosition.y, ((Component) this.stampGroupScrollView).transform.localPosition.z);
      ((Behaviour) this.stampGroupSpringPanel).enabled = true;
    }
  }

  public void SelectStamp(int stampID)
  {
    this.currentSelectedStampID = stampID;
    Singleton<CommonRoot>.GetInstance().guildChatManager.SetStampSprite(this.previewImage, this.currentSelectedStampID);
    this.previewArea.SetActive(true);
  }

  public void OpenStampSelectView()
  {
    if (this.isStampSelectViewOpened)
      return;
    Debug.Log((object) "OpenStampSelectView is invoked!");
    ((Component) this).gameObject.SetActive(true);
    this.InitializeBufferedStampList();
    this.RefreshStampData();
    this.ResetStampSelectViewStatus(true);
    if (this.isFirstTimeOpeningStampSelectView)
      this.InitializeStampGroupItemList();
    this.isFirstTimeOpeningStampSelectView = false;
    this.guildChatManager.PlayGuildChatAnimation(GuildChatManager.GuildChatAnimationType.Open_Stamp_Panel, (EventDelegate.Callback) (() => this.isStampSelectViewOpened = true));
  }

  private void InitializeStampGroupItemList()
  {
    if (this.availableStampGroupIDs.Length == 0)
      return;
    this.StartCoroutine(this.InitializeStampGroupItemListCoroutine());
  }

  private IEnumerator InitializeStampGroupItemListCoroutine()
  {
    while (Object.op_Equality((Object) Singleton<CommonRoot>.GetInstance().guildChatManager.stampGroupSelectItemPrefab, (Object) null))
    {
      Debug.Log((object) "<color=yellow>The prefab of stampe group select item is not loaded.</color>");
      yield return (object) null;
    }
    GameObject selectItemPrefab = Singleton<CommonRoot>.GetInstance().guildChatManager.stampGroupSelectItemPrefab;
    this.stampGroupSelectItemWidth = (float) selectItemPrefab.GetComponent<UIWidget>().width;
    this.stampGroupSelectItems = new List<GuildChatStampGroupSelectItemController>();
    for (int index = 0; index < ((Component) this.stampGroupItemContainer).transform.childCount; ++index)
      Object.Destroy((Object) ((Component) ((Component) this.stampGroupItemContainer).transform.GetChild(index)).gameObject);
    for (int index = 0; index < this.availableStampGroupIDs.Length; ++index)
    {
      int availableStampGroupId = this.availableStampGroupIDs[index];
      int stampGroupIconId = this.stampGroupIconIDs[availableStampGroupId];
      GameObject gameObject = Object.Instantiate<GameObject>(selectItemPrefab);
      GuildChatStampGroupSelectItemController component = gameObject.GetComponent<GuildChatStampGroupSelectItemController>();
      component.InitializeGuildChatStampGroupItem(availableStampGroupId, stampGroupIconId);
      this.stampGroupSelectItems.Add(component);
      gameObject.transform.SetParent(((Component) this.stampGroupItemContainer).transform);
      gameObject.transform.localScale = Vector3.one;
      gameObject.transform.localPosition = new Vector3((float) index * this.stampGroupSelectItemWidth, 0.0f, 0.0f);
      gameObject.SetActive(true);
    }
    this.stampGroupScrollView.ResetPosition();
    this.SelectStampGroup(this.currentSelectedStampGroupID);
  }

  public void CloseStampSelectView()
  {
    if (!this.isStampSelectViewOpened)
      return;
    Debug.Log((object) "CloseStampSelectView is invoked!");
    this.ResetStampSelectViewStatus(false);
    this.guildChatManager.PlayGuildChatAnimation(GuildChatManager.GuildChatAnimationType.Close_Stamp_Panel, (EventDelegate.Callback) (() =>
    {
      ((Component) this).gameObject.SetActive(false);
      this.isStampSelectViewOpened = false;
    }));
  }

  public void OnPreviewImageClicked()
  {
    if (this.currentSelectedStampID <= 0)
      return;
    this.guildChatManager.SendStamp(this.currentSelectedStampID);
    this.CloseStampSelectView();
  }

  public void OnCloseButtonClicked() => this.CloseStampSelectView();

  public void OnScrollBarValueChanged() => this.shouldUpdateStampList = true;
}
