// Decompiled with JetBrains decompiler
// Type: Crypt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Security.Cryptography;
using System.Text;

#nullable disable
public static class Crypt
{
  private static AesCryptoServiceProvider GetCryptInstance()
  {
    AesCryptoServiceProvider cryptInstance = new AesCryptoServiceProvider();
    cryptInstance.BlockSize = 128;
    cryptInstance.KeySize = 256;
    cryptInstance.IV = Encoding.UTF8.GetBytes("4689a688-1b17-4a");
    cryptInstance.Key = Encoding.UTF8.GetBytes("6589f002-e5a0-4028-8648-54be143a");
    cryptInstance.Mode = CipherMode.CBC;
    cryptInstance.Padding = PaddingMode.PKCS7;
    return cryptInstance;
  }

  public static byte[] CryptProcess(byte[] src, bool isEncrypt)
  {
    AesCryptoServiceProvider aes = Crypt.GetCryptInstance();
    byte[] numArray = ((Func<byte[]>) (() =>
    {
      using (ICryptoTransform encryptor = aes.CreateEncryptor())
        return encryptor.TransformFinalBlock(src, 0, src.Length);
    }))();
    return !isEncrypt ? ((Func<byte[]>) (() =>
    {
      using (ICryptoTransform decryptor = aes.CreateDecryptor())
        return decryptor.TransformFinalBlock(src, 0, src.Length);
    }))() : ((Func<byte[]>) (() =>
    {
      using (ICryptoTransform encryptor = aes.CreateEncryptor())
        return encryptor.TransformFinalBlock(src, 0, src.Length);
    }))();
  }

  public static byte[] Encrypt(byte[] src)
  {
    using (ICryptoTransform encryptor = Crypt.GetCryptInstance().CreateEncryptor())
      return encryptor.TransformFinalBlock(src, 0, src.Length);
  }

  public static byte[] Decrypt(byte[] src)
  {
    using (ICryptoTransform decryptor = Crypt.GetCryptInstance().CreateDecryptor())
      return decryptor.TransformFinalBlock(src, 0, src.Length);
  }
}
