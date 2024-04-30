using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    public Image currentHealthBar;
    public TextMeshProUGUI scoreText;
    public GameObject pauseWindow;
    public Image damageFrame;
    public float damageTime;

    public static HUDController instance;

    private Coroutine disappearCoroutine;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Update Healt Bar 
    /// </summary>
    /// <param name="currentHealth">current Health</param>
    /// <param name="maxHealth">Max Health</param>
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        //Fill the current Healt Bar with the percentaje of Health 
        currentHealthBar.fillAmount = (float)currentHealth / (float)maxHealth;
    }


    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString("00000");
    }

    public void ChangeStatePauseWindow(bool paused)
    {
        pauseWindow.SetActive(paused);
    }

    /// <summary>
    /// Turn on the Damage Frame when the player is shooted
    /// </summary>
    public void ShowDamageIndicator()
    {
        //If there is the coroutine running stop it
        if (disappearCoroutine != null)
            StopCoroutine(disappearCoroutine);

        //enable de image component 
        damageFrame.enabled = true;
        //restart the image color 
        damageFrame.color = Color.white;
        //Start the fade out coroutine
        disappearCoroutine = StartCoroutine(DamageDisappear());
    }

    IEnumerator DamageDisappear()
    {
        //Alpha color reset to 1 
        float alpha = 1.0f;

        //While  alpha color is greater than 0
        while (alpha > 0.0f)
        {
            alpha -= (1.0f / damageTime) * Time.deltaTime;
            damageFrame.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            yield return null;
        }
        
        damageFrame.enabled = false;
    }
}
