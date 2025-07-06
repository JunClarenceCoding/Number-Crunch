using UnityEngine;
using Photon.Pun;

public class Boss2 : MonoBehaviourPunCallbacks, IPunObservable
{
    public int MaxHealth = 1000;
    public int CurrentHealth;
    private Boss2BattleHandler boss2BattleHandler;

    void Start()
    {
        boss2BattleHandler = FindObjectOfType<Boss2BattleHandler>();
        if (boss2BattleHandler == null)
        {
            Debug.LogError("boss2BattleHandler not found in the scene.");
            return;
        }

        if (photonView.IsMine)
        {
            CurrentHealth = MaxHealth;
            photonView.RPC("UpdateHealth", RpcTarget.AllBuffered, CurrentHealth); 
            Debug.Log("Start: Initial Health Set. CurrentHealth = " + CurrentHealth);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CurrentHealth);
            Debug.Log("OnPhotonSerializeView: Sending CurrentHealth = " + CurrentHealth);
        }
        else
        {
            CurrentHealth = (int)stream.ReceiveNext();
            Debug.Log("OnPhotonSerializeView: Received CurrentHealth = " + CurrentHealth);
            Boss2BattleHandler.Instance.UpdateBossHealthUI(CurrentHealth, MaxHealth);
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage: Damage received = " + damage);
        
        // Apply damage to the monster's health
        CurrentHealth -= damage;
        Debug.Log("TakeDamage: New Health = " + CurrentHealth);

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Debug.Log("TakeDamage: Boss2 has been defeated!");
            GetComponent<Animator>().SetBool("isDead", true);
            boss2BattleHandler.CallVictory();
        }

        // Send updated health to all clients
        photonView.RPC("UpdateHealth", RpcTarget.AllBuffered, CurrentHealth);
        Debug.Log("TakeDamage: Updated health broadcasted to all clients. CurrentHealth = " + CurrentHealth);
    }

    [PunRPC]
    public void UpdateHealth(int newHealth)
    {
        Debug.Log("UpdateHealth: New Health Received = " + newHealth);
        CurrentHealth = newHealth;
        Boss2BattleHandler.Instance.UpdateBossHealthUI(CurrentHealth, MaxHealth);
        Debug.Log("UpdateHealth: UI Updated. CurrentHealth = " + CurrentHealth);
    }
}
