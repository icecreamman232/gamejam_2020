using System;
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
    public List<Vector3> availablePlaces;
    public List<Vector3> availableDoors;
    public int max_num_x;
    public int max_num_y;
    // Start is called before the first frame update
    void Awake()
    {
        availableDoors = new List<Vector3>();
        getCells(m_startPtsMap, availableDoors, false);
        availablePlaces = new List<Vector3>();
        getCells(m_tilemap, availablePlaces, true);
    }
    void Start()
    {
        //First Spawn Random
        int random_door_pos = UnityEngine.Random.Range(1, availableDoors.Count);
        m_char.transform.position = new Vector3(availableDoors[random_door_pos].x + m_startPtsMap.cellSize.x / 2, availableDoors[random_door_pos].y + m_startPtsMap.cellSize.y / 2, -1f);


        /*availableDoors = new List<Vector3>();

        getCells(m_startPtsMap, availableDoors, false);*/

        //availablePlaces = new List<Vector3>();

        //getCells(m_tilemap, availablePlaces, true);

        /*for (int n = m_tilemap.cellBounds.xMin; n < m_tilemap.cellBounds.xMax; n++)
        {
            for (int p = m_tilemap.cellBounds.yMin; p < m_tilemap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)m_tilemap.transform.position.y));
                Vector3 place = m_tilemap.CellToWorld(localPlace);
                if (m_tilemap.HasTile(localPlace))
                {
                    //Tile at "place"
                    availablePlaces.Add(place);
                }
                else
                {
                    //No tile at "place"
                    availablePlaces.Add(Vector3.zero);
                }
            }
        }*/
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
        int total_cell = max_num_x * max_num_y;
        int random_cell = UnityEngine.Random.Range(1, total_cell);
        while(availablePlaces[random_cell].x ==0)
        {
            m_char.transform.position = new Vector3(availablePlaces[random_cell].x + m_tilemap.cellSize.x / 2, availablePlaces[random_cell].y + m_tilemap.cellSize.y / 2, -1f);
            break;
        } 
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
}
