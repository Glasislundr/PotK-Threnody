// Decompiled with JetBrains decompiler
// Type: URLScheme
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class URLScheme : MonoBehaviour
{
  private static URLScheme instance = (URLScheme) null;
  private static bool isInitialized = false;
  public static int fggBtnStatus = -1;

  public static URLScheme Instance => URLScheme.instance;

  private void Awake()
  {
    if (URLScheme.isInitialized)
      Object.Destroy((Object) this);
    else
      URLScheme.instance = this;
    URLScheme.isInitialized = true;
  }

  private void OnDestroy()
  {
    if (!Object.op_Equality((Object) URLScheme.instance, (Object) this))
      return;
    URLScheme.isInitialized = false;
  }

  public void RestartFromActivity(string message)
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    string str1 = message;
    char[] chArray = new char[1]{ '&' };
    foreach (string str2 in str1.Split(chArray))
    {
      string[] strArray = str2.Split('=');
      if (str2.Length >= 2)
        dictionary[strArray[0]] = strArray[1];
    }
    string str3 = "";
    int num;
    try
    {
      num = int.Parse(dictionary["id"]);
    }
    catch (FormatException ex)
    {
      num = -1;
    }
    try
    {
      dictionary.TryGetValue("s", out str3);
    }
    catch (FormatException ex)
    {
      str3 = "";
    }
    NGGameDataManager.UrlSchemePresentId = num;
    NGGameDataManager.UrlSchemeSerial = str3 ?? "";
    Scene activeScene = SceneManager.GetActiveScene();
    if (((Scene) ref activeScene).name.Equals("start"))
      return;
    if (Object.op_Inequality((Object) Singleton<PopupManager>.GetInstance(), (Object) null))
      Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    ModalWindow objectOfType = Object.FindObjectOfType<ModalWindow>();
    if (Object.op_Inequality((Object) objectOfType, (Object) null))
      objectOfType.Hide(false);
    StartScript.Restart();
  }

  public IEnumerator RequestPresentGet()
  {
    DateTime time = DateTime.Now;
    while (true)
    {
      switch (URLScheme.fggBtnStatus)
      {
        case -1:
          if ((DateTime.Now - time).Seconds < 10)
          {
            yield return (object) null;
            continue;
          }
          goto label_14;
        case 2:
        case 3:
          goto label_5;
        default:
          goto label_2;
      }
    }
label_14:
    yield break;
label_2:
    yield break;
label_5:
    Future<WebAPI.Response.PresentUrlPresent> futureF = WebAPI.PresentUrlPresent(NGGameDataManager.UrlSchemePresentId, NGGameDataManager.UrlSchemeSerial);
    IEnumerator e = futureF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    WebAPI.Response.PresentUrlPresent result = futureF.Result;
    if (result != null)
    {
      if (!result.is_enabled || NGGameDataManager.UrlSchemePresentId == -1)
        ModalWindow.Show("", "報酬付与期間外です", (Action) (() => { }));
      else if (result.is_received)
        ModalWindow.Show("", "このURLの報酬は既に受け取り済みです", (Action) (() => { }));
      else
        ModalWindow.Show("", result.popup_message, (Action) (() => { }));
    }
    NGGameDataManager.UrlSchemePresentId = -1;
  }

  public void TestLog(string message) => Debug.Log((object) ("TestLog:" + message));
}
