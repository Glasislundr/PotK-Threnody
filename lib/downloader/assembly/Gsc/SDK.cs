// Decompiled with JetBrains decompiler
// Type: Gsc.SDK
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using Gsc.Auth;
using Gsc.Core;
using Gsc.Device;
using Gsc.Network;
using Gsc.Purchase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Gsc
{
  public static class SDK
  {
    public static bool Initialized { get; private set; }

    public static bool hasError { get; private set; }

    public static Configuration Configuration { get; private set; }

    public static Coroutine Initialize<T>(Configuration.Builder<T> confBuilder, string specifiedEnv = null) where T : struct, Configuration.IEnvironment
    {
      SDK.hasError = true;
      SDK._PreInit(confBuilder.name, confBuilder.webQueueObserver);
      Gsc.Auth.Device.Initialize();
      LogKit.Logger.SetPlatform(Gsc.Auth.Device.Platform);
      Configuration.IEnvironment environment;
      if (specifiedEnv != null && confBuilder.envCollection.TryGetValue(specifiedEnv, out environment))
      {
        confBuilder.envCollection.Clear();
        confBuilder.envCollection.Add(specifiedEnv, environment);
      }
      return ImmortalObject.Instance.StartCoroutine(SDK.Initializer<T>.Initialize(confBuilder));
    }

    public static void Reset()
    {
      SDK.Initialized = false;
      SDK._PreInit(SDK.Configuration.AppName, SDK.Configuration.webQueueObserver);
      SDK._PostInit(SDK.Configuration);
    }

    private static void _PreInit(string appName, IWebQueueObserver webQueueObserver)
    {
      Gsc.Core.Logger.Init();
      UnityErrorLogSender.Initialize();
      ImmortalObject.Initialize();
      WebQueue.Init(webQueueObserver);
      LogKit.Logger.Init(appName, Path.applicationDataPath + "/_logs", ((Component) ImmortalObject.Instance).gameObject);
    }

    private static void _PostInit(Configuration conf)
    {
      SDK.Configuration = conf;
      Session.Init(conf.EnvName, AccountManager.Create(conf.accountManager));
      LogKit.Logger.SetServerUrl(conf.Env.LogCollectionUrl);
      Session.DefaultSession.AddDeviceIdCallback((Action<string>) (n_device_id => LogKit.Logger.SetDeviceID(n_device_id)));
      App.Hardkey.Init(((Component) ImmortalObject.Instance).gameObject);
      PurchaseHandler.Initialize();
      SDK.Initialized = true;
    }

    private static class Initializer<T> where T : struct, Configuration.IEnvironment
    {
      public static IEnumerator Initialize(Configuration.Builder<T> confBuilder)
      {
        while (!Gsc.Auth.Device.initialized)
          yield return (object) null;
        if (Gsc.Auth.Device.hasError)
        {
          SDK.Initialized = true;
        }
        else
        {
          if (confBuilder.envUrl != null)
          {
            WebTask<EnvLoader<T>, EnvLoader<T>.Response> loadTask = new EnvLoader<T>(confBuilder.envUrl, confBuilder.version).SerialSend().OnResponse((VoidCallback<EnvLoader<T>.Response>) (r => confBuilder.envCollection = new Dictionary<string, Configuration.IEnvironment>()
            {
              {
                r.EnvName,
                r.Env
              }
            }));
            do
            {
              yield return (object) loadTask;
            }
            while (loadTask.Result != WebTaskResult.Success);
            loadTask = (WebTask<EnvLoader<T>, EnvLoader<T>.Response>) null;
          }
          yield return (object) SDK.Initializer<T>.BuildConfigration(confBuilder);
        }
      }

      private static IEnumerator BuildConfigration(Configuration.Builder<T> confBuilder)
      {
        KeyValuePair<string, Configuration.IEnvironment> keyValuePair = confBuilder.envCollection.First<KeyValuePair<string, Configuration.IEnvironment>>();
        SDK.Initializer<T>.SelectedEnvironment(confBuilder, keyValuePair.Key, keyValuePair.Value);
        yield break;
      }

      private static void SelectedEnvironment(
        Configuration.Builder<T> confBuilder,
        string envName,
        Configuration.IEnvironment env)
      {
        SDK.hasError = false;
        Application.runInBackground = true;
        SDK._PostInit(new Configuration((Configuration.IBuilder) confBuilder, envName, env));
      }
    }

    public class BootLoader : MonoBehaviour
    {
      private static SDK.BootLoader currentLoader;
      private IEnumerator enumerator;

      public static void Run(IEnumerator enumerator)
      {
        if (Object.op_Inequality((Object) SDK.BootLoader.currentLoader, (Object) null))
          Object.Destroy((Object) ((Component) SDK.BootLoader.currentLoader).gameObject);
        GameObject gameObject = new GameObject("Gsc.SDK.Bootloader");
        ((Object) gameObject).hideFlags = (HideFlags) 52;
        Object.DontDestroyOnLoad((Object) gameObject);
        SDK.BootLoader.currentLoader = gameObject.AddComponent<SDK.BootLoader>();
        SDK.BootLoader.currentLoader.enumerator = enumerator;
      }

      private IEnumerator Start()
      {
        // ISSUE: reference to a compiler-generated field
        int num = this.\u003C\u003E1__state;
        SDK.BootLoader bootLoader = this;
        if (num != 0)
        {
          if (num != 1)
            return false;
          // ISSUE: reference to a compiler-generated field
          this.\u003C\u003E1__state = -1;
          Object.Destroy((Object) ((Component) bootLoader).gameObject);
          SDK.BootLoader.currentLoader = (SDK.BootLoader) null;
          return false;
        }
        // ISSUE: reference to a compiler-generated field
        this.\u003C\u003E1__state = -1;
        // ISSUE: reference to a compiler-generated field
        this.\u003C\u003E2__current = (object) bootLoader.enumerator;
        // ISSUE: reference to a compiler-generated field
        this.\u003C\u003E1__state = 1;
        return true;
      }
    }
  }
}
