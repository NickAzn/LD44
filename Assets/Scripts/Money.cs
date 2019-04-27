using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour {

    public float moneyValue;
    public GameObject particles;
    public bool randomRot = true;
    public AudioClip soundEffect;

    private void Start() {
        if (randomRot) {
            transform.Rotate(0, 0, Random.Range(-180, 180));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals("Player")) {
            collision.GetComponent<Player>().ChangeMoney(moneyValue);
            if (particles != null) {
                GameObject prt = Instantiate(particles);
                prt.transform.localPosition = collision.transform.position;
                if (moneyValue > 0) {
                    prt.transform.SetParent(collision.transform);
                    prt.transform.localScale = Vector3.one;
                }
            }
            if (soundEffect != null) {
                SoundManager.Instance.PlaySfx(soundEffect);
            }
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        LevelManager.Instance.RemoveItem(transform);
    }

}
