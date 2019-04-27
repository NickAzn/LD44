using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance;

    public Player player;
    public TextMeshProUGUI money;
    public TextMeshProUGUI highscoreMoney;
    public Transform grid;
    public GameObject endPanel;
    public TextMeshProUGUI endHighscore;
    public GameObject startPanel;
    public Toggle musicToggle;
    public Toggle sfxToggle;

    float speedMult = 1;
    float highscore = 0;

    public float baseItemSpeed;
    public float moneySpawnTime;
    public float wallSpawnTime;
    float curMoneyTime;
    float curWallTime;
    public GameObject[] moneyPrefabs;
    public GameObject[] wallPrefabs;
    public Tile groundTile;
    public Tile topTile;
    public Tile bottomTile;
    public Tilemap background;
    List<Transform> mapItems = new List<Transform>();

    float gameTime = 1;
    public bool started = false;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            Time.timeScale = 0f;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        endPanel.SetActive(false);
        startPanel.SetActive(true);
        highscoreMoney.text = string.Format("Largest Cash Size: ${0:#.00}", PlayerPrefs.GetFloat("HIGHSCORE", 0));
        musicToggle.isOn = SoundManager.Instance.GetMusicToggle();
        sfxToggle.isOn = SoundManager.Instance.GetSfxToggle();
    }

    public float GetSpeedMult() {
        return speedMult;
    }

    private void Update() {

        if (player == null || !started) {
            return;
        }

        gameTime += Time.deltaTime;
        speedMult = Mathf.Log(gameTime, 7f) + 1;

        float curMoney = player.GetMoney();
        if (curMoney > highscore) {
            highscore = curMoney;
        }
        money.text = string.Format("${0:#.00}", player.GetMoney());
        highscoreMoney.text = string.Format("Largest Cash Size: ${0:#.00}", highscore);

        curMoneyTime += Time.deltaTime;
        if (curMoneyTime >= moneySpawnTime / speedMult) {
            curMoneyTime -= moneySpawnTime / speedMult;
            GameObject moneyObj = Instantiate(moneyPrefabs[Random.Range(0, moneyPrefabs.Length)]);
            mapItems.Add(moneyObj.transform);
            moneyObj.transform.position = new Vector3(15, Random.Range(-4, 5), 0);
        }

        curWallTime += Time.deltaTime;
        if (curWallTime >= wallSpawnTime / speedMult) {
            curWallTime -= wallSpawnTime / speedMult;
            GameObject wallObj = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]);
            mapItems.Add(wallObj.transform);
            wallObj.transform.position = new Vector3(15, Random.Range(-4, 5), 0);
        }

        grid.transform.position = new Vector3(grid.transform.position.x - baseItemSpeed * Time.deltaTime * speedMult, 0, 0);
        GenerateNewBackground(11 - (int)grid.transform.position.x);

        foreach(Transform item in mapItems) {
            item.position = new Vector3(item.position.x - baseItemSpeed * Time.deltaTime * speedMult, item.position.y, 0);
        }
    }

    void GenerateNewBackground(int xPos) {
        for (int i = -6; i < 6; i++) {
            if (i == -6) {
                background.SetTile(new Vector3Int(xPos, i, 0), bottomTile);
            } else if (i == 5) {
                background.SetTile(new Vector3Int(xPos, i, 0), topTile);
            } else {
                background.SetTile(new Vector3Int(xPos, i, 0), groundTile);
            } 
        }
    }

    public void RemoveItem(Transform item) {
        mapItems.Remove(item);
    }

    public void StartGame() {
        started = true;
        startPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void EndGame() {
        float overallHighscore = PlayerPrefs.GetFloat("HIGHSCORE", 0);
        if (highscore > overallHighscore) {
            PlayerPrefs.SetFloat("HIGHSCORE", highscore);
            overallHighscore = highscore;
        }
        endHighscore.text = string.Format("Highscore: ${0:#.00}", overallHighscore);
        endPanel.SetActive(true);
    }

    public void RestartGame() {
        SceneManager.LoadScene(0);
    }

    public void ToggleMusic(Toggle tgl) {
        SoundManager.Instance.ToggleMusic(tgl.isOn);
    }

    public void ToggleSfx(Toggle tgl) {
        SoundManager.Instance.ToggleSfx(tgl.isOn);
    }
}
