using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testListEquipment : MonoBehaviour
{
  
    // Start is called before the first frame update
    void Start()
    {
        List<EquipmentDataBaseDTO> e = EquipmentDataController.Instance.InventoryDataManager.equipmentDatasDTO;
        Debug.Log(e.Count);
        foreach (EquipmentDataBaseDTO data in e)
        {
            Debug.Log(data.Id);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
