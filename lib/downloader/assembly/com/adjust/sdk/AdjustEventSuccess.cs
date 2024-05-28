// Decompiled with JetBrains decompiler
// Type: com.adjust.sdk.AdjustEventSuccess
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace com.adjust.sdk
{
  public class AdjustEventSuccess
  {
    public string Adid { get; set; }

    public string Message { get; set; }

    public string Timestamp { get; set; }

    public string EventToken { get; set; }

    public string CallbackId { get; set; }

    public Dictionary<string, object> JsonResponse { get; set; }

    public AdjustEventSuccess()
    {
    }

    public AdjustEventSuccess(Dictionary<string, string> eventSuccessDataMap)
    {
      if (eventSuccessDataMap == null)
        return;
      this.Adid = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyAdid);
      this.Message = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyMessage);
      this.Timestamp = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyTimestamp);
      this.EventToken = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyEventToken);
      this.CallbackId = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyCallbackId);
      JSONNode jsonNode = JSON.Parse(AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyJsonResponse));
      if (!(jsonNode != (object) null) || !((JSONNode) jsonNode.AsObject != (object) null))
        return;
      this.JsonResponse = new Dictionary<string, object>();
      AdjustUtils.WriteJsonResponseDictionary(jsonNode.AsObject, this.JsonResponse);
    }

    public AdjustEventSuccess(string jsonString)
    {
      JSONNode node = JSON.Parse(jsonString);
      if (node == (object) null)
        return;
      this.Adid = AdjustUtils.GetJsonString(node, AdjustUtils.KeyAdid);
      this.Message = AdjustUtils.GetJsonString(node, AdjustUtils.KeyMessage);
      this.Timestamp = AdjustUtils.GetJsonString(node, AdjustUtils.KeyTimestamp);
      this.EventToken = AdjustUtils.GetJsonString(node, AdjustUtils.KeyEventToken);
      this.CallbackId = AdjustUtils.GetJsonString(node, AdjustUtils.KeyCallbackId);
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
