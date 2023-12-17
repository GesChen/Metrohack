using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    bool playing;

    public TextMeshProUGUI scoreText;
    public int score;
    public AudioSource collectedSound;
    public AudioSource scoreGood;
    public AudioSource scoreBad;
    [Space]
    public float time;
    public TextMeshProUGUI minutes;
    public TextMeshProUGUI seconds;
    public AudioSource beep;
    public AudioSource endBeep;

    float beepThreshold = 5;

    [Header("End Cutscene")]
    public Transform scoreObj;
    public Transform alarmClock;
    public Transform inventory;
    public float transitionTime;
    public float amountToMoveUp;
    public TextMeshProUGUI finalScore;
    public Transform resultsScreen;
    public Transform resultsTargetPos;

    [Header("End Post Processing")]
    public Volume PP;
    public float endPPsmoothness;
    public float targetVignette;
    public float targetFocusDist;
    

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<DistributeTrash>().Distribute();
        playing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            time -= Time.deltaTime;
            minutes.text = " " + Mathf.FloorToInt(time / 60).ToString();
            seconds.text = Mathf.FloorToInt(time % 60).ToString("D2");
        }

        if (time < beepThreshold)
        {
            beep.Play();
            beepThreshold--;
        }
        if (time < 0 && playing)
        {
            StartCoroutine(End());
            StartCoroutine(EndPP());
        }
    }
    IEnumerator EndPP()
    {
        Vignette vignette;
        DepthOfField dof;
        PP.profile.TryGet(out vignette);
        PP.profile.TryGet(out dof);

        while (Mathf.Abs(dof.focusDistance.value - targetFocusDist) > .01f && Mathf.Abs(vignette.intensity.value - targetVignette) > .01f)
        {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetVignette, endPPsmoothness);
            dof.focusDistance.value = Mathf.Lerp(dof.focusDistance.value, targetFocusDist, endPPsmoothness);

            yield return null;
        }

    }
    IEnumerator End()
    {
		playing = false;
		endBeep.Play();
		minutes.text = " 0";
		seconds.text = "00";
        finalScore.text = score.ToString();

        FindObjectOfType<PlayerController>().enabled = false;
		FindObjectOfType<PlayerController>().steps.volume = 0;
		FindObjectOfType<CameraController>().enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

		// move the objects out of the way and ease in the sign
		float scoreStarty = scoreObj.position.y;
        float alarmStarty = alarmClock.position.y;
        float inventoryStarty = 0;
        if(inventory) inventoryStarty = inventory.position.y;
        float resultStarty = resultsScreen.position.y;

        float elapsed = 0f;
        while (elapsed < transitionTime)
        {
            float t = elapsed / transitionTime;
            scoreObj.position = new(scoreObj.position.x, EaseInBack(t, scoreStarty, scoreStarty + amountToMoveUp));
            alarmClock.position = new(alarmClock.position.x, EaseInBack(t, alarmStarty, alarmStarty + amountToMoveUp));
            resultsScreen.position = new(resultsScreen.position.x, EaseOutQuart(resultStarty, resultsTargetPos.position.y, t));
            if (inventory) inventory.position = new(inventory.position.x, EaseInBack(t, inventoryStarty, inventoryStarty - amountToMoveUp * 3));

			yield return null;
            elapsed += Time.deltaTime;
		}
    }
	float EaseInBack(float t, float start, float end)
	{
		float s = 1.70158f; // overshooting amount

		t = t / 1f;
		return end * (t) * t * ((s + 1f) * t - s) + start;
	}
	float EaseOutElastic(float t, float start, float end)
	{
		const float c4 = (2 * Mathf.PI) / 3;
        return Mathf.Lerp(start, end, Mathf.Clamp01(Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1));
	}
    float EaseOutBack(float t, float start, float end)
    {
		const float c1 = 1.70158f;
		const float c3 = c1 + 1;

		return Mathf.Lerp(start, end, 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2));
	}
    float EaseOutQuart(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, 1 - Mathf.Pow(1 - t, 4));
	}
	public void Score(Trash obj, TrashType type)
    {
        if (Time.time > 2f)
            StartCoroutine(ScoreC(obj, type));
    }
    IEnumerator ScoreC(Trash obj, TrashType type)
    {
        collectedSound.Play();
        yield return new WaitForSeconds(.7f);

		if (obj.type == type)
		{
			scoreGood.Play();
            score++;
		}
		else
		{
			scoreBad.Play();
		}

        scoreText.text = score.ToString();
	}
}
