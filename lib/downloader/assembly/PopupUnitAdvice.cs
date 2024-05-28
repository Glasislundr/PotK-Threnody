// Decompiled with JetBrains decompiler
// Type: PopupUnitAdvice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/Recommend/UnitAdvice")]
public class PopupUnitAdvice : BackButtonPopupBase
{
  [SerializeField]
  private UIScrollView scrollView_;
  [SerializeField]
  private UILabel txtMain_;

  public static Future<GameObject> loadResource()
  {
    return new ResourceObject("Prefabs/unit/Popup_Advice_Detail").Load<GameObject>();
  }

  public static void open(GameObject prefab, UnitAdvice advice)
  {
    PopupUnitAdvice component = Singleton<PopupManager>.GetInstance().open(prefab).GetComponent<PopupUnitAdvice>();
    component.setTopObject(((Component) component).gameObject);
    component.txtMain_.SetTextLocalize(advice.advice);
    component.scrollView_.ResetPosition();
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
