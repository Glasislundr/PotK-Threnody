// Decompiled with JetBrains decompiler
// Type: BattleCutScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleCutScene : NGSceneBase
{
  private BattleCutSceneMenu menu;
  private Color mOriginalAmbient;
  private GameObject mNonDuelDirectionalLight;

  public static void changeScene(bool stack, BL.Unit unit, BL.Skill skill)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(nameof (BattleCutScene), (stack ? 1 : 0) != 0, (object) unit, (object) skill);
  }

  public override IEnumerator onInitSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BattleCutScene battleCutScene = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battleCutScene.menu = battleCutScene.menuBase as BattleCutSceneMenu;
    battleCutScene.mOriginalAmbient = RenderSettings.ambientLight;
    battleCutScene.mNonDuelDirectionalLight = GameObject.Find("Directional light");
    return false;
  }

  public IEnumerator onStartSceneAsync(BL.Unit unit, BL.Skill skill)
  {
    RenderSettings.ambientLight = new Color(1f, 1f, 1f);
    if (Object.op_Inequality((Object) this.mNonDuelDirectionalLight, (Object) null))
      this.mNonDuelDirectionalLight.SetActive(false);
    yield return (object) this.menu.OnStartSceneAsync(unit, skill);
  }

  public void onStartScene(BL.Unit unit, BL.Skill skill)
  {
    Singleton<CommonRoot>.GetInstance().isActiveBackground3DCamera = false;
    this.menu.OnStartScene();
  }

  public override void onEndScene()
  {
    this.menu.OnEndScene();
    Singleton<CommonRoot>.GetInstance().isActiveBackground3DCamera = true;
    RenderSettings.ambientLight = this.mOriginalAmbient;
    if (Object.op_Inequality((Object) this.mNonDuelDirectionalLight, (Object) null))
      this.mNonDuelDirectionalLight.SetActive(true);
    Singleton<NGSoundManager>.GetInstance().crossFadeCurrentBGM(2.5f, 0.0f);
  }
}
