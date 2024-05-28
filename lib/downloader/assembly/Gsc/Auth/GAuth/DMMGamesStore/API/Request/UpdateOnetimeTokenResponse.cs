// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.GAuth.DMMGamesStore.API.Request.UpdateOnetimeTokenResponse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network;
using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Auth.GAuth.DMMGamesStore.API.Request
{
  public class UpdateOnetimeTokenResponse : Response<UpdateOnetimeTokenResponse>
  {
    public string OnetimeToken { get; private set; }

    public UpdateOnetimeTokenResponse(byte[] payload)
    {
      Dictionary<string, object> result = Response<UpdateOnetimeTokenResponse>.GetResult(payload);
      this.OnetimeToken = Deserializer.Instance.Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string>(result["dmm_onetime_token"]);
    }
  }
}
