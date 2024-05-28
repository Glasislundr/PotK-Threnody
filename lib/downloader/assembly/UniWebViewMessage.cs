// Decompiled with JetBrains decompiler
// Type: UniWebViewMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public struct UniWebViewMessage
{
  public string rawMessage { get; private set; }

  public string scheme { get; private set; }

  public string path { get; private set; }

  public Dictionary<string, string> args { get; private set; }

  public UniWebViewMessage(string rawMessage)
    : this()
  {
    this.rawMessage = rawMessage;
    string[] strArray1 = rawMessage.Split(new string[1]
    {
      "://"
    }, StringSplitOptions.None);
    if (strArray1.Length >= 2)
    {
      this.scheme = strArray1[0];
      string str1 = "";
      for (int index = 1; index < strArray1.Length; ++index)
        str1 += strArray1[index];
      string[] strArray2 = str1.Split("?"[0]);
      this.path = strArray2[0].TrimEnd('/');
      this.args = new Dictionary<string, string>();
      if (strArray2.Length <= 1)
        return;
      string str2 = strArray2[1];
      char[] chArray1 = new char[1]{ "&"[0] };
      foreach (string str3 in str2.Split(chArray1))
      {
        char[] chArray2 = new char[1]{ "="[0] };
        string[] strArray3 = str3.Split(chArray2);
        if (strArray3.Length > 1)
          this.args[strArray3[0]] = WWW.UnEscapeURL(strArray3[1]);
      }
    }
    else
      Debug.LogError((object) ("Bad url scheme. Can not be parsed to UniWebViewMessage: " + rawMessage));
  }
}
