using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SecondTriggerBox : MonoBehaviour
{
    public GameObject NPC;
    public GameObject ConvoPanel;
    public ArrowTriggerBox ArrowToNpc;
    public SecondDialogueScript dialogueScript; 
    public LoadCharacter loadCharacter; 
    private bool hasTriggered = false;
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

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            ArrowToNpc.playerArrow.gameObject.SetActive(false);

            if (!ConvoPanel.activeInHierarchy)
            {
                dialogueScript.ConvoStart();
            }

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
}