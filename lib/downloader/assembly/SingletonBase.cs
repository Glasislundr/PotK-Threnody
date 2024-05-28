// Decompiled with JetBrains decompiler
// Type: SingletonBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class SingletonBase : MonoBehaviour
{
  protected abstract void Initialize();

  protected virtual void Finlaize()
  {
  }

  protected abstract void clearInstance();

  public void forceDestroy()
  {
    this.clearInstance();
    this.Finlaize();
  }
}
