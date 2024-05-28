// Decompiled with JetBrains decompiler
// Type: Mypage051Transition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Mypage051Transition : MonoBehaviour
{
  [SerializeField]
  private Mypage051Menu menu;

  public void onHeavenly()
  {
    if (this.menu.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.playSE("SE_1039");
    this.StartCoroutine(this.menu.CloudAnimationStart());
  }

  public void onEventQuest()
  {
    if (this.menu.IsPushAndSet())
      return;
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.playSE("SE_1002");
    Quest052171Scene.ChangeScene(true);
  }

  public void onQuest()
  {
    if (this.menu.IsPushAndSet())
      return;
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.playSE("SE_1002");
    Quest0528Scene.changeScene(true);
  }

  public void onMenu()
  {
    if (this.menu.IsPushAndSet())
      return;
    Menu0591Scene.ChangeScene(true);
  }
}
