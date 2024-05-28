// Decompiled with JetBrains decompiler
// Type: CachedFile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
public static class CachedFile
{
  private static HashSet<string> hash = new HashSet<string>();

  public static bool Exists(string path)
  {
    if (CachedFile.hash.Contains(path))
      return true;
    if (!File.Exists(path))
      return false;
    CachedFile.hash.Add(path);
    return true;
  }

  public static void Add(string path) => CachedFile.hash.Add(path);

  public static void Clear() => CachedFile.hash.Clear();
}
