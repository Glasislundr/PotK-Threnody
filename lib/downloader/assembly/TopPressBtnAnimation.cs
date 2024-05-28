// Decompiled with JetBrains decompiler
// Type: TopPressBtnAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TopPressBtnAnimation : MonoBehaviour
{
  [SerializeField]
  private Animator anim;
  private TopScene topScene;

  public void Init(TopScene scene)
  {
    this.topScene = scene;
    this.anim.Play("TOP_PressStart");
  }

  public void StartAnimFinish()
  {
    if (!Object.op_Inequality((Object) this.topScene, (Object) null))
      return;
    this.topScene.EnablePressStartBtn = true;
  }
}
