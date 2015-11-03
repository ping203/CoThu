using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProfilePlayer : MonoBehaviour {
    public UISprite avatarRender;
    public List<GuiBall> guiBalls = new List<GuiBall> ();
    public List<GuiCard> guiCards = new List<GuiCard> ();
    public GameManager gameManager;
    public UILabel playerName;
    public UILabel playerNameAvata;
    public UILabel winner;
    public UILabel coins;

    public TextureSlider timeSlider;
    public bool isMain;

    public Toast infoBall;

    int countBall;
    int countCard;

    public void OnAwakeGameManager () {
        timeSlider.Resset ();

        winner.gameObject.SetActive (false);

        for(int i = 0; i < guiBalls.Count; i++) {
            guiBalls[i].gameObject.SetActive (false);
        }

        for(int i = 0; i < guiCards.Count; i++) {
            guiCards[i].gameObject.SetActive (false);
        }

        countBall = guiBalls.Count;
        countCard = guiCards.Count;

        SetActive (isMain ? ServerController.serverController.isMyQueue : !ServerController.serverController.isMyQueue);
        ServerController.serverController.OnChangeQueueEvent += ChangeBack;
    }

    void ChangeBack (bool myTurn) {
        SetActive (isMain ? myTurn : !myTurn);
    }
    void OnDestroy () {
        if(ServerController.serverController) {
            ServerController.serverController.OnChangeQueueEvent -= ChangeBack;
        }
    }
    public void SetActive (bool value) {
        //back.sprite = value ? activeBack : inactiveBack;
    }
    public void AddGuiBall (int id, int tt) {
        guiBalls[tt].id = id;
        guiBalls[tt].gameObject.SetActive (true);
        guiBalls[tt].GetComponent<UISprite> ().spriteName = "bito" + id;
    }

    public void AddGuiCard (int id, int tt) {
        guiCards[tt].gameObject.SetActive (true);
        guiCards[tt].setId (id);
    }

    public void RemoveGuiBall (int id) {
        for(int i = 0; i < guiBalls.Count; i++) {
            if(guiBalls[i].id == id) {
                guiBalls[i].gameObject.SetActive (false);
                infoBall.setText ("Bi vào lỗ: " + id);
                countBall--;
                break;
            }
        }
        if(countBall == 0 && (isMain ? !gameManager.remainedBlackBall : !gameManager.otherRemainedBlackBall)) {
            guiBalls[0].gameObject.SetActive (true);
            guiBalls[0].GetComponent<UISprite> ().spriteName = "bito8";
            guiBalls[0].id = 8;
            if(isMain) {
                gameManager.remainedBlackBall = true;
            } else {
                gameManager.otherRemainedBlackBall = true;
            }
        }
    }

    public void RemoveGuiCard (int id) {
        for(int i = 0; i < guiCards.Count; i++) {
            for(int j = 0; j < 4; j++) {
                if(guiCards[i].getId () == ((id - 1) + 13 * j)) {
                    guiCards[i].gameObject.SetActive (false);
                    countCard--;
                }
            }
        }
        if(countCard == 0){
            Debug.Log ("Bạn đã chiến thắng! " + playerName.text);
        }
    }

    public GuiBall FaindGuiBallById (int id) {
        GuiBall guiBall = null;
        foreach(GuiBall item in guiBalls) {
            if(item.id == id) {
                guiBall = item;
                return guiBall;
            }
        }
        return guiBall;
    }
    public IEnumerator SetAvatar (Sprite avatar) {
        yield return null;
    }

    public static void SetUserDate (string key, int value) {
        PlayerPrefs.SetInt (key, value);
    }
    public static int GetUserDate (string key) {
        return PlayerPrefs.GetInt (key);
    }

    public void setWinner () {
        winner.gameObject.SetActive (true);
        winner.text = "Winner!";
    }

    void Update () {

    }
}
