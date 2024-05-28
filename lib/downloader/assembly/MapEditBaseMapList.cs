// Decompiled with JetBrains decompiler
// Type: MapEditBaseMapList
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
public class MapEditBaseMapList : MonoBehaviour
{
  [SerializeField]
  private UIButton btnSelect_;
  [SerializeField]
  private GameObject lnkIcon_;
  [SerializeField]
  private UILabel txtName_;
  [SerializeField]
  private UILabel txtDescription_;
  [SerializeField]
  private UILabel txtCost_;
  [SerializeField]
  private UIButton btnDetail_;
  private const int DEPTH_INTERVAL = 1;

  public PlayerGuildTown town_ { get; private set; }

  public IEnumerator initialize(
    GameObject prefabIcon,
    PlayerGuildTown town,
    Action<PlayerGuildTown> eventSelect,
    Action<int> eventDetail)
  {
    this.town_ = town;
    MapTown master = this.town_.master;
    this.txtName_.SetTextLocalize(master.name);
    this.txtDescription_.SetTextLocalize(master.description);
    Hashtable args = new Hashtable()
    {
      {
        (object) "used",
        (object) 0
      },
      {
        (object) "limited",
        (object) master.cost_capacity
      }
    };
    this.txtCost_.SetTextLocalize(Consts.Format(Consts.GetInstance().MAPEDIT_031_SLOT_COST, (IDictionary) args));
    EventDelegate.Set(this.btnSelect_.onClick, (EventDelegate.Callback) (() => eventSelect(this.town_)));
    EventDelegate.Set(this.btnDetail_.onClick, (EventDelegate.Callback) (() => eventDetail(this.town_._master)));
    GameObject goIcon = prefabIcon.Clone(this.lnkIcon_.transform);
    UniqueIcons icon = goIcon.GetComponent<UniqueIcons>();
    IEnumerator e = icon.SetGuildMap(town._master);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UIWidget component = this.lnkIcon_.GetComponent<UIWidget>();
    icon.SetSize(Mathf.RoundToInt(component.localSize.x), Mathf.RoundToInt(component.localSize.y));
    NGUITools.AdjustDepth(goIcon, component.depth + 1);
    Singleton<NGSceneManager>.GetInstance().StartCoroutine(this.doResizeButton(((Component) icon).transform));
  }

  private IEnumerator doResizeButton(Transform trsIcon)
  {
    yield return (object) new WaitForEndOfFrame();
    Bounds relativeWidgetBounds = NGUIMath.CalculateRelativeWidgetBounds(trsIcon, trsIcon);
    ((Component) this.btnDetail_).GetComponent<UIWidget>().SetDimensions(Mathf.RoundToInt(((Bounds) ref relativeWidgetBounds).size.x), Mathf.RoundToInt(((Bounds) ref relativeWidgetBounds).size.y));
  }

  public void setCurrent(bool bCurrent) => ((UIButtonColor) this.btnSelect_).isEnabled = !bCurrent;
}
