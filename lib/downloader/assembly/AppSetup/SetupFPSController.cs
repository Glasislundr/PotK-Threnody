// Decompiled with JetBrains decompiler
// Type: AppSetup.SetupFPSController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace AppSetup
{
  public class SetupFPSController : MonoBehaviour
  {
    [SerializeField]
    private UIButton uiButton30;
    [SerializeField]
    private UIButton uiButton60;
    [SerializeField]
    private bool isChangeDisable;
    private string uiButton30NormalSpriteName;
    private string uiButton30DisabledSpriteName;
    private string uiButton60NormalSpriteName;
    private string uiButton60DisabledSpriteName;

    private void Start()
    {
      EventDelegate.Set(this.uiButton30.onClick, (EventDelegate.Callback) (() => this.OnChangeFps30()));
      EventDelegate.Set(this.uiButton60.onClick, (EventDelegate.Callback) (() => this.OnChangeFps60()));
      if (this.isChangeDisable)
      {
        this.uiButton30NormalSpriteName = this.uiButton30.normalSprite;
        this.uiButton30DisabledSpriteName = this.uiButton30.disabledSprite;
        this.uiButton60NormalSpriteName = this.uiButton30.normalSprite;
        this.uiButton60DisabledSpriteName = this.uiButton30.disabledSprite;
      }
      this.SetCurrentButton(Persist.appFPS.Data.MaxFPS);
    }

    public void OnChangeFps30()
    {
      AppSetupFPS.SetMaxFPS(30);
      this.SetCurrentButton(30);
      this.Save(30);
    }

    public void OnChangeFps60()
    {
      AppSetupFPS.SetMaxFPS(60);
      this.SetCurrentButton(60);
      this.Save(60);
    }

    private void SetCurrentButton(int fps)
    {
      if (!this.isChangeDisable)
        return;
      if (fps == 30)
      {
        this.uiButton30.normalSprite = this.uiButton30NormalSpriteName;
        this.uiButton60.normalSprite = this.uiButton60DisabledSpriteName;
      }
      else
      {
        this.uiButton30.normalSprite = this.uiButton30DisabledSpriteName;
        this.uiButton60.normalSprite = this.uiButton60NormalSpriteName;
      }
    }

    private void Save(int fps)
    {
      if (Persist.appFPS.Data.IsSetup && Persist.appFPS.Data.MaxFPS == fps)
        return;
      Persist.appFPS.Data.IsSetup = true;
      Persist.appFPS.Data.MaxFPS = fps;
      Persist.appFPS.Flush();
    }
  }
}
