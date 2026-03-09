using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Загальний інтерфейс сенсора для НПС.
/// TTarget — ціль для роботи (дерево, камінь, рослина тощо).
/// TDrop   — ресурс що випадає після роботи.
/// </summary>
public interface ISensor<TTarget, TDrop>
    where TTarget : Component
    where TDrop : Component
{
    /// <summary>
    /// Знаходить найближчу ціль для роботи.
    /// </summary>
    bool TryFindNearest(out TTarget target);

    /// <summary>
    /// Знаходить найближчий випавший ресурс.
    /// </summary>
    TDrop AcquireNearestDrop();

    /// <summary>
    /// Повертає всі випавші ресурси у вказаному радіусі.
    /// </summary>
    List<TDrop> GetDropsInRadius(float radius);
}
