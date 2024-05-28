// Decompiled with JetBrains decompiler
// Type: Mypage00117Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Mypage00117Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel textTitle;
  [SerializeField]
  private UITextList textList;

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public IEnumerator InitSceneAsync(string title, string text)
  {
    this.textTitle.SetTextLocalize(title);
    this.textList.Clear();
    this.textList.Add(text);
    yield return (object) null;
    this.textList.scrollValue = 0.0f;
  }

  public override void onBackButton() => this.IbtnBack();
}
