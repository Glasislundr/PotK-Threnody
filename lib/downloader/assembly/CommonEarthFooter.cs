// Decompiled with JetBrains decompiler
// Type: CommonEarthFooter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CommonEarthFooter : CommonFooterBase
{
  public void onButtonMypageEarth()
  {
    if (Singleton<NGSceneManager>.GetInstance().sceneName == "mypage051")
    {
      Mypage051Scene sceneBase = Singleton<NGSceneManager>.GetInstance().sceneBase as Mypage051Scene;
      if (Object.op_Inequality((Object) sceneBase, (Object) null) && sceneBase.isAnimePlaying)
        return;
    }
    Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = true;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Singleton<NGSceneManager>.GetInstance().clearStack();
    Mypage051Scene.ChangeScene(false);
  }

  public void onButtonUnitEarth()
  {
    Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = true;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    this.changeScene("unit054_1", false, true);
  }

  public void onButtonWeaponEarth()
  {
    Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = true;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    this.changeScene("bugu055_1", false, true);
  }

  public void onButtonShopEarth()
  {
    Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = true;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    this.changeScene("shop057_4", false, true);
  }
}
