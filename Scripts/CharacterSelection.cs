using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
   public GameObject[] characters;
   public int selectedCharacter = 0;
   public GameObject loadingScreen;
   public Slider slider;
   public Animator FlowerAnim;
   private AudioManager audioManager;

   void Start()
   {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
            return;
        }
   }
   public void NextCharacter()
   {
       characters[selectedCharacter].SetActive(false);
       selectedCharacter = (selectedCharacter + 1) % characters.Length;
       characters[selectedCharacter].SetActive(true);
   }
   public void PreviousCharacter()
   {
       characters[selectedCharacter].SetActive(false);
       selectedCharacter--;
       if (selectedCharacter < 0)
       {
           selectedCharacter += characters.Length;
       }
       characters[selectedCharacter].SetActive(true);
   }
   public void StartGame()
   {
       // Save the selected character
       PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);

       // Activate the loading screen before the loading starts
       loadingScreen.SetActive(true);
       slider.value = 0; 
       FlowerAnim.Play("FlowersLoading");
       audioManager.StopMusic();

       // Start loading the scene asynchronously
       StartCoroutine(LoadAsynchronously("TutorialScene"));
   }
   IEnumerator LoadAsynchronously(string sceneName)
   {
       // Start loading the scene asynchronously
       AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
       operation.allowSceneActivation = false; 

       float targetProgress = 0f;

       // While the scene is loading, update the progress bar
       while (operation.progress < 0.9f)
       {
           targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

           while (slider.value < targetProgress)
           {
               slider.value = Mathf.MoveTowards(slider.value, targetProgress, Time.deltaTime * 0.5f); 
               yield return null; 
           }
           yield return null;
       }
       targetProgress = 1f;

       while (slider.value < targetProgress)
       {
           slider.value = Mathf.MoveTowards(slider.value, targetProgress, Time.deltaTime * 0.5f);
           yield return null;
       }
       // Add a short delay to keep the loading screen visible
       yield return new WaitForSeconds(1f);

       // Allow the scene to activate
       operation.allowSceneActivation = true;
   }
}