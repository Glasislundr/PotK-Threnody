// Decompiled with JetBrains decompiler
// Type: StoreUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;

#nullable disable
public static class StoreUtil
{
  public static void OpenMyStore()
  {
    string str = "http://dg-pk.fg-games.co.jp/";
    if (str.Length > 0)
      App.OpenStore(str);
    else
      Debug.Log((object) "appHomeUrl is not defined.");
  }
}
