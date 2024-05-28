// Decompiled with JetBrains decompiler
// Type: MapEditFacilityStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class MapEditFacilityStorage : MonoBehaviour
{
  [SerializeField]
  private NGxScroll scroll_;
  [SerializeField]
  private GameObject topScroll_;
  [SerializeField]
  private GameObject topNotExist_;
  [SerializeField]
  private UIButton btnSwitch_;
  [SerializeField]
  private UIButton btnClose_;
  [SerializeField]
  private GameObject topCost_;
  [SerializeField]
  private UILabel txtCost_;
  [SerializeField]
  private MapEditFacilityStorage.Form formNormal_;
  [SerializeField]
  private MapEditFacilityStorage.Form formEdit_;
  private bool layoutEdit_ = true;
  private MapEditFacilityStorage.Form currentForm_;
  private PlayerGuildFacility[] facilities_;
  private GameObject prefabIcon_;
  private GameObject prefabPart_;
  private GameObject prefabGenre_;
  private Action<PlayerGuildFacility> eventSelect_;
  private List<MapEditFacilityList> storage_ = new List<MapEditFacilityList>();
  private MapEditFacilityStorage.DrawMode drawMode_;
  private int limitedCost_;
  private BL.StructValue<int> instRemainingCost_ = new BL.StructValue<int>(0);
  private BL.StructValue<bool> notLocate_ = new BL.StructValue<bool>(false);
  private int totalQuantity_;
  private List<Tuple<PlayerGuildFacility, int>> usedFacilities_;

  public bool isInitialized_ { get; private set; }

  public bool isLayoutEdit_
  {
    get => this.layoutEdit_;
    set => this.layoutEdit_ = value;
  }

  public int remainingCost_
  {
    get => this.instRemainingCost_.value;
    private set
    {
      if (this.instRemainingCost_.value == value)
        return;
      this.instRemainingCost_.value = value;
    }
  }

  public bool isNotLocate_
  {
    get => this.notLocate_.value;
    private set
    {
      if (this.notLocate_.value == value)
        return;
      this.notLocate_.value = value;
    }
  }

  public int totalUsed_ { get; private set; }

  public int numLocation_ { get; private set; }

  public IEnumerator initialize(
    PlayerGuildFacility[] facilities,
    Action<PlayerGuildFacility> eventSelect,
    Action eventClose,
    Dictionary<int, int> used,
    int limitedCost,
    int numLocation)
  {
    this.isInitialized_ = false;
    this.topCost_.SetActive(this.isLayoutEdit_);
    if (this.isLayoutEdit_)
    {
      this.formNormal_.enabled_ = false;
      this.formEdit_.enabled_ = true;
      this.currentForm_ = this.formEdit_;
    }
    else
    {
      this.formNormal_.enabled_ = true;
      this.formEdit_.enabled_ = false;
      this.currentForm_ = this.formNormal_;
    }
    this.facilities_ = facilities != null ? facilities : new PlayerGuildFacility[0];
    this.eventSelect_ = eventSelect;
    this.storage_.Clear();
    EventDelegate.Set(this.btnClose_.onClick, (EventDelegate.Callback) (() => eventClose()));
    this.totalUsed_ = 0;
    this.totalQuantity_ = ((IEnumerable<PlayerGuildFacility>) this.facilities_).Sum<PlayerGuildFacility>((Func<PlayerGuildFacility, int>) (f => f == null ? 0 : f.hasnum));
    this.numLocation_ = numLocation;
    this.limitedCost_ = limitedCost;
    if (used == null)
      used = new Dictionary<int, int>();
    this.usedFacilities_ = ((IEnumerable<PlayerGuildFacility>) this.facilities_).Where<PlayerGuildFacility>((Func<PlayerGuildFacility, bool>) (x => x.hasnum > 0)).Select<PlayerGuildFacility, Tuple<PlayerGuildFacility, int>>((Func<PlayerGuildFacility, Tuple<PlayerGuildFacility, int>>) (f =>
    {
      int num;
      if (f == null || !used.TryGetValue(f._master, out num))
        return Tuple.Create<PlayerGuildFacility, int>(f, 0);
      this.totalUsed_ += num;
      limitedCost -= f.unit.cost * num;
      return Tuple.Create<PlayerGuildFacility, int>(f, num);
    })).ToList<Tuple<PlayerGuildFacility, int>>();
    this.remainingCost_ = limitedCost;
    this.isNotLocate_ = this.totalUsed_ >= this.numLocation_;
    this.updateTotal();
    this.topScroll_.SetActive(false);
    this.topNotExist_.SetActive(false);
    Future<GameObject> ldprefab = MapEdit.Prefabs.facility_list.Load<GameObject>();
    IEnumerator e = ldprefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabPart_ = ldprefab.Result;
    ldprefab = (Future<GameObject>) null;
    ldprefab = Res.Icons.SkillGenreIcon.Load<GameObject>();
    e = ldprefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabGenre_ = ldprefab.Result;
    ldprefab = (Future<GameObject>) null;
    ldprefab = MapEdit.Prefabs.facility_thumb.Load<GameObject>();
    e = ldprefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabIcon_ = ldprefab.Result;
    ldprefab = (Future<GameObject>) null;
  }

  private IEnumerator Start()
  {
    MapEditFacilityStorage editFacilityStorage = this;
    while (Object.op_Equality((Object) editFacilityStorage.prefabPart_, (Object) null) || Object.op_Equality((Object) editFacilityStorage.prefabGenre_, (Object) null) || Object.op_Equality((Object) editFacilityStorage.prefabIcon_, (Object) null))
      yield return (object) null;
    editFacilityStorage.scroll_.Reset();
    if (editFacilityStorage.usedFacilities_ != null && editFacilityStorage.usedFacilities_.Any<Tuple<PlayerGuildFacility, int>>())
    {
      editFacilityStorage.topScroll_.SetActive(true);
      ((UIButtonColor) editFacilityStorage.btnSwitch_).isEnabled = true;
      int firstdraw = (int) editFacilityStorage.drawMode_;
      foreach (Tuple<PlayerGuildFacility, int> usedFacility in editFacilityStorage.usedFacilities_)
      {
        GameObject gameObject = editFacilityStorage.prefabPart_.Clone();
        editFacilityStorage.scroll_.Add(gameObject, true);
        MapEditFacilityList cntl = gameObject.GetComponent<MapEditFacilityList>();
        cntl.preInitialize(editFacilityStorage.isLayoutEdit_);
        IEnumerator e = cntl.initialize(editFacilityStorage.prefabIcon_, editFacilityStorage.prefabGenre_, usedFacility.Item1, editFacilityStorage.eventSelect_, usedFacility.Item2, editFacilityStorage.instRemainingCost_, editFacilityStorage.notLocate_);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        cntl.changeDraw(firstdraw);
        editFacilityStorage.storage_.Add(cntl);
        cntl = (MapEditFacilityList) null;
      }
      // ISSUE: method pointer
      editFacilityStorage.scroll_.GridReposition(new UIGrid.OnReposition((object) editFacilityStorage, __methodptr(\u003CStart\u003Eb__47_0)));
    }
    else
    {
      ((UIButtonColor) editFacilityStorage.btnSwitch_).isEnabled = false;
      editFacilityStorage.topNotExist_.SetActive(true);
      editFacilityStorage.isInitialized_ = true;
    }
    editFacilityStorage.usedFacilities_ = (List<Tuple<PlayerGuildFacility, int>>) null;
  }

  public void onClickedSwitch()
  {
    int index = (int) (this.drawMode_ + 1);
    if (index >= 2)
      index = 0;
    this.drawMode_ = (MapEditFacilityStorage.DrawMode) index;
    foreach (MapEditFacilityList editFacilityList in this.storage_)
      editFacilityList.changeDraw(index);
  }

  public bool checkAvailability(int id)
  {
    if (!this.isNotLocate_ && this.storage_ != null)
    {
      MapEditFacilityList editFacilityList = this.storage_.FirstOrDefault<MapEditFacilityList>((Func<MapEditFacilityList, bool>) (f => f.ID_ == id));
      if (Object.op_Inequality((Object) editFacilityList, (Object) null))
        return this.remainingCost_ >= editFacilityList.cost_;
    }
    return false;
  }

  public PlayerGuildFacility getFacility(int id)
  {
    MapEditFacilityList editFacilityList = this.storage_ != null ? this.storage_.FirstOrDefault<MapEditFacilityList>((Func<MapEditFacilityList, bool>) (f => f.ID_ == id)) : (MapEditFacilityList) null;
    return !Object.op_Inequality((Object) editFacilityList, (Object) null) ? (PlayerGuildFacility) null : editFacilityList.facility_;
  }

  public void useFacility(int id) => this.countFacility(id, 1);

  public void returnFacility(int id) => this.countFacility(id, -1);

  private void countFacility(int id, int inc)
  {
    if (this.storage_ == null)
      return;
    MapEditFacilityList editFacilityList = this.storage_.FirstOrDefault<MapEditFacilityList>((Func<MapEditFacilityList, bool>) (f => f.ID_ == id));
    if (Object.op_Equality((Object) editFacilityList, (Object) null))
      return;
    editFacilityList.used_ += inc;
    this.totalUsed_ += inc;
    this.remainingCost_ += inc > 0 ? -editFacilityList.cost_ : editFacilityList.cost_;
    this.isNotLocate_ = this.totalUsed_ >= this.numLocation_;
    this.updateTotal();
  }

  public void returnFacilityAll() => this.resetCountAll();

  public void resetFacilityCountAll(int numLocation, int limitedCost)
  {
    this.resetCountAll(numLocation, limitedCost);
  }

  private void resetCountAll(int numLocation = -1, int limitedCost = -1)
  {
    if (this.storage_ == null)
      return;
    foreach (MapEditFacilityList editFacilityList in this.storage_)
      editFacilityList.used_ = 0;
    this.totalUsed_ = 0;
    if (limitedCost >= 0)
      this.limitedCost_ = limitedCost;
    this.remainingCost_ = this.limitedCost_;
    if (numLocation >= 0)
      this.numLocation_ = numLocation;
    this.isNotLocate_ = this.totalUsed_ >= this.numLocation_;
    this.updateTotal();
  }

  private void updateTotal()
  {
    Consts instance = Consts.GetInstance();
    if (this.currentForm_ != null)
      this.currentForm_.setTotalQuantity(this.totalUsed_, this.totalQuantity_);
    if (!this.isLayoutEdit_ || !Object.op_Inequality((Object) this.txtCost_, (Object) null))
      return;
    Hashtable args = new Hashtable()
    {
      {
        (object) "used",
        (object) (this.limitedCost_ - this.remainingCost_)
      },
      {
        (object) "limited",
        (object) this.limitedCost_
      }
    };
    this.txtCost_.SetTextLocalize(Consts.Format(instance.MAPEDIT_031_FOOTER_COST, (IDictionary) args));
  }

  public void UpdatePossession(int facilityID)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MapEditFacilityStorage.\u003C\u003Ec__DisplayClass58_0 cDisplayClass580 = new MapEditFacilityStorage.\u003C\u003Ec__DisplayClass58_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass580.facilityID = facilityID;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass580.\u003C\u003E4__this = this;
    MapEditFacilityList[] componentsInChildren = ((Component) this.scroll_).GetComponentsInChildren<MapEditFacilityList>();
    if (componentsInChildren == null || componentsInChildren.Length == 0)
      return;
    int num = 0;
    for (int index1 = 0; index1 < componentsInChildren.Length; ++index1)
    {
      // ISSUE: reference to a compiler-generated field
      if (componentsInChildren[index1].facility_._master == cDisplayClass580.facilityID)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PlayerGuildFacility playerGuildFacility = ((IEnumerable<PlayerGuildFacility>) SMManager.Get<PlayerGuildFacility[]>()).FirstOrDefault<PlayerGuildFacility>(cDisplayClass580.\u003C\u003E9__0 ?? (cDisplayClass580.\u003C\u003E9__0 = new Func<PlayerGuildFacility, bool>(cDisplayClass580.\u003CUpdatePossession\u003Eb__0)));
        if (playerGuildFacility != null)
        {
          if (playerGuildFacility.hasnum <= 0)
          {
            Object.Destroy((Object) ((Component) componentsInChildren[index1]).gameObject);
            this.scroll_.Reset();
            for (int index2 = 0; index2 < componentsInChildren.Length; ++index2)
            {
              if (index1 != index2)
                this.scroll_.Add(((Component) componentsInChildren[index2]).gameObject);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: method pointer
            this.scroll_.GridReposition(cDisplayClass580.\u003C\u003E9__1 ?? (cDisplayClass580.\u003C\u003E9__1 = new UIGrid.OnReposition((object) cDisplayClass580, __methodptr(\u003CUpdatePossession\u003Eb__1))));
            this.scroll_.scrollView.ResetPosition();
            continue;
          }
          componentsInChildren[index1].facility_.hasnum = playerGuildFacility.hasnum;
          componentsInChildren[index1].updateQuantity();
        }
        else
          continue;
      }
      num += componentsInChildren[index1].facility_.hasnum;
    }
    this.totalQuantity_ = num;
    this.updateTotal();
    this.topNotExist_.SetActive(num <= 0);
    ((UIButtonColor) this.btnSwitch_).isEnabled = num > 0;
  }

  [Serializable]
  public class Form
  {
    [SerializeField]
    private GameObject top_;
    [SerializeField]
    private UILabel txtQuantity_;
    [SerializeField]
    private bool enabledUsed_ = true;

    public bool enabled_
    {
      get => this.top_.activeSelf;
      set => this.top_.SetActive(value);
    }

    public void setTotalQuantity(int used, int total)
    {
      if (this.enabledUsed_)
        this.txtQuantity_.SetTextLocalize(Consts.Format(Consts.GetInstance().MAPEDIT_031_FACILITY_QUANTITY, (IDictionary) new Hashtable()
        {
          {
            (object) nameof (used),
            (object) used
          },
          {
            (object) "hasnum",
            (object) total
          }
        }));
      else
        this.txtQuantity_.SetTextLocalize(total);
    }
  }

  private enum DrawMode
  {
    Description,
    Parameter,
    Max,
  }
}
