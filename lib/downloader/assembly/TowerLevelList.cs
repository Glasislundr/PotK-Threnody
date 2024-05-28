// Decompiled with JetBrains decompiler
// Type: TowerLevelList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class TowerLevelList : MonoBehaviour
{
  [SerializeField]
  private GameObject slc_clear;
  [SerializeField]
  private GameObject ibtn_locked;
  [SerializeField]
  private UILabel lblFloor;
  [SerializeField]
  private SpriteSelectDirect _spriteDirect;
  private int floor;
  private GameObject _marker;

  public SpriteSelectDirect spriteDirect => this._spriteDirect;

  public int floorNum => this.floor;

  public GameObject marker => this._marker;

  public bool isClear => this.slc_clear.activeSelf;

  public bool isLocked => this.ibtn_locked.activeSelf;

  public void Init(
    bool isClear,
    bool isLocked,
    int floor,
    GameObject marker,
    Action<GameObject> clickAction,
    TowerFloorName floorName)
  {
    this.slc_clear.SetActive(isClear);
    this.ibtn_locked.SetActive(isLocked);
    ((Component) this._spriteDirect).gameObject.SetActive(!isLocked);
    if (floorName == null)
      this.lblFloor.SetTextLocalize(Consts.Format(Consts.GetInstance().TOWER_LEVEL_LIST_LABEL, (IDictionary) new Hashtable()
      {
        {
          (object) "level",
          (object) floor
        }
      }));
    else
      this.lblFloor.SetTextLocalize(string.Format("{0}{1}{2}", floorName.prefix == null ? (object) string.Empty : (object) floorName.prefix, (object) (floor * floorName.interval), floorName.suffix == null ? (object) string.Empty : (object) floorName.suffix));
    this.floor = floor;
    this._marker = marker;
    UIButton component1 = ((Component) this.spriteDirect).GetComponent<UIButton>();
    if (Object.op_Inequality((Object) component1, (Object) null))
    {
      component1.onClick.Clear();
      component1.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => clickAction(((Component) this).gameObject))));
    }
    UIButton component2 = this.ibtn_locked.GetComponent<UIButton>();
    if (!Object.op_Inequality((Object) component2, (Object) null))
      return;
    component2.onClick.Clear();
    component2.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => clickAction(((Component) this).gameObject))));
  }
}
