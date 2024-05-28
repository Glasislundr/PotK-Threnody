// Decompiled with JetBrains decompiler
// Type: FullScreenWrapContent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Custum/UI/FullScreenWrapContent")]
public class FullScreenWrapContent : UIWrapContent
{
  [SerializeField]
  [Tooltip("Start()コールタイミングをコントロールしたい時にON")]
  private bool isDeactivateOnAwake_ = true;

  public FullScreenWrapContent.callbackUpdateItem onUpdateItem { get; set; }

  private void Awake()
  {
    if (!this.isDeactivateOnAwake_)
      return;
    ((Component) this).gameObject.SetActive(false);
  }

  protected virtual void Start()
  {
    this.resetItemSize();
    base.Start();
  }

  public void resetItemSize()
  {
    if (!this.CacheScrollView())
      return;
    UIPanel inParents = NGUITools.FindInParents<UIPanel>(((Component) this).gameObject);
    UIScrollView component = ((Component) inParents).GetComponent<UIScrollView>();
    ((UIRect) inParents).UpdateAnchors();
    UIScrollView.Movement movement = component.movement;
    if (movement != null)
    {
      if (movement != 1)
        return;
      this.itemSize = (int) inParents.height;
    }
    else
      this.itemSize = (int) inParents.width;
  }

  protected virtual void UpdateItem(Transform item, int index)
  {
    FullScreenWrapContent.callbackUpdateItem onUpdateItem = this.onUpdateItem;
    if (onUpdateItem == null)
      return;
    onUpdateItem(item, index);
  }

  public delegate void callbackUpdateItem(Transform item, int index);
}
