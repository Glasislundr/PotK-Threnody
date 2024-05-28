// Decompiled with JetBrains decompiler
// Type: Bugu005ReisouFusionMaterialScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;

#nullable disable
public class Bugu005ReisouFusionMaterialScene : NGSceneBase
{
  public Bugu005ReisouFusionMaterialMenu menu;

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public static void ChangeScene(bool stack, PlayerItem baseItem)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_reisou_fusion_material", (stack ? 1 : 0) != 0, (object) baseItem);
  }

  public virtual IEnumerator onStartSceneAsync(PlayerItem baseItem)
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    IEnumerator e = this.menu.Init(baseItem);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void onStartScene(PlayerItem baseItem)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = true;
  }

  public virtual void onBackScene(PlayerItem baseItem) => this.menu.onBackScene();

  public override void onEndScene() => this.menu.onEndScene();
}
