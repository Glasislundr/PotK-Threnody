// Decompiled with JetBrains decompiler
// Type: GuildEstablishmentPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GuildEstablishmentPopup : MonoBehaviour
{
  [SerializeField]
  private Animator anim;
  private const float animNormalSpeed = 1f;
  private const float animSkipSpeed = 162f;

  public void Skip() => this.anim.speed = 162f;

  public void Stop() => this.anim.speed = 1f;

  public void ChangeScene()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD, true);
  }

  public void PlaySound(string clip) => Singleton<NGSoundManager>.GetInstance().PlaySe(clip);
}
