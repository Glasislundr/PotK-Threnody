// Decompiled with JetBrains decompiler
// Type: RouletteAwardDetailPopupController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RouletteAwardDetailPopupController : MonoBehaviour
{
  [SerializeField]
  private UIGrid awardItemContainer;
  [SerializeField]
  private UIScrollView awardItemScrollView;
  private RouletteMenu menu;

  public IEnumerator Init(
    RouletteMenu menu,
    List<RouletteR001FreeDeckEntity> awardDataList,
    GameObject awardItemPrefab)
  {
    this.menu = menu;
    for (int i = 0; i < awardDataList.Count; ++i)
    {
      IEnumerator e = awardItemPrefab.Clone(((Component) this.awardItemContainer).transform).GetComponent<RouletteAwardDetailPopupItemController>().Init(awardDataList[i]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.awardItemContainer.Reposition();
    yield return (object) new WaitForEndOfFrame();
    this.awardItemScrollView.ResetPosition();
  }

  public void OnTapBackButton()
  {
    this.menu.isDisplayingAwardDetailPopup = false;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
