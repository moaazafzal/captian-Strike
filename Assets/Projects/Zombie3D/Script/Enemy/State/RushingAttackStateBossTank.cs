using UnityEngine;
using System.Collections;
namespace Zombie3D
{



    /*   
     * 
     * 
     */

    public class RushingAttackStateBossTank : EnemyState
    {

        public override void NextState(Enemy enemy, float deltaTime, Player player)
        {
            if (enemy.HP <= 0)
            {
                enemy.OnDead();
                enemy.SetState(Enemy.DEAD_STATE);
                return;
            }

			Tank_Boss tankboss = enemy as Tank_Boss;

            if (tankboss != null)
            {
                if (tankboss.RushAttack(deltaTime))
                {
                    enemy.SetState(Enemy.IDLE_STATE);

                }

            }


        }

        public override void OnHit(Enemy enemy)
        {

        }
    }
}