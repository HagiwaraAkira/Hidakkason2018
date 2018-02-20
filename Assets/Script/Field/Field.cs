using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
	public Cell Cell1;
	public Cell Cell2;
	public Cell Cell3;
	public Cell Cell4;
	public Cell Cell5;
	public Cell Cell6;
	public Cell Cell7;
	public Cell Cell8;
	public Cell Cell9;
	public List<Cell> CellList;
	
	// Use this for initialization
	void Start () {
		CellList = new List<Cell>()
		{
			Cell1,
			Cell2,	
			Cell3,
			Cell4,	
			Cell5,
			Cell6,	
			Cell7,
			Cell8,	
						Cell9,	
		};
		
	}

	public void SetCard(string cellName ,Card card)
	{

		var cell = GetCell(cellName);
		if (cell.Card != null)
		{
			return;
		}
		card.transform.SetParent(cell.transform);
		card.transform.localPosition= Vector3.zero;
		cell.Card = card;
		cell.ChangeImage(InGameController.Instance.CurrentState);
		card.Used = true;


	}

	public Cell GetCell(string cellName)
	{
		if (cellName == "cell1") return Cell1;
		if (cellName == "cell2") return Cell2;
		if (cellName == "cell3") return Cell3;
		if (cellName == "cell4") return Cell4;
		if (cellName == "cell5") return Cell5;
		if (cellName == "cell6") return Cell6;
		if (cellName == "cell7") return Cell7;
		if (cellName == "cell8") return Cell8;
		if (cellName == "cell9") return Cell9;
		return Cell1;
	}

}
