// Decompiled with JetBrains decompiler
// Type: Singleton`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class Singleton<T_TYPE> : SingletonBase where T_TYPE : Singleton<T_TYPE>
{
  private static T_TYPE sInstance;

  public static T_TYPE GetInstance()
  {
    if (Object.op_Equality((Object) (object) Singleton<T_TYPE>.sInstance, (Object) null))
      Singleton<T_TYPE>.sInstance = Object.FindObjectOfType(typeof (T_TYPE)) as T_TYPE;
    return Singleton<T_TYPE>.sInstance;
  }

  public static T_TYPE GetInstanceOrNull()
  {
    if (Object.op_Equality((Object) (object) Singleton<T_TYPE>.sInstance, (Object) null))
      Singleton<T_TYPE>.sInstance = Object.FindObjectOfType(typeof (T_TYPE)) as T_TYPE;
    return Singleton<T_TYPE>.sInstance;
  }

  private void Awake()
  {
    T_TYPE type = Singleton<T_TYPE>.GetInstance();
    if (Object.op_Equality((Object) (object) type, (Object) null))
      type = this as T_TYPE;
    if (Object.op_Inequality((Object) (object) type, (Object) (object) (this as T_TYPE)))
    {
      Object.Destroy((Object) ((Component) this).gameObject);
    }
    else
    {
      Singleton<T_TYPE>.sInstance = type;
      this.Initialize();
      Object.DontDestroyOnLoad((Object) ((Component) this).gameObject);
    }
  }

  protected override void clearInstance() => Singleton<T_TYPE>.sInstance = default (T_TYPE);
}
