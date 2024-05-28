// Decompiled with JetBrains decompiler
// Type: PopupBattleMissionList
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
public class PopupBattleMissionList : BackButtonMenuBase
{
  [SerializeField]
  private battle01718aMissionList mMissionList;
  private GameObject mItemPrefab;

  public IEnumerator InitializeCorps(int settingId, int[] clearIds)
  {
    IEnumerator e = this.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    CorpsMissionReward[] array = ((IEnumerable<CorpsMissionReward>) MasterData.CorpsMissionRewardList).Where<CorpsMissionReward>((Func<CorpsMissionReward, bool>) (x => x.setting_id == settingId)).OrderBy<CorpsMissionReward, int>((Func<CorpsMissionReward, int>) (x => x.priority)).ToArray<CorpsMissionReward>();
    e = this.mMissionList.InitValue(clearIds, array, this.mItemPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator LoadResources()
  {
    Future<GameObject> prefab = new ResourceObject("Prefabs/quest002_2/dir_Misson_List_Battle_Item").Load<GameObject>();
    IEnumerator future = prefab.Wait();
    while (future.MoveNext())
      yield return future.Current;
    future = (IEnumerator) null;
    this.mItemPrefab = prefab.Result;
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.mMissionList.ibtnBack();
  }
}
