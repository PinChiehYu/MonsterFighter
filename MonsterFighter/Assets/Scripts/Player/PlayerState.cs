using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

    [SerializeField]
    private int baseHealthPoint;
    [SerializeField]
    private int baseManaPoint;
    [SerializeField]
    private int baseEnergyPoint;

    private int currentHealthPoint;
    public float CurrentHPRatio { get { return currentHealthPoint / (float)baseHealthPoint; } }
    private int currentManaPoint;
    public float CurrentMPRatio { get { return currentManaPoint / (float)baseManaPoint; } }
    private int currentEnergyPoint;
    public int CurrentEPRatio { get { return (int)Mathf.Floor(currentEnergyPoint / (float)baseEnergyPoint); } }

    private bool inAir;

    void Awake()
    {
        ResetState();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetState()
    {
        currentHealthPoint = baseHealthPoint;
        currentManaPoint = 0;
        currentEnergyPoint = 0;
    }

    private void ReduceHP(int hp)
    {
        currentHealthPoint -= hp;
    }

    private void ReduceMP(int mp)
    {
        currentManaPoint -= mp;
    }

    private void ReduceEP(int energy)
    {
        currentEnergyPoint -= energy;
    }
}
