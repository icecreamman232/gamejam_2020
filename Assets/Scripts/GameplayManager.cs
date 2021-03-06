﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameplayManager : MonoBehaviour
{
    public RenderTexture main_tex;
    public MainCharacterMovementController m_char;
    public Tilemap m_tilemap;
    public Tilemap m_startPtsMap;
    public Tilemap m_sawMap;
    public List<Vector3> availablePlaces;
    public List<Vector3> availableDoors;
    public List<Vector3> availableSaw;
    public int max_num_x;
    public int max_num_y;


    public GameObject SawTrap;
    // Start is called before the first frame update
    void Awake()
    {
        availableDoors = new List<Vector3>();
        getCells(m_startPtsMap, availableDoors, false);
        availablePlaces = new List<Vector3>();
        getCells(m_tilemap, availablePlaces, true);
        availableSaw = new List<Vector3>();
        getCells(m_sawMap, availableSaw, false);

    }
    void Start()
    {
        //First Spawn Random
        int random_door_pos = UnityEngine.Random.Range(1, availableDoors.Count);
        m_char.transform.position = new Vector3(availableDoors[random_door_pos].x + m_startPtsMap.cellSize.x / 2, availableDoors[random_door_pos].y + m_startPtsMap.cellSize.y / 2, -1f);

        SetupSawTrap(SawTrap, availableSaw);

        max_num_x = Mathf.Abs(m_tilemap.cellBounds.xMin) + Mathf.Abs(m_tilemap.cellBounds.xMax)-1;
        max_num_y = Mathf.Abs(m_tilemap.cellBounds.yMin) + Mathf.Abs(m_tilemap.cellBounds.yMax)-1;
    }

    // Update is called once per frame
    void Update()
    {
        //Temporary code to clear the render texture
        if (Input.GetKey(KeyCode.C))
        {
            ClearActiveRenderTexture(main_tex);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ReSpawn();
        }
    }
    public void ClearActiveRenderTexture(RenderTexture tex)
    {
        /*RenderTexture rt = RenderTexture.active;
        RenderTexture.active = tex;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;*/
        /*Release (in this case, we'r trying to clear the current render texture.
         It's recommended to release the render texture whenever we finished using them.
        */
        tex.Release();
    }
    void ReSpawn()
    {
        m_char.gameObject.SetActive(true);
        m_char.transform.localScale = Vector3.one;
        m_char.isDeath = false;
        int random_door_pos = UnityEngine.Random.Range(1, availableDoors.Count);
        m_char.transform.position = new Vector3(availableDoors[random_door_pos].x + m_startPtsMap.cellSize.x / 2, availableDoors[random_door_pos].y + m_startPtsMap.cellSize.y / 2, -1f);
        m_char.circle.transform.localScale = Vector3.one;
    }
    private void getCells(Tilemap m_map,List<Vector3> list, bool isGetZeroCell)
    {
        for (int n = m_map.cellBounds.xMin; n < m_map.cellBounds.xMax; n++)
        {
            for (int p = m_map.cellBounds.yMin; p < m_map.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)m_map.transform.position.y));
                Vector3 place = m_map.CellToWorld(localPlace);
                if (m_map.HasTile(localPlace))
                {
                    //Tile at "place"
                    list.Add(place);
                }
                else
                {
                    //No tile at "place"
                    if(isGetZeroCell)
                    {
                        list.Add(Vector3.zero);
                    }                  
                }
            }
        }
    }

    private void SetupSawTrap(GameObject SawTrap,List<Vector3> pos_list)
    {
        for(int i =0;i<pos_list.Count;i++)
        {
            Vector3 tmp_pos = new Vector3(pos_list[i].x+m_sawMap.cellSize.x/2, pos_list[i].y + m_sawMap.cellSize.y / 2, pos_list[i].z);
            Quaternion rot = new Quaternion(0, 0, 0, 0);
            Instantiate(SawTrap, tmp_pos, rot);
        }
        
    }
    private void UpdateSaw()
    {

    }
}
