// Decompiled with JetBrains decompiler
// Type: LocaImageCache
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Globalization;
using System.IO;
using UnityEngine;

#nullable disable
public class LocaImageCache
{
  private static string dir;
  private string filePath;
  private string url;
  private string _lastWriteTime;
  private Texture2D _texture = new Texture2D(0, 0, (TextureFormat) 5, false);
  private static bool enableSetLastWriteTime = true;

  public string lastWriteTime => this._lastWriteTime;

  public Texture2D texture => this._texture;

  static LocaImageCache()
  {
    LocaImageCache.dir = Path.Combine(Application.temporaryCachePath, "image");
    LocaImageCache.Cleaning();
  }

  public static void Clear()
  {
    try
    {
      if (!Directory.Exists(LocaImageCache.dir))
        return;
      Directory.Delete(LocaImageCache.dir, true);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
    }
  }

  private static void Cleaning()
  {
    try
    {
      if (!Directory.Exists(LocaImageCache.dir))
        return;
      DateTime dateTime = DateTime.Today.AddDays(-3.0);
      foreach (string file in Directory.GetFiles(LocaImageCache.dir, "*", SearchOption.AllDirectories))
      {
        if (dateTime > File.GetLastAccessTime(file))
          File.Delete(file);
      }
    }
    catch (Exception ex)
    {
    }
  }

  public LocaImageCache(string url)
  {
    this.url = url;
    this.filePath = LocaImageCache.dir + new Uri(this.url).AbsolutePath;
  }

  public bool Read()
  {
    bool flag = false;
    try
    {
      if (File.Exists(this.filePath))
      {
        if (ImageConversion.LoadImage(this._texture, File.ReadAllBytes(this.filePath)))
        {
          this._lastWriteTime = File.GetLastWriteTime(this.filePath).ToString("r");
          flag = true;
        }
      }
    }
    catch (Exception ex)
    {
    }
    return flag;
  }

  public bool Write(byte[] bytes, string lastWriteTime)
  {
    bool flag = false;
    try
    {
      string directoryName = Path.GetDirectoryName(this.filePath);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      File.WriteAllBytes(this.filePath, bytes);
      if (LocaImageCache.enableSetLastWriteTime)
        File.SetLastWriteTime(this.filePath, DateTime.ParseExact(lastWriteTime, new string[2]
        {
          "ddd, d MMM yyyy HH':'mm':'ss zzz",
          "r"
        }, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None));
      ImageConversion.LoadImage(this._texture, bytes);
      this._lastWriteTime = lastWriteTime;
      flag = true;
    }
    catch (Exception ex)
    {
      LocaImageCache.enableSetLastWriteTime = false;
    }
    return flag;
  }
}
