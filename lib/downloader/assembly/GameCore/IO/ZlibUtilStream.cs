// Decompiled with JetBrains decompiler
// Type: GameCore.IO.ZlibUtilStream
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using gu3.Utils;
using Ionic.Zlib;
using System;
using System.IO;

#nullable disable
namespace GameCore.IO
{
  public class ZlibUtilStream : Stream
  {
    private Stream stream;

    public override long Seek(long offset, SeekOrigin origin) => this.stream.Seek(offset, origin);

    public override void SetLength(long value) => this.stream.SetLength(value);

    public override bool CanRead => this.stream.CanRead;

    public override bool CanWrite => this.stream.CanWrite;

    public override bool CanSeek => this.stream.CanSeek;

    public override long Length => this.stream.Length;

    public override long Position
    {
      get => this.stream.Position;
      set => this.stream.Position = value;
    }

    protected override void Dispose(bool disposing) => this.stream.Dispose();

    public override void Flush() => this.stream.Flush();

    public override int Read(byte[] buffer, int offset, int count)
    {
      return this.stream.Read(buffer, offset, count);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      this.stream.Write(buffer, offset, count);
    }

    private ZlibUtilStream(Stream stream) => this.stream = stream;

    public static ZlibUtilStream Decompress(Stream stream, ZlibFormat format)
    {
      if (format == null)
        return new ZlibUtilStream((Stream) new DeflateStream(stream, (CompressionMode) 1));
      if (format == 1)
        return new ZlibUtilStream((Stream) new ZlibStream(stream, (CompressionMode) 1));
      if (format == 2)
        return new ZlibUtilStream((Stream) new GZipStream(stream, (CompressionMode) 1));
      throw new Exception("format error");
    }

    public static ZlibUtilStream DecompressBytes(
      byte[] buffer,
      int offset,
      int length,
      ZlibFormat format)
    {
      if (format == null)
        return new ZlibUtilStream((Stream) new DeflateStream((Stream) new MemoryStream(buffer, offset, length), (CompressionMode) 1));
      if (format == 1)
        return new ZlibUtilStream((Stream) new ZlibStream((Stream) new MemoryStream(buffer, offset, length), (CompressionMode) 1));
      if (format == 2)
        return new ZlibUtilStream((Stream) new GZipStream((Stream) new MemoryStream(buffer, offset, length), (CompressionMode) 1));
      throw new Exception("format error");
    }

    public static ZlibUtilStream Compress(
      Stream stream,
      ZlibFormat format,
      ZlibCompressionLevel compressionLevel)
    {
      CompressionLevel compressionLevel1 = compressionLevel == -1 ? (CompressionLevel) (object) 6 : (CompressionLevel) (object) compressionLevel;
      if (format == null)
        return new ZlibUtilStream((Stream) new DeflateStream(stream, (CompressionMode) 0, compressionLevel1));
      if (format == 1)
        return new ZlibUtilStream((Stream) new ZlibStream(stream, (CompressionMode) 0, compressionLevel1));
      if (format == 2)
        return new ZlibUtilStream((Stream) new GZipStream(stream, (CompressionMode) 0, compressionLevel1));
      throw new Exception("format error");
    }
  }
}
