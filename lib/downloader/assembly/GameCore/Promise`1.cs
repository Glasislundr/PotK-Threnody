// Decompiled with JetBrains decompiler
// Type: GameCore.Promise`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace GameCore
{
  public class Promise<T>
  {
    private bool hasResult;
    private T result;
    private Exception exception;

    public bool HasResult => this.hasResult;

    public T Result
    {
      get
      {
        if (!this.hasResult)
          throw new Exception("result or exception is not assigned.");
        return this.result;
      }
      set
      {
        this.hasResult = true;
        this.result = value;
      }
    }

    public Exception Exception
    {
      get
      {
        if (!this.hasResult)
          throw new Exception("result or exception is not assigned.");
        return this.exception;
      }
      set
      {
        this.hasResult = !this.hasResult ? true : throw new Exception("result or exception is already assigned.");
        this.exception = value;
      }
    }

    public T ResultOrException
    {
      get
      {
        if (!this.hasResult)
          throw new Exception("result is not assigned.");
        if (this.exception != null)
          throw this.exception;
        return this.result;
      }
    }
  }
}
