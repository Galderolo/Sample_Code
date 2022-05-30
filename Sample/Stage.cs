//This class should only manage to detect the stage selected

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Formulas;


public class Stage : MonoBehaviour
{

    public Stage_Selection selector;
    public GameObject panel_info;
    public GameObject panel_click;
    private Animator anim;
    [SerializeField] private bool IsClicked;
    private bool hide;

    public Button btn_enter;

    public TextMeshProUGUI title;

    public GameObject bat;
    private Quaternion originalLookAt;
    private Vector3 newDirection;
    private float speedBatLooking = 10f;
    [SerializeField] private bool batLooking;


    [SerializeField] string currentSelected;
    private PointerEventData currentData;

    public GameObject new_panel_dungeon_raid;
    private Panel_Info_Dungeon pid;

    #region INFO UI
    public TextMeshProUGUI name;
    public TextMeshProUGUI waves;
    public TextMeshProUGUI enemy;
    public TextMeshProUGUI life;
    public TextMeshProUGUI armor;
    public TextMeshProUGUI shield;
    public TextMeshProUGUI gold;
    public TextMeshProUGUI gem;
    public Image avatar;
    public TextMeshProUGUI keys;
    #endregion

    private Keys my_keys;
    private Scrolls my_scrolls;
    private int currentkeys;
    private bool loading = false;


    #region DUNGEONS CONST
    private const string D1 = "d1"; //DEBE COINCIDIR CON EL NOMBRE EN EL INSPECTOR
    private const string D2 = "d2";
    private const string D3 = "d3";
    private const string D4 = "d4";
    private const string D5 = "d5";
    private const string D6 = "d6";
    private const string DGOLD = "dgold";
    #endregion

    private void Awake()
    {
        my_keys = Keys.Instance;
        my_scrolls = Scrolls.Instance;
    }
    void Start()
    {


        selector.StageIsClicked += StageIsSelected;
        selector.StageEnter += StageEnter;
        selector.StageExit += StageExit;
        selector.StageDeselected += StageDeselected;

        anim = panel_click.GetComponent<Animator>();
        currentSelected = "";

        btn_enter.onClick.AddListener(Callback);
        btn_enter.interactable = false;

        originalLookAt = bat.transform.rotation;
        bat.transform.LookAt(Camera.main.transform);

        pid = new_panel_dungeon_raid.GetComponent<Panel_Info_Dungeon>();
    }

    private void Update()
    {
        if (batLooking)
        {
            var targetRot = Quaternion.LookRotation(newDirection - bat.transform.position);
            bat.transform.rotation = Quaternion.Slerp(bat.transform.rotation, targetRot, speedBatLooking * Time.deltaTime);
        }
        else if (!batLooking && !IsClicked)
        {
            var targeRot = Quaternion.LookRotation(Camera.main.transform.position - bat.transform.position);
            //bat.transform.rotation = originalLookAt;
            bat.transform.rotation = Quaternion.Slerp(bat.transform.rotation, targeRot, speedBatLooking * Time.deltaTime);
        }


    }



    private void StageIsSelected(PointerEventData data)
    {
        if (currentSelected != data.pointerEnter.name && currentSelected != "")
        {
            GameObject.Find(currentSelected).transform.GetChild(0).gameObject.SetActive(false);
            btn_enter.interactable = false;
            currentData = null;
        }

        if (currentSelected == data.pointerEnter.name) //esto es nuevo
        {
            data.pointerEnter.transform.GetChild(0).gameObject.SetActive(false);
            IsClicked = false;
            btn_enter.interactable = false;
            currentSelected = "";
            batLooking = false;
            currentData = null;
            return;
        }
        else
        {
            currentSelected = data.pointerEnter.name;
            currentData = data;
            //Debug.Log($"CLICK:  {data.pointerEnter.name}");
            IsClicked = true;
            SetDescription(data);
            GameObject.Find(currentSelected).transform.GetChild(0).gameObject.SetActive(true);
            SFX_StageSelected();
            new_panel_dungeon_raid.SetActive(true); //Activate new window
            pid.SetDataToInfoDungeonRaid(data);//send data

            newDirection = data.pointerEnter.transform.position;
            batLooking = true;

        }

    }

    private void StageEnter(PointerEventData data)
    {
        if (!IsClicked)
        {
            SetDescription(data);
        }

        data.pointerEnter.transform.GetChild(0).gameObject.SetActive(true);
        newDirection = data.pointerEnter.transform.position;
        batLooking = true;
        SFX_StageEnter();

    }

    private void StageExit(PointerEventData data)
    {

        if (!IsClicked && data.pointerEnter.name != currentSelected) //sino esta clickead, ocultamos, sino dejamos
        {
            data.pointerEnter.transform.GetChild(0).gameObject.SetActive(false);

        }
        if (currentSelected != "") //si algun stage esta seleccionado en el momento de sacar el raton
        {
            if (data.pointerEnter.name != currentSelected) //si se ha puesto el raton encima de otro diferente y lo quitamos, ocultamos
            {
                data.pointerEnter.transform.GetChild(0).gameObject.SetActive(false);
                currentData = null;
            }

        }
        if (!IsClicked)
            StageClear();

        batLooking = false;

    }

    private void StageDeselected(BaseEventData data)
    {

    }

    //CLICK DEL RATON
    public void Callback()
    {

        SFX_StageClicked();

        switch (currentSelected)
        {
            case D1:
                Load_Dungeon(1);
                break;
            case D2:
                Load_Dungeon(2);
                break;
            case D3:
                Load_Dungeon(3);
                break;
            case D4:
                Load_Dungeon(4);
                break;
            case D5:
                Load_Dungeon(5);
                break;
            case D6:
                Load_Dungeon(6);
                break;
            case DGOLD:
                Load_Dungeon(7, CurrenciesAndValues.keys_for_goldchamber);
                break;

            default:
                return;
        }

    }

    public void CallBackToRaid()
    {
        SFX_StageClicked();

        switch (currentSelected)
        {
            case D1:
                Load_Dungeon(8, CurrenciesAndValues.keys_for_raid, CurrenciesAndValues.scrolls_for_raid, ref my_scrolls.scroll_ruby);
                break;
            case D2:
                Load_Dungeon(9, CurrenciesAndValues.keys_for_raid, CurrenciesAndValues.scrolls_for_raid, ref my_scrolls.scroll_sapphire);
                break;
            case D3:
                Load_Dungeon(10, CurrenciesAndValues.keys_for_raid, CurrenciesAndValues.scrolls_for_raid, ref my_scrolls.scroll_emerald);
                break;
            case D4:
                Load_Dungeon(11, CurrenciesAndValues.keys_for_raid, CurrenciesAndValues.scrolls_for_raid, ref my_scrolls.scroll_topaz);
                break;
            case D5:
                Load_Dungeon(12, CurrenciesAndValues.keys_for_raid, CurrenciesAndValues.scrolls_for_raid, ref my_scrolls.scroll_amethyst);
                break;
            case D6:
                Load_Dungeon(13, CurrenciesAndValues.keys_for_raid, CurrenciesAndValues.scrolls_for_raid, ref my_scrolls.scroll_diamond);
                break;
            case DGOLD:
                Load_Dungeon(14, CurrenciesAndValues.keys_for_mimic);
                break;

        }
    }


    private void SetDescription(PointerEventData data)
    {


        if (data != null)
        {
            var desc = data.pointerEnter.transform.GetComponent<Stage_Description>();
            var ID = data.pointerCurrentRaycast.gameObject.GetComponent<Stage_Description>().ID;
            name.text = desc.s_dungeon_name;
            waves.text = desc.s_waves;
            enemy.text = desc.s_enemy_type;
            SetHP(ID);
            SetGold(ID);
            gem.text = desc.s_gem_dropped;
            avatar.sprite = desc.i_avatar_dungeon;
            keys.text = desc.s_keys;

        }


    }

    //Guardamos el stage actual que esta clickeado
    private void SetCurrentData(PointerEventData data)
    {
        //currentData = data;

    }

    private void StageClear()
    {
        name.text = "";
        name.text = "";
        waves.text = "";
        enemy.text = "";
        life.text = "";
        gold.text = "";
        gem.text = "";
        keys.text = "";
        avatar.sprite = null;
    }

    private void SetHP(int ID)
    {
        FormatoNumeros format = FormatoNumeros.Instance;
        Config_HP confighp = Config_HP.Instance;

        if (ID != 8)
        {

            var normal_dungeons_data = confighp.Normal_Dungeons_Data;
            var armored_dungeons_data = confighp.Armored_Dungeons_Data;
            var shielded_dungeons_data = confighp.Shielded_Dungeons_Data;

            //HP INFO
            double _base = normal_dungeons_data[ID]._base;
            float _const = normal_dungeons_data[ID]._const;
            double start_life = confighp.GetConfigHpByWave(_base, _const, false, 1);
            double end_life = confighp.GetConfigHpByWave(_base, _const, false, 200);


            //ARMOR INFO
            double _base_armor = armored_dungeons_data[ID]._base;
            float _const_armor = armored_dungeons_data[ID]._const;
            double start_armor = confighp.GetConfigHpByWave(_base_armor, _const_armor, false, 1);
            double end_armor = confighp.GetConfigHpByWave(_base_armor, _const_armor, false, 200);


            //SHIELD INFO
            double _base_shield = shielded_dungeons_data[ID]._base;
            float _const_shield = shielded_dungeons_data[ID]._const;
            double start_shield = confighp.GetConfigHpByWave(_base_shield, _const_shield, false, 1);
            double end_shield = confighp.GetConfigHpByWave(_base_shield, _const_shield, false, 200);


            //SETTING THE INFO
            life.text = format.formatNumber(start_life) + " - " + format.formatNumber(end_life);
            armor.text = format.formatNumber(start_armor) + " - " + format.formatNumber(end_armor);
            shield.text = format.formatNumber(start_shield) + " - " + format.formatNumber(end_shield);
        }
        else
        {
            double start_life = confighp.GetConfigHP();
            double end_life = confighp.GetConfigHP();
            life.text = format.formatNumber(start_life) + " - " + format.formatNumber(end_life);
            armor.text = format.formatNumber(0) + " - " + format.formatNumber(0);
            shield.text = format.formatNumber(0) + " - " + format.formatNumber(0);

        }
    }

    private void SetGold(int ID)
    {
        FormatoNumeros format = FormatoNumeros.Instance;
        double g = GoldFormula.Instance.GetTotalGold(ID);
        gold.text = format.formatNumber(g);
    }

    private void Load_Dungeon(int dungeon_id)
    {
        if (my_keys.keys >= 1)
        {
            if (!loading)
            {
                loading = true;
                //Debug.Log("Cuantas veces se ejecuta esto keys--");
                PlayerPrefs.SetInt("dungeon", dungeon_id);
                my_keys.keys--;
                if (my_keys.keys < 0) my_keys.keys = 0;
                Load_Save_Manager.Save_Obj_Key("keys", my_keys.keys);
                Load_Save_Manager.InsertFromCache_ToFile();
                SceneManager.LoadScene("Load", LoadSceneMode.Single);
                //Load_Dungeon(1);
            }
        }
        else
        {
            Notify_No_Keys();
        }
    }

    private void Load_Dungeon(int dungeon_id, int keys_quantity)
    {
        if (my_keys.keys >= keys_quantity)
        {
            if (!loading)
            {
                loading = true;
                //Debug.Log("Cuantas veces se ejecuta esto keys--");
                PlayerPrefs.SetInt("dungeon", dungeon_id);//dungeon_id);
                my_keys.keys -= keys_quantity;
                if (my_keys.keys < 0) my_keys.keys = 0;
                Load_Save_Manager.Save_Obj_Key("keys", my_keys.keys);
                Load_Save_Manager.InsertFromCache_ToFile();
                SceneManager.LoadScene("Load", LoadSceneMode.Single);
                //Load_Dungeon(1);
            }
        }
        else
        {
            Notify_No_Keys();
        }
    }



    /// <summary>
    /// Load Raid
    /// </summary>
    /// <param name="dungeon_id">ID</param>
    /// <param name="keys_quantity">KEYS QUANTITY</param>
    /// <param name="scroll_quantity">SCROLLS QUANTITY</param>
    /// <param name="scroll_type">SCROLLS TYPE</param>
    private void Load_Dungeon(int dungeon_id, int keys_quantity, int scroll_quantity, ref int scroll_type)
    {
        if (my_keys.keys >= keys_quantity && scroll_type >= scroll_quantity)
        {
            Debug.Log("scroll_quantity: " + scroll_type);
            if (!loading)
            {
                loading = true;
                PlayerPrefs.SetInt("dungeon", dungeon_id);
                my_keys.keys -= keys_quantity;
                if (my_keys.keys < 0) my_keys.keys = 0;
                scroll_type -= scroll_quantity;
                Load_Save_Manager.Save_Obj_Key("keys", my_keys.keys);
                Load_Save_Manager.Save_Obj_Scrolls("scrolls", my_scrolls);
                Load_Save_Manager.InsertFromCache_ToFile();
                SceneManager.LoadScene("Load", LoadSceneMode.Single);
            }
        }
        else
        {
            Notify_No_Resources();
        }
    }

    private void Notify_No_Keys()
    {
        Notify.Instance.SetNotification("You have no keys to enter this Dungeon");
    }

    private void Notify_No_Resources()
    {
        Notify.Instance.SetNotification("You have not enough resources");
    }

    private void SFX_StageClicked()
    {
        FMOD_AUDIO.instance._Play_Once("event:/STAGE_CLICKED");
    }

    private void SFX_StageEnter()
    {
        FMOD_AUDIO.instance._Play_Once("event:/STAGE_ENTER");
    }

    private void SFX_StageSelected()
    {
        FMOD_AUDIO.instance._Play_Once("event:/STAGE_SELECTED");
    }


}
