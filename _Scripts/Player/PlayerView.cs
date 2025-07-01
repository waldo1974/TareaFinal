using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerView : MonoBehaviour
{
    
    private PlayerPresenter playerPresenter;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private AudioSource coinSound;

    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private GameObject gameOverScreen;

    private void Awake()
    {
        playerPresenter = GetComponent<PlayerPresenter>();
        gameOverScreen.SetActive(false); //aseguramos que no este activa la screen de game over

        if (lifeText != null)
            lifeText.text = "Life: 100"; // Valor inicial de la vida
    }
    private void OnEnable()
    {
        if (playerPresenter != null)
        {
            playerPresenter.OnCoinsCollected += HandleCoinsCollected;
            playerPresenter.OnLifeChanged += HandleLifeChanged;

            playerPresenter.OnPlayerMoving += HandlePlayerMoving;
            playerPresenter.OnGameOver += GameOver;
        }


    }

    private void OnDisable()
    {
        if (playerPresenter != null)
        {
            playerPresenter.OnCoinsCollected -= HandleCoinsCollected;
            playerPresenter.OnLifeChanged -= HandleLifeChanged;

            playerPresenter.OnPlayerMoving -= HandlePlayerMoving;
            playerPresenter.OnGameOver -= GameOver;
        }

    }
    private void HandlePlayerMoving(bool value)
    {
        if (value)
        {
            _particleSystem.Play();
        }
        else
        {
            _particleSystem.Stop();
        }

    }
    private void HandleCoinsCollected(int newCoinCount)
    {

        // Reproduce sonido si está asignado
        if (coinSound != null)
            coinSound.Play();

        // Actualiza el texto con la nueva cantidad de monedas
        if (coinsText != null)
            coinsText.text = "Coins: " + newCoinCount;

    }

    private void HandleLifeChanged(int newCurrentLife)
    {
        if (lifeText != null)
        {
            if (newCurrentLife < 0)
            {
                newCurrentLife = 0;
            }
            lifeText.text = "Life: " + newCurrentLife;
        }
    }



    private void GameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }
}

