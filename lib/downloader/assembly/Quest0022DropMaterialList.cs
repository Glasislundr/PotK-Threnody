// Decompiled with JetBrains decompiler
// Type: Quest0022DropMaterialList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest0022DropMaterialList : Quest0022MissionDescriptions
{
  public List<UI2DSprite> thum_list_;

  public void Init()
  {
  }

  public IEnumerator Init(int quest_s_id, CommonQuestType quest_type)
  {
    Future<GameObject> fprefab = (Future<GameObject>) null;
    this.ClearThum();
    foreach (QuestCommonDrop obj in ((IEnumerable<QuestCommonDrop>) MasterData.QuestCommonDropList).Where<QuestCommonDrop>((Func<QuestCommonDrop, bool>) (x => x.quest_type == quest_type)).Where<QuestCommonDrop>((Func<QuestCommonDrop, bool>) (x => x.quest_s_id == quest_s_id)).OrderBy<QuestCommonDrop, int>((Func<QuestCommonDrop, int>) (x => x.priority)).ToList<QuestCommonDrop>())
    {
      UnitIcon unitIcon;
      IEnumerator e;
      switch (obj.entity_type)
      {
        case MasterDataTable.CommonRewardType.unit:
        case MasterDataTable.CommonRewardType.material_unit:
          fprefab = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
          e = fprefab.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          GameObject result1 = fprefab.Result;
          unitIcon = this.SetThum(obj.priority, result1).GetComponent<UnitIcon>();
          e = unitIcon.SetUnit(MasterData.UnitUnit[obj.entity_id], MasterData.UnitUnit[obj.entity_id].GetElement(), false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          unitIcon.setLevelText("1");
          unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
          break;
        case MasterDataTable.CommonRewardType.supply:
        case MasterDataTable.CommonRewardType.gear:
        case MasterDataTable.CommonRewardType.material_gear:
        case MasterDataTable.CommonRewardType.gear_body:
          fprefab = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
          e = fprefab.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          GameObject result2 = fprefab.Result;
          e = this.SetThum(obj.priority, result2).GetComponent<ItemIcon>().InitByQuestDrop(obj);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
      }
      unitIcon = (UnitIcon) null;
    }
  }

  public void ClearThum()
  {
    this.thum_list_.ForEach((Action<UI2DSprite>) (x => ((Component) x).transform.GetChildren().ForEach<Transform>((Action<Transform>) (y => Object.Destroy((Object) ((Component) y).gameObject)))));
  }

  public GameObject SetThum(int id, GameObject prefab)
  {
    return prefab.Clone(((Component) this.thum_list_[id - 1]).transform);
  }
}
