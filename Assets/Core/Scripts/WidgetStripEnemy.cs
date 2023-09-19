using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts
{
	public class WidgetStripEnemy : MonoBehaviour
	{
		[SerializeField] private Image imgEnemy;

		public void SetEnemySprite(Sprite sprite) => imgEnemy.sprite = sprite;
	} 
}