// Decompiled with JetBrains decompiler
// Type: PresentInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;

#nullable disable
public class PresentInfo
{
  public Mypage0017Scroll scroll;

  public PlayerPresent present { get; set; }

  public PresentInfo TempCopy()
  {
    PresentInfo presentInfo = (PresentInfo) this.MemberwiseClone();
    presentInfo.scroll = (Mypage0017Scroll) null;
    return presentInfo;
  }
}
