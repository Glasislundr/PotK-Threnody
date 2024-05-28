// Decompiled with JetBrains decompiler
// Type: GameCore.PerformanceConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using UnityEngine;

#nullable disable
namespace GameCore
{
  public class PerformanceConfig
  {
    public static bool IsEnablePerformanceTest;
    public static bool IsForceTest;
    public static float CreateSceneTime;
    public static float LoadingBlanceTime = 0.2f;
    private const int tuningNotUseDeepCopyId = 1;
    private const int tuningNotAutoSaveBeforeDuelId = 2;
    private const int tuningEventTopSettingId = 3;
    private const int tuningTitleToHomeId = 4;
    private const int tuningCommonTextureLoaded = 5;
    private const int tuningGachaInitializeId = 7;
    private const int tuningMissionInitializeId = 8;
    private const int enableWebAutoRetryId = 10;
    private const int enablePvPAutoButtonId = 11;
    private const int enableReisouMixerButtonId = 12;
    private const int enableOnedariDateID = 13;
    private const int enableQuestResult = 14;
    private const int enableGear3 = 15;
    private const int enableSendErrorTracker = 16;
    private bool isSetupSpeck;
    private bool isLowMemory;
    private bool isGachaLowMemory;
    private bool isHiMemory;
    private static PerformanceConfig instance;
    private bool? _EnablePvpAutoButton;
    private bool? _EnableReisouMixerButton;

    public static PerformanceConfig GetInstance()
    {
      if (PerformanceConfig.instance == null)
        PerformanceConfig.instance = new PerformanceConfig();
      return PerformanceConfig.instance;
    }

    public bool IsNotUseDeepCopy
    {
      get
      {
        AppSetupTuning appSetupTuning = MasterData.AppSetupTuning[1];
        return appSetupTuning != null && appSetupTuning.IsEnable;
      }
    }

    public bool IsNotAutoSaveBeforeDuel
    {
      get => !MasterData.AppSetupTuning.ContainsKey(2) || MasterData.AppSetupTuning[2].IsEnable;
    }

    public bool IsTuningEventTopSetting
    {
      get => !MasterData.AppSetupTuning.ContainsKey(3) || MasterData.AppSetupTuning[3].IsEnable;
    }

    public bool IsTuningTitleToHome
    {
      get => !MasterData.AppSetupTuning.ContainsKey(4) || MasterData.AppSetupTuning[4].IsEnable;
    }

    public bool IsTuningCommonTextureLoaded
    {
      get
      {
        if (!this.IsHiMemory)
          return false;
        return !MasterData.AppSetupTuning.ContainsKey(5) || MasterData.AppSetupTuning[5].IsEnable;
      }
    }

    public bool IsTuningGachaInitialize
    {
      get => !MasterData.AppSetupTuning.ContainsKey(7) || MasterData.AppSetupTuning[7].IsEnable;
    }

    public bool IsTuningMissionInitialize
    {
      get => !MasterData.AppSetupTuning.ContainsKey(8) || MasterData.AppSetupTuning[8].IsEnable;
    }

    public bool IsLowMemory
    {
      get
      {
        if (!this.isSetupSpeck)
          this.SetupSpeck();
        return this.isLowMemory;
      }
    }

    public bool IsGachaLowMemory
    {
      get
      {
        if (!this.isSetupSpeck)
          this.SetupSpeck();
        return this.isGachaLowMemory;
      }
    }

    public bool IsHiMemory
    {
      get
      {
        if (!this.isSetupSpeck)
          this.SetupSpeck();
        return this.isHiMemory;
      }
    }

    public bool IsSpeedPriority { get; set; }

    private void SetupSpeck()
    {
      this.isLowMemory = SystemInfo.systemMemorySize < 2048;
      this.isHiMemory = SystemInfo.systemMemorySize >= this.GetHiSpecMemorySize();
      this.isGachaLowMemory = SystemInfo.systemMemorySize < 1900;
      this.isSetupSpeck = true;
    }

    private int GetHiSpecMemorySize() => 4096;

    public bool EnableWebAutoRetry
    {
      get
      {
        AppSetupTuning appSetupTuning = (AppSetupTuning) null;
        return MasterData.AppSetupTuning.TryGetValue(10, out appSetupTuning) && appSetupTuning.IsEnable;
      }
    }

    public bool EnablePvPAutoButton
    {
      get
      {
        if (this._EnablePvpAutoButton.HasValue)
          return this._EnablePvpAutoButton.Value;
        if (!MasterData.AppSetupTuning.ContainsKey(11))
          return true;
        AppSetupTuning appSetupTuning = MasterData.AppSetupTuning[11];
        this._EnablePvpAutoButton = new bool?(appSetupTuning.IsEnable);
        return appSetupTuning.IsEnable;
      }
    }

    public bool EnableReisouMixerButton
    {
      get
      {
        if (this._EnableReisouMixerButton.HasValue)
          return this._EnableReisouMixerButton.Value;
        if (!MasterData.AppSetupTuning.ContainsKey(12))
          return true;
        AppSetupTuning appSetupTuning = MasterData.AppSetupTuning[12];
        this._EnableReisouMixerButton = new bool?(appSetupTuning.IsEnable);
        return appSetupTuning.IsEnable;
      }
    }

    public bool IsOnedariDate
    {
      get => !MasterData.AppSetupTuning.ContainsKey(13) || MasterData.AppSetupTuning[13].IsEnable;
    }

    public bool IsQuestResult
    {
      get => !MasterData.AppSetupTuning.ContainsKey(14) || MasterData.AppSetupTuning[14].IsEnable;
    }

    public bool IsGear3
    {
      get => !MasterData.AppSetupTuning.ContainsKey(15) || MasterData.AppSetupTuning[15].IsEnable;
    }

    public bool IsSendErrorTracker
    {
      get => !MasterData.AppSetupTuning.ContainsKey(16) || MasterData.AppSetupTuning[16].IsEnable;
    }
  }
}
