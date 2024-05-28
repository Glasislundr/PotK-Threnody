// Decompiled with JetBrains decompiler
// Type: Story0090Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class Story0090Scene : NGSceneBase
{
  public Story0090Menu menu;

  public override IEnumerator onInitSceneAsync() => base.onInitSceneAsync();

  public IEnumerator onStartSceneAsync()
  {
    this.menu.Init();
    if (Singleton<CommonRoot>.GetInstance().isLoading)
    {
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    }
  }
}
