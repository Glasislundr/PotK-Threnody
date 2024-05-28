// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.GAuth.GAuth.Session
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Core;
using Gsc.Device;
using Gsc.DOM.Json;
using Gsc.Network;
using Gsc.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Gsc.Auth.GAuth.GAuth
{
  public class Session : ISession
  {
    public readonly string EnvName;
    private readonly IAccountManager accountManager;
    private static readonly string userAgentCache = MiniJSON.Json.Serialize((object) new Dictionary<string, object>()
    {
      {
        "device_model",
        (object) DeviceInfo.DeviceModel
      },
      {
        "device_vendor",
        (object) DeviceInfo.DeviceVendor
      },
      {
        "os_info",
        (object) DeviceInfo.OperatingSystem
      },
      {
        "cpu_info",
        (object) DeviceInfo.ProcessorType
      },
      {
        "memory_size",
        (object) (((double) (DeviceInfo.SystemMemorySize >> 10) / 1000.0).ToString() + "GB")
      }
    });

    public string AccessToken { get; protected set; }

    public virtual string DeviceID => this.accountManager.GetDeviceId(this.EnvName);

    public virtual string SecretKey => this.accountManager.GetSecretKey(this.EnvName);

    public virtual string UserAgent => Session.userAgentCache;

    private event Action<string> deviceIdCallback;

    public Session(string envName, IAccountManager accountManager)
    {
      this.EnvName = envName;
      this.accountManager = accountManager;
      ((Component) ImmortalObject.Instance).gameObject.AddComponent<Session.AccessTokenChecker>();
    }

    public void DeleteAuthKeys() => this.accountManager.Remove(this.EnvName);

    public void AddDeviceIdCallback(Action<string> callback)
    {
      if (this.DeviceID != null)
      {
        callback(this.DeviceID);
      }
      else
      {
        this.deviceIdCallback -= callback;
        this.deviceIdCallback += callback;
      }
    }

    public virtual bool CanRefreshToken(Type requestType)
    {
      return !requestType.Equals(typeof (Gsc.Auth.GAuth.GAuth.API.Request.AccessToken));
    }

    public virtual IRefreshTokenTask GetRefreshTokenTask()
    {
      return (IRefreshTokenTask) new Session.RefreshTokenTask(this);
    }

    public virtual IWebTask RegisterEmailAddressAndPassword(
      string email,
      string password,
      bool disableValicationEmail,
      Action<RegisterEmailAddressAndPasswordResult> callback)
    {
      return (IWebTask) new Gsc.Auth.GAuth.GAuth.API.Request.RegisterEmailAddressAndPassword(this.DeviceID, this.SecretKey, email, password)
      {
        DisableValidationEmail = disableValicationEmail
      }.Send().OnResponse((VoidCallbackWithError<Gsc.Auth.GAuth.GAuth.API.Response.RegisterEmailAddressAndPassword>) ((response, error) => callback(Session.GetRegisterEmailAddressWithPasswordResult(response, (Gsc.Auth.GAuth.GAuth.API.Response.ErrorResponse) error))));
    }

    private static RegisterEmailAddressAndPasswordResult GetRegisterEmailAddressWithPasswordResult(
      Gsc.Auth.GAuth.GAuth.API.Response.RegisterEmailAddressAndPassword response,
      Gsc.Auth.GAuth.GAuth.API.Response.ErrorResponse error)
    {
      if (error == null)
        return new RegisterEmailAddressAndPasswordResult(RegisterEmailAddressAndPasswordResultCode.Success);
      RegisterEmailAddressAndPasswordResultCode resultCode = RegisterEmailAddressAndPasswordResultCode.UnknownError;
      switch (error.ErrorCode)
      {
        case "000":
          resultCode = RegisterEmailAddressAndPasswordResultCode.InvalidEmailAddress;
          break;
        case "001":
          resultCode = RegisterEmailAddressAndPasswordResultCode.InvalidPassword;
          break;
        case "002":
          resultCode = RegisterEmailAddressAndPasswordResultCode.DuplicatedEmailAddress;
          break;
        default:
          Value root = error.data.Root;
          if (root.GetValueByPointer("/reason/email", (string) null) != null)
          {
            resultCode = RegisterEmailAddressAndPasswordResultCode.InvalidEmailAddress;
            break;
          }
          root = error.data.Root;
          if (root.GetValueByPointer("/reason/password", (string) null) != null)
          {
            resultCode = RegisterEmailAddressAndPasswordResultCode.InvalidPassword;
            break;
          }
          break;
      }
      return new RegisterEmailAddressAndPasswordResult(resultCode);
    }

    public virtual IWebTask AddDeviceWithEmailAddressAndPassword(
      string email,
      string password,
      Action<AddDeviceWithEmailAddressAndPasswordResult> callback)
    {
      return (IWebTask) new Gsc.Auth.GAuth.GAuth.API.Request.AddDeviceWithEmailAddressAndPassword(email, password)
      {
        Idfv = Gsc.Auth.GAuth.GAuth.Device.Instance.ID
      }.Send().OnResponse((VoidCallbackWithError<Gsc.Auth.GAuth.GAuth.API.Response.AddDeviceWithEmailAddressAndPassword>) ((response, error) =>
      {
        AddDeviceWithEmailAddressAndPasswordResult addressAndPassword = Session.GetAddDeviceWithEmailAddressAndPassword((Gsc.Auth.GAuth.GAuth.API.Response.ErrorResponse) error);
        if (addressAndPassword == AddDeviceWithEmailAddressAndPasswordResultCode.Success)
          this.accountManager.SetKeyPair(this.EnvName, response.SecretKey, response.DeviceId);
        callback(addressAndPassword);
      }));
    }

    private static AddDeviceWithEmailAddressAndPasswordResult GetAddDeviceWithEmailAddressAndPassword(
      Gsc.Auth.GAuth.GAuth.API.Response.ErrorResponse error)
    {
      if (error == null)
        return new AddDeviceWithEmailAddressAndPasswordResult(AddDeviceWithEmailAddressAndPasswordResultCode.Success);
      AddDeviceWithEmailAddressAndPasswordResultCode resultCode1 = AddDeviceWithEmailAddressAndPasswordResultCode.UnknownError;
      Value root;
      switch (error.ErrorCode)
      {
        case "000":
          resultCode1 = AddDeviceWithEmailAddressAndPasswordResultCode.MissingDeviceId;
          break;
        case "001":
          resultCode1 = AddDeviceWithEmailAddressAndPasswordResultCode.MissingEmailOrPassword;
          break;
        case "002":
          resultCode1 = AddDeviceWithEmailAddressAndPasswordResultCode.Locked;
          break;
        default:
          root = error.data.Root;
          if (root.GetValueByPointer("/reason/email", (string) null) != null)
          {
            resultCode1 = AddDeviceWithEmailAddressAndPasswordResultCode.MissingEmailOrPassword;
            break;
          }
          root = error.data.Root;
          if (root.GetValueByPointer("/reason/password", (string) null) != null)
          {
            resultCode1 = AddDeviceWithEmailAddressAndPasswordResultCode.MissingEmailOrPassword;
            break;
          }
          break;
      }
      if (resultCode1 != AddDeviceWithEmailAddressAndPasswordResultCode.Locked)
        return new AddDeviceWithEmailAddressAndPasswordResult(resultCode1);
      int resultCode2 = (int) resultCode1;
      root = error.data.Root;
      root = root["expires_in"];
      int lockedExpiresIn = root.ToInt();
      root = error.data.Root;
      root = root["trial_counter"];
      int trialCounter = root.ToInt();
      return new AddDeviceWithEmailAddressAndPasswordResult((AddDeviceWithEmailAddressAndPasswordResultCode) resultCode2, lockedExpiresIn, trialCounter);
    }

    public class AccessTokenChecker : MonoBehaviour
    {
      private const float FAILED_POLLING_INTERVAL = 30f;
      private const WebTaskAttribute TASK_ATTRIBUTES = WebTaskAttribute.Reliable | WebTaskAttribute.Silent | WebTaskAttribute.Parallel;
      private const WebTaskResult ACCEPT_RESULTS = WebTaskResult.kLocalResult | WebTaskResult.kGrobalResult | WebTaskResult.kCreticalError | WebTaskResult.Maintenance | WebTaskResult.UpdateApplication;
      private bool isRunning;
      private int cachedInstanceId;

      private void Awake() => this.cachedInstanceId = ((UnityEngine.Object) this).GetInstanceID();

      private void OnApplicationFocus(bool focusState)
      {
        if (!(!this.isRunning & (focusState && this.cachedInstanceId == ((UnityEngine.Object) this).GetInstanceID())) || WebQueue.defaultQueue.isPause)
          return;
        this.StartCoroutine(this.CheckToken());
      }

      private IEnumerator CheckToken()
      {
        this.isRunning = true;
        while (true)
        {
          while (WebQueue.defaultQueue.isPause)
            yield return (object) new WaitForSeconds(30f);
          WebTask<Gsc.Auth.GAuth.GAuth.API.Request.AccessTokenVerify, Gsc.Auth.GAuth.GAuth.API.Response.AccessTokenVerify> task = new Gsc.Auth.GAuth.GAuth.API.Request.AccessTokenVerify().GetTask(WebTaskAttribute.Reliable | WebTaskAttribute.Silent | WebTaskAttribute.Parallel).SetAcceptResults(WebTaskResult.kLocalResult | WebTaskResult.kGrobalResult | WebTaskResult.kCreticalError | WebTaskResult.Maintenance | WebTaskResult.UpdateApplication);
          yield return (object) task;
          if ((task.Result & (WebTaskResult.Success | WebTaskResult.ExpiredSessionError | WebTaskResult.InvalidDeviceError | WebTaskResult.InternalExpiredTokenError)) == WebTaskResult.None)
          {
            yield return (object) new WaitForSeconds(30f);
            task = (WebTask<Gsc.Auth.GAuth.GAuth.API.Request.AccessTokenVerify, Gsc.Auth.GAuth.GAuth.API.Response.AccessTokenVerify>) null;
          }
          else
            break;
        }
        this.isRunning = false;
      }
    }

    public class RefreshTokenTask : IRefreshTokenTask, ITask
    {
      private readonly Session session;

      public WebTaskResult Result { get; protected set; }

      public bool isDone { get; protected set; }

      public RefreshTokenTask(Session session) => this.session = session;

      public void OnStart()
      {
      }

      public void OnFinish()
      {
      }

      public IEnumerator Run()
      {
        if (this.session.DeviceID == null)
        {
          WebInternalTask<Gsc.Auth.GAuth.GAuth.API.Request.Register, Gsc.Auth.GAuth.GAuth.API.Response.Register> task = WebInternalTask.Create<Gsc.Auth.GAuth.GAuth.API.Request.Register, Gsc.Auth.GAuth.GAuth.API.Response.Register>((Gsc.Network.Request<Gsc.Auth.GAuth.GAuth.API.Request.Register, Gsc.Auth.GAuth.GAuth.API.Response.Register>) new Gsc.Auth.GAuth.GAuth.API.Request.Register(this.session.SecretKey)
          {
            Idfa = Gsc.Auth.GAuth.GAuth.Device.Instance.IDFA,
            Idfv = Gsc.Auth.GAuth.GAuth.Device.Instance.ID
          });
          task.OnStart();
          yield return (object) task.Run();
          task.OnFinish();
          if (task.Result == WebTaskResult.Success)
          {
            this.session.accountManager.SetDeviceId(this.session.EnvName, task.Response.DeviceId);
            if (this.session.deviceIdCallback != null)
            {
              this.session.deviceIdCallback(this.session.DeviceID);
              this.session.deviceIdCallback = (Action<string>) null;
            }
            task = (WebInternalTask<Gsc.Auth.GAuth.GAuth.API.Request.Register, Gsc.Auth.GAuth.GAuth.API.Response.Register>) null;
          }
          else
          {
            this.Result = task.Result;
            yield break;
          }
        }
        WebInternalTask<Gsc.Auth.GAuth.GAuth.API.Request.AccessToken, Gsc.Auth.GAuth.GAuth.API.Response.AccessToken> task1 = WebInternalTask.Create<Gsc.Auth.GAuth.GAuth.API.Request.AccessToken, Gsc.Auth.GAuth.GAuth.API.Response.AccessToken>((Gsc.Network.Request<Gsc.Auth.GAuth.GAuth.API.Request.AccessToken, Gsc.Auth.GAuth.GAuth.API.Response.AccessToken>) new Gsc.Auth.GAuth.GAuth.API.Request.AccessToken(this.session.SecretKey, this.session.DeviceID)
        {
          Idfa = Gsc.Auth.GAuth.GAuth.Device.Instance.IDFA,
          Idfv = Gsc.Auth.GAuth.GAuth.Device.Instance.ID
        });
        task1.OnStart();
        yield return (object) task1.Run();
        task1.OnFinish();
        this.Result = task1.Result;
        if (task1.Result == WebTaskResult.Success)
        {
          this.session.AccessToken = task1.Response.Token;
          this.isDone = true;
          task1 = (WebInternalTask<Gsc.Auth.GAuth.GAuth.API.Request.AccessToken, Gsc.Auth.GAuth.GAuth.API.Response.AccessToken>) null;
        }
      }
    }
  }
}
