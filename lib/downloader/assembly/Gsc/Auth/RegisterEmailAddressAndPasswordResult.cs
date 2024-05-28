// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.RegisterEmailAddressAndPasswordResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace Gsc.Auth
{
  public struct RegisterEmailAddressAndPasswordResult
  {
    public RegisterEmailAddressAndPasswordResultCode ResultCode { get; private set; }

    public RegisterEmailAddressAndPasswordResult(
      RegisterEmailAddressAndPasswordResultCode resultCode)
    {
      this.ResultCode = resultCode;
    }

    public static bool operator true(RegisterEmailAddressAndPasswordResult self)
    {
      return self.ResultCode == RegisterEmailAddressAndPasswordResultCode.Success;
    }

    public static bool operator false(RegisterEmailAddressAndPasswordResult self)
    {
      return self.ResultCode != 0;
    }

    public static bool operator ==(
      RegisterEmailAddressAndPasswordResult self,
      RegisterEmailAddressAndPasswordResultCode resultCode)
    {
      return self.ResultCode == resultCode;
    }

    public static bool operator !=(
      RegisterEmailAddressAndPasswordResult self,
      RegisterEmailAddressAndPasswordResultCode resultCode)
    {
      return self.ResultCode != resultCode;
    }

    public override bool Equals(object obj) => base.Equals(obj);

    public override int GetHashCode() => base.GetHashCode();
  }
}
