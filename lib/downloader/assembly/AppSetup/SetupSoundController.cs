// Decompiled with JetBrains decompiler
// Type: AppSetup.SetupSoundController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace AppSetup
{
  public class SetupSoundController : MonoBehaviour
  {
    [SerializeField]
    private UIButton normalButton;
    [SerializeField]
    private UIButton highButton;

    public void OnNormalButton()
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
      Persist.normalDLC.Data.IsSound = true;
      Persist.normalDLC.Data.IsSoundSetup = true;
      Persist.normalDLC.Flush();
    }

    public void OnHighButton()
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
      Persist.normalDLC.Data.IsSound = false;
      Persist.normalDLC.Data.IsSoundSetup = true;
      Persist.normalDLC.Flush();
    }
  }
}
