using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmotionControl : MonoBehaviour {
    int id = 2;
    public UI2DSpriteAnimation animation;
    Sprite[] sprites;

    public void setId (int idNew) {
        id = idNew;
        sprites = null;
        if(id > 10)
            sprites = Resources.LoadAll<Sprite> ("Emotions/emotion_" + id);
        else
            sprites = Resources.LoadAll<Sprite> ("Emotions/emotion_0" + id);
        animation.frames = sprites;
        animation.framesPerSecond = sprites.Length;
    }

    public int getId () {
        return id;
    }

    // Use this for initialization
    void Start () {
        //setId (26);
    }

    // Update is called once per frame
    void Update () {

    }
}
