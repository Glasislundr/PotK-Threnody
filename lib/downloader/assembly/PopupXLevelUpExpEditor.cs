// Decompiled with JetBrains decompiler
// Type: PopupXLevelUpExpEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/Unit/PopupXLevelUpExpEditor")]
public class PopupXLevelUpExpEditor : VolumeEditor
{
  [Header("XLevelUpの為のアイテム数を操作用設定")]
  [SerializeField]
  private Transform lnkIcon_;
  [SerializeField]
  private UILabel txtQuantity_;
  [SerializeField]
  private UILabel txtExp_;

  public static Future<GameObject> createLoader()
  {
    return new ResourceObject("Prefabs/unit004_2/dir_vscroll_ExpItem").Load<GameObject>();
  }

  public IEnumerator initialize(
    GameObject iconPrefab,
    PopupXLevelUp popup,
    PlayerMaterialUnit item)
  {
    PopupXLevelUpExpEditor xlevelUpExpEditor = this;
    UnitIcon icon = iconPrefab.Clone(xlevelUpExpEditor.lnkIcon_).GetComponent<UnitIcon>();
    UnitUnit unit = item.unit;
    MaterialXLevelExp material = MasterData.MaterialXLevelExp[unit.ID];
    ((UIWidget) xlevelUpExpEditor.txtExp_).color = popup.isBoostExp ? Color.yellow : Color.white;
    yield return (object) icon.SetUnit(unit, unit.GetElement(), false);
    icon.BottomModeValue = UnitIconBase.BottomMode.Nothing;
    icon.SetIconBoxCollider(false);
    icon.SetCounter(item.quantity);
    xlevelUpExpEditor.maxReal = Mathf.Min(item.quantity, xlevelUpExpEditor.maxVolume);
    xlevelUpExpEditor.onChangedVolume = (Func<int, int>) (value =>
    {
      int num = popup.onChangeValue(item, value);
      this.txtQuantity_.SetTextLocalize(num);
      this.txtExp_.SetTextLocalize(popup.multiply(material, num));
      return num;
    });
    xlevelUpExpEditor.initialize();
  }
}
