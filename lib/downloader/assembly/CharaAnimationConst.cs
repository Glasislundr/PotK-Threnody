// Decompiled with JetBrains decompiler
// Type: CharaAnimationConst
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class CharaAnimationConst : MonoBehaviour
{
  public Animator anim;
  public AnimatorCtrl animC;
  public UI2DSprite[] charaSprite;
  public UILabel[] charaName;
  public UILabel _english_name;
  private Action _act_finish;

  public void onAnimationFinish()
  {
    if (this._act_finish == null)
      return;
    this._act_finish();
  }

  public void setFinishAction(Action act_finish) => this._act_finish = act_finish;
}
