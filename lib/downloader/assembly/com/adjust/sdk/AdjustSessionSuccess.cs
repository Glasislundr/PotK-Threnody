// Decompiled with JetBrains decompiler
// Type: com.adjust.sdk.AdjustSessionSuccess
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace com.adjust.sdk
{
  public class AdjustSessionSuccess
  {
    public string Adid { get; set; }

    public string Message { get; set; }

    public string Timestamp { get; set; }

    public Dictionary<string, object> JsonResponse { get; set; }

    public AdjustSessionSuccess()
    {
    }

    public AdjustSessionSuccess(Dictionary<string, string> sessionSuccessDataMap)
    {
      if (sessionSuccessDataMap == null)
        return;
      this.Adid = AdjustUtils.TryGetValue(sessionSuccessDataMap, AdjustUtils.KeyAdid);
      this.Message = AdjustUtils.TryGetValue(sessionSuccessDataMap, AdjustUtils.KeyMessage);
      this.Timestamp = AdjustUtils.TryGetValue(sessionSuccessDataMap, AdjustUtils.KeyTimestamp);
      JSONNode jsonNode = JSON.Parse(AdjustUtils.TryGetValue(sessionSuccessDataMap, AdjustUtils.KeyJsonResponse));
      if (!(jsonNode != (object) null) || !((JSONNode) jsonNode.AsObject != (object) null))
        return;
      this.JsonResponse = new Dictionary<string, object>();
      AdjustUtils.WriteJsonResponseDictionary(jsonNode.AsObject, this.JsonResponse);
    }

    public AdjustSessionSuccess(string jsonString)
    {
      JSONNode node = JSON.Parse(jsonString);
      if (node == (object) null)
        return;
      this.Adid = AdjustUtils.GetJsonString(node, AdjustUtils.KeyAdid);
      this.Message = AdjustUtils.GetJsonString(node, AdjustUtils.KeyMessage);
      this.Timestamp = AdjustUtils.GetJsonString(node, AdjustUtils.KeyTimestamp);
      JSONNode jsonNode = node[AdjustUtils.KeyJsonResponse];
      if (jsonNode == (object) null || (JSONNode) jsonNode.AsObject == (object) null)
        return;
      this.JsonResponse = new Dictionary<string, object>();
      AdjustUtils.WriteJsonResponseDictionary(jsonNode.AsObject, this.JsonResponse);
    }

    public void BuildJsonResponseFromString(string jsonResponseString)
    {
      JSONNode jsonNode = JSON.Parse(jsonResponseString);
      if (jsonNode == (object) null)
        return;
      this.JsonResponse = new Dictionary<string, object>();
      AdjustUtils.WriteJsonResponseDictionary(jsonNode.AsObject, this.JsonResponse);
    }

    public string GetJsonResponse() => AdjustUtils.GetJsonResponseCompact(this.JsonResponse);
  }
}
