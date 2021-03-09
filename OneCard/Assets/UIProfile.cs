using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProfile : MonoBehaviour
{
    private string PlayerNickName;
    private string PlayerRank;
    private Image PlayerImage;
    private int PlayerCount;
    [SerializeField]
    Player[] player;

    TurnManager turnManager;
    private void Awake()
    {
        turnManager = FindObjectOfType<TurnManager>();
        PlayerCount = turnManager.PlayerCount;
        player = turnManager.getorderPlaeyrs();
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PlayerCount; i++)
        {
            player[i].setPlayerProfile(PlayerNickName, PlayerRank, PlayerImage);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
