// Decompiled with JetBrains decompiler
// Type: MapEdit031MenuEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class MapEdit031MenuEffect : MapEditMenuBase
{
  private IEnumerator process_;

  public override MapEdit031TopMenu.EditState editState_ => MapEdit031TopMenu.EditState.Effect;

  protected override IEnumerator initializeAsync()
  {
    yield break;
  }

  protected override void onEnable()
  {
    this.ui3DEvent_.isEnabled_ = false;
    this.topMenu_.isActiveButtonStorage_ = true;
    this.topMenu_.isEnabledButtonStorage_ = false;
    this.process_ = this.hasProcessAsync_ ? this.processAsync() : (IEnumerator) null;
  }

  protected override void onDisable()
  {
  }

  protected override void Update()
  {
    if (!this.isInitialized_)
      return;
    base.Update();
    if (this.isWait_ || this.process_ == null)
      return;
    this.StartCoroutine(this.process_);
    this.process_ = (IEnumerator) null;
  }

  public override void onBackButton()
  {
  }
}
