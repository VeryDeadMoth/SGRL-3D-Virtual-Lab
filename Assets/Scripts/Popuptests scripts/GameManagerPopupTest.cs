using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*public class GameManagerPopupTest : MonoBehaviour
{

    public delegate void PopupEvents();
    public static event PopupEvents OnPopupOpensEvent;

    public int maxMessages = 10;

    public GameObject chatPanel, panel;

    public float maxTimeBeforeDeletion = 10f;

    [SerializeField]
    List<Message1> messageList = new List<Message1>();


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SendMessageHere("this is a very long text a bip bop wow aaaaaaaaaaaaaaaa aaaaaaaajdjdjbjzfjzfzjfeej mrrrrrrrrrrrrrrrheghvzejvghgvhzehvggrhvgej wowoowowowoowowowow huehrueh hsdhzuhehjhdhdh aaaaaaaaaaaaaaaaaaaa");
            DeleteMessage();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            SendMessageHere("shimp");
        }
    }

    //(try object pooling later ?)

    public void SendMessageHere(string text)
    {
        if (messageList.Count < maxMessages)
        {
            Message1 newMessage = new Message1();
            newMessage.text = text;

            GameObject newText = Instantiate(panel, chatPanel.transform);

            newMessage.panelInMessage = newText.gameObject;

            newMessage.panelInMessage.GetComponentInChildren<TMP_Text>().text = newMessage.text;

            messageList.Add(newMessage);

            //animation

            newText.GetComponent<CanvasGroup>().alpha = 0;
            newText.GetComponent<CanvasGroup>().LeanAlpha(1, 0.5f);
            Debug.Log(OnPopupOpensEvent);
            StartCoroutine("DeleteMessage");
            //code can pursue after coroutine called
            if (OnPopupOpensEvent != null)
            {
                OnPopupOpensEvent();
            }
        }


    }

    public IEnumerator DeleteMessage()
    {
        yield return new WaitForSeconds(maxTimeBeforeDeletion);
        //after (set) seconds, destroy message
        messageList[0].panelInMessage.GetComponent<CanvasGroup>().LeanAlpha(0, 0.2f);
        Destroy(messageList[0].panelInMessage.gameObject, 0.2f);
        messageList.Remove(messageList[0]);

    }

}

[System.Serializable]
public class Message1
{
    public string text;

    //TMP_Text ?
    //public TMP_Text textObject;

    public GameObject panelInMessage;*/