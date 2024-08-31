using UnityEngine;
using System.Collections;
namespace Zombie3D
{



    /*   
     * 
     * 
     */

    public class LookAroundStateBossHunter : EnemyState
    {

        public override void NextState(Enemy enemy, float deltaTime, Player player)
        {
            if (enemy.HP <= 0)
            {
                enemy.OnDead();
                enemy.SetState(Enemy.DEAD_STATE);
                return;
            }

			Hunter_Boss hunterboss = enemy as Hunter_Boss;

            enemy.Animate(AnimationName.ENEMY_IDLE, WrapMode.Loop);
            enemy.GetTransform().LookAt(player.GetTransform());
            //hunter.Patrol(deltaTime);          

			if (hunterboss.LookAroundTimOut())
			{				
				if (hunterboss.ReadyForJump())
				{
					int rnd = Random.Range(0, 100);
					if (rnd < 100)
					{
						hunterboss.StartJump();
						
						hunterboss.SetState(Hunter_Boss.JUMP_STATE);
					}
					else
					{
						hunterboss.SetState(Hunter_Boss.CATCHING_STATE);
					}
				}
				else
				{
					hunterboss.SetState(Hunter_Boss.CATCHING_STATE);
				}
				
			}

        }

        

    }
}