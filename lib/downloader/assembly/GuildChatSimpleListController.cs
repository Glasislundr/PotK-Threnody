// Decompiled with JetBrains decompiler
// Type: GuildChatSimpleListController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GuildChatSimpleListController : MonoBehaviour
{
  [SerializeField]
  private GuildChatManager guildChatManager;
  [SerializeField]
  private UIScrollView scrollView;
  [SerializeField]
  private UIPanel panel;
  [SerializeField]
  private SpringPanel sp;
  [SerializeField]
  private UIGrid grid;
  private float normalizedCurrentScrollPositionY;
  private float simpleViewItemHeight = 30f;
  public const int maxDisplayingItemCount = 3;

  private void Awake()
  {
  }

  private void Start()
  {
  }

  private void Update()
  {
  }

  public void AddSimpleMessageItems(List<GuildChatMessageData> dataList)
  {
    float num1 = ((Component) this.grid).transform.childCount <= 0 ? 0.0f : ((Component) this.grid).transform.GetChild(((Component) this.grid).transform.childCount - 1).localPosition.y - this.simpleViewItemHeight;
    for (int index = 0; index < dataList.Count; ++index)
    {
      GameObject self = Object.Instantiate<GameObject>(this.guildChatManager.simpleMessageItemPrefab);
      self.GetComponent<GuildChatMessageItemController>().InitializeSimpleMessageItem(dataList[index]);
      self.SetParent(((Component) this.grid).gameObject);
      self.transform.localScale = Vector3.one;
      self.transform.localPosition = new Vector3(0.0f, num1 - (float) index * this.simpleViewItemHeight, 0.0f);
    }
    if (((Component) this.grid).transform.childCount <= 0)
      return;
    ((Behaviour) this.sp).enabled = false;
    this.scrollView.currentMomentum = Vector3.zero;
    int num2 = ((Component) this.grid).transform.childCount < 3 ? 3 - ((Component) this.grid).transform.childCount : 0;
    this.sp.target = new Vector3(0.0f, -(((Component) ((Component) this.grid).transform.GetChild(((Component) this.grid).transform.childCount - 1)).transform.localPosition.y - (float) num2 * this.simpleViewItemHeight) + this.simpleViewItemHeight, 0.0f);
    // ISSUE: method pointer
    this.sp.onFinished = new SpringPanel.OnFinished((object) this, __methodptr(OnScrollFinished));
    ((Behaviour) this.sp).enabled = true;
  }

  private void OnScrollFinished()
  {
    int num = ((Component) this.grid).transform.childCount - 3;
    if (num <= 0)
      return;
    for (int index = 0; index < num; ++index)
    {
      Transform child = ((Component) this.grid).transform.GetChild(num - 1 - index);
      child.SetParent((Transform) null);
      Object.Destroy((Object) ((Component) child).gameObject);
    }
  }

  public void ClearMessageItemList()
  {
    if (Object.op_Equality((Object) this.grid, (Object) null))
      Debug.Log((object) "grid = null");
    foreach (Transform transform in ((Component) this.grid).transform)
    {
      if (Object.op_Equality((Object) transform, (Object) null))
        Debug.Log((object) "t = null");
      if (Object.op_Equality((Object) ((Component) transform).gameObject, (Object) null))
        Debug.Log((object) "(t.gameObject = null");
      Object.Destroy((Object) ((Component) transform).gameObject);
    }
    if (Object.op_Equality((Object) this.sp, (Object) null))
      Debug.Log((object) "(sp = null");
    if (Object.op_Equality((Object) this.scrollView, (Object) null))
      Debug.Log((object) "(scrollView = null");
    Vector3 currentMomentum = this.scrollView.currentMomentum;
    ((Behaviour) this.sp).enabled = false;
    this.scrollView.currentMomentum = Vector3.zero;
    if (Object.op_Equality((Object) this.scrollView.panel, (Object) null))
      Debug.Log((object) "(scrollView.panel = null");
    ((Component) this.scrollView).transform.localPosition = Vector3.zero;
    if (Object.op_Equality((Object) this.scrollView.panel, (Object) null))
      ((Component) this.scrollView).GetComponent<UIPanel>().clipOffset = Vector2.zero;
    else
      this.scrollView.panel.clipOffset = Vector2.zero;
  }
}
