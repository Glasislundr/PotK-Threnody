// Decompiled with JetBrains decompiler
// Type: MissionGetPointRewardEffectPopupController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MissionGetPointRewardEffectPopupController : MonoBehaviour
{
  [SerializeField]
  private List<GameObject> IconObjects;
  [SerializeField]
  private GameObject messageBox;
  private Action callback;

  public IEnumerator Init(List<PointReward> pointRewardList, Action callback = null)
  {
    this.callback = callback;
    this.IconObjects.ForEachIndex<GameObject>((Action<GameObject, int>) ((x, i) => x.SetActive(pointRewardList.Count - 1 == i)));
    GameObject iconObject = this.IconObjects[pointRewardList.Count - 1];
    int count = 0;
    foreach (Component component in iconObject.transform)
    {
      IEnumerator e = component.gameObject.GetOrAddComponent<CreateIconObject>().CreateThumbnail(pointRewardList[count].reward_type, pointRewardList[count].reward_id, pointRewardList[count].reward_quantity, isButton: false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ++count;
    }
  }

  public void OnClickTouchToNextButton()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    Action callback = this.callback;
    if (callback == null)
      return;
    callback();
  }
}
