// Decompiled with JetBrains decompiler
// Type: Persist`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore.Serialization;

#nullable disable
public class Persist<T> : Persist.ISaveUnit where T : class, new()
{
  private T value;

  public Persist(string fileName)
    : base(fileName)
  {
  }

  public T Data
  {
    get
    {
      if ((object) this.value == null)
      {
        object obj = this.DeserializeObjectFromFile(this.FilePath);
        this.value = obj != null ? (T) obj : new T();
      }
      return this.value;
    }
    set => this.value = value;
  }

  public override void NewData() => this.value = new T();

  public override void Clear() => this.value = default (T);

  public override void Flush()
  {
    EasySerializer.SerializeObjectToFile((object) this.Data, this.FilePath);
  }
}
