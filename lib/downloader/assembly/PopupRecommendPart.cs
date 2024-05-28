// Decompiled with JetBrains decompiler
// Type: PopupRecommendPart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/Recommend/Part")]
public class PopupRecommendPart : MonoBehaviour
{
  protected PlayerUnit playerUnit_;
  protected UnitUnit target_;
  private PopupRecommendMenu menu_;

  protected PopupRecommendMenu menu
  {
    get
    {
      return !Object.op_Inequality((Object) this.menu_, (Object) null) ? (this.menu_ = NGUITools.FindInParents<PopupRecommendMenu>(((Component) this).gameObject)) : this.menu_;
    }
  }

  public virtual IEnumerator doInitialize(PlayerUnit playerUnit, UnitUnit target)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    PopupRecommendPart popupRecommendPart = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    ((Component) popupRecommendPart).gameObject.SetActive(false);
    return false;
  }

  protected void onChangedScene()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Action onChangedScene = this.menu.onChangedScene;
    if (onChangedScene == null)
      return;
    onChangedScene();
  }
}
