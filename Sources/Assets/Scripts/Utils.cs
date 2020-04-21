using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
	public static int getRotationAngleForPlayerId(string id)
	{
		int retVal = 0;
		switch(id)
		{
			case "P1":
				retVal = 180;
				break;

			case "P3":
				retVal = 90;
				break;

			case "P4":
				retVal = -90;
				break;

			default:
				break;
		}

		return retVal;
	}

	public static GameObject getSpriteForActionButtonName(string buttonName,GameObject[] prefab)
	{
		switch(buttonName.Substring(buttonName.Length - 1))
		{
			case "G":
			return prefab[0];
			case "R":
			return prefab[3];
			case "B":
			return prefab[1];
			case "Y":
			return prefab[2];
			default:
				break;
		}
		return null;
	}

	public static GameObject getSpriteForPlayerId(string id,GameObject[] prefab)
	{
		switch(id)
		{
		case "P1":
			return prefab[0];
		case "P2":
			return prefab[2];
		case "P3":
			return prefab[3];
		case "P4":
			return prefab[1];
		}

		return null;
	}
}