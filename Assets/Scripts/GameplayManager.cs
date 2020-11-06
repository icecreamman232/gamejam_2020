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
    public List<Vector3> availablePlaces;
    public int max_num_x;
    public int max_num_y;
    // Start is called before the first frame update
    void Start()
    {
        availablePlaces = new List<Vector3>();

        for (int n = m_tilemap.cellBounds.xMin; n < m_tilemap.cellBounds.xMax; n++)
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
                }
            }
        }
        Debug.Log("min X =" + m_tilemap.cellBounds.xMin);
        Debug.Log("min Y =" + m_tilemap.cellBounds.yMin);
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
        try
        {
            m_char.transform.position = new Vector3(availablePlaces[random_cell].x + m_tilemap.cellSize.x / 2, availablePlaces[random_cell].y + m_tilemap.cellSize.y / 2, -1f);
        }
        catch(Exception e)
        {
            Debug.Log("Random cell="+random_cell);
        }
        
    }
}
