// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.API.Request.ChargeAge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Purchase.API.Request
{
  public class ChargeAge : Gsc.Network.Request<ChargeAge, Gsc.Purchase.API.Response.ChargeAge>
  {
    private const string ___path = "/age";

    public int BirthDay { get; set; }

    public int BirthYear { get; set; }

    public int BirthMonth { get; set; }

    public ChargeAge(int birthMonth, int birthYear)
    {
      this.BirthMonth = birthMonth;
      this.BirthYear = birthYear;
    }

    public override string GetPath() => SDK.Configuration.Env.PurchaseApiPrefix + "/age";

    public override string GetMethod() => "POST";

    protected override Dictionary<string, object> GetParameters()
    {
      return new Dictionary<string, object>()
      {
        ["birth_day"] = Serializer.Instance.Add<int>(new Func<int, object>(Serializer.From<int>)).Serialize<int>(this.BirthDay),
        ["birth_year"] = Serializer.Instance.Add<int>(new Func<int, object>(Serializer.From<int>)).Serialize<int>(this.BirthYear),
        ["birth_month"] = Serializer.Instance.Add<int>(new Func<int, object>(Serializer.From<int>)).Serialize<int>(this.BirthMonth)
      };
    }
  }
}
