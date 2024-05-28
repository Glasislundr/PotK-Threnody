// Decompiled with JetBrains decompiler
// Type: MapEdit031MenuStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MapEdit031MenuStorage : MapEditMenuBase
{
  [SerializeField]
  private GameObject topInterface_;
  private MapEditFacilityStorage facilityStorage_;

  public override MapEdit031TopMenu.EditState editState_ => MapEdit031TopMenu.EditState.Storage;

  protected override IEnumerator initializeAsync()
  {
    MapEdit031MenuStorage edit031MenuStorage = this;
    GameObject go = (GameObject) null;
    if (Object.op_Inequality((Object) edit031MenuStorage.facilityStorage_, (Object) null))
    {
      Object.Destroy((Object) ((Component) edit031MenuStorage.facilityStorage_).gameObject);
      edit031MenuStorage.facilityStorage_ = (MapEditFacilityStorage) null;
    }
    Future<GameObject> ldprefab = MapEdit.Prefabs.facility_storage.Load<GameObject>();
    IEnumerator e = ldprefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) ldprefab.Result, (Object) null))
      go = ldprefab.Result.Clone(edit031MenuStorage.topInterface_.transform);
    ldprefab = (Future<GameObject>) null;
    PlayerGuildFacility[] facilities = SMManager.Get<PlayerGuildFacility[]>();
    int costCapacity = edit031MenuStorage.topMenu_.data_.mapTown_.cost_capacity;
    int length = edit031MenuStorage.topMenu_.data_.facilitySetting_ != null ? edit031MenuStorage.topMenu_.data_.facilitySetting_.Length : 0;
    Dictionary<int, int> used = new Dictionary<int, int>();
    foreach (PlayerGuildTownSlotPosition townSlotPosition in edit031MenuStorage.topMenu_.data_.originalPosition_)
    {
      if (!used.ContainsKey(townSlotPosition.master_id))
        used.Add(townSlotPosition.master_id, 1);
      else
        used[townSlotPosition.master_id]++;
    }
    if (Object.op_Inequality((Object) go, (Object) null))
    {
      edit031MenuStorage.addControlObject(go);
      edit031MenuStorage.facilityStorage_ = go.GetComponent<MapEditFacilityStorage>();
      if (Object.op_Inequality((Object) edit031MenuStorage.facilityStorage_, (Object) null))
      {
        // ISSUE: reference to a compiler-generated method
        e = edit031MenuStorage.facilityStorage_.initialize(facilities, new Action<PlayerGuildFacility>(edit031MenuStorage.\u003CinitializeAsync\u003Eb__4_0), new Action(((BackButtonMenuBase) edit031MenuStorage).onBackButton), used, costCapacity, length);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        while (!edit031MenuStorage.facilityStorage_.isInitialized_)
          yield return (object) null;
      }
    }
  }

  protected override void onEnable() => this.ui3DEvent_.isEnabled_ = false;

  protected override void onDisable()
  {
  }

  protected override void Update() => base.Update();

  public override void onBackButton()
  {
    if (this.waitAndSet())
      return;
    this.topMenu_.backStorage();
  }

  public PlayerGuildFacility useFacility(int id)
  {
    PlayerGuildFacility playerGuildFacility = (PlayerGuildFacility) null;
    if (Object.op_Inequality((Object) this.facilityStorage_, (Object) null) && this.facilityStorage_.checkAvailability(id))
    {
      playerGuildFacility = this.facilityStorage_.getFacility(id);
      this.facilityStorage_.useFacility(id);
    }
    return playerGuildFacility;
  }

  public PlayerGuildFacility getFacility(int id)
  {
    return !Object.op_Inequality((Object) this.facilityStorage_, (Object) null) ? (PlayerGuildFacility) null : this.facilityStorage_.getFacility(id);
  }

  public void returnFacility(int id)
  {
    if (!Object.op_Inequality((Object) this.facilityStorage_, (Object) null))
      return;
    this.facilityStorage_.returnFacility(id);
  }

  public void returnFacilityAll()
  {
    if (!Object.op_Inequality((Object) this.facilityStorage_, (Object) null))
      return;
    this.facilityStorage_.returnFacilityAll();
  }

  public void resetFacilityCountAll(int numLocation, int limitedCost)
  {
    if (!Object.op_Inequality((Object) this.facilityStorage_, (Object) null))
      return;
    this.facilityStorage_.resetFacilityCountAll(numLocation, limitedCost);
  }

  public int totalUsed_
  {
    get
    {
      return !Object.op_Inequality((Object) this.facilityStorage_, (Object) null) ? 0 : this.facilityStorage_.totalUsed_;
    }
  }

  public int remainingCost_
  {
    get
    {
      return !Object.op_Inequality((Object) this.facilityStorage_, (Object) null) ? 0 : this.facilityStorage_.remainingCost_;
    }
  }
}
