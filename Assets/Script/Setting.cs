using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting :SingletonMonoBehaviour<Setting>
{
    public int EnemyMode = 0;
    public int VisibleMode = 0;
    public int RuleMode = 0;

    public int Win;
    public int Lose;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

    }

    public void SetEnemyMode(int value)
    {
        EnemyMode = value;
    }
    
        public void SetVisibleMode(int value)
        {
            VisibleMode = value;
        }
    
        public void SetRuleMode(int value)
    {
        RuleMode = value;
    }
}
