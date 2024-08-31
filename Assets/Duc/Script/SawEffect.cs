using UnityEngine;
using System.Collections;
using Zombie3D;

public class SawEffect : MonoBehaviour
{

	public GameObject _SawEffect;
	protected GameScene gameScene;
	protected Player player;

	public void Start()
	{
		gameScene = GameApp.GetInstance().GetGameScene();
		player = gameScene.GetPlayer();

	}
	
	// Update is called once per frame
	void Update () 
	{
		int weaponLogoIndex = GameApp.GetInstance().GetGameState().GetWeaponIndex(player.GetWeapon());
		if (weaponLogoIndex == 8) 
		{
			_SawEffect.SetActive(true);
		}
		else 
		{
			_SawEffect.SetActive(false);
		}
	}
}
