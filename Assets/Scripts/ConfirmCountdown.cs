using UnityEngine;
using TMPro;
using Photon.Pun;

public class ConfirmCountdown : MonoBehaviour
{
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
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("1");
    }
}