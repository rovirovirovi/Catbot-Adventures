using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text playerHPText;
    public RectTransform playerHPBarImage, playerHPBarBGImage;
    Player player;
    public float lerpDelta = 5f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        playerHPText.text = player.health.ToString();
        playerHPBarImage.sizeDelta = new Vector2(((float)player.health / 20f) * 1000f, 72f);
        playerHPBarBGImage.sizeDelta = Vector2.Lerp(playerHPBarBGImage.sizeDelta, playerHPBarImage.sizeDelta, lerpDelta * Time.deltaTime);
    }
}
