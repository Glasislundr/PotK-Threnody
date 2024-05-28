// Decompiled with JetBrains decompiler
// Type: Bugu005MaterialListScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class Bugu005MaterialListScene : NGSceneBase
{
  public Bugu005MaterialListMenu menu;

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_material_list", stack);
  }

  public virtual IEnumerator onStartSceneAsync()
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    IEnumerator e = this.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void onStartScene()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = true;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    this.StartCoroutine(this.menu.GoldSell());
  }

  public virtual void onBackScene() => this.menu.onBackScene();

  public override void onEndScene() => this.menu.onEndScene();
}
