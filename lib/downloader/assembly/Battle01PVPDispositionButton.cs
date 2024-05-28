// Decompiled with JetBrains decompiler
// Type: Battle01PVPDispositionButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class Battle01PVPDispositionButton : NGBattleMenuBase
{
  private bool isCompleted;

  public void onClick()
  {
    if (this.isCompleted)
      return;
    this.battleManager.gameEngine.locateUnitsCompleted();
    this.isCompleted = true;
  }
}
