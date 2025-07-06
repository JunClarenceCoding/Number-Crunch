using UnityEngine;
using Photon.Pun;
public class Boss1 : MonoBehaviourPunCallbacks, IPunObservable
{
    public int MaxHealth = 1500;
    public int CurrentHealth;
    private Boss1BattleHandler boss1BattleHandler;
    void Start()
    {
        boss1BattleHandler = FindObjectOfType<Boss1BattleHandler>();
        if (boss1BattleHandler == null)
        {
            Debug.LogError("boss1BattleHandler not found in the scene.");
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
            Boss1BattleHandler.Instance.UpdateBossHealthUI(CurrentHealth, MaxHealth);
        }
    }
    [PunRPC]
    public void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage: Damage received = " + damage);
        CurrentHealth -= damage;
        Debug.Log("TakeDamage: New Health = " + CurrentHealth);
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Debug.Log("TakeDamage: Boss1 has been defeated!");
            GetComponent<Animator>().SetBool("isDead", true);
            boss1BattleHandler.CallVictory();
        }
        else{
            GetComponent<Animator>().SetTrigger("isHit");
        }
        photonView.RPC("UpdateHealth", RpcTarget.AllBuffered, CurrentHealth);
        Debug.Log("TakeDamage: Updated health broadcasted to all clients. CurrentHealth = " + CurrentHealth);
    }

    [PunRPC]
    public void UpdateHealth(int newHealth)
    {
        Debug.Log("UpdateHealth: New Health Received = " + newHealth);
        CurrentHealth = newHealth;
        Boss1BattleHandler.Instance.UpdateBossHealthUI(CurrentHealth, MaxHealth);
        Debug.Log("UpdateHealth: UI Updated. CurrentHealth = " + CurrentHealth);
    }
}