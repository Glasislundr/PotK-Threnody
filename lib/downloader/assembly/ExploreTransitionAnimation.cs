// Decompiled with JetBrains decompiler
// Type: ExploreTransitionAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ExploreTransitionAnimation : MonoBehaviour
{
  [SerializeField]
  private SpriteTransitionController mTrantionController;
  [SerializeField]
  private Texture2D mMaskIn;
  [SerializeField]
  private Texture2D mMaskOut;
  public bool IsExploreOnly;

  public void SetInMode() => this.mTrantionController.mask = this.mMaskIn;

  public void SetOutMode() => this.mTrantionController.mask = this.mMaskOut;

  private void playSound(string seName)
  {
    if (this.IsExploreOnly && !Singleton<ExploreSceneManager>.GetInstance().IsSceneActive || string.IsNullOrEmpty(seName))
      return;
    string[] strArray = seName.Split('.');
    Singleton<NGSoundManager>.GetInstance().playSE(strArray[0]);
  }
}
