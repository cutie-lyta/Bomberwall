using UnityEngine;
using UnityEngine.UI;

public class AIBombUI : MonoBehaviour
{
	private AIBombPlacer _placer;

	[SerializeField] private Image _slot1;
	[SerializeField] private Image _slot2;
	
    private void Awake(){
		_placer = GetComponent<AIBombPlacer>();
		_placer.OnBombPlaced += () =>
		{
			_slot1.color = new Color32(0, 0, 0, 0);
			_slot2.color = new Color32(0, 0, 0, 0);
		};

		_placer.OnNewBomb += i =>
		{
			if (i == 1)
			{
				_slot1.color = new Color32(0, 0, 0, 255);
			}
			else if (i == 2)
			{
				_slot1.color = new Color32(0, 0, 0, 255);
				_slot2.color = new Color32(0, 0, 0, 255);
			}
		};
    }
    
    
}
