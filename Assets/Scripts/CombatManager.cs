using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShrinelandsTactics;
using UnityEngine.Tilemaps;
using YamlDotNet.RepresentationModel;
using System.IO;
using System.Text;
using Assets.Scripts.FSM;
using Assets.Scripts.Events;
using System;
using Assets.Scripts.UI;
using ShrinelandsTactics.World;

namespace Assets.Scripts
{
    public class CombatManager : MonoBehaviour
    {
        public DungeonMaster DM;
        public Tilemap tileMap;
        public Tilemap overlayMap;
        public NameplateUI Nameplate;

        public UnityEngine.Tilemaps.Tile emptyTile;
        public UnityEngine.Tilemaps.Tile wallTile;


        public GameObject characterPrefab;

        public TextAsset characterJson;
        public TextAsset tileJson;
        public TextAsset actionJson;
        public TextAsset levelYaml;
        public Texture2D levelLayout;

        public float CamVelocity;
        public float CamMaxZoom;
        public float CamZoomSpeed;

        private Camera camera;
        public Animator anim { get; private set; }
        public Character SelectedCharacter { get; internal set; }

        public event CharacterClickedEventHandler CharacterClicked;
        public event EventHandler<Vector3> TileClicked;
        public event EventHandler Deselect;

        void Start()
        {
            camera = Camera.main;
            anim = GetComponent<Animator>();

            var data = GameData.CreateFromJson(tileJson.text, characterJson.text, actionJson.text);

            var yaml = new YamlStream();
            using (StringReader r = new StringReader(levelYaml.text))
            {
                yaml.Load(r);
            }

            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            var mapFile = (mapping[new YamlScalarNode("map_file")] as YamlScalarNode).Value;


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Test Map");
            sb.AppendLine(levelLayout.width + " " + levelLayout.height);

            for (int y = 0; y < levelLayout.height; y++)
            {
                for (int x = 0; x < levelLayout.width; x++)
                {
                    var color = levelLayout.GetPixel(x, y);
                    var r = (byte)(color.r * 255);
                    var g = (byte)(color.g * 255);
                    var b = (byte)(color.b * 255);
                    sb.Append(data.GetIconByColor(r, g, b).ToString());
                }
                sb.AppendLine();
            }

            DM = DungeonMaster.LoadEncounter(mapping, sb.ToString(), data);
            SetupTiles();
            SetupUnits();
        }

        public void SetupTiles()
        {
            overlayMap.ClearAllTiles();
            tileMap.ClearAllTiles();
            for (int y = 0; y < DM.map.Height; y++)
            {
                for (int x = 0; x < DM.map.Width; x++)
                {
                    var tile = DM.map.GetTile(x, y);
                    if (tile.Passable)
                    {
                        tileMap.SetTile(new Vector3Int(x, y, 0), emptyTile);
                    }
                    else
                    {
                        tileMap.SetTile(new Vector3Int(x, y, 0), wallTile);
                    }
                }
            }
        }

        public void SetupUnits()
        {
            foreach (var guy in DM.Characters)
            {
                var pos = new Vector3(guy.Pos.x, DM.map.Height - guy.Pos.y, 0);
                var go = Instantiate(characterPrefab, pos, Quaternion.identity, this.transform);
                var controller = go.GetComponent<CharacterRenderer>();
                controller.RepresentCharacter(guy);
            }
        }

        // Update is called once per frame
        void Update()
        {

            //handle input
            //move camera
            //TODO: hold middle mouse button to pan around
            var cameraX = camera.transform.position.x + CamVelocity * Input.GetAxis("Horizontal") * Time.deltaTime;
            var cameraY = camera.transform.position.y + CamVelocity * Input.GetAxis("Vertical") * Time.deltaTime;
            camera.transform.position = new Vector3(cameraX, cameraY, camera.transform.position.z);
            camera.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * CamZoomSpeed * Time.deltaTime;
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, 2, CamMaxZoom);

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(Deselect != null)
                {
                    Deselect(this, null);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                //find out what we clicked on
                //is it unit?
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null)
                {
                    Debug.LogError(hit.collider.name);
                    if(hit.collider.tag == "Character")
                    {
                        //TODO: maybe should use position?
                        var guy = hit.collider.GetComponent<CharacterRenderer>().CharacterRepresented; 
                        if(CharacterClicked != null)
                        {
                            CharacterClicked(this, new CharacterClickedEventArgs(guy));
                        }
                            
                    }
                    else
                    {
                        if(hit.collider.tag == "Tilemap")
                        {
                            if(TileClicked != null)
                            {
                                Debug.Log("Clicked on tile at " + mousePos);
                                TileClicked(this, mousePos);
                            }
                        }
                    }
                }

            }
        }


    public delegate void CharacterClickedEventHandler(object sender, CharacterClickedEventArgs a);
    }

}
