using UnityEngine;
using System.Collections;
namespace Zombie3D
{



    /*   
     * 
     * 
     */

    public class RushingAttackState : EnemyState
    {

        public override void NextState(Enemy enemy, float deltaTime, Player player)
        {
            if (enemy.HP <= 0)
            {
                enemy.OnDead();
                enemy.SetState(Enemy.DEAD_STATE);
                return;
            }

            Tank tank = enemy as Tank;

            if (tank != null)
            {
                if (tank.RushAttack(deltaTime))
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