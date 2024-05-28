// Decompiled with JetBrains decompiler
// Type: Mypage00117Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Mypage00117Scene : NGSceneBase
{
  [SerializeField]
  private Mypage00117Menu menu;

  public IEnumerator onStartSceneAsync(Mypage00117Scene.Rule rule)
  {
    IEnumerator e;
    if (rule == Mypage00117Scene.Rule.TransmissionRule)
    {
      e = this.menu.InitSceneAsync(Consts.GetInstance().MYPAGE001_17_TITLE_TRANSMISSON_RULE, Consts.GetInstance().MYPAGE001_17_DESCRIPT_TRANSMISSON_RULE);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.menu.InitSceneAsync(Consts.GetInstance().MYPAGE001_17_TITLE_TERM_OF_SERVICE, TermsOfService.GetData().content.text);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public enum Rule
  {
    TermsOfService,
    TransmissionRule,
  }
}
