using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NumPlayer : MonoBehaviour
{
    public UIAtlas m_Atlas;
    public List<UISprite> list_star;
    public string name_star_white;
    public string name_star_blue;
    // Use this for initialization
    void Start()
    {

    }
    public void init(int num, int max)
    {
        
        for (int i = 0; i < list_star.Count; i++)
        {
            list_star[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < max; i++)
        {
            list_star[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < num; i++)
        {
            UISprite sprite = list_star[i];
            sprite.atlas = m_Atlas;
            sprite.spriteName = name_star_blue;
            sprite.depth = 20;
            Color color = sprite.color;
            color.a = 1f;
            sprite.color = color;
            sprite.SetDimensions(34, 32);
            sprite.transform.localPosition = new Vector3(0 + i * sprite.localSize.x, 0, 0);
        }
        for (int i = num; i < max; i++)
        {
            UISprite sprite = list_star[i];
            sprite.atlas = m_Atlas;
            sprite.spriteName = name_star_white;
            sprite.depth = 20;
            Color color = sprite.color;
            color.a = 0.5f;
            sprite.color = color;
            sprite.SetDimensions(34, 32);
            sprite.transform.localPosition = new Vector3(0 + i * sprite.localSize.x, 0, 0);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
