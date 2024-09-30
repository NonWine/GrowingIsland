using System.Collections.Generic;

public interface IDetector
{
    List<BaseEnemy> TryFindEnemies();
}