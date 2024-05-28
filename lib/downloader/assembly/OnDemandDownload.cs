// Decompiled with JetBrains decompiler
// Type: OnDemandDownload
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class OnDemandDownload
{
  private static Modified<PlayerUnit[]> playerUnitsObserver = (Modified<PlayerUnit[]>) null;
  private static Modified<PlayerMaterialUnit[]> playerMaterialUnitsObserver = (Modified<PlayerMaterialUnit[]>) null;
  private static HashSet<int> checkedUnitIds = new HashSet<int>();

  public static void InitVariable()
  {
    OnDemandDownload.playerUnitsObserver = (Modified<PlayerUnit[]>) null;
    OnDemandDownload.playerMaterialUnitsObserver = (Modified<PlayerMaterialUnit[]>) null;
    OnDemandDownload.checkedUnitIds = new HashSet<int>();
  }

  public static long SizeOfLoadAllUnits()
  {
    ResourceManager rm = Singleton<ResourceManager>.GetInstance();
    string[] array = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).SelectMany<UnitUnit, string>((Func<UnitUnit, IEnumerable<string>>) (x => (IEnumerable<string>) rm.PathsFromUnit(x))).ToArray<string>();
    DLC dlc = rm.CreateDLC(((IEnumerable<string>) array).ToArray<string>());
    return dlc.DownloadRequired ? dlc.GetStoreSize() : 0L;
  }

  public static IEnumerator WaitLoadAllUnits(bool confirmDLC)
  {
    IEnumerator e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<UnitUnit>) MasterData.UnitUnitList, confirmDLC);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator WaitLoadHasUnitResource(bool confirmDLC, bool isStartupSequence = false)
  {
    OnDemandDownload.playerUnitsObserver = OnDemandDownload.playerUnitsObserver ?? SMManager.Observe<PlayerUnit[]>();
    OnDemandDownload.playerMaterialUnitsObserver = OnDemandDownload.playerMaterialUnitsObserver ?? SMManager.Observe<PlayerMaterialUnit[]>();
    IEnumerator e;
    if (OnDemandDownload.playerUnitsObserver != null && OnDemandDownload.playerUnitsObserver.IsChangedOnce())
    {
      e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) OnDemandDownload.playerUnitsObserver.Value, confirmDLC, isStartupSequence: isStartupSequence);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (OnDemandDownload.playerMaterialUnitsObserver != null && OnDemandDownload.playerMaterialUnitsObserver.IsChangedOnce())
    {
      e = OnDemandDownload.WaitLoadMaterialUnitResource((IEnumerable<PlayerMaterialUnit>) OnDemandDownload.playerMaterialUnitsObserver.Value, confirmDLC, isStartupSequence);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static IEnumerator WaitLoadUnitResource(
    IEnumerable<PlayerUnit> xs,
    bool confirmDLC,
    IEnumerable<string> file_patterns = null,
    bool isStartupSequence = false)
  {
    if (xs != null)
    {
      IEnumerator e = OnDemandDownload.WaitLoadUnitResource(xs.Select<PlayerUnit, UnitUnit>((Func<PlayerUnit, UnitUnit>) (x => x.unit)), confirmDLC, file_patterns, isStartupSequence);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static IEnumerator WaitLoadMaterialUnitResource(
    IEnumerable<PlayerMaterialUnit> xs,
    bool confirmDLC,
    bool isStartupSequence = false)
  {
    if (xs != null)
    {
      IEnumerator e = OnDemandDownload.WaitLoadUnitResource(xs.Select<PlayerMaterialUnit, UnitUnit>((Func<PlayerMaterialUnit, UnitUnit>) (x => x.unit)), confirmDLC, isStartupSequence: isStartupSequence);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static IEnumerator WaitLoadUnitResource(
    IEnumerable<UnitUnit> xs,
    bool confirmDLC,
    IEnumerable<string> file_patterns = null,
    bool isStartupSequence = false)
  {
    ResourceManager rm = Singleton<ResourceManager>.GetInstance();
    IEnumerable<UnitUnit> checkUnits = xs.Where<UnitUnit>((Func<UnitUnit, bool>) (x => x != null && !OnDemandDownload.checkedUnitIds.Contains(x.ID)));
    IEnumerable<string> strings = checkUnits.SelectMany<UnitUnit, string>((Func<UnitUnit, IEnumerable<string>>) (x => (IEnumerable<string>) rm.PathsFromUnit(x)));
    if (file_patterns != null && file_patterns.Count<string>() > 0)
      strings = strings.Where<string>((Func<string, bool>) (x => file_patterns.Any<string>((Func<string, bool>) (pattern => x.Contains(pattern)))));
    IEnumerator e = OnDemandDownload.WaitLoadUnitResource(strings, confirmDLC, isStartupSequence);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (file_patterns == null || file_patterns != null && file_patterns.Count<string>() == 0)
    {
      foreach (UnitUnit unitUnit in checkUnits)
        OnDemandDownload.checkedUnitIds.Add(unitUnit.ID);
    }
  }

  public static IEnumerator WaitLoadUnitResource(
    IEnumerable<string> paths,
    bool confirmDLC,
    bool isStartupSequence = false)
  {
    App.SetAutoSleep(false);
    IEnumerator e = OnDemandDownload.waitLoadResource(paths, confirmDLC, isStartupSequence: isStartupSequence);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    App.SetAutoSleep(true);
  }

  public static IEnumerator waitLoadMovieResource(IEnumerable<string> paths, bool confirmDLC)
  {
    App.SetAutoSleep(false);
    IEnumerator e = OnDemandDownload.waitLoadResource(paths, confirmDLC);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    App.SetAutoSleep(true);
  }

  public static IEnumerator waitLoadSomethingResource(
    IEnumerable<string> paths,
    bool confirmDLC,
    bool fileCheckDisable = false)
  {
    App.SetAutoSleep(false);
    IEnumerator e = OnDemandDownload.waitLoadResource((IEnumerable<string>) paths.Distinct<string>().ToArray<string>(), confirmDLC, fileCheckDisable);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    App.SetAutoSleep(true);
  }

  private static IEnumerator waitLoadResource(
    IEnumerable<string> paths,
    bool confirmDLC,
    bool fileCheckDisable = false,
    bool isStartupSequence = false)
  {
    if (!Object.op_Equality((Object) Singleton<CommonRoot>.GetInstance(), (Object) null))
    {
      bool showLoading = !Singleton<CommonRoot>.GetInstance().isLoading;
      bool touchBlock = !Singleton<CommonRoot>.GetInstance().isTouchBlock;
      if (showLoading)
      {
        Singleton<CommonRoot>.GetInstance().loadingMode = 1;
        Singleton<CommonRoot>.GetInstance().isLoading = true;
        Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
      }
      if (touchBlock)
        Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
label_6:
      DLC dlc = Singleton<ResourceManager>.GetInstance().CreateDLC(paths.ToArray<string>(), fileCheckDisable, isStartupSequence);
      if (dlc.DownloadRequired)
      {
        long downloadSize = dlc.GetDownloadSize();
        if (confirmDLC)
        {
          DLC[] loaders = new DLC[1]{ dlc };
          int tempTipsLoadingNumber = Singleton<CommonRoot>.GetInstance().loadingMode;
          if (Singleton<CommonRoot>.GetInstance().loadingMode == 4)
          {
            Singleton<CommonRoot>.GetInstance().isLoading = false;
            Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
          }
          bool toNext = false;
          yield return (object) ModalDownloadWindow.Show((IEnumerable<DLC>) loaders, (Action) (() => toNext = true), Consts.GetInstance().dlc_comfirm_desc1);
          if (Singleton<CommonRoot>.GetInstance().loadingMode != tempTipsLoadingNumber)
            Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(tempTipsLoadingNumber);
          if (!toNext)
          {
            StartScript.Restart();
            yield break;
          }
        }
        DownloadGauge gauge = Singleton<CommonRoot>.GetInstance().viewDownloadGauge();
        if (Object.op_Inequality((Object) gauge, (Object) null))
          gauge.setValue(0, 100);
        IEnumerator e = dlc.Start((MonoBehaviour) Singleton<ResourceManager>.GetInstance());
        while (e.MoveNext())
        {
          yield return e.Current;
          if (Object.op_Inequality((Object) gauge, (Object) null))
            gauge.setValue(Mathf.Min(Mathf.RoundToInt((float) ((double) dlc.GetDownloadedSize() / (double) downloadSize * 100.0)), 100), 100);
        }
        e = (IEnumerator) null;
        gauge = (DownloadGauge) null;
      }
      if (dlc.Error != null)
      {
        Debug.LogError((object) dlc.Error);
        bool waitRetry = true;
        ModalWindow.ShowRetryTitle(Consts.GetInstance().dlc_fail_download_title, dlc.Error, (Action) (() => waitRetry = false), (Action) (() => StartScript.Restart()));
        while (waitRetry)
          yield return (object) new WaitForEndOfFrame();
        goto label_6;
      }
      else
      {
        if (showLoading)
        {
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
          Singleton<CommonRoot>.GetInstance().isLoading = false;
        }
        if (touchBlock)
          Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
        dlc = (DLC) null;
      }
    }
  }
}
