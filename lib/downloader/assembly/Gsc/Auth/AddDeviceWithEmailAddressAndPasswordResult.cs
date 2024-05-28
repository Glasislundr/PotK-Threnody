// Decompiled with JetBrains decompiler
// Type: Gsc.Auth.AddDeviceWithEmailAddressAndPasswordResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace Gsc.Auth
{
  public struct AddDeviceWithEmailAddressAndPasswordResult
  {
    public AddDeviceWithEmailAddressAndPasswordResultCode ResultCode { get; private set; }

    public int LockedExpiresIn { get; private set; }

    public int TrialCounter { get; private set; }

    public AddDeviceWithEmailAddressAndPasswordResult(
      AddDeviceWithEmailAddressAndPasswordResultCode resultCode)
      : this(resultCode, 0, 0)
    {
    }

    public AddDeviceWithEmailAddressAndPasswordResult(
      AddDeviceWithEmailAddressAndPasswordResultCode resultCode,
      int lockedExpiresIn,
      int trialCounter)
    {
      this.ResultCode = resultCode;
      this.LockedExpiresIn = lockedExpiresIn;
      this.TrialCounter = trialCounter;
    }

    public static bool operator true(AddDeviceWithEmailAddressAndPasswordResult self)
    {
      return self.ResultCode == AddDeviceWithEmailAddressAndPasswordResultCode.Success;
    }

    public static bool operator false(AddDeviceWithEmailAddressAndPasswordResult self)
    {
      return self.ResultCode != 0;
    }

    public static bool operator ==(
      AddDeviceWithEmailAddressAndPasswordResult self,
      AddDeviceWithEmailAddressAndPasswordResultCode resultCode)
    {
      return self.ResultCode == resultCode;
    }

    public static bool operator !=(
      AddDeviceWithEmailAddressAndPasswordResult self,
      AddDeviceWithEmailAddressAndPasswordResultCode resultCode)
    {
      return self.ResultCode != resultCode;
    }

    public override bool Equals(object obj) => base.Equals(obj);

    public override int GetHashCode() => base.GetHashCode();
  }
}
