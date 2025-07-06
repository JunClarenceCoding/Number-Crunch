using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCTriggerBox : MonoBehaviour
{
    public GameObject NPC;
    public GameObject ConvoPanel;
    public ArrowTriggerBox arrowNPC;
    public NPCDialogue npcDialogue;
    private bool hasTriggered = false;
    private LoadCharacter loadCharacter; 
    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
            return;
        }
        gameObject.SetActive(false); 
        if (loadCharacter == null)
        {
            loadCharacter = FindObjectOfType<LoadCharacter>();
        }
    }
    public void EnableTriggerBox()
    {
        if (!hasTriggered)
        {
            gameObject.SetActive(true);
            hasTriggered = true; 
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (!ConvoPanel.activeInHierarchy)
            {
                arrowNPC.playerArrow.gameObject.SetActive(false);
                npcDialogue.ConvoStart();
                NavMeshAgent playerAgent = col.GetComponent<NavMeshAgent>();
                if (playerAgent != null)
                {
                    playerAgent.isStopped = true;
                    playerAgent.velocity = Vector3.zero;
                }
                Animator playerAnimator = col.GetComponent<Animator>();
                if (playerAnimator != null)
                {
                    playerAnimator.SetBool("isRun", false);
                    audioManager.stopRun();
                }
            }
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            NPC.transform.LookAt(col.transform);
            GameObject instantiatedPlayer = loadCharacter.GetInstantiatedPlayer();
            if (instantiatedPlayer != null)
            {
                instantiatedPlayer.transform.LookAt(NPC.transform);
            }
        }
    }
    public void DeactivateTriggerBox()
    {
        gameObject.SetActive(false);
    }
}