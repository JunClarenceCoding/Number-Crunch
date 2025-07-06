[System.Serializable]
public class RewardItem
{
    public string rewardType; 
    public int amount; 

    public RewardItem(string rewardType, int amount)
    {
        this.rewardType = rewardType;
        this.amount = amount;
    }
}