// Decompiled with JetBrains decompiler
// Type: EquipmentDetailMenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EquipmentDetailMenuBase : BackButtonMenuBase
{
  private GameObject statusPrefab;

  protected List<IncrementalInfo> GetIncrementalParams(GearGear gear)
  {
    List<IncrementalInfo> collection = new List<IncrementalInfo>();
    List<IncrementalInfo> incrementalParams = new List<IncrementalInfo>();
    IncrementalInfo incrementalInfo1 = new IncrementalInfo(Consts.GetInstance().UNIT_00443_LUCK, gear.lucky_incremental);
    if (gear.lucky_incremental > 0)
      collection.Add(incrementalInfo1);
    else if (gear.lucky_incremental < 0)
      incrementalParams.Add(incrementalInfo1);
    IncrementalInfo incrementalInfo2 = new IncrementalInfo(Consts.GetInstance().UNIT_00443_DEXTERITY, gear.dexterity_incremental);
    if (gear.dexterity_incremental > 0)
      collection.Add(incrementalInfo2);
    else if (gear.dexterity_incremental < 0)
      incrementalParams.Add(incrementalInfo2);
    IncrementalInfo incrementalInfo3 = new IncrementalInfo(Consts.GetInstance().UNIT_00443_AGILITY, gear.agility_incremental);
    if (gear.agility_incremental > 0)
      collection.Add(incrementalInfo3);
    else if (gear.agility_incremental < 0)
      incrementalParams.Add(incrementalInfo3);
    IncrementalInfo incrementalInfo4 = new IncrementalInfo(Consts.GetInstance().UNIT_00443_MIND, gear.mind_incremental);
    if (gear.mind_incremental > 0)
      collection.Add(incrementalInfo4);
    else if (gear.mind_incremental < 0)
      incrementalParams.Add(incrementalInfo4);
    IncrementalInfo incrementalInfo5 = new IncrementalInfo(Consts.GetInstance().UNIT_00443_INTELLIGENCE, gear.intelligence_incremental);
    if (gear.intelligence_incremental > 0)
      collection.Add(incrementalInfo5);
    else if (gear.intelligence_incremental < 0)
      incrementalParams.Add(incrementalInfo5);
    IncrementalInfo incrementalInfo6 = new IncrementalInfo(Consts.GetInstance().UNIT_00443_VITALITY, gear.vitality_incremental);
    if (gear.vitality_incremental > 0)
      collection.Add(incrementalInfo6);
    else if (gear.vitality_incremental < 0)
      incrementalParams.Add(incrementalInfo6);
    IncrementalInfo incrementalInfo7 = new IncrementalInfo(Consts.GetInstance().UNIT_00443_STRENGTH, gear.strength_incremental);
    if (gear.strength_incremental > 0)
      collection.Add(incrementalInfo7);
    else if (gear.strength_incremental < 0)
      incrementalParams.Add(incrementalInfo7);
    IncrementalInfo incrementalInfo8 = new IncrementalInfo(Consts.GetInstance().UNIT_00443_HP, gear.hp_incremental);
    if (gear.hp_incremental > 0)
      collection.Add(incrementalInfo8);
    else if (gear.hp_incremental < 0)
      incrementalParams.Add(incrementalInfo8);
    incrementalParams.AddRange((IEnumerable<IncrementalInfo>) collection);
    return incrementalParams;
  }

  protected List<IncrementalInfo> GetIncrementalParams(PlayerItem gear)
  {
    List<IncrementalInfo> collection = new List<IncrementalInfo>();
    List<IncrementalInfo> incrementalParams = new List<IncrementalInfo>();
    IncrementalInfo incrementalInfo1 = new IncrementalInfo(Consts.GetInstance().UNIT_00443_LUCK, gear.lucky_incremental);
    if (gear.lucky_incremental > 0)
      collection.Add(incrementalInfo1);
    else if (gear.lucky_incremental < 0)
      incrementalParams.Add(incrementalInfo1);
    IncrementalInfo incrementalInfo2 = new IncrementalInfo(Consts.GetInstance().UNIT_00443_DEXTERITY, gear.dexterity_incremental);
    if (gear.dexterity_incremental > 0)
      collection.Add(incrementalInfo2);
    else if (gear.dexterity_incremental < 0)
      incrementalParams.Add(incrementalInfo2);
    IncrementalInfo incrementalInfo3 = new IncrementalInfo(Consts.GetInstance().UNIT_00443_AGILITY, gear.agility_incremental);
    if (gear.agility_incremental > 0)
      collection.Add(incrementalInfo3);
    else if (gear.agility_incremental < 0)
      incrementalParams.Add(incrementalInfo3);
    IncrementalInfo incrementalInfo4 = new IncrementalInfo(Consts.GetInstance().UNIT_00443_MIND, gear.mind_incremental);
    if (gear.mind_incremental > 0)
      collection.Add(incrementalInfo4);
    else if (gear.mind_incremental < 0)
      incrementalParams.Add(incrementalInfo4);
    IncrementalInfo incrementalInfo5 = new IncrementalInfo(Consts.GetInstance().UNIT_00443_INTELLIGENCE, gear.intelligence_incremental);
    if (gear.intelligence_incremental > 0)
      collection.Add(incrementalInfo5);
    else if (gear.intelligence_incremental < 0)
      incrementalParams.Add(incrementalInfo5);
    IncrementalInfo incrementalInfo6 = new IncrementalInfo(Consts.GetInstance().UNIT_00443_VITALITY, gear.vitality_incremental);
    if (gear.vitality_incremental > 0)
      collection.Add(incrementalInfo6);
    else if (gear.vitality_incremental < 0)
      incrementalParams.Add(incrementalInfo6);
    IncrementalInfo incrementalInfo7 = new IncrementalInfo(Consts.GetInstance().UNIT_00443_STRENGTH, gear.strength_incremental);
    if (gear.strength_incremental > 0)
      collection.Add(incrementalInfo7);
    else if (gear.strength_incremental < 0)
      incrementalParams.Add(incrementalInfo7);
    IncrementalInfo incrementalInfo8 = new IncrementalInfo(Consts.GetInstance().UNIT_00443_HP, gear.hp_incremental);
    if (gear.hp_incremental > 0)
      collection.Add(incrementalInfo8);
    else if (gear.hp_incremental < 0)
      incrementalParams.Add(incrementalInfo8);
    incrementalParams.AddRange((IEnumerable<IncrementalInfo>) collection);
    return incrementalParams;
  }

  protected IEnumerator SetIncrementalParameter(ItemInfo gear, GameObject target)
  {
    List<IncrementalInfo> list = (List<IncrementalInfo>) null;
    list = !(gear.playerItem == (PlayerItem) null) ? this.GetIncrementalParams(gear.playerItem) : this.GetIncrementalParams(gear.gear);
    target.SetActive(false);
    if (list.Count > 0)
    {
      target.SetActive(true);
      IEnumerator e;
      if (Object.op_Equality((Object) this.statusPrefab, (Object) null))
      {
        Future<GameObject> statusPrefabF = Res.Prefabs.unit004_4_3.dir_AddStatus.Load<GameObject>();
        e = statusPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.statusPrefab = statusPrefabF.Result.Clone(target.transform);
        statusPrefabF = (Future<GameObject>) null;
      }
      e = this.statusPrefab.GetComponent<DirAddStatus>().Init(list);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  protected IEnumerator SetIncrementalParameter(GearGear gear, GameObject target)
  {
    List<IncrementalInfo> list = this.GetIncrementalParams(gear);
    target.SetActive(false);
    if (list.Count > 0)
    {
      target.SetActive(true);
      IEnumerator e;
      if (Object.op_Equality((Object) this.statusPrefab, (Object) null))
      {
        Future<GameObject> statusPrefabF = Res.Prefabs.unit004_4_3.dir_AddStatus.Load<GameObject>();
        e = statusPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.statusPrefab = statusPrefabF.Result.Clone(target.transform);
        statusPrefabF = (Future<GameObject>) null;
      }
      e = this.statusPrefab.GetComponent<DirAddStatus>().Init(list);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public override void onBackButton()
  {
  }
}
