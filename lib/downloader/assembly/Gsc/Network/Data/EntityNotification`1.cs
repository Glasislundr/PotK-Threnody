// Decompiled with JetBrains decompiler
// Type: Gsc.Network.Data.EntityNotification`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Components;

#nullable disable
namespace Gsc.Network.Data
{
  public class EntityNotification<T> : INotification where T : Gsc.Network.Data.Entity<T>
  {
    public readonly T Entity;
    public readonly EntityNotificationType NotificationType;

    public EntityNotification(T entity, EntityNotificationType type)
    {
      this.Entity = entity;
      this.NotificationType = type;
    }
  }
}
