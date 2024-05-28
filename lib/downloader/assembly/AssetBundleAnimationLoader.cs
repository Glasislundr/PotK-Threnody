// Decompiled with JetBrains decompiler
// Type: AssetBundleAnimationLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class AssetBundleAnimationLoader : MonoBehaviour
{
  [Tooltip("Resouces以下のパスを拡張子なしで入力")]
  public string Path = string.Empty;
  public bool AutoLoad = true;

  public bool isDone { get; private set; }

  private IEnumerator Start()
  {
    if (this.AutoLoad)
      yield return (object) this.Load();
  }

  public IEnumerator Load()
  {
    this.isDone = false;
    IEnumerator e = this.LoadAnimationController(this.Path);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.isDone = true;
  }

  private IEnumerator LoadAnimationController(string path)
  {
    AssetBundleAnimationLoader bundleAnimationLoader = this;
    Future<RuntimeAnimatorController> loader = Singleton<ResourceManager>.GetInstance().Load<RuntimeAnimatorController>(path);
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    RuntimeAnimatorController result = loader.Result;
    Animator component = ((Component) bundleAnimationLoader).GetComponent<Animator>();
    if (Object.op_Inequality((Object) component, (Object) null))
      component.runtimeAnimatorController = result;
  }
}
