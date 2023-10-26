using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Ship
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Sprites Animation")]
    [SerializeField] private Sprite[] shipSprites;

    void Update()
    {
        Movement();
        ShotMissiles();
    }

    void Movement(){
        float x = Input.GetAxis("Horizontal");

        rigidBody2D.velocity = new Vector2(x * moveSpeedX, 0);

        float maxDistance = ScreenWidth() / 2 - maxDistanceOffSet;

        if(x > 0){
            ChangeShipSprite(1);
        }else if(x < 0){
            ChangeShipSprite(2);
        }else{
            ChangeShipSprite(0);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -maxDistance, maxDistance), transform.position.y, 0);
    }

    protected override void ShotMissiles(){
        if(Input.GetKey(KeyCode.Space) && shotDelay <= 0f){
            GameObject missileInstance = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            missileInstance.GetComponent<Rigidbody2D>().AddForce(Vector2.up * shotStrength, ForceMode2D.Impulse);
            Destroy(missileInstance, destroyMissileInstanceTimer);
            shotDelay = shotDelayTimer;
            
            SFXSource.PlayOneShot(shootingSFX);
        }
        else{
            shotDelay -= Time.deltaTime;
        }
    }

    void ChangeShipSprite(int index){
        spriteRenderer.sprite = shipSprites[index];
    }

    public void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "EnemyMissile"){
            healthManager.TakeDamage(1);
            Destroy(col.gameObject);
        }
    }
}