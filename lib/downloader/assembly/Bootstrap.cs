// Decompiled with JetBrains decompiler
// Type: Bootstrap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc;
using Gsc.App;
using Gsc.App.NetworkHelper;
using Gsc.Core;
using Gsc.Device;
using Gsc.Network;
using Gsc.Purchase;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Bootstrap : MonoBehaviour
{
  private static string[] productIDs;

  [RuntimeInitializeOnLoadMethod]
  private static void OnBoot() => SDK.BootLoader.Run(Bootstrap._InitializeApplication());

  public static void RebootGSCC() => SDK.Reset();

  private static IEnumerator _InitializeApplication()
  {
    bool review_app_connect = false;
    bool review_dlc_connect = false;
    IEnumerator e = WebAPI.RpcReviewEnvConnect((Action<bool[]>) (ret =>
    {
      review_app_connect = ret[0];
      review_dlc_connect = ret[1];
    }));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnityErrorLogSender.Instance.Callback = new Action<CustomHeaders, Dictionary<string, object>, Dictionary<string, object>, Dictionary<string, object>>(GsccBridge.UnityErrorLogCallback);
    LocalLogUtil.Init();
    Application.logMessageReceived -= new Application.LogCallback(LocalLogUtil.LogFatalError);
    Application.logMessageReceived += new Application.LogCallback(LocalLogUtil.LogFatalError);
    Gsc.App.AccountManager accountManager = new Gsc.App.AccountManager();
    yield return (object) SDK.Initialize<Gsc.App.Environment>(Gsc.App.Environment.SetProduction(new Configuration.Builder<Gsc.App.Environment>().SetApplicationName("punk").SetApplicationVersion(DeviceKit.App.GetBundleVersion()).SetAccountManager((IAccountManager) accountManager).SetWebQueueObserver((IWebQueueObserver) new WebQueueListener()), review_app_connect, review_dlc_connect), "production");
    if (SDK.hasError)
      ModalWindow.Show("エラー", "アプリを直接起動することはできません。", (Action) (() => Application.Quit()));
    else
      yield return (object) Bootstrap.InitPurchase();
  }

  private static IEnumerator InitPurchase()
  {
    while (!ResourceDownloader.Completed)
      yield return (object) new WaitForSeconds(5f);
    while (MasterData.CoinProductList == null)
      yield return (object) new WaitForSeconds(5f);
    while (SMManager.Get<Player>() == null)
      yield return (object) null;
    Bootstrap.productIDs = ((IEnumerable<CoinProduct>) MasterData.CoinProductList).Where<CoinProduct>((Func<CoinProduct, bool>) (x => x.platform == Gsc.Auth.Device.Platform && x.type != 0)).Select<CoinProduct, string>((Func<CoinProduct, string>) (x => x.product_id)).ToArray<string>();
    if (Bootstrap.productIDs.Length == 0)
    {
      string platformName = "";
      platformName = "windows";
      Bootstrap.productIDs = ((IEnumerable<CoinProduct>) MasterData.CoinProductList).Where<CoinProduct>((Func<CoinProduct, bool>) (x => x.platform == platformName && x.type != 0)).Select<CoinProduct, string>((Func<CoinProduct, string>) (x => x.product_id)).ToArray<string>();
    }
    NativeRootObject.CreateInstance();
    PurchaseFlow.Init(Bootstrap.productIDs, (IPurchaseGlobalListener) new PurchaseListener());
  }

  private static void RunOnUiThread()
  {
    PurchaseFlow.Init(Bootstrap.productIDs, (IPurchaseGlobalListener) new PurchaseListener());
  }
}
