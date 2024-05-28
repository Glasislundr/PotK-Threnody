// Decompiled with JetBrains decompiler
// Type: MapEditBaseMapSelect
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
public class MapEditBaseMapSelect : MonoBehaviour
{
  [SerializeField]
  private UIButton btnClose_;
  [SerializeField]
  private NGxScroll scroll_;
  [SerializeField]
  private UILabel txtQuantity_;
  private PlayerGuildTown[] towns_;
  private Action<PlayerGuildTown> eventSelect_;
  private Action<int> eventDetail_;
  private List<MapEditBaseMapList> baseMapList_ = new List<MapEditBaseMapList>();
  private int current_;

  public IEnumerator initialize(
    PlayerGuildTown[] towns,
    int current,
    Action<PlayerGuildTown> eventSelect,
    Action<int> eventDetail,
    Action eventClose)
  {
    this.txtQuantity_.SetTextLocalize(towns.Length);
    EventDelegate.Set(this.btnClose_.onClick, (EventDelegate.Callback) (() => eventClose()));
    this.towns_ = towns;
    this.current_ = current;
    this.eventSelect_ = eventSelect;
    this.eventDetail_ = eventDetail;
    this.baseMapList_.Clear();
    yield break;
  }

  private IEnumerator Start()
  {
    MapEditBaseMapSelect editBaseMapSelect = this;
    while (editBaseMapSelect.towns_ == null)
      yield return (object) null;
    editBaseMapSelect.scroll_.Reset();
    if (editBaseMapSelect.towns_.Length != 0)
    {
      GameObject prefabList = (GameObject) null;
      Future<GameObject> ldprefab = MapEdit.Prefabs.base_map_select_list.Load<GameObject>();
      IEnumerator e = ldprefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      prefabList = ldprefab.Result;
      if (!Object.op_Equality((Object) prefabList, (Object) null))
      {
        ldprefab = (Future<GameObject>) null;
        GameObject prefabIcon = (GameObject) null;
        ldprefab = Res.Icons.UniqueIconPrefab.Load<GameObject>();
        e = ldprefab.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        prefabIcon = ldprefab.Result;
        if (!Object.op_Equality((Object) prefabIcon, (Object) null))
        {
          ldprefab = (Future<GameObject>) null;
          PlayerGuildTown[] playerGuildTownArray = editBaseMapSelect.towns_;
          for (int index = 0; index < playerGuildTownArray.Length; ++index)
          {
            PlayerGuildTown town = playerGuildTownArray[index];
            GameObject gameObject = prefabList.Clone();
            editBaseMapSelect.scroll_.Add(gameObject, true);
            MapEditBaseMapList cntl = gameObject.GetComponent<MapEditBaseMapList>();
            editBaseMapSelect.baseMapList_.Add(cntl);
            e = cntl.initialize(prefabIcon, town, editBaseMapSelect.eventSelect_, editBaseMapSelect.eventDetail_);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            if (editBaseMapSelect.current_ == town._master)
              cntl.setCurrent(true);
            cntl = (MapEditBaseMapList) null;
            town = (PlayerGuildTown) null;
          }
          playerGuildTownArray = (PlayerGuildTown[]) null;
          // ISSUE: method pointer
          editBaseMapSelect.scroll_.GridReposition(new UIGrid.OnReposition((object) editBaseMapSelect, __methodptr(\u003CStart\u003Eb__9_0)));
        }
      }
    }
  }

  public void setCurrent(int id)
  {
    if (this.current_ == id)
      return;
    MapEditBaseMapList mapEditBaseMapList1 = this.baseMapList_.FirstOrDefault<MapEditBaseMapList>((Func<MapEditBaseMapList, bool>) (m => m.town_._master == this.current_));
    if (Object.op_Inequality((Object) mapEditBaseMapList1, (Object) null))
      mapEditBaseMapList1.setCurrent(false);
    MapEditBaseMapList mapEditBaseMapList2 = this.baseMapList_.FirstOrDefault<MapEditBaseMapList>((Func<MapEditBaseMapList, bool>) (m => m.town_._master == id));
    if (Object.op_Inequality((Object) mapEditBaseMapList2, (Object) null))
      mapEditBaseMapList2.setCurrent(true);
    this.current_ = id;
  }
}
