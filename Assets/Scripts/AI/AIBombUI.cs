using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIBombUI : MonoBehaviour
{
	private AIBombPlacer _placer;

	[SerializeField] private List<Image> _images;
	
    private void Awake(){
	    print(GetType().FullName + "::Awake");
		_placer = GetComponent<AIBombPlacer>();
		
		_placer.OnBombPlaced += DrawBombCounter;
		_placer.OnNewBomb += DrawBombCounter;
    }

    private void DrawBombCounter(int bomb)
    {
	    print(GetType().FullName + "::DrawBombCounter");
	    int i = 0;
	    for (; i < bomb; i++)
	    {
		    _images[i].gameObject.SetActive(true);
		    _images[i].color = new Color(0f, 0f, 0f, 1f);
	    }

	    for (; i < _images.Count; i++)
	    {
		    _images[i].gameObject.SetActive(false);
		    _images[i].color = new Color(0f, 0f, 0f, 0f);
	    }
    }
    
    
}
