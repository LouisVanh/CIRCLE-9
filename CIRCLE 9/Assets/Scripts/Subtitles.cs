using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Subtitles : MonoBehaviour
{
    public TextMeshProUGUI subtitleText;
    public float displayTime = 3f;

    private Queue<string> subtitlesQueue = new Queue<string>();

    private void Start()
    {
        // Add your provided text as subtitles
        subtitlesQueue.Enqueue("Rise, lost soul. You've been slumbering");
        subtitlesQueue.Enqueue("Blissfully ignorant. But now you are here");
        subtitlesQueue.Enqueue("Where shadows devour all that remains.");
        subtitlesQueue.Enqueue("You've come far from the world you once knew.");
        subtitlesQueue.Enqueue("Your journey has brought you to the darkest depths");
        subtitlesQueue.Enqueue("Where your past sins have earned you a place among");
        subtitlesQueue.Enqueue("the most wretched of souls. Prepare yourself,");
        subtitlesQueue.Enqueue("for the ninth circle is ravenous, and its hunger knows no end.");
        subtitlesQueue.Enqueue("Redemption is but a distant dream in this");
        subtitlesQueue.Enqueue("malevolent void. This is the final destination");
        subtitlesQueue.Enqueue("for those who committed treacherous deeds,");
        subtitlesQueue.Enqueue("betraying the trust of those closest to them. Here, they are denied the warmth");
        subtitlesQueue.Enqueue("of the living world forever buried in the ice,");
        subtitlesQueue.Enqueue("to bear witness to their own wickedness.");
        subtitlesQueue.Enqueue("There are ofcourse your fellow travelers, the forsaken, the forsakers.");
        subtitlesQueue.Enqueue("Their agony is your prelude,");
        subtitlesQueue.Enqueue("condemned to eternity in the icy tomb.");
        subtitlesQueue.Enqueue("The frozen lake, where the damned linger in perpetual torment,");
        subtitlesQueue.Enqueue("cursed to feed on human brains for all eternity.");
        subtitlesQueue.Enqueue("Perhaps, lost one,");
        subtitlesQueue.Enqueue("you'll find your way through the shadows.");

        StartCoroutine(DisplaySubtitles());
    }

    private IEnumerator DisplaySubtitles()
    {
        while (subtitlesQueue.Count > 0)
        {
            string subtitle = subtitlesQueue.Dequeue();
            subtitleText.text = subtitle;
            yield return new WaitForSeconds(displayTime);
            subtitleText.text = ""; // Clear the text
            yield return new WaitForSeconds(1f); // Add a brief pause between subtitles
        }
    }
}
