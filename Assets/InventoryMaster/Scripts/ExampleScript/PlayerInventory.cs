﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInventory : MonoBehaviour
{
    public GameObject target;
    public GameObject inventory;
    public GameObject characterSystem;
    public GameObject craftSystem;
    private Inventory craftSystemInventory;
    private CraftSystem cS;
    private Inventory mainInventory;
    private Inventory characterSystemInventory;
    private Tooltip toolTip;

    private InputManager inputManagerDatabase;
    private PlayerController player;

    public GameObject HPMANACanvas;

    Text hpText;
    Text manaText;
    Image hpImage;
    Image manaImage;

    int normalSize = 3;

    public void OnEnable()
    {
        Inventory.ItemEquip += OnBackpack;
        Inventory.UnEquipItem += UnEquipBackpack;

        Inventory.ItemEquip += OnGearItem;
        Inventory.ItemConsumed += OnConsumeItem;
        Inventory.UnEquipItem += OnUnEquipItem;

        Inventory.ItemEquip += EquipWeapon;
        Inventory.UnEquipItem += UnEquipWeapon;
    }

    public void OnDisable()
    {
        Inventory.ItemEquip -= OnBackpack;
        Inventory.UnEquipItem -= UnEquipBackpack;

        Inventory.ItemEquip -= OnGearItem;
        Inventory.ItemConsumed -= OnConsumeItem;
        Inventory.UnEquipItem -= OnUnEquipItem;

        Inventory.UnEquipItem -= UnEquipWeapon;
        Inventory.ItemEquip -= EquipWeapon;
    }

    void EquipWeapon(Item item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            //add the weapon if you unequip the weapon
        }
    }

    void UnEquipWeapon(Item item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            //delete the weapon if you unequip the weapon
        }
    }

    void OnBackpack(Item item)
    {
        if (item.itemType == ItemType.Backpack)
        {
            for (int i = 0; i < item.itemAttributes.Count; i++)
            {
                if (mainInventory == null)
                    mainInventory = inventory.GetComponent<Inventory>();
                mainInventory.sortItems();
                if (item.itemAttributes[i].attributeName == "Slots")
                    changeInventorySize(item.itemAttributes[i].attributeValue);
            }
        }
    }

    void UnEquipBackpack(Item item)
    {
        if (item.itemType == ItemType.Backpack)
            changeInventorySize(normalSize);
    }

    void changeInventorySize(int size)
    {
        dropTheRestItems(size);

        if (mainInventory == null)
            mainInventory = inventory.GetComponent<Inventory>();
        if (size == 3)
        {
            mainInventory.width = 3;
            mainInventory.height = 1;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
        if (size == 6)
        {
            mainInventory.width = 3;
            mainInventory.height = 2;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
        else if (size == 12)
        {
            mainInventory.width = 4;
            mainInventory.height = 3;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
        else if (size == 16)
        {
            mainInventory.width = 4;
            mainInventory.height = 4;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
        else if (size == 24)
        {
            mainInventory.width = 6;
            mainInventory.height = 4;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
    }

    void dropTheRestItems(int size)
    {
        if (size < mainInventory.ItemsInInventory.Count)
        {
            for (int i = size; i < mainInventory.ItemsInInventory.Count; i++)
            {
                GameObject dropItem = (GameObject)Instantiate(mainInventory.ItemsInInventory[i].itemModel);
                dropItem.AddComponent<PickUpItem>();
                dropItem.GetComponent<PickUpItem>().item = mainInventory.ItemsInInventory[i];
                dropItem.transform.localPosition = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
            }
        }
    }

    void Start()
    {
        //if (HPMANACanvas != null)
        //{
        //    hpText = HPMANACanvas.transform.GetChild(1).GetChild(0).GetComponent<Text>();

        //    manaText = HPMANACanvas.transform.GetChild(2).GetChild(0).GetComponent<Text>();

        //    hpImage = HPMANACanvas.transform.GetChild(1).GetComponent<Image>();
        //    manaImage = HPMANACanvas.transform.GetChild(1).GetComponent<Image>();

        //    UpdateHPBar();
        //    UpdateManaBar();
        //}

        if (inputManagerDatabase == null)
            inputManagerDatabase = (InputManager)Resources.Load("InputManager");

        if (player == null)
            player = target.GetComponent<PlayerController>();

        if (craftSystem != null)
            cS = craftSystem.GetComponent<CraftSystem>();

        if (GameObject.FindGameObjectWithTag("Tooltip") != null)
            toolTip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
        if (inventory != null)
            mainInventory = inventory.GetComponent<Inventory>();
        if (characterSystem != null)
            characterSystemInventory = characterSystem.GetComponent<Inventory>();
        if (craftSystem != null)
            craftSystemInventory = craftSystem.GetComponent<Inventory>();
    }

    //void UpdateHPBar()
    //{
    //    hpText.text = (currentHealth + "/" + maxHealth);
    //    float fillAmount = currentHealth / maxHealth;
    //    hpImage.fillAmount = fillAmount;
    //}

    //void UpdateManaBar()
    //{
    //    manaText.text = (currentMana + "/" + maxMana);
    //    float fillAmount = currentMana / maxMana;
    //    manaImage.fillAmount = fillAmount;
    //}


    public void OnConsumeItem(Item item)
    {
        for (int i = 0; i < item.itemAttributes.Count; i++)
        {
            if (item.itemAttributes[i].attributeName == "Health")
            {
                if ((player.health + item.itemAttributes[i].attributeValue) > player.maxHealth)
                    player.health = player.maxHealth;
                else
                    player.health += item.itemAttributes[i].attributeValue;
            }
            if (item.itemAttributes[i].attributeName == "Mana")
            {
                if ((player.mana + item.itemAttributes[i].attributeValue) > player.maxMana)
                    player.mana = player.maxMana;
                else
                    player.mana += item.itemAttributes[i].attributeValue;
            }
            if (item.itemAttributes[i].attributeName == "Armor")
            {
                if ((player.armor + item.itemAttributes[i].attributeValue) > player.maxArmor)
                    player.armor = player.maxArmor;
                else
                    player.armor += item.itemAttributes[i].attributeValue;
            }
            if (item.itemAttributes[i].attributeName == "Damage")
            {
                if ((player.damage + item.itemAttributes[i].attributeValue) > player.maxDamage)
                    player.damage = player.maxDamage;
                else
                    player.damage += item.itemAttributes[i].attributeValue;
            }
        }
        //if (HPMANACanvas != null)
        //{
        //    UpdateManaBar();
        //    UpdateHPBar();
        //}
    }

    public void OnGearItem(Item item)
    {
        for (int i = 0; i < item.itemAttributes.Count; i++)
        {
            if (item.itemAttributes[i].attributeName == "Health")
                player.maxHealth += item.itemAttributes[i].attributeValue;
            if (item.itemAttributes[i].attributeName == "Mana")
                player.maxMana += item.itemAttributes[i].attributeValue;
            if (item.itemAttributes[i].attributeName == "Armor")
                player.maxArmor += item.itemAttributes[i].attributeValue;
            if (item.itemAttributes[i].attributeName == "Damage")
                player.maxDamage += item.itemAttributes[i].attributeValue;
        }
        //if (HPMANACanvas != null)
        //{
        //    UpdateManaBar();
        //    UpdateHPBar();
        //}
    }

    public void OnUnEquipItem(Item item)
    {
        for (int i = 0; i < item.itemAttributes.Count; i++)
        {
            if (item.itemAttributes[i].attributeName == "Health")
                player.maxHealth -= item.itemAttributes[i].attributeValue;
            if (item.itemAttributes[i].attributeName == "Mana")
                player.maxMana -= item.itemAttributes[i].attributeValue;
            if (item.itemAttributes[i].attributeName == "Armor")
                player.maxArmor -= item.itemAttributes[i].attributeValue;
            if (item.itemAttributes[i].attributeName == "Damage")
                player.maxDamage -= item.itemAttributes[i].attributeValue;
        }
        //if (HPMANACanvas != null)
        //{
        //    UpdateManaBar();
        //    UpdateHPBar();
        //}
    }

    public void addItemToInventory(int id)
    {
        mainInventory.addItemToInventory(id);
        mainInventory.updateItemList();
        mainInventory.stackableSettings();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(inputManagerDatabase.CharacterSystemKeyCode))
        {
            if (!characterSystem.activeSelf)
            {
                characterSystemInventory.openInventory();
            }
            else
            {
                if (toolTip != null)
                    toolTip.deactivateTooltip();
                characterSystemInventory.closeInventory();
            }
        }

        if (Input.GetKeyDown(inputManagerDatabase.InventoryKeyCode))
        {
            if (!inventory.activeSelf)
            {
                mainInventory.openInventory();
            }
            else
            {
                if (toolTip != null)
                    toolTip.deactivateTooltip();
                mainInventory.closeInventory();
            }
        }

        if (Input.GetKeyDown(inputManagerDatabase.CraftSystemKeyCode))
        {
            if (!craftSystem.activeSelf)
                craftSystemInventory.openInventory();
            else
            {
                if (cS != null)
                    cS.backToInventory();
                if (toolTip != null)
                    toolTip.deactivateTooltip();
                craftSystemInventory.closeInventory();
            }
        }

    }

}
