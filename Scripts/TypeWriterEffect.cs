using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class TypeWriterEffect : MonoBehaviour
{
    public float delay = 0.05f;
    private string fullText;
    private string currentText = "";
    private Action onTypingComplete;
    public void StartTypewriterEffect(string newText, Action onComplete = null)
    {
        StopAllCoroutines(); 
        fullText = newText;
        onTypingComplete = onComplete;
        StartCoroutine(ShowText());
    }
    IEnumerator ShowText()
    {
        currentText = "";
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            this.GetComponent<TMP_Text>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
        onTypingComplete?.Invoke();
    }
}