using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int rotateSpeed;
    public float speed;

    public float drainTime;
    float curDrainTime = 0;
    public float drainMult;

    public ParticleSystem[] trailParticles;

    private Rigidbody2D rb;

    float money = 1250;

    public float GetMoney() {
        return money;
    }

    public void ChangeMoney(float amount) {
        money += amount;
        if (money <= 0f) {
            LevelManager.Instance.EndGame();
            foreach (Transform child in transform) {
                child.SetParent(null);
            }
            foreach (ParticleSystem particles in trailParticles) {
                Destroy(particles.gameObject);
            }
            Destroy(gameObject);
        }
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime * LevelManager.Instance.GetSpeedMult());
        float size = money / 2500f;
        if (size > 1.25f) {
            size = 1.25f;
        } else if (size < 0.2) {
            size = 0.2f;
        }
        transform.localScale = new Vector3(size, size, 1);

        float yDir = Input.GetAxis("Vertical");
        if (!LevelManager.Instance.started && Input.GetAxisRaw("Vertical") != 0) {
            LevelManager.Instance.StartGame();
        }
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y + yDir * Time.deltaTime * speed, transform.position.z);
        rb.MovePosition(newPos);

        foreach (ParticleSystem part in trailParticles) {
            part.transform.position = transform.position;
            part.transform.localScale = new Vector3(size, size, size);
        }


        curDrainTime += Time.deltaTime;
        if (curDrainTime >= drainTime) {
            curDrainTime -= drainTime;
            money *= drainMult;
        }
    }
}
