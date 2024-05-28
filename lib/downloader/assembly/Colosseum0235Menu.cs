// Decompiled with JetBrains decompiler
// Type: Colosseum0235Menu
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
public class Colosseum0235Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel README;
  [SerializeField]
  protected UILabel TxtRankName;
  [SerializeField]
  protected UILabel TxtRankPoint;
  [SerializeField]
  protected UILabel TxtReward;
  [SerializeField]
  private NGxScroll scrollContainer;
  private int cellsPerPage;
  private int[] opponents;
  private ColosseumUtility.Info colosseumInfo;
  private Colosseum0235Scene.Param param;

  public virtual void Foreground()
  {
  }

  public virtual void VScrollBar()
  {
  }

  public override void onBackButton() => this.IbtnBack();

  public IEnumerator Initialize(Colosseum0235Scene.Param param)
  {
    Colosseum0235Menu colosseum0235Menu = this;
    colosseum0235Menu.param = param;
    ColosseumRank[] rankArray = ((IEnumerable<ColosseumRank>) MasterData.ColosseumRankList).Where<ColosseumRank>((Func<ColosseumRank, bool>) (v => v.ID <= param.viewUnlockId + 1)).OrderByDescending<ColosseumRank, int>((Func<ColosseumRank, int>) (v => v.ID)).ToArray<ColosseumRank>();
    IEnumerator e = Singleton<CommonRoot>.GetInstance().GetColosseumHeaderComponent().SetInfo(CommonColosseumHeader.BtnMode.Back, new Action(colosseum0235Menu.IbtnBack));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> unitPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    e = unitPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject unitPrefab = unitPrefabF.Result;
    Future<GameObject> gearPrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    e = gearPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gearPrefab = gearPrefabF.Result;
    Future<GameObject> uniquePrefabF = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    e = uniquePrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject uniquePrefab = uniquePrefabF.Result;
    Future<GameObject> prefabF = Res.Prefabs.colosseum.colosseum023_5.dir_Rank_Reward.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result;
    colosseum0235Menu.scrollContainer.Clear();
    colosseum0235Menu.cellsPerPage = rankArray.Length;
    for (int i = 1; i < colosseum0235Menu.cellsPerPage; ++i)
    {
      GameObject gameObject = prefab.Clone();
      colosseum0235Menu.scrollContainer.Add(gameObject);
      ColosseumRankPrefab component = gameObject.GetComponent<ColosseumRankPrefab>();
      if (rankArray[i].ID <= param.maxRank + 1)
      {
        int toPoint = i + 1 < rankArray.Length ? rankArray[i + 1].to_point : 0;
        int num = i == 0 ? (rankArray[0].ID <= param.maxRank ? rankArray[0].to_point : -1) : (rankArray[i - 1].ID <= param.maxRank + 1 ? rankArray[i - 1].to_point : -1);
        e = component.Set(rankArray[i], rankArray[i].ID == param.nowId, toPoint + 1, num + 1, gearPrefab, unitPrefab, uniquePrefab, new Action<ColosseumRank, int, int>(colosseum0235Menu.IbtnRankDetail));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
        component.Reset(gearPrefab);
    }
    if (param.isInit)
      colosseum0235Menu.scrollContainer.ResolvePosition(colosseum0235Menu.cellsPerPage - param.nowId);
    else
      colosseum0235Menu.scrollContainer.ResolvePosition(param.scrollPos);
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    Colosseum0234Scene.ChangeScene(this.param.opponents, this.param.collosseumInfo);
  }

  private void IbtnRankDetail(ColosseumRank rankInfo, int fromPoint, int nextPoint)
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    this.param.isInit = false;
    this.param.scrollPos = this.scrollContainer.GetScrollPosition();
    Colosseum02351Scene.ChangeScene(this.param, rankInfo, fromPoint, nextPoint);
  }
}
