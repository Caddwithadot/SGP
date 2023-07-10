using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _roomNameText;
    private TMP_Text RoomNameText
    {
        get { return _roomNameText; }
    }

    [SerializeField]
    private TMP_Text _roomPlayerCount;
    private TMP_Text RoomCountText
    {
        get { return _roomPlayerCount; }
    }

    public string RoomName { get; private set; }
    public string RoomCount { get; private set; }

    public bool Updated { get; set; }

    private void Start()
    {
        GameObject lobbyCanvasObj = MainCanvasManager.Instance.LobbyCanvas.gameObject;
        if (lobbyCanvasObj == null)
        {
            return;
        }

        LobbyCanvas lobbyCanvas = lobbyCanvasObj.GetComponent<LobbyCanvas>();

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => lobbyCanvas.OnClickJoinRoom(RoomNameText.text));
    }

    private void OnDestroy()
    {
        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
    }

    public void SetRoomNameText(string text)
    {
        RoomName = text;
        RoomNameText.text = RoomName;
    }

    public void SetRoomCountText(int text)
    {
        RoomCount = text.ToString();
        RoomCountText.text = RoomCount;
    }
}
