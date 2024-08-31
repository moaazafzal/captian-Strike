using UnityEngine;
using System.Collections;
namespace Zombie3D
{



    /*   
     * 
     * 
     */

    public class JumpStateBossHunter : EnemyState
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

			Hunter_Boss hunterboss = enemy as Hunter_Boss;
            
			if (hunterboss != null)
			{
				if (hunterboss.Jump(deltaTime))
				{
					hunterboss.SetState(Hunter_Boss.IDLE_STATE);
				}
				else
				{
					if (!hunterboss.JumpEnded)
					{
						hunterboss.Animate(AnimationName.ENEMY_JUMPING, WrapMode.Loop);
					}
				}
				
			}

        }

        public override void OnHit(Enemy enemy)
        {

        }
    }
}