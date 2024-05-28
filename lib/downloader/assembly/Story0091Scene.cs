// Decompiled with JetBrains decompiler
// Type: Story0091Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Story0091Scene : NGSceneBase
{
  public Story0091Menu menu;
  [SerializeField]
  private NGxScroll ScrollContainer;

  public IEnumerator onStartSceneAsync(int XL)
  {
    PlayerStoryQuestS[] self = SMManager.Get<PlayerStoryQuestS[]>();
    this.ScrollContainer.Clear();
    IEnumerator e = this.menu.InitPartButton(self.DisplayScrollL(XL), XL);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ScrollContainer.ResolvePosition();
    if (Singleton<CommonRoot>.GetInstance().isLoading)
    {
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    }
  }
}
