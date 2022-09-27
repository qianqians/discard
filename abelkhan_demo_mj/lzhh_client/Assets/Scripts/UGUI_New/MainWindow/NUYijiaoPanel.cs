using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NUYijiaoPanel:MonoBehaviour
{
    public GameObject YijiaoPanel = null;
    public GameObject CreateGamePanel = null;
    public GameObject JoinGamePanel = null;
    public GameObject LobbyPanel = null;

    public Button OpenCreateGamePanelBtn = null;
    public Button OpenJoinGamePanelBtn = null;

    // Use this for initialization
    public Button backBtn;
    void Start () {
        backBtn.onClick.AddListener(delegate ()
        {
            YijiaoPanel.SetActive(false);
            LobbyPanel.SetActive(true);
        });

        OpenCreateGamePanelBtn.onClick.AddListener(delegate ()
        {
            YijiaoPanel.SetActive(false);
            CreateGamePanel.SetActive(true);
        });
    
        OpenJoinGamePanelBtn.onClick.AddListener(delegate ()
        {
            YijiaoPanel.SetActive(false);
            JoinGamePanel.SetActive(true);
        });
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
