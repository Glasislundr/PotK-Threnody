// Decompiled with JetBrains decompiler
// Type: com.adjust.sdk.AdjustConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace com.adjust.sdk
{
  public class AdjustConfig
  {
    public const string AdjustUrlStrategyChina = "china";
    public const string AdjustUrlStrategyIndia = "india";
    public const string AdjustDataResidencyEU = "data-residency-eu";
    public const string AdjustAdRevenueSourceMopub = "mopub";
    public const string AdjustAdRevenueSourceAdmob = "admob";
    public const string AdjustAdRevenueSourceFbNativeAd = "facebook_native_ad";
    public const string AdjustAdRevenueSourceFbAudienceNetwork = "facebook_audience_network";
    public const string AdjustAdRevenueSourceIronsource = "ironsource";
    public const string AdjustAdRevenueSourceFyber = "fyber";
    public const string AdjustAdRevenueSourceAerserv = "aerserv";
    public const string AdjustAdRevenueSourceAppodeal = "appodeal";
    public const string AdjustAdRevenueSourceAdincube = "adincube";
    public const string AdjustAdRevenueSourceFusePowered = "fusepowered";
    public const string AdjustAdRevenueSourceAddaptr = "addapptr";
    public const string AdjustAdRevenueSourceMillenialMediation = "millennial_mediation";
    public const string AdjustAdRevenueSourceFlurry = "flurry";
    public const string AdjustAdRevenueSourceAdmost = "admost";
    public const string AdjustAdRevenueSourceDeltadna = "deltadna";
    public const string AdjustAdRevenueSourceUpsight = "upsight";
    public const string AdjustAdRevenueSourceUnityads = "unityads";
    public const string AdjustAdRevenueSourceAdtoapp = "adtoapp";
    public const string AdjustAdRevenueSourceTapdaq = "tapdaq";
    internal string appToken;
    internal string sceneName;
    internal string userAgent;
    internal string defaultTracker;
    internal string externalDeviceId;
    internal string urlStrategy;
    internal long? info1;
    internal long? info2;
    internal long? info3;
    internal long? info4;
    internal long? secretId;
    internal double? delayStart;
    internal bool? isDeviceKnown;
    internal bool? sendInBackground;
    internal bool? eventBufferingEnabled;
    internal bool? allowSuppressLogLevel;
    internal bool? needsCost;
    internal bool launchDeferredDeeplink;
    internal AdjustLogLevel? logLevel;
    internal AdjustEnvironment environment;
    internal Action<string> deferredDeeplinkDelegate;
    internal Action<AdjustEventSuccess> eventSuccessDelegate;
    internal Action<AdjustEventFailure> eventFailureDelegate;
    internal Action<AdjustSessionSuccess> sessionSuccessDelegate;
    internal Action<AdjustSessionFailure> sessionFailureDelegate;
    internal Action<AdjustAttribution> attributionChangedDelegate;
    internal bool? readImei;
    internal bool? preinstallTrackingEnabled;
    internal string processName;
    internal bool? allowiAdInfoReading;
    internal bool? allowAdServicesInfoReading;
    internal bool? allowIdfaReading;
    internal bool? skAdNetworkHandling;
    internal Action<string> logDelegate;

    public AdjustConfig(string appToken, AdjustEnvironment environment)
    {
      this.sceneName = "";
      this.processName = "";
      this.appToken = appToken;
      this.environment = environment;
    }

    public AdjustConfig(string appToken, AdjustEnvironment environment, bool allowSuppressLogLevel)
    {
      this.sceneName = "";
      this.processName = "";
      this.appToken = appToken;
      this.environment = environment;
      this.allowSuppressLogLevel = new bool?(allowSuppressLogLevel);
    }

    public void setLogLevel(AdjustLogLevel logLevel)
    {
      this.logLevel = new AdjustLogLevel?(logLevel);
    }

    public void setDefaultTracker(string defaultTracker) => this.defaultTracker = defaultTracker;

    public void setExternalDeviceId(string externalDeviceId)
    {
      this.externalDeviceId = externalDeviceId;
    }

    public void setLaunchDeferredDeeplink(bool launchDeferredDeeplink)
    {
      this.launchDeferredDeeplink = launchDeferredDeeplink;
    }

    public void setSendInBackground(bool sendInBackground)
    {
      this.sendInBackground = new bool?(sendInBackground);
    }

    public void setEventBufferingEnabled(bool eventBufferingEnabled)
    {
      this.eventBufferingEnabled = new bool?(eventBufferingEnabled);
    }

    public void setNeedsCost(bool needsCost) => this.needsCost = new bool?(needsCost);

    public void setDelayStart(double delayStart) => this.delayStart = new double?(delayStart);

    public void setUserAgent(string userAgent) => this.userAgent = userAgent;

    public void setIsDeviceKnown(bool isDeviceKnown)
    {
      this.isDeviceKnown = new bool?(isDeviceKnown);
    }

    public void setUrlStrategy(string urlStrategy) => this.urlStrategy = urlStrategy;

    public void deactivateSKAdNetworkHandling() => this.skAdNetworkHandling = new bool?(true);

    public void setDeferredDeeplinkDelegate(
      Action<string> deferredDeeplinkDelegate,
      string sceneName = "Adjust")
    {
      this.deferredDeeplinkDelegate = deferredDeeplinkDelegate;
      this.sceneName = sceneName;
    }

    public Action<string> getDeferredDeeplinkDelegate() => this.deferredDeeplinkDelegate;

    public void setAttributionChangedDelegate(
      Action<AdjustAttribution> attributionChangedDelegate,
      string sceneName = "Adjust")
    {
      this.attributionChangedDelegate = attributionChangedDelegate;
      this.sceneName = sceneName;
    }

    public Action<AdjustAttribution> getAttributionChangedDelegate()
    {
      return this.attributionChangedDelegate;
    }

    public void setEventSuccessDelegate(
      Action<AdjustEventSuccess> eventSuccessDelegate,
      string sceneName = "Adjust")
    {
      this.eventSuccessDelegate = eventSuccessDelegate;
      this.sceneName = sceneName;
    }

    public Action<AdjustEventSuccess> getEventSuccessDelegate() => this.eventSuccessDelegate;

    public void setEventFailureDelegate(
      Action<AdjustEventFailure> eventFailureDelegate,
      string sceneName = "Adjust")
    {
      this.eventFailureDelegate = eventFailureDelegate;
      this.sceneName = sceneName;
    }

    public Action<AdjustEventFailure> getEventFailureDelegate() => this.eventFailureDelegate;

    public void setSessionSuccessDelegate(
      Action<AdjustSessionSuccess> sessionSuccessDelegate,
      string sceneName = "Adjust")
    {
      this.sessionSuccessDelegate = sessionSuccessDelegate;
      this.sceneName = sceneName;
    }

    public Action<AdjustSessionSuccess> getSessionSuccessDelegate() => this.sessionSuccessDelegate;

    public void setSessionFailureDelegate(
      Action<AdjustSessionFailure> sessionFailureDelegate,
      string sceneName = "Adjust")
    {
      this.sessionFailureDelegate = sessionFailureDelegate;
      this.sceneName = sceneName;
    }

    public Action<AdjustSessionFailure> getSessionFailureDelegate() => this.sessionFailureDelegate;

    public void setAppSecret(long secretId, long info1, long info2, long info3, long info4)
    {
      this.secretId = new long?(secretId);
      this.info1 = new long?(info1);
      this.info2 = new long?(info2);
      this.info3 = new long?(info3);
      this.info4 = new long?(info4);
    }

    public void setAllowiAdInfoReading(bool allowiAdInfoReading)
    {
      this.allowiAdInfoReading = new bool?(allowiAdInfoReading);
    }

    public void setAllowAdServicesInfoReading(bool allowAdServicesInfoReading)
    {
      this.allowAdServicesInfoReading = new bool?(allowAdServicesInfoReading);
    }

    public void setAllowIdfaReading(bool allowIdfaReading)
    {
      this.allowIdfaReading = new bool?(allowIdfaReading);
    }

    public void setProcessName(string processName) => this.processName = processName;

    [Obsolete("This is an obsolete method.")]
    public void setReadMobileEquipmentIdentity(bool readMobileEquipmentIdentity)
    {
    }

    public void setPreinstallTrackingEnabled(bool preinstallTrackingEnabled)
    {
      this.preinstallTrackingEnabled = new bool?(preinstallTrackingEnabled);
    }

    public void setLogDelegate(Action<string> logDelegate) => this.logDelegate = logDelegate;
  }
}
