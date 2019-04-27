using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour {

    public float moneyValue;
    public GameObject particles;
    public bool randomRot = true;

    private void Start() {
        if (randomRot) {
            transform.Rotate(0, 0, Random.Range(-180, 180));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals("Player")) {
            collision.GetComponent<Player>().ChangeMoney(moneyValue);
            if (particles != null) {
                GameObject prt = Instantiate(particles, collision.transform);
                prt.transform.localPosition = new Vector3(0, 0, 0);
            }
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        LevelManager.Instance.RemoveItem(transform);
    }

}
