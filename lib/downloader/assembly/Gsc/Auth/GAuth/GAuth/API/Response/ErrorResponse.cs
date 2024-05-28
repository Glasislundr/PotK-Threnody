// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.GAuth.GAuth.API.Response.ErrorResponse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.DOM;
using Gsc.DOM.Json;
using Gsc.Network;

#nullable disable
namespace Gsc.Auth.GAuth.GAuth.API.Response
{
  public class ErrorResponse : Gsc.Network.Response<ErrorResponse>, IErrorResponse, IResponse
  {
    public string ErrorCode { get; private set; }

    public Document data { get; private set; }

    IDocument IErrorResponse.data => (IDocument) this.data;

    public ErrorResponse(byte[] payload)
    {
      this.data = Document.Parse(payload);
      Value root = this.data.Root;
      string valueByPointer = root.GetValueByPointer("/code", (string) null);
      if (valueByPointer == null)
      {
        root = this.data.Root;
        valueByPointer = root.GetValueByPointer("/error_code", (string) null);
      }
      this.ErrorCode = valueByPointer;
    }
  }
}
