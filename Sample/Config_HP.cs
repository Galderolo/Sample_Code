
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Formulas
{
    class Config_HP : MonoBehaviour
    {
        private const string STR_WAVE = "wave";
        private const int WAVE_ARMOR_LIMIT = 10000;
        private const int WAVE_SHIELD_LIMIT = 20000;

        #region INSPECTOR
        [SerializeField] private double hp;
        [SerializeField] private double armor;
        [SerializeField] private double shields;

        [Header("NORMAL WAVES")]
        [SerializeField] [Tooltip("Base Life")] private double nw_base;
        [SerializeField] [Tooltip("Increment Base Life for Bosses")] private float nw_base_inc;
        [SerializeField] [Tooltip("Constant")] private float nw_const;
        [SerializeField] [Tooltip("Constant Increment for Bosses")] private float nw_const_inc;

        [Header("DUNGEON DATA")]

        public struct Normal_Dungeons
        {
            public double _base;
            public double _const;
        }
        public Normal_Dungeons[] normal_dungeons_data;

        public struct Armored_Dungeons
        {
            public double _base;
            public float _const;
        }
        public Armored_Dungeons[] armored_dungeons_data;


        public struct Shielded_Dungeons
        {
            public double _base;
            public float _const;
        }
        public Shielded_Dungeons[] shielded_dungeons_data;
        
        
        #endregion

        private readonly double max = Double.MaxValue;
        private int level, wave, new_wave = 1, checked_wave;
        private double boss_hp, escalable_base;
        private float constant;

        private WaveManager waves;

        private bool enemies_have_armor, enemies_have_shields;

        public static Config_HP Instance { get; private set; }

        //Check file exist and key 
        private bool CheckFile_CheckKey => ES3.FileExists() && ES3.KeyExists(STR_WAVE);

        //Returns saved wave. !Rfctr
        private int LoadSaveWave => ES3.Load<int>(STR_WAVE);

        private bool IsInfinity(double num) => double.IsFinite(num);
        


        private bool IsBossWave => waves.Instance.wavesLevel % 10 == 0;

        #region SINGLETON
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
            Instance = this;

        }
        #endregion

        private void Start()
        {
            waves = WaveManager.Instance;
            SetEscalableWave();
            CheckTheWave();
        }


        //Check if the key is correct and load saved wave in current wave
        private void CheckTheWave()
        {
            if (CheckFile_CheckKey)
            {
                checked_wave = LoadSaveWave;

            }

        }

        
        #region GET DUNGEON STATS
        public Normal_Dungeons[] Normal_Dungeons_Data => normal_dungeons_data;

        public Armored_Dungeons[] Armored_Dungeons_Data => armored_dungeons_data;

        public Shielded_Dungeons[] Shielded_Dungeons_Data => shielded_dungeons_data; 
        #endregion


        #region SET DUNGEONS STATS
        /// <summary>
        /// Set Scalable Life for Enemies by Waves
        /// </summary>
        private void SetScalableHPByWaves()
        {
            int num_waves = 200;
            if (CheckFile_CheckKey) new_wave = LoadSaveWave;
            if (new_wave <= 200) num_waves = 1;

            //Start from saved wave in normal game and scale it
            escalable_base = Math.Round((new_wave - 1) + nw_base * Math.Pow(nw_const, new_wave - num_waves));

            //Check armor & shield at wave limit
            if (new_wave >= WAVE_ARMOR_LIMIT) enemies_have_armor = true;
            if (new_wave >= WAVE_SHIELD_LIMIT) enemies_have_shields = true;
        }


        /// <summary>
        /// Config normal and Boss waves hp.
        /// </summary>
        /// <param name="_base">double</param>
        /// <param name="_const">float</param>
        /// <returns>double</returns>
        private double ConfigHPByWave (double _base, float _const)
        {
            double tempBoss_hp = Math.Round(waves.waveLevel + _base * Math.Pow(_const, waves.waveLevel));
            double tempNormal_hp = Math.Round((waves.waveLevel - 1) + _base * Math.Pow(_const, waves.waveLevel));
            double max_num = Math.Round(double.MaxValue);

            if (_base <= 0) return 0;

            if (IsInfinity(tempBoss_hp)) return max_num;

            if(IsBossWave)
            {
                if (!IsInfinity(tempBoss_hp * 20)) return tempBoss_hp * 20;
                else return max_num;
            }
            else
            {
                if (IsInfinity(tempNormal_hp)) return max_num;
                return tempNormal_hp;
            }
        }

        #endregion
    }
}
