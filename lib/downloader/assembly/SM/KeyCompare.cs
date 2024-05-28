// Decompiled with JetBrains decompiler
// Type: SM.KeyCompare
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace SM
{
  [Serializable]
  public class KeyCompare
  {
    protected bool _hasKey;
    protected object _key;

    public bool hasKey => this._hasKey;

    public object Key => this._key;
  }
}
