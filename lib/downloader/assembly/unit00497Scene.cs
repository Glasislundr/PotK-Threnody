// Decompiled with JetBrains decompiler
// Type: unit00497Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class unit00497Scene : NGSceneBase
{
  [SerializeField]
  private unit00497Menu menu;
  private PrincesEvolutionParam changeSceneParam;
  private string nowBgmName;

  public PrincesEvolutionParam ScenePara => this.changeSceneParam;

  public static void ChangeScene(bool stack, PrincesEvolutionParam param)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_9_7", (stack ? 1 : 0) != 0, (object) param);
  }

  public IEnumerator onStartSceneAsync(PrincesEvolutionParam param)
  {
    this.changeSceneParam = param;
    IEnumerator e = this.menu.Init(param);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<PopupManager>.GetInstance().closeAll();
    this.nowBgmName = Singleton<NGSoundManager>.GetInstance().GetBgmName();
    Singleton<NGSoundManager>.GetInstance().StopBgm();
  }

  public void onStartScene(PrincesEvolutionParam param) => this.onStartScene();

  public override void onEndScene()
  {
    base.onEndScene();
    Singleton<PopupManager>.GetInstance().open((GameObject) null);
    Singleton<NGSoundManager>.GetInstance().PlayBgm(this.nowBgmName);
  }

  public override IEnumerator onEndSceneAsync()
  {
    yield return (object) new WaitForSeconds(0.5f);
    ((Component) this.menu.effect).gameObject.SetActive(false);
  }
}
