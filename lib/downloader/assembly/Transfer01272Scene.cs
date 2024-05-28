// Decompiled with JetBrains decompiler
// Type: Transfer01272Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class Transfer01272Scene : NGSceneBase
{
  public Transfer01272Menu menu;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("transfer012_7_2", stack);
  }

  public IEnumerator onStartSceneAsync()
  {
    this.menu.InitTransfer();
    yield break;
  }
}
