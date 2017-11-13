using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TerrainScript : MonoBehaviour {

    public GameObject player; //it is you :D
    int? currentX, currentY, prevX, prevY; //current = mis ollaan nyt 8x8 ruudukolla (world on 8x8), prev on se viimeinen jos ollaan oltu

    public GameObject[,] terrainList = new GameObject[8,8]; //world lista


    public void GetPlayer(GameObject _player)
    {
        player = _player;
    }

	void Start () // luo 8x8 matriisin worldistä joka on oikeassa järjestyksessä katsomalla worldin lapsien koordinaatteja ja vertailemalla niitä toisiinsa
    {
        //lista
        Transform[] childs = gameObject.GetComponentsInChildren<Transform>();
        List<Transform> list = childs.ToList<Transform>();
        list.RemoveAt(0);
        float px = Mathf.Infinity, py = Mathf.Infinity;
        for(int y = 0; y < 8; y++)
        {
            for(int x = 0; x < 8; x++)
            {
                int k = -1;
                int r = 65;
                GameObject go = null;
                px = Mathf.Infinity;
                foreach (Transform g in list)
                {
                    if(Mathf.Round(g.transform.localPosition.y) <= py)
                    {
                        if (py != Mathf.Round(g.transform.localPosition.y))
                        {
                            px = Mathf.Infinity;
                        }
                        py = Mathf.Round(g.transform.localPosition.y);
                        if(Mathf.Round(g.transform.localPosition.x) < px)
                        {
                            px = Mathf.Round(g.transform.localPosition.x);
                            go = g.gameObject;
                            r = k;
                        }
                    }
                    k++;
                }
                terrainList[x, y] = go;
                list.RemoveAt(r+1);
            }          
            py = Mathf.Infinity;
        }
        //lista loppuu
	}
	
	void Update () //does nothing
    {
		
	}

    void FixedUpdate() //does things
    {
        if (!currentX.HasValue)
        {
            FindPlayer();
        }
        if(!prevY.HasValue || !prevX.HasValue || currentX.Value != prevX.Value || currentY.Value != prevY.Value)
        {
            SetVisible();
        }
        PlayerPositionInWorld();
        
    }

    void PlayerPositionInWorld() //päivittää playerin sijaintia world ruudukossa
    {
        float px = Mathf.Infinity;
        float py = Mathf.Infinity;
        int vx = -1, vy = -1;
        for (int x = currentX.Value - 1; x < currentX.Value + 2; x++)
        {
            if (x >= 0 && x < 8 && Vector3.Distance(terrainList[x, currentY.Value].transform.position, player.transform.position) < px)
            {
                px = Vector3.Distance(terrainList[x, currentY.Value].transform.position, player.transform.position);
                vx = x;
            }

        }
        for (int y = currentY.Value - 1; y < currentY.Value + 2; y++)
        {
            if (y >= 0 && y < 8 && Vector3.Distance(terrainList[currentX.Value, y].transform.position, player.transform.position) < py)
            {
                py = Vector3.Distance(terrainList[currentX.Value, y].transform.position, player.transform.position);
                vy = y;
            }

        }
        prevX = currentX;
        prevY = currentY;
        currentX = vx;
        currentY = vy;
    }

    void FindPlayer() //löytää playerin pelin alussa
    {
        float px = Mathf.Infinity;
        float py = Mathf.Infinity;
        int vx = -1, vy = -1;
        for (int x = 0; x < 8; x++)
        {
            if(Vector3.Distance(terrainList[x,0].transform.position,player.transform.position) < px)
            {
                px = Vector3.Distance(terrainList[x, 0].transform.position, player.transform.position);
                vx = x;
            }

        }
        for (int y = 0; y < 8; y++)
        {
            if (Vector3.Distance(terrainList[0, y].transform.position, player.transform.position) < py)
            {
                py = Vector3.Distance(terrainList[0, y].transform.position, player.transform.position);
                vy = y;
            }

        }
        currentX = vx;
        currentY = vy;
        Debug.Log(currentX + " " + currentY);
    }

    void SetVisible() //asettaa palasien rendererei ja collidereita päälle ja pois
    {
        if (!prevX.HasValue) // tekee alkuun jos mitään ei oo vielä ladattuna
        {
            for (int x = currentX.Value - 1; x < currentX.Value + 2; x++)
            {
                for (int y = currentY.Value - 1; y < currentY.Value + 2; y++)
                {
                    if (y >= 0 && y < 8 && x >= 0 && x < 8)
                    {
                        terrainList[x, y].GetComponent<MeshCollider>().enabled = true;
                        terrainList[x, y].GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
        }
        else
        {
            if(prevX.Value-currentX.Value != 0) //jos x arvo muuttuu
            {
                for (int i = -1; i < 2; i++)
                {
                    int x1 = prevX.Value - currentX.Value + prevX.Value, x2 = -(prevX.Value - currentX.Value) + currentX.Value, y = prevY.Value + i;
                    if(y >= 0 && y < 8 &&  x1 >= 0 && x1 < 8)
                    {
                        terrainList[x1, y].GetComponent<MeshCollider>().enabled = false;
                        terrainList[x1, y].GetComponent<MeshRenderer>().enabled = false;
                    }
                    if (y >= 0 && y < 8 &&  x2 >= 0 && x2 < 8) // if chekkaa että reunal ollessa ei yritetä laittaa päälle arrayn ulkopuolel olevii (eli olemattomii)
                    {
                        terrainList[x2, y].GetComponent<MeshCollider>().enabled = true;
                        terrainList[x2, y].GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
            else //jos y arvo muuttuu
            {
                for (int i = -1; i < 2; i++)
                {
                    int y1 = prevY.Value - currentY.Value + prevY.Value, y2 = -(prevY.Value - currentY.Value) + currentY.Value, x = prevX.Value + i;
                    if(x >= 0 && x < 8 && y1 >= 0 && y1 < 8)
                    {
                        terrainList[x, y1].GetComponent<MeshCollider>().enabled = false;
                        terrainList[x, y1].GetComponent<MeshRenderer>().enabled = false;
                    }
                    if (x >= 0 && x < 8 && y2 >= 0 && y2 < 8)
                    {
                        terrainList[x, y2].GetComponent<MeshCollider>().enabled = true;
                        terrainList[x, y2].GetComponent<MeshRenderer>().enabled = true;
                    }
                    
                }
            }
        }
        
    }

}
