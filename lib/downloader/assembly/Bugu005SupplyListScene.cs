// Decompiled with JetBrains decompiler
// Type: Bugu005SupplyListScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class Bugu005SupplyListScene : NGSceneBase
{
  public Bugu005SupplyListMenu menu;

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_supply_list", stack);
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
  }

  public virtual void onBackScene() => this.menu.onBackScene();

  public override void onEndScene() => this.menu.onEndScene();
}
