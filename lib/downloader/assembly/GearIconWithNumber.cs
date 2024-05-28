// Decompiled with JetBrains decompiler
// Type: GearIconWithNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class GearIconWithNumber : MonoBehaviour
{
  private GameObject gearIconPrefab;
  public GameObject gearIconParent;
  public UILabel numberLabel;
  private ItemIcon itemIcon;
  private GearGear gearData;

  public ItemIcon ItemIcon => this.itemIcon;

  private void onClickButton()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("guide011_4_1", true, (object) this.gearData);
  }

  public IEnumerator SetGear(GearGear gear, bool contains, BattleInfo info = null)
  {
    GearIconWithNumber gearIconWithNumber = this;
    gearIconWithNumber.gearData = gear;
    Future<GameObject> f;
    IEnumerator e;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea && info != null && info.seaQuest != null)
    {
      f = Res.Prefabs.Sea.ItemIcon.prefab_sea.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      gearIconWithNumber.gearIconPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    else
    {
      f = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      gearIconWithNumber.gearIconPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    ItemIcon itemIcon = gearIconWithNumber.gearIconPrefab.CloneAndGetComponent<ItemIcon>(gearIconWithNumber.gearIconParent);
    GearGear gearData = contains ? gearIconWithNumber.gearData : (GearGear) null;
    e = itemIcon.InitByGear(gearData, gearData != null ? gearData.GetElement() : CommonElement.none);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (contains)
      EventDelegate.Add(itemIcon.gear.button.onClick, new EventDelegate.Callback(gearIconWithNumber.onClickButton));
    gearIconWithNumber.numberLabel.SetTextLocalize(string.Format("NO.{0:D3}", (object) (gearIconWithNumber.gearData.ID % 1000)));
    gearIconWithNumber.itemIcon = itemIcon;
  }

  public IEnumerator SetGear(PlayerItem gear, bool contains, BattleInfo info = null)
  {
    GearIconWithNumber gearIconWithNumber = this;
    gearIconWithNumber.gearData = gear.gear;
    Future<GameObject> f;
    IEnumerator e;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea && info != null && info.seaQuest != null)
    {
      f = Res.Prefabs.Sea.ItemIcon.prefab_sea.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      gearIconWithNumber.gearIconPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    else
    {
      f = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      gearIconWithNumber.gearIconPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    ItemIcon itemIcon = gearIconWithNumber.gearIconPrefab.CloneAndGetComponent<ItemIcon>(gearIconWithNumber.gearIconParent);
    e = itemIcon.InitByPlayerItem(gear);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (contains)
      EventDelegate.Add(itemIcon.gear.button.onClick, new EventDelegate.Callback(gearIconWithNumber.onClickButton));
    gearIconWithNumber.numberLabel.SetTextLocalize(string.Format("NO.{0:D3}", (object) (gearIconWithNumber.gearData.ID % 1000)));
    gearIconWithNumber.itemIcon = itemIcon;
  }
}
