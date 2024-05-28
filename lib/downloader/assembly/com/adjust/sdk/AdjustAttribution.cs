// Decompiled with JetBrains decompiler
// Type: com.adjust.sdk.AdjustAttribution
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace com.adjust.sdk
{
  public class AdjustAttribution
  {
    public string adid { get; set; }

    public string network { get; set; }

    public string adgroup { get; set; }

    public string campaign { get; set; }

    public string creative { get; set; }

    public string clickLabel { get; set; }

    public string trackerName { get; set; }

    public string trackerToken { get; set; }

    public string costType { get; set; }

    public double? costAmount { get; set; }

    public string costCurrency { get; set; }

    public AdjustAttribution()
    {
    }

    public AdjustAttribution(string jsonString)
    {
      JSONNode node = JSON.Parse(jsonString);
      if (node == (object) null)
        return;
      this.trackerName = AdjustUtils.GetJsonString(node, AdjustUtils.KeyTrackerName);
      this.trackerToken = AdjustUtils.GetJsonString(node, AdjustUtils.KeyTrackerToken);
      this.network = AdjustUtils.GetJsonString(node, AdjustUtils.KeyNetwork);
      this.campaign = AdjustUtils.GetJsonString(node, AdjustUtils.KeyCampaign);
      this.adgroup = AdjustUtils.GetJsonString(node, AdjustUtils.KeyAdgroup);
      this.creative = AdjustUtils.GetJsonString(node, AdjustUtils.KeyCreative);
      this.clickLabel = AdjustUtils.GetJsonString(node, AdjustUtils.KeyClickLabel);
      this.adid = AdjustUtils.GetJsonString(node, AdjustUtils.KeyAdid);
      this.costType = AdjustUtils.GetJsonString(node, AdjustUtils.KeyCostType);
      try
      {
        this.costAmount = new double?(double.Parse(AdjustUtils.GetJsonString(node, AdjustUtils.KeyCostAmount), (IFormatProvider) CultureInfo.InvariantCulture));
      }
      catch (Exception ex)
      {
      }
      this.costCurrency = AdjustUtils.GetJsonString(node, AdjustUtils.KeyCostCurrency);
    }

    public AdjustAttribution(Dictionary<string, string> dicAttributionData)
    {
      if (dicAttributionData == null)
        return;
      this.trackerName = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyTrackerName);
      this.trackerToken = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyTrackerToken);
      this.network = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyNetwork);
      this.campaign = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyCampaign);
      this.adgroup = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyAdgroup);
      this.creative = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyCreative);
      this.clickLabel = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyClickLabel);
      this.adid = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyAdid);
      this.costType = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyCostType);
      try
      {
        this.costAmount = new double?(double.Parse(AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyCostAmount), (IFormatProvider) CultureInfo.InvariantCulture));
      }
      catch (Exception ex)
      {
      }
      this.costCurrency = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyCostCurrency);
    }
  }
}
