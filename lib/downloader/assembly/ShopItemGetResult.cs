// Decompiled with JetBrains decompiler
// Type: ShopItemGetResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ShopItemGetResult : MonoBehaviour
{
  [SerializeField]
  private List<GameObject> IconObjects;
  [SerializeField]
  private GameObject messageBox;
  private Action onFinishCallback;

  public IEnumerator Init(
    List<ShopItemGetResultInfo> items,
    bool isOverflow,
    Action finishCallback)
  {
    this.onFinishCallback = finishCallback;
    this.IconObjects.ForEachIndex<GameObject>((Action<GameObject, int>) ((x, i) => x.SetActive(items.Count - 1 == i)));
    GameObject iconObject = this.IconObjects[items.Count - 1];
    int count = 0;
    foreach (Component component in iconObject.transform)
    {
      IEnumerator e = component.gameObject.GetOrAddComponent<CreateIconObject>().CreateThumbnail(items[count].rewardType, items[count].rewardId, items[count].quantity, isButton: false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ++count;
    }
    if (isOverflow)
      this.messageBox.SetActive(true);
    else
      this.messageBox.SetActive(false);
  }

  public void OnTouch() => this.onFinishCallback();
}
