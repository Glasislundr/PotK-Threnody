// Decompiled with JetBrains decompiler
// Type: com.adjust.sdk.AdjustThirdPartySharing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace com.adjust.sdk
{
  public class AdjustThirdPartySharing
  {
    internal bool? isEnabled;
    internal Dictionary<string, List<string>> granularOptions;

    public AdjustThirdPartySharing(bool? isEnabled)
    {
      this.isEnabled = isEnabled;
      this.granularOptions = new Dictionary<string, List<string>>();
    }

    public void addGranularOption(string partnerName, string key, string value)
    {
      if (partnerName == null || key == null || value == null)
        return;
      List<string> stringList;
      if (this.granularOptions.ContainsKey(partnerName))
      {
        stringList = this.granularOptions[partnerName];
      }
      else
      {
        stringList = new List<string>();
        this.granularOptions.Add(partnerName, stringList);
      }
      stringList.Add(key);
      stringList.Add(value);
    }
  }
}
