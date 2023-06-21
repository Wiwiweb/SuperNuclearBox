using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TestScript : MonoBehaviour
{
  public Tilemap floorTilemap;
  public Tilemap wallTilemap;
  public Tile wallTile;
  public Tile floorTile;

  // Start is called before the first frame update
  void Start()
  {
    GameObject grenade = GameObject.FindWithTag("Test");
    grenade.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10 * 35);
    // floorTilemap.SetTile(new Vector3Int(0,0), floorTile);
    // wallTilemap.SetTile(new Vector3Int(0,0), wallTile);
  }

  // Update is called once per frame
  void Update()
  {
    if (Keyboard.current.rKey.wasPressedThisFrame)
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
  }
}
