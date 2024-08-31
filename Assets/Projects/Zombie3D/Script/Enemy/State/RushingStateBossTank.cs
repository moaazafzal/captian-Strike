using UnityEngine;
using System.Collections;
namespace Zombie3D
{



    /*   
     * 
     * 
     */

    public class RushingStateBossTank : EnemyState
    {

        public override void NextState(Enemy enemy, float deltaTime, Player player)
        {
            if (enemy.HP <= 0)
            {
                enemy.OnDead();
                enemy.SetState(Enemy.DEAD_STATE);
                return;
            }
            Transform enemyTransform = enemy.GetTransform();

			Tank_Boss tankboss = enemy as Tank_Boss;

			if (tankboss != null)
            {
				if (tankboss.Rush(deltaTime))
                {
					tankboss.SetState(Tank_Boss.RUSHINGATTACK_STATE);
                }

            }

        }

        public override void OnHit(Enemy enemy)
        {

        }
    }
}