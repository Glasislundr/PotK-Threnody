// Decompiled with JetBrains decompiler
// Type: GameCore.GameGlobalVariable`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace GameCore
{
  public class GameGlobalVariable<T>
  {
    private T value;

    public T Get() => this.value;

    public void Reset(T v) => this.value = v;

    public static GameGlobalVariable<T> New(params object[] args)
    {
      return new GameGlobalVariable<T>()
      {
        value = (T) Activator.CreateInstance(typeof (T), args)
      };
    }

    public static GameGlobalVariable<T> Null()
    {
      return new GameGlobalVariable<T>()
      {
        value = default (T)
      };
    }
  }
}
