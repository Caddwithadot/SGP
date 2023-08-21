using UnityEngine;
using TMPro;
using Photon.Pun;
using Unity.VisualScripting;

public class ConfirmCountdown : MonoBehaviour
{
    public GameObject player;

    public float countdownDuration = 10f;
    private TMP_Text countdownText;

    private float currentCountdown;
    private bool isKicking = false;

    private void Start()
    {
        countdownText = GetComponent<TMP_Text>();

        currentCountdown = countdownDuration;
        UpdateCountdownText();
    }

    private void Update()
    {
        if (currentCountdown > 0f)
        {
            currentCountdown -= Time.deltaTime;
            UpdateCountdownText();

            if (currentCountdown <= 0f && !isKicking)
            {
                KickPlayer();
            }
        }
    }

    private void UpdateCountdownText()
    {
        int seconds = Mathf.FloorToInt(currentCountdown);
        countdownText.text = seconds.ToString();
    }

    private void KickPlayer()
    {
        isKicking = true;

        AotList playerList = Variables.Application.Get<AotList>("playerList");
        playerList.Remove(player.name);
        Variables.Application.Set("playerList", playerList);

        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("1");
    }
}