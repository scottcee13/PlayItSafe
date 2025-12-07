using UnityEngine;

public class BadPointsTracker : MonoBehaviour
{
    public static BadPointsTracker Instance;

    public int totalBadPoints = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddBadPoints(int amount)
    {
        totalBadPoints += amount;
        Debug.Log("Bad Points: " + totalBadPoints);
    }
}
