﻿// Decompiled with JetBrains decompiler
// Type: Gsc.Configuration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Device;
using Gsc.Network;
using System.Collections.Generic;

#nullable disable
namespace Gsc
{
  public struct Configuration
  {
    public readonly string AppName;
    public readonly string EnvName;
    public readonly Configuration.IEnvironment Env;
    public readonly IAccountManager accountManager;
    public readonly IWebQueueObserver webQueueObserver;

    public Configuration(
      Configuration.IBuilder builder,
      string envName,
      Configuration.IEnvironment env)
    {
      this.AppName = builder.name;
      this.EnvName = envName;
      this.Env = env;
      this.accountManager = builder.accountManager;
      this.webQueueObserver = builder.webQueueObserver;
    }

    public T GetEnv<T>() where T : struct, Configuration.IEnvironment => (T) this.Env;

    public interface IEnvironment
    {
      string ServerUrl { get; }

      string NativeBaseUrl { get; }

      string LogCollectionUrl { get; }

      string ClientErrorApi { get; }

      string AuthApiPrefix { get; }

      string PurchaseApiPrefix { get; }

      void SetValue(string key, string value);
    }

    public interface IBuilder
    {
      string name { get; }

      IAccountManager accountManager { get; }

      IWebQueueObserver webQueueObserver { get; }
    }

    public class Builder<T> : Configuration.IBuilder where T : struct, Configuration.IEnvironment
    {
      public string envUrl;
      public Dictionary<string, Configuration.IEnvironment> envCollection;

      public string name { get; private set; }

      public string version { get; private set; }

      public IAccountManager accountManager { get; private set; }

      public IWebQueueObserver webQueueObserver { get; private set; }

      public Configuration.Builder<T> SetApplicationName(string name)
      {
        this.name = name;
        return this;
      }

      public Configuration.Builder<T> SetApplicationVersion(string version)
      {
        this.version = version;
        return this;
      }

      public Configuration.Builder<T> SetEnvironment(string url)
      {
        this.envUrl = url;
        return this;
      }

      public Configuration.Builder<T> AddEnvironment(string label, T env)
      {
        if (this.envCollection == null)
          this.envCollection = new Dictionary<string, Configuration.IEnvironment>();
        this.envCollection.Add(label, (Configuration.IEnvironment) env);
        return this;
      }

      public Configuration.Builder<T> SetWebQueueObserver(IWebQueueObserver observer)
      {
        this.webQueueObserver = observer;
        return this;
      }

      public Configuration.Builder<T> SetAccountManager(IAccountManager accountManager)
      {
        this.accountManager = accountManager;
        return this;
      }
    }
  }
}
