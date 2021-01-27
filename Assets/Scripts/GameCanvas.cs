using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameCanvas : MonoBehaviour
{
    //public GameController go;

    private string p1 = "Player ";
    private string p2 = " Skill ";
    private string p3 = " Button";
    private string health = " Health";
    [SerializeField]
    GameObject king1;
    [SerializeField]
    GameObject king2;

    void Start()
    {
        //GameObject[] kings = GameObject.FindGameObjectsWithTag("King");
        //GameObject king1 = kings.First(z => z.GetComponent<KingController>().playerNumber == 1);
        //GameObject king2 = kings.First(z => z.GetComponent<KingController>().playerNumber == 2);

        king1 = GameController.instance.king1;
        king2 = GameController.instance.king2;

        Sprite skill1king1 = king1.GetComponent<KingController>().kingType.firstSkill.skillIcon;
        Sprite skill2king1 = king1.GetComponent<KingController>().kingType.secondSkill.skillIcon;
        Sprite skill1king2 = king2.GetComponent<KingController>().kingType.firstSkill.skillIcon;
        Sprite skill2king2 = king2.GetComponent<KingController>().kingType.secondSkill.skillIcon;

        Transform temp = transform.Find(p1 + 1 + p2 + 1 + p3);
        temp.GetComponent<Image>().sprite = skill1king1;
        temp = transform.Find(p1 + 2 + p2 + 1 + p3);
        temp.GetComponent<Image>().sprite = skill1king2;
        temp = transform.Find(p1 + 1 + p2 + 2 + p3);
        temp.GetComponent<Image>().sprite = skill2king1;
        temp = transform.Find(p1 + 2 + p2 + 2 + p3);
        temp.GetComponent<Image>().sprite = skill2king2;
    }

    public void UpdateCanvasSkillText(short playerNum, short firstCooldown, short secondCooldown)
    {
        Transform temp = transform.Find(p1 + playerNum.ToString() + p2 + 1 + p3);
        if (temp == null)
            Debug.LogWarning("Cant find canvas! " + p1 + playerNum.ToString() + p2 + 1 + p3);
        if (firstCooldown == 0)
        {
            temp.GetComponent<Image>().color = Color.white;
        }
        else
        {
            Debug.Log("Lol");
            temp.GetComponent<Image>().color = Color.grey;
        }
        temp.GetChild(0).GetComponent<TextMeshProUGUI>().text = firstCooldown.ToString();
        temp = transform.Find(p1 + playerNum.ToString() + p2 + 2 + p3);
        if (secondCooldown == 0)
        {
            temp.GetComponent<Image>().color = Color.white;
        }
        else
        {
            Debug.Log("Lol");
            temp.GetComponent<Image>().color = Color.grey;
        }
        temp.GetChild(0).GetComponent<TextMeshProUGUI>().text = secondCooldown.ToString();
    }

    public void UpdateCanvasHpText(short playerNum, float curHp)
    {
        Transform temp = transform.Find(p1 + playerNum.ToString() + health);
        temp.Find("Health Text").GetComponent<TextMeshProUGUI>().text = curHp.ToString();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}