﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance;

    public Player player;
    public TextMeshProUGUI money;
    public TextMeshProUGUI highscoreMoney;
    public Transform grid;

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

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            Time.timeScale = 0f;
        } else {
            Destroy(gameObject);
        }
    }

    public float GetSpeedMult() {
        return speedMult;
    }

    private void Update() {

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
        Time.timeScale = 1f;
    }

    public void EndGame() {
        Time.timeScale = 0f;
    }

    public void RestartGame() {
        SceneManager.LoadScene(0);
    }
}
