using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{

	public TextMeshProUGUI text;

	public bool isPlayerA = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (isPlayerA)
			text.text = GameManager.playerAScore.ToString();
		else
			text.text = GameManager.playerBScore.ToString();

	}
}
