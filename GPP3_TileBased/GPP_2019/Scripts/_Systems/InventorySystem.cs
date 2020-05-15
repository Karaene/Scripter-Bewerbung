
using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class InventorySystem : ISystem
    {
        private InventorySystem() { }
        private static InventorySystem instance = null;
        public static InventorySystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InventorySystem();
                }
                return instance;
            }
        }

        private List<InventoryComponent> inventories = new List<InventoryComponent>();

        internal IEntityComponent CreateInventoryComponent()
        {
            InventoryComponent inventory = new InventoryComponent(this);
            inventories.Add(inventory);
            return inventory;
        }

        public void Init()
        {
            EventSystem.Instance.AddListener("AddItem", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    foreach (var inventory in inventories)
                    {
                        if (inventory.GameObject == parameters[0])
                        {
                            inventory.Items.Add((GameObject)parameters[1]);
                            /*
                            Console.WriteLine("Inevtory: ");
                            for (int i = 0; i < inventory.Items.Count; i++)
                            {
                                Console.WriteLine("Item_" + i + " = " + inventory.Items[i].Id);
                            }
                            */
                        }
                    }
                }
            });

            EventSystem.Instance.AddListener("ShowInventory", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    ShowInventory((GameObject)parameters[0]);
                }
            });

            EventSystem.Instance.AddListener("ChangeToWeapon", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    ChangeWeaponTo((GameObject)parameters[0], (int)parameters[1]);
                }
            });
        }

        public void AddItemToInventory(GameObject player, GameObject item)
        {
            if(player.GetComponent<InventoryComponent>() != null && !player.GetComponent<InventoryComponent>().Items.Contains(item))
                player.GetComponent<InventoryComponent>().Items.Add(item);
        }

        public void RemoveItemFromInventory(GameObject player, GameObject item)
        {
            if (player.GetComponent<InventoryComponent>() != null && player.GetComponent<InventoryComponent>().Items.Contains(item))
                player.GetComponent<InventoryComponent>().Items.Remove(item);
        }

        public void ShowInventory(GameObject player)
        {
            Console.WriteLine("Inventory: ");
            int i = 0;
            foreach (var item in player.GetComponent<InventoryComponent>().Items)
            {
                Console.WriteLine("Slot " + (i+1) + " = " + item.Id);
                i++;
            }
        }

        private void ChangeWeaponTo(GameObject player, int slot)
        {
            if (player.GetComponent<InventoryComponent>().Items.Count > slot && player.GetComponent<InventoryComponent>().Items[slot] != null)
            {
                Console.WriteLine("Changed to weaponslot: " + slot + ". WeaponType = " + player.GetComponent<InventoryComponent>().Items[slot].GetComponent<PickUpComponent>().WeaponType);
                player.GetComponent<WeaponComponent>().WeaponType = player.GetComponent<InventoryComponent>().Items[slot].GetComponent<PickUpComponent>().WeaponType;
            }
        }
    }
}
