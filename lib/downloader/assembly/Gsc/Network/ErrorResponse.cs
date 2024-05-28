// Decompiled with JetBrains decompiler
// Type: Gsc.Network.ErrorResponse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.DOM;
using Gsc.DOM.Json;

#nullable disable
namespace Gsc.Network
{
  public class ErrorResponse : Response<ErrorResponse>, IErrorResponse, IResponse
  {
    public IDocument data { get; private set; }

    public string ErrorCode { get; private set; }

    public ErrorResponse(byte[] payload)
    {
      if (payload.Length == 0)
        return;
      Document document = Document.Parse(payload);
      this.data = (IDocument) document;
      this.ErrorCode = document.Root.GetValueByPointer("/error_code", (string) null);
    }
  }
}
