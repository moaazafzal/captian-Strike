using UnityEngine;
using System.Collections;
namespace Zombie3D
{



    /*   
     * 
     * 
     */

    public class RushingStartStateBossTank : EnemyState
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
				tankboss.Audio.PlayAudio(AudioName.SHOUT);
				if (tankboss.IsAnimationPlayedPercentage(AnimationName.ENEMY_RUSHINGSTART, 1.0f))
                {       

					tankboss.SetState(Tank_Boss.RUSHING_STATE);
					tankboss.Animate(AnimationName.ENEMY_RUSHING, WrapMode.Loop);
                }
            }
            

        }

        public override void OnHit(Enemy enemy)
        {

        }
    }
}