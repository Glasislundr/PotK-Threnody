// Decompiled with JetBrains decompiler
// Type: InfomationButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;

#nullable disable
public class InfomationButton : MypageEventButton
{
  public override bool IsActive() => true;

  public override bool IsBadge()
  {
    OfficialInformationArticle[] array = SMManager.Get<OfficialInformationArticle[]>();
    if (array == null)
      return false;
    try
    {
      return Array.FindIndex<OfficialInformationArticle>(array, (Predicate<OfficialInformationArticle>) (w => !Persist.infoUnRead.Data.GetUnRead(w))) > -1;
    }
    catch
    {
      Persist.infoUnRead.Delete();
      return array.Length != 0;
    }
  }
}
