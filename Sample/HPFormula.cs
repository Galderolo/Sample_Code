using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HPFormula : MonoBehaviour
{
    /// <summary>
    /// NEED TO REFACTOR
    /// </summary>
    
    private readonly double max_double = 9.985e307;
    [HideInInspector] public double hp;
    [HideInInspector] public double armor;
    [HideInInspector] public double shields;
    private int level, wave;
    private double bossHP;
    private float constant;

    private int checked_wave = 0;

    private int waveToHaveArmor;
    

    private WaveManager waves;
    private double escalable_base;

    private const string DUNGEON1 = "Dungeon1";
    private const string DUNGEON2 = "Dungeon2";
    private const string DUNGEON3 = "Dungeon3";
    private const string DUNGEON4 = "Dungeon4";
    private const string DUNGEON5 = "Dungeon5";
    private const string DUNGEON6 = "Dungeon6";
    private const string DUNGEON7 = "Dungeon7"; //GOLD DUNGEON
    private const string DUNGEON8 = "Dungeon8";
    private const string RAID1 = "Raid1";
    private const string RAID2 = "Raid2";
    private const string RAID3 = "Raid3";
    private const string RAID4 = "Raid4";
    private const string RAID5 = "Raid5";
    private const string RAID6 = "Raid6";
    private const string RAID7 = "Raid7"; //Raid Gold Chamber


    [Header("NORMAL WAVES")]
    [SerializeField] [Tooltip("Base Life")] private double nw_base;
    [SerializeField] [Tooltip("Increment Base Life for Bosses")] private float nw_base_inc;
    [SerializeField] [Tooltip("Constant")] private float nw_const;
    [SerializeField] [Tooltip("Constant Increment for Bosses")] private float nw_const_inc;

   

    [Header("BASES DUNGEON")]
    [SerializeField] private double d1_base;
    [SerializeField] private double d2_base;
    [SerializeField] private double d3_base;
    [SerializeField] private double d4_base;
    [SerializeField] private double d5_base;
    [SerializeField] private double d6_base;
    [SerializeField] private double d7_base;
    [SerializeField] private double d8_base;

    [Header("CONSTANTES DUNGEON")]
    [SerializeField] private float d1_const;
    [SerializeField] private float d2_const;
    [SerializeField] private float d3_const;
    [SerializeField] private float d4_const;
    [SerializeField] private float d5_const;
    [SerializeField] private float d6_const;
    [SerializeField] private float d7_const;
    [SerializeField] private float d8_const;

    [Header("ARMOR BASES")]
    [SerializeField] private double d1_armor_base;
    [SerializeField] private double d2_armor_base;
    [SerializeField] private double d3_armor_base;
    [SerializeField] private double d4_armor_base;
    [SerializeField] private double d5_armor_base;
    [SerializeField] private double d6_armor_base;
    [SerializeField] private double d7_armor_base;
    [SerializeField] private double d8_armor_base;

    [Header("ARMOR CONSTANTES")]
    [SerializeField] private float d1_armor_const;
    [SerializeField] private float d2_armor_const;
    [SerializeField] private float d3_armor_const;
    [SerializeField] private float d4_armor_const;
    [SerializeField] private float d5_armor_const;
    [SerializeField] private float d6_armor_const;
    [SerializeField] private float d7_armor_const;
    [SerializeField] private float d8_armor_const;

    [Header("SHIELDS BASES")]
    [SerializeField] private double d1_shield_base;
    [SerializeField] private double d2_shield_base;
    [SerializeField] private double d3_shield_base;
    [SerializeField] private double d4_shield_base;
    [SerializeField] private double d5_shield_base;
    [SerializeField] private double d6_shield_base;
    [SerializeField] private double d7_shield_base;
    [SerializeField] private double d8_shield_base;

    [Header("SHIELD CONSTANTES")]
    [SerializeField] private float d1_shield_const;
    [SerializeField] private float d2_shield_const;
    [SerializeField] private float d3_shield_const;
    [SerializeField] private float d4_shield_const;
    [SerializeField] private float d5_shield_const;
    [SerializeField] private float d6_shield_const;
    [SerializeField] private float d7_shield_const;
    [SerializeField] private float d8_shield_const;

    private int new_wave = 1;
    private bool enemies_have_armor;
    private bool enemies_have_shields;

    //DUNGEON 2
    //..


    #region Singleton
    public static HPFormula Instance;
    private void Awake()
    {
        Instance = this;


    }
    #endregion

    private void Start()
    {

        waves = WaveManager.Instance;

        SetEscalableBase();
        CheckWave();
    }

    private void CheckWave()
    {

        if (ES3.FileExists() && ES3.KeyExists("wave"))
            checked_wave = ES3.Load<int>("wave");
    }
    private void SetEscalableBase()
    {
        int stage = 200;
        if (ES3.FileExists() && ES3.KeyExists("wave"))
            new_wave = ES3.Load<int>("wave");
        if (new_wave <= 200) stage = 1;                                                                 
        escalable_base = Math.Round((new_wave - 1) + nw_base * Math.Pow(nw_const, new_wave - stage));
        if (new_wave >= 10000) enemies_have_armor = true;
        if (new_wave >= 20000) enemies_have_shields = true;

        Debug.Log("SetEscalableBase: wave: " + new_wave);
    }


    public HPFormula(int level, int wave)
    {
        this.level = level;
        this.wave = wave;
        bossHP = 1;
        constant = 1.55f;
    }

    public HPFormula(int wave)
    {
        this.wave = wave;
    }

    /// <summary>
    /// Returns in Tuple multiple values for Dungeons
    /// </summary>
    /// <returns>double, float, double, float, double, float</returns>
    public Tuple<double, float, double, float, double, float> GetInfoDungeon1(int ID)
    {
        switch (ID)
        {
            case 1:
                var tuple1 = new Tuple<double, float, double, float, double, float>(d1_base, d1_const, d1_armor_base, d1_armor_const, d1_shield_base, d1_shield_const);
                return tuple1;
            case 2:
                var tuple2 = new Tuple<double, float, double, float, double, float>(d2_base, d2_const, d2_armor_base, d2_armor_const, d2_shield_base, d2_shield_const);
                return tuple2;
            case 3:
                var tuple3 = new Tuple<double, float, double, float, double, float>(d3_base, d3_const, d3_armor_base, d3_armor_const, d3_shield_base, d3_shield_const);
                return tuple3;
            case 4:
                var tuple4 = new Tuple<double, float, double, float, double, float>(d4_base, d4_const, d4_armor_base, d4_armor_const, d4_shield_base, d4_shield_const);
                return tuple4;
            case 5:
                var tuple5 = new Tuple<double, float, double, float, double, float>(d5_base, d5_const, d5_armor_base, d5_armor_const, d5_shield_base, d5_shield_const);
                return tuple5;
            case 6:
                var tuple6 = new Tuple<double, float, double, float, double, float>(d6_base, d6_const, d6_armor_base, d6_armor_const, d6_shield_base, d6_shield_const);
                return tuple6;
            case 7:
                var tuple7 = new Tuple<double, float, double, float, double, float>(d7_base, d7_const, d7_armor_base, d7_armor_const, d7_shield_base, d7_shield_const);
                return tuple7;
            case 8:
                var tuple8 = new Tuple<double, float, double, float, double, float>(d8_base, d8_const, d8_armor_base, d8_armor_const, d8_shield_base, d8_shield_const);
                return tuple8;

            default:
                var tuple_default = new Tuple<double, float, double, float, double, float>(0, 0, 0, 0, 0, 0);
                return tuple_default;
        }

    }

    public double GetEnemiesHP()
    {
        var name = SceneManager.GetActiveScene().name;
        return name switch
        {
            DUNGEON1 => GetFormulaResultByWave(d1_base, d1_const),
            DUNGEON2 => GetFormulaResultByWave(d2_base, d2_const),
            DUNGEON3 => GetFormulaResultByWave(d3_base, d3_const),
            DUNGEON4 => GetFormulaResultByWave(d4_base, d4_const),
            DUNGEON5 => GetFormulaResultByWave(d5_base, d5_const),
            DUNGEON6 => GetFormulaResultByWave(d6_base, d6_const),
            DUNGEON7 => GetFormulaResultByWave_Escalable(1.010f),//return GetFormulaResultByWave(d7_base, d7_const); Golden Dungeon
            DUNGEON8 => GetFormulaResultByWave(d8_base, d8_const),
            RAID1 => GetTotalDataForRaid(d1_base, d1_const),
            RAID2 => GetTotalDataForRaid(d2_base, d2_const),
            RAID3 => GetTotalDataForRaid(d3_base, d3_const),
            RAID4 => GetTotalDataForRaid(d4_base, d4_const),
            RAID5 => GetTotalDataForRaid(d5_base, d5_const),
            RAID6 => GetTotalDataForRaid(d6_base, d6_const),
            RAID7 => GetTotalDataForRaidEscalable(),
            _ => GetFormulaResultByWave(nw_base, nw_const),
        };
    }

    public double GetEnemiesArmor()
    {

        var name = SceneManager.GetActiveScene().name;

        switch (name)
        {
            case DUNGEON1:
                return GetFormulaResultByWave(d1_armor_base, d1_armor_const);
            case DUNGEON2:
                return GetFormulaResultByWave(d2_armor_base, d2_armor_const);
            case DUNGEON3:
                return GetFormulaResultByWave(d3_armor_base, d3_armor_const);
            case DUNGEON4:
                return GetFormulaResultByWave(d4_armor_base, d4_armor_const);
            case DUNGEON5:
                return GetFormulaResultByWave(d5_armor_base, d5_armor_const);
            case DUNGEON6:
                return GetFormulaResultByWave(d6_armor_base, d6_armor_const);
            case DUNGEON7:
                {
                    if (enemies_have_armor) return GetFormulaResultByWave_Escalable(1.012f);
                    else return 0;
                }

            case DUNGEON8:
                return GetFormulaResultByWave(d8_armor_base, d8_armor_const);
            case RAID1:
                return GetTotalDataForRaid(d1_armor_base, d1_armor_const);
            case RAID2:
                return GetTotalDataForRaid(d2_armor_base, d2_armor_const);
            case RAID3:
                return GetTotalDataForRaid(d3_armor_base, d3_armor_const);
            case RAID4:
                return GetTotalDataForRaid(d4_armor_base, d4_armor_const);
            case RAID5:
                return GetTotalDataForRaid(d5_armor_base, d5_armor_const);
            case RAID6:
                return GetTotalDataForRaid(d6_armor_base, d6_armor_const);
            case RAID7:
                if (enemies_have_armor) return GetTotalDataForRaidEscalable();
                else return 0;
            default:
                return Return_Armor();

        }

    }

    private double Return_Armor()
    {
        if (waves.waveLevel >= 10000) 
        {
            if (waves.waveLevel % 10 == 0) //BOSS
            {

                if (double.IsInfinity(Math.Round(100 * Math.Pow(1.15, waves.waveLevel - 9999)))) //si llegamos a infinito corregimos
                {

                    return Math.Round(double.MaxValue);
                }
                armor = Math.Round(100 * Math.Pow(1.15, waves.waveLevel - 9999));
            }
            else //ENEMY
            {
                if (double.IsInfinity(Math.Round(10 * Math.Pow(1.10, waves.waveLevel - 9999))))
                {
                    return Math.Round(double.MaxValue);
                }
                armor = Math.Round(10 * Math.Pow(1.10, waves.waveLevel - 9999));
            }


        }
        return armor;
    }

    public double GetEnemiesShields()
    {


        var name = SceneManager.GetActiveScene().name;

        switch (name)
        {
            case DUNGEON1:
                if (d1_shield_base == 0) return 0;
                else return GetFormulaResultByWave(d1_shield_base, d1_shield_const);
            case DUNGEON2:
                return GetFormulaResultByWave(d2_shield_base, d2_shield_const);
            case DUNGEON3:
                return GetFormulaResultByWave(d3_shield_base, d3_shield_const);
            case DUNGEON4:
                return GetFormulaResultByWave(d4_shield_base, d4_shield_const);
            case DUNGEON5:
                return GetFormulaResultByWave(d5_shield_base, d5_shield_const);
            case DUNGEON6:
                return GetFormulaResultByWave(d6_shield_base, d6_shield_const);
            case DUNGEON7:
                {
                    if (enemies_have_shields) return GetFormulaResultByWave_Escalable(1.015f);
                    else return 0;
                }
            case DUNGEON8:
                return GetFormulaResultByWave(d8_shield_base, d8_shield_const);
            case RAID1:
                return GetTotalDataForRaid(d1_shield_base, d1_shield_const);
            case RAID2:
                return GetTotalDataForRaid(d2_shield_base, d2_shield_const);
            case RAID3:
                return GetTotalDataForRaid(d3_shield_base, d3_shield_const);
            case RAID4:
                return GetTotalDataForRaid(d4_shield_base, d4_shield_const);
            case RAID5:
                return GetTotalDataForRaid(d5_shield_base, d5_shield_const);
            case RAID6:
                return GetTotalDataForRaid(d6_shield_base, d6_shield_const);
            case RAID7:
                //SetEscalableBase();
                if (enemies_have_shields) return GetTotalDataForRaidEscalable();
                else return 0;
            default:
                return Return_Shields();
        }

    }

    private double Return_Shields()
    {
        if (waves.waveLevel >= 20000) //A partir de la wave X empezamos a poner armor
        {
            if (wave % 10 == 0)
            {
                if (double.IsInfinity(Math.Round(100 * Math.Pow(1.15, waves.waveLevel - 19500))))
                {
                    return Math.Round(double.MaxValue);
                }
                shields = Math.Round(100 * Math.Pow(1.15, waves.waveLevel - 19500));
            }
            else
            {
                if (double.IsInfinity(Math.Round(100 * Math.Pow(1.15, waves.waveLevel - 19500))))
                {
                    return Math.Round(double.MaxValue);
                }
                shields = Math.Round(10 * Math.Pow(1.10, waves.waveLevel - 19500));

            }

        }

        return shields;
    }


    /// <summary>
    /// Used for HP/ARMOR/SHIELDS Enemy. Returns double based on constant and base life. Bosses have an increment of 50% more life and 5% more curve
    /// </summary>
    /// <param name="_base"></param>
    /// <param name="_constant"></param>
    /// <param name="boss">true if needed formula for a boss, false for normal enemies</param>
    /// <returns>double</returns>
    public double GetFormulaResultByWave(double _base, float _constant, bool boss, int wave_level)
    {
        if (boss)
        {
            double new_base = _base * 1.5f;
            float new_constant = _constant * 1.05f;
            return Math.Round(new_base * Math.Pow(new_constant, wave_level) + 1); 
        }
        else
        {
            return Math.Round(_base * Math.Pow(_constant, wave_level)); 
        }
    }

    /// <summary>
    /// Returns start life of wave 1 and last wave life
    /// </summary>
    /// <param name="wave_level"></param>
    /// <returns>Double</returns>
    public double GetFormulaResultByEscalableWave(int wave_level)
    {
        SetEscalableBase();
        double _base = escalable_base;
        float _const = nw_const * 1.015f;
        return Math.Round((wave_level - 1) + _base * Math.Pow(_const, wave_level));


    }

    private bool BossWave => waves.waveLevel % 10 == 0;

    /// <summary>
    /// Used for Armor and Shields. Returns armor or shields based on constant and base. Bosses have an increment of 50% more life and 5% more curve
    /// </summary>
    /// <param name="_base"></param>
    /// <param name="_constant"></param>
    /// <param name="base_boss_inc">Set to 1 if no inc needed</param>
    /// <param name="constant_boss_inc">Set to 1 if no const needed</param>
    /// <returns></returns>
    private double GetFormulaResultByWave(double _base, float _constant)
    {
        if (_base <= 0) return 0;
        if (BossWave)
        {
            #region NEW FOR BALANCE
            //If double returns infinity
            if (double.IsInfinity(Math.Round(waves.waveLevel + _base * Math.Pow(_constant, waves.waveLevel))))
            {
                return Math.Round(double.MaxValue);
            }
            if (!double.IsInfinity(Math.Round((waves.waveLevel + _base * Math.Pow(_constant, waves.waveLevel)) * 20)))
            {
                return Math.Round((waves.waveLevel + _base * Math.Pow(_constant, waves.waveLevel)) * 20);
            }
            else return Math.Round(double.MaxValue);
            #endregion
        }
        else
        {
            if (double.IsInfinity(Math.Round((waves.waveLevel - 1) + _base * Math.Pow(_constant, waves.waveLevel))))
            {
                return Math.Round(double.MaxValue);
            }
            return Math.Round((waves.waveLevel - 1) + _base * Math.Pow(_constant, (waves.waveLevel)));
        }
    }


    public double GetTotalDataForRaid(double _base, float _const)
    {
        double total_hp = 0;

        for (int i = 0; i < 200; i++)
        {
            total_hp += Math.Round(_base * Math.Pow(_const, i));
        }

        if (double.IsInfinity(total_hp * 10)) return double.MaxValue;
        return total_hp * 10;
    }

    public double GetTotalDataForRaid_Armor(double _base, float _const)
    {

        if (checked_wave >= 10000) return GetTotalDataForRaid(_base, _const);
        else return 0;

    }

    public double GetTotalDataForRaid_Shields(double _base, float _const)
    {
        if (checked_wave >= 20000) return GetTotalDataForRaid(_base, _const);
        else return 0;
    }

    /// <summary>
    /// Return HP for Raid Golden Chamber
    /// </summary>
    /// <returns></returns>
    public double GetTotalDataForRaidEscalable()
    {
        SetEscalableBase();
        double _base = escalable_base;
        float _const = nw_const * 1.015f;

        double total_hp = 0;
        for (int i = 0; i < 200; i++)
        {
            total_hp += Math.Round((i - 1) + _base * Math.Pow(_const, i));
        }
        if (double.IsInfinity(total_hp * 10)) return double.MaxValue;
        return total_hp * 10;
    }

    /// <summary>
    /// Return Armor for Raid Golden Chamber
    /// </summary>
    /// <returns></returns>
    public double GetTotalDataForRaidEscalableArmor ()
    {
        SetEscalableBase();
        if (!enemies_have_armor) return 0;
        else
        {
            double _base = escalable_base;
            float _const = nw_const * 1.015f;

            double total_hp = 0;
            for (int i = 0; i < 200; i++)
            {
                total_hp += Math.Round((i - 1) + _base * Math.Pow(_const, i));
            }
            if (double.IsInfinity(total_hp * 10)) return double.MaxValue;
            return total_hp * 10;
        }
        
    }

    /// <summary>
    /// Return Shields for Raid Golden Chamber
    /// </summary>
    /// <returns></returns>
    public double GetTotalDataForRaidEscalableShield ()
    {
        SetEscalableBase();
        if (!enemies_have_shields) return 0;
        else
        {
            double _base = escalable_base;
            float _const = nw_const * 1.015f;

            double total_hp = 0;
            for (int i = 0; i < 200; i++)
            {
                total_hp += Math.Round((i - 1) + _base * Math.Pow(_const, i));
            }
            if (double.IsInfinity(total_hp * 10)) return double.MaxValue;
            return total_hp * 10;
        }
    }


    /// <summary>
    /// Returns hp based on the actual wave -10
    /// </summary>
    /// <returns>double</returns>
    private double GetFormulaResultByWave_Escalable(float plus)
    {
        SetEscalableBase();

        double _base = escalable_base;
        float _const = nw_const * 1.005f * plus;


        double is_infinite_boss = Math.Round(waves.waveLevel + _base * Math.Pow(_const, waves.waveLevel) * 20);
        double is_infinite_normal = Math.Round((waves.waveLevel - 1) + _base * Math.Pow(_const, waves.waveLevel));

        if (BossWave)
        {
            if (double.IsInfinity(is_infinite_boss)) return Math.Round(double.MaxValue);
            else return Math.Round(waves.waveLevel + _base * Math.Pow(_const, waves.waveLevel) * 20);
        }
        else
        {
            Debug.Log("waves.waveLevel - 1: " + (waves.waveLevel - 1) + "|| _base: " + _base + "|| _const: " + _const + "wave.level " + waves.waveLevel);
            Debug.Log("RESULT: " + Math.Round((waves.waveLevel - 1) + _base * Math.Pow(_const, waves.waveLevel)));
            if (double.IsInfinity(is_infinite_normal)) return Math.Round(double.MaxValue);
            else return Math.Round((waves.waveLevel - 1) + _base * Math.Pow(_const, waves.waveLevel));
        }
    }



    private double GetFormulaResultByWave_Armor(int _base, float _constant)
    {
        if (waves.waveLevel >= 7001 && waves.waveLevel % 10 == 0)
        {
            float new_base = _base * 1.5f;
            armor = Math.Round(new_base * Math.Pow(_constant, waves.waveLevel - 7000));
        }
        else if (waves.waveLevel >= 7001)
        {
            armor = Math.Round(_base * Math.Pow(_constant, waves.waveLevel - 7000));
        }
        return armor;
    }

    private double GetGeneralFormula(double _base, float _constant)
    {
        return Math.Round(waves.waveLevel + _base * Math.Pow(_constant, waves.waveLevel));
    }
}
