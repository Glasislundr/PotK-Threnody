// Decompiled with JetBrains decompiler
// Type: TowerUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TowerUtil
{
  public const int SupplyBoxTypeID = 3;
  public static TowerDeckUnit[] towerDeckUnits;
  private static int borderLv = -1;
  private static int maxUnitNum = -1;
  private static int recoveryCoinNum = -1;
  public static int PayRecoveryCoinNum = 0;
  private static int restartCoinNum = -1;
  private static int maxTowerMedalNum = -1;
  private static int maxRankingNum = -1;
  public static string BgmFile = string.Empty;
  public static string BgmName = string.Empty;
  public static TowerPlayer TowerPlayer = (TowerPlayer) null;

  public static int BorderLevel
  {
    get
    {
      if (TowerUtil.borderLv == -1)
      {
        int? nullable = MasterData.TowerCommon.FirstIndexOrNull<KeyValuePair<int, TowerCommon>>((Func<KeyValuePair<int, TowerCommon>, bool>) (x => x.Value.key.Equals("TOWER_UNIT_LOWER_LEVEL")));
        if (!nullable.HasValue)
          return -1;
        TowerUtil.borderLv = MasterData.TowerCommonList[nullable.Value].value;
      }
      return TowerUtil.borderLv;
    }
  }

  public static int MaxUnitNum
  {
    get
    {
      if (TowerUtil.maxUnitNum == -1)
      {
        int? nullable = MasterData.TowerCommon.FirstIndexOrNull<KeyValuePair<int, TowerCommon>>((Func<KeyValuePair<int, TowerCommon>, bool>) (x => x.Value.key.Equals("TOWER_ENTER_UNIT_COUNT")));
        if (!nullable.HasValue)
          return -1;
        TowerUtil.maxUnitNum = MasterData.TowerCommonList[nullable.Value].value;
      }
      return TowerUtil.maxUnitNum;
    }
  }

  public static int RecoveryCoinNum
  {
    get
    {
      if (TowerUtil.recoveryCoinNum == -1)
      {
        int? nullable = MasterData.TowerCommon.FirstIndexOrNull<KeyValuePair<int, TowerCommon>>((Func<KeyValuePair<int, TowerCommon>, bool>) (x => x.Value.key.Equals("TOWER_RECOVER_COIN_COUNT")));
        if (!nullable.HasValue)
          return -1;
        TowerUtil.recoveryCoinNum = MasterData.TowerCommonList[nullable.Value].value;
      }
      return TowerUtil.recoveryCoinNum;
    }
  }

  public static int RestartCoinNum
  {
    get
    {
      if (TowerUtil.restartCoinNum == -1)
      {
        int? nullable = MasterData.TowerCommon.FirstIndexOrNull<KeyValuePair<int, TowerCommon>>((Func<KeyValuePair<int, TowerCommon>, bool>) (x => x.Value.key.Equals("TOWER_RESTART_COIN_COUNT")));
        if (!nullable.HasValue)
          return -1;
        TowerUtil.restartCoinNum = MasterData.TowerCommonList[nullable.Value].value;
      }
      return TowerUtil.restartCoinNum;
    }
  }

  public static int MaxTowerMedalNum
  {
    get
    {
      if (TowerUtil.maxTowerMedalNum == -1)
      {
        int? nullable = MasterData.TowerCommon.FirstIndexOrNull<KeyValuePair<int, TowerCommon>>((Func<KeyValuePair<int, TowerCommon>, bool>) (x => x.Value.key.Equals("TOWER_MEDAL_LIMIT")));
        if (!nullable.HasValue)
          return -1;
        TowerUtil.maxTowerMedalNum = MasterData.TowerCommonList[nullable.Value].value;
      }
      return TowerUtil.maxTowerMedalNum;
    }
  }

  public static int MaxRankingNum
  {
    get
    {
      if (TowerUtil.maxRankingNum == -1)
      {
        int? nullable = MasterData.TowerCommon.FirstIndexOrNull<KeyValuePair<int, TowerCommon>>((Func<KeyValuePair<int, TowerCommon>, bool>) (x => x.Value.key.Equals("TOWER_RANKING_TARGET")));
        if (!nullable.HasValue)
          return -1;
        TowerUtil.maxRankingNum = MasterData.TowerCommonList[nullable.Value].value;
      }
      return TowerUtil.maxRankingNum;
    }
  }

  public static int GetHp(int hp, float rate)
  {
    int hp1 = Mathf.CeilToInt((float) ((double) hp * (double) rate / 100.0));
    if ((double) rate < 100.0 && hp1 == hp)
      --hp1;
    return hp1;
  }

  public static void GotoMypage()
  {
    Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = true;
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = -1;
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = -1;
    MypageScene.ChangeScene();
  }

  public enum UnitSelectionMode
  {
    Auto,
    Manual,
  }

  public enum SequenceType
  {
    None = -1, // 0xFFFFFFFF
    Start = 0,
    Recovery = 1,
    Restart = 2,
  }

  public enum UnitSelectionOrder
  {
    LEVEL,
    ATTRIBUTE,
    WEAPON,
    FAVORITE,
  }
}
