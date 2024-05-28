// Decompiled with JetBrains decompiler
// Type: GameCore.Future`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;

#nullable disable
namespace GameCore
{
  public class Future<T>
  {
    private Promise<T> promise;
    private IEnumerator enumerator;
    private Action<T> callback;

    public Future(Func<Promise<T>, IEnumerator> func)
    {
      this.promise = new Promise<T>();
      this.enumerator = this.WaitResult(func);
    }

    private IEnumerator ThenFunc<U>(Future<T> future, Func<T, U> f, Promise<U> promise)
    {
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      promise.Result = f(future.Result);
    }

    public Future<U> Then<U>(Func<T, U> f)
    {
      return new Future<U>((Func<Promise<U>, IEnumerator>) (promise => this.ThenFunc<U>(this, f, promise)));
    }

    public void SetCallback(Action<T> callback) => this.callback = callback;

    public IEnumerator Wait() => this.enumerator;

    private IEnumerator WaitResult(Func<Promise<T>, IEnumerator> func)
    {
      IEnumerator e = func(this.promise);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (!this.promise.HasResult)
        this.promise.Exception = new Exception("Result or Exception is not set.");
      if (this.callback != null)
        this.callback(this.Result);
    }

    public bool HasResult => this.promise.HasResult;

    public T Result => this.promise.Result;

    public Exception Exception => this.promise.Exception;

    public T GetResultOrException() => this.promise.ResultOrException;
  }
}
