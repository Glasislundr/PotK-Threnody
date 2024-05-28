﻿// Decompiled with JetBrains decompiler
// Type: LogKit.Writter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MiniJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace LogKit
{
  public class Writter
  {
    private static readonly DateTime timeStampDelta = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private readonly List<Buffer> mBuffers = new List<Buffer>();
    private readonly string key;

    public Writter(string key) => this.key = key;

    public void Push(Buffer buffer)
    {
      lock (this.mBuffers)
        this.mBuffers.Add(buffer);
    }

    public void Flush()
    {
      lock (this.mBuffers)
      {
        for (int index1 = 0; index1 < this.mBuffers.Count; ++index1)
        {
          Buffer mBuffer = this.mBuffers[index1];
          List<object> objectList = new List<object>(mBuffer.Count);
          for (int index2 = 0; index2 < mBuffer.Count; ++index2)
          {
            Log log = mBuffer[index2];
            objectList.Add((object) new Dictionary<string, object>()
            {
              {
                "app_id",
                (object) Logger.AppName
              },
              {
                "id",
                (object) log.ID.ToString()
              },
              {
                "tag",
                (object) string.Format("{0}.{1}", (object) this.key, (object) log.Tag)
              },
              {
                "timestamp",
                (object) (log.Date - Writter.timeStampDelta).TotalSeconds
              },
              {
                "log_level",
                (object) log.LogLevel
              },
              {
                "device_id",
                (object) Logger.DeviceID
              },
              {
                "os",
                (object) "windows"
              },
              {
                "platform",
                (object) Logger.Platform
              },
              {
                "log_version",
                (object) 2
              },
              {
                "user_info",
                (object) log.UserInfo
              }
            });
          }
          Guid id = mBuffer[0].ID;
          byte[] bytes = Encoding.UTF8.GetBytes(Json.Serialize((object) objectList));
          using (FileStream fileStream = new FileStream(Logger.GetLogFilePath(this.key, id), FileMode.CreateNew))
          {
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
          }
          mBuffer.Clear();
          mBuffer.Release();
        }
        this.mBuffers.Clear();
      }
    }
  }
}
