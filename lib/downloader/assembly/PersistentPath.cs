// Decompiled with JetBrains decompiler
// Type: PersistentPath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public static class PersistentPath
{
  public static string Fallback => PersistentPath.Value;

  public static string Value => StorageUtil.persistentDataPath;
}
