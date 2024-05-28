// Decompiled with JetBrains decompiler
// Type: OverkillersUnitBaseTag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/unit004_2/OverkillersUnitBaseTag")]
public class OverkillersUnitBaseTag : MonoBehaviour
{
  [SerializeField]
  private GameObject lnkIcon_;
  private UnitIcon icon_;
  private Action onClicked_;
  private Action onLongPressed_;

  public static Future<GameObject> createLoander(bool isSea)
  {
    return new ResourceObject("Prefabs/unit004_2" + (isSea ? "_sea" : "") + "/ibtn_UnitEquipment_IsSet" + (isSea ? "_Sea" : "")).Load<GameObject>();
  }

  public GameObject objButton => this.lnkIcon_;

  public void initialize(
    GameObject unitIconPrefab,
    UIScrollView scrollView,
    Action actClicked,
    Action actLongPressed)
  {
    if (Object.op_Equality((Object) this.icon_, (Object) null))
    {
      this.icon_ = unitIconPrefab.Clone(this.lnkIcon_.transform).GetComponent<UnitIcon>();
      ((Collider) this.icon_.buttonBoxCollider).enabled = false;
      ((UIWidget) ((Component) this.icon_).GetComponentInChildren<UI2DSprite>(true)).depth = this.lnkIcon_.GetComponent<UIWidget>().depth;
    }
    this.objButton.GetComponent<UIDragScrollView>().scrollView = scrollView;
    this.onClicked_ = actClicked;
    this.onLongPressed_ = actLongPressed;
  }

  public IEnumerator doReset(PlayerUnit unit)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    OverkillersUnitBaseTag overkillersUnitBaseTag = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      overkillersUnitBaseTag.icon_.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
      ((Component) overkillersUnitBaseTag).gameObject.SetActive(true);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    ((Component) overkillersUnitBaseTag).gameObject.SetActive(false);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) overkillersUnitBaseTag.icon_.SetUnit(unit, unit.GetElement(), false);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void onClickedIcon() => this.onClicked_();

  public void onLongPressedIcon() => this.onLongPressed_();
}
