// Decompiled with JetBrains decompiler
// Type: Gsc.Network.Data.Entity`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace Gsc.Network.Data
{
  public abstract class Entity<T> : IEntity, IObject where T : Entity<T>
  {
    private uint ver;

    public string pk { get; protected set; }

    public abstract void Update();

    public abstract void ResolveRefs();

    public T Clone() => (T) this.MemberwiseClone();

    IEntity IEntity.Clone() => (IEntity) this.Clone();

    protected bool IsUpdatedOnce()
    {
      int num = (int) this.ver != (int) EntityRepository.ver ? 1 : 0;
      this.ver = EntityRepository.ver;
      return num != 0;
    }
  }
}
