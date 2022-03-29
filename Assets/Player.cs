using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed;
    public float rotateAmount;
    float rot;
    int score;
    int y = 0; // muuttuja buildindexin arvon tallettamiseksi

    //viimeisellä tasolla pelin loppuessa tuhotaan kaikki Danger -peliobjektit, jotta Toukka voi jatkaa kiertelyään
    // törmäämättä niihin, kun käyttäjä miettii, pelaisinko uudelleen
    // tämän vuoksi tarvitaan seuraavat muuttujat

    GameObject[] dangers;

    // luodaan GameManager -tyyppinen muuttuja gameManager
    public GameManager gameManager;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        dangers = GameObject.FindGameObjectsWithTag("Danger");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.x < 0)
            {
                rot = rotateAmount;
            }
            else
            {
                rot = -rotateAmount;
            }

            transform.Rotate(0, 0, rot);

        }



    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //haetaan ladatun scenen buildindex ja tallennetaan se muuttujaan y
        y = SceneManager.GetActiveScene().buildIndex;


        if (collision.gameObject.tag == "Food")
        {
            Destroy(collision.gameObject);
            score++;

            if (score >= 5)
            {
                //print("Taso meni läpi");
                // tarkistetaan, ollaanko jo viimeisellä tasolla, jonka buildindex on 3
                // jos ei olla, ladataan seuraava taso

                if (y != 3)
                {
                    score = 0;
                    y++;
                    SceneManager.LoadScene(y);
                }

                // kun ollaan viimeisellä tasolla, jonka buildindex on 3, niin ladataan buttonit -play again ja quit
                if (y >= 3)
                {
                    score = 0;

                    // tässä tuhotaan kaikki Danger -peliobjektit, jotta Toukka voi jatkaa kiertelyään
                    // törmäämättä niihin, kun käyttäjä miettii, pelaisinko uudelleen
                    foreach (GameObject danger in dangers)
                    {
                        Destroy(danger);
                    }

                    // kutsutaan EndGame -funktiota, joka näyttää lopetusbuttonit näytöllä
                    y = 0;
                    gameManager.EndGame();
                }



            }
        }

        // jos törmätään Danger- peliobjektiin, aloitetaan taso alusta
        if (collision.gameObject.tag == "Danger")
        {
            SceneManager.LoadScene("Game");
        }
    }

}
