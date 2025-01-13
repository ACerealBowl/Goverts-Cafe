using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pastry1 : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject cake;
    [SerializeField] private Sprite[] pastrySprites; 
    private GameObject aLie;
    private bool isFollowingMouse = false;

    private void OnMouseDown()
    {
        if (gameObject.activeSelf)
        {
            int randomNumber = Random.Range(1, 5); // it be 5 but its actually 4 lmao

            // Change sprite based on random number
            if (spriteRenderer != null && pastrySprites != null && pastrySprites.Length >= 4)
            {
                spriteRenderer.sprite = pastrySprites[randomNumber - 1];
            }

            // Spawn cake 
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;  //  cake spawns at correct Z position bcs of unity gay

            aLie = Instantiate(cake, mousePosition, Quaternion.identity);
            isFollowingMouse = true;
        }
    }

    private void Update()
    {
        if (isFollowingMouse && aLie != null)
        {
            // Update cake position to follow mouse
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            aLie.transform.position = mousePosition;
        }
    }
}